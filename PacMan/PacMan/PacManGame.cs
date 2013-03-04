using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PacManClient.Components;
using PacManClient.Components.GameScreens;
namespace PacManClient
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PacManGame: Game
    {
        private GraphicsDeviceManager graphics;
        private readonly ScreenManager screenManager;

        /// <summary>
        /// Creates a new Instance of PacManGame
        /// </summary>
        public PacManGame()
        {

            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            screenManager = new ScreenManager(this);
            //console = Console.GetInstance(this);

            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
            
            Components.Add(screenManager);
            //Components.Add(console);

        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            foreach (GameComponent component in Components)
            {
                component.Initialize();
            }
            base.Initialize();

            DisplayModeCollection modes = graphics.GraphicsDevice.Adapter.SupportedDisplayModes;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            base.Update(gameTime); 
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void OnDeactivated(object sender, System.EventArgs args)
        {
            if (GraphicsDevice.PresentationParameters.IsFullScreen)
            {
                GraphicsDevice.PresentationParameters.IsFullScreen = false;
                GraphicsDevice.Reset();
            }

            base.OnDeactivated(sender, args);
        }
    }
}