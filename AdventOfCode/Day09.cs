using System.IO;
using System.Linq;
using AdventOfCode2019.Utils;
using FluentAssertions;

namespace AdventOfCode2019
{
    public static class Day09
    {
        public static long[] Input => File
            .ReadAllText("Inputs/Day09.txt")
            .ToLongArray();


        public static void Step1()
        {
            IntCodeMachine.RunUntilStopped(new[]
                    {109L, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99})
                .Outputs.Should().BeEquivalentTo(new[]
                    {109L, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99});

            IntCodeMachine.RunUntilStopped(new long[] {1102, 34915192, 34915192, 7, 4, 7, 99, 0})
                .Outputs.Single().Decimate().Should().HaveCount(16);

            IntCodeMachine.RunUntilStopped(new long[] {104, 1125899906842624, 99})
                .Outputs.Single().Should().Be(1125899906842624);

            var result = IntCodeMachine.RunUntilStopped(Input, 1);
            result.Outputs.Single().Should().Be(3989758265L);

            var result2 = IntCodeMachine.RunUntilStopped(Input, 2);
            result2.Outputs.Single().Should().Be(76791L);
        }
    }
}