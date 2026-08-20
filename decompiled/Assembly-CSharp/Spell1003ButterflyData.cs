using Unity.Entities;

public struct Spell1003ButterflyData : IComponentData, IQueryTypeParameter
{
	public float InitialSpeed;

	public bool IsInitialize;

	public bool StartTraceTargets;
}
