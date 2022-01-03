﻿using System;
using System.Threading.Tasks;

namespace Core.Maybe;

public static class MaybeAsync
{
  /// <summary>
  /// Flips Maybe and Task: instead of having Maybe&lt;Task&lt;T&gt;&gt; (as in case of Select) we get Task&lt;Maybe&lt;T&gt;&gt; and have possibility to await on it.
  /// </summary>
  /// <typeparam name="T">source type</typeparam>
  /// <typeparam name="TR">async result type</typeparam>
  /// <param name="this">maybe to map</param>
  /// <param name="res">async mapper</param>
  /// <returns>Task of Maybe of TR</returns>
  public static async Task<Maybe<TR>> SelectAsync<T, TR>(this Maybe<T> @this, Func<T, Task<TR?>> res)
    where T : notnull where TR : notnull =>
    @this.HasValue
      ? (await res(@this.Value())).ToMaybe()
      : (default);

  public static async Task<TR> OrElseAsync<T, TR>(this Task<Maybe<T>> @this, Func<Task<TR>> orElse)
    where T : notnull, TR
  {
    var res = await @this;
    return res.HasValue ? res.Value() : await orElse();
  }
  public static async Task<TResult> OrElse<T, TResult>(this Task<Maybe<T>> @this, TResult orElse) 
    where T : notnull, TResult
  {
    var res = await @this;
    return res.HasValue ? res.Value() : orElse;
  }
  public static async Task<TResult> OrElse<T, TResult>(this Task<Maybe<T>> @this, Func<TResult> orElse) 
    where T : notnull, TResult
  {
    var res = await @this;
    return res.HasValue ? res.Value() : orElse();
  }

  public static async Task<TR> MatchAsync<T, TR>(this Maybe<T> @this,
    Func<T, Task<TR>> res,
    Func<Task<TR>> orElse)  where T : notnull => @this.HasValue
    ? await res(@this.Value())
    : await orElse();

  public static async Task DoAsync<T>(this Maybe<T> @this,
    Func<T, Task> res) where T : notnull
  {
    if (@this.HasValue)
    {
      await res(@this.Value());
    }
  }
}