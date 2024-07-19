using System.Collections;

namespace Core.Maybe.Tests;

[TestFixture]
public class MaybeReturnsTests
{
  [Test]
  public void OrElseTestWhenMaybeIsNothingAndOtherIsNotNull()
  {
    //do not inline or 'var' it
    // ReSharper disable once SuggestVarOrType_BuiltInTypes
    string @default = Maybe<string>.Nothing.OrElse("a");
    @default.Should().Be("a");
    Maybe<string>.Nothing.OrElse(() => "a").Should().Be("a");
  }

  [Test]
  public void OrElseTestWhenMaybeIsNothingAndOtherIsNull()
  {
    Maybe<string>.Nothing.OrElse(null as string).Should().BeNull();
    Maybe<string>.Nothing.OrElseNullable(() => null).Should().BeNull();
  }

  [Test]
  public void OrElseTestWhenMaybeIsNothingAndOtherIsSubclass()
  {
    Maybe<IEnumerable>.Nothing.OrElse(new ArrayList()).Should().BeOfType<ArrayList>();
    Maybe<IEnumerable>.Nothing.OrElse(() => new ArrayList()).Should().BeOfType<ArrayList>();
  }

  [Test]
  public void OrTestWithDefaultValueWhenMaybeIsNothingAndOtherIsNull()
  {
    Maybe<string>.Nothing.Or(null as string).Should().Be(Maybe<string>.Nothing);
  }
}