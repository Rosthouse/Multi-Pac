using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PacManClient.Controller.Local;
using PacManShared;
using PacManShared.Entities.Player;
using PacManShared.LevelClasses;

namespace PacManClient.Components
{
    internal class GameObjectManager : DrawableGameComponent
    {
        //Camera
        private Camera camera;


        //Scale dependent Members
        private float currentScale;
        private GraphicsDeviceManager graphics;
        private bool isResizing;
        private Level level;
        private LevelParser levelParser;

        private PacMan pacman;
        private SpriteBatch spriteBatch;
        private float wantedScale;

        /// <summary>
        /// Creates a new GameObjectManager
        /// </summary>
        /// <param name="game">Reference to the parent game class</param>
        public GameObjectManager(Game game)
            : base(game)
        {
            graphics = new GraphicsDeviceManager(game);
            game.Content.RootDirectory = "Content";
            //camera = new Camera(1, Vector2.Zero, 0, Game.GraphicsDevice);
        }

        /// <summary>
        /// Gets or sets the new wanted scale of the grid
        /// </summary>
        public float WantedScale
        {
            get { return wantedScale; }
            set
            {
                wantedScale = value;
                //isResizing = true;
            }
        }

        public override void Initialize()
        {
            levelParser = new LevelParser();
            level = levelParser.generateLevel(@"Content\Level1.csv", 1);


            camera = new Camera(1, Vector2.Zero, 0, false, Game.GraphicsDevice);

            //pacman = new PacMan(level.getCell(1, 1), level,
//                                                new PlayerController(Direction.Down, "Player 1", PlayerIndex.One, ), Direction.None, 2f,
//                                                new Point(50, 50));
            //pacman.Scale = 1;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            level.LoadContent(Game.Content);
            pacman.LoadContent(Game.Content);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                var random = new Random(gameTime.TotalGameTime.Milliseconds);
                WantedScale = (float) random.NextDouble();
                camera.Zoom = WantedScale;
            }

            Resize(0.005f);


            level.Update(gameTime);
            pacman.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //
            //camera.Position = new Vector2(-200, -200);
            //camera.Zoom = 2;
            //camera.Zoom = 0.5f;

            spriteBatch.Begin(SpriteSortMode.Deferred,
                              BlendState.AlphaBlend,
                              null, null, null, null,
                              camera.getTransformation(GraphicsDevice));


            level.Draw(spriteBatch, 5);
            pacman.Draw(spriteBatch, 4, level.LevelPosition);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Handles the live resizing of the level
        /// </summary>
        /// <param name="changeValue">The value the Level has to change in this step</param>
        public void Resize(float changeValue)
        {
            if (isResizing)
            {
                if (currentScale < wantedScale)
                {
                    currentScale += changeValue;
                    if (currentScale >= wantedScale)
                    {
                        isResizing = false;
                        wantedScale = currentScale;
                    }
                }
                else if (currentScale > wantedScale)
                {
                    currentScale -= changeValue;
                    if (currentScale <= wantedScale)
                    {
                        isResizing = false;
                        wantedScale = currentScale;
                    }
                }

                camera.Zoom = WantedScale;
            }
        }
    }
}