using System;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using PacManNetworkShared;
using PacManShared.Controllers.AI;
using PacManShared.Enums;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Controllers.Network
{
    public class NetworkGhostController: ControllerDecorator,  IGhostStateObserver, INetworkController
    {
        private INetworkManager networkManager;

        public NetworkGhostController(INetworkManager networkManager, Controller controller, int id) : base(controller, id)
        {
            this.networkManager = networkManager;
        }

        public override Direction Direction
        {
            get { return controller.Direction; }
            set { controller.Direction = value; }
        }

        public override string Name
        {
            get { return controller.Name; }
            set { controller.Name = Name; }
        }

        public override MovObjType MovObjType
        {
            get { return controller.MovObjType; }
        }

        public override void Update(Cell currentCell)
        {
            controller.Update(currentCell);
        }

        public void SetGhostState(EGhostBehaviour eGhostStateBehaviour)
        {
            IGhostStateObserver ghostStateObserver = controller as IGhostStateObserver;
            if(ghostStateObserver != null)
            {
                ghostStateObserver.SetGhostState(eGhostStateBehaviour);
            }
        }

        public void Send()
        {

            NetOutgoingMessage msg = networkManager.createMessage();
            msg.Write("newDirection");
            msg.Write(Convert.ToInt32(Direction));
            //TODO: implement the timestamp correctly
            msg.Write(45);
            networkManager.sendMessage(msg);
            //client.SendMessage(msg, NetDeliveryMethod.Unreliable);
        }

        public void Receive()
        {
            NetIncomingMessage msg = networkManager.getMessage();
            if (msg != null)
            {
                string msgStr = msg.ReadString();
                string[] values = msgStr.Split(';');

                int msgUID = Convert.ToInt32(values[1]);
                int direction = Convert.ToInt32(values[2]);

                if (values[0] == "updateDirection" && ID == msgUID)
                {
                    Direction = (Direction)direction;
                }
            }
            
        }

        public void Update(IGameTime gameTime)
        {
            //throw new NotImplementedException();
        }
    }
}
