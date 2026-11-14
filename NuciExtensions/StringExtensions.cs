using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace NuciExtensions
{
    /// <summary>
    /// Provides extension methods for string operations.
    /// </summary>
    public static class StringExtensions
    {
        private static readonly Dictionary<char, string> CustomMappings = new()
        {
            { 'Ö', "Oe" },
            { 'Č', "Ch" },
            { 'Š', "Sh" },
            { 'Ř', "Rzh" },
            { 'Ž', "Zh" },
        };

        /// <summary>
        /// Inverts the case of each character in the specified string.
        /// </summary>
        /// <param name="text">The string whose case is to be inverted. If null or empty, it is returned unchanged.</param>
        /// <returns>A new string with each character's case inverted, null when <paramref name="text"/> is null, or the empty string when it is empty.</returns>
        public static string? InvertCase(this string? text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            char[] result = new char[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (char.IsUpper(c))
                {
                    result[i] = char.ToLower(c);
                }
                else
                {
                    result[i] = char.ToUpper(c);
                }
            }

            return new string(result);
        }

        /// <summary>
        /// Reverses the characters in the specified string.
        /// The reversal is performed by Unicode code point, so the two halves of a
        /// surrogate pair always remain adjacent and no unpaired surrogate is produced.
        /// </summary>
        /// <param name="text">The string to reverse.</param>
        /// <returns>A new string with the characters in reverse order.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the string is null.</exception>
        /// <remarks>
        /// Combining-mark sequences are reversed as part of the reversed sequence,
        /// so a base character followed by its combining marks ends up preceded by
        /// those marks in the result. Grapheme-cluster segmentation is not performed.
        /// </remarks>
        public static string Reverse(this string text)
        {
            ArgumentNullException.ThrowIfNull(text);

            if (text.Length < 2)
            {
                return text;
            }

            char[] reversedChars = new char[text.Length];
            int writeIndex = 0;
            int readIndex = text.Length - 1;

            while (readIndex >= 0)
            {
                if (char.IsLowSurrogate(text[readIndex]) &&
                    readIndex > 0 &&
                    char.IsHighSurrogate(text[readIndex - 1]))
                {
                    reversedChars[writeIndex] = text[readIndex - 1];
                    reversedChars[writeIndex + 1] = text[readIndex];
                    writeIndex += 2;
                    readIndex -= 2;
                }
                else
                {
                    reversedChars[writeIndex] = text[readIndex];
                    writeIndex++;
                    readIndex--;
                }
            }

            return new string(reversedChars);
        }

        /// <summary>
        /// Repeats the specified string a given number of times.
        /// This method concatenates the string to itself the specified number of times.
        /// If the count is less than or equal to zero, an empty string is returned.
        /// </summary>
        /// <param name="source">The string to repeat.</param>
        /// <param name="count">The number of times to repeat the string.</param>
        /// <returns>A new string that is the result of repeating the source string the specified number of times.</returns>
        /// <remarks>
        /// The result length is <c>source.Length * count</c> and is computed with a
        /// widened multiplication, so it cannot overflow. It must not exceed
        /// <see cref="Array.MaxLength"/> characters, which is the largest length the
        /// runtime supports for an array-backed string. The result builder is
        /// pre-allocated with the exact result length, so the successful path does
        /// not grow or reallocate its buffer.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the requested result length exceeds <see cref="Array.MaxLength"/> characters.
        /// </exception>
        public static string Repeat(this string? source, int count)
        {
            if (count <= 0 || source is null || source.Length == 0)
            {
                return string.Empty;
            }

            long resultLength = (long)source.Length * count;

            if (resultLength > Array.MaxLength)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count),
                    count,
                    $"Repeating the string {count} times would produce a string of {resultLength} characters, which exceeds the maximum supported length of {Array.MaxLength} characters.");
            }

            StringBuilder result = new((int)resultLength);

            for (int i = 0; i < count; i++)
            {
                result.Append(source);
            }

            return result.ToString();
        }

        /// <summary>
        /// Replaces the first occurrence of a specified string in the source string with a new value.
        /// </summary>
        /// <param name="source">The source string in which to perform the replacement.</param>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string to replace the old value with. If null, it will be replaced with an empty string.</param>
        /// <returns>A new string with the first occurrence of the old value replaced by the new value.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source or oldValue is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the old value is an empty string.</exception>
        public static string ReplaceFirst(this string source, string oldValue, string? newValue)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(oldValue);

            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("String to replace cannot be null or empty.", nameof(oldValue));
            }

            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            newValue ??= string.Empty;

            int index = source.IndexOf(oldValue, StringComparison.Ordinal);

            if (index < 0)
            {
                return source;
            }

            return string.Concat(
                source.AsSpan(0, index),
                newValue,
                source.AsSpan(index + oldValue.Length));
        }

        /// <summary>
        /// Removes diacritics from the specified string.
        /// Characters in the custom mapping table are transliterated to their
        /// German-style or Czech-style digraph equivalents; every other character
        /// is decomposed and its non-spacing marks are removed.
        /// </summary>
        /// <param name="source">The source string from which to remove diacritics.</param>
        /// <returns>A new string with diacritics removed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the source string is null.</exception>
        /// <remarks>
        /// Each mapped character is replaced by a digraph whose casing follows its
        /// casing context. A lowercase character yields the lowercase digraph. An
        /// uppercase character yields the uppercase digraph when the adjacent letter
        /// is uppercase, and the title-case digraph otherwise, where the adjacent
        /// letter is the following character when it is a letter, otherwise the
        /// preceding character when it is a letter. The input is traversed once and
        /// the case conversion is culture-invariant; no regular expressions are used.
        /// </remarks>
        public static string RemoveDiacritics(this string source)
        {
            ArgumentNullException.ThrowIfNull(source);

            string transliterated = TransliterateCustomMappings(source);
            string normalisedSource = transliterated.Normalize(NormalizationForm.FormD);
            StringBuilder result = new();

            foreach (char c in normalisedSource)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);

                if (category.NotEquals(UnicodeCategory.NonSpacingMark))
                {
                    result.Append(c);
                }
            }

            return result.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Removes punctuation characters from the specified string.
        /// </summary>
        /// <param name="source">The source string from which to remove punctuation.</param>
        /// <returns>A new string with punctuation characters removed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the source string is null.</exception>
        public static string RemovePunctuation(this string source)
        {
            ArgumentNullException.ThrowIfNull(source);

            StringBuilder result = new();

            foreach (char c in source)
            {
                if (!char.IsPunctuation(c))
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Converts a string to a sentence format.
        /// Underscores and tabs are replaced with spaces, a space is inserted before
        /// each interior ASCII uppercase letter, and runs of consecutive spaces are
        /// collapsed into a single space. Leading and trailing whitespace is removed.
        /// </summary>
        /// <param name="source">The source string to convert.</param>
        /// <returns>A new string formatted as a sentence.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the source string is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the source string is empty.</exception>
        /// <remarks>
        /// Interior uppercase letters are preserved and a space is inserted before each
        /// of them, so "already Sentence." keeps its capital letter. Underscores and tabs
        /// are transliterated to spaces for every character, including the first, and
        /// every emitted space is collapsed into an adjacent one, so the trimmed result
        /// never contains two consecutive spaces and never begins or ends with a space.
        /// </remarks>
        public static string ToSentence(this string source)
        {
            ArgumentNullException.ThrowIfNull(source);

            if (source.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(source));
            }

            StringBuilder sentence = new();
            string charsToReplaceWithSpace = "_\t";

            for (int i = 0; i < source.Length; i++)
            {
                char character = source[i];

                if (i > 0 && character >= 'A' && character <= 'Z')
                {
                    AppendCollapsingSpaces(sentence, ' ');
                    AppendCollapsingSpaces(sentence, character);
                }
                else if (charsToReplaceWithSpace.Contains(character))
                {
                    AppendCollapsingSpaces(sentence, ' ');
                }
                else
                {
                    AppendCollapsingSpaces(sentence, character);
                }
            }

            return sentence.ToString().Trim();
        }

        /// <summary>
        /// Truncates a string to a specified maximum length.
        /// The length is measured in UTF-16 code units, but the cut never splits
        /// a surrogate pair: if the boundary falls between the two halves of a
        /// surrogate pair, the preceding whole character is dropped instead, so
        /// the result never ends with an unpaired surrogate.
        /// </summary>
        /// <param name="value">The string to truncate.</param>
        /// <param name="maxLength">The maximum length of the string after truncation.</param>
        /// <returns>A new string that is truncated to the specified maximum length.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxLength"/> is negative.</exception>
        public static string Truncate(this string value, int maxLength)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (value.Length == 0)
            {
                return value;
            }

            ArgumentOutOfRangeException.ThrowIfNegative(maxLength);

            if (value.Length <= maxLength)
            {
                return value;
            }

            int cutLength = maxLength;

            if (cutLength > 0 && char.IsHighSurrogate(value[cutLength - 1]))
            {
                cutLength--;
            }

            return value[..cutLength];
        }

        /// <summary>
        /// Deserializes a JSON string into an object of the specified type.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to deserialize into.</typeparam>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>An object of type <typeparamref name="TObject"/> represented by the JSON string.</returns>
        /// <throws>JsonException if the JSON string is not valid or does not match the type.</throws>
        public static TObject? FromJson<TObject>(this string json)
            => JsonSerializer.Deserialize<TObject>(json);

        /// <summary>
        /// Deserializes a JSON string into an object of the specified type.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to deserialize into.</typeparam>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <param name="options">Options to control the JSON deserialization.</param>
        /// <returns>An object of type <typeparamref name="TObject"/> represented by the JSON string.</returns>
        /// <throws>JsonException if the JSON string is not valid or does not match the type.</throws>
        public static TObject? FromJson<TObject>(this string json, JsonSerializerOptions options)
            => JsonSerializer.Deserialize<TObject>(json, options);

        private static void AppendCollapsingSpaces(StringBuilder builder, char character)
        {
            if (char.Equals(character, ' ') && builder.Length > 0 && char.Equals(builder[^1], ' '))
            {
                return;
            }

            builder.Append(character);
        }

        private static string TransliterateCustomMappings(string source)
        {
            StringBuilder result = new(source.Length);

            for (int i = 0; i < source.Length; i++)
            {
                string? replacement = GetCustomMappingReplacement(source, i);

                if (replacement is null)
                {
                    result.Append(source[i]);
                }
                else
                {
                    result.Append(replacement);
                }
            }

            return result.ToString();
        }

        private static string? GetCustomMappingReplacement(string source, int index)
        {
            char character = source[index];
            char uppercaseCharacter = char.ToUpperInvariant(character);

            if (!CustomMappings.ContainsKey(uppercaseCharacter))
            {
                return null;
            }

            string mapping = CustomMappings[uppercaseCharacter];

            if (char.IsLower(character))
            {
                return mapping.ToLowerInvariant();
            }

            if (IsInUpperCaseContext(source, index))
            {
                return mapping.ToUpperInvariant();
            }

            return mapping;
        }

        private static bool IsInUpperCaseContext(string source, int index)
        {
            char? adjacentLetter = GetAdjacentLetter(source, index);

            return adjacentLetter is not null && char.IsUpper(adjacentLetter.Value);
        }

        private static char? GetAdjacentLetter(string source, int index)
        {
            if (index + 1 < source.Length && char.IsLetter(source[index + 1]))
            {
                return source[index + 1];
            }

            if (index > 0 && char.IsLetter(source[index - 1]))
            {
                return source[index - 1];
            }

            return null;
        }
    }
}
