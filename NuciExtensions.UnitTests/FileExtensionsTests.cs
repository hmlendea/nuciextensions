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

            Assert.That(
                FileExtensions.ExistsInPathVariable(executableFileName),
                Is.True,
                $"The '{executableFileName}' executable should exist in the PATH environment variable.");
        }

        [Test]
        public void ExistsInPathVariable_ReturnsFalse_ForNonExistingExecutable()
        {
            var executableFileName = "this-does-not-exist";

            Assert.That(
                FileExtensions.ExistsInPathVariable(executableFileName),
                Is.False,
                $"The '{executableFileName}' executable should not exist in the PATH environment variable.");
        }
    }
}
