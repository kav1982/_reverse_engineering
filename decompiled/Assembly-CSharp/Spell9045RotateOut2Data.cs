using Unity.Entities;
using Unity.Mathematics;

public struct Spell9045RotateOut2Data : IComponentData, IQueryTypeParameter
{
	public float rotateSpeedRatio;

	public float originSpeedRatio;

	public float3 initialPoint;

	public bool Initialized;
}
