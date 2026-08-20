using System;
using Newtonsoft.Json;
using Unity.Collections;

public class FixedString64BytesConverter : JsonConverter<FixedString64Bytes>
{
	public override FixedString64Bytes ReadJson(JsonReader reader, Type objectType, FixedString64Bytes existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
		{
			return default(FixedString64Bytes);
		}
		return new FixedString64Bytes(reader.Value?.ToString() ?? string.Empty);
	}

	public override void WriteJson(JsonWriter writer, FixedString64Bytes value, JsonSerializer serializer)
	{
		writer.WriteValue(value.ToString());
	}
}
