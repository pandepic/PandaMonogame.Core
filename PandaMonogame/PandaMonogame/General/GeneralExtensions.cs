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

    } // GeneralExtensions
}
