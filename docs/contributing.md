# Contributing to NuciExtensions

Thank you for your interest in contributing to NuciExtensions! This document outlines the guidelines and processes for contributing.

## 🤝 Ways to Contribute

- **Bug Reports** — Found an issue? [Open an issue](https://github.com/hmlendea/nuciextensions/issues)
- **Feature Requests** — Have an idea? [Open an issue](https://github.com/hmlendea/nuciextensions/issues)
- **Code Contributions** — Submit a pull request with improvements
- **Documentation** — Improve docs, examples, or comments
- **Testing** — Add edge case tests or improve coverage

## 📋 Before You Start

### Check Existing Issues

Search [existing issues](https://github.com/hmlendea/nuciextensions/issues) to avoid duplicates.

### Discuss Major Changes

For significant changes (new extension classes, breaking changes, architectural shifts), open an issue first to discuss the approach.

## 🛠️ Development Setup

### Prerequisites

- **.NET 10.0 SDK** — [Download](https://dotnet.microsoft.com/download/dotnet/10.0)
- **Git** — For version control

### Clone and Build

```bash
git clone https://github.com/hmlendea/nuciextensions.git
cd nuciextensions
dotnet restore
dotnet build NuciExtensions.sln
```

### Run Tests

```bash
# Basic test run
dotnet test NuciExtensions.sln --nologo

# With coverage (required for PRs)
dotnet test NuciExtensions.sln --nologo --collect:"XPlat Code Coverage" --results-directory /tmp/coverage
```

**All 118 tests must pass with 100% coverage.**

## 📝 Code Standards

### C# Style

Follow the existing code style in the repository:

- **Nullable reference types enabled** (`<Nullable>enable</Nullable>`)
- **Latest C# language version** (`<LangVersion>latest</LangVersion>`)
- **File-scoped namespaces** (single file, single namespace)
- **Expression-bodied members** where appropriate
- **Collection expressions** for initialization (`[]`, `[.. list]`)
- **ArgumentNullException.ThrowIfNull** for null checks

### Extension Method Patterns

```csharp
public static class DomainExtensions
{
    /// <summary>
    /// Brief description of what the method does.
    /// </summary>
    /// <param name="source">Description of the extended instance.</param>
    /// <param name="parameter">Description of parameter.</param>
    /// <returns>Description of return value.</returns>
    /// <exception cref="ExceptionType">When this exception is thrown.</exception>
    public static ReturnType MethodName(this ExtendedType source, ParameterType parameter)
    {
        // Implementation
    }
}
```

### XML Documentation

All public methods must have comprehensive XML documentation:

- `<summary>` — What the method does
- `<param name="...">` — Each parameter
- `<returns>` — Return value description
- `<exception cref="...">` — Each documented exception
- `<remarks>` — Additional notes (optional)

### Naming Conventions

| Element | Convention |
|---------|------------|
| Extension Classes | `PascalCase` + `Extensions` suffix (e.g., `StringExtensions`) |
| Methods | `PascalCase` |
| Parameters | `camelCase` |
| Type Parameters | `PascalCase` (e.g., `TKey`, `TValue`) |
| Test Methods | `Given[Precondition]_When[Action]_Then[Assertion]` |
| Test Classes | `[ClassName]ExtensionsTests` |

## ✅ Pull Request Requirements

### Before Submitting

1. **All tests pass** — `dotnet test` shows 118/118 passing
2. **100% coverage** — Line and branch coverage both at 100%
3. **No breaking changes** — Unless explicitly discussed and version bumped
4. **Documentation updated** — README.md, API docs, and XML comments
5. **Code style matches** — Run `dotnet format` if available

### PR Checklist

- [ ] Tests added for new functionality
- [ ] All existing tests still pass
- [ ] Coverage remains at 100%
- [ ] XML documentation on all new public members
- [ ] README.md updated with new features
- [ ] Version bumped in `NuciExtensions.csproj` (patch/minor/major as appropriate)
- [ ] No unrelated changes (whitespace, formatting in untouched files)

### Version Bumping

| Change Type | Version Bump |
|-------------|--------------|
| Bug fix, internal refactor | Patch (5.3.2 → 5.3.3) |
| New method, non-breaking enhancement | Minor (5.3.2 → 5.4.0) |
| Breaking change (signature, behavior, exception type) | Major (5.3.2 → 6.0.0) |

## 🧪 Testing Requirements

### New Extension Methods

For each new method, add tests covering:

1. **Happy path** — Representative valid inputs
2. **Error cases** — All documented exception types
3. **Boundaries** — Null, empty, single element, max values
4. **Parameterized cases** — Multiple input variations

### Test Coverage

- New code must achieve 100% line and branch coverage
- Run coverage verification before submitting
- No coverage exclusions without justification

## 📚 Documentation Updates

When adding functionality:

1. **README.md** — Add to Capabilities and Usage sections
2. **docs/api-reference.md** — Add method documentation
3. **XML comments** — On the method itself
4. **ARCHITECTURE.md** — Update if architectural impact

## 🔄 Development Workflow

### Branch Strategy

- `master` — Main branch, always deployable
- Feature branches — `feature/description` or `fix/description`
- No long-lived branches; merge frequently

### Commit Messages

Use conventional commits:

```
feat: add new string casing method
fix: handle null input in GetRandomElement
docs: update API reference for DateTimeExtensions
test: add edge cases for DictionaryExtensions
refactor: simplify Shuffle implementation
```

### Code Review

All PRs require review. Reviewers will check:

- Correctness and edge cases
- Test coverage and quality
- Documentation completeness
- Performance implications
- Backward compatibility

## 🚫 What Not to Do

- **Don't break existing signatures** — Major version required
- **Don't add external dependencies** — Zero NuGet dependencies policy
- **Don't skip tests** — 100% coverage is mandatory
- **Don't modify unrelated files** — Keep PRs focused
- **Don't ignore thread safety** — Document if not thread-safe

## 🏗️ Architecture Principles

When contributing, respect these architectural decisions:

1. **Static extension methods only** — No inheritance, composition, or stateful facades
2. **Zero allocation overhead** — Use StringBuilder, avoid unnecessary allocations
3. **Single .NET target** — .NET 10.0 only
4. **Fail-fast semantics** — Throw documented exceptions, no silent failures
5. **No package dependencies** — Only .NET BCL
6. **Backward compatibility** — Public signatures immutable after release

## 📞 Getting Help

- **Questions:** Open a [discussion](https://github.com/hmlendea/nuciextensions/discussions)
- **Bugs:** [Issue tracker](https://github.com/hmlendea/nuciextensions/issues)
- **Security:** See [SECURITY.md](../SECURITY.md)

## 📄 License

By contributing, you agree that your contributions will be licensed under the **GPL-3.0-or-later** license (same as the project).

---

*Thank you for contributing to NuciExtensions!*