using Unity.Entities;

public struct ItemPopData : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public float Timer;

	public float MaxHeight;

	public bool IsFinish;
}
