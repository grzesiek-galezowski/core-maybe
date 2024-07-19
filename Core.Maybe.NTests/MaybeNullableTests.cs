namespace Core.Maybe.Tests;

[TestFixture]
public class MaybeNullableTests
{
  [Test]
  public void ToNullableTest()
  {
    var nothing = Maybe<Guid>.Nothing;
    ClassicAssert.AreEqual(null, nothing.ToNullable());
  }
}