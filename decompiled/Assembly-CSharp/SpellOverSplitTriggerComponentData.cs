using Unity.Entities;

public struct SpellOverSplitTriggerComponentData : IEnableableComponent, IComponentData, IQueryTypeParameter
{
	public Entity TriggerBufferEntity;
}
