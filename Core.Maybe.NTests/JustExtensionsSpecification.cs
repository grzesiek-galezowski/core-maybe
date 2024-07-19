using FluentAssertions;
using System.Threading.Tasks;

namespace Core.Maybe.Tests;

public class JustExtensionsSpecification
{
  [Test]
  public async Task Test1()
  {
    string? nullString = null;
    FluentActions.Invoking(() => nullString.Just()).Should().ThrowExactly<ArgumentNullException>();
    "a".Just().Should().Be("a".ToMaybe());
    "a".Just().Value().Should().Be("a");
    FluentActions.Invoking(() => nullString.Just())
      .Should().ThrowExactly<ArgumentNullException>()
      .WithMessage(
        "Cannot create a Just<System.String>, because expression {nullString} is null (Parameter 'value')");
    (await Task.FromResult<string?>("a").JustAsync()).Value().Should().Be("a");
    (await Task.FromResult(1).JustAsync()).Value().Should().Be(1);
    await FluentActions.Awaiting(() => Task.FromResult(nullString).JustAsync()).Should()
      .ThrowExactlyAsync<ArgumentNullException>().WithMessage(
        "Cannot create a Just<System.String>, because expression {Task.FromResult(nullString)} is null (Parameter 'value')");
  }
}