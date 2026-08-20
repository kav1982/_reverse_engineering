using Unity.Entities;
using Unity.Mathematics;

public struct Monster8_Dots_Amaze : IComponentData, IQueryTypeParameter
{
	public bool informOthers;

	public Entity targetEtt;

	public float3 informPosition;
}
