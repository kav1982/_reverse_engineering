using Unity.Entities;

public struct AutoDestroy : IComponentData, IQueryTypeParameter
{
	public float duration;

	public float durationTimer;
}
