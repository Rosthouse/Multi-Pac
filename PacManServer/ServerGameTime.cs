using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PacManShared;

namespace PacManServer
{
    [Serializable]
    public class ServerGameTime: IGameTime
    {
        [NonSerialized] private Stopwatch elapsed;
        [NonSerialized] private Stopwatch total;
       
        

        public ServerGameTime()
        {
            total = new Stopwatch();
            elapsed = new Stopwatch();
            this.Start();
        }

        public TimeSpan TotalGameTime
        {
            get
            {
                //return new TimeSpan(total.ElapsedTicks);
                //return new TimeSpan(elapsed.ElapsedTicks);
                return new TimeSpan((int)total.Elapsed.Days , (int)total.Elapsed.Hours, (int)total.Elapsed.Minutes, (int)total.Elapsed.Seconds, (int)total.Elapsed.Milliseconds);
            }
            set { ; }
        }

        public TimeSpan ElapsedGameTime
        {
            get { return elapsed.Elapsed; }
            set { ; }
        }

        public bool IsRunningSlowly
        {
            get { return false; }
            set { ; }
        }

        public void Reset()
        {
            elapsed.Restart();
        }

        private void Start()
        {
            elapsed.Start();
            total.Start();
        }
    }
}
