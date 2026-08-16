using System;
using System.IO;

using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    [TestFixture]
    [NonParallelizable]
    public sealed class FileExtensionsTests
    {
        private string originalPathVariable = null!;
        private DirectoryInfo temporaryDirectory = null!;

        [SetUp]
        public void SetUp()
        {
            originalPathVariable = Environment.GetEnvironmentVariable("PATH")!;
            temporaryDirectory = Directory.CreateTempSubdirectory();
            Environment.SetEnvironmentVariable("PATH", temporaryDirectory.FullName);
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable("PATH", originalPathVariable);
            temporaryDirectory.Delete(true);
        }

        [Test]
        public void GivenAFileInThePathVariable_WhenCheckingWhetherItExists_ThenTrueIsReturned()
        {
            string fileName = Path.GetRandomFileName();
            File.WriteAllText(
                Path.Combine(temporaryDirectory.FullName, fileName),
                string.Empty);

            Assert.That(FileExtensions.ExistsInPathVariable(fileName));
        }

        [Test]
        public void GivenAnExistingFilePath_WhenCheckingWhetherItExists_ThenTrueIsReturned()
        {
            string filePath = Path.Combine(
                temporaryDirectory.FullName,
                Path.GetRandomFileName());
            File.WriteAllText(filePath, string.Empty);

            Assert.That(FileExtensions.ExistsInPathVariable(filePath));
        }

        [Test]
        public void GivenAMissingFile_WhenCheckingWhetherItExists_ThenFalseIsReturned()
        {
            string fileName = Path.GetRandomFileName();

            Assert.That(
                FileExtensions.ExistsInPathVariable(fileName),
                Is.False);
        }

        [Test]
        public void GivenAnEmptyPathVariable_WhenCheckingForAMissingFile_ThenFalseIsReturned()
        {
            string fileName = Path.GetRandomFileName();
            Environment.SetEnvironmentVariable("PATH", string.Empty);

            Assert.That(
                FileExtensions.ExistsInPathVariable(fileName),
                Is.False);
        }

        [Test]
        public void GivenANullPathVariable_WhenCheckingForAMissingFile_ThenANullReferenceExceptionIsThrown()
        {
            string fileName = Path.GetRandomFileName();
            Environment.SetEnvironmentVariable("PATH", null);

            Assert.That(
                () => FileExtensions.ExistsInPathVariable(fileName),
                Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void GivenANullFileName_WhenCheckingWhetherItExists_ThenAnArgumentNullExceptionIsThrown()
        {
            string fileName = null!;

            Assert.That(
                () => FileExtensions.ExistsInPathVariable(fileName),
                Throws.TypeOf<ArgumentNullException>());
        }
    }
}
