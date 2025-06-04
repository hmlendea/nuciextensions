using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace NuciExtensions
{
    /// <summary>
    /// Enumeration extensions.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the display name of the enumeration item.
        /// </summary>
        /// <returns>The display name string.</returns>
        /// <param name="value">Enumeration item.</param>
        public static string GetDisplayName(this Enum value)
        {
            DisplayAttribute displayAttribute = value
                .GetType()
                .GetMember(value.ToString())
                .FirstOrDefault()
                .GetCustomAttribute<DisplayAttribute>();

            if (displayAttribute is not null)
            {
                return displayAttribute.GetName();
            }

            return value.ToString();
        }
    }
}
