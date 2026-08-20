using Unity.Entities;
using Unity.Mathematics;

public struct Spell4004StartData : IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<Spell4004ChargeStars> Star;

	public bool Released;

	public float3 WandShootDirection;

	public bool NeedBreak;
}
