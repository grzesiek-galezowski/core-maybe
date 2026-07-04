using Core.Either;

namespace Core.Maybe.Tests;

[TestFixture]
public class EitherTests
{
  private readonly Either<int, string> _eitherLeft;
  private readonly Either<int, string> _eitherRight;
  private const int EitherLeftValue = 5;
  private const string EitherRightValue = "Five";

  public EitherTests()
  {
    _eitherLeft = EitherLeftValue.ToLeft<int, string>();
    _eitherRight = EitherRightValue.ToRight<int, string>();
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
    AssertExtension.Throws<ArgumentNullException>(() => _eitherLeft.Match(nullAction!, MockAction));
    AssertExtension.Throws<ArgumentNullException>(() => _eitherLeft.Match(MockAction, nullAction!));
    _eitherLeft.Match(MockAction, MockAction);

    AssertExtension.Throws<ArgumentNullException>(() => _eitherRight.Match(nullAction!, MockAction));
    AssertExtension.Throws<ArgumentNullException>(() => _eitherRight.Match(MockAction, nullAction!));
    _eitherRight.Match(MockAction, MockAction);

    AssertExtension.Throws<ArgumentNullException>(() => _eitherRight.Match(nullActionInt!, MockActionString));
    AssertExtension.Throws<ArgumentNullException>(() => _eitherRight.Match(MockActionInt, nullActionString!));
    _eitherLeft.Match(MockActionInt, MockActionString);
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

    _eitherLeft.Match(SetBool1Action, SetBool2Action);
    bool1.Should().BeTrue();
    bool2.Should().BeFalse();

    ResetTestValues();
    _eitherRight.Match(SetBool1Action, SetBool2Action);
    bool1.Should().BeFalse();
    bool2.Should().BeTrue();

    ResetTestValues();
    _eitherLeft.Match(SetTestInt, SetTestString);
    testInt.Should().Be(EitherLeftValue);
    testString.Should().BeNull();

    ResetTestValues();
    _eitherRight.Match(SetTestInt, SetTestString);
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

    _eitherLeft.Match(FuncTlt, FuncTrt).Should().BeTrue();
    testInt.Should().Be(EitherLeftValue);
    testString.Should().BeNull();

    ResetTestValues();
    _eitherRight.Match(FuncTlt, FuncTrt).Should().BeFalse();
    testString.Should().Be(EitherRightValue);
    testInt.Should().Be(0);

    ResetTestValues();
    _eitherLeft.Match(() => true, () => false).Should().BeTrue();
    _eitherRight.Match(() => true, () => false).Should().BeFalse();
  }

  [Test]
  public void OrDefaultFunctionsTests()
  {
    _eitherLeft.LeftOrDefault().Should().Be(EitherLeftValue);
    _eitherRight.RightOrDefault().Should().Be(EitherRightValue);

    _eitherRight.LeftOrDefault().Should().Be(0);
    _eitherLeft.RightOrDefault().Should().Be(default);

    _eitherRight.LeftOrDefault(29).Should().Be(29);
    _eitherLeft.RightOrDefault("Twenty nine").Should().Be("Twenty nine");
  }

  [Test]
  public void SameTLeftTRightTests()
  {
    var eitherLeft = Either<string, string>.Left("Left defined");
    var eitherRight = Either<string, string>.Right("Right defined");

    eitherLeft.LeftOrDefault().Should().Be("Left defined");
    eitherRight.RightOrDefault().Should().Be("Right defined");

    eitherRight.LeftOrDefault().Should().BeNull();
    eitherLeft.RightOrDefault().Should().BeNull();
  }

  [Test]
  public void ExtensionMethodTests()
  {
    var eitherLeft = 29.ToLeft<int, string>();
    var eitherRight = "Twenty nine".ToRight<int, string>();

    eitherLeft.LeftOrDefault().Should().Be(29);
    eitherLeft.RightOrDefault().Should().BeNull();

    eitherRight.RightOrDefault().Should().Be("Twenty nine");
    eitherRight.LeftOrDefault().Should().Be(0);
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
