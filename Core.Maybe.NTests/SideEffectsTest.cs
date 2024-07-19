namespace Core.Maybe.Tests;

[TestFixture]
public class SideEffectsTest
{
  [Test]
  public void DoOnNothing_DoesNothing()
  {
    var target = "unchanged";
    Maybe<string>.Nothing.Do(_ => target = "changed");
    ClassicAssert.AreEqual("unchanged", target);
  }
  [Test]
  public void DoOnSomething_DoesSomething()
  {
    var target = "unchanged";
    "changed".ToMaybe().Do(_ => target = _);
    ClassicAssert.AreEqual("changed", target);
  }

  [Test]
  public void MatchOnNothing_MatchesNothing()
  {
    var target1 = "unchanged";
    var target2 = "unchanged";
    Maybe<string>.Nothing.Match(_ => target1 = "changed", () => target2 = "changed");
    ClassicAssert.AreEqual("unchanged", target1);
    ClassicAssert.AreEqual("changed", target2);
  }
  [Test]
  public void MatchOnSomething_MatchesSomething()
  {
    var target1 = "unchanged";
    var target2 = "unchanged";
    "κατι".ToMaybe().Match(_ => target1 = "changed", () => target2 = "changed");
    ClassicAssert.AreEqual("changed", target1);
    ClassicAssert.AreEqual("unchanged", target2);
  }
}