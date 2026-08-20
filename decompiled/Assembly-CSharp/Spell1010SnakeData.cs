using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Spell1010SnakeData : IComponentData, IQueryTypeParameter
{
	[MarshalAs(UnmanagedType.U1)]
	public bool IsInitialize;

	public float TotalTime;

	public float3 LastPos;

	public float3 TargetDirection;

	public float LineLength;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsFadingTail;

	public float OnGroundDisapperDistance;

	public float OnGroundSpeed;

	public float OnGroundDmgLoop;

	public UnityObjectRef<LineRenderer> BodyLine;

	public UnityObjectRef<LineRenderer> ShadowLine;
}
