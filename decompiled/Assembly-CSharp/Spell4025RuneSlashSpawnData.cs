using Unity.Entities;
using Unity.Mathematics;

public struct Spell4025RuneSlashSpawnData : IBufferElementData
{
	public Entity SpellEntity;

	public float3 TargetPosition;

	public bool IsCriticalHit;
}
