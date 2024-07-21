using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core.Maybe.Tests;

[TestFixture]
public class MaybeEnumerableTests
{
  [Test]
  public void WhereValueExist_Should_remove_Nothing_values()
  {
    var sequence = new[] { 1.ToMaybe(), Maybe<int>.Nothing, 2.ToMaybe() };
    int[] expected = [1, 2];

    var actual = sequence.WhereValueExist().ToArray();

    actual.Should().HaveCount(expected.Length);
    for (var i = 0; i < expected.Length; i++)
    {
      actual[i].Should().Be(expected[i]);
    }
  }

  [Test]
  public void SelectWhereValueExist_Should_work_with_null_returned_from_transformation()
  {
    var sequence = new[] { "a".ToMaybe(), "b".ToMaybe(), "c".ToMaybe() };
    var expected = new string?[] { null, null, null };

    var actual = sequence.SelectWhereValueExist<string, string?>(s => null).ToArray();

    actual.Should().Equal(expected);
  }

  [Test]
  public void Given_ThreeSome_UnionReturnsCollectionOfAll()
  {
    var one = 1.ToMaybe();
    var two = 2.ToMaybe();
    var three = 3.ToMaybe();

    var res = one.Union(two, three);
    res.Should().HaveCount(3);
    res.SequenceEqual([1, 2, 3]).Should().BeTrue();
  }

  [Test]
  public void Given_EnumerableOfMaybes_SelectShouldWorkWithNullValues()
  {
    var enumerable = new[] { "a".ToMaybe(), "b".ToMaybe(), "c".ToMaybe() };

    var results = enumerable.Select<string, string>(s => null);

    results.Should()
      .Equal(
        Maybe<string>.Nothing,
        Maybe<string>.Nothing,
        Maybe<string>.Nothing);
  }

  [Test]
  public void Given_OneSome_UnionReturnsCollectionOfOne()
  {
    var one = 1.ToMaybe();
    var two = Maybe<int>.Nothing;

    var res = one.Union(two);
    res.Should().ContainSingle();
    res.SequenceEqual([1]).Should().BeTrue();
  }

  [Test]
  public void Given_CollectionAndSome_UnionReturnsCollectionPlusSome()
  {
    var one = new[] { 1, 3 };
    var two = 2.ToMaybe();

    var res = one.Union(two);
    res.Should().HaveCount(3);
    res.SequenceEqual([1, 3, 2]).Should().BeTrue();
  }

  [Test]
  public void FirstMaybe_WhenCalledOnEmptyEnumerable_ReturnsNothing()
  {
    var maybe = Enumerable.Empty<object>().FirstMaybe();

    maybe.IsNothing().Should().BeTrue();
  }

  [Test]
  public void FirstMaybe_WhenCalledOnEmptyEnumerableOfNrts_ReturnsNothing()
  {
    var maybe = Enumerable.Empty<object?>().FirstMaybe();

    maybe.IsNothing().Should().BeTrue();
  }

  [Test]
  public void FirstMaybe_WhenCalledOnEnumerableWithNoMatches_ReturnsNothing()
  {
    var collection = new[] { 1, 2 };
    var itemToSearch = 3;

    var maybe = collection.FirstMaybe(i => i == itemToSearch);

    maybe.IsNothing().Should().BeTrue();
  }

  [Test]
  public void FirstMaybe_WhenCalledOnEnumerableWithMatches_ReturnsFirstMatchingElement()
  {
    var expectedItem = Tuple.Create(2);
    var collection = new[]
    {
      Tuple.Create(1),
      expectedItem, // first matching element
      Tuple.Create(2), // last matching element
      Tuple.Create(3)
    };

    var maybe = collection.FirstMaybe(i => i.Item1 == 2);

    maybe.IsSomething().Should().BeTrue();
    // use AreSame to compare expected value with actual one by reference
    maybe.Value().Should().BeSameAs(expectedItem);
  }

  [Test]
  public void SingleMaybe_WhenCalledOnEmptyEnumerable_ReturnsNothing()
  {
    var maybe = Enumerable.Empty<object>().SingleMaybe();

    maybe.IsNothing().Should().BeTrue();
  }

  [Test]
  public void SingleMaybe_WhenCalledOnEnumerableWithNoMatches_ReturnsNothing()
  {
    var collection = new[] { 1, 2 };
    var itemToSearch = 3;

    var maybe = collection.SingleMaybe(i => i == itemToSearch);

    maybe.IsNothing().Should().BeTrue();
  }

  [Test]
  public void SingleMaybe_WhenCalledOnNonEmptyEnumerableWithMultipleMatches_ReturnsNothing()
  {
    var collection = new[] { 1, 1, 2 };
    var itemToSearch = 1;

    var maybe = collection.SingleMaybe(i => i == itemToSearch);

    maybe.IsNothing().Should().BeTrue();
  }

  [Test]
  public void SingleMaybe_WhenCalledOnNonEmptyEnumerableWithSingleMatch_ReturnsSingleMatchingElement()
  {
    var collection = new[] { 1, 2, 3 };
    var itemToSearch = 2;

    var maybe = collection.SingleMaybe(i => i == itemToSearch);

    maybe.IsSomething().Should().BeTrue();
    maybe.Value().Should().Be(itemToSearch);
  }

  [Test]
  public void SingleMaybe_WhenSubtypeSpecified_ReturnsDowncastedItem()
  {
    var collection = new object[] { "3", 2 };
    var itemToSearch = "3";

    var maybe = collection.SingleMaybe<object, string>(i => i.Equals(itemToSearch));

    maybe.IsSomething().Should().BeTrue();
    maybe.Value().Should().Be(itemToSearch);
  }

  [Test]
  public void SingleMaybe_WhenWrongSubtypeIsSpecified_ReturnsNothing()
  {
    var collection = new object[] { "3", 2 };
    var itemToSearch = "3";

    var maybe = collection.SingleMaybe<object, Regex>(i => i.Equals(itemToSearch));

    maybe.IsNothing().Should().BeTrue();
  }

  [Test]
  public void LastMaybe_WhenCalledOnEmptyEnumerable_ReturnsNothing()
  {
    var maybe = Enumerable.Empty<object>().LastMaybe();

    maybe.IsNothing().Should().BeTrue();
  }

  [Test]
  public void LastMaybe_WhenCalledOnEnumerableWithNoMatches_ReturnsNothing()
  {
    var collection = new[] { 1, 2 };
    var itemToSearch = 3;

    var maybe = collection.LastMaybe(i => i == itemToSearch);

    maybe.IsNothing().Should().BeTrue();
  }

  [Test]
  public void LastMaybe_WhenCalledOnEnumerableWithMatches_ReturnsLastMatchingElement()
  {
    var expectedItem = Tuple.Create(2);
    var collection = new[]
    {
      Tuple.Create(1),
      Tuple.Create(2), // first matching element
      expectedItem, // last matching element
      Tuple.Create(3)
    };

    var maybe = collection.LastMaybe(i => i.Item1 == 2);

    maybe.IsSomething().Should().BeTrue();
    // use AreSame to compare expected value with actual one by reference
    maybe.Value().Should().BeSameAs(expectedItem);
  }

  [Test]
  public void FromMaybe_WhenInvokedOnNothingOfEnumerableOfNrts_ReturnsEmptyEnumerableOfNrts()
  {
    var maybe = Maybe<IEnumerable<string?>>.Nothing;

    var result = maybe.FromMaybe();

    result.Should().BeEmpty();
  }

  [Test]
  public void FromMaybe_WhenInvokedOnMaybeOfEmptyEnumerableOfNrts_ReturnsEmptyEnumerableOfNrts()
  {
    var maybe = new List<string?>().ToMaybe<IEnumerable<string?>>();

    var result = maybe.FromMaybe();

    result.Should().BeEmpty();
  }
}