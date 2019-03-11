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

        public static IList<T> Shuffle<T>(this IList<T> list)
        {  
            if (list is null)
            {
                throw new NullReferenceException();
            }
            else if (list.Count == 0)
            {
                return list;
            }

            if (random == null)
            {
                random = new Random();
            }

            IList<T> clone = list.ToList();
            IList<T> result = list.ToList();
            
            while (clone.Count > 0)
            {
                int randomIndex = random.Next(clone.Count);
                T obj = clone.ElementAt(randomIndex);

                clone.RemoveAt(randomIndex);
                result.Add(obj);
            }

            return result;
        }
    }
}
