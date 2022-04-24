using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Maybe.TextJson;
using FluentAssertions;
using NUnit.Framework;

namespace Core.Maybe.Tests.System.Text.Json;

public class Tests
{
  private const string jsonWithNullPrimitive = "{\"AnInteger\":null}";

  [TestCase(null, null)]
  [TestCase("something", "something")]
  public void ShouldSerializeAndDeserializeComplexDataStructures(string member1, string member2)
  {
    var haveMaybe = new ObjectWithMaybeReferenceType
    {
      Whatever = new ObjectWithMaybeReferenceType.SomeData
      {
        Member1 = member1,
        Member2 = member2
      }.ToMaybe()
    };
    var serializedObject = JsonSerializer.Serialize(haveMaybe);

    var deSerialize = JsonSerializer.Deserialize<ObjectWithMaybeReferenceType>(serializedObject) ??
                      throw new Exception();
    deSerialize.Whatever.Value().Member1.Should().Be(haveMaybe.Whatever.Value().Member1);
    deSerialize.Whatever.Value().Member2.Should().Be(haveMaybe.Whatever.Value().Member2);
  }

  [Test]
  public void ShouldSerializeNothingOfPrimitiveValueAsNull()
  {
    var obj = new ObjectWithMaybePrimitiveType
    {
      AnInteger = Maybe<int>.Nothing
    };

    var serializedObject = JsonSerializer.Serialize(obj);
    serializedObject.Should().Be(jsonWithNullPrimitive);
  }

  [Test]
  public void ShouldSerializeJustOfPrimitiveValue()
  {
    var obj = new ObjectWithMaybePrimitiveType
    {
      AnInteger = 2.Just()
    };

    var serializedObject = JsonSerializer.Serialize(obj);
    serializedObject.Should().Be("{\"AnInteger\":2}");
  }

  [Test]
  public void ShouldDeserializeNullOfPrimitiveTypeAsNothing()
  {
    var deSerialize = JsonSerializer.Deserialize<ObjectWithMaybePrimitiveType>(jsonWithNullPrimitive) ??
                      throw new Exception();
    deSerialize.AnInteger.HasValue.Should().BeFalse();
  }

  [Test]
  public void ShouldDeserializeValueOfPrimitiveTypeAsJust()
  {
    var deSerialize = JsonSerializer.Deserialize<ObjectWithMaybePrimitiveType>("{\"AnInteger\":2}") ??
                      throw new Exception();
    deSerialize.AnInteger.Should().Be(2.Just());
  }

  [Test]
  public void ShouldDeserializeSerializedSubclassButWithoutSuperclassSupport()
  {
    var serializedObject = JsonSerializer.Serialize(
      new Root()
      {
        SO = new SubObject().Just(),
      });
    var deSerialize = JsonSerializer.Deserialize<Root>(serializedObject) ?? throw new Exception();
    deSerialize.SO.Value().Should().BeOfType<SubObject>();
    deSerialize.SO.Select(v => (SubObject)v).Value().B.Should().Be(321);
    deSerialize.SO2.Should().BeOfType<SuperObject>();
  }
}

public class Root
{
  [JsonConverter(typeof(MaybeConverter<SubObject>))]
  public Maybe<SubObject> SO { get; set; }

  public SuperObject SO2 { get; set; } = new SuperObject();

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