using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PacManShared.Simulation;
using PacManShared.Util.TimeStamps;

namespace PacManServer.Validation
{
    class MovementComparer: IComparer<Command>
    {
        public  int Compare(Command x, Command y)
        {
            if (x.time > y.time)
            {
                return 1;
            }
            else if (x.time < y.time)
            {
                return -1;
            }

            return 0;
        }
    }
}
