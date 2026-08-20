using Unity.Entities;
using UnityEngine;

public struct Spell9017Chapter3BulletData : IComponentData, IQueryTypeParameter
{
	public bool initialized;

	public UnityObjectRef<GameObject> SpellObj;
}
