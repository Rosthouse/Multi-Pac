using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PacManShared.Entities.Player;

namespace PacManShared.Util.TimeStamps
{
    public class TimeStampEventArgs:EventArgs
    {
        private TimeSpan time;
        private MovableObject sender;
    
        TimeStampEventArgs(TimeSpan time, MovableObject sender)
        {
            this.time = time;
            this.sender = sender;
        }
    }
}
