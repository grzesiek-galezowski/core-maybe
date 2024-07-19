using System.Collections.Generic;

namespace Core.Maybe.Tests;

internal class Examples
{
  [Test]
  public void ShouldXXXXXXXXXXXXX() //bug
  {
    //GIVEN
    var x = GetList();

    //WHEN

    //THEN
    x.Should().Be(Maybe<List<int>>.Nothing);
  }

  private Maybe<List<int>> GetList()
  {
    var maybe = true.Then(null as List<int>);
    return maybe;
  }

  [Test]
  public void ShouldXXXXXXXXXXXXX2() //bug
  {
    //GIVEN
    var dictionary = new Dictionary<string, string?>
    {
      ["a"] = null
    };
    //WHEN
    var maybeValue = dictionary.MaybeValue("a");
    var lookedUp = dictionary.LookupNullable("a");
    var maybeValue2 = dictionary.MaybeValue("b");
    var lookedUp2 = dictionary.LookupNullable("b");
    var maybeValue3 = dictionary["a"].ToMaybe(); //throws exception on missing key

    //THEN
    maybeValue.Should().Be(Maybe<string>.Nothing);
    lookedUp.Should().Be(Maybe<string>.Nothing);
    maybeValue2.Should().Be(Maybe<string>.Nothing);
    lookedUp2.Should().Be(Maybe<string>.Nothing);
    maybeValue3.Should().Be(Maybe<string>.Nothing);
  }

  [Test]
  public void ShouldXXXXXXXXXXXXX21() //bug
  {
    //GIVEN
    var dictionary = new Dictionary<string, string>();
    //WHEN
    var lookedUp = dictionary.Lookup("a");
    var lookedUp2 = dictionary.Lookup("b");

    //THEN
    lookedUp.Should().Be(Maybe<string>.Nothing);
    lookedUp2.Should().Be(Maybe<string>.Nothing);
  }

  [Test]
  public void ShouldXXXXXXXXXXXXX3() //bug
  {
    var element1 = Maybe<int>.Nothing;
    var element2 = Maybe<int>.Nothing;
    var element3 = Maybe<int>.Nothing;
    var element4 = 5.Just();

    element1.Or(element2).Or(element3).Or(element4).Value().Should().Be(5);
    element1.Or(() => Maybe<int>.Nothing).Or(() => Maybe<int>.Nothing).Or(() => 5.Just()).Value().Should().Be(5);
  }

  [Test]
  public void ShouldXXXXXXXXXXXXX31() //bug
  {
    var nothing = Maybe<int>.Nothing;

    nothing.OrElse(5).Should().Be(5);
    nothing.OrElse(() => 5).Should().Be(5);
    nothing.Invoking(e => e.OrElse(() => new Exception())).Should().ThrowExactly<Exception>();

    nothing.OrElseDefault().Should().Be(0); //!
    Maybe<string>.Nothing.OrElseDefault().Should().Be(null); //!
  }

  [Test]
  public void ShouldXXXXXXXXXXXXX4() //bug
  {
    var element1 = 2.Just();
    var element2 = 4.Just();
    var element3 = Maybe<int>.Nothing;
    var element4 = 5.Just();

    var x = from e1 in element1 //bug watch coverage
            from e2 in element2
            from e3 in element3
            from e4 in element4
            select e1 + e2 + e3 + e4;

    x.Should().Be(Maybe<int>.Nothing);
  }

  [Test]
  public void ShouldXXXXXXXXXXXXX5() //bug
  {
    //bug explain maybe can accept superclasses!!
    "".ToMaybe<object>().Cast<object, string>().Value().Should().BeEmpty();

    //casting nothings it prohibited!
    Maybe<object>.Nothing.Invoking(m => m.Cast<object, string>().Value().Should().BeEmpty()).Should().Throw<Exception>();

    new List<int>().MaybeCast<object, string>().Should().Be(Maybe<string>.Nothing); //returns nothing on incompatible types
  }

  [Test]
  public void ShouldXXXXXXXXXXXXX6() //bug
  {
    var maybe = new[] { 1.Just(), default, 2.Just() };

    maybe.WhereValueExist().Should().Equal([1, 2]);
    maybe.SelectWhereValueExist(n => n + 1).Should().Equal([2, 3]);

    new[] { "1", "2", "2" }.FirstMaybe().Should().Be("1".Just());
    new[] { null, "2", "2" }.FirstMaybe().Should().Be(Maybe<string>.Nothing);
    Array.Empty<string>().FirstMaybe().Should().Be(Maybe<string>.Nothing);

    new[] { "1", "2", "2" }.SingleMaybe().Should().Be(Maybe<string>.Nothing); //!
    new[] { null, "2", "2" }.SingleMaybe().Should().Be(Maybe<string>.Nothing);
    Array.Empty<string>().SingleMaybe().Should().Be(Maybe<string>.Nothing);
  }

  [Test]
  public void ShouldXXXXXXXXXXXXX7() //bug
  {
    List<List<List<string?>>> nested =
    [
      [
        new List<string?>
        {
          "Hello!"
        }
      ]
    ];

    List<List<List<string?>>> nested2 = [];

    nested.ToMaybe()
        .Select(l1 => l1.FirstMaybe()
            .Select(l2 => l2.FirstMaybe()
                .Select(l3 => l3.FirstMaybe())))
        .Value().Value().Value().Value().Should().Be("Hello!");

    nested2.ToMaybe()
        .Select(l1 => l1.FirstMaybe()
            .Select(l2 => l2.FirstMaybe()
                .Select(l3 => l3.FirstMaybe())))
        .Value().IsNothing().Should().BeTrue();

    nested.ToMaybe()
        .SelectMany(l1 => l1.FirstMaybe())
        .SelectMany(l2 => l2.FirstMaybe())
        .SelectMany(l3 => l3.FirstMaybe())
        .Value().Should().Be("Hello!");
    nested2.ToMaybe()
        .SelectMany(l1 => l1.FirstMaybe())
        .SelectMany(l2 => l2.FirstMaybe())
        .SelectMany(l3 => l3.FirstMaybe())
        .Should().Be(Maybe<string>.Nothing);
    nested.ToMaybe() //alias for SelectMany
        .SelectMaybe(l1 => l1.FirstMaybe())
        .SelectMaybe(l2 => l2.FirstMaybe())
        .SelectMaybe(l3 => l3.FirstMaybe())
        .Value().Should().Be("Hello!");

    (from l1 in nested.ToMaybe()
     from l2 in l1.FirstMaybe()
     from l3 in l2.FirstMaybe()
     from l4 in l3.FirstMaybe()
     select l4).Value().Should().Be("Hello!");
    (from l1 in nested2.ToMaybe()
     from l2 in l1.FirstMaybe()
     from l3 in l2.FirstMaybe()
     from l4 in l3.FirstMaybe()
     select l4).Should().Be(Maybe<string>.Nothing);
  }

  [Test]
  public void ShouldXXXXXXXXXXXXX8() //bug
  {
    "".Just().Do(Console.WriteLine);

    "".Just().Do(Console.WriteLine);
    (null as string).ToMaybe().Do(s =>
    {
      Assert.Fail("Boo");
    });
  }

  [Test]
  public void ShouldAllowPatternMatchingWithRefType()
  {
    //GIVEN
    if ("a".Just() is var (str))
      str.Should().Be("a");
    else
      Assert.Fail();

    if ("a".Just() is var (_, _)) Assert.Fail();

    if (Maybe<string>.Nothing is var (str3)) Assert.Fail();

    if (Maybe<string>.Nothing is not var (str4))
    {

    }

    if (Maybe<string>.Nothing is var ())
    {

    }
    else
      Assert.Fail();
  }

  [Test]
  public void ShouldAllowPatternMatchingWithValueType()
  {
    //GIVEN
    if (1.Just() is var (num))
      num.Should().Be(1);
    else
      Assert.Fail();

    if (1.Just() is var (num1, num2)) Assert.Fail();

    if (Maybe<int>.Nothing is var (num3)) Assert.Fail();

    if (Maybe<int>.Nothing is not var (num4))
    {

    }

    if (Maybe<int>.Nothing is var ())
    {

    }
    else
      Assert.Fail();
  }
}