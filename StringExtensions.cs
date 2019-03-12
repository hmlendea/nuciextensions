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

        public static string Repeat(this string source, int count)
        {
            string result = string.Empty;

            for (int i = 0; i < count; i++)
            {
                result += source;
            }

            return result;
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

            bool isNewSentance = true;
            for (int i = 0; i < source.Length; i++)
            {
                if (!isNewSentance)
                {
                    continue;
                }

                if (chars[i] >= 'a' && chars[i] <= 'z')
                {
                    chars[i] = char.ToUpper(chars[i]);
                    isNewSentance = false;
                }
                else if (chars[i] == '.')
                {
                    isNewSentance = true;
                }
            }

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
    }
}
