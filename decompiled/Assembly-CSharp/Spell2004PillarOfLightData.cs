using Unity.Entities;

public struct Spell2004PillarOfLightData : IComponentData, IQueryTypeParameter
{
	public enum CrushAttackState
	{
		FindEnemy,
		ReadyToAttack,
		DelayToAttack,
		AfterAttack,
		ResetFloatSpeed
	}

	public float HeavyCrashTimer;

	public float HeavyCrashRecheckTimer;

	public float CurrentFloatingLerpSpeed;

	public float PillarFloatTimer;

	public float ApplyVenomTimer;

	public float ApplyDebuffTimer;

	public float AttackTimer;

	public float CrushAttackAnimTimer;

	public float SelfScaleTimer;

	public CrushAttackState AttackState;
}
