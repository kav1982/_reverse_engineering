using Unity.Entities;

public struct SpellKeepCastingCleanup : ICleanupComponentData, IComponentData, IQueryTypeParameter
{
	public Entity OwnerUnit;
}
