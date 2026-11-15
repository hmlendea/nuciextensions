using System;
using System.Text;

namespace NuciExtensions
{
    /// <summary>
    /// Provides extension methods for string casing operations.
    /// </summary>
    public static class StringCasingExtensions
    {
        /// <summary>
        /// Converts the string to title case (each word starts with uppercase).
        /// </summary>
        /// <param name="source">The source string that is to be converted. Must not be null.</param>
        /// <returns>A new string in title case. Never null.</returns>
        /// <remarks>
        /// Case conversion is culture-invariant: <see cref="string.ToLowerInvariant"/>,
        /// <see cref="char.ToUpperInvariant(char)"/> and <see cref="char.ToLowerInvariant(char)"/>
        /// are used, so the result does not depend on the current culture. In particular, the
        /// Turkish-I behaviour is never applied, and <c>"iki".ToTitleCase()</c> always returns
        /// <c>"Iki"</c>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the source string is null.</exception>
        public static string ToTitleCase(this string source)
        {
            ArgumentNullException.ThrowIfNull(source);

            char[] chars = source.ToLowerInvariant().ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (i.Equals(0) || !char.IsLetterOrDigit(chars[i - 1]))
                {
                    chars[i] = char.ToUpperInvariant(chars[i]);
                }
            }

            return new string(chars);
        }

        /// <summary>
        /// Converts the first letter of each sentence to uppercase.
        /// </summary>
        /// <param name="source">The source string that is to be converted. Must not be null.</param>
        /// <returns>
        /// A new string with the first letter of each sentence in uppercase. When the source is
        /// empty, the empty string is returned. Never null.
        /// </returns>
        /// <remarks>
        /// Case conversion is culture-invariant (<see cref="char.ToUpperInvariant(char)"/>), and
        /// the remaining characters are left untouched, so interior capitals are preserved.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the source string is null.</exception>
        public static string ToSentenceCase(this string source)
        {
            ArgumentNullException.ThrowIfNull(source);

            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            char[] chars = source.ToCharArray();
            bool isNewSentence = true;

            for (int i = 0; i < source.Length; i++)
            {
                if (isNewSentence && char.IsLetter(chars[i]))
                {
                    chars[i] = char.ToUpperInvariant(chars[i]);
                    isNewSentence = false;
                }
                else if (chars[i] == '.')
                {
                    isNewSentence = true;
                }
            }

            return new string(chars);
        }

        /// <summary>
        /// Converts the string to upper snake case.
        /// Upper snake case means that all letters are uppercase and words are separated by underscores.
        /// For example, "Hello World" becomes "HELLO_WORLD".
        /// </summary>
        /// <param name="source">The source string that is to be converted. Must not be null.</param>
        /// <returns>A new string in upper snake case. Never null.</returns>
        /// <remarks>
        /// Uppercasing is performed with <see cref="string.ToUpperInvariant"/>, so the result does
        /// not depend on the current culture.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the source string is null.</exception>
        public static string ToUpperSnakeCase(this string source)
            => source.ToSnakeCase().ToUpperInvariant();

        /// <summary>
        /// Converts the string to lower snake case.
        /// Lower snake case means that all letters are lowercase and words are separated by underscores.
        /// For example, "Hello World" becomes "hello_world".
        /// </summary>
        /// <param name="source">The source string that is to be converted. Must not be null.</param>
        /// <returns>A new string in lower snake case. Never null.</returns>
        /// <remarks>
        /// Lowercasing is performed with <see cref="string.ToLowerInvariant"/>, so the result does
        /// not depend on the current culture.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the source string is null.</exception>
        public static string ToLowerSnakeCase(this string source)
            => source.ToSnakeCase().ToLowerInvariant();

        /// <summary>
        /// Converts the string to snake case.
        /// </summary>
        /// <param name="source">The source string that is to be converted. Must not be null.</param>
        /// <returns>A new string in snake case. Never null.</returns>
        /// <remarks>
        /// Only alphanumeric characters and whitespace are retained; whitespace runs become single
        /// underscores. All other characters (including existing underscores, hyphens, dots, and
        /// other separators) are removed. Letters are copied unchanged (never upper- or lower-cased),
        /// so no case conversion is performed and the result is culture-invariant. Consecutive
        /// underscores are collapsed in a single pass.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the source string is null.</exception>
        public static string ToSnakeCase(this string source)
        {
            ArgumentNullException.ThrowIfNull(source);

            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            StringBuilder result = new();

            foreach (char c in source)
            {
                if (char.IsLetterOrDigit(c))
                {
                    result.Append(c);
                }
                else if (char.IsWhiteSpace(c))
                {
                    if (result.Length == 0 || result[^1] != '_')
                    {
                        result.Append('_');
                    }
                }
                // All other characters (underscores, hyphens, dots, etc.) are silently dropped
            }

            return result.ToString();
        }
    }
}
