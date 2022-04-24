using System;
using System.Text.Json;
using Core.Maybe.TextJson;
using NUnit.Framework;

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
			
    Assert.AreEqual("{\"Name\":\"Test\"}", json);
  }
		
  [Test]
  public void CanDeSerialize()
  {
    var settings = new JsonSerializerOptions();
    settings.Converters.Add(new MaybeConverter<string>());
    var obj = JsonSerializer.Deserialize<MyClass>("{\"Name\":\"Test\"}", settings);
			
    Assert.AreEqual("Test".ToMaybe(), (obj ?? throw new Exception()).Name);
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
			
    Assert.AreEqual("Test".ToMaybe(), (obj ?? throw new Exception()).Something.Name);
  }
}

internal class MyClass
{
  public MyClass(Maybe<string> name)
  {
    Name = name;
  }

  public Maybe<string> Name { get; }
}
	
internal class MyContainer
{
  public MyClass Something { get; }

  public MyContainer(MyClass something)
  {
    Something = something;
  }
}