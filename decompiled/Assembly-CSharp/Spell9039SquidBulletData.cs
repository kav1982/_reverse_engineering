using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Spell9039SquidBulletData : IComponentData, IQueryTypeParameter
{
	public float SinTimer;

	public float OriginSpeed;

	public float3 InitialDirection;

	public bool Initialized;

	public UnityObjectRef<GameObject> SpellObj;

	public UnityObjectRef<GameObject> ShadowObj;
}
