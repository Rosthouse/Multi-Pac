using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PacManShared.Entities.Player;
using PacManShared.Enums;

namespace PacManShared.GameplayBehaviour
{
    /// <summary>
    /// The normal playbehaviour
    /// </summary>
    public class PlayBehaviour: IGamePlayBehaviour
    {
        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="gameTime">the current gametime</param>
        /// <param name="gameStateManager">a reference to the gamestateManager</param>
        public void Update(IGameTime gameTime, GameStateManager gameStateManager)
        {
            gameStateManager.Level.Update(gameTime);

            foreach (MovableObject movableObject in gameStateManager.MovableObjects)
            {
                movableObject.Update(gameTime);
                if (movableObject is PacMan)
                {
                    foreach (MovableObject ghost in gameStateManager.MovableObjects)
                    {
                        if (ghost is Ghost && ghost.CurrentCell.GridPosition == movableObject.CurrentCell.GridPosition)
                        {
                            gameStateManager.GameState = GameState.Death;
                        }
                    }
                } 
            }

            
        }
    }
}
