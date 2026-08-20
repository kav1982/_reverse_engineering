using System.Runtime.InteropServices;
using Unity.Entities;
using UnityEngine;

public struct Spell1019HighPressureData : IComponentData, IQueryTypeParameter
{
	[MarshalAs(UnmanagedType.U1)]
	public bool StopFollowShooter;

	public float StartSpeed;

	public Entity lastDestroyEntity;

	public UnityObjectRef<GameObject> StartObj;
}
