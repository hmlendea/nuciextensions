using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace NuciExtensions
{
    /// <summary>
    /// Provides extension methods for enumeration operations.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the display name of the enumeration item.
        /// </summary>
        /// <returns>The display name string.</returns>
        /// <param name="value">Enumeration item.</param>
        /// <exception cref="ArgumentNullException">Thrown if the value is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the value is not a valid enumeration value.</exception>
        public static string GetDisplayName(this Enum value)
        {
            ArgumentNullException.ThrowIfNull(value);

            MemberInfo? member = value
                .GetType()
                .GetMember(value.ToString())
                .FirstOrDefault();

            if (member is null)
            {
                throw new ArgumentException("The specified value is not a valid enumeration value.", nameof(value));
            }

            DisplayAttribute? displayAttribute = member.GetCustomAttribute<DisplayAttribute>();

            if (displayAttribute is not null)
            {
                return displayAttribute.GetName() ?? value.ToString();
            }

            return value.ToString();
        }
    }
}
