using System.Linq;
using AdventOfCode2019.Utils;
using FluentAssertions;

namespace AdventOfCode2019
{
    public static class Day07
    {
        private const string Input =
            "3,8,1001,8,10,8,105,1,0,0,21,38,59,84,93,110,191,272,353,434,99999,3,9,101,5,9,9,1002,9,5,9,101,5,9,9,4,9,99,3,9,1001,9,3,9,1002,9,2,9,101,4,9,9,1002,9,4,9,4,9,99,3,9,102,5,9,9,1001,9,4,9,1002,9,2,9,1001,9,5,9,102,4,9,9,4,9,99,3,9,1002,9,2,9,4,9,99,3,9,1002,9,5,9,101,4,9,9,102,2,9,9,4,9,99,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,99,3,9,102,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,1,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,2,9,4,9,99,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,1,9,4,9,3,9,1001,9,1,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,99,3,9,1001,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,102,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,99,3,9,101,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,1,9,4,9,99";


        public static void Step1()
        {
            InternalStep1("3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0")
                .Should().Be(43210);
            InternalStep1("3,23,3,24,1002,24,10,24,1002,23,-1,23,101, 5, 23, 23, 1, 24, 23, 23, 4, 23, 99, 0, 0")
                .Should().Be(54321);
            InternalStep1("3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002, 33, 7, 33, 1, 33, 31, 31, 1, 32, 31, 31, 4, 31, 99, 0, 0, 0")
                .Should().Be(65210);
            var result = InternalStep1(Input);
            result.Should().Be(225056);

        }

        public static void Step2()
        {
            InternalStep2("3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27, 4, 27, 1001, 28, -1, 28, 1005, 28, 6, 99, 0, 0, 5")
                .Should().Be(139629729);

            InternalStep2(@"3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,
                            -5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,
                            53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10")
                .Should().Be(18216);

            var result = InternalStep2(Input);
            result.Should().Be(14260332);
        }

        private static long InternalStep1(string str)
        {
            var s = str.ToLongArray();
            return new long[] {0, 1, 2, 3, 4}.Permute().Max(permutation =>
            {
                var amplifier = IntCodeMachine.RunUntilStopped(s, permutation[0], 0);
                amplifier = IntCodeMachine.RunUntilStopped(s, permutation[1], amplifier.Outputs.First());
                amplifier = IntCodeMachine.RunUntilStopped(s, permutation[2], amplifier.Outputs.First());
                amplifier = IntCodeMachine.RunUntilStopped(s, permutation[3], amplifier.Outputs.First());
                amplifier = IntCodeMachine.RunUntilStopped(s, permutation[4], amplifier.Outputs.First());
                return amplifier.Outputs.First();
            });
        }

        private static long InternalStep2(string s)
        {
            return new long[] { 5,6,7,8,9 }.Permute()
                .Max(permutation =>
                {
                    var machines = permutation
                        .Select((phase, index) =>
                        {
                            var machine = new IntCodeMachine(s.ToLongArray());
                            machine.Run(phase);
                            return machine;
                        }).ToArray();
                    long tail = 0;
                    while (machines.Last().Status != IntCodeStatus.Stopped)
                    {
                        machines.ForEach(m =>
                        {
                            m.Run(tail);
                            tail = m.Outputs.Last();
                        });
                    }

                    machines.Select(it => it.Status).Should().AllBeEquivalentTo(IntCodeStatus.Stopped);
                    return tail;
                });
        }
    }
}