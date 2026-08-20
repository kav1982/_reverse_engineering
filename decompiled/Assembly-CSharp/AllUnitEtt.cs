using Unity.Collections;
using Unity.Entities;

[ChunkSerializable]
public struct AllUnitEtt : IComponentData, IQueryTypeParameter
{
	public NativeHashMap<int, Entity> map;
}
