using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;

public struct LegsData : IBufferElementData
{
	public LegState LegState;

	public float3 CurrentEndPoint;

	public float3 MoveToEndPoint;

	public float3 MoveBeforeEndPoint;

	public RandomFloat LegRadiusRatio;

	public float3 Dir;

	public float AttackTimer;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsCantAttackLeg;

	public Entity Target;

	public int FuseHeadIndex;

	public bool IsFuseLeg => FuseHeadIndex >= 0;
}
