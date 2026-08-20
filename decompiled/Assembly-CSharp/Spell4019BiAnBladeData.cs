using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Spell4019BiAnBladeData : IComponentData, IQueryTypeParameter
{
	[MarshalAs(UnmanagedType.U1)]
	public bool IsInitialized;

	public int StateCurrent;

	public int StateNext;

	public float3 FollowPosition;

	public float2 FollowRandomDiff;

	public float OnGroundRatio;

	public float OnGroundShakeCurrent;

	public float OnGroundShakeResult;

	public float RandomDuration;

	public int RebounceTimeTotal;

	public int RebounceTimeCurrent;

	public float3 WallRebounceRandomPosition;

	public float3 WallHitPos;

	public float WallRebounceLength;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsRotClockwise;

	public float ResizeForHideShader;

	public Entity Tail;

	public Entity Head;

	public Entity Material;

	public int WandIndex;

	[MarshalAs(UnmanagedType.U1)]
	public bool CanShoot;

	[MarshalAs(UnmanagedType.U1)]
	public bool CanReturn;

	public float RotateRadius;

	public float Speed;

	[MarshalAs(UnmanagedType.U1)]
	public bool ShowTrail;

	[MarshalAs(UnmanagedType.U1)]
	public bool RequiredTrail;

	public UnityObjectRef<GameObject> TrailEffect;

	public float FallGravity;

	public float OriginSpeed;

	public float FallReturnRandom;

	[MarshalAs(UnmanagedType.U1)]
	public bool FallReturnIsGenerate;
}
