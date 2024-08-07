﻿using System;
using Core.NullableReferenceTypesExtensions;

namespace Core.Either;

/// <summary>
/// A functional monadic concept Either to make validation code more expressive and easier to maintain.
/// <typeparam name="TResult">Type of Result item</typeparam>
/// <typeparam name="TError">Type of Error item</typeparam>
/// </summary>
public readonly struct Either<TResult, TError>
{
  private readonly TResult? _resultValue;
  private readonly TError? _errorValue;
  private readonly bool _success;

  private Either(TResult? result, TError? error, bool success)
  {
    _success = success;

    if (success)
    {
      _resultValue = result;
      _errorValue = default;
    }
    else
    {
      _errorValue = error;
      _resultValue = default;
    }
  }

  /// <summary>
  ///  Constructs new <see cref="Either{TResult, TError}"/> with the Result part defined.
  /// </summary>
  public static Either<TResult, TError> Result(TResult result) => new(result, default, true);

  /// <summary>
  ///  Constructs new <see cref="Either{TResult, TError}"/> with the Error part defined.
  /// </summary>
  public static Either<TResult, TError> Error(TError error) => new(default, error, false);

  /// <summary>
  /// Executes result or error function depending on the Either state.
  /// </summary>
  public T Match<T>(Func<TResult, T> resultFunc, Func<TError, T> errorFunc)
  {
    ArgumentNullException.ThrowIfNull(resultFunc);
    ArgumentNullException.ThrowIfNull(errorFunc);
    if (_success)
    {
      return resultFunc(_resultValue.OrThrow());
    }
    else
    {
      return errorFunc(_errorValue.OrThrow());
    }
  }

  /// <summary>
  /// Executes result or error function depending on the Either state.
  /// </summary>
  public T Match<T>(Func<T> leftFunc, Func<T> rightFunc)
  {
    ArgumentNullException.ThrowIfNull(leftFunc);
    ArgumentNullException.ThrowIfNull(rightFunc);

    return _success ? leftFunc() : rightFunc();
  }

  /// <summary>
  /// Executes result or error action depending on the Either state.
  /// </summary>
  public void Match(Action<TResult> resultAction, Action<TError> errorAction)
  {
    ArgumentNullException.ThrowIfNull(resultAction);
    ArgumentNullException.ThrowIfNull(errorAction);

    if (_success)
    {
      resultAction(_resultValue.OrThrow());
    }
    else
    {
      errorAction(_errorValue.OrThrow());
    }
  }

  /// <summary>
  /// Executes result or error action depending on the Either state.
  /// </summary>
  public void Match(Action resultAction, Action errorAction)
  {
    ArgumentNullException.ThrowIfNull(resultAction);
    ArgumentNullException.ThrowIfNull(errorAction);

    if (_success)
    {
      resultAction();
    }
    else
    {
      errorAction();
    }
  }

  public TResult? ResultOrDefault() =>
    Match<TResult?>(res => res, err => default);

  public TError? ErrorOrDefault() =>
    Match<TError?>(res => default, err => err);

  public TResult ResultOrDefault(TResult defaultValue) =>
    Match(res => res, err => defaultValue);

  public TError ErrorOrDefault(TError defaultValue) =>
    Match(res => defaultValue, err => err);

  public static implicit operator Either<TResult, TError>(TResult result)
    => Result(result);
  public static implicit operator Either<TResult, TError>(TError error)
    => Error(error);
}

public static class EitherExtensions
{
  public static Either<TL, TR> ToResult<TL, TR>(this TL result) => Either<TL, TR>.Result(result);
  public static Either<TL, TR> ToError<TL, TR>(this TR error) => Either<TL, TR>.Error(error);
}