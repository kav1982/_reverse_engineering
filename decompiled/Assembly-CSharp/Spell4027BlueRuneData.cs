using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

public struct Spell4027BlueRuneData : IComponentData, IQueryTypeParameter
{
	public bool IsInitialized;

	public float ConstMaxDuration;

	public float MpRefillAmount;

	public CollisionResponsePolicy Collider1Type;

	public CollisionResponsePolicy Collider2Type;

	public float DisableColliderTimer;

	public bool IsStartCollide;

	public bool RecordColliderType;

	public float3 InitialShootDirection;

	public float3 NoTargetPosition;

	public float NormalChasePower;

	public float NormalIgnoreChaseDuration;

	public float ChaseMouseAngleRotateSpeed;

	public bool NeedResetChaseTargetAngleSpeed;

	public float MaxRotationRadius;

	public float CurrentRotationRadius;

	public bool IsSuperBlueRune;
}
