using System;
using System.Collections.Generic;

namespace Core.Maybe
{
  public static class MaybeCollection
  {
    [Obsolete("Use .Lookup() method")]
    public static Maybe<TValue> MaybeValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue?> dictionary,
      TKey key) where TValue : notnull
    {
      var result = dictionary.TryGetValue(key, out var value);
      return (result && value != null) ? value.Just() : Maybe<TValue>.Nothing;
    }

    [Obsolete("Use .Lookup() method")]
    public static Maybe<TValue> MaybeValue<TKey, TValue>(
      this IDictionary<TKey, TValue?> dictionary,
      TKey key) where TValue : notnull
    {
      var result = dictionary.TryGetValue(key, out var value);
      return (result && value != null) ? value.Just() : Maybe<TValue>.Nothing;
    }

    [Obsolete("Use .Lookup() method")]
    public static Maybe<TValue> MaybeValue<TKey, TValue>(
      this Dictionary<TKey, TValue?> dictionary,
      TKey key) where TValue : notnull 
      => ((IDictionary<TKey, TValue?>)dictionary).MaybeValue(key);
  }
}