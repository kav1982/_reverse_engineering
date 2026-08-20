using Unity.Entities;
using Unity.Mathematics;

public struct Spell1023JudgementBladeData : IComponentData, IQueryTypeParameter
{
	public bool IsInitialized;

	public JudgementBladeState State;

	public Entity Target;

	public float3 TargetLastFramePosition;

	public float3 OwnerLastFramePosition;

	public float3 LockTargetLookingDirection;

	public bool IsBladeInQuery;

	public float LockingTargetTimer;

	public float FadeInTimer;

	public float FadeOutTimer;

	public bool LockRotateInClockWise;

	public float BladeLockRotateLerpSpeed;

	public float BladeRecheckTargetTimer;

	public float EnemyDetectRange;

	public bool IsFirstOnGroundFrame;
}
