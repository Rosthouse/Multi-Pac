using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PacManShared;

namespace PacManServer
{
    class SimulationGameTime: IGameTime 
    {
        private TimeSpan elapsedGameTime;
        private TimeSpan totalGameTime;
        private int simulationTime;

        public SimulationGameTime(TimeSpan totalGameTime, int simulationTime)
        {
            this.totalGameTime = totalGameTime;
        }

        public TimeSpan TotalGameTime
        {
            get { return totalGameTime; }
        }

        public TimeSpan ElapsedGameTime
        {
            get { return elapsedGameTime; }
        }

        public bool IsRunningSlowly
        {
            get { return true; }
        }

        public int SimulationTime
        {
            get { return simulationTime; }
            set { simulationTime = value; }
        }

        public void SetElapsedMilliseconds(int milliseconds)
        {
            elapsedGameTime = new TimeSpan(0, 0, 0, 0, milliseconds);
            totalGameTime += elapsedGameTime;
        }
    }
}
