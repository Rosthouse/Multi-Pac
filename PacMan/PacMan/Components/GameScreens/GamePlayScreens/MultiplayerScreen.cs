using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using PacManShared;
using PacManShared.Controllers;
using PacManShared.Controllers.AI;
using PacManShared.Controllers.AI.IndividualAI;
using PacManShared.Controllers.Network;
using PacManShared.Entities.Player;
using PacManClient.Controller.Local;
using PacManShared.Enums;
using PacManShared.Initialization;
using PacManShared.LevelClasses;
using PacManShared.GameplayBehaviour;
using System.Diagnostics;
using PacManNetworkShared;
using PacManShared.Simulation;
using PacManShared.Util.TimeStamps;
using PacManClient.Components.GameScreens.GamePlayScreens.GUI;

namespace PacManClient.Components.GameScreens
{
    public class MultiplayerScreen: GameplayScreen
    {
        private TimeSpan timeOffset;
        private int timestampcounter = 50;
        private readonly int timestamptimer = 50;
        private INetworkManager networkManager;

        private TimeSpan sendTime;
        TimeSpan serverTime = new TimeSpan();
        TimeSpan latency = new TimeSpan(0);
        private long ticks;
        private int count = 0;

        //public NetClient client { get; set; }
        
        
        public TimeSpan TimeOffset
        {
            get { return this.timeOffset; }
            set { this.timeOffset = value; }
        }

        public MultiplayerScreen(INetworkManager networkManager)
        {
            this.networkManager = networkManager;
        }

        /// <summary>
        /// Initializes the current screen
        /// </summary>
        public override void Initialize()
        {
            //Store everything inside the gameStateManager
            gameStateManager = new GameStateManager();
            gameStateManager.GameState = GameState.Loading;
            gameStateManager.Level = level;

            //Initialize the gameplay behaviours
            playBehaviour = new PlayBehaviour();
            deathBehaviour = new DeathBehaviour();

            var parser = new LevelParser();
            
            gameStateManager.Level = parser.GenerateLevel(ScreenManager.Content.Load<List<String[]>>(@"Levels\level1"));
            guiElements = new List<GamePlayScreens.GUI.GUIElement>();


            //camera = new Camera(1, Vector2.Zero, 0);
            camera = new Camera(0.4f, Vector2.Zero, 0, true, null);

            // ask server for List of connected and waiting clients
            networkManager.sendMessage("getPlayers");

            // wait for the response to the request of available players
            NetIncomingMessage msg;
            bool waitingForServer = true;
            while (waitingForServer)
            {
                // check for messages as long the incoming messages queue has messages stored
                while ((msg = networkManager.getMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            string msgStr = msg.ReadString();
                            
                            // check if the message is the response to the request
                            string[] clients = msgStr.Split(';');
                            if (clients[0] == "getPlayers")
                            {
                                int i = 1;
                                foreach (string player in clients)
                                {
                                    if (player != "" && player != "getPlayers")
                                    {
                                        string[] values = player.Split('/');
                                        int localUID = networkManager.getUID();
                                        int remoteUID = Convert.ToInt32(values[0]);

                                        switch (values[1])
                                        {
                                            case "1": // Player is a PacMan
                                                // Check if it is a local or remote controlled gameobject
                                                if (localUID == remoteUID) // local controlled
                                                {
                                                    PlayerController playerController = new PlayerController(
                                                        Direction.None, values[0], remoteUID, PlayerIndex.One,
                                                        MovObjType.LocalPacMan, ScreenManager.Input);

                                                    NetworkController networkController =
                                                        new NetworkController(playerController.ID, playerController,
                                                                              networkManager);

                                                    gameStateManager.AddPlayers(new PacMan(@"Sprites\PacMan",
                                                        gameStateManager.Level.getCell(1, 1), gameStateManager.Level, networkController, Direction.None, 1.3f, new Point(50, 50)));
                                                }
                                                else // remote controlled
                                                {
                                                    EmptyController emptyController = new EmptyController(Direction.None,
                                                                                                          values[0],
                                                                                                          remoteUID,
                                                                                                          MovObjType.
                                                                                                              NetPacMan);

                                                    gameStateManager.AddPlayers(new PacMan(@"Sprites\PacMan",
                                                        gameStateManager.Level.getCell(1, 1), gameStateManager.Level,
                                                        new NetworkController(emptyController.ID, emptyController, networkManager),
                                                        Direction.None, 1.3f, new Point(50, 50)));
                                                }

                                                GUIString stringElement = new GUIString(gameStateManager.MovableObjects.Last(), Color.White, new Vector2(0, 0), new Vector2(10, i * 10));
                                                guiElements.Add(stringElement);
                                             
                                                break;

                                            case "2": // Player is a Ghost

                                                Blinky blinky = new Blinky(gameStateManager.GetAllOfType<PacMan>()[0],
                                                                           new Point(0, 0), new Point(15, 15),
                                                                           new Point(17, 17));

                                                GhostController ghostController = new GhostController(level,
                                                                                                      blinky,
                                                                                                      remoteUID);

                                                NetworkGhostController ghostNetworkController =
                                                    new NetworkGhostController(networkManager, ghostController,
                                                                               ghostController.ID);
                                                gameStateManager.AddPlayers(new Ghost(@"Sprites\GhostBase", gameStateManager.Level.getCell(26, 1), gameStateManager.Level,
                                                    Direction.Down, 0.8f, new Point(50, 50), Color.Crimson, ghostNetworkController));

                                                //                                            else
                                                //                                            {
                                                //                                               
                                                //
                                                //                                                gameStateManager.AddPlayers(new Ghost(@"Sprites\GhostBase", gameStateManager.Level.getCell(26, 1), gameStateManager.Level,
                                                //                                                    Direction.Down, 0.8f, new Point(50, 50), Color.Crimson, new NetworkController(Direction.None, values[0], Convert.ToInt32(values[0]),
                                                //                                                        PlayerIndex.One, MovObjType.NetGhost, ScreenManager.Input, networkManager)));
                                                //                                            }
                                                break;
                                        }
                                        i += 10;
                                    }
                                }
                                // All mvable objects are initialized, start game
                                waitingForServer = false;
                            }

                            break;
                    }
                }
            }

            gameStateManager.CreateTimestamp(0);
        }

        /// <summary>
        /// Updates this screene
        /// </summary>
        /// <param name="gameTime">the current gametime</param>
        /// <param name="otherScreenHasFocus">If the gui has the focus</param>
        /// <param name="coveredByOtherScreen">If this screen is covered by another screen</param>
        public override void Update(IGameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            switch (gameStateManager.GameState)
            {
                case GameState.Playing:
                    PlayGame(gameTime);
                    break;
                case GameState.Death:
                    OnDeath(gameTime);
                    break;
                case GameState.Loading:
                    gameStateManager.GameState = GameState.Starting;
                    break;
                case GameState.Starting:
                    StartGame(gameTime);
                    break;
                default:
                    break;
            }

            // check periodically for new messages (one Message per update cycle)
            NetIncomingMessage msg = networkManager.getMessage();
            if (msg != null)
            {
                string msgStr = msg.ReadString();
                string[] values = msgStr.Split(';');

                switch (values[0])
                {
                        // apply direction update
                    case "updateDirection":
                        int msgUID = Convert.ToInt32(values[1]);
                        int direction = Convert.ToInt32(values[2]);
                        gameStateManager.getFromId(msgUID).Direction = (Direction)direction;
                        break;

                    // Section to applay Timestamp
                    case "TimeStamp":
                        TimeStamp t = (TimeStamp)networkManager.Deserialize(msg, values[0]);
                        t.time -= networkManager.Offset.TotalMilliseconds;
                        //t.time+=timeOffset.TotalMilliseconds;
                        //ApplyTimeStamp(t, gameTime.TotalGameTime, networkManager.getUID());
                        break;

                    // response to the time synchronisation request
                    case "syncTime":
                        serverTime = (TimeSpan)networkManager.Deserialize(msg, values[0]);
                        sendTime = gameTime.TotalGameTime;
                        networkManager.sendMessage("latencyTime");
                        break;

                    // message for the calculcation of the network delay
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
                            // calculate and save the new offset
                            networkManager.Offset = serverTime.Subtract(gameTime.TotalGameTime.Add(new TimeSpan(ticks / 5))); 
                            Console.WriteLine("upated offset: " + Math.Round(networkManager.Offset.TotalSeconds,2));
                        }
                        break;
                     
                }
                
            }

            // request every 10 seconds a time synchronisation
            if (Math.Round(gameTime.TotalGameTime.TotalMilliseconds, 0) % 10000 == 0)
            {
                ticks = 0;
                count = 0;
                sendTime = gameTime.TotalGameTime;
                networkManager.sendMessage("syncTime");
            }

            timestampcounter -= gameTime.ElapsedGameTime.Milliseconds;

            if (timestampcounter <= 0)
            {
                timestampcounter = timestamptimer;
                gameStateManager.CreateTimestamp(gameTime.TotalGameTime.TotalMilliseconds);
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }


        public void ApplyTimeStamp(TimeStamp timeStamp, TimeSpan totalGameTime, int localID)
        {
            Queue<Command> localCommands = new Queue<Command>(gameStateManager.RollBack(timeStamp.time, localID));
            gameStateManager.TimeStampManager.SwapFirstTimeStamp(timeStamp);

            GameLoop gameLoop = new GameLoop(0);


            SimulationGameTime simulationGameTime = new SimulationGameTime(totalGameTime, totalGameTime.TotalMilliseconds - timeStamp.time);
            gameLoop.InputQueue = localCommands;

            while (simulationGameTime.SimulationTime > 0)
            {
                gameLoop.SimulationLoop(simulationGameTime, this.gameStateManager);
                simulationGameTime.SimulationTime -= 50;
            }

            
        }

    }

    
}
