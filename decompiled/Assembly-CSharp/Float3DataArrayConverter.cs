using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Collections;
using Unity.Entities;

public class Float3DataArrayConverter : JsonConverter<BlobAssetReference<BlobArray<Float3Data>>>
{
	public override BlobAssetReference<BlobArray<Float3Data>> ReadJson(JsonReader reader, Type objectType, BlobAssetReference<BlobArray<Float3Data>> existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (existingValue.IsCreated)
		{
			existingValue.Dispose();
		}
		List<Float3Data> list = serializer.Deserialize<List<Float3Data>>(reader);
		if (list == null)
		{
			return default(BlobAssetReference<BlobArray<Float3Data>>);
		}
		BlobAssetReference<BlobArray<Float3Data>> blobAssetReference = default(BlobAssetReference<BlobArray<Float3Data>>);
		using BlobBuilder blobBuilder = new BlobBuilder(Allocator.TempJob);
		BlobBuilderArray<Float3Data> blobBuilderArray = blobBuilder.Allocate(ref blobBuilder.ConstructRoot<BlobArray<Float3Data>>(), list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			blobBuilderArray[i] = list[i];
		}
		return blobBuilder.CreateBlobAssetReference<BlobArray<Float3Data>>(Allocator.Persistent);
	}

	public override void WriteJson(JsonWriter writer, BlobAssetReference<BlobArray<Float3Data>> value, JsonSerializer serializer)
	{
		writer.WriteStartArray();
		for (int i = 0; i < value.Value.Length; i++)
		{
			Float3Data float3Data = value.Value[i];
			writer.WriteStartObject();
			writer.WritePropertyName("x");
			writer.WriteValue(float3Data.x);
			writer.WritePropertyName("y");
			writer.WriteValue(float3Data.y);
			writer.WritePropertyName("z");
			writer.WriteValue(float3Data.z);
			writer.WriteEndObject();
		}
		writer.WriteEndArray();
	}
}
