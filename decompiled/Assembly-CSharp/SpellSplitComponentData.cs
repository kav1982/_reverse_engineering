using Unity.Entities;

public struct SpellSplitComponentData : IComponentData, IQueryTypeParameter
{
	public int Count;

	public float DamageRatio;
}
