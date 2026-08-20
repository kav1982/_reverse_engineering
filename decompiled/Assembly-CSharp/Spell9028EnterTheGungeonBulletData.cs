using Unity.Entities;
using UnityEngine;

public struct Spell9028EnterTheGungeonBulletData : IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<GameObject> SpellObj;

	public UnityObjectRef<GameObject> TrailObj;
}
