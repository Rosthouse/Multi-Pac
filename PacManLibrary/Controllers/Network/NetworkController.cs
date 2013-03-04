using System;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using PacManNetworkShared;
using PacManShared.Enums;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Controllers.Network
{
    public class NetworkController : ControllerDecorator, INetworkController
    {
        private double timestamp;
        private INetworkManager networkManager;

        /// <summary>
        /// Constructs a new Networkcontroller
        /// </summary>
        /// <param name="direction">the startingdirection</param>
        /// <param name="name">The name of this controller</param>
        /// <param name="index">the playerindex</param>
        /// <param name="movObjType">The type of this networkcontroller</param>
        /// <param name="inputState">The inputstate for this controller</param>
        /// <param name="client">The network client</param>
        public NetworkController(int id, Controller controller, INetworkManager networkManager) : base(controller, id)
        {
            //TODO: Make network controller independent from playercontroller. A networkcontroller has to stand up for himselv
            this.networkManager = networkManager;
        }

        /// <summary>
        /// Gets or sets the current direction
        /// </summary>
        public override Direction Direction
        {
            get { return this.controller.Direction; }
            set
            {
//                if (direction != value)
//                {
//                    lastDirection = direction;
//                    direction = value;
//                } 
                this.controller.Direction = value;
            }
        }

        public override string Name
        {
            get { return controller.Name; }
            set { controller.Name = value; }
        }

        public override MovObjType MovObjType
        {
            get { return controller.MovObjType; }
        }

        public override void Update(Cell currentCell)
        {
            Direction lastDirection = controller.Direction;

            controller.Update(currentCell);

            if(Direction != lastDirection)
            {
                Send();
            }
        }

        /// <summary>
        /// updates this controller
        /// </summary>
        /// <param name="gameTime">the current gametime</param>
        [Obsolete("Use Update(Cell currentCell)")]
        public void Update(IGameTime gameTime)
        {
            timestamp = gameTime.TotalGameTime.TotalMilliseconds;
            System.Console.WriteLine(timestamp);

            //controller.Update();

            /*if (this.MovObjType == MovObjType.LocalPacMan)
            {

                KeyboardState state = Keyboard.GetState();

                if (inputState.IsNewKeyPress(Keys.Up, this.index, out this.index))
                {
                    Direction = Direction.Up;
                    Send();
                }
                else if (inputState.IsNewKeyPress(Keys.Down, this.index, out this.index))
                {
                    Direction = Direction.Down;
                    Send();
                }
                else if (inputState.IsNewKeyPress(Keys.Right, this.index, out this.index))
                {
                    Direction = Direction.Right;
                    Send();
                }
                else if (inputState.IsNewKeyPress(Keys.Left, this.index, out this.index))
                {
                    Direction = Direction.Left;
                    Send();
                }
            }
            else
            {
                Receive();
            }
            */

            //this.Update();

        }

        /// <summary>
        /// Sends a message over the network
        /// </summary>
        public void Send()
        {
            NetOutgoingMessage msg = networkManager.createMessage();
            msg.Write("newDirection");
            msg.Write(Convert.ToInt32(Direction));
            msg.Write(timestamp + networkManager.Offset.TotalMilliseconds);
            networkManager.sendMessage(msg);
            //client.SendMessage(msg, NetDeliveryMethod.Unreliable);
            /*msg = networkManager.createMessage();
            msg.Write("newObject");
            msg.Write(networkManager.Serialize(Direction));
            networkManager.sendMessage(msg);*/
        }

        /// <summary>
        /// Handles receiving of messages
        /// </summary>
        public void Receive()
        {
           throw new NotImplementedException();            
        }
    }
}
