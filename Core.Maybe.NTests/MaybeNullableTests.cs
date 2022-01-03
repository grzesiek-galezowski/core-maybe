using NUnit.Framework;
using System;

namespace Core.Maybe.Tests;

[TestFixture]
public class MaybeNullableTests
{
  [Test]
  public void ToNullableTest()
  {
    var nothing = Maybe<Guid>.Nothing;
    Assert.AreEqual(null, nothing.ToNullable());
  }
}