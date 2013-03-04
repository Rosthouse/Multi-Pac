using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PacManShared.Enums;

namespace PacManShared.Util.TimeStamps
{
    [Serializable]
    public struct MovObjStruct
    {
        public Point currentCell;
        public Vector2 offset;
        public Direction direction;
        public int ID;
        public MovObjType movObjType;

        public MovObjStruct(Point currentCell, Vector2 offset, Direction direction, int ID, MovObjType movObjType)
        {
            this.currentCell = currentCell;
            this.offset = offset;
            this.direction = direction;
            this.ID = ID;
            this.movObjType = movObjType;
        }


        public static int Compare(MovObjStruct x, MovObjStruct y)
        {
            if(x.ID > y.ID)
            {
                return 1;
            }
            else if(x.ID < y.ID)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    [Serializable]
    public struct LevelStruct
    {
        public bool[,] grid;

        public LevelStruct(bool[,] grid)
        {
            this.grid = grid;
        }
    }

    [Serializable]
    public struct TimeStamp
    {
        public double time;
        public LevelStruct levelStruct;
        public List<MovObjStruct> movableObjects;
        public GameState gameState;

        public TimeStamp(double time, LevelStruct levelStruct, List<MovObjStruct> movObjStructs, GameState gameState)
        {
            this.time = time;
            this.levelStruct = levelStruct;
            this.movableObjects = movObjStructs;
            this.gameState = gameState;
        }
    }

    public struct Command
    {
        public readonly Direction direction;
        public readonly int ID;
        public readonly double time;

        public Command(Direction direction, int ID, double time)
        {
            this.direction = direction;
            this.ID = ID;
            this.time = time;
        }
    }
}