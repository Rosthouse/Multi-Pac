using System;
using Microsoft.Xna.Framework;

namespace PacManShared.Enums
{
    public enum Direction
    {
        None = 0,
        Up = 1,
        Left = 2,
        Down = 3,
        Right = 4
    }

    public static class DirectionExtension
    {
        public static Point PointFromDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Point(0, -1);
                case Direction.Down:
                    return new Point(0, 1);
                case Direction.Left:
                    return new Point(-1, 0);
                case Direction.Right:
                    return new Point(1, 0);
                case Direction.None:
                default:
                    return Point.Zero;
            }
        }

        public static float DirectionToAngle(Direction direction)
        {
            switch(direction)
            {
                case Direction.Down:
                    return MathHelper.ToRadians(180);
                case Direction.Left:
                    return MathHelper.ToRadians(270);
                case Direction.Right:
                    return MathHelper.ToRadians(90);
                default:
                    return MathHelper.ToRadians(0);
            }
        }

        public static Direction DirectionFromPoint(Point point)
        {
            if (point.X == 1)
            {
                return Direction.Right;
            }
            else if (point.X == -1)
            {
                return Direction.Left;
            }
            else if (point.Y == 1)
            {
                return Direction.Down;
            }
            else if (point.Y == -1)
            {
                return Direction.Up;
            }
            else
            {
                return Direction.None;
            }
        }

        public static Direction GetOppositeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    return Direction.None;
            }
        }

        public static Direction StringToDirection(String directionString)
        {
            String checkString = directionString.Trim();

            if(checkString.StartsWith("Up"))
            {
                return Direction.Up;
            }else if(checkString.StartsWith("Down"))
            {
                return Direction.Down;
            } else if(checkString.StartsWith("Left"))
            {
                return Direction.Left;  
            } else if (checkString.StartsWith("Right"))
            {
                return Direction.Right;
            } else
            {
                return Direction.None;
            }
        }
    }
}