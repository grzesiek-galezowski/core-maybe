namespace Core.Maybe.Tests;

file class MaybeLinqTests
{
  [Test]
  public void SelectOrElseWithConversionToNullableTest()
  {
    var maybeString = Maybe<string>.Nothing;

    var alternativeValue = maybeString.SelectOrElse<string, string?>(s => s, () => null);

    alternativeValue.Should().BeNull();
  }

  [Test]
  public void SelectMaybeWithTransformationReturningNullTest()
  {
    var maybeString = "a".ToMaybe();

    var result = maybeString.SelectMaybe<string, string, string>(
      s => s.ToMaybe(),
      (s, s1) => null);

    result.Should().Be(Maybe<string>.Nothing);
  }

  [Test]
  public void SelectManyWithTransformationReturningNullTest()
  {
    var maybeString = "a".ToMaybe();

    var result = maybeString.SelectMany<string, string, string>(
      s => s.ToMaybe(),
      (s, s1) => null);

    result.Should().Be(Maybe<string>.Nothing);
  }
}