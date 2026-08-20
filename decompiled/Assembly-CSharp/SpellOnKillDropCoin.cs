using Unity.Entities;

public struct SpellOnKillDropCoin : IComponentData, IQueryTypeParameter
{
	public float DropRatio;
}
