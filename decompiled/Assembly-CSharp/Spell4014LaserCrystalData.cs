using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Spell4014LaserCrystalData : IComponentData, IQueryTypeParameter
{
	[MarshalAs(UnmanagedType.U1)]
	public bool IsInitialized;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsHit;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsCreatedCrystal;

	public float OrbitLongRadius;

	public UnityObjectRef<GameObject> LaserCrystalGO;

	public UnityObjectRef<GameObject> LaserCrystalPowerUpGO;

	public UnityObjectRef<GameObject> LaserCrystal2SecGO;

	public UnityObjectRef<GameObject> LaserCrystal5SecGO;

	public UnityObjectRef<GameObject> LaserCrystal10SecGO;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsRequiredPowerUp;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsRequired2Sec;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsRequired5Sec;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsRequired10Sec;

	public float3 OwnerDirection;

	public float KeepHittingRate;

	public float LaserWidthBase;

	public float LaserWidth;

	public float CurrentDmgLoopTime;

	public int SplitCount;

	public float CurrentWandMana;

	public float CurrentWandMaxMana;

	public float ManaCostThisFrame;

	public bool ForceCooling;

	public float MpCostRatio;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsInitializedWandCost;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsHitFrame;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsSplitCenter;

	public float3 TouchGroundPos;

	public float CrystalCenterMoveSpeed;

	public float3 OrbitCenter;

	public float3 TargetPosition;

	[MarshalAs(UnmanagedType.U1)]
	public bool HaveTarget;

	public float RotateDegreeSpeed;

	public float CurrentRotateDegree;

	public float ThunderStoneCd;

	public float ThunderStoneRate;

	public int WandCrystalCount;
}
