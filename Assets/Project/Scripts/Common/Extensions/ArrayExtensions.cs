using UnityEngine;

namespace Common.Extensions
{
    public static class ArrayExtensions
    {
        public static T PeakRandom<T>(this T[] array)
        {
            int rand = Random.Range(0, array.Length);
            return array[rand];
        }
    }
}