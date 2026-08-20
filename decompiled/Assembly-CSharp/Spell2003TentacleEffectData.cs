using Unity.Entities;

public struct Spell2003TentacleEffectData : IBufferElementData
{
	public int TentacleIndex;

	public Entity EffectEntity;

	public bool IsInitialized;

	public Entity IdleEffectEntity;

	public Entity AttackEffectEntity;

	public bool StartAttack;

	public float AttackTimer;

	public bool AttackFinished;

	public float AttackingHoldTimer;
}
