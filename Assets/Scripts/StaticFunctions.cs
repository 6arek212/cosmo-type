using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class StaticFunctions
    {

        // extention function to shuffle a list
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, list.Count);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }
}
