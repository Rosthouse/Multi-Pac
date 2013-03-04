using Microsoft.Xna.Framework;
using PacManShared.Entities.Player;
using PacManShared.Enums;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Controllers.AI.IndividualAI
{
    public class Inky: GhostAi 
    {
        private PacMan pacMan;
        private Ghost blinky;

        public Inky(PacMan pacMan, Ghost blinky, Point scatterTarget, Point exitPoint, Point homeTarget): base(scatterTarget, exitPoint, homeTarget, MovObjType.Inky)
        {
            this.pacMan = pacMan;
            this.blinky = blinky;
        }

        public override string Name
        {
            get { return "Inky"; }
        }

        private Point CalculateCell(Direction direction, Cell pacCell)
        {
            Point lookingDirection = DirectionExtension.PointFromDirection(direction);

            Point sourcePoint = pacCell.GridPosition;

            lookingDirection.X *= 2;
            lookingDirection.Y *= 2;

            return new Point(sourcePoint.X + lookingDirection.X, sourcePoint.Y + lookingDirection.Y);
        }


        protected override Point HuntBehaviour(Cell currentCell)
        {
            Point pivot = CalculateCell(pacMan.Direction, pacMan.CurrentCell);

            Point distanceToBlinky = new Point(pivot.X - blinky.CurrentCell.GridPosition.X,
                                               pivot.Y - blinky.CurrentCell.GridPosition.Y);

            Point target = new Point(pivot.X + distanceToBlinky.X, pivot.Y + distanceToBlinky.Y);

            return target;
        }
    }
}
