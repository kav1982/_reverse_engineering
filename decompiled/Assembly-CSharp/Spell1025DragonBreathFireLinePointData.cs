using Unity.Entities;

public struct Spell1025DragonBreathFireLinePointData : IComponentData, IQueryTypeParameter
{
	public float offset;

	public Entity Parent;

	public int CurrentIndex;

	public float CurrentPercent;
}
