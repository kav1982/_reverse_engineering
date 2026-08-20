using Unity.Entities;

public struct ItemDelayActiveTriggerData : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public bool isCoin;

	public float DelayTimer;
}
