using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PacManShared.Entities.Player;
using PacManShared.Enums;

namespace PacManShared.GameplayBehaviour
{
    public class DeathBehaviour: IGamePlayBehaviour
    {
        public void Update(IGameTime gameTime, GameStateManager gameStateManager)
        {
            foreach (MovableObject movObj in gameStateManager.MovableObjects)
            {
                movObj.Reset();
            }

            //level.Reset();

            gameStateManager.Level.Reset();

            gameStateManager.GameState = GameState.Playing;
        }
    }
}
