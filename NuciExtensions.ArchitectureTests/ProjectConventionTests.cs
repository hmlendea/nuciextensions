using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using NUnit.Framework;

namespace NuciExtensions.ArchitectureTests
{
    [TestFixture]
    public sealed class ProjectConventionTests
    {
        private static readonly string RepositoryDirectory = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));

        [Test]
        public static void GivenProductionProjectFiles_WhenInspectingProjectSettings_ThenRequiredSettingsAreDeclared()
        {
            string[] projectFiles = Directory
                .EnumerateFiles(RepositoryDirectory, "*.csproj", SearchOption.AllDirectories)
                .Where(file => !file.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
                .Where(file => !file.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
                .OrderBy(file => file, StringComparer.Ordinal)
                .ToArray();

            Assert.That(projectFiles, Is.Not.Empty, "No project files were found.");

            foreach (string projectFile in projectFiles)
            {
                XElement project = XDocument.Load(projectFile).Root!;
                XNamespace msbuild = project.GetDefaultNamespace();
                XElement propertyGroup = project.Elements(msbuild + "PropertyGroup").First();

                Assert.That(propertyGroup.Element(msbuild + "TargetFramework")?.Value, Is.EqualTo("net10.0"), projectFile);
                Assert.That(propertyGroup.Element(msbuild + "ImplicitUsings")?.Value, Is.EqualTo("disable"), projectFile);
                Assert.That(propertyGroup.Element(msbuild + "LangVersion")?.Value, Is.EqualTo("latest"), projectFile);
                Assert.That(propertyGroup.Element(msbuild + "Nullable")?.Value, Is.EqualTo("enable"), projectFile);
                Assert.That(propertyGroup.Element(msbuild + "IsPackable")?.Value, Is.Not.Null, projectFile);
            }
        }

        [Test]
        public static void GivenArchitectureProject_WhenInspectingTestSettings_ThenCodeCoverageIsExcluded()
        {
            string projectFile = Path.Combine(
                RepositoryDirectory,
                "NuciExtensions.ArchitectureTests",
                "NuciExtensions.ArchitectureTests.csproj");
            XElement project = XDocument.Load(projectFile).Root!;
            XNamespace msbuild = project.GetDefaultNamespace();
            XElement propertyGroup = project.Elements(msbuild + "PropertyGroup").First();

            Assert.That(propertyGroup.Element(msbuild + "IsTestProject")?.Value, Is.EqualTo("true"));
            Assert.That(propertyGroup.Element(msbuild + "ExcludeFromCodeCoverage")?.Value, Is.EqualTo("true"));
        }
    }
}
