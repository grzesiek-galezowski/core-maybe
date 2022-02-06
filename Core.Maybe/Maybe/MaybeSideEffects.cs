﻿using System;

namespace Core.Maybe;

/// <summary>
/// Applying side effects into the Maybe call chain
/// </summary>
public static class MaybeSideEffects
{
  /// <summary>
  /// Calls <paramref name="fn"/> if <paramref name="m"/> has value, otherwise does nothing
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="m"></param>
  /// <param name="fn"></param>
  /// <returns></returns>
  public static Maybe<T> Do<T>(this Maybe<T> m, Action<T> fn) where T : notnull
  {
    if (m.IsSomething())
    {
        fn(m.Value());
    }

    return m;
  }

  /// <summary>
  /// Calls <paramref name="fn"/> if <paramref name="m"/> has value, otherwise calls <paramref name="else"/>
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="m"></param>
  /// <param name="fn"></param>
  /// <param name="else"></param>
  /// <returns></returns>
  public static Maybe<T> Match<T>(this Maybe<T> m, Action<T> fn, Action @else) where T : notnull
  {
    if (m.IsSomething())
    {
        fn(m.Value());
    }
    else
    {
        @else();
    }

    return m;
  }
}