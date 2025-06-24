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
                if (i.Equals(0) || !char.IsLetterOrDigit(chars[i - 1]))
                {
                    chars[i] = char.ToUpper(chars[i]);
                }
            }

            return new string(chars);
        }

        /// <summary>
        /// Converts the first letter of each sentence to uppercase.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <returns>A new string with the first letter of each sentence in uppercase.</returns>
        public static string ToSentenceCase(this string source)
        {
            char[] chars = source.ToCharArray();
            bool isNewSentence = true;

            for (int i = 0; i < source.Length; i++)
            {
                if (isNewSentence && char.IsLetter(chars[i]))
                {
                    chars[i] = char.ToUpper(chars[i]);
                    isNewSentence = false;
                }
                else if (chars[i].Equals('.'))
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
        /// <param name="source">The source string.</param>
        /// <returns>A new string in upper snake case.</returns>
        public static string ToUpperSnakeCase(this string source)
            => source.ToSnakeCase().ToUpper();

        /// <summary>
        /// Converts the string to lower snake case.
        /// Lower snake case means that all letters are lowercase and words are separated by underscores.
        /// For example, "Hello World" becomes "hello_world".
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <returns>A new string in lower snake case.</returns>
        public static string ToLowerSnakeCase(this string source)
            => source.ToSnakeCase().ToLower();

        /// <summary>
        /// Converts the string to snake case.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <returns>A new string in snake case.</returns>
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
