using System;
using Microsoft.Xna.Framework;
using PacManShared.Entities.Player;
using PacManShared.Enums;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Controllers.AI.IndividualAI
{
    public class Blinky: GhostAi
    {
        private const String name = "Blinky";
        private PacMan pacMan;

        /// <summary>
        /// Creates a new blinky AI
        /// </summary>
        /// <param name="pacMan">A pacman</param>
        /// <param name="scatterTarget">the cell this ghost targets in scatter mode</param>
        /// <param name="exitPoint">the cell where this ghost leaves the house</param>
        /// <param name="homeTarget">the cell where this ghost has his home</param>
        public Blinky(PacMan pacMan, Point scatterTarget, Point exitPoint, Point homeTarget): base(scatterTarget, exitPoint, homeTarget, MovObjType.Blinky)
        {
            this.pacMan = pacMan;
            this.ghostBehaviour = EGhostBehaviour.Exiting;
        }

        /// <summary>
        /// Gets the name of this ghost
        /// </summary>
        public override string Name
        {
            get { return name; }
        }

        /// <summary>
        /// returns the targeted cell for this AI
        /// </summary>
        /// <returns></returns>
        public Cell TargetCell()
        {
            return new Cell();   
        }

        /// <summary>
        /// Returns a target Blinky wants to go to
        /// </summary>
        /// <param name="currentCell">The current cell</param>
        /// <returns>The target this AI wants to go to</returns>
        protected override Point HuntBehaviour(Cell currentCell)
        {
            return pacMan.CurrentCell.GridPosition;
        }
    }
}
