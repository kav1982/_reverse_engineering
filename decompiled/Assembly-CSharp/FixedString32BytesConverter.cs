using System;
using Newtonsoft.Json;
using Unity.Collections;

public class FixedString32BytesConverter : JsonConverter<FixedString32Bytes>
{
	public override FixedString32Bytes ReadJson(JsonReader reader, Type objectType, FixedString32Bytes existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
		{
			return default(FixedString32Bytes);
		}
		return new FixedString32Bytes(reader.Value?.ToString() ?? string.Empty);
	}

	public override void WriteJson(JsonWriter writer, FixedString32Bytes value, JsonSerializer serializer)
	{
		writer.WriteValue(value.ToString());
	}
}
