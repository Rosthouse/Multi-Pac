using Microsoft.Xna.Framework;
using PacManShared.Entities.Player;
using PacManShared.Enums;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Controllers.AI.IndividualAI
{
    public class Pinky: GhostAi
    {
        private PacMan pacMan;

        public Pinky(PacMan pacMan, Point scatterTarget, Point exitTarget, Point homeTarget): base(scatterTarget, exitTarget, homeTarget, Enums.MovObjType.Clyde)
        {
            this.pacMan = pacMan;
        }

        public override string Name
        {
            get { return "Pinky"; }
        }


        protected override  Point HuntBehaviour(Cell currentCell)
        {
            Point lookingDirection = DirectionExtension.PointFromDirection(pacMan.Direction);

            Point sourcePoint = pacMan.CurrentCell.GridPosition;

            lookingDirection.X *= 4;
            lookingDirection.Y *= 4;

            return new Point(sourcePoint.X + lookingDirection.X, sourcePoint.Y + lookingDirection.Y);
        }
    }
}
