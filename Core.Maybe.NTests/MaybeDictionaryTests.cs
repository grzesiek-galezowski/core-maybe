using System.Collections.Generic;

namespace Core.Maybe.Tests;

file class MaybeDictionaryTests
{
  [Test]
  public void LookupReturnsNothingWhenThereIsNoNrtValueForKey()
  {
    var dictionary = new Dictionary<string, string?>();

    var maybe = dictionary.Lookup<string, string?, string>("a");

    maybe.Should().Be(Maybe<string>.Nothing);
  }

  [Test]
  public void LookupReturnsNothingWhenNrtValueForKeyIsNull()
  {
    var dictionary = new Dictionary<string, string?>
    {
      ["a"] = null
    };

    var maybe = dictionary.Lookup<string, string?, string>("a");

    maybe.Should().Be(Maybe<string>.Nothing);
  }

  [Test]
  public void LookupReturnsMaybeOfNrtValueForKeyWhenValueExists()
  {
    var dictionary = new Dictionary<string, string?>
    {
      ["a"] = "b"
    };

    var maybe = dictionary.Lookup<string, string?, string>("a");

    maybe.Should().Be("b".ToMaybe());
  }
}