using Unity.Entities;
using Unity.Mathematics;

public struct Spell1028MrBingArrowHitEffectData : IComponentData, IQueryTypeParameter
{
	public float3 velocity;

	public float rotateSpeed;

	public bool initialized;
}
