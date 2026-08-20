using Unity.Entities;
using Unity.Mathematics;

public struct Spell2006Data : IComponentData, IQueryTypeParameter
{
	public bool IsInitialized;

	public Teammate6State CurrentState;

	public float IdleTimer;

	public float IdleInterval;

	public float IdleWalkTimer;

	public float IdleWalkDuration;

	public float IdleWalkCoolDownTimer;

	public bool IsIdleWalkCoolDown;

	public float RecheckTargetTimer;

	public float3 TargetIdleWalkPoint;

	public bool IsFaceRight;

	public bool IsCloseAttacking;

	public float CloseAttackRange;

	public float CloseAttackTimer;

	public bool SpawnCloseAttackShockWave;

	public float MeleeAttackDamage;

	public float RecheckTargetTeammateTimer;

	public Entity TargetTeammate;

	public bool IsPickingTeammateP1;

	public bool IsPickingTeammateP2;

	public float PickingTeammateTimer;

	public float HookDetectRange;

	public bool IsStartThrowingHook;

	public bool IsHookOut;

	public bool IsHookCatchTarget;

	public float ThrowHookTimer;

	public int MaxHookCount;

	public bool IsStartShoot;

	public float ShootingTimer;

	public bool IsBombShoot;

	public bool IsQuickReloading;

	public float QuickReloadTimer;

	public float SoulBombRange;

	public bool ActiveGhostEffect;

	public bool ActiveFuseEffect;

	public int KillCounter;

	public int CurrentKillCounter;

	public int UID;

	public float SBDecreaseRadiusToDamageRatio;
}
