using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PacManShared.GameplayBehaviour
{
    interface IGamePlayBehaviour
    {
        void Update(IGameTime gameTime, GameStateManager gameStateManager);
    }
}
