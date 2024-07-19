using System.Threading.Tasks;
using Core.NullableReferenceTypesExtensions;

namespace Core.Maybe.Tests;

public class NullableReferenceTypeExtensionsSpecification
{
  [Test]
  public async Task METHOD()
  {
    (await Task.FromResult("")).OrThrow();
    await (Task.FromResult<string?>("").OrThrowAsync());
    await this.Awaiting(_ => Task.FromResult<string?>(null).OrThrowAsync())
        .Should().ThrowExactlyAsync<InvalidOperationException>()
        .WithMessage("Could not convert the result of {Task.FromResult<string?>(null)} of type System.String? to non-nullable reference type because it is null");
    await Task.FromResult("").OrThrow();
    this.Invoking(_ => (null as string).OrThrow())
        .Should().ThrowExactly<InvalidOperationException>()
        .WithMessage("Could not convert the result of {null as string} of type System.String? to non-nullable reference type because it is null");
  }
}