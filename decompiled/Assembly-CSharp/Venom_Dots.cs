using Unity.Entities;
using Unity.Mathematics;

public struct Venom_Dots : IComponentData, IQueryTypeParameter
{
	public float radius;

	public float duration;

	public float durationTimer;

	public bool durationFinish;

	public bool isRectangle;

	public float3 rectangleDir;

	public float rectangleDistance;

	public float createBubbleTimer;

	public float createBubbleSpeed;
}
