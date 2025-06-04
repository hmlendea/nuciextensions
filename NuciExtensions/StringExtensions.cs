using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

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
