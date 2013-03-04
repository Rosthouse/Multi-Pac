using System;
namespace PacManClient.Components.Workers
{
    interface Loader
    {
        bool IsFinished { get; }
        void Load();

        void AddWorkingItems(ScreenManager ScreenManager, GameScreen[] screensToLoad);
    }
}
