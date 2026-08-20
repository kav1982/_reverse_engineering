using Unity.Entities;
using Unity.Mathematics;

public struct Spell2001BoBoData : IComponentData, IQueryTypeParameter
{
	public float AttackIntervalTimer;

	public float AttackCoolDownTimer;

	public float AfterAttackCoolDownTimer;

	public int NormalBulletLeft;

	public Spell2001State State;

	public float AttackRange;

	public float3 TargetEntityLastFramePosition;

	public float AttackMouseOpenAnimeTimer;

	public float IdleMoveCoolDownTimer;

	public float fakeMoveTimer;

	public float BodyAnimaTimer;

	public bool BoBoBombReady;

	public float3 LastFramePosition;
}
