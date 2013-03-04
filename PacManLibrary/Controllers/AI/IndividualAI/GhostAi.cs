using System;
using Microsoft.Xna.Framework;
using PacManShared.Enums;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Controllers.AI.IndividualAI
{
    public abstract class GhostAi: IGhostStateObserver
    {
        protected EGhostBehaviour ghostBehaviour;
        private Random rand = new Random();
        protected Point scatterTarget;
        protected Point exitTarget;
        protected Point homeTarget;
        private MovObjType movObjType;

        public abstract String Name { get; }

        public MovObjType MovObjType
        {
            get { return movObjType; }
        }

        public GhostAi(Point scatterTarget, Point exitTarget, Point homeTarget, MovObjType movObjType)
        {
            this.scatterTarget = scatterTarget;
            this.exitTarget = exitTarget;
            this.homeTarget = homeTarget;
            this.movObjType = movObjType;
        }

        public Point TargetCell(Cell currentCell, EGhostBehaviour behaviour)
        {
            switch (behaviour)
            {
                case EGhostBehaviour.Fright:
                    return FrightBehaviour(currentCell);
                case EGhostBehaviour.Hunt:
                    return HuntBehaviour(currentCell);
                case EGhostBehaviour.Scatter:
                    return scatterTarget;
                case EGhostBehaviour.Exiting:
                    return exitTarget;
                case EGhostBehaviour.Dead:
                case EGhostBehaviour.InHouse:
                    return homeTarget;
                default:
                    return currentCell.GridPosition;
            }   
        }

        protected abstract Point HuntBehaviour(Cell currentCell);
        
        protected Point FrightBehaviour(Cell curCell)
        {
            return DirectionExtension.PointFromDirection((Direction) rand.Next(1, 4));
        }

        public void SetGhostState(EGhostBehaviour eGhostStateBehaviour)
        {
            this.ghostBehaviour = eGhostStateBehaviour;
        }
    }
}