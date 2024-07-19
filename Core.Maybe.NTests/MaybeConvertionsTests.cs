﻿namespace Core.Maybe.Tests;

internal class MaybeConvertionsTests
{
  [Test]
  public void MaybeCastConvertsValueToItsMaybe()
  {
    var result = "a".MaybeCast<string, string>();
    ClassicAssert.AreEqual("a".ToMaybe(), result);
  }

  [Test]
  public void MaybeCastConvertsValueOfNrtToItsMaybe()
  {
    var result = "a".MaybeCast<string?, string>();
    ClassicAssert.AreEqual("a".ToMaybe(), result);
  }

  [Test]
  public void MaybeCastConvertsNonCastableValueOfNrtToNothing()
  {
    var result = "a".MaybeCast<object?, int>();
    ClassicAssert.AreEqual(Maybe<int>.Nothing, result);
  }

  [Test]
  public void MaybeCastConvertsNullToNothing()
  {
    var result = (null as string).MaybeCast<string?, string>();
    ClassicAssert.AreEqual(Maybe<string>.Nothing, result);
  }

  [Test]
  public void ShouldAllowToMaybeWithSelectOnNulls()
  {
    var exception = null as Exception;
    var maybeInnerException = exception.ToMaybe(e => e.InnerException);
    ClassicAssert.AreEqual(Maybe<Exception>.Nothing, maybeInnerException);
  }

  [Test]
  public void ShouldAllowToMaybeWithSelectOnNonNullInnerObjects()
  {
    var exception = new Exception();
    var maybeInnerException = exception.ToMaybe(e => e.InnerException);
    ClassicAssert.AreEqual(Maybe<Exception>.Nothing, maybeInnerException);
  }

  [Test]
  public void ShouldAllowToMaybeWithSelectOnNonNulls()
  {
    var innerException = new Exception();
    var exception = new Exception("a", innerException);
    var maybeInnerException = exception.ToMaybe(e => e.InnerException);
    ClassicAssert.AreEqual(innerException.Just(), maybeInnerException);
  }

}