# API Reference

Complete method reference for all extension classes in NuciExtensions.

## 📋 Table of Contents

- [StringExtensions](#stringextensions)
- [StringCasingExtensions](#stringcasingextensions)
- [EnumerableExtensions](#enumerableextensions)
- [EnumerableExt](#enumerableext)
- [ListExtensions](#listextensions)
- [DictionaryExtensions](#dictionaryextensions)
- [DateTimeExtensions](#datetimeextensions)
- [EnumExtensions](#enumextensions)
- [FileExtensions](#fileextensions)
- [ObjectExtensions](#objectextensions)

---

## StringExtensions

**Namespace:** `NuciExtensions`
**File:** `StringExtensions.cs`
**Extends:** `string`

### InvertCase

```csharp
public static string? InvertCase(this string? text)
```

Inverts the case of each character in the string.

| Parameter | Type | Description |
|-----------|------|-------------|
| `text` | `string?` | The string whose case is to be inverted. If null, null is returned without modification. |

**Returns:** `string?` — A new string with each character's case inverted, or null when text is null, or the empty string when it is empty.

**Exceptions:** None (null and empty input are returned unchanged)

**Example:**
```csharp
"Hello".InvertCase(); // "hELLO"
```

---

### Reverse

```csharp
public static string Reverse(this string text)
```

Reverses the characters in the string.

Reversal is performed by Unicode code point, so the two halves of a surrogate pair always remain adjacent: no unpaired surrogate is produced, and `Reverse().Reverse()` returns the original string.

| Parameter | Type | Description |
|-----------|------|-------------|
| `text` | `string` | The string to reverse |

**Returns:** `string` — A new string with characters in reverse order

**Exceptions:** `ArgumentNullException` if `text` is null

**Limitation:** Combining-mark sequences are not reordered as grapheme clusters; a base character followed by combining marks ends up preceded by those marks in the result. Grapheme-cluster segmentation is not performed.

**Example:**
```csharp
"hello".Reverse(); // "olleh"
"a🎮b".Reverse(); // "b🎮a"
```

---

### Repeat

```csharp
public static string Repeat(this string source, int count)
```

Repeats the string a specified number of times.

| Parameter | Type | Description |
|-----------|------|-------------|
| `source` | `string` | The string to repeat |
| `count` | `int` | Number of repetitions |

**Returns:** `string` — Concatenated result, or empty string if count ≤ 0

**Exceptions:** None (returns empty if source is null)

**Example:**
```csharp
"ab".Repeat(3); // "ababab"
```

---

### ReplaceFirst

```csharp
public static string ReplaceFirst(this string source, string oldValue, string newValue)
```

Replaces the first occurrence of `oldValue` with `newValue`.

| Parameter | Type | Description |
|-----------|------|-------------|
| `source` | `string` | Source string |
| `oldValue` | `string` | String to replace (cannot be null/empty) |
| `newValue` | `string` | Replacement string (null treated as empty) |

**Returns:** `string` — New string with first occurrence replaced

**Exceptions:**
- `NullReferenceException` if `source` is null
- `ArgumentNullException` if `oldValue` is null
- `ArgumentException` if `oldValue` is empty

**Example:**
```csharp
"a b a".ReplaceFirst("a", "x"); // "x b a"
```

---

### RemoveDiacritics

```csharp
public static string RemoveDiacritics(this string source)
```

Removes diacritics (accents) from the string with custom mappings for special characters.

| Parameter | Type | Description |
|-----------|------|-------------|
| `source` | `string` | Source string |

**Returns:** `string` — String with diacritics removed

**Exceptions:** `ArgumentNullException` if `source` is null

**Custom Mappings:**

Each mapped character is replaced by a digraph whose casing follows its context. The character is matched case-insensitively; the casing of the produced digraph depends on the case of the mapped character and of the adjacent letter.

| Mapped Character | Digraph | Lowercase Result | Title-Case Result | Uppercase Result |
|------------------|---------|------------------|-------------------|------------------|
| `Ö` | `Oe` | `oe` | `Oe` | `OE` |
| `Č` | `Ch` | `ch` | `Ch` | `CH` |
| `Š` | `Sh` | `sh` | `Sh` | `SH` |
| `Ř` | `Rzh` | `rzh` | `Rzh` | `RZH` |
| `Ž` | `Zh` | `zh` | `Zh` | `ZH` |

**Casing Rule:**
- A lowercase mapped character always produces the lowercase digraph.
- An uppercase mapped character produces the uppercase digraph when the adjacent letter is uppercase, and the title-case digraph otherwise.
- The adjacent letter is the following character when it is a letter, otherwise the preceding character when it is a letter.
- Case conversion is culture-invariant; the Turkish dotless-i behaviour is not applied.

**Complexity:** A single pass over the input. No regular expressions are constructed or evaluated.

**Examples:**
```csharp
"Horațiu".RemoveDiacritics(); // "Horatiu"
"Šimšat".RemoveDiacritics();  // "Shimshat"
"Ångström".RemoveDiacritics(); // "Angstroem"
"Žižek".RemoveDiacritics();   // "Zhizhek"
"ŽIŽEK".RemoveDiacritics();   // "ZHIZHEK"
"Ö".RemoveDiacritics();       // "Oe"
"ö".RemoveDiacritics();       // "oe"
"OÖ".RemoveDiacritics();      // "OOE"
```

---

### RemovePunctuation

```csharp
public static string RemovePunctuation(this string source)
```

Removes all punctuation characters from the string.

| Parameter | Type | Description |
|-----------|------|-------------|
| `source` | `string` | Source string |

**Returns:** `string` — String without punctuation

**Exceptions:** `NullReferenceException` if `source` is null

**Example:**
```csharp
"Hello, world!".RemovePunctuation(); // "Hello world"
```

---

### ToSentence

```csharp
public static string ToSentence(this string source)
```

Converts a string to sentence format: underscores and tabs are replaced with spaces, a space is inserted before every interior ASCII uppercase letter, runs of consecutive spaces are collapsed into a single space, and the result is trimmed.

| Parameter | Type | Description |
|-----------|------|-------------|
| `source` | `string` | Source string |

**Returns:** `string` — Formatted sentence

**Behaviour:**
- Every underscore (`_`) and tab (`\t`) is transliterated to a space, including in the first and last position.
- A space is inserted before each interior ASCII uppercase letter (`A`–`Z`); the uppercase letter itself is preserved.
- Two or more consecutive spaces never appear in the result, regardless of whether they came from the input or from the transliteration.
- Leading and trailing spaces are removed.

**Exceptions:**
- `ArgumentNullException` if `source` is null
- `ArgumentOutOfRangeException` if `source` is empty

**Example:**
```csharp
"hello_world".ToSentence();       // "Hello world"
"PraiseTheSun_today\tforever".ToSentence(); // "Praise The Sun today forever"
"HELLO WORLD".ToSentence();       // "H E L L O W O R L D"
"already  Sentence.".ToSentence(); // "already Sentence."
"_x".ToSentence();                // "x"
```

---

### Truncate

```csharp
public static string Truncate(this string value, int maxLength)
```

Truncates a string to a maximum length.

The length is counted in UTF-16 code units, but the cut never splits a surrogate pair. If the boundary falls between the halves of a surrogate pair, the whole character that would have been split is dropped, so the result never ends with an unpaired surrogate and may be shorter than `maxLength` by one code unit.

| Parameter | Type | Description |
|-----------|------|-------------|
| `value` | `string` | String to truncate |
| `maxLength` | `int` | Maximum length |

**Returns:** `string` — Truncated string (or original if shorter). Never ends with an unpaired surrogate.

**Exceptions:**
- `ArgumentNullException` if `value` is null
- `ArgumentOutOfRangeException` if `maxLength` is negative

**Example:**
```csharp
"Hello world".Truncate(5); // "Hello"
"🎮".Truncate(1);          // ""    (the pair is not split)
"a🎮".Truncate(2);         // "a"   (the pair is not split)
```

---

### FromJson (Deserialization)

```csharp
public static TObject FromJson<TObject>(this string json)
public static TObject FromJson<TObject>(this string json, JsonSerializerOptions options)
```

Deserializes a JSON string to an object.

| Parameter | Type | Description |
|-----------|------|-------------|
| `json` | `string` | JSON string |
| `options` | `JsonSerializerOptions` | Optional serialization options |

**Returns:** `TObject` — Deserialized object

**Exceptions:** `JsonException` on invalid JSON; `ArgumentNullException` if json is null

**Example:**
```csharp
"{\"name\":\"Alice\"}".FromJson<Person>();
```

---

## StringCasingExtensions

**Namespace:** `NuciExtensions`
**File:** `StringCasingExtensions.cs`
**Extends:** `string`

### ToTitleCase

```csharp
public static string ToTitleCase(this string source)
```

Converts to title case (each word capitalized).

**Returns:** `string` — Title-cased string

**Example:**
```csharp
"hello world".ToTitleCase(); // "Hello World"
```

---

### ToSentenceCase

```csharp
public static string ToSentenceCase(this string source)
```

Capitalizes the first letter of each sentence.

**Returns:** `string` — Sentence-cased string

**Example:**
```csharp
"hello. world!".ToSentenceCase(); // "Hello. World!"
```

---

### ToSnakeCase

```csharp
public static string ToSnakeCase(this string source)
```

Converts to snake_case (words separated by underscores).

**Returns:** `string` — Snake case string

**Example:**
```csharp
"Hello World".ToSnakeCase(); // "Hello_World"
```

---

### ToUpperSnakeCase

```csharp
public static string ToUpperSnakeCase(this string source)
```

Converts to UPPER_SNAKE_CASE.

**Returns:** `string` — Upper snake case string

**Example:**
```csharp
"Hello World".ToUpperSnakeCase(); // "HELLO_WORLD"
```

---

### ToLowerSnakeCase

```csharp
public static string ToLowerSnakeCase(this string source)
```

Converts to lower_snake_case.

**Returns:** `string` — Lower snake case string

**Example:**
```csharp
"Hello World".ToLowerSnakeCase(); // "hello_world"
```

---

## EnumerableExtensions

**Namespace:** `NuciExtensions`
**File:** `EnumerableExtensions.cs`
**Extends:** `IEnumerable<T>`

### GetRandomElement

```csharp
public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
public static T GetRandomElement<T>(this IEnumerable<T> enumerable, Random random)
```

Returns a random element from the sequence.

| Parameter | Type | Description |
|-----------|------|-------------|
| `enumerable` | `IEnumerable<T>` | Source sequence |
| `random` | `Random` | Optional random instance (uses shared static if omitted) |

**Returns:** `T` — Random element

**Exceptions:** `NullReferenceException` if sequence is null or empty

**Example:**
```csharp
new[] { 1, 2, 3 }.GetRandomElement();
new[] { 1, 2, 3 }.GetRandomElement(new Random(42));
```

---

### GetDuplicates

```csharp
public static IEnumerable<T> GetDuplicates<T>(this IEnumerable<T> source)
```

Returns each duplicated element once, in order of first duplicate occurrence.

| Parameter | Type | Description |
|-----------|------|-------------|
| `source` | `IEnumerable<T>` | Source sequence |

**Returns:** `IEnumerable<T>` — Duplicated elements

**Exceptions:** `NullReferenceException` if `source` is null

**Example:**
```csharp
new[] { 1, 2, 2, 3, 3, 3 }.GetDuplicates(); // [2, 3]
```

---

### IsEmpty

```csharp
public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
```

Checks if the sequence is empty.

| Parameter | Type | Description |
|-----------|------|-------------|
| `enumerable` | `IEnumerable<T>` | Source sequence |

**Returns:** `bool` — True if empty

**Exceptions:** `NullReferenceException` if `enumerable` is null

**Example:**
```csharp
new int[0].IsEmpty(); // true
new[] { 1 }.IsEmpty(); // false
```

---

## EnumerableExt

**Namespace:** `NuciExtensions`
**File:** `EnumerableExt.cs`
**Extends:** `IEnumerable<T>` (static methods, not extension methods)

### IsNullOrEmpty

```csharp
public static bool IsNullOrEmpty<T>(IEnumerable<T> enumerable)
```

Null-safe check for null or empty sequence.

| Parameter | Type | Description |
|-----------|------|-------------|
| `enumerable` | `IEnumerable<T>` | Source sequence (can be null) |

**Returns:** `bool` — True if null or empty

**Example:**
```csharp
EnumerableExt.IsNullOrEmpty(null);     // true
EnumerableExt.IsNullOrEmpty(new int[0]); // true
EnumerableExt.IsNullOrEmpty(new[] { 1 }); // false
```

---

### IsEmpty

```csharp
public static bool IsEmpty<T>(IEnumerable<T> enumerable)
```

Checks if sequence is empty (throws on null).

| Parameter | Type | Description |
|-----------|------|-------------|
| `enumerable` | `IEnumerable<T>` | Source sequence |

**Returns:** `bool` — True if empty

**Exceptions:** `ArgumentNullException` if `enumerable` is null

**Example:**
```csharp
EnumerableExt.IsEmpty(new int[0]); // true
```

---

## ListExtensions

**Namespace:** `NuciExtensions`
**File:** `ListExtensions.cs`
**Extends:** `IList<T>`

### Shuffle

```csharp
public static IList<T> Shuffle<T>(this IList<T> list)
```

Returns a new list with elements shuffled (original unchanged).

| Parameter | Type | Description |
|-----------|------|-------------|
| `list` | `IList<T>` | Source list |

**Returns:** `IList<T>` — New shuffled list

**Exceptions:** `NullReferenceException` if `list` is null

**Example:**
```csharp
var original = new List<int> { 1, 2, 3 };
var shuffled = original.Shuffle(); // New list, original unchanged
```

---

### Pop

```csharp
public static T Pop<T>(this IList<T> source)
```

Removes and returns the last element.

| Parameter | Type | Description |
|-----------|------|-------------|
| `source` | `IList<T>` | Source list (modified in place) |

**Returns:** `T` — The removed last element

**Exceptions:**
- `IndexOutOfRangeException` if list is empty
- `NullReferenceException` if `source` is null

**Example:**
```csharp
var list = new List<int> { 1, 2, 3 };
var last = list.Pop(); // Returns 3, list now { 1, 2 }
```

---

## DictionaryExtensions

**Namespace:** `NuciExtensions`
**File:** `DictionaryExtensions.cs`
**Extends:** `IDictionary<TKey, TValue>`

### AddOrUpdate

```csharp
public static void AddOrUpdate<TKey, TElement>(this IDictionary<TKey, TElement> source, TKey key, TElement value)
```

Adds a key-value pair or updates if key exists.

| Parameter | Type | Description |
|-----------|------|-------------|
| `source` | `IDictionary<TKey, TElement>` | Target dictionary |
| `key` | `TKey` | Key to add/update |
| `value` | `TElement` | Value to set |

**Returns:** `void`

**Example:**
```csharp
var dict = new Dictionary<string, int>();
dict.AddOrUpdate("a", 1); // Adds
dict.AddOrUpdate("a", 2); // Updates to 2
```

---

### TryGetValue

```csharp
public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
```

Gets value or returns default if key missing.

| Parameter | Type | Description |
|-----------|------|-------------|
| `source` | `IDictionary<TKey, TValue>` | Source dictionary |
| `key` | `TKey` | Key to look up |

**Returns:** `TValue` — Value or default(TValue)

**Exceptions:** `ArgumentNullException` if `key` is null

**Example:**
```csharp
var dict = new Dictionary<string, int> { { "a", 1 } };
dict.TryGetValue("a"); // 1
dict.TryGetValue("b"); // 0 (default for int)
```

---

## DateTimeExtensions

**Namespace:** `NuciExtensions`
**File:** `DateTimeExtensions.cs`
**Extends:** `DateTime`

### GetElapsedUnixTime

```csharp
public static TimeSpan GetElapsedUnixTime(this DateTime time)
```

Converts DateTime to elapsed time since UNIX epoch (1970-01-01 UTC).

| Parameter | Type | Description |
|-----------|------|-------------|
| `time` | `DateTime` | Date/time to convert |

**Returns:** `TimeSpan` — Elapsed time since epoch

**Exceptions:** `ArgumentOutOfRangeException` if date is before epoch

**Example:**
```csharp
DateTime.UtcNow.GetElapsedUnixTime();
```

---

### FromUnixTime (String)

```csharp
public static DateTime FromUnixTime(string unixTimestamp)
```

Converts UNIX timestamp string to DateTime (UTC).

| Parameter | Type | Description |
|-----------|------|-------------|
| `unixTimestamp` | `string` | Timestamp as string |

**Returns:** `DateTime` — UTC DateTime

**Exceptions:**
- `ArgumentException` if string is not a valid number
- `ArgumentOutOfRangeException` if timestamp is negative

**Example:**
```csharp
DateTimeExtensions.FromUnixTime("1234567890");
```

---

### FromUnixTime (Double)

```csharp
public static DateTime FromUnixTime(double unixTime)
```

Converts UNIX timestamp (double) to DateTime (UTC).

| Parameter | Type | Description |
|-----------|------|-------------|
| `unixTime` | `double` | Timestamp as seconds since epoch |

**Returns:** `DateTime` — UTC DateTime

**Exceptions:** `ArgumentOutOfRangeException` if timestamp is negative

**Example:**
```csharp
DateTimeExtensions.FromUnixTime(1234567890.5);
```

---

## EnumExtensions

**Namespace:** `NuciExtensions`
**File:** `EnumExtensions.cs`
**Extends:** `Enum`

### GetDisplayName

```csharp
public static string GetDisplayName(this Enum value)
```

Gets display name from `DisplayAttribute` or falls back to `ToString()`.

| Parameter | Type | Description |
|-----------|------|-------------|
| `value` | `Enum` | Enum value |

**Returns:** `string` — Display name or enum value name

**Exceptions:**
- `NullReferenceException` if `value` is null
- `ArgumentNullException` if enum value is undefined

**Example:**
```csharp
public enum Status { [Display(Name = "Active")] Active, Inactive }
Status.Active.GetDisplayName();    // "Active"
Status.Inactive.GetDisplayName();  // "Inactive"
```

---

## FileExtensions

**Namespace:** `NuciExtensions`
**File:** `FileExtensions.cs`
**Static class** (not extension methods)

### ExistsInPathVariable

```csharp
public static bool ExistsInPathVariable(string fileName)
```

Checks if a file exists in current directory or any PATH directory.

| Parameter | Type | Description |
|-----------|------|-------------|
| `fileName` | `string` | File name to search for |

**Returns:** `bool` — True if found

**Exceptions:**
- `ArgumentNullException` if `fileName` is null
- `NullReferenceException` if PATH environment variable is null

**Example:**
```csharp
FileExtensions.ExistsInPathVariable("dotnet"); // true
FileExtensions.ExistsInPathVariable("nonexistent.exe"); // false
```

---

## ObjectExtensions

**Namespace:** `NuciExtensions`
**File:** `ObjectExtensions.cs`
**Extends:** `object` (via generic `TObject`)

### NotEquals

```csharp
public static bool NotEquals<TObject>(this TObject self, TObject other)
```

Checks if two objects are not equal.

| Parameter | Type | Description |
|-----------|------|-------------|
| `self` | `TObject` | First object |
| `other` | `TObject` | Second object |

**Returns:** `bool` — True if not equal

**Exceptions:** `NullReferenceException` if `self` is null

**Example:**
```csharp
4.NotEquals(4);     // false
4.NotEquals(8);     // true
"a".NotEquals("b"); // true
```

---

### ToJson (Serialization)

```csharp
public static string ToJson<TObject>(this TObject obj)
public static string ToJson<TObject>(this TObject obj, JsonSerializerOptions options)
```

Serializes an object to JSON.

| Parameter | Type | Description |
|-----------|------|-------------|
| `obj` | `TObject` | Object to serialize |
| `options` | `JsonSerializerOptions` | Optional serialization options |

**Returns:** `string` — JSON representation

**Example:**
```csharp
new { Name = "Alice" }.ToJson(); // {"Name":"Alice"}
```

---

## 📊 Method Count Summary

| Class | Methods | Extension Methods | Static Methods |
|-------|---------|-------------------|----------------|
| StringExtensions | 12 | 12 | 0 |
| StringCasingExtensions | 5 | 5 | 0 |
| EnumerableExtensions | 4 | 4 | 0 |
| EnumerableExt | 2 | 0 | 2 |
| ListExtensions | 2 | 2 | 0 |
| DictionaryExtensions | 2 | 2 | 0 |
| DateTimeExtensions | 3 | 1 | 2 |
| EnumExtensions | 1 | 1 | 0 |
| FileExtensions | 1 | 0 | 1 |
| ObjectExtensions | 3 | 3 | 0 |
| **Total** | **35** | **30** | **5** |