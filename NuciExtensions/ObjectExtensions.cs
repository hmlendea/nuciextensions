using System.Text.Json;

namespace NuciExtensions
{
    /// <summary>
    /// Provides extension methods for general object operations.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Determines whether the specified object is not equal to another object.
        /// </summary>
        /// <typeparam name="TObject">The type of the objects.</typeparam>
        /// <param name="self">The first object to compare.</param>
        /// <param name="other">The second object to compare.</param>
        /// <returns>True if the objects are not equal; otherwise, false.</returns>
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
