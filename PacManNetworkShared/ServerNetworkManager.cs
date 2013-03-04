using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Lidgren.Network;

namespace PacManNetworkShared
{
    public class ServerNetworkManager : INetworkManager
    {
        private NetServer netServer;

        public ServerNetworkManager(NetServer netServer)
        {
            this.netServer = netServer;
            imh = new ServerIncomingMessageHandler(netServer, IncomingMessageQueue);
            omh = new ServerOutgoingMessageHandler(netServer, OutgoingMessageQueue);
            IncomingMessageHandlerThread = new Thread(imh.run);
            IncomingMessageHandlerThread.Start();
        }

        public override NetOutgoingMessage createMessage()
        {
            return netServer.CreateMessage();
        }

        public override NetSendResult sendMessage(NetOutgoingMessage msg, NetConnection con)
        {
            try
            {
                KeyValuePair<NetConnection, NetOutgoingMessage> combinedMsg = new KeyValuePair<NetConnection, NetOutgoingMessage>(con, msg);
                OutgoingMessageQueue.Enqueue(combinedMsg);
                ThreadPool.QueueUserWorkItem(omh.ThreadPoolCallback);
                return NetSendResult.Sent;
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("Thread Error");
                return NetSendResult.Failed;
            }
        }

        public override NetSendResult sendMessage(string content, NetConnection con)
        {
            NetOutgoingMessage msg = netServer.CreateMessage();
            msg.Write(content);
            return sendMessage(msg,con);
        }

        public override NetSendResult sendMessage(NetOutgoingMessage msg)
        {
            throw new NotImplementedException();
        }

        public override NetSendResult sendMessage(string msg)
        {
            throw new NotImplementedException();
        }

        public override int getUID()
        {
            throw new NotImplementedException();
        }

        public override NetSendResult sendMessage(object o)
        {
            throw new NotImplementedException();
        }

        public override NetSendResult sendMessage(object o, NetConnection con)
        {
            NetOutgoingMessage msg = createMessage();
            switch (o.GetType().Name)
            {
                case "TimeStamp":
                    msg.Write(o.GetType().Name);
                    byte[] a = Serialize(o);
                    Object b = Deserialize(a);
                    msg.Write(a);
                    break;
            }

            if (msg != null)
            {
                return sendMessage(msg,con);
            }
            return NetSendResult.Failed;
        }
    }
}
