using System;
using System.IO;
using System.Linq;
using System.Threading;
using AdventOfCode2019.Utils;
using FluentAssertions;

namespace AdventOfCode2019
{
    public static class Day08
    {
        public static char[] Input => File
            .ReadAllText("Inputs/Day08.txt").Trim().ToCharArray();

        private const int Width = 25;
        private const int Height = 6;

        public static void Step1()
        {
            var result = Input.InGroupsOf(Width * Height)
                    .Select(group =>
                    {
                        return group.Aggregate(new Summary(),
                            (accum, item) =>
                            {
                                if (item == '0') accum.Zeroes += 1;
                                if (item == '1') accum.Ones += 1;
                                if (item == '2') accum.Twos += 1;
                                return accum;
                            });
                    })
                    .ItemByMin(it => it.Zeroes)
                ;
            var result2 = result.Ones * result.Twos;
            result2.Should().Be(2286);
        }

        public static void Step2()
        {
            var result = Input
                .InGroupsOf(Width * Height)
                .Aggregate((accum, group) =>
                {
                    return accum.Zip(group).Select(z =>
                    {
                        if (z.First == '2') return z.Second;
                        return z.First;
                    }).ToList();
                });

            result.InGroupsOf(Width)
                .ForEach(row => Console.WriteLine(row.Select(it => it == '0' ? ' ' : '*').Join("")));
        }
    }

    public class Summary
    {
        public int Zeroes { get; set; }
        public int Ones { get; set; }
        public int Twos { get; set; }
    }
}