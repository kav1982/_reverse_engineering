using Unity.Entities;
using Unity.Mathematics;

public struct Monster320_Dots : IComponentData, IQueryTypeParameter
{
	public bool Initialized;

	public float3 moveDir;

	public float moveTimer;

	public float floatTimer;

	public float3 floatRootOriginPos;
}
