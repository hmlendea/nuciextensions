namespace NuciExtensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Inverts the case of each character in the specified string.
        /// </summary>
        /// <param name="obj">The string whose case is to be inverted.</param>
        /// <returns>A new string with each character's case inverted.</returns>
        public static bool NotEquals<TObject>(this TObject self, TObject other)
            => !self.Equals(other);
    }
}
