using Unity.Entities;
using Unity.Mathematics;

public struct Spell3129DamageRequestBuffer : IBufferElementData
{
	public float MaxHp;

	public Spell3129VoidExplosion.VoidExplosionData_Dots voidExplosionData;

	public float Timer;

	public float3 SpawnPosition;
}
