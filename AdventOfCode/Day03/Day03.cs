using System;
using System.IO;
using System.Linq;
using AdventOfCode2019.Utils;
using FluentAssertions;
using JetBrains.Annotations;

namespace AdventOfCode.Day03
{
    [UsedImplicitly]
    public static class Day03
    {
        private static readonly string[][] Input = File
            .ReadAllLines("Day03/input.txt")
            .Select(line => line.Split(","))
            .ToArray();

        public static void TestStep1()
        {
            InternalStep1("R8,U5,L5,D3".Split(","), "U7,R6,D4,L4".Split(","))
                .Should().Be(6);
            InternalStep1("R75,D30,R83,U83,L12,D49,R71,U7,L72".Split(","),
                    "U62,R66,U55,R34,D71,R55,D58,R83".Split(",")).Should().Be(159);
            InternalStep1("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51".Split(","),
                    "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7".Split(","))
                .Should().Be(135);
            InternalStep1(Input[0], Input[1]).Should().Be(860);
        }

        public static int Step1()
        {
            TestStep1();
            return InternalStep1(Input[0], Input[1]);
        }

        public static int InternalStep1(string[] steps1, string[] steps2)
        {
            var path1 = GeneratePath(steps1);
            var path2 = GeneratePath(steps2);

            var temp = path1.Intersections(path2);
            return temp
                .Select(p => p.ManhattanDistance(new Point(0, 0)))
                // Per instructions, remove hit at origin
                .Where(d => d > 0)
                .Min();
        }


        public static void TestStep2()
        {
            InternalStep2("R8,U5,L5,D3".Split(","), "U7,R6,D4,L4".Split(","))
                .Should().Be(30);
            InternalStep2("R75,D30,R83,U83,L12,D49,R71,U7,L72".Split(","),
                    "U62,R66,U55,R34,D71,R55,D58,R83".Split(","))
                .Should().Be(610);
            InternalStep2("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51".Split(","),
                    "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7".Split(","))
                .Should().Be(410);

            InternalStep2(Input[0], Input[1]).Should().Be(9238);
        }

        public static int Step2()
        {
            TestStep2();
            return InternalStep2(Input[0], Input[1]);
        }

        private static int InternalStep2(string[] steps1, string[] steps2)
        {
            var path1 = GeneratePath(steps1);
            var path2 = GeneratePath(steps2);

            var temp = path1.Intersections(path2);
            return temp
                .Select(p =>
                {
                    return path1.DistanceToPoint[p] + path2.DistanceToPoint[p];
                })
                .Min();
        }


        private static LinePath GeneratePath(string[] instructions)
        {
            var result = new LinePath();
            result.AddLine(new LineSegment(new Point(0, 0), instructions.First()));
            instructions.Skip(1).ForEach(instruction =>
            {
                result.AddLine(result.Lines.Last().Extend(instruction));
            });

            return result;
        }
    }
}