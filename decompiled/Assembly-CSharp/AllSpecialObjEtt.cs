using Unity.Collections;
using Unity.Entities;

[ChunkSerializable]
public struct AllSpecialObjEtt : IComponentData, IQueryTypeParameter
{
	public NativeHashMap<int, Entity> map;
}
