using Unity.Entities;

public struct SpellOnKillDropCrystal : IComponentData, IQueryTypeParameter
{
	public float DropRatio;
}
