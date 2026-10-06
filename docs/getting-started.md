# Getting Started with NuciExtensions

This guide covers installation, basic usage, and common patterns for using NuciExtensions in your .NET projects.

## 📦 Installation

### Via .NET CLI

```bash
dotnet add package NuciExtensions
```

### Via Package Manager Console (Visual Studio)

```powershell
Install-Package NuciExtensions
```

### Via PackageReference (Manual)

```xml
<PackageReference Include="NuciExtensions" Version="5.3.2" />
```

## 🔧 Requirements

- **.NET 10.0 SDK** or later
- The package has **zero runtime dependencies** — all functionality derives from the .NET Base Class Library

## 🚀 Basic Usage

All extension methods are accessed by adding the namespace import:

```csharp
using NuciExtensions;
```

Methods are grouped by the type they extend. Once imported, they appear as instance methods on the extended types.

## 📝 String Operations

### Case Manipulation

```csharp
using NuciExtensions;

string text = "hello-world";

text.InvertCase();        // "HELLO-WORLD"
text.Reverse();           // "dlrow-olleh"
text.ToTitleCase();       // "Hello-World"
text.ToSentenceCase();    // "Hello-world"
text.ToLowerSnakeCase();  // "hello_world"
text.ToUpperSnakeCase();  // "HELLO_WORLD"
```

### Text Transformation

```csharp
string text = "Praise the Sun!";

text.Truncate(8);              // "Praise t"
text.RemoveDiacritics();       // Removes accents (e.g., "Horațiu" → "Horatiu")
text.RemovePunctuation();      // "Praise the Sun"
text.ToSentence();             // "Praise the Sun!"
text.Repeat(3);                // "Praise the Sun!Praise the Sun!Praise the Sun!"
text.ReplaceFirst("Sun", "Moon"); // "Praise the Moon! Praise the Sun!"
```

### JSON Round-trip

```csharp
var obj = new { name = "Alice", age = 30 };
string json = obj.ToJson();                    // {"name":"Alice","age":30}
var restored = json.FromJson<dynamic>();       // Deserializes back

// With custom options
var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
string camelJson = obj.ToJson(options);        // {"name":"Alice","age":30}
```

## 📊 Collection Operations

### Enumerable Utilities

```csharp
using NuciExtensions;

var items = new[] { 1, 2, 3, 4, 5 };
var names = new[] { "Alice", "Bob", "Charlie" };

// Random selection
var randomItem = items.GetRandomElement();           // Random element
var seededRandom = items.GetRandomElement(new Random(42)); // Deterministic

// Duplicate detection
var withDuplicates = new[] { 1, 2, 2, 3, 3, 3 };
var duplicates = withDuplicates.GetDuplicates();     // [2, 3]

// Emptiness checks
items.IsEmpty();              // false
items.IsNullOrEmpty();        // false (null-safe)
((IEnumerable<int>)null).IsNullOrEmpty();  // true
```

### List Operations (Mutable)

```csharp
var list = new List<int> { 1, 2, 3, 4, 5 };

list.Shuffle();      // Randomizes order in-place, returns new list
var last = list.Pop(); // Removes and returns last element (5)
```

### Dictionary Operations

```csharp
var dict = new Dictionary<string, int>();

dict.AddOrUpdate("count", 1);  // Adds: count=1
dict.AddOrUpdate("count", 2);  // Updates: count=2

var value = dict.TryGetValue("missing");  // Returns 0 (default for int)
var existing = dict.TryGetValue("count"); // Returns 2
```

## ⏰ DateTime Operations

### UNIX Timestamp Conversion

```csharp
using NuciExtensions;

var now = DateTime.UtcNow;

// DateTime → TimeSpan (elapsed since epoch)
TimeSpan elapsed = now.GetElapsedUnixTime();

// UNIX timestamp (double) → DateTime (UTC)
DateTime fromDouble = DateTimeExtensions.FromUnixTime(1234567890.5);

// UNIX timestamp (string) → DateTime (UTC)
DateTime fromString = DateTimeExtensions.FromUnixTime("1234567890");
```

**Important:** All conversions use UTC. Callers must manage timezone awareness.

## 🏷️ Enum Operations

### Display Name Extraction

```csharp
using NuciExtensions;
using System.ComponentModel.DataAnnotations;

public enum Status
{
    [Display(Name = "Not Started")]
    NotStarted,

    [Display(Name = "In Progress")]
    InProgress,

    Completed  // No Display attribute
}

Status.NotStarted.GetDisplayName();  // "Not Started"
Status.InProgress.GetDisplayName();  // "In Progress"
Status.Completed.GetDisplayName();   // "Completed" (falls back to ToString())
```

## 📁 File Operations

### PATH Lookup

```csharp
using NuciExtensions;

// Checks if file exists in current directory or any PATH directory
bool exists = FileExtensions.ExistsInPathVariable("dotnet");     // true
bool missing = FileExtensions.ExistsInPathVariable("nonexistent"); // false
```

## 🔄 Object Operations

### Inequality Check

```csharp
using NuciExtensions;

4.NotEquals(4);           // false
4.NotEquals(8);           // true
"hello".NotEquals("world"); // true
```

### JSON Serialization

```csharp
var obj = new { name = "Alice", age = 30 };

obj.ToJson();                              // Default options
obj.ToJson(new JsonSerializerOptions { WriteIndented = true }); // Pretty print
```

## 🎯 Common Patterns

### Chaining String Transformations

```csharp
string input = "  Hello_World-Test  ";

string result = input
    .Trim()
    .ToLowerSnakeCase()    // "hello_world_test"
    .ToTitleCase();        // "Hello_World_Test"
```

### Safe Collection Processing

```csharp
IEnumerable<string> GetItems() => /* ... */;

var items = GetItems();

if (!items.IsNullOrEmpty())
{
    var random = items.GetRandomElement();
    var dupes = items.GetDuplicates();
    // Process...
}
```

### Dictionary Accumulation

```csharp
var counts = new Dictionary<string, int>();

foreach (var item in items)
{
    counts.AddOrUpdate(item, counts.TryGetValue(item) + 1);
}
```

## ⚠️ Important Notes

| Aspect | Behavior |
|--------|----------|
| **Null handling** | Most methods throw `NullReferenceException` on null input (fail-fast) |
| **Immutability** | String methods return new instances; never modify the original |
| **Thread safety** | Shared `Random` instance is **not thread-safe**; pass your own for concurrent use |
| **Performance** | StringBuilder used for loops; lazy Random initialization; zero-allocation primitives |
| **Exceptions** | Documented exception types are thrown; no silent failures or fallbacks |

## 🔍 Verification

After installation, verify the package works:

```csharp
using NuciExtensions;

var test = "hello";
var result = test.InvertCase(); // Should return "HELLO"
Console.WriteLine(result);
```

Run your project to confirm the extension methods are available.