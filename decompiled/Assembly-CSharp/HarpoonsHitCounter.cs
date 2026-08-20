using Unity.Entities;

public struct HarpoonsHitCounter : IComponentData, IQueryTypeParameter
{
	public int HitCount;
}
