using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Spell4024DaveHarpoonData : IComponentData, IQueryTypeParameter
{
	[MarshalAs(UnmanagedType.U1)]
	public bool IsInitialized;

	public float NodeDistance;

	public int Iterations;

	public float VelocityDampen;

	public float3 GravityStrength;

	public float GroundZ;

	public float3 StartPos;

	public float3 EndPos;

	[MarshalAs(UnmanagedType.U1)]
	public bool AllowStretch;

	public float StretchStiffness;

	public bool EndPinned;

	public float3 ShootPos;

	public ChainState ChainState;

	public HarpoonState HarpoonState;

	public float ChainLength;

	[MarshalAs(UnmanagedType.U1)]
	public bool ShowBubble;

	[MarshalAs(UnmanagedType.U1)]
	public bool RequiredBubble;

	public UnityObjectRef<GameObject> BubbleEffect;

	[MarshalAs(UnmanagedType.U1)]
	public bool ShowDust;

	[MarshalAs(UnmanagedType.U1)]
	public bool RequiredDust;

	public UnityObjectRef<GameObject> DustEffect;

	[MarshalAs(UnmanagedType.U1)]
	public bool ShowGate;

	[MarshalAs(UnmanagedType.U1)]
	public bool RequiredGate;

	public UnityObjectRef<GameObject> GateEffect;

	[MarshalAs(UnmanagedType.U1)]
	public bool ShowRelicLightning;

	[MarshalAs(UnmanagedType.U1)]
	public bool RequiredRelicLightning;

	public UnityObjectRef<GameObject> RelicLightningEffect;

	public float ReturnSpeed;

	public float3 WallHitReboudVelocity;

	public int RebounceTimeCurrent;

	public float RebounceRotateRandomResult;

	public Entity CatchEntity;

	public float3 DragVelocity;

	public float CatchDmgLoop;

	public float3 CatchPosRandom;

	public float3 ShakeVelocity;

	public float3 MouseLerpVelocity;

	public float3 FallRebounceVelocity;

	public float3 FallStartPos;

	public float3 FallGroundPos;

	public float FallRotateCurrentR;

	public Entity HarpoonMat;

	public float HarpoonHideRate;

	public float ThunderRelicTimer;
}
