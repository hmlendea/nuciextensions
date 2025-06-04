using System;
using System.Globalization;
using System.Text;

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
            string normalisedSource = source.Normalize(NormalizationForm.FormD);
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

            return sentance.Trim();
        }
    }
}
