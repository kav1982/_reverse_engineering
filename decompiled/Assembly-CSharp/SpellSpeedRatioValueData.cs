using Unity.Entities;

public struct SpellSpeedRatioValueData : IComponentData, IQueryTypeParameter
{
	public AttributeValue Speed;
}
