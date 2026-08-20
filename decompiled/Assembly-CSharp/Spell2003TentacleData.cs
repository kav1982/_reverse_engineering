using Unity.Entities;
using Unity.Mathematics;

public struct Spell2003TentacleData : IComponentData, IQueryTypeParameter
{
	public float AttackCoolDownTimer;

	public float AttackCoolDownTime;

	public Spell2003State State;

	public float3 TargetLastFramePosition;

	public float RecheckTargetTimer;

	public int CurrentAttackingTentacleIndex;

	public float LifeDuration;

	public int ChainTentacleAccountRequirement;

	public int CurrentAttackAccount;
}
