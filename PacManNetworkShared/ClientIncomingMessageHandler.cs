using System.Collections.Generic;
using Lidgren.Network;
using System.Threading;

namespace PacManNetworkShared
{
    class ClientIncomingMessageHandler : IIncomingMessageHandler
    {
        private NetClient netClient;
        private Queue<NetIncomingMessage> IncomingMessageQueue;
        private bool isRunning;
        

        public ClientIncomingMessageHandler(NetClient netClient, Queue<NetIncomingMessage> IncomingMessageQueue)
        {
            this.netClient = netClient;
            this.IncomingMessageQueue = IncomingMessageQueue;
            this.isRunning = true;
        }

        public override void run()
        {
            while (isRunning)
            {
                NetIncomingMessage msg;
                while ((msg = netClient.ReadMessage()) != null)
                {
                    lock (IncomingMessageQueue)
                    {
                        IncomingMessageQueue.Enqueue(msg);
                    }
                    /*switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            lock (IncomingMessageQueue)
                            {
                                IncomingMessageQueue.Enqueue(msg);
                            }
                            break;
                    }*/
                }
                Thread.Sleep(50);
            }
        }
    }
}
