using Unity.Burst;

[BurstCompile]
public struct RadiusAttributeValue
{
	public float Base;

	public float FallRadius;

	public float AddRatio;

	public float MulRatio;

	public float Extra;

	public RadiusAttributeValue(float baseValue = 0f)
	{
		this = default(RadiusAttributeValue);
		Base = baseValue;
		MulRatio = 1f;
	}

	[BurstCompile]
	public readonly float Calculate()
	{
		return (Base + FallRadius) * (1f + AddRatio) * MulRatio + Extra;
	}

	[BurstCompile]
	public readonly float CalculateIgnoreFall()
	{
		return Base * (1f + AddRatio) * MulRatio + Extra;
	}
}
