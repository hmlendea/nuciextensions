using System;

namespace NuciExtensions.UnitTests.Helpers
{
    public class DummyTestObject : IEquatable<DummyTestObject>
    {
        public string StringProperty { get; set; }

        public int IntProperty { get; set; }

        public bool Equals(DummyTestObject other)
        {
            if (other is null)
            {
                return false;
            }

            return
                StringProperty == other.StringProperty &&
                IntProperty == other.IntProperty;
        }

        public override bool Equals(object obj) => Equals(obj as DummyTestObject);

        public override int GetHashCode() => HashCode.Combine(StringProperty, IntProperty);
    }
}
