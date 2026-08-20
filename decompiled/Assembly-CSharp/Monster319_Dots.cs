using Unity.Entities;
using Unity.Mathematics;

public struct Monster319_Dots : IComponentData, IQueryTypeParameter
{
	public bool Initialized;

	public float3 moveDir;

	public float moveTimer;

	public float attackCDTimer;

	public float floatTimer;

	public float3 floatRootOriginPos;

	public float ballScale;

	public Entity ballEntity;
}
