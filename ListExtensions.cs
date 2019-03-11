using System;
using System.Collections.Generic;
using System.Linq;

namespace NuciExtensions
{
    /// <summary>
    /// List extensions.
    /// </summary>
    public static class ListExtensions
    {
        static Random random;

        public static void Shuffle<T>(this IList<T> list)  
        {  
            if (EnumerableExt.IsNullOrEmpty(list))
            {
                throw new NullReferenceException();
            }

            if (random == null)
            {
                random = new Random();
            }

            int elementsCount = list.Count;

            while (elementsCount > 1)
            {
                elementsCount--;
                int randomIndex = random.Next(elementsCount + 1);

                T element = list[randomIndex];
                list[randomIndex] = list[elementsCount];
                list[elementsCount] = element;
            }
        }
    }
}
