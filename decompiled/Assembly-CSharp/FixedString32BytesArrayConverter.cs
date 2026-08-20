using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Collections;
using Unity.Entities;

public class FixedString32BytesArrayConverter : JsonConverter<BlobAssetReference<BlobArray<FixedString32Bytes>>>
{
	public override BlobAssetReference<BlobArray<FixedString32Bytes>> ReadJson(JsonReader reader, Type objectType, BlobAssetReference<BlobArray<FixedString32Bytes>> existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (existingValue.IsCreated)
		{
			existingValue.Dispose();
		}
		List<string> list = serializer.Deserialize<List<string>>(reader);
		if (list == null)
		{
			return default(BlobAssetReference<BlobArray<FixedString32Bytes>>);
		}
		BlobAssetReference<BlobArray<FixedString32Bytes>> blobAssetReference = default(BlobAssetReference<BlobArray<FixedString32Bytes>>);
		using BlobBuilder blobBuilder = new BlobBuilder(Allocator.TempJob);
		BlobBuilderArray<FixedString32Bytes> blobBuilderArray = blobBuilder.Allocate(ref blobBuilder.ConstructRoot<BlobArray<FixedString32Bytes>>(), list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			blobBuilderArray[i] = list[i];
		}
		return blobBuilder.CreateBlobAssetReference<BlobArray<FixedString32Bytes>>(Allocator.Persistent);
	}

	public override void WriteJson(JsonWriter writer, BlobAssetReference<BlobArray<FixedString32Bytes>> value, JsonSerializer serializer)
	{
		string[] array = new string[value.Value.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = value.Value[i].ToString();
		}
		serializer.Serialize(writer, array);
	}
}
