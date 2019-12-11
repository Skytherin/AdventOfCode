using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Utils;

namespace AdventOfCode.Day03
{
    public class LinePath
    {
        public List<LineSegment> Lines { get; } = new List<LineSegment>();

        public Dictionary<Point, int> DistanceToPoint = new Dictionary<Point, int>();

        private int _extremis = 0;

        public void AddLine(LineSegment line)
        {
            Lines.Add(line);
            var points = line.Points().ToArray();
            points.Skip(1)
                .ForEach(point =>
                {
                    ++_extremis;
                    if (!DistanceToPoint.ContainsKey(point))
                    {
                        DistanceToPoint[point] = _extremis;
                    }
                });
        }

        public IEnumerable<Point> Intersections(LinePath other)
        {
            return DistanceToPoint.Keys
                .Intersect(other.DistanceToPoint.Keys);
        }
    }
}