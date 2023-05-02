using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    [TestFixture]
    public class FileExtensionsTests
    {
        [Test]
        public void ExistsInPathVariable_ReturnsTrue_ForExecutableInPath()
        {
            var executableFileName = "dotnet";
            var result = FileExtensions.ExistsInPathVariable(executableFileName);

            Assert.That(result, Is.True, $"The '{executableFileName}' executable should exist in the PATH environment variable.");
        }

        [Test]
        public void ExistsInPathVariable_ReturnsFalse_ForNonExistingExecutable()
        {
            var executableFileName = "this-does-not-exist";
            var result = FileExtensions.ExistsInPathVariable(executableFileName);

            Assert.That(result, Is.False, $"The '{executableFileName}' executable should not exist in the PATH environment variable.");
        }
    }
}
