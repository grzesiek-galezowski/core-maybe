﻿namespace Core.Maybe.Tests;

[TestFixture]
public class MaybeEnumeratorTests
{
  [Test]
  public void MaybeWithValueEnumerates()
  {
    var m = 1.ToMaybe().ToEnumerable();
    var c = 0;
    foreach (var val in m)
    {
      c++;
    }

    foreach (var val in m)
    {
      c++;
    }

    (c == 2).Should().BeTrue();
  }

  [Test]
  public void EmptyDoesntEnumerate()
  {
    var gotHere = false;
    foreach (var val in Maybe<bool>.Nothing.ToEnumerable())
    {
      gotHere = true;
    }

    gotHere.Should().BeFalse();
  }
}