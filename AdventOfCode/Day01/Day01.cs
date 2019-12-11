using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace AdventOfCode.Day01
{
    [UsedImplicitly]
    public static class Day01
    {
        public static long[] Input => File.ReadAllLines("Day1/input.txt").Select(it => Convert.ToInt64(it)).ToArray();

        private static long FuelForMass(long mass)
        {
            return Math.Max(0, mass / 3 - 2);
        }

        public static long Step1()
        {
            return Input.Select(FuelForMass).Sum();
        }

        private static long FuelForFuel(long fuel)
        {
            if (fuel == 0) return 0;
            var mass = FuelForMass(fuel);
            return fuel + FuelForFuel(mass);
        }

        public static long Step2()
        {
            return Input.Select(it => FuelForFuel(FuelForMass(it))).Sum();
        }
    }
}