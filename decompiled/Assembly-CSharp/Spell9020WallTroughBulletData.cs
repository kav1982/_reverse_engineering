using Unity.Entities;
using UnityEngine;

public struct Spell9020WallTroughBulletData : IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<GameObject> SpellObj;
}
