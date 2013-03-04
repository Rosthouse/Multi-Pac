using System;
using PacManShared.LevelClasses.Cells;
using PacManShared.Entities.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PacManShared.LevelClasses.Effects
{
    class CellHideEffect : ICellEffect
    {
        private Cell actualCell;

        public CellHideEffect(ref Cell actualCell)
        {
            this.actualCell = actualCell;
        }

        public void ApplyEffect(MovableObject movObj)
        {
            if(movObj is PacMan) 
            {
                if (this.actualCell.PickUp.IsActive == true)
                {
                    movObj.score += actualCell.PickUp.Score;
                }
                
                this.actualCell.PickUp.IsActive = false;   
            }
        }

    }
}
