using System.Collections.Generic;
using Lidgren.Network;
using System;

namespace PacManNetworkShared
{
    class ClientOutgoingMessageHandler : IOutgoingMessageHandler
    {
        private NetClient netClient;
        private Queue<KeyValuePair<NetConnection, NetOutgoingMessage>> OutgoingMessageQueue;
        //private bool isRunning;

        public ClientOutgoingMessageHandler(NetClient netClient, Queue<KeyValuePair<NetConnection, NetOutgoingMessage>> OutgoingMessageQueue)
        {
            this.netClient = netClient;
            this.OutgoingMessageQueue = OutgoingMessageQueue;
            //this.isRunning = true;
        }

        public override void run()
        {

            //while (isRunning)
            //{
            KeyValuePair<NetConnection, NetOutgoingMessage> msg;
            if (OutgoingMessageQueue.Count > 0)
            {
                lock (OutgoingMessageQueue)
                {
                    msg = OutgoingMessageQueue.Dequeue();
                }

                netClient.SendMessage(msg.Value, msg.Key, NetDeliveryMethod.ReliableOrdered);
            }
            //}
        }
    }
}
