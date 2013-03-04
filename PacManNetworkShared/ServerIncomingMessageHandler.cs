using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Threading;

namespace PacManNetworkShared
{
    class ServerIncomingMessageHandler : IIncomingMessageHandler
    {
        private NetServer netServer;
        private Queue<NetIncomingMessage> IncomingMessageQueue;
        private bool isRunning;
        

        public ServerIncomingMessageHandler(NetServer netServer, Queue<NetIncomingMessage> IncomingMessageQueue)
        {
            this.netServer = netServer;
            this.IncomingMessageQueue = IncomingMessageQueue;
            this.isRunning = true;
        }

        public override void run()
        {
            while (isRunning)
            {
                NetIncomingMessage msg;
                while ((msg = netServer.ReadMessage()) != null)
                {
                    lock (IncomingMessageQueue)
                    {
                        IncomingMessageQueue.Enqueue(msg);
                    }
                    break;
                    /*switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            lock (IncomingMessageQueue)
                            {
                                IncomingMessageQueue.Enqueue(msg);
                            }
                            break;

                        case NetIncomingMessageType.DiscoveryRequest:
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
