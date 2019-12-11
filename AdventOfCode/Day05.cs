using System;
using System.IO;
using System.Linq;
using AdventOfCode2019.Utils;
using FluentAssertions;

namespace AdventOfCode2019
{
    public static class Day05
    {
        public static long[] Input => File
            .ReadAllLines("Inputs/Day05.txt")
            .First()
            .Split(",")
            .Select(it => Convert.ToInt64(it))
            .ToArray();

        public static void Step1()
        {
            IntCodeMachine.RunUntilStopped(new long[]{1002,5,3,5,99,33})
                .Memory[5].Should().Be(99);

            var machine = IntCodeMachine.RunUntilStopped(Input, new long[] { 1 });
            machine.Outputs.Last().Should().Be(7839346);
        }

        public static void Step2()
        {
            IntCodeMachine.RunUntilStopped(new long[]{3,9,8,9,10,9,4,9,99,-1,8},
                new long[] {8}).Outputs.Single().Should().Be(1);
            IntCodeMachine.RunUntilStopped(new long[]{3,9,8,9,10,9,4,9,99,-1,8},
                new long[] { 7 }).Outputs.Single().Should().Be(0);

            var example = new long[]
            {
                3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31,
                1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104,
                999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99
            };

            IntCodeMachine.RunUntilStopped(example, 6).Outputs.Single().Should().Be(999);
            IntCodeMachine.RunUntilStopped(example, 7).Outputs.Single().Should().Be(999);
            IntCodeMachine.RunUntilStopped(example, 8).Outputs.Single().Should().Be(1000);
            IntCodeMachine.RunUntilStopped(example, 9).Outputs.Single().Should().Be(1001);
            IntCodeMachine.RunUntilStopped(example, 10).Outputs.Single().Should().Be(1001);

            var machine = IntCodeMachine.RunUntilStopped(Input, 5);
            machine.Outputs.Last().Should().Be(447803);
        }
    }
}