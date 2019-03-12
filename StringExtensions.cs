using System;
using System.Linq;

namespace NuciExtensions
{
    public  static class StringExtensions
    {
        public static string Reverse(this string text)
        {
            char[] stringChars = text.ToCharArray();

            Array.Reverse(stringChars);

            return new string(stringChars);
        }
        
        /// <summary>
        /// Gets the duplicated elements.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <returns>The duplicated elements.</returns>
        public static string ToTitleCase(this string source)
        {
            char[] chars = source.ToLower().ToCharArray();

            for (int i = 0; i < chars.Count(); i++)
            {
                if (i == 0 || chars[i - 1] == ' ')
                {
                    chars[i] = char.ToUpper(chars[i]);
                }
            }

            return new string(chars);
        }

        public static string ToSentanceCase(this string source)
        {
            char[] chars = source.ToLower().ToCharArray();

            chars[0] = char.ToUpper(chars[0]);

            return new string(chars);
        }

        public static string ToSentance(this string source)
        {
            string sentance = source.Substring(0, 1);
            string charsToReplaceWithSpace = "_\t";

            for (int i = 1; i < source.Length; i++)
            {
                if (source[i] >= 'A' && source[i] <= 'Z')
                {
                    sentance += $" {source[i]}";
                }
                else if (charsToReplaceWithSpace.Contains(source[i]))
                {
                    sentance += ' ';
                }
                else
                {
                    sentance += source[i];
                }
            }

            sentance = sentance.Trim();

            return sentance;
        }

        public static string Repeat(this string source, int count)
        {
            string result = string.Empty;

            for (int i = 0; i < count; i++)
            {
                result += source;
            }

            return result;
        }
    }
}
