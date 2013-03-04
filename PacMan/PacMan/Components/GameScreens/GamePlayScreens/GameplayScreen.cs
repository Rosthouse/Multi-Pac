#region Using Statements

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PacManClient.Components.GameScreens.GamePlayScreens.GUI;
using PacManClient.Controller.Local;
using PacManShared;
using PacManShared.Entities.PickUp;
using PacManShared.Entities.Player;
using PacManShared.Enums;
using PacManShared.GameplayBehaviour;
using PacManShared.LevelClasses;
using Lidgren.Network;
using PacManShared.LevelClasses.Cells;
using PacManShared.LevelClasses.Effects;
using PacManShared.Util.TimeStamps;

#endregion

namespace PacManClient.Components.GameScreens
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    public abstract class GameplayScreen : GameScreen
    {
        #region Fields

        //Pacman members
        protected Camera camera;

        protected ContentManager content;
        private SpriteFont gameFont;
        protected Level level;

        private float pauseAlpha;
        private Random random = new Random();
        private SpriteBatch spriteBatch;

        protected GameStateManager gameStateManager;

        protected PlayBehaviour playBehaviour;
        protected DeathBehaviour deathBehaviour;

        protected List<GUIElement> guiElements;

        #endregion

        #region Accessors

        /// <summary>
        /// Gets or sets the current gamestate
        /// </summary>
        public GameState GameState
        {
            get { return gameStateManager.GameState; }
            set { gameStateManager.GameState = value; }
        }

        #endregion - Accessors

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            playBehaviour = new PlayBehaviour();
            deathBehaviour = new DeathBehaviour();
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }
                

            gameFont = content.Load<SpriteFont>(@"spritefonts\gamefont");

            camera.GraphicsDevice = ScreenManager.GraphicsDevice;

            gameStateManager.Level.LoadContent(content);

            foreach (Cell cell in gameStateManager.Level.Grid)
            {
                ICellEffect cellEffect = cell.CellEffect;

                PowerUp pu = cellEffect as PowerUp;

                if(pu != null)
                {
                    pu.OnPickedUp += new PowerUpEffectEvent(OnPowerUpPickedUp);
                }
            }

            foreach(MovableObject movableObject in gameStateManager.MovableObjects)
            {
               movableObject.LoadContent(content); 
            }


            Vector2 levelOffset = Vector2.Zero;

            foreach(GUIElement element in guiElements)
            {
                element.LoadContent(content);
                levelOffset.X += element.Size.X;
            }

            if (levelOffset.X == 0)
            {
                gameStateManager.Level.LevelPosition = new Vector2(300, 0);
            }
            else
            {
                gameStateManager.Level.LevelPosition = levelOffset;
            }

            if(camera.IsStatic)
            {
                float scale;
                if(gameStateManager.Level.Size.X >= gameStateManager.Level.Size.Y)
                {
                    scale = ScreenManager.GraphicsDevice.Viewport.Width / (gameStateManager.Level.Size.X * gameStateManager.Level.Grid[0, 0].Size.X);
                }
                else
                {
                    scale = ScreenManager.GraphicsDevice.Viewport.Height / (gameStateManager.Level.Size.Y * gameStateManager.Level.Grid[0, 0].Size.Y);
                }

                camera.Zoom = scale;
            }


            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            //Thread.Sleep(5000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        /// <summary>
        /// Creates a player after a MovObjStruct
        /// </summary>
        /// <param name="movObjStruct">The MovObjStruct containing the informations for a player</param>
        /// <remarks>Isn't implemented right now</remarks>
        private void CreatePlayer(MovObjStruct movObjStruct)
        {
//            switch (movObjStruct.movObjType)
//            {
//                case MovObjType.PacMan:
//                    PacMan pacMan = new PacMan(@"Sprites\PacManEating", 
//                        level.getCell(movObjStruct.currentCell), 
//                        level,
//                        new PlayerController(Direction.None, movObjStruct.ID.ToString(), PlayerIndex.One, MovObjType.PacMan, ScreenManager.Input), 
//                        movObjStruct.direction, 
//                        3f, 
//                        new Point(50, 50));
//                    gameStateManager.AddPlayers(pacMan);
//                    break;
//                case MovObjType.Blinky:
//                    Ghost blinky = new Ghost(@"Sprites\GhostBase", 
//                        level.getCell(movObjStruct.currentCell), 
//                        level,
//                        movObjStruct.direction, 
//                        3f,
//                        new Point(50, 50),
//                        Color.Red,
//                        new Blinky(gameStateManager.MovableObjects.Find(), new Point(0, 0), new Point(15, 15), new Point(16, 16)));
//                    gameStateManager.AddPlayers(blinky);
//                    break;
//                case MovObjType.Inky:
//                    Ghost inky = new Ghost(@"Sprites\GhostBase", 
//                        level.getCell(26, 14), 
//                        level, 
//                        Direction.None, 
//                        3f, 
//                        new Point(50, 50), 
//                        Color.Blue,
//                        new Inky(player, blinky, new Point(31, 0), new Point(15, 15), new Point(16, 16)));
//            
//            
//            }
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(IGameTime gameTime, bool otherScreenHasFocus,
                                    bool coveredByOtherScreen)
        {
            if(IsActive)
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
 
            }
            
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
            {

                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            }
            else
            {
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
                if(GameState == GameState.Loading)
                {
                    GameState = GameState.Playing;
                }
            }
                

        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            var playerIndex = (int) ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(IGameTime gameTime)
        {
            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin(SpriteSortMode.Deferred,
                              BlendState.AlphaBlend,
                              null, null, null, null,
                              camera.GetTransformation(ScreenManager.GraphicsDevice));

            gameStateManager.Level.Draw(spriteBatch, 5);

            //foreach (MovableObject movableObject in movOb)
            foreach(MovableObject movableObject in gameStateManager.MovableObjects)
            {
                movableObject.Draw(spriteBatch, 4, gameStateManager.Level.LevelPosition);   
            }

            foreach(GUIElement element in guiElements)
            {
                element.Draw(spriteBatch, 6);
            }

            spriteBatch.End();

            doTransition();
        }

        /// <summary>
        /// Does the screen transition (fading in and out)
        /// </summary>
        public void doTransition()
        {
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha/2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        /// <summary>
        /// Starts the current game
        /// </summary>
        /// <param name="gameTime">the current gametime</param>
        public void StartGame(IGameTime gameTime)
        {
            gameStateManager.GameState = GameState.Playing;
        }

        /// <summary>
        /// Plays the game, updates it according to the current playbehaviour
        /// </summary>
        /// <param name="gameTime">the current gametime</param>
        public void PlayGame(IGameTime gameTime)
        {
            
            playBehaviour.Update(gameTime, gameStateManager);
            

            camera.Update();
        }

        /// <summary>
        /// Handles the gamestate if pacman is eaten
        /// </summary>
        /// <param name="gameTime">the current gametime</param>
        public void OnDeath(IGameTime gameTime)
        {
            foreach (MovableObject movableObject in gameStateManager.MovableObjects)
            {
                movableObject.Reset();
                
            }

            //Das hier auskommentiert lassen, damit beim Sterben nicht das Level von neuem beginnt
            //gameStateManager.Level.Reset();

            gameStateManager.GameState = GameState.Starting;
        }

        #endregion

        #region Eventhandeling

        /// <summary>
        /// This gets called when a Pacman eats a PowerUp
        /// </summary>
        /// <param name="sender">The powerup that has been eaten</param>
        /// <param name="e">The event arguments</param>
        public void OnPowerUpPickedUp(object sender, PowerUpEffectEventArgs e)
        {
            PowerUp from = (PowerUp)sender;



            foreach (MovableObject movableObject in gameStateManager.MovableObjects)
            {
                Ghost ghost = movableObject as Ghost;
                if(ghost != null)
                {
                    ghost.PowerUpTimer = e.timer;
                    ghost.CellsPerSecond *= e.speed;
                    ghost.GhostBehaviour = e.ghostBehaviour;
                }
            }
        }

        /// <summary>
        /// Creates a timestamp when a timestamp even has been fired
        /// </summary>
        /// <param name="sender">The object that fired the event</param>
        /// <param name="e">The timestamp arguments</param>
        public void CreateTimeStamp(object sender, TimeStampEventArgs e)
        {
            
        }

        #endregion
    }
}
