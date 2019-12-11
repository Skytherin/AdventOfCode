using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;

namespace AdventOfCode2019.Utils
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach (var item in self)
            {
                action(item);
            }
        }

        public static T Shift<T>(this List<T> self)
        {
            var result = self.First();
            self.RemoveAt(0);
            return result;
        }

        public static long[] ToLongArray(this string self)
        {
            return self.Split(",").Select(it => Convert.ToInt64(it)).ToArray();
        }

        public static IEnumerable<List<T>> Permute<T>(this IEnumerable<T> self)
        {
            var temp = self.ToList();
            if (temp.Count == 0)
            {
                yield return new List<T>();
                yield break;
            }
            if (temp.Count == 1)
            {
                yield return temp;
                yield break;
            }

            var item = temp.First();
            var remainder = temp.Skip(1).Permute();
            foreach (var list in remainder)
            {
                for (int i = 0; i <= list.Count; i++)
                {
                    var dup = list.ToList();
                    dup.Insert(i, item);
                    yield return dup;
                }
            }
        }

        public static IEnumerable<List<T>> InGroupsOf<T>(this IEnumerable<T> self, int count)
        {
            var current = new List<T>();
            foreach (var item in self)
            {
                current.Add(item);
                if (current.Count == count)
                {
                    yield return current;
                    current = new List<T>();
                }
            }
        }

        public static T ItemByMin<T>(this IEnumerable<T> self, Func<T, int> func)
        {
            return self
                .Select(item => new {item, value = func(item)})
                .Aggregate((accum, item) =>
                {
                    if (item.value < accum.value) return item;
                    return accum;
                })
                .item;
        }

        public static T ItemByMax<T>(this IEnumerable<T> self, Func<T, int> func)
        {
            return self
                .Select(item => new { item, value = func(item) })
                .Aggregate((accum, item) =>
                {
                    if (item.value > accum.value) return item;
                    return accum;
                })
                .item;
        }

        public static string Join<T>(this IEnumerable<T> self, string joiner)
        {
            return string.Join(joiner, self);
        }

        public static IEnumerable<KeyValuePair<TK,TV>> Items<TK, TV>(this IDictionary<TK, TV> self)
            where TK : notnull
        {
            return self.Select(it => it);
        }
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
            where TKey: notnull => dictionary.TryGetValue(key, out var ret) ? ret : default;

        public static void AlterWithDefault<TK, TV>(this Dictionary<TK, TV> self, TK key, TV dflt, Func<TV,TV> func) where TK : notnull
        {
            var value = self.GetValueOrDefault(key, dflt);
            self[key] = func(value);
        }

        public static void AlterWithDefault<TK, TV>(this Dictionary<TK, TV> self, TK key, TV dflt, Action<TV> action) where TK : notnull
        {
            if (self.TryGetValue(key, out var ret))
            {
                action(ret);
            }
            else
            {
                self[key] = dflt;
                action(dflt);
            }
        }
    }
}