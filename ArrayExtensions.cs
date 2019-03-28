using System;
using System.Collections.Generic;
using System.Linq;

namespace NuciExtensions
{
    /// <summary>
    /// Array extensions.
    /// </summary>
    public static class ArrayExtensions
    {
        public static void Append<T>(this T[] source, T item)
        {
            T[] newArray = new T[source.Length + 1];
            newArray[source.Length] = item;
            
            Array.Copy(source, 0, newArray, 0, source.Length);
        }

        public static void Prepend<T>(this T[] source, T item)
        {
            T[] newArray = new T[source.Length + 1];
            newArray[0] = item;

            Array.Copy(source, 0, newArray, 1, source.Length);
        }
        
        public static T Pop<T>(this T[] source)
        {
            T[] newArray = new T[source.Length - 1];
            T item = source[source.Length - 1];
            
            Array.Copy(source, 0, newArray, 0, source.Length - 1);
            
            return item;
        }
    }
}
