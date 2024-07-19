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

    ClassicAssert.AreEqual(Maybe<string>.Nothing, result);
  }

  [Test]
  public void CatcherFromFuncThrowingExpectedExceptionReturnsNothing()
  {
    var catcher = MaybeFunctionalWrappers
      .Catcher<string, string?, string, Exception>(
        _ => throw new Exception());

    var result = catcher("a");

    ClassicAssert.AreEqual(Maybe<string>.Nothing, result);
  }

  [Test]
  public void CatcherFromThrowingUnexpectedExceptionThrowsException()
  {
    var catcher = MaybeFunctionalWrappers
      .Catcher<string, string?, string, InvalidCastException>(
        _ => throw new Exception());

    ClassicAssert.Throws<Exception>(() => catcher("a"));
  }

  [Test]
  public void CatcherWorksWhenInvokedWithNull()
  {
    var catcher = MaybeFunctionalWrappers
      .Catcher<string?, string?, string, InvalidCastException>(
        _ => throw new Exception());

    ClassicAssert.Throws<Exception>(() => catcher(null));
  }

  [Test]
  public void WrapProducesFuncWorkingCorrectlyWithNullInput()
  {
    MaybeFunctionalWrappers.TryGet<string?, int> tryParse = int.TryParse;
    var wrapped = MaybeFunctionalWrappers.Wrap(tryParse);

    var result = wrapped(null);

    ClassicAssert.AreEqual(Maybe<int>.Nothing, result);
  }

  [Test]
  public void WrapProducesFuncWorkingCorrectlyWithNullOutput()
  {
    MaybeFunctionalWrappers.TryGet<string, string?> tryGetValue =
      new Dictionary<string, string?>().TryGetValue;
    var wrapped = MaybeFunctionalWrappers
      .Wrap<string, string?, string>(tryGetValue);

    var result = wrapped("a");

    ClassicAssert.AreEqual(Maybe<string>.Nothing, result);
  }
}