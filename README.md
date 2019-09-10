[![Build Status](https://travis-ci.com/hmlendea/nuciextensions.svg?branch=master)](https://travis-ci.com/hmlendea/nuciextensions)

[![Nuget](https://img.shields.io/nuget/v/NuciExtensions.svg)](https://www.nuget.org/packages/NuciExtensions/)

# NuciExtensions
C# NuGet package with useful extension methods

# Extensions

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

## List

  - Shuffle()
    - Sorts the elements of the list randomly
  - Pop()
    - Removes the last element from the list

## string

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
  - ToTitleCase()
    - Returns a new string that is the original one with the first letter of each word in upper case, and the rest in lower case
  - ToSentanceCase()
    - Returns a new string that is the original one with the first letter of each sentance in upper case, and the rest in lower case
  - ToSentance()
    - Returns a new string that is the original one split into words and trimmed
    - The delimitation is done by looking for upper case characters and underscores
  - ToSnakeCase()
    - Returns a new string that is the original one with no punctuation, and the whitespaces trimmed and replaced with underscores
  - ToUpperSnakeCase()
    - Returns a snake case version of the original string, with all letters in upper case
  - ToLowerSnakeCase()
    - Returns a snake case verison of the original string, with all letters in lower case
