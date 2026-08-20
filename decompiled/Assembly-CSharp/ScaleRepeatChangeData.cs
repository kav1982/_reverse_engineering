using Unity.Entities;

public struct ScaleRepeatChangeData : IComponentData, IQueryTypeParameter
{
	public float BaseScale;

	public float TargetScale;

	public float ChangePeriod;

	public float TimeOffset;
}
