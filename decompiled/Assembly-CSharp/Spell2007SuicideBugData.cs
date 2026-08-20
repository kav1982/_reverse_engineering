using Unity.Entities;
using Unity.Mathematics;

public struct Spell2007SuicideBugData : IComponentData, IQueryTypeParameter
{
	public enum WormState
	{
		Initial,
		Idle,
		ChaseToTarget,
		ReadyToExplode,
		Suicide,
		Die
	}

	public float LifeTimer;

	public float LifeTime;

	public float3 Velocity;

	public float CheckTargetTimer;

	public float StateTimer;

	public float PushPower;

	public float CurrentAroundRadius;

	public float BugInheritHp;

	public float BugSacrificeExplosionDamageRatio;

	public WormState State;

	public float IdleTime;

	public float Scale;

	public float ExplodeRadius;

	public Spell3129VoidExplosion.VoidExplosionData_Dots VoidExplosionData;
}
