using SharpNeat.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaMonogame
{
    public static class GeneralExtensions
    {
        public static void AddSet<TKey, TValue>(this Dictionary<TKey, TValue> d, TKey key, TValue value)
        {
            if (!d.ContainsKey(key))
                d.Add(key, value);
            else
                d[key] = value;
        }

        public static void AddIncrement<TKey, TValue>(this Dictionary<TKey, TValue> d, TKey key, TValue value)
        {
            if (!d.ContainsKey(key))
                d.Add(key, value);
            else
            {
                dynamic a = d[key];
                dynamic b = value;
                d[key] = a + b;
            }
        }
        
        public static T ToEnum<T>(this string str) where T : IConvertible
        {
            return (T)Enum.Parse(typeof(T), str);
        }

        // todo : static instance of rng when none passed
        public static T GetRandomItem<T>(this List<T> list, FastRandom rng = null)
        {
            if (rng == null)
                rng = new FastRandom();

            return list[rng.Next(0, list.Count)];
        }

        // https://stackoverflow.com/questions/273313/randomize-a-listt
        // todo : static instance of rng when none passed
        public static void Shuffle<T>(this List<T> list, FastRandom rng = null)
        {
            if (rng == null)
                rng = new FastRandom();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T GetLastItem<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }

        public static bool ListCompare<T>(this List<T> list1, List<T> list2)
        {
            return list1.All(list2.Contains);
        }

    } // GeneralExtensions
}
