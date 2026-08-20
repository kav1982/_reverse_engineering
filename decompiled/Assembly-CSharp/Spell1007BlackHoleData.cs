using Unity.Entities;

public struct Spell1007BlackHoleData : IComponentData, IQueryTypeParameter
{
	public bool IsInitialized;

	public Entity TrailEntity;

	public float implosionBonusDamageRatio;
}
