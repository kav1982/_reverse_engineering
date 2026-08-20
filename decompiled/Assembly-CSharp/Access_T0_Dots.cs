using Unity.Entities;
using Unity.Mathematics;

public struct Access_T0_Dots : IComponentData, IQueryTypeParameter
{
	public Entity ett_Access;

	public Entity ett_AccessNotNeedKey;

	public float3 torch1Offst;

	public float3 torch2Offst;

	public bool isInitialized;
}
