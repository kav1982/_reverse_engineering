using Unity.Entities;
using Unity.Mathematics;

public struct SpecialObj101Compound_Dots : IComponentData, IQueryTypeParameter
{
	public Entity ett_CarpetLayer;

	public bool isInitialized;

	public float3 position;
}
