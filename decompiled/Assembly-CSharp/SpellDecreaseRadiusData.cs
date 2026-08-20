using Unity.Entities;

public struct SpellDecreaseRadiusData : IComponentData, IQueryTypeParameter
{
	public float RadiusMult;

	public float RadiusToDamageRatio;
}
