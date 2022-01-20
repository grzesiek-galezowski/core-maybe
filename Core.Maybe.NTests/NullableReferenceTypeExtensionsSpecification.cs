using System;
using System.Threading.Tasks;
using Core.NullableReferenceTypesExtensions;
using FluentAssertions;
using NUnit.Framework;

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
            .WithMessage("Could not convert instance of type System.String? to non-nullable reference type because it is null");
        await Task.FromResult("").OrThrow();
        this.Invoking(_ => (null as string).OrThrow())
            .Should().ThrowExactly<InvalidOperationException>()
            .WithMessage("Could not convert instance of type System.String? to non-nullable reference type because it is null");
    }
}