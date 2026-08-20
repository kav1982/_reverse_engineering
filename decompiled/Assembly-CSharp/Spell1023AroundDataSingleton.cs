using Unity.Collections;
using Unity.Entities;

[ChunkSerializable]
public struct Spell1023AroundDataSingleton : IComponentData, IQueryTypeParameter
{
	public NativeHashMap<Entity, NativeList<Entity>> Data;

	public NativeHashMap<Entity, Spell1023OwnerData> BladeDetectTargetData;
}
