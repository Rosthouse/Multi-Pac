using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using PacManClient.Components.GameScreens.GUIElements;
using PacManClient.Components.Workers;
using PacManShared;
using PacManNetworkShared;

namespace PacManClient.Components.GameScreens
{
    /// <summary>
    /// Allows to configure a multiplayer session before starting a multiplayer game
    /// </summary>
    class ConfigMultiplayerScreen : MenuScreen
    {
        private static readonly string[,] figures = { {"PacMan","1"} , {"Ghost","2"} };

        private NetClient client;

        private bool waitServerConnect = false;
        private bool waitTimeSync = false;

        private TimeSpan sendTime;
        TimeSpan serverTime = new TimeSpan();
        TimeSpan latency = new TimeSpan(0);
        private long ticks;
        private int count;

        Stopwatch stopwatch = new Stopwatch();
        private string connectedServer = "Not Connected";
        private static int currentFigure;
        private INetworkManager networkManager;

        private readonly MenuEntry figureMenuEntry;
        private readonly MenuEntry serverMenuEntry;
        IGameTime gameTime;
        private TimeSpan timeOffset = new TimeSpan();

        /// <summary>
        /// Sets the networkclient
        /// </summary>
        /// <param name="client"></param>
        /*public void setClient(NetClient client)
        {
            this.client = client;
        }*/

        /// <summary>
        /// Sets the server ip
        /// </summary>
        /// <param name="server">The ip of the server as a string</param>
        public void setConntectedServer(string server)
        {
            this.connectedServer = server;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigMultiplayerScreen()
            :base("Multiplayer options")
        {
            // Create Menu Entries
            figureMenuEntry = new MenuEntry(string.Empty);
            serverMenuEntry = new MenuEntry("Connect To Server");
            var start = new MenuEntry("Start game");
            var back = new MenuEntry("Back");

            // Hook up menue event handlers
            figureMenuEntry.Selected += figureMenuSelected;
            serverMenuEntry.Selected += serverMenuSelected;
            start.Selected += StartGame;
            back.Selected += OnCancel;

            // Add Entries to Menu
            MenuEntries.Add(serverMenuEntry);
            MenuEntries.Add(figureMenuEntry);
            MenuEntries.Add(start);
            MenuEntries.Add(back);
            //Initialize();

            //initialize networkManager
            //configure client
            client = new NetClient(new NetPeerConfiguration("pacman"));
            //Start client
            client.Start();
            networkManager = new ClientNetworkManager(client);
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StartGame(object sender, PlayerIndexEventArgs e)
        {
            if(networkManager != null)
            {
                // send the server the startGame message to tell him this client is ready
                NetOutgoingMessage msg = networkManager.createMessage();
                msg.Write("startGame");
                msg.Write(Convert.ToInt32(figures[currentFigure, 1]));
                if (networkManager.sendMessage(msg) == NetSendResult.Sent)
                {
                    LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new MultiplayerLoader(this.networkManager, timeOffset),
                                       new MultiplayerScreen(this.networkManager));
                } 
            } else
            {
                ScreenManager.AddScreen(new MessageBoxScreen("No connection to a server has been found"), e.PlayerIndex);
            }
            
        }

        /// <summary>
        /// If the user cancels, the main menuscreen will be brought up again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public  void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new MainMenuScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// Initializes this screen
        /// </summary>
        public override void Initialize()
        {
            SetMenuEntryText();
        }

        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        private void SetMenuEntryText()
        {
            figureMenuEntry.Text = "Preferred figure: " + figures[currentFigure,0];
        }

        /// <summary>
        /// Gets fired when the user wants to connect to a server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serverMenuSelected(object sender, PlayerIndexEventArgs e)
        {
            if (client != null)
            {
                if (client.ServerConnection == null)
                {
                    waitServerConnect = true;

                    // Contact Server
                    //client.DiscoverKnownPeer("147.87.243.50", 666);
                    client.DiscoverLocalPeers(666);

                    //Stopwatch to see if the server times out.
                    stopwatch.Start();
                }
                else
                {
                    // Connection to server already exists, just request a time synchronisation
                    waitTimeSync = true;
                    networkManager.sendMessage("syncTime");
                    sendTime = gameTime.TotalGameTime;
                    stopwatch.Start(); // start the stopwatch for timeout check
                    count = 0;
                }

                
                
            }
        }

        /// <summary>
        /// Gets fires when the server wants to select a different MovObjType
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void figureMenuSelected(object sender, PlayerIndexEventArgs e)
        {
            currentFigure = (currentFigure + 1) % (figures.Length/2);
            SetMenuEntryText();
        }

        /// <summary>
        /// Updates this screen
        /// </summary>
        /// <param name="gameTime">the current gametime</param>
        /// <param name="otherScreenHasFocus">if another screen has the focus</param>
        /// <param name="coveredByOtherScreen">if this screen is covered by another screen</param>
        public override void Update(IGameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this.gameTime = gameTime;

            // Waiting for the server response to the discovery request
            if (waitServerConnect)
            {
                NetIncomingMessage msg;
                msg = networkManager.getMessage();

                if (msg != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryResponse:
                            try
                            {
                                client.Connect(msg.SenderEndpoint);
                            }
                            catch (Exception ex)
                            {
                                System.Console.WriteLine(ex.ToString());
                            }
                            break;
                        case NetIncomingMessageType.Data:
                            if (msg.ReadString() == "connected") // client successfully connected
                            {
                                waitServerConnect = false;
                                if (client.ServerConnection != null)
                                {
                                    networkManager.sendMessage("syncTime"); // send a request for time synchronisation
                                    waitTimeSync = true;
                                    sendTime = gameTime.TotalGameTime;
                                    count = 0;
                                }
                            }
                            break;
                    }
                }

                if (stopwatch.ElapsedMilliseconds >= 10000)
                {
                    waitServerConnect = false;
                    stopwatch.Stop();
                    ScreenManager.AddScreen(new MessageBoxScreen("Could not find a server"), null);
                }
            }

            if (waitTimeSync)
            {
                NetIncomingMessage msg;
                msg = networkManager.getMessage();

                if (msg != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            string msgString = msg.ReadString();
                            switch (msgString)
                            {
                                case "syncTime":
                                    serverTime = (TimeSpan)networkManager.Deserialize(msg, msgString); // deserialize the TimeSpan from the server
                                    sendTime = gameTime.TotalGameTime;
                                    networkManager.sendMessage("latencyTime");
                                    break;

                                case "latencyTime":
                                    ticks += gameTime.TotalGameTime.Subtract(sendTime).Ticks / 2;
                                    sendTime = gameTime.TotalGameTime;
                                    count++;
                                    if (count < 5)
                                    {
                                        networkManager.sendMessage("latencyTime");
                                    }
                                    else
                                    {
                                        // calcluate the offset
                                        networkManager.Offset = serverTime.Subtract(gameTime.TotalGameTime.Add(new TimeSpan(ticks / 5)));
                                        // update the menu entry
                                        serverMenuEntry.Text = "Server " + client.ServerConnection.RemoteEndpoint.Address + " (offset " + Math.Round(networkManager.Offset.TotalSeconds, 2) + " sec)";
                                        stopwatch.Stop();
                                        stopwatch.Reset();
                                        ticks = 0;
                                        waitTimeSync = false;
                                    }
                                    break;
                            }
                            break;
                    }
                }

                if (stopwatch.ElapsedMilliseconds >= 10000)
                {
                    waitTimeSync = false;
                    stopwatch.Stop();
                    ScreenManager.AddScreen(new MessageBoxScreen("Could not syncronize time with server"), null);
                }
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
