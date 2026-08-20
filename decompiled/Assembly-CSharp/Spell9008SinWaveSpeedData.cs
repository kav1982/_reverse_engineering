using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Spell9008SinWaveSpeedData : IComponentData, IQueryTypeParameter
{
	public float sinTimer;

	public float originSpeed;

	public float3 InitialDirection;

	public bool Initialized;

	public UnityObjectRef<GameObject> SpellShadowObj;
}
