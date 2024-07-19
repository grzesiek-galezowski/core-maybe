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
    ClassicAssert.AreEqual("a", @default);
    ClassicAssert.AreEqual("a", Maybe<string>.Nothing.OrElse(() => "a"));
  }

  [Test]
  public void OrElseTestWhenMaybeIsNothingAndOtherIsNull()
  {
    ClassicAssert.IsNull(Maybe<string>.Nothing.OrElse(null as string));
    ClassicAssert.IsNull(Maybe<string>.Nothing.OrElseNullable(() => null));
  }

  [Test]
  public void OrElseTestWhenMaybeIsNothingAndOtherIsSubclass()
  {
    ClassicAssert.IsInstanceOf<ArrayList>(Maybe<IEnumerable>.Nothing.OrElse(new ArrayList()));
    ClassicAssert.IsInstanceOf<ArrayList>(Maybe<IEnumerable>.Nothing.OrElse(() => new ArrayList()));
  }

  [Test]
  public void OrTestWithDefaultValueWhenMaybeIsNothingAndOtherIsNull()
  {
    ClassicAssert.AreEqual(Maybe<string>.Nothing, Maybe<string>.Nothing.Or(null as string));
  }
}