using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Maybe.TextJson;

public class MaybeConverter<T> : JsonConverter<Maybe<T>> where T : notnull
{
  public override Maybe<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var jsonConverter = (JsonConverter<T>)options.GetConverter(typeof(T));
    if (reader.TokenType == JsonTokenType.Null)
    {
      return Maybe<T>.Nothing;
    }
    var item = jsonConverter.Read(ref reader, typeof(T), options);
    return item.ToMaybe();
  }

  public override void Write(Utf8JsonWriter writer, Maybe<T> maybe, JsonSerializerOptions options)
  {
    var jsonConverter = (JsonConverter<T>)options.GetConverter(typeof(T));
    if (maybe.HasValue)
    {
      jsonConverter.Write(writer, maybe.Value(), options);
    }
    else
    {
      writer.WriteNullValue();
    }
  }
}
