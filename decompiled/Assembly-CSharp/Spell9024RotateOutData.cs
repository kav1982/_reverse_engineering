using Unity.Entities;
using Unity.Mathematics;

public struct Spell9024RotateOutData : IComponentData, IQueryTypeParameter
{
	public float rotateSpeedRatio;

	public float straightSpeedRatio;

	public float3 initialPoint;

	public bool Initialized;
}
