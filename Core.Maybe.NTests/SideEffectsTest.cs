namespace Core.Maybe.Tests;

[TestFixture]
public class SideEffectsTest
{
  [Test]
  public void DoOnNothing_DoesNothing()
  {
    var target = "unchanged";
    Maybe<string>.Nothing.Do(_ => target = "changed");
    target.Should().Be("unchanged");
  }
  [Test]
  public void DoOnSomething_DoesSomething()
  {
    var target = "unchanged";
    "changed".ToMaybe().Do(_ => target = _);
    target.Should().Be("changed");
  }

  [Test]
  public void MatchOnNothing_MatchesNothing()
  {
    var target1 = "unchanged";
    var target2 = "unchanged";
    Maybe<string>.Nothing.Match(_ => target1 = "changed", () => target2 = "changed");
    target1.Should().Be("unchanged");
    target2.Should().Be("changed");
  }
  [Test]
  public void MatchOnSomething_MatchesSomething()
  {
    var target1 = "unchanged";
    var target2 = "unchanged";
    "κατι".ToMaybe().Match(_ => target1 = "changed", () => target2 = "changed");
    target1.Should().Be("changed");
    target2.Should().Be("unchanged");
  }
}