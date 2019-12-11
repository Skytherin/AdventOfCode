using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Day03
{
    public class Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            var hash = 23;
            hash = hash * 31 + Y;
            hash = hash * 31 + X;
            return hash;
        }

        public override bool Equals(object? obj)
        {
            return obj is Point p && Equals(p);
        }

        public bool Equals(Point other)
        {
            return other != null && X == other.X && Y == other.Y;
        }
    }

    public static class PointExtensions
    {
        public static Point Up(this Point p, int distance)
        {
            return new Point(p.X, p.Y + distance);
        }

        public static Point Down(this Point p, int distance)
        {
            return new Point(p.X, p.Y - distance);
        }

        public static Point Right(this Point p, int distance)
        {
            return new Point(p.X + distance, p.Y);
        }

        public static Point Left(this Point p, int distance)
        {
            return new Point(p.X - distance, p.Y);
        }

        public static Point Translate(this Point p, string instruction)
        {
            var rx = new Regex(@"^[UDRL]\d+$");
            rx.IsMatch(instruction).Should().BeTrue();

            var instructions = new Dictionary<char, Func<Point, int, Point>>
            {
                {'U', Up},
                {'D', Down},
                {'L', Left},
                {'R', Right}
            };

            return instructions[instruction[0]](p, Convert.ToInt32(instruction.Substring(1)));
        }

        public static int ManhattanDistance(this Point p, Point other)
        {
            return Math.Abs(p.X - other.X) + Math.Abs(p.Y - other.Y);
        }
    }
}