using System;
using System.Linq;
using System.Reflection;

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.NUnit;

using NUnit.Framework;

using ReflectionAssembly = System.Reflection.Assembly;
using ReflectionType = System.Type;

using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NuciExtensions.ArchitectureTests
{
    [TestFixture]
    public sealed class ProductionArchitectureTests
    {
        private static readonly ReflectionAssembly ProductionAssembly = typeof(NuciExtensions.DateTimeExtensions).Assembly;

        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssemblies(ProductionAssembly)
            .Build();

        [Test]
        public static void GivenTheProductionAssembly_WhenInspectingItsTypes_ThenAllPublicTypesAreInTheRootNamespace()
        {
            ReflectionType[] productionTypes = ProductionAssembly
                .GetExportedTypes()
                .OrderBy(type => type.FullName, StringComparer.Ordinal)
                .ToArray();

            Assert.That(productionTypes, Is.Not.Empty, "The production assembly contains no public types.");
            Assert.That(
                productionTypes.All(type => string.Equals(type.Namespace, "NuciExtensions", StringComparison.Ordinal)),
                Is.True,
                "Every public production type must be declared in the NuciExtensions namespace.");
        }

        [Test]
        public static void GivenTheProductionAssembly_WhenInspectingItsPublicTypes_ThenEveryTypeIsASealedStaticClass()
        {
            ReflectionType[] productionTypes = ProductionAssembly
                .GetExportedTypes()
                .OrderBy(type => type.FullName, StringComparer.Ordinal)
                .ToArray();

            Assert.That(productionTypes, Is.Not.Empty, "The production assembly contains no public types.");
            Assert.That(
                productionTypes.All(type => type.IsClass && type.IsAbstract && type.IsSealed),
                Is.True,
                "Every public production type must be a sealed static class.");
        }

        [Test]
        public static void GivenTheProductionArchitecture_WhenCheckingDependencies_ThenProductionTypesDoNotDependOnTestTypes()
        {
            ReflectionType[] productionTypes = ProductionAssembly
                .GetExportedTypes()
                .OrderBy(type => type.FullName, StringComparer.Ordinal)
                .ToArray();

            Assert.That(productionTypes, Is.Not.Empty, "The production assembly contains no public types.");

            IArchRule productionMustNotDependOnTests = ArchUnitNET.Fluent.ArchRuleDefinition
                .Types()
                .That()
                .ResideInAssembly(ProductionAssembly)
                .Should()
                .NotDependOnAny(ArchUnitNET.Fluent.ArchRuleDefinition.Types().That().ResideInNamespace("NuciExtensions.UnitTests"))
                .Because("the production library must not depend on its test project");

            ArchUnitNET.NUnit.ArchRuleExtensions.Check(productionMustNotDependOnTests, Architecture);
        }

        [Test]
        public static void GivenTheProductionAssembly_WhenInspectingReferences_ThenNoTestAssemblyIsReferenced()
        {
            System.Reflection.AssemblyName[] references = ProductionAssembly.GetReferencedAssemblies();

            Assert.That(
                references.Any(reference => reference.Name is not null && reference.Name.Contains("UnitTests", StringComparison.Ordinal)),
                Is.False,
                "The production project must not reference a test assembly.");
        }
    }
}
