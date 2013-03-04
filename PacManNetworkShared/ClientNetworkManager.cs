using System.Collections.Generic;
using System.Threading;
using Lidgren.Network;
using System.Collections;
using System;

namespace PacManNetworkShared
{
    public class ClientNetworkManager : INetworkManager
    {
        private NetClient netClient;

        public ClientNetworkManager(NetClient netClient)
        {
            this.netClient = netClient;
            imh = new ClientIncomingMessageHandler(netClient, IncomingMessageQueue);
            omh = new ClientOutgoingMessageHandler(netClient, OutgoingMessageQueue);
            IncomingMessageHandlerThread = new Thread(imh.run);
            IncomingMessageHandlerThread.Start();
        }

        public override NetOutgoingMessage createMessage()
        {
            return netClient.CreateMessage();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int getUID()
        {
            return Convert.ToInt32(netClient.UniqueIdentifier.ToString().Substring(netClient.UniqueIdentifier.ToString().Length - 5));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public override NetSendResult sendMessage(NetOutgoingMessage msg)
        {
            try
            {
                KeyValuePair<NetConnection, NetOutgoingMessage> combinedMsg = new KeyValuePair<NetConnection, NetOutgoingMessage>(netClient.Connections[0], msg);
                OutgoingMessageQueue.Enqueue(combinedMsg);
                ThreadPool.QueueUserWorkItem(omh.ThreadPoolCallback);
                return NetSendResult.Sent;
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("Client send thread error");
                return NetSendResult.Failed;
            }
        }

        public override NetSendResult sendMessage(string content)
        {
            NetOutgoingMessage msg = this.createMessage();
            msg.Write(content);
            return sendMessage(msg);
        }

        public override NetSendResult sendMessage(NetOutgoingMessage msg, NetConnection con)
        {
            throw new NotImplementedException();
        }

        public override NetSendResult sendMessage(string msg, NetConnection con)
        {
            throw new NotImplementedException();
        }

        public override NetSendResult sendMessage(object o)
        {
            NetOutgoingMessage msg = createMessage();
            switch (o.GetType().Name)
            {
                case "TimeStamp":
                    msg.Write(o.GetType().Name);
                    msg.Write(Serialize(o));
                    break;
            }

            if (msg != null)
            {
                return sendMessage(msg);
            }
            return NetSendResult.Failed;
            //throw new NotImplementedException();
        }

        public override NetSendResult sendMessage(object o, NetConnection con)
        {
            throw new NotImplementedException();
        }
    }
}
