using Unity.Entities;
using Unity.Mathematics;

public struct EndlessItemPick : IComponentData, IQueryTypeParameter
{
	public float3 startPoint;

	public float lerpTimer;

	public float startLerpTime;

	public float lerpTime;
}
