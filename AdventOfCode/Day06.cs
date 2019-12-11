using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AdventOfCode2019.Utils;
using FluentAssertions;

namespace AdventOfCode2019
{
    public static class Day06
    {
        public static string[] Input => File
            .ReadAllLines("Inputs/Day06.txt")
            .ToArray();

        public static void Step1()
        {
            InternalStep1(new[]{"COM)B",
                "B)C",
                "C)D",
                "D)E",
                "E)F",
                "B)G",
                "G)H",
                "D)I",
                "E)J",
                "J)K",
                "K)L"}).Should().Be(42);
            InternalStep1(Input).Should().Be(142497);
        }

        public static void Step2()
        {
            InternalStep2(new[]{"COM)B",
                "B)C",
                "C)D",
                "D)E",
                "E)F",
                "B)G",
                "G)H",
                "D)I",
                "E)J",
                "J)K",
                "K)L",
                "K)YOU",
                "I)SAN"
            }).Should().Be(4);
            var result = InternalStep2(Input);
            result.Should().Be(301);
        }

        private static int InternalStep2(string[] orbits)
        {
            var master = GetOrbits(orbits);
            var path1 = Walk(master, "YOU");
            var path2 = Walk(master, "SAN");
            var commonList = path1.Intersect(path2);
            var common = master[commonList.First()];
            var a = master["YOU"];
            var b = master["SAN"];
            return (a.Distance - common.Distance - 1) + (b.Distance - common.Distance - 1);
        }

        private static IEnumerable<string> Walk(Dictionary<string, OrbitEntry> master, string startingPosition)
        {
            var current = master[startingPosition].Parent;
            while (current != null)
            {
                yield return current.Name;
                current = current.Parent;
            }
        }

        private static int InternalStep1(string[] orbits)
        {
            return GetOrbits(orbits).Aggregate(0, (accum, kv) => accum + kv.Value.Distance);
        }

        private static Dictionary<string, OrbitEntry> GetOrbits(string[] orbits)
        {
            var master = new Dictionary<string, OrbitEntry>();
            foreach (var orbit in orbits)
            {
                var temp = orbit.Split(")");
                var name = temp[0];
                var satellite = temp[1];
                if (!master.ContainsKey(satellite))
                {
                    master.Add(satellite, new OrbitEntry(satellite));
                }
                if (!master.ContainsKey(name))
                {
                    master.Add(name, new OrbitEntry(name));
                }

                var p = master[name];
                var c = master[satellite];
                p.Satellites.Add(c);
                // sanity check a satellite only directly orbits one parent
                c.Parent.Should().BeNull();
                c.Parent = p;
            }

            var open = new List<OrbitEntry> { master["COM"] };
            while (open.Any())
            {
                var entry = open.Shift();
                entry.Satellites.ForEach(s =>
                {
                    s.Distance = entry.Distance + 1;
                    open.Add(s);
                });
            }

            // sanity check everything is parented correctly
            master.Where(kv => kv.Value.Distance == 0).Should().ContainSingle()
                .Which.Key.Should().Be("COM");
            return master;
        }
    }

    public class OrbitEntry
    {
        public OrbitEntry(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public int Distance { get; set; } = 0;
        public List<OrbitEntry> Satellites { get; } = new List<OrbitEntry>();
        public OrbitEntry? Parent { get; set; }
    }
}