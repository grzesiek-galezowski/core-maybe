using System.Collections.Generic;

namespace Core.Maybe.Tests;

internal class MaybeFunctionalWrappersTests
{
  [Test]
  public void CatcherFromLambdaReturningNullReturnsNothing()
  {
    var catcher = MaybeFunctionalWrappers
      .Catcher<string, string?, string, Exception>(_ => null);

    var result = catcher("a");

    result.Should().Be(Maybe<string>.Nothing);
  }

  [Test]
  public void CatcherFromFuncThrowingExpectedExceptionReturnsNothing()
  {
    var catcher = MaybeFunctionalWrappers
      .Catcher<string, string?, string, Exception>(
        _ => throw new Exception());

    var result = catcher("a");

    result.Should().Be(Maybe<string>.Nothing);
  }

  [Test]
  public void CatcherFromThrowingUnexpectedExceptionThrowsException()
  {
    var catcher = MaybeFunctionalWrappers
      .Catcher<string, string?, string, InvalidCastException>(
        _ => throw new Exception());

    FluentActions.Invoking(() => catcher("a")).Should().ThrowExactly<Exception>();
  }

  [Test]
  public void CatcherWorksWhenInvokedWithNull()
  {
    var catcher = MaybeFunctionalWrappers
      .Catcher<string?, string?, string, InvalidCastException>(
        _ => throw new Exception());

    FluentActions.Invoking(() => catcher(null)).Should().ThrowExactly<Exception>();
  }

  [Test]
  public void WrapProducesFuncWorkingCorrectlyWithNullInput()
  {
    MaybeFunctionalWrappers.TryGet<string?, int> tryParse = int.TryParse;
    var wrapped = MaybeFunctionalWrappers.Wrap(tryParse);

    var result = wrapped(null);

    result.Should().Be(Maybe<int>.Nothing);
  }

  [Test]
  public void WrapProducesFuncWorkingCorrectlyWithNullOutput()
  {
    MaybeFunctionalWrappers.TryGet<string, string?> tryGetValue =
      new Dictionary<string, string?>().TryGetValue;
    var wrapped = MaybeFunctionalWrappers
      .Wrap<string, string?, string>(tryGetValue);

    var result = wrapped("a");

    result.Should().Be(Maybe<string>.Nothing);
  }
}