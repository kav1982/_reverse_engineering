using Unity.Entities;

public struct SelfPeriodRandomRotateData : IComponentData, IQueryTypeParameter
{
	public float ChangePeriod;

	public float Timer;
}
