using Core.Maybe.Json;
using Newtonsoft.Json;

namespace Core.Maybe.Tests.Newtonsoft.Json;

[TestFixture]
public class CanSerializeBothWays
{
  [Test]
  public void CanSerialize()
  {
    var settings = new JsonSerializerSettings();
    settings.Converters.Add(new MaybeConverter<string>());
    var json = JsonConvert.SerializeObject(new MyClass("Test".ToMaybe()), settings);

    ClassicAssert.AreEqual("{\"Name\":\"Test\"}", json);
  }

  [Test]
  public void CanDeSerialize()
  {
    var settings = new JsonSerializerSettings();
    settings.Converters.Add(new MaybeConverter<string>());
    var obj = JsonConvert.DeserializeObject<MyClass>("{\"Name\":\"Test\"}", settings);

    ClassicAssert.AreEqual("Test".ToMaybe(), (obj ?? throw new Exception()).Name);
  }

  [Test]
  public void CanDealWithContainer()
  {
    var settings = new JsonSerializerSettings();
    settings.Converters.Add(new MaybeConverter<string>());
    var obj = JsonConvert.DeserializeObject<MyContainer>(
      JsonConvert.SerializeObject(new MyContainer(new MyClass("Test".ToMaybe())), settings),
      settings
    );

    ClassicAssert.AreEqual("Test".ToMaybe(), (obj ?? throw new Exception()).Something.Name);
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