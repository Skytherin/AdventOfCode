using System;

namespace AdventOfCode2019.Utils
{
    public class Vector
    {
        public Vector(int dX, int dY)
        {
            this.dY = dY;
            this.dX = dX;
        }

        public int dX { get; }
        public int dY { get; }

        public override string ToString()
        {
            return $"<{dX},{dY}>";
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector other && Equals(other);
        }

        protected bool Equals(Vector other)
        {
            return dX == other.dX && dY == other.dY;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (dX * 397) ^ dY;
            }
        }

        public double ToAngle()
        {
            var theta = dX switch
            {
                0 => 90,
                _ => Math.Abs(Math.Atan(1.0 * dY / dX) * 180 / Math.PI)
            };
            if (dX >= 0 && dY <= 0)
            {
                return 90 - theta;
            }

            if (dX >= 0 && dY > 0)
            {
                return theta + 90;
            }

            if (dX < 0 && dY >= 0)
            {
                return 270 - theta;
            }

            return 270 + theta;
        }

        public Vector TurnRight()
        {
            if (dX == 1) return new Vector(0, 1);
            if (dX == -1) return new Vector(0, -1);
            if (dY == 1) return new Vector(-1, 0);
            if (dY == -1) return new Vector(1, 0);
            throw new ApplicationException();
        }

        public Vector TurnLeft()
        {
            if (dX == 1) return new Vector(0, -1);
            if (dX == -1) return new Vector(0, 1);
            if (dY == 1) return new Vector(1, 0);
            if (dY == -1) return new Vector(-1, 0);
            throw new ApplicationException();
        }
    }
}