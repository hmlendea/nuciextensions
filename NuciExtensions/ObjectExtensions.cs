using System.Text.Json;

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

        /// <summary>
        /// Converts an object to its JSON representation.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to convert.</typeparam>
        /// <param name="obj">The object to convert to JSON.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string ToJson<TObject>(this TObject obj)
            => JsonSerializer.Serialize(obj);

        /// <summary>
        /// Converts an object to its JSON representation.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to convert.</typeparam>
        /// <param name="obj">The object to convert to JSON.</param>
        /// <param name="options">Options to control the JSON serialization.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string ToJson<TObject>(this TObject obj, JsonSerializerOptions options)
            => JsonSerializer.Serialize(obj, options);
    }
}
