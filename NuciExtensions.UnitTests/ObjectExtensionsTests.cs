using System;
using System.Text.Json;

using NUnit.Framework;

using NuciExtensions.UnitTests.Helpers;

namespace NuciExtensions.UnitTests
{
    [TestFixture]
    public sealed class ObjectExtensionsTests
    {
        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        [Test]
        [TestCase(4, 4, false)]
        [TestCase(4, 8, true)]
        [TestCase("Minecraft", "Minecraft", false)]
        [TestCase("Minecraft", "Terraria", true)]
        [TestCase("Minecraft", null, true)]
        public void GivenAnObject_WhenCallingNotEquals_ThenTheExpectedValueIsReturned(
            object object1,
            object object2,
            bool expected)
            => Assert.That(object1.NotEquals(object2), Is.EqualTo(expected));

        [Test]
        [TestCase(null, null)]
        [TestCase(null, "Minecraft")]
        [TestCase(null, 4)]
        public void GivenANullObject_WhenCallingNotEquals_ThenANullReferenceExceptionIsThrown(
            object object1,
            object object2)
            => Assert.That(
                () => object1.NotEquals(object2),
                Throws.TypeOf<NullReferenceException>());

        [Test]
        public void GivenAnObject_WhenCallingToJson_ThenTheExpectedValueIsReturned()
        {
            DummyTestObject dummyObject = new()
            {
                StringProperty = "Minecraft",
                IntProperty = 4,
            };

            Assert.That(
                dummyObject.ToJson(),
                Is.EqualTo(
                    $"{{\"{nameof(DummyTestObject.StringProperty)}\":\"Minecraft\"," +
                    $"\"{nameof(DummyTestObject.IntProperty)}\":4}}"));
        }

        [Test]
        public void GivenAnObjectAndCustomOptions_WhenCallingToJson_ThenTheConfiguredJsonIsReturned()
        {
            DummyTestObject dummyObject = new()
            {
                StringProperty = "Minecraft",
                IntProperty = 4,
            };

            Assert.That(
                dummyObject.ToJson(jsonOptions),
                Is.EqualTo("{\"stringProperty\":\"Minecraft\",\"intProperty\":4}"));
        }

        [Test]
        public void GivenANullObject_WhenCallingToJson_ThenTheJsonNullLiteralIsReturned()
        {
            DummyTestObject dummyObject = null!;

            Assert.That(dummyObject.ToJson(), Is.EqualTo("null"));
        }

        [Test]
        public void GivenANullObjectAndCustomOptions_WhenCallingToJson_ThenTheJsonNullLiteralIsReturned()
        {
            DummyTestObject dummyObject = null!;

            Assert.That(dummyObject.ToJson(jsonOptions), Is.EqualTo("null"));
        }
    }
}
