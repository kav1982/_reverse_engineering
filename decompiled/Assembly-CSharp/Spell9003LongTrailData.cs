using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Spell9003LongTrailData : IComponentData, IQueryTypeParameter
{
	public bool InitOver;

	public UnityObjectRef<GameObject> TrailObj;

	public UnityObjectRef<GameObject> TrailShadowObj;

	public float3 CreateBulletPos;
}
