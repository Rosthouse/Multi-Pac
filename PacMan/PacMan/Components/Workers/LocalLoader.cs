using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PacManClient.Components.Workers
{
    class LocalLoader : PacManClient.Components.Workers.Loader
    {
        private static volatile bool isFinished;
        private GameScreen[] gameScreens;
        private ScreenManager screenManager;

        public bool IsFinished
        {
            get { return isFinished; }
        }

        public void Load()
        {
            isFinished = false;

            foreach (GameScreen screen in gameScreens)
            {
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
