using Unity.Entities;
using UnityEngine;

public struct Spell9015IceBallData : IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<GameObject> SpellShadowObj;

	public bool initialized;
}
