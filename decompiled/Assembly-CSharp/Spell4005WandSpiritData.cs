using Unity.Entities;
using Unity.Mathematics;

public struct Spell4005WandSpiritData : IComponentData, IQueryTypeParameter
{
	public bool IsInitialized;

	public WandSpiritState State;

	public float3 WandLookDirection;

	public float SpiritRotateAroundPlayerLerpRatio;

	public float RotateAroundPlayerFloatingAngle;

	public float RotateAroundPlayerFloatingTimer;

	public bool ReadyToAttack;

	public float AttackDurationTimer;

	public float RecheckTargetTimer;

	public SpellSpecialMovementType CurrentShootGroupMovementType;

	public UnityObjectRef<Wand> Wand;

	public Entity ChaseTarget;

	public float3 LastFramePosition;
}
