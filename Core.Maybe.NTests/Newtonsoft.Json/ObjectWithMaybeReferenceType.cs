using Core.Maybe.Json;
using Newtonsoft.Json;

namespace Core.Maybe.Tests.Newtonsoft.Json;

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