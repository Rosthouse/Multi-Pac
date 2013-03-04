using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PacManClient.Components;
using PacManShared.Enums;
using PacManShared.LevelClasses.Cells;

namespace PacManClient.Controller.Local
{
    /// <summary>
    /// Controls a movable object with user input
    /// </summary>
    public class PlayerController : PacManShared.Controllers.Controller
    {
        protected InputState inputState;
        protected PlayerIndex index;
        protected Direction direction;
        protected Direction lastDirection;
        private MovObjType movObjType;
        int id;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="direction">the starting direction</param>
        /// <param name="name">The name of this controller</param>
        /// <param name="index">The playerindex of this controller</param>
        /// <param name="movObjType">What type this controller controlls</param>
        /// <param name="baseController"></param>
        /// <param name="inputState">The input from inputdevices</param>
        public PlayerController(Direction direction, string name, int id, PlayerIndex index, MovObjType movObjType, InputState inputState): base(id)
        {
            this.index = index;
            this.direction = direction;
            lastDirection = direction;
            this.Name = name;
            this.movObjType = movObjType;
            this.inputState = inputState;

            
        }

        #region Controller Members

        /// <summary>
        /// Gets or sets the direction
        /// </summary>
        public override Direction Direction
        {
            get { return direction; }
            set
            {
                if (direction != value)
                {
                    lastDirection = direction;
                    direction = value;
                }
            }
            
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public override string Name { get; set; }


        /// <summary>
        /// Gets the type of the movable object this controller controlls
        /// </summary>
        public override MovObjType MovObjType
        {
            get { return movObjType; }
        }


        /// <summary>
        /// Updates this controller
        /// </summary>
        public override void Update(Cell CurrentCell)
        {
            KeyboardState state = Keyboard.GetState();

            if (inputState.IsNewKeyPress(Keys.Up, this.index, out this.index))
            {
                Direction = Direction.Up;
            }
            else if (inputState.IsNewKeyPress(Keys.Down, this.index, out this.index))
            {
                Direction = Direction.Down;
            }
            else if (inputState.IsNewKeyPress(Keys.Right, this.index, out this.index))
            {
                Direction = Direction.Right;
            }
            else if (inputState.IsNewKeyPress(Keys.Left, this.index, out this.index))
            {
                Direction = Direction.Left;
            }
        }


        #endregion


    }
}
