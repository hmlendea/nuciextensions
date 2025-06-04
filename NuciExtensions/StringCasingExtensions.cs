namespace NuciExtensions
{
    public static class StringCasingExtensions
    {
        /// <summary>
        /// Gets the duplicated elements.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <returns>The duplicated elements.</returns>
        public static string ToTitleCase(this string source)
        {
            char[] chars = source.ToLower().ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 0 || !char.IsLetterOrDigit(chars[i - 1]))
                {
                    chars[i] = char.ToUpper(chars[i]);
                }
            }

            return new string(chars);
        }

        public static string ToSentanceCase(this string source)
        {
            char[] chars = source.ToCharArray();

            bool isNewSentance = true;
            for (int i = 0; i < source.Length; i++)
            {
                if (isNewSentance && char.IsLetter(chars[i]))
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

        public static string ToUpperSnakeCase(this string source)
            => source.ToSnakeCase().ToUpper();

        public static string ToLowerSnakeCase(this string source)
            => source.ToSnakeCase().ToLower();

        public static string ToSnakeCase(this string source)
        {
            string result = string.Empty;

            foreach (char c in source)
            {
                if (char.IsLetterOrDigit(c))
                {
                    result += c;
                }
                else if (char.IsWhiteSpace(c))
                {
                    result += '_';
                }
            }

            while (result.Contains("__"))
            {
                result = result.Replace("__", "_");
            }

            return result;
        }
    }
}
