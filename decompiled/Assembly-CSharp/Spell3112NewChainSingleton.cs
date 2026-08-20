using Unity.Entities;
using Unity.Mathematics;

public struct Spell3112NewChainSingleton : IBufferElementData
{
	public float3 StartPos;

	public float3 EndPos;

	public SpellColorType ColorType;

	public float3 LineScale;

	public Entity TargetEntity;

	public TakeDamageInfo_Dots Info;
}
