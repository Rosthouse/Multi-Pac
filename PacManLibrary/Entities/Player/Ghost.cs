using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using PacManShared.Controllers;
using PacManShared.Controllers.AI;
using PacManShared.Controllers.AI.IndividualAI;
using PacManShared.Enums;
using PacManShared.LevelClasses;
using PacManShared.LevelClasses.Cells;
using PacManShared.Util.TimeStamps;

namespace PacManShared.Entities.Player
{
    public class Ghost : MovableObject
    {
        private Point framePosition;
        private Color color;
        private readonly Color defaultColor;

        /// <summary>
        /// Creates a new Ghost
        /// </summary>
        /// <param name="textureAsset">The path to the texture of this ghost</param>
        /// <param name="startCell">The starting cell for this ghost</param>
        /// <param name="level">The level</param>
        /// <param name="startDirection">The starting direction for this ghost</param>
        /// <param name="speed">The speed in cells per secon</param>
        /// <param name="frameSize">The framesize of a frame from the texture</param>
        /// <param name="color">The default color of this ghost</param>
        /// <param name="ghostAi">The ghostAi for this ghost</param>
        public Ghost(String textureAsset, Cell startCell, Level level, Direction startDirection, float speed, Point frameSize, Color color, GhostAi ghostAi)
            : this(textureAsset, startCell, level, startDirection, speed, frameSize, color, new GhostController(level, ghostAi, 1))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textureAsset">The path to the texture of this ghost</param>
        /// <param name="startCell">The starting cell for this ghost</param>
        /// <param name="level">The level</param>
        /// <param name="startDirection">The starting direction for this ghost</param>
        /// <param name="speed">The speed in cells per secon</param>
        /// <param name="frameSize">The framesize of a frame from the texture</param>
        /// <param name="color">The default color of this ghost</param>
        /// <param name="controller">A controller for this ghost</param>
        public Ghost(String textureAsset, Cell startCell, Level level, Direction startDirection, float speed, Point frameSize, Color color, Controller controller)
            : base(textureAsset, startCell, level, startDirection, speed, controller, frameSize)
        {
            framePosition = new Point(1, 1);
            position = startCell.Position;
            this.color = color;
            this.defaultColor = this.color;

            GhostBehaviour = EGhostBehaviour.Hunt;
        }

        /// <summary>
        /// Gets or sets the current direction
        /// </summary>
        public override Direction Direction
        {
            get { return currentDirection; }
            set
            {
                this.lastDirection = currentDirection;
                this.currentDirection = value;
                
            }
        }

        /// <summary>
        /// Sets the current GhostBehaviour
        /// </summary>
        public EGhostBehaviour GhostBehaviour
        {
            set 
            { 
                ((IGhostStateObserver)controller).SetGhostState(value);
            }
        }

        /// <summary>
        /// Loads the content for this Ghost
        /// </summary>
        /// <param name="contentManager">A contentManager</param>
        public override void LoadContent(ContentManager contentManager)
        {
            //Texture = contentManager.Load<Texture2D>(@"Sprites\GhostBase");
            base.LoadContent(contentManager);
            position = calculatePosition();

        }

        /// <summary>
        /// Updates this ghost
        /// </summary>
        /// <param name="gameTime">the current gametime</param>
        public override void Update(IGameTime gameTime)
        {
            controller.Update(CurrentCell);
            
            if (controller.MovObjType != MovObjType.LocalGhost)
            {
                Direction = controller.Direction;
            }
            else
            {
                Direction = currentDirection;
            }

            SpeedVector = GetSpeedVector(Direction, Vector2.Zero, gameTime.ElapsedGameTime.Milliseconds);

            position += SpeedVector;

            bool validMove = ValidateMove(position, CurrentCell, Direction);

            if(!validMove)
            {
                position -= SpeedVector;
                ResetSpeedVector();

                position += SpeedVector;
                Direction = lastDirection;
            }

            CheckNextCell(Direction);

            #region old Movement code
            //            foreach (Cell cell in level.Grid)
//            {
                //Check for the current cell
//                if (cell.IntersectRectangle.Intersects((new Rectangle((int) Center.X, (int) Center.Y, 1, 1))))
//                {
//                    CurrentCell = cell;
//
//                    if (CurrentCell.IsWall)
//                    {
//                        string somethingFucked = "lol";
//                    }
//
//
//                }
//            }
//
//                if(cell.IsWall)
//                {
//                    if (cell.IntersectRectangle.Intersects(this.IntersectRectangle))
//                    {
//                        position -= SpeedVector;
//                        ResetSpeedVector();
//                        position += SpeedVector;
//                    }  
//                }
//                
            //            }

            #endregion - old Movement code

            base.Update(gameTime);
        }

        /// <summary>
        /// Checks if the currentCell is still valid and sets it to the next one if needed
        /// </summary>
        /// <param name="movingDirection">the direction we're moving to</param>
        private void CheckNextCell(Direction movingDirection)
        {
            Point directionPoint = DirectionExtension.PointFromDirection(movingDirection);

            Cell nextCell = level.getCell(CurrentCell.GridPosition.X + directionPoint.X, CurrentCell.GridPosition.Y + directionPoint.Y);

            Rectangle centerIntersect = new Rectangle((int)Center.X, (int)Center.Y, 1, 1);

            if (centerIntersect.Intersects(nextCell.IntersectRectangle))
            {
                CurrentCell = nextCell;
                if(nextCell.IsWall)
                {
                    string s = "something";
                }
            }
        }

        /// <summary>
        /// Checks if we didn't hit a wall with our last move
        /// </summary>
        /// <param name="position">The current position</param>
        /// <param name="currentCell">Our current cell</param>
        /// <param name="direction">the current direction</param>
        /// <returns>true if we didn't hit a wall, false if we did</returns>
        private bool ValidateMove(Vector2 position, Cell currentCell, Direction direction)
        {
            Point movement = DirectionExtension.PointFromDirection(direction);

            Point[] cellsToCheck = new Point[3];
            switch (direction)
            {
                case Enums.Direction.Up:
                    for (int i = -1; i < 2; i++)
                    {
                        cellsToCheck[i + 1] = new Point(currentCell.GridPosition.X + i, currentCell.GridPosition.Y - 1);
                    }
                    break;
                case Enums.Direction.Down:
                    for (int i = -1; i < 2; i++)
                    {
                        cellsToCheck[i + 1] = new Point(currentCell.GridPosition.X + i, currentCell.GridPosition.Y + 1);
                    }
                    break;
                case Enums.Direction.Left:
                    for (int i = -1; i < 2; i++)
                    {
                        cellsToCheck[i + 1] = new Point(currentCell.GridPosition.X - 1, currentCell.GridPosition.Y + i);
                    }
                    break;
                case Enums.Direction.Right:
                    for (int i = -1; i < 2; i++)
                    {
                        cellsToCheck[i + 1] = new Point(currentCell.GridPosition.X + 1, currentCell.GridPosition.Y + i);
                    }
                    break;
            }

            foreach (Point point in cellsToCheck)
            {
                Cell cellToCheck = level.getCell(point);
                
                if(this.IntersectRectangle.Intersects(cellToCheck.IntersectRectangle) && cellToCheck.IsWall)
                {
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// Draws this ghost
        /// </summary>
        /// <param name="spriteBatch">the spritebatch</param>
        /// <param name="layer">the layer we want to render into</param>
        /// <param name="levelPosition">the levelposition onscreen</param>
        public override void Draw(SpriteBatch spriteBatch, int layer, Vector2 levelPosition)
        {
            spriteBatch.Draw(Texture, position + levelPosition,null, color,
                             0, Vector2.Zero, 1, SpriteEffects.None, layer-1);
        }

        /// <summary>
        /// Resets this ghost to its default values
        /// </summary>
        protected override void ResetToDefault()
        {
            this.color = defaultColor;
            base.ResetToDefault();
        }

        /// <summary>
        /// Returns a MovObjStruct for this ghost
        /// </summary>
        /// <returns>A MovObjStruct for this ghost</returns>
        public override MovObjStruct GetStruct()
        {
            return new MovObjStruct(this.CurrentCell.GridPosition, this.Offset, this.Direction, this.controller.ID, controller.MovObjType);
        }
    }
}