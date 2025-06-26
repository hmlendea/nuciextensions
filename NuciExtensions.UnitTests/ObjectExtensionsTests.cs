using System;
using System.Text.Json;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NuciExtensions.UnitTests.Helpers;
using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    public class ObjectExtensionsTests
    {
        [Test]
        [TestCase(1, 1, false)]
        [TestCase(1, 2, true)]
        [TestCase("test", "test", false)]
        [TestCase("test", "this", true)]
        [TestCase("test", null, true)]
        public void GivenAnObject_WhenCallingNotEquals_ThenTheExpectedValueIsReturned(
            object object1,
            object object2,
            bool expected)
            => Assert.That(object1.NotEquals(object2), Is.EqualTo(expected));

        [Test]
        [TestCase(null, null)]
        [TestCase(null, "test")]
        [TestCase(null, 1)]
        public void GivenANullObject_WhenCallingNotEquals_ThenNullReferenceExceptionIsThrown(
            object object1,
            object object2)
            => Assert.Throws<NullReferenceException>(() => object1.NotEquals(object2));

        [Test]
        public void GivenAnObject_WhenCallingToJson_ThenTheExpectedValueIsReturned()
        {
            DummyTestObject obj = new()
            {
                StringProperty = "test",
                IntProperty = 1
            };

            Assert.That(obj.ToJson(), Is.EqualTo($"{{\"{nameof(DummyTestObject.StringProperty)}\":\"test\",\"{nameof(DummyTestObject.IntProperty)}\":1}}"));
        }
    }
}
