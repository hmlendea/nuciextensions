using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

namespace NuciExtensions.ArchitectureTests
{
    [TestFixture]
    public sealed class SourceFileConventionTests
    {
        private static readonly string ProductionDirectory = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "NuciExtensions"));

        [Test]
        public static void GivenProductionSourceFiles_WhenInspectingDeclaredTypes_ThenEachFileHasOneMatchingType()
        {
            string[] sourceFiles = Directory
                .EnumerateFiles(ProductionDirectory, "*.cs", SearchOption.TopDirectoryOnly)
                .Where(file => !file.Contains(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar, StringComparison.Ordinal))
                .Where(file => !file.Contains(Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar, StringComparison.Ordinal))
                .OrderBy(file => file, StringComparer.Ordinal)
                .ToArray();

            Assert.That(sourceFiles, Is.Not.Empty, "No eligible production source files were found.");

            List<string> violations = new();

            foreach (string sourceFile in sourceFiles)
            {
                string source = File.ReadAllText(sourceFile);
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);
                TypeDeclarationSyntax[] declarations = syntaxTree
                    .GetRoot()
                    .DescendantNodes()
                    .OfType<TypeDeclarationSyntax>()
                    .Where(declaration => declaration.Parent is not TypeDeclarationSyntax)
                    .ToArray();

                string fileName = Path.GetFileNameWithoutExtension(sourceFile);
                string[] typeNames = declarations
                    .Select(declaration => declaration.Identifier.ValueText)
                    .OrderBy(name => name, StringComparer.Ordinal)
                    .ToArray();

                if (typeNames.Length != 1 || !string.Equals(typeNames[0], fileName, StringComparison.Ordinal))
                {
                    violations.Add($"{sourceFile}: expected exactly one top-level type named {fileName}, found {string.Join(", ", typeNames)}.");
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public static void GivenProductionSourceFiles_WhenInspectingLanguageFeatures_ThenNoPreviewFeaturesAreUsed()
        {
            string[] sourceFiles = Directory
                .EnumerateFiles(ProductionDirectory, "*.cs", SearchOption.TopDirectoryOnly)
                .OrderBy(file => file, StringComparer.Ordinal)
                .ToArray();

            Assert.That(sourceFiles, Is.Not.Empty, "No eligible production source files were found.");

            List<string> violations = new();

            foreach (string sourceFile in sourceFiles)
            {
                CSharpParseOptions options = new(LanguageVersion.Latest);
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(sourceFile), options, sourceFile);

                foreach (Diagnostic diagnostic in syntaxTree.GetDiagnostics(CancellationToken.None))
                {
                    if (diagnostic.Severity == DiagnosticSeverity.Error && diagnostic.Id is "CS8652" or "CS9202")
                    {
                        violations.Add($"{sourceFile}:{diagnostic.Location.GetLineSpan().StartLinePosition.Line + 1}: {diagnostic.GetMessage()}");
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }
    }
}
