using Core.Either;
using FluentAssertions;

namespace Core.Maybe.Tests;

[TestFixture]
public class EitherTests
{
  private readonly Either<int, string> _eitherResult;
  private readonly Either<int, string> _eitherError;
  private const int EitherLeftValue = 5;
  private const string EitherRightValue = "Five";

  public EitherTests()
  {
    _eitherResult = EitherLeftValue.ToResult<int, string>();
    _eitherError = EitherRightValue.ToError<int, string>();
  }
#pragma warning disable 219

  [Test]
  public void NullCheckingTests()
  {
    Action<int>? nullActionInt = null;

    void MockActionInt(int x)
    {
      var y = 5;
    }

    Action<string>? nullActionString = null;

    void MockActionString(string x)
    {
      var y = 5;
    }

    Action? nullAction = null;

    void MockAction()
    {
      var a = 1;
    }

    // ReSharper disable ExpressionIsAlwaysNull
    AssertExtension.Throws<ArgumentNullException>(() => _eitherResult.Match(nullAction!, MockAction));
    AssertExtension.Throws<ArgumentNullException>(() => _eitherResult.Match(MockAction, nullAction!));
    _eitherResult.Match(MockAction, MockAction);

    AssertExtension.Throws<ArgumentNullException>(() => _eitherError.Match(nullAction!, MockAction));
    AssertExtension.Throws<ArgumentNullException>(() => _eitherError.Match(MockAction, nullAction!));
    _eitherError.Match(MockAction, MockAction);

    AssertExtension.Throws<ArgumentNullException>(() => _eitherError.Match(nullActionInt!, MockActionString));
    AssertExtension.Throws<ArgumentNullException>(() => _eitherError.Match(MockActionInt, nullActionString!));
    _eitherResult.Match(MockActionInt, MockActionString);
  }

  [Test]
  public void MatchActionTests()
  {
    var bool1 = false;
    var bool2 = false;
    var testInt = 0;
    var testString = null as string;

    void ResetTestValues()
    {
      bool1 = false;
      bool2 = false;
      testInt = 0;
      testString = null;
    }

    void SetBool1Action() => bool1 = true;
    void SetBool2Action() => bool2 = true;

    void SetTestInt(int value) => testInt = value;
    void SetTestString(string value) => testString = value;

    _eitherResult.Match(SetBool1Action, SetBool2Action);
    ClassicAssert.IsTrue(bool1);
    ClassicAssert.IsFalse(bool2);

    ResetTestValues();
    _eitherError.Match(SetBool1Action, SetBool2Action);
    ClassicAssert.IsFalse(bool1);
    ClassicAssert.IsTrue(bool2);

    ResetTestValues();
    _eitherResult.Match(SetTestInt, SetTestString);
    testInt.Should().Be(EitherLeftValue);
    ClassicAssert.AreEqual(null, testString);

    ResetTestValues();
    _eitherError.Match(SetTestInt, SetTestString);
    ClassicAssert.AreEqual(0, testInt);
    ClassicAssert.AreEqual(EitherRightValue, testString);

    ResetTestValues();
  }

  [Test]
  public void MatchFunctionTests()
  {
    var testInt = 0;
    var testString = null as string;

    void ResetTestValues()
    {
      testInt = 0;
      testString = null;
    }

    bool FuncTlt(int x)
    {
      testInt = x;
      return true;
    }

    bool FuncTrt(string x)
    {
      testString = x;
      return false;
    }

    ClassicAssert.IsTrue(_eitherResult.Match(FuncTlt, FuncTrt));
    ClassicAssert.AreEqual(EitherLeftValue, testInt);
    ClassicAssert.AreEqual(null, testString);

    ResetTestValues();
    ClassicAssert.IsFalse(_eitherError.Match(FuncTlt, FuncTrt));
    ClassicAssert.AreEqual(EitherRightValue, testString);
    ClassicAssert.AreEqual(0, testInt);

    ResetTestValues();
    ClassicAssert.IsTrue(_eitherResult.Match(() => true, () => false));
    ClassicAssert.IsFalse(_eitherError.Match(() => true, () => false));
  }

  [Test]
  public void OrDefaultFunctionsTests()
  {
    ClassicAssert.AreEqual(EitherLeftValue, _eitherResult.ResultOrDefault());
    ClassicAssert.AreEqual(EitherRightValue, _eitherError.ErrorOrDefault());

    ClassicAssert.AreEqual(0, _eitherError.ResultOrDefault());
    ClassicAssert.AreEqual(default, _eitherResult.ErrorOrDefault());

    ClassicAssert.AreEqual(29, _eitherError.ResultOrDefault(29));
    ClassicAssert.AreEqual("Twenty nine", _eitherResult.ErrorOrDefault("Twenty nine"));
  }

  [Test]
  public void SameTResultTErrorTests()
  {
    var eitherResult = Either<string, string>.Result("Left defined");
    var eitherError = Either<string, string>.Error("Right defined");

    ClassicAssert.AreEqual("Left defined", eitherResult.ResultOrDefault());
    ClassicAssert.AreEqual("Right defined", eitherError.ErrorOrDefault());

    ClassicAssert.AreEqual(null, eitherError.ResultOrDefault());
    ClassicAssert.AreEqual(null, eitherResult.ErrorOrDefault());
  }

  [Test]
  public void ExtensionMethodTests()
  {
    var eitherResult = 29.ToResult<int, string>();
    var eitherError = "Twenty nine".ToError<int, string>();

    ClassicAssert.AreEqual(29, eitherResult.ResultOrDefault());
    ClassicAssert.AreEqual(null, eitherResult.ErrorOrDefault());

    ClassicAssert.AreEqual("Twenty nine", eitherError.ErrorOrDefault());
    ClassicAssert.AreEqual(0, eitherError.ResultOrDefault());
  }
}

internal static class AssertExtension
{
  public static void Throws<T>(Action action) where T : Exception
  {
    var exceptionThrown = false;
    try
    {
      action.Invoke();
    }
    catch (T)
    {
      exceptionThrown = true;
    }

    if (!exceptionThrown)
    {
      throw new Exception(
        $"An exception of type {typeof(T)} was expected, but not thrown"
      );
    }
  }
}