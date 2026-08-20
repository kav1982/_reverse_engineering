using Unity.Entities;
using UnityEngine;

public struct Spell9043GarbageBulletData : IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<GameObject> SpellObj;

	public UnityObjectRef<GameObject> TrailShadowObj;
}
