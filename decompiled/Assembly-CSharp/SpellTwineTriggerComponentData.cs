using Unity.Entities;

public struct SpellTwineTriggerComponentData : IEnableableComponent, IComponentData, IQueryTypeParameter
{
	public int Count;

	public float Radius;
}
