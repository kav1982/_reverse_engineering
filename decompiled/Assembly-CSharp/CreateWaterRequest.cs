using Unity.Entities;
using Unity.Mathematics;

public struct CreateWaterRequest : IBufferElementData
{
	public bool isCircle;

	public float radius;

	public float3 point;

	public float3 point1;

	public float3 point2;

	public CreateWaterRequest(float3 point, float radius)
	{
		this = default(CreateWaterRequest);
		point = Tool2D.IgnoreZPoint(point);
		isCircle = true;
		this.point = point;
		this.radius = radius;
	}

	public CreateWaterRequest(float3 point1, float3 point2, float radius)
	{
		this = default(CreateWaterRequest);
		point1 = Tool2D.IgnoreZPoint(point1);
		point2 = Tool2D.IgnoreZPoint(point2);
		isCircle = false;
		this.radius = radius;
		point = (point2 + point1) / 2f;
		this.point1 = point1;
		this.point2 = point2;
	}
}
