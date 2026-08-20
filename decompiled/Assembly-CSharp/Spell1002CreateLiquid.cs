using Unity.Entities;
using Unity.Mathematics;

public struct Spell1002CreateLiquid : IComponentData, IQueryTypeParameter
{
	public float3 lastCreatePos;
}
