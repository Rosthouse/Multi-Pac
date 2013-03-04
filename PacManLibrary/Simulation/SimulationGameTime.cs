using System;

namespace PacManShared.Simulation
{
    public class SimulationGameTime: IGameTime 
    {
        private TimeSpan elapsedGameTime;
        private TimeSpan totalGameTime;
        private double simulationTime;

        public SimulationGameTime(TimeSpan totalGameTime, double simulationTime)
        {
            this.totalGameTime = totalGameTime;
            this.simulationTime = simulationTime;
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

        public double SimulationTime
        {
            get { return simulationTime; }
            set { simulationTime = value; }
        }

        public void SetElapsedMilliseconds(double milliseconds)
        {
            totalGameTime += elapsedGameTime;
            elapsedGameTime = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(milliseconds));
        }
    }
}
