using NUnit.Framework;
using System;
using Core.Either;


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
    Assert.IsTrue(bool1);
    Assert.IsFalse(bool2);

    ResetTestValues();
    _eitherError.Match(SetBool1Action, SetBool2Action);
    Assert.IsFalse(bool1);
    Assert.IsTrue(bool2);

    ResetTestValues();
    _eitherResult.Match(SetTestInt, SetTestString);
    Assert.AreEqual(EitherLeftValue, testInt);
    Assert.AreEqual(null, testString);

    ResetTestValues();
    _eitherError.Match(SetTestInt, SetTestString);
    Assert.AreEqual(0, testInt);
    Assert.AreEqual(EitherRightValue, testString);

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

    Assert.IsTrue(_eitherResult.Match(FuncTlt, FuncTrt));
    Assert.AreEqual(EitherLeftValue, testInt);
    Assert.AreEqual(null, testString);

    ResetTestValues();
    Assert.IsFalse(_eitherError.Match(FuncTlt, FuncTrt));
    Assert.AreEqual(EitherRightValue, testString);
    Assert.AreEqual(0, testInt);

    ResetTestValues();
    Assert.IsTrue(_eitherResult.Match(() => true, () => false));
    Assert.IsFalse(_eitherError.Match(() => true, () => false));
  }

  [Test]
  public void OrDefaultFunctionsTests()
  {
    Assert.AreEqual(EitherLeftValue, _eitherResult.ResultOrDefault());
    Assert.AreEqual(EitherRightValue, _eitherError.ErrorOrDefault());

    Assert.AreEqual(0, _eitherError.ResultOrDefault());
    Assert.AreEqual(default, _eitherResult.ErrorOrDefault());

    Assert.AreEqual(29, _eitherError.ResultOrDefault(29));
    Assert.AreEqual("Twenty nine", _eitherResult.ErrorOrDefault("Twenty nine"));
  }

  [Test]
  public void SameTResultTErrorTests()
  {
    var eitherResult = Either<string, string>.Result("Left defined");
    var eitherError = Either<string, string>.Error("Right defined");

    Assert.AreEqual("Left defined", eitherResult.ResultOrDefault());
    Assert.AreEqual("Right defined", eitherError.ErrorOrDefault());

    Assert.AreEqual(null, eitherError.ResultOrDefault());
    Assert.AreEqual(null, eitherResult.ErrorOrDefault());
  }

  [Test]
  public void ExtensionMethodTests()
  {
    var eitherResult = 29.ToResult<int, string>();
    var eitherError = "Twenty nine".ToError<int, string>();

    Assert.AreEqual(29, eitherResult.ResultOrDefault());
    Assert.AreEqual(null, eitherResult.ErrorOrDefault());

    Assert.AreEqual("Twenty nine", eitherError.ErrorOrDefault());
    Assert.AreEqual(0, eitherError.ResultOrDefault());
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