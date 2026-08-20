using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;

public struct Spell1016DashData : IComponentData, IQueryTypeParameter
{
	public Entity Dirver;

	public float HitSpeedCoolDownDuration;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsInCooldown;

	public float RemainingTime;

	public float OriginalMovementSpeed;

	public float3 OriginalPhysicsVelocitySpeed;

	public float3 LastLinear;

	public bool PauseMouseEffect;

	public bool AcceessTheme6StopTrail;
}
