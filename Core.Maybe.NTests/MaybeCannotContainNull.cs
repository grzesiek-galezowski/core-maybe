namespace Core.Maybe.Tests;

[TestFixture]
public class MaybeCannotContainNull
{
  private class User
  {
    public string? Name { get; set; }
  }

  [Test]
  public void WhenSelectingNull_GettingNothing()
  {
    var user = new User { Name = null };

    var maybeUser = user.ToMaybe();

    maybeUser.Select(_ => _.Name).Should().Be(Maybe<string>.Nothing);
  }

}