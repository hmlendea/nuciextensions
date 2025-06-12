using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace NuciExtensions
{
    public  static class StringExtensions
    {
        /// <summary>
        /// Reverses the characters in the specified string.
        /// </summary>
        /// /// <param name="text">The string to reverse.</param>
        /// <returns>A new string with the characters in reverse order.</returns>
        public static string Reverse(this string text)
        {
            char[] stringChars = text.ToCharArray();

            Array.Reverse(stringChars);

            return new string(stringChars);
        }

        /// <summary>
        /// Repeats the specified string a given number of times.
        /// This method concatenates the string to itself the specified number of times.
        /// If the count is less than or equal to zero, an empty string is returned.
        /// </summary>
        /// /// <param name="source">The string to repeat.</param>
        /// <param name="count">The number of times to repeat the string.</param>
        /// <returns>A new string that is the result of repeating the source string the specified number of times.</returns>
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
        /// Replaces the first occurrence of a specified string in the source string with a new value.
        /// </summary>
        /// <param name="source">The source string in which to perform the replacement.</param>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string to replace the old value with. If null, it will be replaced with an empty string.</param>
        /// <returns>A new string with the first occurrence of the old value replaced by the new value.</returns>
        /// <exception cref="NullReferenceException">Thrown if the source string is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the old value is null or an empty string.</exception>
        public static string ReplaceFirst(this string source, string oldValue, string newValue)
        {
            if (source is null)
            {
                throw new NullReferenceException();
            }

            ArgumentNullException.ThrowIfNull(oldValue);

            if (oldValue.Equals(string.Empty))
            {
                throw new ArgumentException("String cannot be of zero length.");
            }

            newValue ??= string.Empty;

            if (source.Equals(string.Empty))
            {
                return string.Empty;
            }

            int loc = source.IndexOf(oldValue);

            if (loc < 0)
            {
                return source;
            }

            return source
                .Remove(loc, oldValue.Length)
                .Insert(loc, newValue);
        }

        /// <summary>
        /// Removes diacritics from the specified string.
        /// </summary>
        /// <param name="source">The source string from which to remove diacritics.</param>
        /// <returns>A new string with diacritics removed.</returns>
        public static string RemoveDiacritics(this string source)
        {
            string firstPass = source;

            Dictionary<string, string> customMappings = new()
            {
                { "Ö", "OE" },
                { "Č", "Ch" },
                { "Š", "Sh" },
                { "Ř", "Rzh" },
                { "Ž", "Zh" },
            };

            foreach (var customMapping in customMappings)
            {
                firstPass = Regex.Replace(firstPass, "([\\p{Lu}])" + customMapping.Key, "$1" + customMapping.Value.ToUpper());
                firstPass = Regex.Replace(firstPass, customMapping.Key + @"([^\p{Lu}])", customMapping.Value + "$1");
                firstPass = Regex.Replace(firstPass, customMapping.Key.ToLower(), customMapping.Value.ToLower());
            }

            string normalisedSource = firstPass.Normalize(NormalizationForm.FormD);
            string result = string.Empty;

            foreach (char c in normalisedSource)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);

                if (category != UnicodeCategory.NonSpacingMark)
                {
                    result += c;
                }
            }

            return result.Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Removes punctuation characters from the specified string.
        /// </summary>
        /// <param name="source">The source string from which to remove punctuation.</param>
        /// <returns>A new string with punctuation characters removed.</returns>
        public static string RemovePunctuation(this string source)
        {
            string result = string.Empty;

            foreach (char c in source)
            {
                if (!char.IsPunctuation(c))
                {
                    result += c;
                }
            }

            return result;
        }

        public static string Reeverse(this string source)
        {
            char[] chars = source.ToCharArray();

            Array.Reverse(source.ToCharArray());

            return new string(chars);
        }

        /// <summary>
        /// Converts a string to a sentence format.
        /// </summary>
        /// <param name="source">The source string to convert.</param>
        /// <returns>A new string formatted as a sentence.</returns>
        public static string ToSentence(this string source)
        {
            string sentence = source[..1];
            string charsToReplaceWithSpace = "_\t";

            for (int i = 1; i < source.Length; i++)
            {
                if (source[i] >= 'A' && source[i] <= 'Z')
                {
                    sentence += $" {source[i]}";
                }
                else if (charsToReplaceWithSpace.Contains(source[i]))
                {
                    sentence += ' ';
                }
                else
                {
                    sentence += source[i];
                }
            }

            return sentence.Trim();
        }
    }
}
