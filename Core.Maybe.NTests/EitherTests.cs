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
    bool1.Should().BeTrue();
    bool2.Should().BeFalse();

    ResetTestValues();
    _eitherError.Match(SetBool1Action, SetBool2Action);
    bool1.Should().BeFalse();
    bool2.Should().BeTrue();

    ResetTestValues();
    _eitherResult.Match(SetTestInt, SetTestString);
    testInt.Should().Be(EitherLeftValue);
    testString.Should().BeNull();

    ResetTestValues();
    _eitherError.Match(SetTestInt, SetTestString);
    testInt.Should().Be(0);
    testString.Should().Be(EitherRightValue);

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

    _eitherResult.Match(FuncTlt, FuncTrt).Should().BeTrue();
    testInt.Should().Be(EitherLeftValue);
    testString.Should().BeNull();

    ResetTestValues();
    _eitherError.Match(FuncTlt, FuncTrt).Should().BeFalse();
    testString.Should().Be(EitherRightValue);
    testInt.Should().Be(0);

    ResetTestValues();
    _eitherResult.Match(() => true, () => false).Should().BeTrue();
    _eitherError.Match(() => true, () => false).Should().BeFalse();
  }

  [Test]
  public void OrDefaultFunctionsTests()
  {
    _eitherResult.ResultOrDefault().Should().Be(EitherLeftValue);
    _eitherError.ErrorOrDefault().Should().Be(EitherRightValue);

    _eitherError.ResultOrDefault().Should().Be(0);
    _eitherResult.ErrorOrDefault().Should().Be(default);

    _eitherError.ResultOrDefault(29).Should().Be(29);
    _eitherResult.ErrorOrDefault("Twenty nine").Should().Be("Twenty nine");
  }

  [Test]
  public void SameTResultTErrorTests()
  {
    var eitherResult = Either<string, string>.Result("Left defined");
    var eitherError = Either<string, string>.Error("Right defined");

    eitherResult.ResultOrDefault().Should().Be("Left defined");
    eitherError.ErrorOrDefault().Should().Be("Right defined");

    eitherError.ResultOrDefault().Should().BeNull();
    eitherResult.ErrorOrDefault().Should().BeNull();
  }

  [Test]
  public void ExtensionMethodTests()
  {
    var eitherResult = 29.ToResult<int, string>();
    var eitherError = "Twenty nine".ToError<int, string>();

    eitherResult.ResultOrDefault().Should().Be(29);
    eitherResult.ErrorOrDefault().Should().BeNull();

    eitherError.ErrorOrDefault().Should().Be("Twenty nine");
    eitherError.ResultOrDefault().Should().Be(0);
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