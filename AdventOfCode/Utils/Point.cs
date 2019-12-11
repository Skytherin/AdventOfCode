using System;

namespace AdventOfCode2019.Utils
{
    public class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public Vector SlopeTo(Point other)
        {
            var dx = other.X - X;
            var dy = other.Y - Y;
            var signX = dx < 0 ? -1 : 1;
            var signY = dy < 0 ? -1 : 1;
            dx = Math.Abs(dx);
            dy = Math.Abs(dy);

            if (dx == 0) return new Vector(0, signY);
            if (dy == 0) return new Vector(signX, 0);
            if (dx == 1) return new Vector(signX, signY * dy);
            if (dy == 1) return new Vector(signX * dx, signY * 1);

            int[] primes = { 2,3,5,7,9,11,13,15,17,19,23 };

            foreach (var prime in primes)
            {
                if (prime > dx) break;
                while (dx % prime == 0 && dy % prime == 0)
                {
                    dx /= prime;
                    dy /= prime;
                }
            }

            return new Vector(signX * dx, signY * dy);
        }

        public override bool Equals(object? obj)
        {
            return obj is Point p && Equals(p);
        }

        protected bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }
}