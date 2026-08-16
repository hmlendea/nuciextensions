using System;

namespace NuciExtensions.UnitTests.Helpers
{
    public sealed class DummyTestObject : IEquatable<DummyTestObject>
    {
        public string StringProperty { get; set; } = string.Empty;

        public int IntProperty { get; set; }

        public bool Equals(DummyTestObject? other)
        {
            if (other is null)
            {
                return false;
            }

            return
                string.Equals(StringProperty, other.StringProperty, StringComparison.Ordinal) &&
                IntProperty == other.IntProperty;
        }

        public override bool Equals(object? obj) => Equals(obj as DummyTestObject);

        public override int GetHashCode() => HashCode.Combine(StringProperty, IntProperty);
    }
}
