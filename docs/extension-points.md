# Extension Points

This document describes how to extend NuciExtensions with new functionality while maintaining architectural consistency.

## 🔧 Adding a New Extension Class

### Step-by-Step Process

1. **Create the Extension Class**

   ```csharp
   // NuciExtensions/YourDomainExtensions.cs
   using System;
   using System.Collections.Generic;
   // Add necessary using statements

   namespace NuciExtensions
   {
       /// <summary>
       /// Provides extension methods for [domain description].
       /// </summary>
       public static class YourDomainExtensions
       {
           // Add methods here
       }
   }
   ```

2. **Follow Naming Convention**
   - Class name: `PascalCase` + `Extensions` suffix
   - File name: Matches class name exactly
   - Namespace: `NuciExtensions`

3. **Implement Extension Methods**

   ```csharp
   /// <summary>
   /// Brief description of the method.
   /// </summary>
   /// <param name="source">The extended instance.</param>
   /// <param name="parameter">Parameter description.</param>
   /// <returns>Return value description.</returns>
   /// <exception cref="ExceptionType">When thrown.</exception>
   public static ReturnType MethodName(this ExtendedType source, ParameterType parameter)
   {
       // Validate inputs (fail-fast)
       ArgumentNullException.ThrowIfNull(source);

       // Implementation
       return result;
   }
   ```

4. **Add XML Documentation** — Required for all public methods

5. **Create Test File**

   ```
   NuciExtensions.UnitTests/YourDomainExtensionsTests.cs
   ```

6. **Write Comprehensive Tests** — See [Testing](testing.md) for patterns

7. **Verify Coverage**

   ```bash
   dotnet test NuciExtensions.sln --nologo --collect:"XPlat Code Coverage"
   ```

8. **Update Documentation**
   - `README.md` — Add to Capabilities and Usage
   - `docs/api-reference.md` — Add method reference
   - `ARCHITECTURE.md` — Update Components table if needed

9. **Bump Version** in `NuciExtensions.csproj`

10. **Submit PR**

---

## ➕ Adding a Method to Existing Class

### Process

1. **Add Method to Existing Class**

   ```csharp
   // In NuciExtensions/ExistingExtensions.cs
   public static ReturnType NewMethod(this ExtendedType source, ParameterType param)
   {
       // Implementation
   }
   ```

2. **Add XML Documentation**

3. **Add Tests to Existing Test Class**

   ```csharp
   // In NuciExtensions.UnitTests/ExistingExtensionsTests.cs
   [Test]
   public void GivenValidInput_WhenCallingNewMethod_ThenExpectedResult()
   {
       // Test implementation
   }
   ```

4. **Verify Coverage** — Must achieve 100% for new code

5. **Update Documentation**
   - `README.md` — Add to Usage examples
   - `docs/api-reference.md` — Add method entry

6. **Bump Version** (patch or minor)

7. **Submit PR**

---

## 📐 Architectural Rules for Extensions

### Mandatory Rules

| Rule | Description |
|------|-------------|
| **Static classes only** | All extension classes must be `public static class` |
| **Extension methods only** | All public methods must use `this` modifier |
| **No instance state** | No instance fields, properties, or constructors |
| **Static fields allowed** | Only for caching (e.g., `static Random random;`) with lazy init (`??=`) |
| **Fail-fast exceptions** | Throw documented exceptions; no silent failures |
| **Immutability for strings** | String methods return new instances; never modify input |
| **In-place for collections** | List methods may modify; document clearly |

### Performance Rules

| Rule | Description |
|------|-------------|
| **StringBuilder for loops** | Use `StringBuilder` for string concatenation in loops |
| **Lazy initialization** | `static Random random; random ??= new Random();` |
| **No reflection caching** | Enum reflection on each call; callers cache if needed |
| **Zero-allocation primitives** | Methods like `IsEmpty`, `NotEquals` allocate nothing |

### Compatibility Rules

| Rule | Description |
|------|-------------|
| **No signature changes** | Public method signatures immutable after release |
| **No exception type changes** | Documented exception types must not change |
| **No namespace changes** | All extensions in `NuciExtensions` namespace |
| **No new dependencies** | Zero NuGet package dependencies |

---

## 🎯 Extension Categories

### String Extensions

**Location:** `StringExtensions.cs`, `StringCasingExtensions.cs`

**Patterns:**
- Return new string instances (immutability)
- Use `StringBuilder` for multi-step transformations
- Leverage `System.Globalization` for culture-aware operations
- Custom character mappings in `RemoveDiacritics`

**Adding new string methods:**
1. Choose appropriate class (`StringExtensions` for general, `StringCasingExtensions` for case)
2. Follow existing patterns for null/empty handling
3. Add comprehensive diacritic/punctuation test cases if applicable

### Collection Extensions

**Location:** `EnumerableExtensions.cs`, `EnumerableExt.cs`, `ListExtensions.cs`, `DictionaryExtensions.cs`

**Patterns:**
- `EnumerableExtensions` — Pure functions on `IEnumerable<T>` (no modification)
- `EnumerableExt` — Static null-safe wrappers (not extension methods)
- `ListExtensions` — Mutable operations on `IList<T>` (modify in place, return for chaining)
- `DictionaryExtensions` — Convenience methods on `IDictionary<TKey, TValue>`

**Adding new collection methods:**
1. Choose correct class based on mutability and null-safety
2. Use `EnumerableExt.IsNullOrEmpty` for validation in `EnumerableExtensions`
3. Provide `Random` overload for deterministic testing
4. Document thread-safety of shared `Random`

### Type Extensions

**Location:** `DateTimeExtensions.cs`, `EnumExtensions.cs`, `FileExtensions.cs`, `ObjectExtensions.cs`

**Patterns:**
- `DateTimeExtensions` — UTC-only conversions, epoch constant
- `EnumExtensions` — Reflection-based, fallback to `ToString()`
- `FileExtensions` — Static methods, synchronous I/O, PATH lookup
- `ObjectExtensions` — Generic methods, JSON via `System.Text.Json`

**Adding new type methods:**
1. Match the domain to the appropriate class
2. Follow existing null-handling patterns
3. Document UTC assumptions for DateTime
4. Note reflection performance for Enum

---

## 🧪 Testing New Extensions

### Required Test Coverage

For each new method, test:

| Category | Examples |
|----------|----------|
| Happy path | Typical valid inputs |
| Null input | `NullReferenceException` or `ArgumentNullException` |
| Empty input | Empty string, empty collection |
| Boundary values | Min/max, single element |
| Error conditions | All documented exception types |
| Parameterized | Multiple input variations via `[TestCase]` |

### Test File Structure

```csharp
namespace NuciExtensions.UnitTests
{
    [TestFixture]
    public sealed class YourDomainExtensionsTests
    {
        [Test]
        public void GivenValidInput_WhenCallingMethod_ThenExpectedOutput()
        {
            // Test
        }

        [Test]
        [TestCase("input1", "expected1")]
        [TestCase("input2", "expected2")]
        public void GivenVariousInputs_WhenCallingMethod_ThenCorrectOutput(string input, string expected)
        {
            // Parameterized test
        }

        [Test]
        public void GivenNullInput_WhenCallingMethod_ThenNullReferenceExceptionThrown()
        {
            // Exception test
        }
    }
}
```

### Coverage Verification

```bash
# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Check report
# Look for line-rate="1" and branch-rate="1" for new class
```

---

## 📦 Versioning Strategy

### Version Format: `Major.Minor.Patch` (SemVer)

| Change | Version Bump | Example |
|--------|--------------|---------|
| Bug fix, internal refactor | Patch | 5.3.2 → 5.3.3 |
| New method, non-breaking | Minor | 5.3.2 → 5.4.0 |
| New extension class | Minor | 5.3.2 → 5.4.0 |
| Breaking change | Major | 5.3.2 → 6.0.0 |

### Breaking Changes Include

- Removing or renaming public methods
- Changing method signatures (parameters, return type)
- Changing exception types thrown
- Changing namespace
- Changing behavior for same inputs

### Non-Breaking Changes

- Adding new methods
- Adding new extension classes
- Performance improvements (same behavior)
- Bug fixes (correcting to documented behavior)
- Internal refactoring

---

## 📋 PR Checklist for Extensions

### Code
- [ ] Extension class follows naming convention (`*Extensions`)
- [ ] All methods are `public static` with `this` modifier
- [ ] No instance state in extension classes
- [ ] Fail-fast exception handling
- [ ] String methods return new instances
- [ ] Collection methods document mutability
- [ ] `StringBuilder` used for loops
- [ ] Lazy `Random` initialization if needed
- [ ] No new NuGet dependencies

### Documentation
- [ ] XML documentation on all public methods
- [ ] `<summary>`, `<param>`, `<returns>`, `<exception>` present
- [ ] `README.md` updated with new capability
- [ ] `docs/api-reference.md` updated
- [ ] `ARCHITECTURE.md` Components table updated (if new class)

### Tests
- [ ] Test class follows naming convention (`*ExtensionsTests`)
- [ ] Given/When/Then naming for all tests
- [ ] Happy path tests
- [ ] All exception types tested
- [ ] Boundary conditions tested
- [ ] Parameterized tests for variations
- [ ] 100% line and branch coverage achieved
- [ ] All 118+ tests pass

### Version
- [ ] Version bumped in `NuciExtensions.csproj`
- [ ] Correct bump type (patch/minor/major)

---

## 🔍 Code Review Focus Areas

Reviewers will pay special attention to:

1. **Correctness** — Edge cases, null handling, boundary conditions
2. **Performance** — Allocations, StringBuilder usage, lazy initialization
3. **Thread Safety** — Shared `Random` documented, no hidden shared state
4. **Compatibility** — No breaking changes without major version
5. **Consistency** — Matches existing patterns in the codebase
6. **Tests** — Comprehensive coverage, meaningful assertions
7. **Documentation** — Complete XML docs, updated README and API reference

---

## 💡 Extension Ideas (For Inspiration)

### String
- `ToKebabCase()` — kebab-case conversion
- `ToPascalCase()` — PascalCase conversion
- `WordCount()` — Count words
- `ExtractEmails()` — Regex email extraction
- `Mask()` — Mask sensitive data (credit cards, SSN)

### Collections
- `Batch(int size)` — Split into batches
- `DistinctBy<TKey>(Func<T, TKey>)` — Distinct by key selector
- `MaxBy<TKey>(Func<T, TKey>)` — Max by key selector
- `MinBy<TKey>(Func<T, TKey>)` — Min by key selector

### DateTime
- `ToUnixTimestamp()` — Return `long` instead of `TimeSpan`
- `StartOfDay()` / `EndOfDay()` — Day boundaries
- `AddBusinessDays(int)` — Business day arithmetic

### Object
- `DeepClone()` — Serialization-based clone
- `ToDictionary()` — Object to dictionary via reflection

---

## 📞 Questions?

- **Architecture questions:** Open a [discussion](https://github.com/hmlendea/nuciextensions/discussions)
- **Implementation help:** Check existing extension classes for patterns
- **Review process:** See [Contributing](contributing.md)

---

*Extension points documented for NuciExtensions v5.3.2+*