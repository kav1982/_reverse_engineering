using System;
using Newtonsoft.Json;
using Unity.Collections;

public class FixedString128BytesConverter : JsonConverter<FixedString128Bytes>
{
	public override FixedString128Bytes ReadJson(JsonReader reader, Type objectType, FixedString128Bytes existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
		{
			return default(FixedString128Bytes);
		}
		return new FixedString128Bytes(reader.Value?.ToString() ?? string.Empty);
	}

	public override void WriteJson(JsonWriter writer, FixedString128Bytes value, JsonSerializer serializer)
	{
		writer.WriteValue(value.ToString());
	}
}
