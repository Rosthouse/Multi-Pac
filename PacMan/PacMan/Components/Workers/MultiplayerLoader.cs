using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using PacManClient.Components.GameScreens;
using PacManNetworkShared;

namespace PacManClient.Components.Workers
{
    class MultiplayerLoader : PacManClient.Components.Workers.Loader
    {
        private static volatile bool isFinished;
        private static volatile bool playersReady;
        private GameScreen[] gameScreens;
        private ScreenManager screenManager;
        //private NetClient client;
        private INetworkManager networkManager;
        private TimeSpan timeOffset;

        public MultiplayerLoader(INetworkManager networkManager, TimeSpan timeOffset)
        {
            //this.client = client;
            this.networkManager = networkManager;
            this.timeOffset = timeOffset;
        }

        public bool IsFinished
        {
            get { return isFinished; }
        }

        public void Load()
        {
            isFinished = false;
            playersReady = false;

            while (!playersReady)
            {
                NetIncomingMessage msg;
                while ((msg = networkManager.getMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            string msgStr = msg.ReadString();
                            if (msgStr == "start")
                            {
                                playersReady = true;
                            }
                            break;
                    }
                }
            }

            foreach (GameScreen screen in gameScreens)
            {
                if (screen is MultiplayerScreen)
                {
                    //((MultiplayerScreen)screen).client = client;
                    ((MultiplayerScreen)screen).TimeOffset = timeOffset;
                }
                screenManager.AddScreen(screen, PlayerIndex.One);
            }

            isFinished = true;
        }


        public void AddWorkingItems(ScreenManager screenManager, params GameScreen[] screensToLoad)
        {
            this.screenManager = screenManager;
            gameScreens = screensToLoad;
        }  
    }
}
