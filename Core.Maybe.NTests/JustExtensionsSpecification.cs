using System.Threading.Tasks;

namespace Core.Maybe.Tests;

public class JustExtensionsSpecification
{
  [Test]
  public async Task Test1()
  {
    string? nullString = null;
    ClassicAssert.Throws<ArgumentNullException>(() => nullString.Just());
    ClassicAssert.AreEqual("a".ToMaybe(), "a".Just());
    ClassicAssert.AreEqual("a", "a".Just().Value());
    var exceptionFromJust = Assert.Throws<ArgumentNullException>(() => nullString.Just()).ToMaybe();
    exceptionFromJust.Value().Message.Should().Be("Cannot create a Just<System.String>, because expression {nullString} is null (Parameter 'value')");
    ClassicAssert.AreEqual("a", (await Task.FromResult<string?>("a").JustAsync()).Value());
    ClassicAssert.AreEqual(1, (await Task.FromResult(1).JustAsync()).Value());
    var argumentNullException = Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(nullString).JustAsync()).ToMaybe();
    argumentNullException.Value().Message.Should().Be("Cannot create a Just<System.String>, because expression {Task.FromResult(nullString)} is null (Parameter 'value')");
  }
}