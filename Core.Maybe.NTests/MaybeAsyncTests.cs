using System.Threading.Tasks;

namespace Core.Maybe.Tests;

[TestFixture]
public class MaybeAsyncTests
{
  [Test]
  public async Task SelectAsyncTest()
  {
    static Task<int> Two() => Task.FromResult(2);

    var onePlusTwo = await 1.ToMaybe().SelectAsync(async one => one + (await Two()));

    ClassicAssert.AreEqual(3, onePlusTwo.Value());
  }

  [Test]
  public async Task SelectAsyncWithTransformationReturningNullTest()
  {
    static Task<string?> GetNull() => Task.FromResult<string?>(null);

    var result = await "a".ToMaybe().SelectAsync(async _ => await GetNull());

    ClassicAssert.AreEqual(Maybe<string>.Nothing, result);
  }

  [Test]
  public async Task MatchAsyncWithValueTransformationReturningNullTest()
  {
    static Task<string?> GetNull() => Task.FromResult<string?>(null);

    var result = await "a".ToMaybe().MatchAsync(
      async _ => await GetNull(),
      () => Task.FromResult<string?>("a"));

    ClassicAssert.IsNull(result);
  }

  [Test]
  public async Task MatchAsyncWithAlternativeFunctionReturningNullTest()
  {
    static Task<string?> GetNull() => Task.FromResult<string?>(null);

    var result = await Maybe<string>.Nothing.MatchAsync(
      _ => Task.FromResult<string?>("a"),
      async () => await GetNull());

    ClassicAssert.IsNull(result);
  }

  [Test]
  public async Task OrElseWithNullAlternativeValueTest()
  {
    var taskOfMaybe = Task.FromResult(Maybe<string>.Nothing);

    var result = await taskOfMaybe.OrElseNullable(null as string);

    ClassicAssert.IsNull(result);
  }

  [Test]
  public async Task OrElseAsyncWithNullAlternativeValueTest()
  {
    var taskOfMaybe = Task.FromResult(Maybe<string>.Nothing);

    var result = await taskOfMaybe.OrElseAsync(
      () => Task.FromResult(null as string));

    ClassicAssert.IsNull(result);
  }

  [Test]
  public async Task OrElseWithNullAlternativeFuncResultTest()
  {
    var taskOfMaybe = Task.FromResult(Maybe<string>.Nothing);

    var result = await taskOfMaybe.OrElseNullable(() => null);

    ClassicAssert.IsNull(result);
  }
}