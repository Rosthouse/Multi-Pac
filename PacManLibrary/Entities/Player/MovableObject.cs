using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using PacManShared.Controllers;
using PacManShared.Enums;
using PacManShared.LevelClasses;
using PacManShared.LevelClasses.Cells;
using PacManShared.Util.TimeStamps;

namespace PacManShared.Entities.Player
{
    public abstract class MovableObject : Sprite
    {
        //Directions
        private Cell currentCell;
        protected Direction currentDirection; //the current heading
        protected Point frameSize;
        protected Direction lastDirection; //the heading at the next turn

        private Cell startCell;
        
        
        //Cell
        private Vector2 offset;

        //Controller
        protected Controller controller;

        //Level references
        protected Level level;

        //CellsPerSecond
        private float _cellsPerSecond;
        private float _defaultCellsPerSecond;
        private Vector2 speedVector;
        private Vector2 lastSpeedVector;

        //Score
        public int score;

        //Lives
        public int lives;

        //Sound
        protected Dictionary<String, Cue> soundEffects; 

        //Powerup timer
        private int powerUpTimer;


        /// <summary>
        /// Constructor for a movable object
        /// </summary>
        /// <param name="textureAsset">The path to the texture that this object has to load</param>
        /// <param name="startCell">The starting cell of this MO</param>
        /// <param name="level">Reference to the level</param>
        /// <param name="startDirection">The direction this MO is facing at startup</param>
        /// <param name="cellsPerSecond">How many cells per second this MO should pass</param>
        /// <param name="controller">The controller for this object</param>
        /// <param name="frameSize">The size of the frame of this MO</param>
        public MovableObject(String textureAsset, Cell startCell, Level level, Direction startDirection, float cellsPerSecond, Controller controller, Point frameSize): base(textureAsset)
        {
            currentDirection = startDirection;
            lastDirection = startDirection;

            currentCell = startCell;
            this.startCell = startCell;

            this._cellsPerSecond = CalculateSpeedFactor(cellsPerSecond);
            this._defaultCellsPerSecond = this._cellsPerSecond;
            this.level = level;

            this.frameSize = frameSize;

            speedVector = Vector2.Zero;
            lastSpeedVector = Vector2.Zero;

            soundEffects = new Dictionary<string, Cue>();

            this.PowerUpTimer = 0;
            this.controller = controller;
        }

        /// <summary>
        /// Gets or sets the current direction of this object
        /// </summary>
        public abstract Direction Direction { get; set; }

        /// <summary>
        /// Gets the Center of this MO
        /// </summary>
        public override Vector2 Center
        {
            get { return new Vector2(position.X + ((float)frameSize.X / 2), position.Y + ((float)frameSize.Y / 2) ); }
        }

        /// <summary>
        /// Provides an Rectangle to check for intersections
        /// </summary>
        public override Rectangle IntersectRectangle
        {
            get { return new Rectangle((int) position.X, (int) position.Y, (int) Size.X-1, (int) Size.Y-1); }
        }

        /// <summary>
        /// Gets the size of a sprite
        /// </summary>
        public override Vector2 Size
        {
            get { return new Vector2(frameSize.X, frameSize.Y); }
            set { size = value; }
        }

        /// <summary>
        /// Returns the offset from the current cell (how many pixels this object is away from the center of its current cell
        /// </summary>
        public Vector2 Offset
        {
            get { return offset; }
            protected set { this.offset = value; }
        }

        /// <summary>
        /// Gets or sets the current cell
        /// </summary>
        public virtual Cell CurrentCell
        {
            get { return currentCell; }
            set { currentCell = value; }
        }

        /// <summary>
        /// Gets or sets the current CellsPerSecond vector
        /// </summary>
        public Vector2 SpeedVector
        {
            get { return speedVector; }
            set
            {
                lastSpeedVector = speedVector;
                speedVector = value;
            }
        }

        /// <summary>
        /// The name of this object, stored in the controller
        /// </summary>
        public String Name
        {
            get { return controller.Name; }
        }

        /// <summary>
        /// The ID of this object, stored in the controller
        /// </summary>
        public int ID
        {
            get { return controller.ID; }
        }

        /// <summary>
        /// Gets and sets the current speed, in cells per second
        /// </summary>
        public float CellsPerSecond
        {
            get { return _cellsPerSecond; }
            set
            {
                if(_cellsPerSecond == _defaultCellsPerSecond)
                {
                    _cellsPerSecond = value;
                }
                
            }
        }

        /// <summary>
        /// Gets and sets the Poweruptimer
        /// </summary>
        public int PowerUpTimer
        {
            get { return powerUpTimer; }
            set { powerUpTimer = value; }
        }

        /// <summary>
        /// Calculates the offset from the current cell
        /// </summary>
        protected void calculateOffset()
        {
            offset = currentCell.Position - position; 
        }

        /// <summary>
        /// Calculates the positon onscreen, according to the current position and scale
        /// </summary>
        /// <returns>The new position</returns>
        protected Vector2 calculatePosition()
        {
            var newPosition = new Vector2(currentCell.Position.X + offset.X, currentCell.Position.Y + offset.Y);
            return newPosition;
        }

        /// <summary>
        /// Validates if we can move in the desired direction
        /// </summary>
        /// <param name="getDirection">The direction we want to move to</param>
        /// <returns>Either the last direction or the new direction</returns>
        protected bool IsNextDirectionValid(Direction getDirection)
        {
            Point wantedDirection = DirectionExtension.PointFromDirection(getDirection);

            Cell checkCell = level.getCell(currentCell.GridPosition.X + wantedDirection.X,
                                           currentCell.GridPosition.Y + wantedDirection.Y);

            if (checkCell.IsWall)
            {
                return false;
            }

            return true;
            //return PointFromDirection;
        }

        /// <summary>
        /// Sets the position to the one of the current cell
        /// </summary>
        public void RefreshPosition()
        {
            //this.position = CurrentCell.Position + Offset;
            this.position = CurrentCell.Position;
            
        }

        /// <summary>
        /// Sets the current cell to a given target
        /// </summary>
        /// <param name="target">The position of the cell in the grid</param>
        public void SetCurrentCell(Point target)
        {
            this.currentCell = level.getCell(target);
            this.position = CurrentCell.Position;
        }

        /// <summary>
        /// Updates this movable object
        /// </summary>
        /// <param name="gameTime">the current gametime</param>
        public override void Update(IGameTime gameTime)
        {
            if(powerUpTimer > 0)
            {
                powerUpTimer -= gameTime.ElapsedGameTime.Milliseconds;

                if(powerUpTimer <= 0)
                {
                    ResetToDefault();
                }
            }

            offset = CurrentCell.Position - position;

            CurrentCell.CellEffect.ApplyEffect(this);
        }

        /// <summary>
        /// Resets this Movable objects to its original state
        /// </summary>
        public virtual void Reset()
        {
            this.CurrentCell = startCell;
            this.position = startCell.Position;

            this.PowerUpTimer = 0;
            this._cellsPerSecond = _defaultCellsPerSecond;
        }

        /// <summary>
        /// Resets the speedvector to the last known speedvector
        /// </summary>
        public virtual void ResetSpeedVector()
        {
            speedVector = lastSpeedVector;
        }

        /// <summary>
        /// Retruns a Vector 2 according to the current Direction
        /// </summary>
        /// <param name="Direction">The current heading</param>
        /// <param name="currentSpeedVector">The CellsPerSecond vector from the last iteration</param>
        /// <param name="elapsedGameTime">The elapsed game time since the last iteration</param>
        /// <returns>A vector 2</returns>
        protected Vector2 GetSpeedVector(Direction Direction, Vector2 currentSpeedVector, int elapsedGameTime)
        {
            Vector2 newVector = currentSpeedVector;

            switch (Direction)
            {
                case Direction.Up:
                    newVector.Y = -_cellsPerSecond*elapsedGameTime;
                    break;
                case Direction.Down:
                    newVector.Y = _cellsPerSecond*elapsedGameTime;
                    break;
                case Direction.Left:
                    newVector.X = -_cellsPerSecond*elapsedGameTime;
                    break;
                case Direction.Right:
                    newVector.X = _cellsPerSecond*elapsedGameTime;
                    break;
            }

            return newVector;
        }

        /// <summary>
        /// Resets the speed and the poweruptimer to their defaultvalues
        /// </summary>
        protected virtual void ResetToDefault()
        {
            this._cellsPerSecond = _defaultCellsPerSecond;
            this.powerUpTimer = 0;
        }

        /// <summary>
        /// Calculates the speed per millisecond
        /// </summary>
        /// <param name="cells">The speed in cells per second</param>
        /// <returns></returns>
        protected float CalculateSpeedFactor(float cells)
        {
            float f = (50f/1000f)*cells;
            return f;
        }
        
        /// <summary>
        /// Compares two MovableObjects, after the ID
        /// </summary>
        /// <param name="x">The first MovableObject</param>
        /// <param name="y">The second MovableObject</param>
        /// <returns>0 if they are equall, -1 if the first is smaller and 1 if the first is bigger</returns>
        public static int Compare(MovableObject x, MovableObject y)
        {
            if (x.controller.ID> y.controller.ID)
            {
                return 1;
            }
            else if (x.controller.ID < y.controller.ID)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public static int FindAfterID(int id, MovableObject x)
        {
            if (x.ID == id)
            {
                return 1;
            } else
            {
                return -1;
            }
        }

        /// <summary>
        /// Creates a structure with information about his object
        /// </summary>
        /// <returns>A MovObjStruct</returns>
        public abstract MovObjStruct GetStruct();

        /// <summary>
        /// Applys a MovObjStruct to this MovableObject
        /// </summary>
        /// <param name="movObjStruct">the MovObjStruct with the new information for this object</param>
        public void ApplyStruct(MovObjStruct movObjStruct)
        {
            this.Direction = movObjStruct.direction;
            this.SetCurrentCell(movObjStruct.currentCell);
            this.Offset = movObjStruct.offset;
        }
    }
}