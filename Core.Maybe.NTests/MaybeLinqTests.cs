using NUnit.Framework;

namespace Core.Maybe.Tests;

internal class MaybeLinqTests
{
  [Test]
  public void SelectOrElseWithConversionToNullableTest()
  {
    var maybeString = Maybe<string>.Nothing;

    var alternativeValue = maybeString.SelectOrElse<string, string?>(s => s, () => null);

    Assert.IsNull(alternativeValue);
  }

  [Test]
  public void SelectMaybeWithTransformationReturningNullTest()
  {
    var maybeString = "a".ToMaybe();

    var result = maybeString.SelectMaybe<string, string, string>(
      s => s.ToMaybe(), 
      (s, s1) => null);
				
    Assert.AreEqual(Maybe<string>.Nothing, result);
  }

  [Test]
  public void SelectManyWithTransformationReturningNullTest()
  {
    var maybeString = "a".ToMaybe();

    var result = maybeString.SelectMany<string, string, string>(
      s => s.ToMaybe(), 
      (s, s1) => null);
				
    Assert.AreEqual(Maybe<string>.Nothing, result);
  }
}