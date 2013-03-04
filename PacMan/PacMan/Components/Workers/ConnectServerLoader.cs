using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using PacManClient.Components.GameScreens;

namespace PacManClient.Components.Workers
{
    class ConnectServerLoader : Loader
    {
        private static volatile bool isFinished;
        private GameScreen[] gameScreens;
        private ScreenManager screenManager;
        private NetClient client;
        private NetPeerConfiguration clientConfig;
        private GameTime gameTime;
        static private double timeoutLimit = 5000;
        private double timeoutCount = 0;

        #region Loader Members

        public ConnectServerLoader()
        {
            this.gameTime = new GameTime();
        }

        public bool IsFinished
        {
            get { return isFinished; }
        }

        public void Load()
        {
            isFinished = false;

            // create client configuration
            clientConfig = new NetPeerConfiguration("pacman");
            //configure client
            client = new NetClient(clientConfig);
            //Start client
            client.Start();
            
            if (client != null)
            {
                NetIncomingMessage msg;
                bool waitingForServer = true;

                // Contact Server
                client.DiscoverKnownPeer("localhost", 666);
                //client.DiscoverLocalPeers(666);

                while (waitingForServer)
                {
                    while ((msg = client.ReadMessage()) != null)
                    {
                        switch (msg.MessageType)
                        {
                            case NetIncomingMessageType.DiscoveryResponse:
                                try
                                {
                                    client.Connect(msg.SenderEndpoint);
                                    //waitingForServer = false;
                                }
                                catch (Exception e)
                                {
                                    System.Console.WriteLine(e.ToString());
                                    //waitingForServer = false;
                                }
                                break;
                            case NetIncomingMessageType.Data:
                                if (msg.ReadString() == "connected")
                                {
                                    waitingForServer = false;
                                }
                                break;
                        }
                    }
                    
                    if (timeoutCount > timeoutLimit)
                    {
                        waitingForServer = false;
                    }
                    double elapsedTime = gameTime.ElapsedGameTime.TotalMilliseconds;
                    timeoutCount += elapsedTime;
                }
            }

            foreach (GameScreen screen in gameScreens)
            {
                if (screen is ConfigMultiplayerScreen)
                {
                    //((ConfigMultiplayerScreen)screen).setClient(client);
                    //((ConfigMultiplayerScreen)screen).setConntectedServer(client.ServerConnection.RemoteEndpoint.Address.ToString());
                }
                screenManager.AddScreen(screen, PlayerIndex.One);
            }

            isFinished = true;
        }

        public void AddWorkingItems(ScreenManager ScreenManager, GameScreen[] screensToLoad)
        {
            this.screenManager = ScreenManager;
            gameScreens = screensToLoad;
        }

        #endregion
    }
}
