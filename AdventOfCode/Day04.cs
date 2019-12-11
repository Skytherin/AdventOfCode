using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Utils;
using FluentAssertions;

namespace AdventOfCode2019
{
    public static class Day04
    {
        public static bool HasRepeatingDigit(this int value)
        {
            return value
                .Decimate()
                .GroupBy(it => it)
                .Any(group => group.Count() >= 2);
        }

        public static bool HasRepeatingDigitAdvanced(this int value)
        {
            return value
                .Decimate()
                .GroupBy(it => it)
                .Any(group => group.Count() == 2);
        }

        public static IEnumerable<int> GenerateRange(int place, int start)
        {
            if (place == 0)
            {
                yield return 0;
                yield break;
            }

            for (var value = start; value <= 9; value++)
            {
                foreach (var rightvalue in GenerateRange(place / 10, value))
                {
                    yield return rightvalue + value * place;
                }
            }
        }

        public static void Step1()
        {
            var result = GenerateRange(100000, 1)
                .Where(it => it.HasRepeatingDigit())
                .SkipWhile(it => it < 171309)
                .TakeWhile(it => it <= 643603)
                .Count();
            result.Should().Be(1625);
            Console.WriteLine("Day4.1 = " + result); // 1625 is correct
        }

        public static void Step2()
        {
            var result = GenerateRange(100000, 1)
                .Where(it => it.HasRepeatingDigitAdvanced())
                .SkipWhile(it => it < 171309)
                .TakeWhile(it => it <= 643603)
                .Count();
            result.Should().Be(1111);
            Console.WriteLine("Day4.2 = " + result); // 1111 is correct
        }
    }
}