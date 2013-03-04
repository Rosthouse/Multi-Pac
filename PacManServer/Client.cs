using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace PacManServer
{
    class Client
    {
        private NetConnection connection;
        private int figureType;

        public Client(NetConnection con)
        {
            this.connection = con;
        }

        public void setConnection(NetConnection con)
        {
            this.connection = con;
        }

        public NetConnection getConnection()
        {
            return this.connection;
        }

        public int getUID()
        {
            return Convert.ToInt32(this.connection.RemoteUniqueIdentifier.ToString().Substring(this.connection.RemoteUniqueIdentifier.ToString().Length-5));
        }

        public void setFigureType(int type)
        {
            this.figureType = type;
        }

        public int getFigureType()
        {
            return this.figureType;
        }
    }
}
