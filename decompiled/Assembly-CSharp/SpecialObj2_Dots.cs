using Unity.Entities;

public struct SpecialObj2_Dots : IComponentData, IQueryTypeParameter
{
	public float radius;

	public float checkIntervalTimer;

	public float checkInterval;
}
