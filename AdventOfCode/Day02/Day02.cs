using System;
using System.IO;
using System.Linq;
using AdventOfCode2019.Utils;
using JetBrains.Annotations;

namespace AdventOfCode.Day02
{
    [UsedImplicitly]
    public static class Day02
    {
        public static long[] Input => File
            .ReadAllLines("Day02/input.txt")
            .First()
            .Split(",")
            .Select(it => Convert.ToInt64(it))
            .ToArray();

        public static long Step1()
        {
            var input = Input;
            input[1] = 12;
            input[2] = 2;
            return IntCodeMachine.RunUntilStopped(input).Memory[0];
        }

        public static int Step2()
        {
            const int sentinel = 19690720;
            for (var noun = 0; noun < 100; noun++)
            for (var verb = 0; verb < 100; verb++)
            {
                var input = Input;
                input[1] = noun;
                input[2] = verb;
                var result = IntCodeMachine.RunUntilStopped(input).Memory[0];
                if (result == sentinel)
                    return noun * 100 + verb;
            }
            throw new ApplicationException();
        }
    }
}