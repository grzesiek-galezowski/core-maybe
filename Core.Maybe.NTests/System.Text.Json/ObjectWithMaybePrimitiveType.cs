using System.Text.Json.Serialization;
using Core.Maybe.TextJson;

namespace Core.Maybe.Tests.System.Text.Json;

public class ObjectWithMaybePrimitiveType
{
  [JsonConverter(typeof(MaybeConverter<int>))]
  public Maybe<int> AnInteger { get; set; }
}