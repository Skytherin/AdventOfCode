using System.Collections.Generic;

namespace AdventOfCode2019.Utils
{
    public static class IntExtensions
    {
        public static IEnumerable<int> Decimate(this int value)
        {
            if (value == 0)
            {
                yield return 0;
                yield break;
            }
            while (value > 0)
            {
                yield return value % 10;
                value = value / 10;
            }
        }

        public static IEnumerable<long> Decimate(this long value)
        {
            if (value == 0)
            {
                yield return 0;
                yield break;
            }
            while (value > 0)
            {
                yield return value % 10;
                value = value / 10;
            }
        }
    }
}