using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace PacManNetworkShared
{
    public abstract class IOutgoingMessageHandler
    {
        abstract public void run();

        public void ThreadPoolCallback(Object threadContext)
        {
            this.run();
        }
    }
}
