using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacManShared
{
    public interface IGameTime
    {
        TimeSpan TotalGameTime { get; }
        TimeSpan ElapsedGameTime { get; }
        bool IsRunningSlowly { get; }
    }
}
