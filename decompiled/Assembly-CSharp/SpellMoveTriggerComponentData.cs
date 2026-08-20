using Unity.Entities;

public struct SpellMoveTriggerComponentData : IEnableableComponent, IComponentData, IQueryTypeParameter
{
	public float DistanceCounter;

	public float TriggerDistanceRatio;

	public float SubGroupMpCost;

	public bool TriggerDirectionFlag;
}
