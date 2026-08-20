using Unity.Burst;

[BurstCompile]
public struct AttributeValue
{
	public float Base;

	public float AddBase;

	public float AddRatio;

	public float MulRatio;

	public float Extra;

	public AttributeValue(float baseValue = 0f)
	{
		this = default(AttributeValue);
		Base = baseValue;
		MulRatio = 1f;
	}

	[BurstCompile]
	public readonly float Calculate()
	{
		return (Base + AddBase) * (1f + AddRatio) * MulRatio + Extra;
	}
}
