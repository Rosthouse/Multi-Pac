using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using Lidgren.Network;
using System.Collections;
using PacManNetworkShared;
using PacManServer.Initialization;
using PacManServer.Validation;
using PacManShared;
using Microsoft.Xna.Framework;
using PacManShared.Controllers;
using PacManShared.Controllers.Network;
using PacManShared.Entities.Player;
using PacManShared.Enums;
using PacManShared.GameplayBehaviour;
using System.Collections.Generic;
using PacManShared.Simulation;
using PacManShared.Util.TimeStamps;

namespace PacManServer
{
    public partial class FrmServer:Form
    {
        //Connection configurations
        NetPeerConfiguration serverConfig;
        NetServer server;

        //Thread demoThread;
        private List<Client> connectedClients = new List<Client>();
        private List<NetConnection> pendingClients = new List<NetConnection>();

        //Game fields
        private bool gameRunning = false;
        private ServerGameTime gameTime;

        //Managers
        private ServerNetworkManager networkManager;
        private GameStateManager gameStateManager;

        //Gameplay simulation
        private GameLoop gameLoop;

        //Command queue
        private SortedSet<Command> inputQueue;

        public void InitializeServer()
        {
            gameStateManager = new GameStateManager();

            //string ip = "147.87.243.27";
            serverConfig = new NetPeerConfiguration("pacman");
            //serverConfig.LocalAddress = IPAddress.Parse(ip);
            serverConfig.Port = 666;
            
            try
            {
                server = new NetServer(serverConfig);
                server.Start();
                networkManager = new ServerNetworkManager(server);
                WriteToLog("Server wurde gestartet...");
            }
            catch (Exception)
            {
                WriteToLog("Fehler! Server nicht gestartet...");
            }

            //networkManager = new NetworkManager(server);
            gameTime = new ServerGameTime();

            inputQueue = new SortedSet<Command>(new MovementComparer());
           
        }
    
        protected override void OnClosing(CancelEventArgs e)
        {
            server.Shutdown("Anwendung wurde geschlossen");
            base.OnClosing(e);
        }

        public FrmServer()
        {

            InitializeComponent();
            InitializeServer();
        }

        private void msgTicker_Tick(object sender, EventArgs e)
        {
            //Console.WriteLine(gameTime.TotalGameTime.TotalSeconds+" server");
            if (server != null)
            {
                NetIncomingMessage msg;
                NetOutgoingMessage response;
                NetConnection msgCon;
                string msgString;
                int msgInt;

                // check for new messages (all messages in queu per update cycle)
                while ((msg = networkManager.getMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        // Client sucht nach einem Server
                        case NetIncomingMessageType.DiscoveryRequest:
                            this.WriteToLog("Discoveryanfrage von: " + msg.SenderEndpoint.Address.ToString());
                            // Create a response
                            response = networkManager.createMessage();

                            // Send the response to the sender of the request
                            try
                            {
                                server.SendDiscoveryResponse(response, msg.SenderEndpoint);
                                this.WriteToLog("Antwort an " + msg.SenderEndpoint + " geschickt");
                            }
                            catch (Exception)
                            {
                                this.WriteToLog("Antwort an " + msg.SenderEndpoint + " konnte nicht gesendet werden");
                            }
                            break;

                        // Es handelt sich um ein Datenpaket
                        case NetIncomingMessageType.Data:
                            msgString = msg.ReadString();

                            switch (msgString)
                            {
                                // Client startete Spiel und wartet nun auf Initialisierung vom Server
                                case "startGame":
                                    string fileName = @"\Levels\level1.csv";
                                    gameStateManager.Level = new LevelProcessor().LoadMap(fileName);
                                    this.WriteToLog("Level " + fileName + " loaded");

                                    msgInt = msg.ReadInt32();
                                    foreach (Client c in connectedClients)
                                    {
                                        if (c.getConnection() == msg.SenderConnection)
                                        {
                                            c.setFigureType(msgInt);

                                            switch (c.getFigureType())
                                            {
                                                case 1:
                                                    NetworkController networkController =
                                                        new NetworkController(c.getUID(), EmptyController.Empty(c.getUID()),
                                                                              networkManager);

                                                    PacMan pacMan = new PacMan(string.Empty,
                                                                               gameStateManager.Level.getCell(1, 1),
                                                                               gameStateManager.Level, EmptyController.Empty(c.getUID()),
                                                                               Direction.None, 0.9f, new Point(49, 49));
                                                    
                                                    gameStateManager.AddPlayers(pacMan);
                                                    break;
                                                    
                                                default:
                                                    WriteToLog("Ghost isn't implemented yet");
                                                    break;
                                            }

                                            // Remove Client from pending list
                                            pendingClients.Remove(c.getConnection());
                                            //gameStateManager.AddPlayers(new PacMan());
                                            this.WriteToLog("Set figureType " + msgInt + " to Client " + c.getConnection().RemoteEndpoint.Address);
                                        }
                                    }

                                    gameLoop = new GameLoop(5000);

                                    gameLoop.StartingSimulation += StartSimulation;
                                    gameLoop.FinishedSimulation += FinishedSimulation;

                                    //gameTime.Start();

                                    break;

                                // Got direction update from client deploy it to the rest
                                case "newDirection":
                                    msgInt = msg.ReadInt32();
                                    long msgLong = msg.ReadInt64();
                                    msgCon = msg.SenderConnection;

                                    // calculate the UID from the sender of the message
                                    string msgUID = msgCon.RemoteUniqueIdentifier.ToString().Substring(msgCon.RemoteUniqueIdentifier.ToString().Length - 5);

                                    MovableObject toChange = gameStateManager.getFromId(Convert.ToInt32(msgUID));

                                    WriteToLogDebug("UID " + toChange.ID + "s current direction: " + (Direction)msgInt);

                                    inputQueue.Add(new Command((Direction)msgInt, Convert.ToInt32(msgUID),
                                                               gameTime.TotalGameTime.Milliseconds));


                                    //toChange.Direction = (Direction)msgInt;

                                    // send direction updates to all clients except the sender
                                    foreach (Client c in connectedClients)
                                    {
                                        if (c.getConnection() != msgCon)
                                        {
                                            response = networkManager.createMessage();
                                            response.Write("updateDirection;" + msgUID + ";" + msgInt);
                                            networkManager.sendMessage(response, c.getConnection());
                                            this.WriteToLog("Sent direction update for uid " + msgUID + " to " + c.getConnection().RemoteEndpoint.Address + "Time: " + gameTime.TotalGameTime.TotalMilliseconds);
                                        }
                                    }

                                    MovableObject changed = gameStateManager.getFromId(Convert.ToInt32(msgUID));

                                    //WriteToLogDebug("UID " + changed.ID + "s current direction: " + changed.Direction);

                                    break;

                                    // client is asking for the list of connected clients
                                case "getPlayers":
                                    this.WriteToLog("Client asked for Players");
                                    string players = "";
                                    // create a string with all connected clients (example 15482/1;56847/2)
                                    foreach (Client c in connectedClients)
                                    {
                                        if (players == "")
                                        {
                                            players = c.getUID() + "/" + c.getFigureType() + ";";
                                        }
                                        else
                                        {
                                            players = players + c.getUID() + "/" + c.getFigureType()+";";
                                        }
                                    }
                                    response = networkManager.createMessage();
                                    // add "getPlayser" to the string for message identification on the client and send the message
                                    response.Write("getPlayers;"+players);
                                    networkManager.sendMessage(response, msg.SenderConnection);
                                    this.WriteToLog("List of players " + players);
                                    break;

                                case "latencyTime":
                                    //this.WriteToLogDebug("Got timesync request");
                                    response = networkManager.createMessage();
                                    response.Write(msgString);
                                    networkManager.sendMessage(response, msg.SenderConnection);
                                    break;

                                case "syncTime":
                                    response = networkManager.createMessage();
                                    response.Write(msgString);
                                    // serialize the current servertime and send it to the requesting client
                                    response.Write(networkManager.Serialize(gameTime.TotalGameTime));
                                    this.WriteToLogDebug("Sent server time: " + gameTime.TotalGameTime.TotalSeconds);
                                    networkManager.sendMessage(response, msg.SenderConnection);
                                    break;
                            }

                            break;
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.StatusChanged:
                            msgString = msg.ReadString();
                            msgCon = msg.SenderConnection;
                            // Weil bei der connect-Nachricht vom Client spezielle escape-Zeichen
                            // mitgesendet werden, muss der String angepasst werden, damit er
                            // verglichen werden kann
                            string corrString = msgString.Substring(msgString.Length - 2);
                            if (corrString.CompareTo("Co") == 0)
                            {
                                try
                                {
                                    response = networkManager.createMessage();
                                    response.Write("connected");
                                    networkManager.sendMessage(response, msgCon);
                                    // create new client
                                    Client c = new Client(msgCon);
                                    // add new client to the list of connected clients
                                    connectedClients.Add(c);
                                    // add new client to the list of pending clients (clients in this list are still configuring their game options)
                                    pendingClients.Add(msgCon);

                                    this.WriteToLog("Sent connection approval to" + msg.SenderEndpoint + " with UID: "+c.getUID());
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine(ex.ToString());
                                }
                            }
                            break;
                        case NetIncomingMessageType.ErrorMessage:
                            this.WriteToLog("Error: " + msg.ReadString());
                            break;
                        default:
                            this.WriteToLog("Unhandled type: " + msg.MessageType + " -> " + msg.ReadString());
                            break;
                    }
                    server.Recycle(msg);
                }

                // All Players are ready, send start message to all connected clients
                if (pendingClients.Count == 0 && gameRunning == false && connectedClients.Count > 0)
                {
                    response = networkManager.createMessage();
                    response.Write("start");

                    foreach (Client c in connectedClients)
                    {
                        networkManager.sendMessage(response, c.getConnection());
                    }
                    gameRunning = true;
                }
            }


            gameTime.Reset();
        }

        /// <summary>
        /// Simulates the game
        /// </summary>
        /// <param name="sender">the sender (gameTicker from form1)</param>
        /// <param name="e">event arguments</param>
        private void gameTicker_Tick(object sender, EventArgs e)
        {
            if(this.gameRunning)
            {
                gameLoop.SimulationLoop(gameTime, gameStateManager);
                gameTime.Reset();
            }
        }

        public void WriteToLogDebug(string message)
        {

#if DEBUG
            WriteToLog(message);
#endif

        }

        public void WriteToLog(string message)
        {
            txtLog.AppendText(message + "\r\n");
            txtLog.ScrollToCaret();
        }

        public void StartSimulation (object sender, EventArgs e)
        {
            gameLoop.InputQueue.Clear();
            gameLoop.InputQueue = new Queue<Command>(this.inputQueue);
            WriteToLogDebug("Simulating game with " + inputQueue.Count + " commands");

            inputQueue.Clear();

            
        }

        public void FinishedSimulation(object sender, EventArgs e)
        {
            TimeStamp toSend = gameStateManager.TimeStampManager.Pop();

            foreach(Client c in connectedClients)
            {
                //TODO: Implement sending of timestamp
                networkManager.sendMessage(toSend, c.getConnection());
            }
        }
    }
}