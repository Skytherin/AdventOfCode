using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2019.Utils;
using FluentAssertions;

namespace AdventOfCode2019
{
    public static class Day10
    {
        public static string Input => File.ReadAllText("Inputs/Day10.txt");

        public static void Step1()
        {
            InternalStep1(@".#..#
.....
#####
....#
...##").Should().Be(Tuple.Create(new Point(3, 4), 8));

            InternalStep1(@"......#.#.
#..#.#....
..#######.
.#.#.###..
.#..#.....
..#....#.#
#..#....#.
.##.#..###
##...#..#.
.#....####").Should().Be(Tuple.Create(new Point(5, 8), 33));

            InternalStep1(@"#.#...#.#.
.###....#.
.#....#...
##.#.#.#.#
....#.#.#.
.##..###.#
..#...##..
..##....##
......#...
.####.###.").Should().Be(Tuple.Create(new Point(1, 2), 35));

            InternalStep1(@".#..#..###
####.###.#
....###.#.
..###.##.#
##.##.#.#.
....###..#
..#.#..#.#
#..#.#.###
.##...##.#
.....#.#..").Should().Be(Tuple.Create(new Point(6, 3), 41));

            InternalStep1(@".#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##").Should().Be(Tuple.Create(new Point(11, 13), 210));

            var result = InternalStep1(Input);
            result.Should().Be(Tuple.Create(new Point(14, 17), 260));

            Step2();
        }

        public static void Step2()
        {
            InternalStep2(@".#..##.###...#######
                    ##.############..##.
                    .#.######.########.#
                    .###.#######.####.#.
                    #####.##.#.##.###.##
                    ..#####..#.#########
                    ####################
                    #.####....###.#.#.##
                    ##.#################
                    #####.##.###..####..
                    ..######..##.#######
                    ####.##.####...##..#
                    .#####..#.######.###
                    ##...#.##########...
                    #.##########.#######
                    .####.#.###.###.#.##
                    ....##.##.###..#####
                    .#.#.###########.###
                    #.#.#.#####.####.###
                    ###.##.####.##.#..##", new Point(11, 13))
                .Should().Be(802);

            var result = InternalStep2(Input, new Point(14,17));
            result.Should().Be(608);
        }

        public static int InternalStep2(string input, Point center)
        {
            var points = InputToPoints(input);
            var count = 0;
            while (points.Count > 1)
            {
                var pointToOthers = PointToOthers(points);
                var removed = pointToOthers[center].OrderBy(other => other.Item2.ToAngle()).ToList();
                foreach (var item in removed)
                {
                    if (++count == 200)
                    {
                        return item.Item1.X * 100 + item.Item1.Y;
                    }
                }

                var removedPoints = removed.Select(it => it.Item1);
                points.RemoveAll(p => removedPoints.Contains(p));
            }

            throw new ApplicationException();
        }

        public static Dictionary<Point, List<Tuple<Point, Vector>>> PointToOthers(List<Point> points)
        {
            var pointToCount = new Dictionary<Point, List<Tuple<Point, Vector>>>();
            foreach (var item in points.Select((it, index) => new { point = it, index }))
            {
                var slopes = new HashSet<Vector>();
                var point = item.point;
                foreach (var other in points.Skip(item.index + 1))
                {
                    var slope = point.SlopeTo(other);
                    if (!slopes.Contains(slope))
                    {
                        slopes.Add(slope);
                        pointToCount.AlterWithDefault(point, new List<Tuple<Point, Vector>>(), v => v.Add(Tuple.Create(other, slope)));
                        pointToCount.AlterWithDefault(other, new List<Tuple<Point, Vector>>(), v => v.Add(Tuple.Create(point, new Vector(-slope.dX, -slope.dY))));
                    }
                }
            }

            return pointToCount;
        }

        public static Tuple<Point, int> InternalStep1(string input)
        {
            var pointToCount = PointToOthers(InputToPoints(input));

            var result = pointToCount.Items().ItemByMax(kv => kv.Value.Count);
            return Tuple.Create(result.Key, result.Value.Count);
        }

        private static List<Point> InputToPoints(string input)
        {
            var points = input.Split("\n").Select(l => l.Trim())
                .SelectMany((row, y) =>
                    row.ToCharArray().Select((col, x) =>
                        {
                            return col switch
                            {
                                '.' => null as Point,
                                '#' => new Point(x, y),
                                _ => throw new ApplicationException()
                            };
                        })
                        .OfType<Point>())
                .ToList();
            return points;
        }
    }
}