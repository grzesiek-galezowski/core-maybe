namespace Core.Maybe.Tests;

internal class MaybeConvertionsTests
{
  [Test]
  public void MaybeCastConvertsValueToItsMaybe()
  {
    var result = "a".MaybeCast<string, string>();
    result.Should().Be("a".ToMaybe());
  }

  [Test]
  public void MaybeCastConvertsValueOfNrtToItsMaybe()
  {
    var result = "a".MaybeCast<string?, string>();
    result.Should().Be("a".ToMaybe());
  }

  [Test]
  public void MaybeCastConvertsNonCastableValueOfNrtToNothing()
  {
    var result = "a".MaybeCast<object?, int>();
    result.Should().Be(Maybe<int>.Nothing);
  }

  [Test]
  public void MaybeCastConvertsNullToNothing()
  {
    var result = (null as string).MaybeCast<string?, string>();
    result.Should().Be(Maybe<string>.Nothing);
  }

  [Test]
  public void ShouldAllowToMaybeWithSelectOnNulls()
  {
    var exception = null as Exception;
    var maybeInnerException = exception.ToMaybe(e => e.InnerException);
    maybeInnerException.Should().Be(Maybe<Exception>.Nothing);
  }

  [Test]
  public void ShouldAllowToMaybeWithSelectOnNonNullInnerObjects()
  {
    var exception = new Exception();
    var maybeInnerException = exception.ToMaybe(e => e.InnerException);
    maybeInnerException.Should().Be(Maybe<Exception>.Nothing);
  }

  [Test]
  public void ShouldAllowToMaybeWithSelectOnNonNulls()
  {
    var innerException = new Exception();
    var exception = new Exception("a", innerException);
    var maybeInnerException = exception.ToMaybe(e => e.InnerException);
    maybeInnerException.Should().Be(innerException.Just());
  }

}