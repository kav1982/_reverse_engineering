using Unity.Entities;
using UnityEngine;

public struct Spell3127SoulMateComponent : IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<GameObject> effect;

	public Entity RingEffect;

	public float scaleZoomer;

	public bool beDestoried;
}
