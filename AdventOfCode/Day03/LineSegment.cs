using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day03
{
    public class LineSegment
    {
        public LineSegment(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public LineSegment(Point start, string instruction)
        {
            Start = start;
            End = start.Translate(instruction);
        }

        public Point Start { get; }
        public Point End { get; }
    }

    public static class LineSegmentExtensions
    {
        public static LineSegment Extend(this LineSegment originalSegment, string instruction)
        {
            return new LineSegment(originalSegment.End, originalSegment.End.Translate(instruction));
        }

        public static bool IsVertical(this LineSegment segment)
        {
            return segment.Start.X == segment.End.X;
        }

        public static int Top(this LineSegment segment)
        {
            return Math.Max(segment.Start.Y, segment.End.Y);
        }

        public static int Bottom(this LineSegment segment)
        {
            return Math.Min(segment.Start.Y, segment.End.Y);
        }

        public static int Right(this LineSegment segment)
        {
            return Math.Max(segment.Start.X, segment.End.X);
        }

        public static int Left(this LineSegment segment)
        {
            return Math.Min(segment.Start.X, segment.End.X);
        }

        public static IEnumerable<int> Enumerate(int start, int stop)
        {
            if (start <= stop)
            {
                for (var index = start; index <= stop; index++)
                    yield return index;
            }
            else
            {
                for (var index = start; index >= stop; index--)
                    yield return index;
            }
        }

        public static IEnumerable<Point> Points(this LineSegment self)
        {
            return Enumerate(self.Start.X, self.End.X)
                .SelectMany(x => Enumerate(self.Start.Y, self.End.Y).Select(y => new {x, y}))
                .Select(p => new Point(p.x, p.y));
        }
    }
}