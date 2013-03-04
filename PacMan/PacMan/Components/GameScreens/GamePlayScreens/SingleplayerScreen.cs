using System;
using System.Collections.Generic;
using PacManClient.Components.GameScreens.GamePlayScreens.GUI;
using PacManShared.Controllers.AI.IndividualAI;
using PacManShared.Enums;
using PacManShared.GameplayBehaviour;
using PacManShared.Initialization;
using PacManShared.LevelClasses;
using PacManShared.Entities.Player;
using PacManShared;
using Microsoft.Xna.Framework;
using PacManClient.Controller.Local;

namespace PacManClient.Components.GameScreens.GamePlayScreens
{
    class SingleplayerScreen : GameplayScreen
    {
        private int timestampcounter = 0;

        /// <summary>
        /// Instantiates all the classes needed for the game
        /// </summary>
        public override void Initialize()
        {
            //generate Level from file
            var parser = new LevelParser();
            level = parser.GenerateLevel(ScreenManager.Content.Load<List<String[]>>(@"Levels\level1"));

            //create a camer, for we want to see the game
            camera = new Camera(0.4f, Vector2.Zero, 0, true, null);



            Point size = new Point(49, 49);

            PlayerController playerController = new PlayerController(Direction.None, "PacMa", 1, PlayerIndex.One,
                                                                     MovObjType.LocalPacMan, ScreenManager.Input);

            //Create a player object
            PacMan player = new PacMan(@"Sprites\PacMan", level.getCell(1, 1), level,
                                                new PlayerController(Direction.None, "Player 1",1, PlayerIndex.One, MovObjType.LocalPacMan, ScreenManager.Input), Direction.None, 3f, size);

            
            
            //Create all ghosts for the game
            

            Ghost blinky = new Ghost(@"Sprites\GhostBase", level.getCell(26, 1), level, Direction.Down, 3f, size, Color.Red, new Blinky(player, new Point(0, 0), new Point(15, 15), new Point(16, 16)));
            Ghost pinky = new Ghost(@"Sprites\GhostBase", level.getCell(26, 29), level, Direction.None, 3f, size, Color.Pink, new Pinky(player, new Point(0, 28), new Point(15, 15), new Point(16, 16)));
            Ghost inky = new Ghost(@"Sprites\GhostBase", level.getCell(26, 14), level, Direction.None, 3f, size, Color.Blue,
                                   new Inky(player, blinky, new Point(31, 0), new Point(15, 15), new Point(16, 16)));
            Ghost clyde = new Ghost(@"Sprites\GhostBase", level.getCell(12, 17), level, Direction.None, 3f, size, Color.Yellow,
                                    new Clyde(player, new Point(31, 28), new Point(15, 15), new Point(16, 16)));


            //Initialize the list of gui elements
            guiElements = new List<GUIElement>();
            GUIString stringElement = new GUIString(player, Color.White, new Vector2(0, 0), new Vector2(10, 10));


            guiElements.Add(stringElement);


            //Store everything inside the gameStateManager
            gameStateManager = new GameStateManager();
            gameStateManager.GameState = GameState.Loading;

            gameStateManager.AddPlayers(player, blinky, pinky, inky, clyde);

            gameStateManager.Level = level;

//            movOb.Add(player);
//            movOb.Add(blinky);
//            movOb.Add(pinky);
//            movOb.Add(inky);
//            movOb.Add(clyde);

            //Initialize the gameplay behaviours
            playBehaviour = new PlayBehaviour();
            deathBehaviour = new DeathBehaviour();


            gameStateManager.GetAllOfType<PacMan>();
            gameStateManager.GetAllOfType<Ghost>();
        }

        /// <summary>
        /// The update code for the singleplayer screen
        /// </summary>
        /// <param name="gameTime">the current gametime</param>
        /// <param name="otherScreenHasFocus">parameter for the gui-system</param>
        /// <param name="coveredByOtherScreen">parameter for the gui-system</param>
        public override void Update(IGameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
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
            
            timestampcounter += gameTime.ElapsedGameTime.Milliseconds;

            if(timestampcounter >=1000)
            {
                timestampcounter = 0;
                gameStateManager.CreateTimestamp(gameTime.TotalGameTime.TotalMilliseconds);
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
