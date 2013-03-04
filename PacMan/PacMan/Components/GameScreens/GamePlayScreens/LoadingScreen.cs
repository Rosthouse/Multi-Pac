#region File Description

//-----------------------------------------------------------------------------
// LoadingScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PacManClient.Components.Workers;
using PacManShared;
using PacManShared.Entities;

#endregion

namespace PacManClient.Components.GameScreens
{
    /// <summary>
    /// The loading screen coordinates transitions between the menu system and the
    /// game itself. Normally one screen will transition off at the same time as
    /// the next screen is transitioning on, but for larger transitions that can
    /// take a longer time to load their data, we want the menu system to be entirely
    /// gone before we start loading the game. This is done as follows:
    /// 
    /// - Tell all the existing screens to transition off.
    /// - Activate a loading screen, which will transition on at the same time.
    /// - The loading screen watches the state of the previous screens.
    /// - When it sees they have finished transitioning off, it activates the real
    ///   next screen, which may take a long time to load its data. The loading
    ///   screen will be the only thing displayed while this load is taking place.
    /// </summary>
    internal class LoadingScreen : GameScreen
    {
        #region Fields

        private bool loadingIsSlow;

        private GameScreen[] screensToLoad;
        private List<LoadObject> loadObjects;
        private bool otherScreensAreGone;
        private Loader loader;
        private Thread loadThread;

        #endregion

        #region Initialization

        /// <summary>
        /// The constructor is private: loading screens should
        /// be activated via the static Load method instead.
        /// </summary>
        private LoadingScreen(bool loadingIsSlow, Loader loaderToLoad,
                              GameScreen[] screensToLoad)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;

            loader = loaderToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Activates the loading screen.
        /// </summary>
        public static void Load(ScreenManager screenManager, bool loadingIsSlow,
                                PlayerIndex? controllingPlayer, Loader loaderToLoad,
                                params GameScreen[] screensToLoad)
        {
            // Tell all the current screens to transition off.
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            // Create and activate the loading screen.
            var loadingScreen = new LoadingScreen(loadingIsSlow, loaderToLoad,
                                                  screensToLoad);

            screenManager.AddScreen(loadingScreen, controllingPlayer);
        }

        /// <summary>
        /// Starts the loading process
        /// </summary>
        /// <param name="screenManager">The screenmanager</param>
        /// <param name="loadingIsSlow">If loading is slow</param>
        /// <param name="controllingPlayer">The player who induced the loading</param>
        /// <param name="screensToLoad">All the screens that need to be loaded</param>
        public static void Load(ScreenManager screenManager, bool loadingIsSlow,
                                PlayerIndex? controllingPlayer,
                                params GameScreen[] screensToLoad)
        {
            Load(screenManager, loadingIsSlow, controllingPlayer, new LocalLoader(), screensToLoad);
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Initializes this screen
        /// </summary>
        public override void Initialize()
        {
            loadObjects = new List<LoadObject>();

            for (int i = 0; i <= 4; i++)
            {
                loadObjects.Add(new LoadObject(@"Sprites\PacManEating2", new Point(1, 1)));
            }
        }

        /// <summary>
        /// Loads the content for this screen
        /// </summary>
        public override void LoadContent()
        {
            var positon = new Vector2(-50, (float) ScreenManager.GraphicsDevice.Viewport.Height/2);
            foreach (LoadObject loadObject in loadObjects)
            {
                //loadObject.Texture = ScreenManager.Content.Load<Texture2D>(@"Sprites\PacManEating2");
                loadObject.LoadContent(ScreenManager.Content);
                loadObject.Position = positon;
                positon.X -= 50;
            }

            loader.AddWorkingItems(ScreenManager, screensToLoad);

            loadThread = new Thread(loader.Load);
            base.LoadContent();

            loadThread.Start();
        }

        /// <summary>
        /// Updates the loading screen.
        /// </summary>
        public override void Update(IGameTime gameTime, bool otherScreenHasFocus,
                                    bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            

            // If all the previous screens have finished transitioning
            // off, it is time to actually perform the load.
            if (otherScreensAreGone)
            {
                

//                ScreenManager.RemoveScreen(this);
//
//                foreach (GameScreen screen in screensToLoad)
//                {
//                    if (screen != null)
//                    {
//                        ScreenManager.AddScreen(screen, ControllingPlayer);
//                    }
//                }

                if(!loadThread.IsAlive)
                {
                    ScreenManager.RemoveScreen(this);  
                }

                var speedVector = new Vector2(10, 0);

                foreach (LoadObject loadObject in loadObjects)
                {
                    loadObject.Position += speedVector;
                    if(loadObject.Position.X > ScreenManager.GraphicsDevice.Viewport.Width)
                    {
                        loadObject.Position = new Vector2(0 - loadObject.Texture.Width, loadObject.Position.Y);
                    }
                }

                // Once the load has finished, we use ResetElapsedTime to tell
                // the  game timing mechanism that we have just finished a very
                // long frame, and that it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }
        }

        /// <summary>
        /// Hanles input for this screen (Not necessary here)
        /// </summary>
        /// <param name="input">the input</param>
        public override void HandleInput(InputState input)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Draws the loading screen.
        /// </summary>
        public override void Draw(IGameTime gameTime)
        {
            // If we are the only active screen, that means all the previous screens
            // must have finished transitioning off. We check for this in the Draw
            // method, rather than in Update, because it isn't enough just for the
            // screens to be gone: in order for the transition to look good we must
            // have actually drawn a frame without them before we perform the load.
            if ((ScreenState == ScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                otherScreensAreGone = true;
            }

            // The gameplay screen takes a while to load, so we display a loading
            // message while that is going on, but the menus load very quickly, and
            // it would look silly if we flashed this up for just a fraction of a
            // second while returning from the game to the menus. This parameter
            // tells us how long the loading is going to take, so we know whether
            // to bother drawing the message.
            if (loadingIsSlow)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
//                SpriteFont font = ScreenManager.Font;
//
//                const string message = "Loading...";
//
                // Center the text in the viewport.
//                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
//                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
//                Vector2 textSize = font.MeasureString(message);
//                Vector2 textPosition = (viewportSize - textSize) / 2;
//
//                Color color = Color.White * TransitionAlpha;

                // Draw the text.
                spriteBatch.Begin();

                foreach (LoadObject loadObject in loadObjects)
                {
                    loadObject.Draw(spriteBatch, 1, new Vector2(0, 0));
                }

                spriteBatch.End();
            }
        }

        #endregion
    }
}