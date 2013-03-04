using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PacManShared.Controllers;
using PacManShared.Enums;
using PacManShared.LevelClasses;
using PacManShared.LevelClasses.Cells;
using PacManShared.Util.TimeStamps;

namespace PacManShared.Entities.Player
{

    public class PacMan : MovableObject
    {
        private SpriteFont font;

        private Point framePosition;
        private int frameTimer = 0;
        private bool UpWardsAnimation = true;
        private float angle = 0;

        /// <summary>
        /// Constructs a new PacMan
        /// </summary>
        /// <param name="textureAsset">The path to the texture of this pacman</param>
        /// <param name="startCell">the starting cell of this  player</param>
        /// <param name="level">Reference to the level</param>
        /// <param name="controller">The controller for this player</param>
        /// <param name="startDirection">The start heading</param>
        /// <param name="speed">Defines how fast this player can move in cells per second</param>
        /// <param name="frameSize">Sets the framesize for the players texture</param>
        public PacMan(String textureAsset, Cell startCell, Level level, Controller
            controller, Direction startDirection, float speed,
                      Point frameSize)
            : base(textureAsset, startCell, level, startDirection, speed, controller, frameSize)
        {
            framePosition = new Point(0, 0);

            position = base.CurrentCell.Position;
            this.lives = 5;
        }


        /// <summary>
        /// Handles access to the Direction of Pacman
        /// </summary>
        public override Direction Direction
        {
            get { return currentDirection; }
            set
            {
                lastDirection = currentDirection;
                currentDirection = value;
            }
        }

        /// <summary>
        /// loads the content for this pacman
        /// </summary>
        /// <param name="contentManager">The contentmanager</param>
        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
            //Texture = contentManager.Load<Texture2D>(@"Sprites\PacManEating");
            font = contentManager.Load<SpriteFont>(@"Spritefonts\GameFont");
            position = calculatePosition();
        }
        
        /// <summary>
        /// Loads the content for this pacman
        /// </summary>
        /// <param name="contentManager">the contentmanager</param>
        /// <param name="soundBank">the soundbank for this pacman</param>
        public void LoadContent(ContentManager contentManager, SoundBank soundBank)
        {
            LoadContent(contentManager);
 
            soundEffects.Add("Eat1", soundBank.GetCue("PacManEat1"));
            soundEffects.Add("Eat2", soundBank.GetCue("PacManEat2"));
            soundEffects.Add("EatFruit", soundBank.GetCue("EatFruit"));
        }

        /// <summary>
        /// Updates pacman
        /// </summary>
        /// <param name="gameTime">the current gametime</param>
        public override void Update(IGameTime gameTime)
        {

            controller.Update(CurrentCell);
            if (controller.MovObjType == MovObjType.LocalPacMan)
            {
                Direction = controller.Direction;
            }
            else 
            {
                Direction = currentDirection;
            }

            //Create a movement Vector
            SpeedVector = GetSpeedVector(Direction, SpeedVector, gameTime.ElapsedGameTime.Milliseconds);
            Vector2 correctionVector = Vector2.Zero;

            //Update the position + the player camera
            position += SpeedVector;

            Dictionary<Direction, Cell> possibleMoves = GetPossibleMoves(CurrentCell);
            
            bool intersects = false;

            //Check if the next movement is even possible
            for (int i = 1; i <= possibleMoves.Count; i++)
            {
                Cell cellToCheck = possibleMoves[(Direction) i];

                
                if(cellToCheck.IsWall)
                {
                    if(IntersectRectangle.Intersects(cellToCheck.IntersectRectangle))
                    {
                        Direction correction = DirectionExtension.GetOppositeDirection((Direction) i);

                        correctionVector = GetSpeedVector(correction, Vector2.Zero, gameTime.ElapsedGameTime.Milliseconds);

                        position += correctionVector;
                        SpeedVector += correctionVector;
                    }
                }

                if((new Rectangle((int)Center.X, (int)Center.Y, 1, 1)).Intersects(cellToCheck.IntersectRectangle))
                {
                    
                    CurrentCell = cellToCheck;
                    intersects = true;
                }

            }


            if ((new Rectangle((int)Center.X, (int)Center.Y, 1, 1)).Intersects(CurrentCell.IntersectRectangle))
            {
                intersects = true;
            }


            base.Update(gameTime);

            if(intersects == false)
            {
                SpeedVector -= correctionVector;
                position = CurrentCell.Position + Offset + SpeedVector;
                Direction = lastDirection;
                //throw new IndexOutOfRangeException("Pacman has left his grid");
            }

            HandleFramePosition(SpeedVector);
        }

        /// <summary>
        /// Changes the frameposition according to the current time
        /// </summary>
        /// <param name="speedVector">The SpeedVector</param>
        private void HandleFramePosition(Vector2 speedVector)
        {
            float speed = 0;
            if(speedVector.X != 0)
            {
                speed = speedVector.X;

                if (SpeedVector.X > 0)
                {
                    angle = MathHelper.ToRadians(90);
                }
                else
                {
                    angle = MathHelper.ToRadians(270);
                }
            }
            else if(speedVector.Y != 0)
            {
                speed = speedVector.Y;

                if (SpeedVector.Y > 0)
                {
                    angle = MathHelper.ToRadians(180);
                }
                else
                {
                    angle = 0;
                }
            }

            frameTimer += (int)Math.Abs(speed);

            if(frameTimer >= 50)
            {
                if(UpWardsAnimation)
                {
                    framePosition.X++;
                    if (framePosition.X == 2)
                    {
                        UpWardsAnimation = false;
                    }
                } 
                else
                {
                    framePosition.X--;
                    if (framePosition.X == 0)
                    {
                        UpWardsAnimation = true;
                    } 
                }
                

                frameTimer = 0;
            }
        }


        /// <summary>
        /// Draws Pacman
        /// </summary>
        /// <param name="spriteBatch">Spritebatch from the GameObjectManager</param>
        /// <param name="layer">the layer</param>
        /// <param name="levelPosition">The position of the level onscreen</param>
        public override void Draw(SpriteBatch spriteBatch, int layer, Vector2 levelPosition)
        {
            
            Rectangle rectangle = new Rectangle(framePosition.X*frameSize.X, framePosition.Y*frameSize.Y, frameSize.X, frameSize.Y);

            Vector2 origin = new Vector2((float)frameSize.X/2, (float)frameSize.Y/2);
            
            spriteBatch.Draw(Texture, Center+levelPosition, rectangle, Color.White, angle, origin, 1, SpriteEffects.None, layer);

        }

        /// <summary>
        /// Returns a Dictionary countaining the surrounding cells 
        /// </summary>
        /// <param name="currentCell">The current cell</param>
        /// <returns></returns>
        private Dictionary<Direction, Cell> GetPossibleMoves(Cell currentCell)
        {
            
            Dictionary<Direction, Cell> possibleMoves = new Dictionary<Direction, Cell>();
            Point currentPosition = currentCell.GridPosition;

            for (int i = 1; i <= 4; i++)
            {
                Point directionPoint = DirectionExtension.PointFromDirection((Direction)i);

                Point getCell = new Point(currentPosition.X + directionPoint.X,
                                          currentPosition.Y + directionPoint.Y);

                Cell possibleCell = Cell.Empty;

               if(!(getCell.X < 0 || getCell.X >= level.Size.X || getCell.Y < 0 || getCell.Y >= level.Size.Y))
               {
                    possibleCell = level.getCell(currentPosition.X + directionPoint.X,
                                                 currentPosition.Y + directionPoint.Y);
               }

               possibleMoves.Add((Direction)i, possibleCell);
            }

            return possibleMoves;
        }

        /// <summary>
        /// Resets pacman and reduces his lives
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            this.lives--;
        }

        /// <summary>
        /// Gets a structure for this Pacman
        /// </summary>
        /// <returns>A MovObjStruct according to this pacman</returns>
        public override MovObjStruct GetStruct()
        {
            return new MovObjStruct(this.CurrentCell.GridPosition, this.Offset, this.Direction, this.controller.ID, MovObjType.LocalPacMan);
        }
    }
}
