using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacManClient.Components.Workers
{
    class EmptyLoader : PacManClient.Components.Workers.Loader

    {

        public bool IsFinished
        {
            get { return true; }
        }

        public void Load()
        {
            //throw new NotImplementedException();
        }

        public void AddWorkingItems(ScreenManager ScreenManager, GameScreen[] screensToLoad)
        {
            //throw new NotImplementedException();
        }
    }
}
