using System.Collections;
using NUnit.Framework;

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
    Assert.AreEqual("a", @default);
    Assert.AreEqual("a", Maybe<string>.Nothing.OrElse(() => "a"));
  }

  [Test]
  public void OrElseTestWhenMaybeIsNothingAndOtherIsNull()
  {
    Assert.IsNull(Maybe<string>.Nothing.OrElse(null as string));
    Assert.IsNull(Maybe<string>.Nothing.OrElseNullable(() => null));
  }

  [Test]
  public void OrElseTestWhenMaybeIsNothingAndOtherIsSubclass()
  {
    Assert.IsInstanceOf<ArrayList>(Maybe<IEnumerable>.Nothing.OrElse(new ArrayList()));
    Assert.IsInstanceOf<ArrayList>(Maybe<IEnumerable>.Nothing.OrElse(() => new ArrayList()));
  }

  [Test]
  public void OrTestWithDefaultValueWhenMaybeIsNothingAndOtherIsNull()
  {
    Assert.AreEqual(Maybe<string>.Nothing, Maybe<string>.Nothing.Or(null as string));
  }
}