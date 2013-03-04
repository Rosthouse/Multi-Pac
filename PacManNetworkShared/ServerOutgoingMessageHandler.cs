using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace PacManNetworkShared
{
    class ServerOutgoingMessageHandler : IOutgoingMessageHandler
    {
        private NetServer netServer;
        private Queue<KeyValuePair<NetConnection, NetOutgoingMessage>> OutgoingMessageQueue;

        public ServerOutgoingMessageHandler(NetServer netServer, Queue<KeyValuePair<NetConnection, NetOutgoingMessage>> OutgoingMessageQueue)
        {
            this.netServer = netServer;
            this.OutgoingMessageQueue = OutgoingMessageQueue;
        }

        public override void run()
        {
            KeyValuePair<NetConnection, NetOutgoingMessage> msg;
            if (OutgoingMessageQueue.Count > 0)
            {
                lock (OutgoingMessageQueue)
                {
                    msg = OutgoingMessageQueue.Dequeue();
                }

                netServer.SendMessage(msg.Value, msg.Key, NetDeliveryMethod.ReliableOrdered);
            }
        }
    }
}
