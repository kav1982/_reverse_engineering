using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;

public struct Spell2002Data : IComponentData, IQueryTypeParameter
{
	public Spell2002State State;

	public float3 IdleMoveTargetPos;

	public float IdleMoveCoolDownTimer;

	public float AttackRange;

	public float HeadAnimationTimer;

	public float3 MainHeadPos;

	public float3 MainHeadRootPos;

	public float DamageScaleRatio;

	public float ExtraDamage;

	public bool IsLegInvisible;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsPortal;

	public float3 Direction;

	public Entity EssenceLockTarget;

	public int EssenceLegGroupCount;

	public float EssenceAttackInterval;

	public float EssenceAttackTimer;

	public int CurrentEssenceLegAttackIndex;

	public float EssenceDamageRatio;
}
