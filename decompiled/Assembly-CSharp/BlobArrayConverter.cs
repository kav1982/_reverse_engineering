using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Collections;
using Unity.Entities;

public class BlobArrayConverter<T> : JsonConverter<BlobAssetReference<BlobArray<T>>> where T : struct
{
	public override BlobAssetReference<BlobArray<T>> ReadJson(JsonReader reader, Type objectType, BlobAssetReference<BlobArray<T>> existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (existingValue.IsCreated)
		{
			existingValue.Dispose();
		}
		List<T> list = serializer.Deserialize<List<T>>(reader);
		if (list == null)
		{
			return default(BlobAssetReference<BlobArray<T>>);
		}
		BlobAssetReference<BlobArray<T>> blobAssetReference = default(BlobAssetReference<BlobArray<T>>);
		using BlobBuilder blobBuilder = new BlobBuilder(Allocator.TempJob);
		BlobBuilderArray<T> blobBuilderArray = blobBuilder.Allocate(ref blobBuilder.ConstructRoot<BlobArray<T>>(), list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			blobBuilderArray[i] = list[i];
		}
		return blobBuilder.CreateBlobAssetReference<BlobArray<T>>(Allocator.Persistent);
	}

	public override void WriteJson(JsonWriter writer, BlobAssetReference<BlobArray<T>> value, JsonSerializer serializer)
	{
		T[] array = new T[value.Value.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = value.Value[i];
		}
		serializer.Serialize(writer, array);
	}
}
