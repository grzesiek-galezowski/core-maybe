using System;
using Core.NullableReferenceTypesExtensions;

namespace Core.Either;

/// <summary>
/// A functional monadic concept Either to make validation code more expressive and easier to maintain.
/// <typeparam name="TLeft">Type of the left value.</typeparam>
/// <typeparam name="TRight">Type of the right value.</typeparam>
/// </summary>
public readonly struct Either<TLeft, TRight>
{
  private readonly TLeft? _leftValue;
  private readonly TRight? _rightValue;
  private readonly bool _isLeft;

  private Either(TLeft? left, TRight? right, bool isLeft)
  {
    _isLeft = isLeft;

    if (isLeft)
    {
      _leftValue = left;
      _rightValue = default;
    }
    else
    {
      _rightValue = right;
      _leftValue = default;
    }
  }

  /// <summary>
  /// Constructs a new <see cref="Either{TLeft, TRight}"/> with the left side defined.
  /// </summary>
  public static Either<TLeft, TRight> Left(TLeft left) => new(left, default, true);

  /// <summary>
  /// Constructs a new <see cref="Either{TLeft, TRight}"/> with the right side defined.
  /// </summary>
  public static Either<TLeft, TRight> Right(TRight right) => new(default, right, false);

  /// <summary>
  /// Executes the left or right function depending on the Either state.
  /// </summary>
  public T Match<T>(Func<TLeft, T> leftFunc, Func<TRight, T> rightFunc)
  {
    ArgumentNullException.ThrowIfNull(leftFunc);
    ArgumentNullException.ThrowIfNull(rightFunc);

    return _isLeft ? leftFunc(_leftValue.OrThrow()) : rightFunc(_rightValue.OrThrow());
  }

  /// <summary>
  /// Executes the left or right function depending on the Either state.
  /// </summary>
  public T Match<T>(Func<T> leftFunc, Func<T> rightFunc)
  {
    ArgumentNullException.ThrowIfNull(leftFunc);
    ArgumentNullException.ThrowIfNull(rightFunc);

    return _isLeft ? leftFunc() : rightFunc();
  }

  /// <summary>
  /// Executes the left or right action depending on the Either state.
  /// </summary>
  public void Match(Action<TLeft> leftAction, Action<TRight> rightAction)
  {
    ArgumentNullException.ThrowIfNull(leftAction);
    ArgumentNullException.ThrowIfNull(rightAction);

    if (_isLeft)
    {
      leftAction(_leftValue.OrThrow());
    }
    else
    {
      rightAction(_rightValue.OrThrow());
    }
  }

  /// <summary>
  /// Executes the left or right action depending on the Either state.
  /// </summary>
  public void Match(Action leftAction, Action rightAction)
  {
    ArgumentNullException.ThrowIfNull(leftAction);
    ArgumentNullException.ThrowIfNull(rightAction);

    if (_isLeft)
    {
      leftAction();
    }
    else
    {
      rightAction();
    }
  }

  public TLeft? LeftOrDefault() => Match<TLeft?>(left => left, _ => default);

  public TRight? RightOrDefault() => Match<TRight?>(_ => default, right => right);

  public TLeft LeftOrDefault(TLeft defaultValue) => Match(left => left, _ => defaultValue);

  public TRight RightOrDefault(TRight defaultValue) => Match(_ => defaultValue, right => right);

  public static implicit operator Either<TLeft, TRight>(TLeft left) => Left(left);

  public static implicit operator Either<TLeft, TRight>(TRight right) => Right(right);
}

/// <summary>
/// A functional monadic concept Result to make validation code more expressive and easier to maintain.
/// <typeparam name="TValue">Type of the value.</typeparam>
/// <typeparam name="TError">Type of the error.</typeparam>
/// </summary>
public readonly struct Result<TValue, TError>
{
  private readonly TValue? _value;
  private readonly TError? _error;
  private readonly bool _isSuccess;

  private Result(TValue? value, TError? error, bool isSuccess)
  {
    _isSuccess = isSuccess;

    if (isSuccess)
    {
      _value = value;
      _error = default;
    }
    else
    {
      _error = error;
      _value = default;
    }
  }

  /// <summary>
  /// Constructs a new <see cref="Result{TValue, TError}"/> with the value defined.
  /// </summary>
  public static Result<TValue, TError> Value(TValue value) => new(value, default, true);

  /// <summary>
  /// Constructs a new <see cref="Result{TValue, TError}"/> with the error defined.
  /// </summary>
  public static Result<TValue, TError> Error(TError error) => new(default, error, false);

  /// <summary>
  /// Executes the value or error function depending on the Result state.
  /// </summary>
  public T Match<T>(Func<TValue, T> valueFunc, Func<TError, T> errorFunc)
  {
    ArgumentNullException.ThrowIfNull(valueFunc);
    ArgumentNullException.ThrowIfNull(errorFunc);

    return _isSuccess ? valueFunc(_value.OrThrow()) : errorFunc(_error.OrThrow());
  }

  /// <summary>
  /// Executes the value or error function depending on the Result state.
  /// </summary>
  public T Match<T>(Func<T> valueFunc, Func<T> errorFunc)
  {
    ArgumentNullException.ThrowIfNull(valueFunc);
    ArgumentNullException.ThrowIfNull(errorFunc);

    return _isSuccess ? valueFunc() : errorFunc();
  }

  /// <summary>
  /// Executes the value or error action depending on the Result state.
  /// </summary>
  public void Match(Action<TValue> valueAction, Action<TError> errorAction)
  {
    ArgumentNullException.ThrowIfNull(valueAction);
    ArgumentNullException.ThrowIfNull(errorAction);

    if (_isSuccess)
    {
      valueAction(_value.OrThrow());
    }
    else
    {
      errorAction(_error.OrThrow());
    }
  }

  /// <summary>
  /// Executes the value or error action depending on the Result state.
  /// </summary>
  public void Match(Action valueAction, Action errorAction)
  {
    ArgumentNullException.ThrowIfNull(valueAction);
    ArgumentNullException.ThrowIfNull(errorAction);

    if (_isSuccess)
    {
      valueAction();
    }
    else
    {
      errorAction();
    }
  }

  public TValue? ValueOrDefault() => Match<TValue?>(value => value, _ => default);

  public TError? ErrorOrDefault() => Match<TError?>(_ => default, error => error);

  public TValue ValueOrDefault(TValue defaultValue) => Match(value => value, _ => defaultValue);

  public TError ErrorOrDefault(TError defaultValue) => Match(_ => defaultValue, error => error);

  public static implicit operator Result<TValue, TError>(TValue value) => Value(value);

  public static implicit operator Result<TValue, TError>(TError error) => Error(error);
}

public static class EitherExtensions
{
  public static Either<TLeft, TRight> ToLeft<TLeft, TRight>(this TLeft left) => Either<TLeft, TRight>.Left(left);

  public static Either<TLeft, TRight> ToRight<TLeft, TRight>(this TRight right) => Either<TLeft, TRight>.Right(right);

  public static Result<TValue, TError> ToValue<TValue, TError>(this TValue value) => Result<TValue, TError>.Value(value);

  public static Result<TValue, TError> ToError<TValue, TError>(this TError error) => Result<TValue, TError>.Error(error);
}
