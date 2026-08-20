using Unity.Collections;
using Unity.Entities;

[ChunkSerializable]
public struct AllMixedEtt : IComponentData, IQueryTypeParameter
{
	public NativeHashMap<FixedString128Bytes, Entity> map;
}
