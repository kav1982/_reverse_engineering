using Unity.Entities;

public struct Spell3129DamageInfoBuffer : IBufferElementData
{
	public float Damage;

	public float DamageRemainTimer;

	public Entity TargetEntity;

	public Spell3129VoidExplosion.VoidExplosionData_Dots voidExplosionData;
}
