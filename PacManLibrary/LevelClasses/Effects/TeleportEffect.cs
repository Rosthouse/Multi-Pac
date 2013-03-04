using System;
using Microsoft.Xna.Framework;
using PacManShared.Entities.Player;
using PacManShared.Enums;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.LevelClasses.Effects
{
    class TeleportEffect: ICellEffect
    {
        private Point target;

        private Direction teleportDirection;

        public TeleportEffect(Point target, Direction teleportDirection)
        {
            this.target = target;
            this.teleportDirection = teleportDirection;
        }

        public void ApplyEffect(MovableObject movObj)
        {

            if(!
                ((new Rectangle((int)movObj.Center.X, (int)movObj.Center.Y, 1, 1)).Intersects(movObj.CurrentCell.IntersectRectangle)) && movObj.Direction == teleportDirection)
            {
                movObj.SetCurrentCell(target);
            }
        }
        public void Reset()
        {
            
        }
    }
}
