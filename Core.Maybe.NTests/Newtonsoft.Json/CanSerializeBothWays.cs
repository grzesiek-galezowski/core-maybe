using System;
using Core.Maybe.Json;
using Newtonsoft.Json;
using NUnit.Framework;

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

    Assert.AreEqual("{\"Name\":\"Test\"}", json);
  }
		
  [Test]
  public void CanDeSerialize()
  {
    var settings = new JsonSerializerSettings();
    settings.Converters.Add(new MaybeConverter<string>());
    var obj = JsonConvert.DeserializeObject<MyClass>("{\"Name\":\"Test\"}", settings);

    Assert.AreEqual("Test".ToMaybe(), (obj ?? throw new Exception()).Name);
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