[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html) [![Build Status](https://github.com/hmlendea/nuciextensions/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nuciextensions/actions/workflows/dotnet.yml) [![Latest Release](https://img.shields.io/github/v/release/hmlendea/nuciextensions)](https://github.com/hmlendea/nuciextensions/releases/latest) [![NuGet](https://img.shields.io/nuget/v/NuciExtensions)](https://nuget.org/packages/NuciExtensions)

# NuciExtensions

NuciExtensions is a .NET NuGet package that provides small, focused extension methods for common tasks across core types. Each method solves a specific problem with zero overhead and maintains perfect backward compatibility.

## 📑 Table of Contents

- [Capabilities](#-capabilities)
- [Installation](#-installation)
- [Usage](#-usage)
- [Architecture](#️-architecture)
- [Development](#️-development)
- [Project Structure](#️-project-structure)
- [Contributing](#-contributing)
- [License](#-license)

## ✨ Capabilities

- **DateTime:** UNIX timestamp conversion (both directions) and elapsed time calculations
- **IDictionary:** Add-or-update operations and null-safe value lookup
- **IEnumerable:** Random element selection, duplicate detection, emptiness checks
- **IList:** In-place shuffling and pop (remove-and-return) operations
- **Enum:** Display name extraction via `DisplayAttribute` with fallback to `ToString()`
- **string:** Comprehensive text manipulation (casing, normalisation, truncation, JSON round-trip)
- **object:** Generic inequality comparison and JSON serialisation
- **File:** Verify file existence in system PATH environment variable

## 📦 Installation

NuciExtensions is published on NuGet. Install using your package manager of choice:

### Package Manager Installation

```bash
dotnet add package NuciExtensions
```

Or, via the `Package Manager Console`:

```powershell
Install-Package NuciExtensions
```

### Verification

After installation, verify by importing the namespace and using an extension method:

```csharp
using NuciExtensions;

var text = "hello";
var inverted = text.InvertCase(); // Returns "HELLO"
```

## 🚀 Usage

All extension methods are accessed by adding `using NuciExtensions;` to your file. Methods are grouped by the type they extend:

### String Operations

```csharp
using NuciExtensions;

string text = "hello-world";
text.InvertCase();        // "HELLO-WORLD"
text.Reverse();           // "dlrow-olleh"
text.Truncate(5);         // "hello"
text.RemoveDiacritics();  // (no change for ASCII)
text.ToTitleCase();       // "Hello-World"
text.ToLowerSnakeCase();  // "hello_world"

// JSON round-trip
var obj = new { name = "Alice", age = 30 };
string json = obj.ToJson();
var restored = json.FromJson<dynamic>();
```

### Collection Operations

```csharp
using NuciExtensions;

var items = new List<int> { 1, 2, 3, 4, 5 };

items.Shuffle();      // Randomise order in-place
var last = items.Pop(); // Remove and return last element

var enumerable = new[] { 1, 2, 2, 3, 3, 3 };
var duplicates = enumerable.GetDuplicates(); // [2, 3]
var random = enumerable.GetRandomElement(); // Random item

if (enumerable.IsEmpty()) { }  // false
if (enumerable.IsNullOrEmpty()) { } // false (via EnumerableExt)
```

### DateTime and Time

```csharp
using NuciExtensions;

var now = DateTime.UtcNow;
var elapsed = now.GetElapsedUnixTime(); // TimeSpan since 1970-01-01 UTC
var restored = DateTimeExtensions.FromUnixTime(1234567890); // DateTime from UNIX timestamp
```

### Dictionary and Enumeration

```csharp
using NuciExtensions;

var dict = new Dictionary<string, int>();
dict.AddOrUpdate("count", 1);  // Adds entry
dict.AddOrUpdate("count", 2);  // Updates to 2

var value = dict.TryGetValue("missing"); // Returns 0 (default for int)
var displayName = MyEnum.Value.GetDisplayName(); // Extracts DisplayAttribute value
```

## 🏗️ Architecture

See the [architecture documentation](./ARCHITECTURE.md) for the system context, principal components, runtime flows, ownership boundaries, dependencies, constraints, and extension points.

## 🛠️ Development

### Requirements

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Setup

```bash
git clone https://github.com/hmlendea/nuciextensions.git
cd nuciextensions
dotnet restore
```

### Build

```bash
dotnet build NuciExtensions.sln
```

### Test

```bash
dotnet test NuciExtensions.sln --nologo
```

### Coverage

Generate code coverage report:

```bash
dotnet test NuciExtensions.sln --nologo --collect:"XPlat Code Coverage" --results-directory /tmp/coverage
```

Current coverage: **100% line coverage** (293 lines) and **100% branch coverage** (108 branches) across 118 unit tests.

## 🗂️ Project Structure

### Projects and Packages

| Project | Type | Purpose |
|---------|------|---------|
| `NuciExtensions` | .NET Library | Public API with all extension classes |
| `NuciExtensions.UnitTests` | .NET Test Project | Comprehensive unit tests (118 tests, 100% coverage) |

### Directories

| Directory | Purpose |
|-----------|---------|
| `NuciExtensions/` | Extension method classes, one per domain (DateTime, String, Dictionary, etc.) |
| `NuciExtensions.UnitTests/` | Unit tests following `Given[x]_When[y]_Then[z]` naming convention |
| `NuciExtensions.UnitTests/Helpers/` | Test helpers (dummy objects and enumerations) |

## 🤝 Contributing

You are welcome to submit any suggestion, feedback, or modification to this project.

When doing so, please:
- Preserve the existing public contract unless a breaking change is intentional
- Submit focused pull requests that conform to the existing code style
- Maintain your branch synchronised with `master`
- Revise the documentation when functionality changes
- Properly test all modifications, including edge cases and error conditions
- Add tests for additional or modified functionality

## 💝 Project Engagement

Discovered a problem or have a suggestion? [Open an issue](https://github.com/hmlendea/nuciextensions/issues)!

If you find this project useful, consider [funding it](https://hmlendea.go.ro/fund.html) or starring ⭐️ it on GitHub!

[![Donate](https://raw.githubusercontent.com/hmlendea/readme-assets/master/donate_generic.png)](https://hmlendea.go.ro/fund.html)

## 📄 License

This project is being distributed under the `GNU General Public License v3.0 or later`.
See [LICENSE](./LICENSE) for further information.
