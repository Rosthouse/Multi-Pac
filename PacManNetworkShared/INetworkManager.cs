using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Lidgren.Network;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace PacManNetworkShared
{
    public abstract class INetworkManager
    {
        protected Thread IncomingMessageHandlerThread;
        protected Queue<NetIncomingMessage> IncomingMessageQueue
            = new Queue<NetIncomingMessage>();
        protected Queue<KeyValuePair<NetConnection, NetOutgoingMessage>> OutgoingMessageQueue
            = new Queue<KeyValuePair<NetConnection,NetOutgoingMessage>>();
        protected IOutgoingMessageHandler omh;
        protected IIncomingMessageHandler imh;
        protected TimeSpan offset;

        abstract public NetOutgoingMessage createMessage();
        abstract public NetSendResult sendMessage(NetOutgoingMessage msg, NetConnection con);
        abstract public NetSendResult sendMessage(NetOutgoingMessage msg);
        abstract public NetSendResult sendMessage(string msg);
        abstract public NetSendResult sendMessage(string msg, NetConnection con);
        abstract public NetSendResult sendMessage(Object o);
        abstract public NetSendResult sendMessage(Object o, NetConnection con);
        abstract public int getUID();

        public TimeSpan Offset
        {
            get { return this.offset; }
            set { this.offset = value; }
        }

        public NetIncomingMessage getMessage()
        {
            if (IncomingMessageQueue.Count > 0)
            {
                return IncomingMessageQueue.Dequeue();
            }
            return null;
        }

        /// <summary>
        /// Serialisiert ein übergebenes Objekt
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public byte[] Serialize(object o)
        {
            BinaryFormatter bin = new BinaryFormatter();
            MemoryStream mem = new MemoryStream();
            bin.Serialize(mem, o);
            return mem.GetBuffer();
        }

        /// <summary>
        /// Der Inhalt der Nachricht, welcher ein Objekt darstellt
        /// (ganze Nachricht mit Offset der Länge der Bezeichnung z.B. "newDirection"),
        /// wird wieder in ein Objekt deserialisiert. Die Rückgabe muss anschliessend
        /// gecastet werden. Bsp. (Direction)networkManager.Deserialize(msg,msgString)
        /// </summary>
        /// <param name="msg">Eingegangene Nachricht (von IncomingMessageQueue)</param>
        /// <param name="msgString">msg.ReadString()</param>
        /// <returns>Object</returns>
        public object Deserialize(NetIncomingMessage msg, string msgString)
        {
            int offset = msgString.ToArray().Length + 1;
            BinaryFormatter bin = new BinaryFormatter();
            MemoryStream mem = new MemoryStream();
            byte[] dataBuffer = new byte[msg.LengthBytes];
            msg.ReadBytes(dataBuffer, offset, msg.LengthBytes - offset);
            mem.Write(dataBuffer, 0, dataBuffer.Length);
            mem.Seek(offset, 0);
            return bin.Deserialize(mem);
        }

        public object Deserialize(byte[] a)
        {
            //int offset = msgString.ToArray().Length + 1;
            BinaryFormatter bin = new BinaryFormatter();
            MemoryStream mem = new MemoryStream();
            //byte[] dataBuffer = new byte[a.Length];
            //msg.ReadBytes(dataBuffer, offset, msg.LengthBytes - offset);
            mem.Write(a, 0, a.Length);
            mem.Seek(0,0);
            return bin.Deserialize(mem);
        }
    }
}
