using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PacManShared.Controllers.AI.IndividualAI;
using PacManShared.Enums;
using PacManShared.LevelClasses;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Controllers.AI
{
    public class GhostController: Controller, IGhostStateObserver
    {
        private readonly Level level;
        private Direction currentDirection;

        private GhostAi ghostAi;

        private Direction lastDirection;
        private Direction nextDirection;

        private Cell currentCell;
        private Cell lastCell;


        private List<Cell> possibleMoves;

        public GhostController(Level level, GhostAi ghostAi, int id): base(id)
        {
            this.level = level;
            Name = ghostAi.Name;
            this.ghostAi = ghostAi;

            currentCell = Cell.Empty;
            lastCell = Cell.Empty;

            possibleMoves = new List<Cell>();
        }

        #region Controller Members

        public override Direction Direction
        {
            get
            {
                return currentDirection;
            }
            set 
            { 
                lastDirection = currentDirection;
                currentDirection = value;
            }
        }

        public Cell CurrentCell
        {
            get { return currentCell; }
            set
            {
                if(value.GridPosition != currentCell.GridPosition)
                {
                    lastCell = currentCell;
                    currentCell = value;
                }
            }
        }

        public Direction NextDirection
        {
            get { return nextDirection; }
            set
            {
                Direction = nextDirection;
                nextDirection = value;
            }
        }

        public override string Name
        {
            get { return ghostAi.Name; }
            set { ; }
        }


        public override MovObjType MovObjType
        {
            get { return ghostAi.MovObjType; }
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public override void Update(Cell CurrentCell)
        {
            this.Update(CurrentCell, EGhostBehaviour.Hunt);
        }

        #endregion

        public void Update(Cell currentCell, EGhostBehaviour behaviour)
        {
            CurrentCell = currentCell;
            
            for (int i = 1; i <= 4; i++)
            {
                AddCell((Direction)i, CurrentCell.GridPosition);
            }



            for (int i = 0; i < possibleMoves.Count; i++)
            {
                Cell possibleMove = possibleMoves[i];


                Point oppsitDirection = DirectionExtension.PointFromDirection(DirectionExtension.GetOppositeDirection(lastDirection));
                Cell oppositeCell = level.getCell(CurrentCell.GridPosition.X + oppsitDirection.X,
                                                  CurrentCell.GridPosition.Y + oppsitDirection.Y);

                if(possibleMove == null)
                {
                    possibleMoves.Remove(possibleMove);
                    continue;
                }

                if (oppositeCell.GridPosition == possibleMove.GridPosition || possibleMove.GridPosition == lastCell.GridPosition)
                {
                    possibleMoves.Remove(possibleMove);
                    continue;
                }
            }

            if (possibleMoves.Count > 1)
            {
                Cell wantedCell = level.getCell(ghostAi.TargetCell(CurrentCell, behaviour));

                FindShortestPath(CurrentCell, wantedCell, ref possibleMoves);
            } 

            if(possibleMoves.Count >0)
            {
                Cell nextCell = possibleMoves[0];
                Point nextMovement = new Point(nextCell.GridPosition.X - CurrentCell.GridPosition.X,
                                                nextCell.GridPosition.Y - CurrentCell.GridPosition.Y);
                Direction = DirectionExtension.DirectionFromPoint(nextMovement);
            }

            
                    
            possibleMoves.Clear();
        
        }

        private void AddCell(Direction direction, Point currentPosition)
        {
            Point directionPoint = DirectionExtension.PointFromDirection(direction);

            Point getCell = new Point(currentPosition.X + directionPoint.X,
                                          currentPosition.Y + directionPoint.Y);

            if (!(getCell.X < 0 || getCell.X >= level.Size.X || getCell.Y < 0 || getCell.Y >= level.Size.Y))
            {

                Cell possibleCell = level.getCell(currentPosition.X + directionPoint.X, currentPosition.Y + directionPoint.Y);
                if (!possibleCell.IsWall)
                {
                    possibleMoves.Add(level.getCell(currentPosition.X + directionPoint.X, currentPosition.Y + directionPoint.Y));
                }
            }
            
            
        }

        private void FindShortestPath(Cell currentCell, Cell targetCell, ref List<Cell> cells)
        {
            double shortestDistance = float.MaxValue;
            double distance = float.MaxValue;

            for(int i=0; i<cells.Count; i++)
            {
                distance = Math.Sqrt(Math.Pow(cells[i].GridPosition.X - targetCell.GridPosition.X, 2) + Math.Pow(cells[i].GridPosition.Y - targetCell.GridPosition.Y, 2));
                
                if(distance <= shortestDistance)
                {
                    shortestDistance = distance;
                    Cell swapper = cells[i];
                    cells.Remove(cells[i]);
                    cells.Insert(0, swapper);
                }
                else
                {
                    cells.RemoveAt(i);
                }
            }
        }

        public void SetGhostState(EGhostBehaviour eGhostStateBehaviour)
        {
            ghostAi.SetGhostState(eGhostStateBehaviour);
        }
    }
}