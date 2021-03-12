using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    public enum Direction { Up, Down, Left, Right }
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point() { }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public class Snake
    {
        public Point HeadPos { get; set;  } = new Point();        
        public List<Point> OccupiedCells { get; set; } = new List<Point>();
        public List<Point> ErasableCells { get; set; } = new List<Point>();
        public Direction TravelDirection { get; set; } = Direction.Right;
        public Point ArenaSize { get; set; } = new Point();

        public Snake()
        {
            HeadPos.X = 1;
            HeadPos.Y = 1;
            AddSegment();
        }

        public Snake(Point StartingPosition)
        {
            HeadPos = StartingPosition;
            AddSegment();
        }

        public void ChangeDirection(Direction dir)
        {
            if (((dir == Direction.Up) && (TravelDirection != Direction.Down)) ||
                ((dir == Direction.Down) && (TravelDirection != Direction.Up)) ||
                ((dir == Direction.Left) && (TravelDirection != Direction.Right)) ||
                ((dir == Direction.Right) && (TravelDirection != Direction.Left)))
            {
                TravelDirection = dir;
            }
        }

        public bool Crashed()
        {
            //Check to see if the Head is in the same position as any of the cells in OccupiedCells
            bool result = false;
            foreach (Point point in OccupiedCells)
            {
               if ((point.X == HeadPos.X) && (point.Y == HeadPos.Y))
                {
                    result = true;
                    break;
                }
            }
            if ((HeadPos.X <= 0) || (HeadPos.X >= ArenaSize.X) || (HeadPos.Y <= 0) || (HeadPos.Y >= ArenaSize.Y))
                result = true;

            return result;
        }

        public bool Move()
        {
            //Change the position of the Head
            switch(TravelDirection)
            {
                case Direction.Up:
                    HeadPos.Y--;
                    break;
                case Direction.Down:
                    HeadPos.Y++;
                    break;
                case Direction.Left:
                    HeadPos.X--;
                    break;
                case Direction.Right:
                    HeadPos.X++;
                    break;
            }
            if (Crashed())
            {
                return false;
            }
            else
            {
                //Add the new position to OccupiedCells
                AddSegment();

                //Remove the tip of the tail:
                RemoveSegment();

                return true;
            }
        }

        public void AddSegment()
        {
            Point point = new Point();
            point.X = HeadPos.X;
            point.Y = HeadPos.Y;

            OccupiedCells.Add(point);
        }

        public void RemoveSegment()
        {
            //Add the last entry in OccupiedCells to ErasableCells
            Point point = new Point(OccupiedCells[0].X, OccupiedCells[0].Y);
            ErasableCells.Add(point);
            //Remove the last entry from OccupiedCells
            OccupiedCells.RemoveAt(0);
        }
    }
}
