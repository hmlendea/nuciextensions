# Testing Documentation

This document describes the testing structure, conventions, coverage requirements, and how to run tests for NuciExtensions.

## 🧪 Test Project Structure

```
NuciExtensions.UnitTests/
├── DateTimeExtensionsTests.cs
├── DictionaryExtensionsTests.cs
├── EnumerableExtensionsTests.cs
├── EnumerableExtTests.cs
├── EnumExtensionsTests.cs
├── FileExtensionsTests.cs
├── ListExtensionsTests.cs
├── ObjectExtensionsTests.cs
├── StringCasingExtensionsTests.cs
├── StringExtensionsTests.cs
├── Helpers/
│   ├── DummyDisplayEnum.cs
│   └── DummyTestObject.cs
└── NuciExtensions.UnitTests.csproj
```

## 📋 Test Conventions

### Naming Convention

All tests follow the **Given/When/Then** pattern:

```csharp
[Test]
public void Given[Precondition]_When[Action]_Then[Assertion]()
```

Examples:
- `GivenAValidUnixDate_WhenGettingElapsedUnixTime_ThenTheCorrectDurationIsReturned`
- `GivenANullCollection_WhenGettingARandomElement_ThenANullReferenceExceptionIsThrown`
- `GivenAnEmptyString_WhenConvertingToASentence_ThenAnArgumentOutOfRangeExceptionIsThrown`

### Test Attributes

- `[TestFixture]` — Marks a class containing tests
- `[Test]` — Marks a test method
- `[TestCase(...)]` — Parameterized test with multiple inputs
- `[SetUp]` / `[TearDown]` — Per-test initialization/cleanup
- `[NonParallelizable]` — Prevents parallel execution (used for tests modifying global state like PATH)

### Assertions

Uses **NUnit 4** constraint-based assertions:

```csharp
Assert.That(actual, Is.EqualTo(expected));
Assert.That(() => method(), Throws.TypeOf<ExceptionType>());
Assert.That(collection, Is.Empty);
Assert.That(collection, Does.Contain(item));
Assert.That(collection, Is.EquivalentTo(expected)); // Order-independent
Assert.That(collection, Has.Count.EqualTo(expected));
```

## 🎯 Coverage Requirements

### Target: 100% Line and Branch Coverage

| Metric | Target | Current |
|--------|--------|---------|
| Line Coverage | 100% | 100% (293/293 lines) |
| Branch Coverage | 100% | 100% (108/108 branches) |
| Test Count | — | 118 tests |

### Coverage Verification

Run with coverage collection:

```bash
dotnet test NuciExtensions.sln --nologo --collect:"XPlat Code Coverage" --results-directory /tmp/coverage
```

The Cobertura XML report will be generated at:
```
/tmp/coverage/<guid>/coverage.cobertura.xml
```

### Coverage Report Interpretation

```xml
<coverage line-rate="1" branch-rate="1" lines-covered="293" lines-valid="293" branches-covered="108" branches-valid="108">
```

- `line-rate="1"` = 100% line coverage
- `branch-rate="1"` = 100% branch coverage
- Each class shows individual coverage metrics

## 🏃 Running Tests

### Basic Test Run

```bash
dotnet test NuciExtensions.sln --nologo
```

Output:
```
Test summary: total: 118, failed: 0, succeeded: 118, skipped: 0, duration: 0.9s
```

### With Coverage

```bash
dotnet test NuciExtensions.sln --nologo --collect:"XPlat Code Coverage" --results-directory /tmp/coverage
```

### Specific Test Class

```bash
dotnet test NuciExtensions.UnitTests/NuciExtensions.UnitTests.csproj --filter "FullyQualifiedName~StringExtensionsTests"
```

### Verbose Output

```bash
dotnet test NuciExtensions.sln --logger "console;verbosity=detailed"
```

## 📊 Test Categories by Extension Class

| Test Class | Tests | Coverage Focus |
|------------|-------|----------------|
| `DateTimeExtensionsTests` | 5 | Epoch conversion, error cases |
| `DictionaryExtensionsTests` | 5 | Add/update, safe lookup, null keys |
| `EnumerableExtensionsTests` | 12 | Random element (seeded/unseeded), duplicates, empty/null |
| `EnumerableExtTests` | 5 | Null-safe emptiness checks |
| `EnumExtensionsTests` | 4 | Display attribute, fallback, null/undefined |
| `FileExtensionsTests` | 7 | PATH lookup, temp dir isolation, null PATH |
| `ListExtensionsTests` | 8 | Shuffle (preserves elements, count), Pop, empty/null |
| `ObjectExtensionsTests` | 9 | NotEquals (various types), ToJson (default/custom/null) |
| `StringExtensionsTests` | 42 | All string methods, edge cases, JSON round-trip |
| `StringCasingExtensionsTests` | 15 | All casing variants, edge cases |
| **Total** | **118** | **All methods, all branches** |

## 🔬 Test Patterns by Method Type

### Happy Path Tests

```csharp
[Test]
public void GivenValidInput_WhenCallingMethod_ThenExpectedOutput()
{
    // Arrange
    var input = "test";

    // Act
    var result = input.SomeMethod();

    // Assert
    Assert.That(result, Is.EqualTo("expected"));
}
```

### Parameterized Tests

```csharp
[Test]
[TestCase("input1", "expected1")]
[TestCase("input2", "expected2")]
[TestCase("", "expected3")]
public void GivenVariousInputs_WhenCallingMethod_ThenCorrectOutput(string input, string expected)
    => Assert.That(input.SomeMethod(), Is.EqualTo(expected));
```

### Exception Tests

```csharp
[Test]
public void GivenInvalidInput_WhenCallingMethod_ThenSpecificExceptionThrown()
    => Assert.That(
        () => input.SomeMethod(),
        Throws.TypeOf<ArgumentException>());
```

### Null/Empty Boundary Tests

```csharp
[Test]
public void GivenNullInput_WhenCallingMethod_ThenNullReferenceExceptionThrown()
{
    string input = null!;
    Assert.That(() => input.SomeMethod(), Throws.TypeOf<NullReferenceException>());
}

[Test]
public void GivenEmptyInput_WhenCallingMethod_ThenHandledGracefully()
    => Assert.That(string.Empty.SomeMethod(), Is.EqualTo(string.Empty));
```

### Collection Tests

```csharp
[Test]
public void GivenCollection_WhenGettingRandomElement_ThenElementFromCollectionReturned()
{
    var collection = new[] { "a", "b", "c" };
    var result = collection.GetRandomElement();
    Assert.That(collection, Does.Contain(result));
}

[Test]
public void GivenSeededRandom_WhenGettingRandomElement_ThenDeterministicResult()
{
    var collection = new[] { "a", "b", "c" };
    Assert.That(collection.GetRandomElement(new Random(42)), Is.EqualTo("b"));
}
```

### Isolation Tests (FileExtensions)

```csharp
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
```

## 🛠️ Test Helpers

### DummyDisplayEnum

```csharp
public enum DummyDisplayEnum
{
    [Display(Name = "Praise the Sun!")]
    ValueWithDisplayName,
    ValueWithoutDisplayName,
}
```

Used for testing `EnumExtensions.GetDisplayName()`.

### DummyTestObject

```csharp
public sealed class DummyTestObject : IEquatable<DummyTestObject>
{
    public string StringProperty { get; set; } = string.Empty;
    public int IntProperty { get; set; }

    // Implements Equals/GetHashCode for equality testing
}
```

Used for testing `ObjectExtensions.NotEquals()` and JSON serialization.

## 📈 CI Integration

The GitHub Actions workflow (`.github/workflows/dotnet.yml`) runs:

1. `dotnet restore`
2. `dotnet build`
3. `dotnet test --collect:"XPlat Code Coverage"`
4. Coverage verification (fails if < 100%)

## ⚠️ Common Pitfalls

| Issue | Solution |
|-------|----------|
| Flaky random tests | Use seeded `Random` for deterministic tests |
| PATH modification leaks | Always restore in `TearDown`; use `[NonParallelizable]` |
| Null reference in tests | Use `null!` for deliberate null testing |
| Coverage gaps | Ensure every `if/else`, `throw`, and `return` branch is tested |
| Async tests | Not applicable — all methods are synchronous |

## 🔄 Adding New Tests

When adding a new extension method:

1. Create test class `[ClassName]ExtensionsTests.cs` (or add to existing)
2. Add tests covering:
   - Happy path with representative inputs
   - All documented exception types
   - Boundary conditions (null, empty, single element, max values)
   - Parameterized cases for multiple inputs
3. Run `dotnet test --collect:"XPlat Code Coverage"`
4. Verify 100% coverage for new code
5. Update README.md feature list

## 📝 Test Maintenance

- Tests must pass before any PR is merged
- Coverage must remain at 100%
- Test names must follow Given/When/Then convention
- No test should depend on execution order
- External dependencies (file system, environment) must be isolated in SetUp/TearDown