using Core.Maybe.TextJson;
using System.Text.Json;

namespace Core.Maybe.Tests.System.Text.Json;

[TestFixture]
public class CanSerializeBothWays
{
  [Test]
  public void CanSerialize()
  {
    var settings = new JsonSerializerOptions();
    settings.Converters.Add(new MaybeConverter<string>());
    var json = JsonSerializer.Serialize(new MyClass("Test".ToMaybe()), settings);

    json.Should().Be("{\"Name\":\"Test\"}");
  }

  [Test]
  public void CanDeSerialize()
  {
    var settings = new JsonSerializerOptions();
    settings.Converters.Add(new MaybeConverter<string>());
    var obj = JsonSerializer.Deserialize<MyClass>("{\"Name\":\"Test\"}", settings);

    (obj ?? throw new Exception()).Name.Should().Be("Test".ToMaybe());
  }


  [Test]
  public void CanDealWithContainer()
  {
    var settings = new JsonSerializerOptions();
    settings.Converters.Add(new MaybeConverter<string>());
    var obj = JsonSerializer.Deserialize<MyContainer>(
      JsonSerializer.Serialize(new MyContainer(new MyClass("Test".ToMaybe())), settings),
      settings
    );

    (obj ?? throw new Exception()).Something.Name.Should().Be("Test".ToMaybe());
  }
}

internal class MyClass(Maybe<string> name)
{
  public Maybe<string> Name { get; } = name;
}

internal class MyContainer(MyClass something)
{
  public MyClass Something { get; } = something;
}