# Architecture Documentation

This document provides a detailed technical architecture overview of NuciExtensions, complementing the root [ARCHITECTURE.md](../ARCHITECTURE.md) with implementation-level details.

## 📑 Table of Contents

- [System Overview](#system-overview)
- [Component Architecture](#component-architecture)
- [Data Flow](#data-flow)
- [Extension Mechanism](#extension-mechanism)
- [Dependency Graph](#dependency-graph)
- [Runtime Behavior](#runtime-behavior)
- [Build and Deployment](#build-and-deployment)
- [Quality Attributes](#quality-attributes)

---

## System Overview

### Purpose

NuciExtensions is a **curated library of extension methods** that enhance standard .NET types without requiring changes to consuming code. The architecture prioritizes:

1. **Zero overhead** — Compiler-inlined static calls, no boxing/reflection
2. **Non-invasive** — Opt-in via `using NuciExtensions;`
3. **Backward compatible** — Immutable public signatures
4. **Fully tested** — 100% line/branch coverage

### System Boundary

```
┌─────────────────────────────────────────────────────────────┐
│                    NuciExtensions Package                    │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐            │
│  │  String     │ │ Collection  │ │   Type      │            │
│  │  Extensions │ │  Extensions │ │  Extensions │            │
│  └─────────────┘ └─────────────┘ └─────────────┘            │
└─────────────────────────────────────────────────────────────┘
                              │
        ┌─────────────────────┼─────────────────────┐
        ▼                     ▼                     ▼
┌───────────────┐    ┌───────────────┐    ┌───────────────┐
│ .NET BCL      │    │ System.Text.  │    │ System        │
│ Types         │    │ Json          │    │ Environment   │
│ (extended)    │    │ (serialization)│   │ (PATH, FS)    │
└───────────────┘    └───────────────┘    └───────────────┘
```

### Target Framework

- **.NET 10.0** exclusively (`<TargetFramework>net10.0</TargetFramework>`)
- Enables: nullable references, collection expressions, `ArgumentNullException.ThrowIfNull`, modern `System.Text.Json`

---

## Component Architecture

### Extension Classes (10 Total)

| Class | File | Extended Type | Methods | Category |
|-------|------|---------------|---------|----------|
| `StringExtensions` | `StringExtensions.cs` | `string` | 12 | Text manipulation, JSON |
| `StringCasingExtensions` | `StringCasingExtensions.cs` | `string` | 5 | Case formatting |
| `EnumerableExtensions` | `EnumerableExtensions.cs` | `IEnumerable<T>` | 4 | Sequence utilities |
| `EnumerableExt` | `EnumerableExt.cs` | `IEnumerable<T>` (static) | 2 | Null-safe checks |
| `ListExtensions` | `ListExtensions.cs` | `IList<T>` | 2 | Mutable operations |
| `DictionaryExtensions` | `DictionaryExtensions.cs` | `IDictionary<TKey,TValue>` | 2 | Dictionary utilities |
| `DateTimeExtensions` | `DateTimeExtensions.cs` | `DateTime` (1 ext + 2 static) | 3 | UNIX timestamps |
| `EnumExtensions` | `EnumExtensions.cs` | `Enum` | 1 | Display names |
| `FileExtensions` | `FileExtensions.cs` | (static) | 1 | PATH lookup |
| `ObjectExtensions` | `ObjectExtensions.cs` | `object` (generic) | 3 | JSON, inequality |

### Component Responsibilities

#### StringExtensions
- **Core text transformations:** InvertCase, Reverse, Repeat, ReplaceFirst
- **Normalization:** RemoveDiacritics (with custom mappings), RemovePunctuation
- **Formatting:** ToSentence, Truncate
- **JSON:** FromJson (deserialization)

#### StringCasingExtensions
- **Case formatting only:** ToTitleCase, ToSentenceCase, ToSnakeCase, ToUpperSnakeCase, ToLowerSnakeCase
- **Shared logic:** ToSnakeCase is the base for upper/lower variants

#### EnumerableExtensions
- **Random selection:** GetRandomElement (with/without seeded Random)
- **Analysis:** GetDuplicates (yields each duplicate once)
- **Validation:** IsEmpty (throws on null)
- **Dependency:** Uses `EnumerableExt.IsNullOrEmpty` for validation

#### EnumerableExt (Static Utilities)
- **Null-safe:** IsNullOrEmpty (handles null gracefully)
- **Strict:** IsEmpty (throws ArgumentNullException on null)
- **Not extension methods** — called as `EnumerableExt.IsNullOrEmpty(enumerable)`

#### ListExtensions
- **Shuffle:** Returns new shuffled list (original unchanged), uses Fisher-Yates via Random
- **Pop:** Removes and returns last element (modifies in place)

#### DictionaryExtensions
- **AddOrUpdate:** Atomic check-then-add/update (single-threaded)
- **TryGetValue:** Returns default(TValue) if key missing

#### DateTimeExtensions
- **Epoch constant:** `Jan1st1970` (UTC)
- **GetElapsedUnixTime:** Instance method, converts to UTC, validates ≥ epoch
- **FromUnixTime:** Two overloads (string, double) → UTC DateTime

#### EnumExtensions
- **GetDisplayName:** Reflection to find `DisplayAttribute`, fallback to `ToString()`

#### FileExtensions
- **ExistsInPathVariable:** Checks current dir + each PATH entry sequentially

#### ObjectExtensions
- **NotEquals:** Generic inequality (`!self.Equals(other)`)
- **ToJson:** Serialization via `System.Text.Json` (with/without options)

---

## Data Flow

### String Transformation Pipeline

```
Input String
     │
     ▼
┌─────────────────────────────────────┐
│ Validation (null/empty checks)      │
└─────────────────────────────────────┘
     │
     ▼
┌─────────────────────────────────────┐
│ StringBuilder Allocation            │
│ (for multi-char transformations)    │
└─────────────────────────────────────┘
     │
     ▼
┌─────────────────────────────────────┐
│ Character-by-character Processing   │
│ • char.IsUpper/IsLower/IsLetter     │
│ • CultureInfo for case conversion   │
│ • Custom diacritic mappings         │
│ • Unicode normalization (FormD/FormC)│
└─────────────────────────────────────┘
     │
     ▼
┌─────────────────────────────────────┐
│ Result Construction                 │
│ new string(char[]) or StringBuilder │
└─────────────────────────────────────┘
     │
     ▼
Output String (new instance)
```

### Collection Processing Flow

```
Input IEnumerable<T>
     │
     ▼
┌─────────────────────────────────────┐
│ EnumerableExt.IsNullOrEmpty()       │
│ (null-safe validation)              │
└─────────────────────────────────────┘
     │
     ├──────────────────┬──────────────────┐
     ▼                  ▼                  ▼
GetRandomElement   GetDuplicates       IsEmpty
     │                  │                  │
     ▼                  ▼                  ▼
Random.Next()      HashSet<T>         Any()
(count)            tracking           (single pass)
     │                  │                  │
     ▼                  ▼                  ▼
ElementAt(index)   yield return      bool result
```

### JSON Serialization Flow

```
Object → JsonSerializer.Serialize → JSON String
JSON String → JsonSerializer.Deserialize<T> → Object
```

- Delegates entirely to `System.Text.Json.JsonSerializer`
- No custom converters, options, or policies unless caller provides
- Null input serializes to `"null"` literal

---

## Extension Mechanism

### How Extension Methods Work

```csharp
// Definition
public static class StringExtensions
{
    public static string InvertCase(this string text) { ... }
}

// Usage
using NuciExtensions;
string result = "hello".InvertCase();
```

**Compiler Translation:**
```csharp
// Becomes:
string result = StringExtensions.InvertCase("hello");
```

### Namespace Scoping

All extensions reside in `NuciExtensions` namespace:

```csharp
namespace NuciExtensions
{
    public static class StringExtensions { ... }
    public static class ListExtensions { ... }
    // ...
}
```

**Consumer must opt-in:**
```csharp
using NuciExtensions;  // Required to access extensions
```

Without the `using`, methods are not visible — prevents namespace pollution.

### Static Class Requirements

| Requirement | Implementation |
|-------------|----------------|
| `public static class` | All extension classes |
| `this` modifier | First parameter of every extension method |
| No instance members | No constructors, instance fields, properties |
| Static fields allowed | Only for caching (e.g., `static Random random;`) |

---

## Dependency Graph

### Internal Dependencies

```
StringExtensions
    └──► StringCasingExtensions (no direct dependency, but related domain)

EnumerableExtensions
    └──► EnumerableExt.IsNullOrEmpty (validation)

ListExtensions
    └──► (independent)

DictionaryExtensions
    └──► (independent)

DateTimeExtensions
    └──► (independent)

EnumExtensions
    └──► (independent)

FileExtensions
    └──► (independent)

ObjectExtensions
    └──► (independent)

StringExtensions
    └──► ObjectExtensions.FromJson (no, uses JsonSerializer directly)
```

**Key Rule:** No extension class depends on another (except `EnumerableExtensions` → `EnumerableExt`)

### External Dependencies

| Dependency | Used By | Coupling |
|------------|---------|----------|
| `System.Text.Json` | StringExtensions, ObjectExtensions | Tight (API surface) |
| `System.Globalization` | StringExtensions, StringCasingExtensions | Medium (culture behavior) |
| `System.Reflection` | EnumExtensions | Loose (single method) |
| `System.ComponentModel.DataAnnotations` | EnumExtensions | Loose (attribute) |
| `System.IO` | FileExtensions | Medium (sync I/O) |
| `System.Collections.Generic` | All collection extensions | Tight (core types) |
| `System.Linq` | EnumerableExtensions, ListExtensions | Medium (Any, Count, ElementAt) |

### Dependency Direction

```
NuciExtensions (Package)
    │
    ├──► .NET Base Class Library (all dependencies)
    │
    └──► System.Text.Json (serialization)
```

**No:**
- Third-party NuGet packages
- Framework-specific APIs
- Plugin architectures
- Runtime reflection/discovery

---

## Runtime Behavior

### Thread Safety

| Component | Thread-Safe? | Notes |
|-----------|--------------|-------|
| StringExtensions | ✅ Yes | Stateless, immutable returns |
| StringCasingExtensions | ✅ Yes | Stateless |
| EnumerableExtensions | ❌ No | Shared `static Random random` |
| EnumerableExt | ✅ Yes | Stateless |
| ListExtensions | ❌ No | Shared `static Random random` |
| DictionaryExtensions | ✅ Yes | Stateless (caller owns dictionary) |
| DateTimeExtensions | ✅ Yes | Stateless |
| EnumExtensions | ✅ Yes | Stateless (reflection per call) |
| FileExtensions | ✅ Yes | Stateless (read-only FS access) |
| ObjectExtensions | ✅ Yes | Stateless |

**For thread-safe random:** Pass own `Random` instance to `GetRandomElement(enumerable, random)`

### Error Handling Philosophy

**Fail-fast with documented exceptions:**

| Input Condition | Exception | Methods |
|-----------------|-----------|---------|
| Null reference (most methods) | `NullReferenceException` | Reverse, GetRandomElement, Pop, etc. |
| Null argument (explicit check) | `ArgumentNullException` | ReplaceFirst (oldValue), TryGetValue (key) |
| Empty string (semantic error) | `ArgumentOutOfRangeException` | ToSentence, Truncate (negative) |
| Invalid format | `ArgumentException` | FromUnixTime (string) |
| Before epoch | `ArgumentOutOfRangeException` | GetElapsedUnixTime |
| Empty collection | `NullReferenceException` | GetRandomElement, Pop |
| Invalid JSON | `JsonException` | FromJson |
| Undefined enum value | `ArgumentNullException` | GetDisplayName |

**No:**
- Retry logic
- Fallback values
- Silent failures
- Error codes

### Performance Characteristics

| Method Category | Allocation | Complexity |
|-----------------|------------|------------|
| String (single pass) | O(n) new string | O(n) |
| String (loops) | StringBuilder + O(n) | O(n) |
| GetRandomElement | O(1) + enumeration | O(n) for Count() + ElementAt |
| GetDuplicates | 2× HashSet<T> | O(n) |
| Shuffle | 2× List<T> | O(n²) — RemoveAt is O(n) |
| Pop | O(1) | O(1) |
| AddOrUpdate | O(1) | O(1) |
| JSON serialize | JsonSerializer internal | Depends on object graph |
| Enum GetDisplayName | Reflection per call | O(1) reflection |
| File PATH lookup | O(p) paths × O(1) File.Exists | O(p) |

### Memory Management

- **No caching** except static `Random` (single instance per AppDomain)
- **No object pooling**
- **String methods:** New string per call (immutability)
- **Collection methods:** Caller owns collections; methods don't retain references
- **Enum reflection:** No caching; frequent callers should cache results

---

## Build and Deployment

### Project Structure

```
NuciExtensions.sln
├── NuciExtensions/
│   ├── NuciExtensions.csproj
│   ├── *.cs (10 extension classes)
│   └── bin/obj/ (build artifacts)
└── NuciExtensions.UnitTests/
    ├── NuciExtensions.UnitTests.csproj
    ├── *Tests.cs (10 test classes)
    ├── Helpers/
    └── bin/obj/
```

### Build Configuration

**NuciExtensions.csproj:**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <RootNamespace>NuciExtensions</RootNamespace>
    <Version>5.3.2</Version>
    <Nullable>enable</Nullable>
    <GeneratePackageOnBuild>false</GeneratePackageOnBuild>
    <PackageLicenseExpression>GPL-3.0-or-later</PackageLicenseExpression>
  </PropertyGroup>
</Project>
```

**NuciExtensions.UnitTests.csproj:**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>disable</ImplicitUsings>
    <LangVersion>latest</LangVersion>
    <IsPackable>false</IsPackable>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="NUnit" Version="4.5.1" />
    <PackageReference Include="NUnit3TestAdapter" Version="6.2.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="18.3.0" />
    <PackageReference Include="coverlet.collector" Version="6.0.4" />
  </ItemGroup>
</Project>
```

### Build Commands

```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build NuciExtensions.sln

# Run tests
dotnet test NuciExtensions.sln --nologo

# Run tests with coverage
dotnet test NuciExtensions.sln --collect:"XPlat Code Coverage"

# Pack NuGet package
dotnet pack NuciExtensions/NuciExtensions.csproj -c Release
```

### CI Pipeline (GitHub Actions)

`.github/workflows/dotnet.yml`:
1. Checkout
2. Setup .NET 10.0
3. Restore
4. Build
5. Test with coverage collection
6. Verify 100% coverage (fails if below)

---

## Quality Attributes

### Maintainability

| Attribute | Implementation |
|-----------|----------------|
| **Single responsibility** | One extension class per domain |
| **Low coupling** | No inter-extension dependencies |
| **High cohesion** | Related methods grouped together |
| **Documentation** | XML docs on all public APIs |
| **Testability** | Pure functions, deterministic with seeded Random |

### Reliability

| Attribute | Implementation |
|-----------|----------------|
| **Fail-fast** | Exceptions on invalid input |
| **No hidden state** | Stateless methods (except Random cache) |
| **Deterministic** | Same input → same output (with seeded Random) |
| **100% coverage** | All branches and lines tested |

### Performance

| Attribute | Implementation |
|-----------|----------------|
| **Zero overhead** | Extension methods = static calls |
| **StringBuilder** | O(n) concatenation |
| **Lazy Random** | Allocated on first use |
| **No reflection caching** | Explicit — callers control caching |

### Security

| Attribute | Implementation |
|-----------|----------------|
| **No secrets** | No credentials, keys, or sensitive data |
| **No code execution** | No `eval`, dynamic compilation, or deserialization of untrusted types |
| **Path traversal** | FileExtensions only checks existence, no path manipulation |
| **JSON safety** | Uses `System.Text.Json` (secure by default) |

### Compatibility

| Attribute | Implementation |
|-----------|----------------|
| **SemVer** | Major.Minor.Patch versioning |
| **Immutable signatures** | No breaking changes without major bump |
| **Single TFM** | net10.0 only (simplifies compatibility) |
| **No dependencies** | Zero runtime NuGet dependencies |

---

## Architecture Decisions Log

| Date | Decision | Rationale |
|------|----------|-----------|
| 2026-08-16 | Static extension methods only | Non-invasive, zero overhead, compile-time discovery |
| 2026-08-16 | Single .NET 10.0 target | Modern language features, no multi-target complexity |
| 2026-08-16 | Fail-fast exceptions | Immediate feedback, no silent corruption |
| 2026-08-16 | Zero package dependencies | Stability, no version conflicts, minimal attack surface |
| 2026-08-16 | 100% test coverage | Prevent regressions, enforce quality gate |
| 2026-08-16 | Shared Random (no locking) | Performance; thread-safety documented, overload provided |
| 2026-08-16 | Namespace-scoped extensions | Opt-in prevents pollution, clear ownership |

---

## Related Documents

- [Root ARCHITECTURE.md](../ARCHITECTURE.md) — High-level architecture
- [API Reference](api-reference.md) — Complete method reference
- [Getting Started](getting-started.md) — Installation and usage
- [Testing](testing.md) — Test structure and coverage
- [Extension Points](extension-points.md) — How to add new extensions
- [Contributing](contributing.md) — Contribution guidelines

---

*Architecture documentation for NuciExtensions v5.3.2+*