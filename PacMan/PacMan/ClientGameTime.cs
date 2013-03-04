using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PacManShared;

namespace PacManClient
{
    public class ClientGameTime: IGameTime
    {
        private GameTime gameTime;

        public ClientGameTime(GameTime gameTime)
        {
            this.gameTime = gameTime;
        }

        public TimeSpan TotalGameTime
        {
            get { return gameTime.TotalGameTime; }
        }

        public TimeSpan ElapsedGameTime
        {
            get { return gameTime.ElapsedGameTime; }
        }

        public bool IsRunningSlowly
        {
            get { return gameTime.IsRunningSlowly; }
        }
    }
}
