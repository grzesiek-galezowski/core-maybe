using Core.Either;

namespace Core.Maybe.Tests;

[TestFixture]
public class ResultTests
{
  private readonly Result<int, string> _resultValue;
  private readonly Result<int, string> _resultError;
  private const int ResultValue = 5;
  private const string ResultError = "Five";

  public ResultTests()
  {
    _resultValue = ResultValue.ToValue<int, string>();
    _resultError = ResultError.ToError<int, string>();
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
    AssertExtension.Throws<ArgumentNullException>(() => _resultValue.Match(nullAction!, MockAction));
    AssertExtension.Throws<ArgumentNullException>(() => _resultValue.Match(MockAction, nullAction!));
    _resultValue.Match(MockAction, MockAction);

    AssertExtension.Throws<ArgumentNullException>(() => _resultError.Match(nullAction!, MockAction));
    AssertExtension.Throws<ArgumentNullException>(() => _resultError.Match(MockAction, nullAction!));
    _resultError.Match(MockAction, MockAction);

    AssertExtension.Throws<ArgumentNullException>(() => _resultError.Match(nullActionInt!, MockActionString));
    AssertExtension.Throws<ArgumentNullException>(() => _resultError.Match(MockActionInt, nullActionString!));
    _resultValue.Match(MockActionInt, MockActionString);
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

    _resultValue.Match(SetBool1Action, SetBool2Action);
    bool1.Should().BeTrue();
    bool2.Should().BeFalse();

    ResetTestValues();
    _resultError.Match(SetBool1Action, SetBool2Action);
    bool1.Should().BeFalse();
    bool2.Should().BeTrue();

    ResetTestValues();
    _resultValue.Match(SetTestInt, SetTestString);
    testInt.Should().Be(ResultValue);
    testString.Should().BeNull();

    ResetTestValues();
    _resultError.Match(SetTestInt, SetTestString);
    testInt.Should().Be(0);
    testString.Should().Be(ResultError);

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

    _resultValue.Match(FuncTlt, FuncTrt).Should().BeTrue();
    testInt.Should().Be(ResultValue);
    testString.Should().BeNull();

    ResetTestValues();
    _resultError.Match(FuncTlt, FuncTrt).Should().BeFalse();
    testString.Should().Be(ResultError);
    testInt.Should().Be(0);

    ResetTestValues();
    _resultValue.Match(() => true, () => false).Should().BeTrue();
    _resultError.Match(() => true, () => false).Should().BeFalse();
  }

  [Test]
  public void OrDefaultFunctionsTests()
  {
    _resultValue.ValueOrDefault().Should().Be(ResultValue);
    _resultError.ErrorOrDefault().Should().Be(ResultError);

    _resultError.ValueOrDefault().Should().Be(0);
    _resultValue.ErrorOrDefault().Should().Be(default);

    _resultError.ValueOrDefault(29).Should().Be(29);
    _resultValue.ErrorOrDefault("Twenty nine").Should().Be("Twenty nine");
  }

  [Test]
  public void SameTValueTErrorTests()
  {
    var resultValue = Result<string, string>.Value("Left defined");
    var resultError = Result<string, string>.Error("Right defined");

    resultValue.ValueOrDefault().Should().Be("Left defined");
    resultError.ErrorOrDefault().Should().Be("Right defined");

    resultError.ValueOrDefault().Should().BeNull();
    resultValue.ErrorOrDefault().Should().BeNull();
  }

  [Test]
  public void ExtensionMethodTests()
  {
    var resultValue = 29.ToValue<int, string>();
    var resultError = "Twenty nine".ToError<int, string>();

    resultValue.ValueOrDefault().Should().Be(29);
    resultValue.ErrorOrDefault().Should().BeNull();

    resultError.ErrorOrDefault().Should().Be("Twenty nine");
    resultError.ValueOrDefault().Should().Be(0);
  }
}
