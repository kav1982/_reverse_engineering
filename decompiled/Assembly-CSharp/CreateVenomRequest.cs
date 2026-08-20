using Unity.Entities;
using Unity.Mathematics;

public struct CreateVenomRequest : IBufferElementData
{
	public bool isCircle;

	public float radius;

	public float3 point;

	public float3 point1;

	public float3 point2;

	public float duration;

	public CreateVenomRequest(float3 point, float radius, float duration)
	{
		this = default(CreateVenomRequest);
		point = Tool2D.IgnoreZPoint(point);
		isCircle = true;
		this.point = point;
		this.radius = radius;
		this.duration = duration;
	}

	public CreateVenomRequest(float3 point1, float3 point2, float radius, float duration)
	{
		this = default(CreateVenomRequest);
		point1 = Tool2D.IgnoreZPoint(point1);
		point2 = Tool2D.IgnoreZPoint(point2);
		isCircle = false;
		this.radius = radius;
		point = (point2 + point1) / 2f;
		this.point1 = point1;
		this.point2 = point2;
		this.duration = duration;
	}
}
