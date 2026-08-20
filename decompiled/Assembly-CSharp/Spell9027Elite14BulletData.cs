using Unity.Entities;
using UnityEngine;

public struct Spell9027Elite14BulletData : IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<GameObject> SpellObj;

	public UnityObjectRef<GameObject> TrailObj;
}
