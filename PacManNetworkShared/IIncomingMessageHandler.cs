using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacManNetworkShared
{
    public abstract class IIncomingMessageHandler
    {
        abstract public void run();
    }
}
