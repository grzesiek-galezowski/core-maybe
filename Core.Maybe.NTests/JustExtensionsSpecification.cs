using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Core.Maybe.Tests;

public class JustExtensionsSpecification
{
    [Test]
    public async Task Test1()
    {
        string? nullString = null;
        Assert.Throws<ArgumentNullException>(() => nullString.Just());
        Assert.AreEqual("a".ToMaybe(), "a".Just());
        Assert.AreEqual("a", "a".Just().Value());
        var exceptionFromJust = Assert.Throws<ArgumentNullException>(() => nullString.Just()).ToMaybe();
        exceptionFromJust.Value().Message.Should().Be("Cannot create a Just<System.String> from null (Parameter 'value')");
        Assert.AreEqual("a", (await Task.FromResult<string?>("a").JustAsync()).Value());
        Assert.AreEqual(1, (await Task.FromResult(1).JustAsync()).Value());
        var argumentNullException = Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(nullString).JustAsync()).ToMaybe();
        argumentNullException.Value().Message.Should().Be("Cannot create a Just<System.String> from null (Parameter 'value')");
    }
}