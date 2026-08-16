using System.ComponentModel.DataAnnotations;

namespace NuciExtensions.UnitTests.Helpers
{
    public enum DummyDisplayEnum
    {
        [Display(Name = "Praise the Sun!")]
        ValueWithDisplayName,

        ValueWithoutDisplayName,
    }
}