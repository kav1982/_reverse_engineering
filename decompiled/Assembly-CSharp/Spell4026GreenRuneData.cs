using Unity.Entities;

public struct Spell4026GreenRuneData : IComponentData, IQueryTypeParameter
{
	public bool IsInitialized;

	public float CurrentAngle;

	public float TargetAngle;

	public float CurrentGreenRuneBaseDamage;

	public bool ReadyToExplosion;

	public bool IsRuneBall;

	public float RuneExplosionDelayDestroyTimer;

	public bool ExplosionFinish;

	public bool DirectExplosion;

	public int BonusSpawnCount;
}
