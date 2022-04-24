using System.Text.Json.Serialization;
using Core.Maybe.TextJson;

namespace Core.Maybe.Tests.System.Text.Json;

public class ObjectWithMaybeReferenceType
{
  [JsonConverter(typeof(MaybeConverter<SomeData>))]
  public Maybe<SomeData> Whatever { get; set; }

  public class SomeData
  {
    public string Member1 { get; set; } = default!;
    public string Member2 { get; set; } = default!;
  }
}