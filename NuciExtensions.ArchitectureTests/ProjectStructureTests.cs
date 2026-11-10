using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

namespace NuciExtensions.ArchitectureTests
{
    [TestFixture]
    public sealed class ProjectStructureTests
    {
        private static readonly string ProductionDirectory = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "NuciExtensions"));

        [Test]
        public static void GivenProductionSourceFiles_WhenInspectingTopLevelConstructs_ThenAllTypesAreInsideTypeBlocks()
        {
            List<string> violations = [];

            foreach (string file in Directory
                         .EnumerateFiles(ProductionDirectory, "*.cs", SearchOption.TopDirectoryOnly)
                         .OrderBy(file => file, StringComparer.Ordinal))
            {
                Microsoft.CodeAnalysis.SyntaxTree syntaxTree =
                    Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(File.ReadAllText(file), path: file);

                NamespaceDeclarationSyntax? namespaceDeclaration =
                    syntaxTree.GetRoot()
                        .DescendantNodes()
                        .OfType<NamespaceDeclarationSyntax>()
                        .FirstOrDefault();

                Assert.That(namespaceDeclaration, Is.Not.Null, $"{Path.GetFileName(file)} has no namespace block.");

                TypeDeclarationSyntax[] namedTypes = namespaceDeclaration!
                    .DescendantNodes()
                    .OfType<TypeDeclarationSyntax>()
                    .Select(node => node)
                    .ToArray();

                Assert.That(namedTypes.Length, Is.EqualTo(1), $"{Path.GetFileName(file)} must contain exactly one type.");
            }
        }

        [Test]
        public static void GivenProductionSourceFiles_WhenInspectingConstantAndPartialDeclarations_ThenConstAndPartialAreBanned()
        {
            List<string> violations = [];

            foreach (string file in Directory
                         .EnumerateFiles(ProductionDirectory, "*.cs", SearchOption.TopDirectoryOnly)
                         .OrderBy(file => file, StringComparer.Ordinal))
            {
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(file), path: file);

                foreach (FieldDeclarationSyntax field in syntaxTree.GetRoot().DescendantNodes().OfType<FieldDeclarationSyntax>())
                {
                    if (field.Modifiers.Any(SyntaxKind.ConstKeyword))
                    {
                        violations.Add($"{Path.GetFileName(file)}:{field.GetLocation().GetLineSpan().StartLinePosition.Line + 1}: const field banned.");
                    }
                }

                foreach (BaseTypeDeclarationSyntax type in syntaxTree.GetRoot().DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
                {
                    if (type.Modifiers.Any(SyntaxKind.PartialKeyword))
                    {
                        violations.Add($"{Path.GetFileName(file)}:{type.GetLocation().GetLineSpan().StartLinePosition.Line + 1}: partial type banned.");
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public static void GivenProductionSourceFiles_WhenInspectingConstants_ThenConstantsAreStaticReadOnly()
        {
            List<string> violations = [];

            foreach (string file in Directory
                         .EnumerateFiles(ProductionDirectory, "*.cs", SearchOption.TopDirectoryOnly)
                         .OrderBy(file => file, StringComparer.Ordinal))
            {
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(file), path: file);

                foreach (FieldDeclarationSyntax field in syntaxTree.GetRoot().DescendantNodes().OfType<FieldDeclarationSyntax>())
                {
                    if (field.Modifiers.Any(SyntaxKind.StaticKeyword) &&
                        field.Modifiers.Any(SyntaxKind.ReadOnlyKeyword))
                    {
                        continue;
                    }

                    violations.Add($"{Path.GetFileName(file)}:{field.GetLocation().GetLineSpan().StartLinePosition.Line + 1}: field is not static readonly.");
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public static void GivenProductionSourceFiles_WhenInspectingUsingDirectives_ThenNoUsingStaticOrDirectives()
        {
            List<string> violations = [];

            foreach (string file in Directory
                         .EnumerateFiles(ProductionDirectory, "*.cs", SearchOption.TopDirectoryOnly)
                         .OrderBy(file => file, StringComparer.Ordinal))
            {
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(file), path: file);

                foreach (UsingDirectiveSyntax usingDirective in syntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>())
                {
                    if (usingDirective.StaticKeyword.IsKind(SyntaxKind.StaticKeyword))
                    {
                        violations.Add($"{Path.GetFileName(file)}:{usingDirective.GetLocation().GetLineSpan().StartLinePosition.Line + 1}: using static banned.");
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public static void GivenProductionSourceFiles_WhenInspectingPreprocessorDirectives_ThenNoRegionsOrNullableDirectives()
        {
            List<string> violations = [];

            foreach (string file in Directory
                         .EnumerateFiles(ProductionDirectory, "*.cs", SearchOption.TopDirectoryOnly)
                         .OrderBy(file => file, StringComparer.Ordinal))
            {
                string[] lines = File.ReadAllLines(file);

                for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
                {
                    string line = lines[lineNumber].Trim();

                    if (line.StartsWith("#region", StringComparison.Ordinal) ||
                        line.StartsWith("#endregion", StringComparison.Ordinal))
                    {
                        violations.Add($"{Path.GetFileName(file)}:{lineNumber + 1}: #{line} directive banned.");
                    }

                    if (line.StartsWith("#nullable", StringComparison.Ordinal))
                    {
                        violations.Add($"{Path.GetFileName(file)}:{lineNumber + 1}: nullable directive banned.");
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public static void GivenProductionSourceFiles_WhenInspectingMemberOrdering_ThenMembersAreGroupedByKindAndAccessibility()
        {
            // Mechanical spot-check on the top-level types' member order where feasible.
            // Full enforcement requires semantic ordering groups; this test flags out-of-order accesses.
            List<string> violations = [];

            foreach (string file in Directory
                         .EnumerateFiles(ProductionDirectory, "*.cs", SearchOption.TopDirectoryOnly)
                         .OrderBy(file => file, StringComparer.Ordinal))
            {
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(file), path: file);

                foreach (BaseTypeDeclarationSyntax type in syntaxTree.GetRoot()
                             .DescendantNodes()
                             .OfType<BaseTypeDeclarationSyntax>())
                {
                    List<SyntaxKind> kinds = type
                        .DescendantNodes()
                        .OfType<SyntaxNode>()
                        .Select(node => node.Kind())
                        .ToList();

                    int fieldIndex = kinds.IndexOf(SyntaxKind.FieldDeclaration);
                    int propIndex = kinds.IndexOf(SyntaxKind.PropertyDeclaration);
                    int ctorIndex = kinds.IndexOf(SyntaxKind.ConstructorDeclaration);
                    int methodIndex = kinds.IndexOf(SyntaxKind.MethodDeclaration);

                    int[] indices = new[] { fieldIndex, propIndex, ctorIndex, methodIndex }
                        .Where(index => index >= 0)
                        .ToArray();

                    if (indices.Length > 1 && !indices.SequenceEqual(indices.OrderBy(i => i)))
                    {
                        violations.Add($"{Path.GetFileName(file)}: type {type.Identifier} members not ordered fields->properties->constructors->methods.");
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }
    }
}
