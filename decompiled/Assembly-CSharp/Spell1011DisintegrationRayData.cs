using System.Runtime.InteropServices;
using Unity.Entities;

public struct Spell1011DisintegrationRayData : IComponentData, IQueryTypeParameter
{
	[MarshalAs(UnmanagedType.U1)]
	public bool IsInitialize;

	public float SpellSpeed;

	public float SpellFallSpeed;

	public float CurrentDmgLoopTime;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsHideLine;

	public float HitNodeEffLoop;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsInHover;

	[MarshalAs(UnmanagedType.U1)]
	public bool LaserCastOffHand;
}
