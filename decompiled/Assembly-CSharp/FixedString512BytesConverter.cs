using System;
using Newtonsoft.Json;
using Unity.Collections;

public class FixedString512BytesConverter : JsonConverter<FixedString512Bytes>
{
	public override FixedString512Bytes ReadJson(JsonReader reader, Type objectType, FixedString512Bytes existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
		{
			return default(FixedString512Bytes);
		}
		return new FixedString512Bytes(reader.Value?.ToString() ?? string.Empty);
	}

	public override void WriteJson(JsonWriter writer, FixedString512Bytes value, JsonSerializer serializer)
	{
		writer.WriteValue(value.ToString());
	}
}
