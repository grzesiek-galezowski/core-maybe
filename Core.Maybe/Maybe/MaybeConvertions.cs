﻿using System;
using System.Collections.Generic;

namespace Core.Maybe;

/// <summary>
/// Fluent exts for converting the values of Maybe to/from lists, nullables; casting and upshifting
/// </summary>
public static class MaybeConvertions
{
  /// <summary>
  /// If <paramref name="a"/>.Value() exists and can be successfully casted to <typeparamref name="TB"/>, returns the casted one, wrapped as Maybe&lt;TB&gt;, otherwise Nothing
  /// </summary>
  /// <typeparam name="TA"></typeparam>
  /// <typeparam name="TB"></typeparam>
  /// <param name="a"></param>
  /// <returns></returns>
  public static Maybe<TB> Cast<TA, TB>(this Maybe<TA> a) where TB : class where TA : notnull =>
    from m in a
    let t = m as TB
    where t != null
    select t;

  /// <summary>
  /// If <paramref name="a"/> can be successfully casted to <typeparamref name="TR"/>, returns the casted one, wrapped as Maybe&lt;TR&gt;, otherwise Nothing
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <typeparam name="TR"></typeparam>
  /// <param name="a"></param>
  /// <returns></returns>
  public static Maybe<TR> MaybeCast<T, TR>(this T a)
    where TR : notnull, T =>
    MaybeFunctionalWrappers.Catcher<T?, TR, InvalidCastException>(o => (TR)o!)(a);

  /// <summary>
  /// If <paramref name="a"/>.Value() is present, returns an enumerable of that single value, otherwise an empty one
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="a"></param>
  /// <returns></returns>
  public static IEnumerable<T> ToEnumerable<T>(this Maybe<T> a) where T : notnull
  {
    if (a.IsSomething())
    {
      yield return a.Value();
    }
  }

  /// <summary>
  /// Converts Maybe to corresponding Nullable
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="a"></param>
  /// <returns></returns>
  public static T? ToNullable<T>(this Maybe<T> a) where T : struct =>
    a.IsSomething() ? a.Value() : null;

  /// <summary>
  /// Converts Nullable to corresponding Maybe
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="a"></param>
  /// <returns></returns>
  public static Maybe<T> ToMaybe<T>(this T? a) where T : struct =>
    !a.HasValue ? default : a.Value.ToMaybe();

  /// <summary>
  /// Returns <paramref name="a"/> wrapped as Maybe
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="a"></param>
  /// <returns></returns>
  public static Maybe<T> ToMaybe<T>(this T? a) where T : notnull =>
    a == null ? default : new Maybe<T>(a);

  /// <summary>
  /// Returns <paramref name="a"/> wrapped as Maybe
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <typeparam name="TResult"></typeparam>
  /// <param name="a"></param>
  /// <param name="fn"></param>
  /// <returns></returns>
  public static Maybe<TResult> ToMaybe<T, TResult>(this T? a, Func<T, TResult?> fn)
      where T : notnull where TResult : notnull =>
    a == null ? default : new Maybe<T>(a).Select(fn);
}