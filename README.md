[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html) [![Build Status](https://github.com/hmlendea/nuciextensions/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nuciextensions/actions/workflows/dotnet.yml) [![Latest GitHub release](https://img.shields.io/github/v/release/hmlendea/nuciextensions)](https://github.com/hmlendea/nuciextensions/releases/latest)

# NuciExtensions

.NET NuGet package with useful extension methods.

# Installation

[![Get it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciExtensions)

**.NET CLI**:
```bash
dotnet add package NuciExtensions
```

**Package Manager**:
```powershell
Install-Package NuciExtensions
```

# Features

## DateTime

  - GetElapsedUnixTime()
    - Gets the elapsed UTC time since January 1 1970
  - FromUnixTime()
    - Gets a DateTime object from a UNIX timestamp

## IDictionary

  - AddOrUpdate(TKey, TValue)
    - Adds the specified pair to the dictionary, or updates the value if it already exists
  - TryGetValue(TKey)
    - Gets the value of the specified key if it exists, or the default value of TValue if it doesn't

## IEnumerable

  - GetRandomElement()
    - Gets a random element from the collection
  - GetDuplicates()
    - Returns a new collection containing the values of the original one that appear more than once

## Enum

  - GetDisplayName()
    - Gets the value of the DisplayAttribute if it is defined, or the name of the enumeration item if it doesn't

## File

  - ExistsInPathVariable(string)
    - Checks whether a file exists in any of the PATH environment variable's directories

## List

  - Shuffle()
    - Sorts the elements of the list randomly
  - Pop()
    - Removes the last element from the list

## string

  - InvertCase()
    - Returns a new string that is a version of the original one where each lower case character is upper case, and vice-versa.
  - Reverse()
    - Returns a new string that is the reversed version of the original one
  - Repeat(int)
    - Returns a new string that is the original one concatenated with itself for a specified number of times
  - ReplaceFirst(string, string)
    - Returns a new string that is the original one with the first occurance of a substring replaced by another
  - RemoveDiacritics()
    - Returns a new string that is the original one with its diacritic characters replaced with basic latin characters
  - RemovePunctuation()
    - Returns a new string that is the original one with its punctuation characters removed
  - Reverse()
    - Returns a new string that is the original one with all of its characters reversed
  - ToTitleCase()
    - Returns a new string that is the original one with the first letter of each word in upper case, and the rest in lower case
  - ToSentenceCase()
    - Returns a new string that is the original one with the first letter of each sentence in upper case, and the rest in lower case
  - ToSentence()
    - Returns a new string that is the original one split into words and trimmed
    - The delimitation is done by looking for upper case characters and underscores
  - ToSnakeCase()
    - Returns a new string that is the original one with no punctuation, and the whitespaces trimmed and replaced with underscores
  - ToUpperSnakeCase()
    - Returns a snake case version of the original string, with all letters in upper case
  - ToLowerSnakeCase()
    - Returns a snake case verison of the original string, with all letters in lower case
