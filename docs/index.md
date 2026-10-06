# NuciExtensions Documentation

Welcome to the comprehensive documentation for **NuciExtensions**, a .NET NuGet package providing focused extension methods for common tasks across core .NET types.

## 📚 Documentation Structure

| Document | Description |
|----------|-------------|
| [Getting Started](getting-started.md) | Installation, basic usage, and quick examples |
| [API Reference](api-reference.md) | Complete method reference for all extension classes |
| [Architecture](architecture.md) | System design, components, and architectural decisions |
| [Testing](testing.md) | Test structure, coverage requirements, and running tests |
| [Contributing](contributing.md) | Guidelines for contributing to the project |
| [Extension Points](extension-points.md) | How to add new extension methods or classes |

## 🎯 Quick Overview

NuciExtensions provides **118 unit tests** with **100% line and branch coverage** across **10 extension classes**:

| Extension Class | Domain | Methods |
|-----------------|--------|---------|
| `StringExtensions` | Text manipulation, JSON round-trip | 12 |
| `StringCasingExtensions` | Case formatting (title, sentence, snake) | 5 |
| `EnumerableExtensions` | Random selection, duplicates, emptiness | 4 |
| `EnumerableExt` | Null-safe enumerable checks | 2 |
| `ListExtensions` | In-place shuffle and pop | 2 |
| `DictionaryExtensions` | Add-or-update, safe lookup | 2 |
| `DateTimeExtensions` | UNIX timestamp conversion | 3 |
| `EnumExtensions` | Display name extraction | 1 |
| `FileExtensions` | PATH environment lookup | 1 |
| `ObjectExtensions` | Inequality, JSON serialization | 3 |

## 🚀 Quick Start

```bash
dotnet add package NuciExtensions
```

```csharp
using NuciExtensions;

var text = "hello world";
text.ToTitleCase();        // "Hello World"
text.ToLowerSnakeCase();   // "hello_world"

var items = new List<int> { 1, 2, 3, 4, 5 };
items.Shuffle();           // Randomizes in-place
var last = items.Pop();    // Removes and returns last element

var now = DateTime.UtcNow;
var elapsed = now.GetElapsedUnixTime();  // TimeSpan since epoch
var restored = DateTimeExtensions.FromUnixTime(1234567890);
```

## 📦 Package Information

- **Target Framework:** .NET 10.0
- **Package:** [NuGet.org](https://www.nuget.org/packages/NuciExtensions)
- **Repository:** [GitHub](https://github.com/hmlendea/nuciextensions)
- **License:** GPL-3.0-or-later
- **Version:** 5.3.2

## 🔗 Related Files

- [README.md](../README.md) - Project overview and usage examples
- [ARCHITECTURE.md](../ARCHITECTURE.md) - Detailed architecture documentation
- [SECURITY.md](../SECURITY.md) - Security policy and vulnerability reporting
- [LICENSE](../LICENSE) - License terms