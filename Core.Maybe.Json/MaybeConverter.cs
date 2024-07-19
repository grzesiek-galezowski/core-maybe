using System;
using Newtonsoft.Json;

namespace Core.Maybe.Json;

public class MaybeConverter<T> : JsonConverter<Maybe<T>> where T : notnull
{
  public override void WriteJson(JsonWriter writer, Maybe<T> value, JsonSerializer serializer)
  {
    if (value.HasValue)
    {
      if (value.Value().GetType() == typeof(T))
      {
        serializer.Serialize(writer, value.Value());
      }
      else
      {
        var previousTypeNameHandling = serializer.TypeNameHandling;
        serializer.TypeNameHandling = TypeNameHandling.All;
        serializer.Serialize(writer, value.Value());
        serializer.TypeNameHandling = previousTypeNameHandling;
      }
    }
    else
    {
      serializer.Serialize(writer, null);
    }
  }

  public override Maybe<T> ReadJson(JsonReader reader, Type objectType, Maybe<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
  {
    if (reader.TokenType == JsonToken.Null)
    {
      return Maybe<T>.Nothing;
    }

    var previousTypeHandling = serializer.TypeNameHandling;
    serializer.TypeNameHandling = TypeNameHandling.Auto;
    var value = serializer.Deserialize<T>(reader);
    serializer.TypeNameHandling = previousTypeHandling;
    return value.ToMaybe();
  }
}