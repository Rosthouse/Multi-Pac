using System;
using Microsoft.Xna.Framework;
using PacManShared.Entities.Player;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Controllers.AI.IndividualAI
{
    public class Clyde: GhostAi
    {
        private PacMan pacMan;

        public Clyde(PacMan pacMan, Point scatterTarget, Point exitPoint, Point homeTarget): base(scatterTarget, exitPoint, homeTarget, Enums.MovObjType.Clyde) 
        {
            this.pacMan = pacMan;
        }

        public override string Name
        {
            get { return "Clyde"; }
        }

        protected override Point HuntBehaviour(Cell currentCell)
        {
            Point pacManCell = pacMan.CurrentCell.GridPosition;


            double distance = Math.Sqrt(Math.Pow(currentCell.GridPosition.X - pacManCell.X, 2) + Math.Pow(currentCell.GridPosition.Y - pacManCell.Y, 2));


            if (distance >= 8)
            {
                return pacManCell;
            }
            else
            {
                return scatterTarget;
            }
        }
    }
}
