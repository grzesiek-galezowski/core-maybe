using Core.Maybe.Json;
using Newtonsoft.Json;

namespace Core.Maybe.Tests.Newtonsoft.Json;

public class ObjectWithMaybePrimitiveType
{
  [JsonConverter(typeof(MaybeConverter<int>))]
  public Maybe<int> AnInteger { get; set; }
}