using Core.Maybe.Json;
using Newtonsoft.Json;

namespace Core.Maybe.Tests.Newtonsoft.Json;

public class Tests
{
  private const string jsonWithNullPrimitive = "{\"AnInteger\":null}";

  [TestCase(null, null)]
  [TestCase("something", "something")]
  public void ShouldSerializeAndDeserializeComplexDataStructures(string? member1, string? member2)
  {
    var haveMaybe = new ObjectWithMaybeReferenceType
    {
      Whatever = new ObjectWithMaybeReferenceType.SomeData
      {
        Member1 = member1,
        Member2 = member2
      }.Just()
    };
    var serializedObject = JsonConvert.SerializeObject(haveMaybe);

    var deserializeObject = JsonConvert.DeserializeObject<ObjectWithMaybeReferenceType>(serializedObject) ??
                            throw new Exception();
    deserializeObject.Whatever.Value().Member1.Should().Be(haveMaybe.Whatever.Value().Member1);
    deserializeObject.Whatever.Value().Member2.Should().Be(haveMaybe.Whatever.Value().Member2);
  }

  [Test]
  public void ShouldSerializeNothingOfPrimitiveValueAsNull()
  {
    var obj = new ObjectWithMaybePrimitiveType
    {
      AnInteger = Maybe<int>.Nothing
    };

    var serializedObject = JsonConvert.SerializeObject(obj);
    serializedObject.Should().Be(jsonWithNullPrimitive);
  }

  [Test]
  public void ShouldSerializeJustOfPrimitiveValue()
  {
    var obj = new ObjectWithMaybePrimitiveType
    {
      AnInteger = 2.Just()
    };

    var serializedObject = JsonConvert.SerializeObject(obj);
    serializedObject.Should().Be("{\"AnInteger\":2}");
  }

  [Test]
  public void ShouldDeserializeNullOfPrimitiveTypeAsNothing()
  {
    var deserializeObject = JsonConvert.DeserializeObject<ObjectWithMaybePrimitiveType>(jsonWithNullPrimitive) ??
                            throw new Exception();
    deserializeObject.AnInteger.HasValue.Should().BeFalse();
  }

  [Test]
  public void ShouldDeserializeValueOfPrimitiveTypeAsJust()
  {
    var deserializeObject = JsonConvert.DeserializeObject<ObjectWithMaybePrimitiveType>("{\"AnInteger\":2}") ??
                            throw new Exception();
    deserializeObject.AnInteger.Should().Be(2.Just());
  }

  [Test]
  public void ShouldDeserializeSubclassInstance()
  {
    var serializedObject = JsonConvert.SerializeObject(
      new Root()
      {
        SO = ((SuperObject)new SubObject()).ToMaybe(),
      });
    var deserializeObject = JsonConvert.DeserializeObject<Root>(serializedObject) ?? throw new Exception();
    deserializeObject.SO.Value().Should().BeOfType<SubObject>();
    deserializeObject.SO.Select(v => (SubObject)v).Value().B.Should().Be(321);
    deserializeObject.SO2.Should().BeOfType<SuperObject>();
  }
}

public class Root
{
  [JsonConverter(typeof(MaybeConverter<SuperObject>))]
  [JsonProperty("so", TypeNameHandling = TypeNameHandling.All)]
  public Maybe<SuperObject> SO { get; set; }

  [JsonProperty("so2")] public SuperObject SO2 { get; set; } = new SuperObject();

  public string Lol { get; set; } = default!;
}

public class SuperObject
{
  public int A = 123;
}

public class SubObject : SuperObject
{
  public int B = 321;
}