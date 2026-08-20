using Unity.Entities;
using Unity.Mathematics;

public struct SpecialObj48_Dots : IComponentData, IQueryTypeParameter
{
	public Entity ett_AccessTriggerLR;

	public Entity ett_LeftAccess;

	public Entity ett_LeftCollider;

	public float3 accessTriggerPosL;

	public Entity ett_RightAccess;

	public Entity ett_RightCollider;

	public float3 accessTriggerPosR;

	public bool isInitialized;
}
