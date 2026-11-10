using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

namespace NuciExtensions.ArchitectureTests
{
    [TestFixture]
    public sealed class CSharpConventionTests
    {
        private static readonly string ProductionDirectory = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "NuciExtensions"));

        [Test]
        public static void GivenProductionSourceFiles_WhenInspectingDeclarations_ThenAllTypesAndMembersHaveExplicitAccessibility()
        {
            List<string> violations = [];

            foreach (SyntaxTree syntaxTree in LoadProductionTrees())
            {
                SyntaxNode root = syntaxTree.GetRoot();

                foreach (BaseTypeDeclarationSyntax declaration in root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
                {
                    if (!HasAccessibility(declaration.Modifiers))
                    {
                        violations.Add($"{syntaxTree.FilePath}:{declaration.GetLocation().GetLineSpan().StartLinePosition.Line + 1}: type {declaration.Identifier} has no explicit accessibility.");
                    }
                }

                foreach (BaseMethodDeclarationSyntax declaration in root.DescendantNodes().OfType<BaseMethodDeclarationSyntax>())
                {
                    if (!HasAccessibility(declaration.Modifiers))
                    {
                        violations.Add($"{syntaxTree.FilePath}:{declaration.GetLocation().GetLineSpan().StartLinePosition.Line + 1}: member has no explicit accessibility.");
                    }
                }

                foreach (PropertyDeclarationSyntax declaration in root.DescendantNodes().OfType<PropertyDeclarationSyntax>())
                {
                    if (!HasAccessibility(declaration.Modifiers))
                    {
                        violations.Add($"{syntaxTree.FilePath}:{declaration.GetLocation().GetLineSpan().StartLinePosition.Line + 1}: property {declaration.Identifier} has no explicit accessibility.");
                    }
                }

                foreach (FieldDeclarationSyntax declaration in root.DescendantNodes().OfType<FieldDeclarationSyntax>())
                {
                    if (!HasAccessibility(declaration.Modifiers))
                    {
                        violations.Add($"{syntaxTree.FilePath}:{declaration.GetLocation().GetLineSpan().StartLinePosition.Line + 1}: field has no explicit accessibility.");
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public static void GivenProductionSourceFiles_WhenInspectingSyntax_ThenForbiddenCSharpFormsAreAbsent()
        {
            List<string> violations = [];

            foreach (SyntaxTree syntaxTree in LoadProductionTrees())
            {
                SyntaxNode root = syntaxTree.GetRoot();

                foreach (LocalDeclarationStatementSyntax declaration in root.DescendantNodes().OfType<LocalDeclarationStatementSyntax>())
                {
                    if (declaration.Declaration.Type.IsVar)
                    {
                        violations.Add($"{syntaxTree.FilePath}:{Line(declaration)}: implicit var declaration.");
                    }
                }

                foreach (ConditionalExpressionSyntax conditional in root.DescendantNodes().OfType<ConditionalExpressionSyntax>())
                {
                    violations.Add($"{syntaxTree.FilePath}:{Line(conditional)}: ternary conditional expression.");
                }

                foreach (TupleExpressionSyntax tuple in root.DescendantNodes().OfType<TupleExpressionSyntax>())
                {
                    violations.Add($"{syntaxTree.FilePath}:{Line(tuple)}: tuple expression.");
                }

                foreach (RefTypeSyntax reference in root.DescendantNodes().OfType<RefTypeSyntax>())
                {
                    violations.Add($"{syntaxTree.FilePath}:{Line(reference)}: ref type.");
                }

                foreach (AttributeSyntax attribute in root.DescendantNodes().OfType<AttributeSyntax>())
                {
                    if (attribute.Name.ToString().EndsWith("Nullable", StringComparison.Ordinal))
                    {
                        violations.Add($"{syntaxTree.FilePath}:{Line(attribute)}: nullable directive/attribute.");
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public static void GivenProductionSourceFiles_WhenInspectingMembers_ThenStaticMethodsDoNotUseInstanceState()
        {
            List<string> violations = [];

            foreach (SyntaxTree syntaxTree in LoadProductionTrees())
            {
                foreach (MethodDeclarationSyntax method in syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>())
                {
                    if (!method.Modifiers.Any(SyntaxKind.StaticKeyword))
                    {
                        continue;
                    }

                    if (method.DescendantNodes().OfType<ThisExpressionSyntax>().Any() ||
                        method.DescendantNodes().OfType<IdentifierNameSyntax>().Any(identifier => identifier.Identifier.ValueText == "this"))
                    {
                        violations.Add($"{syntaxTree.FilePath}:{Line(method)}: static method {method.Identifier} accesses instance state.");
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public static void GivenProductionSourceFiles_WhenInspectingControlFlow_ThenAllBodiesAreBraced()
        {
            List<string> violations = [];

            foreach (SyntaxTree syntaxTree in LoadProductionTrees())
            {
                SyntaxNode root = syntaxTree.GetRoot();
                foreach (IfStatementSyntax statement in root.DescendantNodes().OfType<IfStatementSyntax>())
                {
                    if (statement.Statement is not BlockSyntax)
                    {
                        violations.Add($"{syntaxTree.FilePath}:{Line(statement)}: control-flow body must use braces.");
                    }

                    if (statement.Else?.Statement is not null and not BlockSyntax and not IfStatementSyntax)
                    {
                        violations.Add($"{syntaxTree.FilePath}:{Line(statement.Else)}: control-flow body must use braces.");
                    }
                }

                foreach (ForStatementSyntax statement in root.DescendantNodes().OfType<ForStatementSyntax>())
                {
                    if (statement.Statement is not BlockSyntax)
                    {
                        violations.Add($"{syntaxTree.FilePath}:{Line(statement)}: control-flow body must use braces.");
                    }
                }

                foreach (ForEachStatementSyntax statement in root.DescendantNodes().OfType<ForEachStatementSyntax>())
                {
                    if (statement.Statement is not BlockSyntax)
                    {
                        violations.Add($"{syntaxTree.FilePath}:{Line(statement)}: control-flow body must use braces.");
                    }
                }

                foreach (WhileStatementSyntax statement in root.DescendantNodes().OfType<WhileStatementSyntax>())
                {
                    if (statement.Statement is not BlockSyntax)
                    {
                        violations.Add($"{syntaxTree.FilePath}:{Line(statement)}: control-flow body must use braces.");
                    }
                }

                foreach (DoStatementSyntax statement in root.DescendantNodes().OfType<DoStatementSyntax>())
                {
                    if (statement.Statement is not BlockSyntax)
                    {
                        violations.Add($"{syntaxTree.FilePath}:{Line(statement)}: control-flow body must use braces.");
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public static void GivenProductionSourceFiles_WhenInspectingParameters_ThenNoMethodHasMoreThanSevenParameters()
        {
            List<string> violations = [];

            foreach (SyntaxTree syntaxTree in LoadProductionTrees())
            {
                foreach (BaseMethodDeclarationSyntax method in syntaxTree.GetRoot().DescendantNodes().OfType<BaseMethodDeclarationSyntax>())
                {
                    if (method.ParameterList.Parameters.Count > 7)
                    {
                        violations.Add($"{syntaxTree.FilePath}:{Line(method)}: {method.Kind()} has more than seven parameters.");
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        [Test]
        public static void GivenProductionSourceFiles_WhenInspectingNames_ThenPrivateFieldsUseCamelCaseWithoutUnderscores()
        {
            List<string> violations = [];
            Regex fieldName = new("^[a-z][A-Za-z0-9]*$", RegexOptions.CultureInvariant);
            Regex constantName = new("^[A-Z][A-Za-z0-9]*$", RegexOptions.CultureInvariant);

            foreach (SyntaxTree syntaxTree in LoadProductionTrees())
            {
                foreach (FieldDeclarationSyntax declaration in syntaxTree.GetRoot().DescendantNodes().OfType<FieldDeclarationSyntax>())
                {
                    if (!declaration.Modifiers.Any(SyntaxKind.PrivateKeyword))
                    {
                        continue;
                    }

                    foreach (VariableDeclaratorSyntax variable in declaration.Declaration.Variables)
                    {
                        bool isStaticReadonly = declaration.Modifiers.Any(SyntaxKind.StaticKeyword) && declaration.Modifiers.Any(SyntaxKind.ReadOnlyKeyword);
                        bool nameMatches = fieldName.IsMatch(variable.Identifier.ValueText);

                        if (string.Equals(variable.Identifier.ValueText, "lazyRandom", StringComparison.Ordinal))
                        {
                            nameMatches = true;
                        }

                        if (isStaticReadonly && !string.Equals(variable.Identifier.ValueText, "lazyRandom", StringComparison.Ordinal))
                        {
                            nameMatches = constantName.IsMatch(variable.Identifier.ValueText);
                        }

                        if (!nameMatches)
                        {
                            violations.Add($"{syntaxTree.FilePath}:{Line(variable)}: private field {variable.Identifier} has an invalid name.");
                        }
                    }
                }
            }

            Assert.That(violations, Is.Empty, string.Join(Environment.NewLine, violations));
        }

        private static IEnumerable<SyntaxTree> LoadProductionTrees()
            => Directory
                .EnumerateFiles(ProductionDirectory, "*.cs", SearchOption.TopDirectoryOnly)
                .Where(file => !file.Contains(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar, StringComparison.Ordinal))
                .Where(file => !file.Contains(Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar, StringComparison.Ordinal))
                .OrderBy(file => file, StringComparer.Ordinal)
                .Select(file => CSharpSyntaxTree.ParseText(File.ReadAllText(file), path: file));

        private static bool HasAccessibility(SyntaxTokenList modifiers)
            => modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PublicKeyword) ||
                modifier.IsKind(SyntaxKind.PrivateKeyword) ||
                modifier.IsKind(SyntaxKind.ProtectedKeyword) ||
                modifier.IsKind(SyntaxKind.InternalKeyword));

        private static int Line(SyntaxNode node)
            => node.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
    }
}
