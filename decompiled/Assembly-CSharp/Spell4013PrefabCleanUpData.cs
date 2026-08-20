using Unity.Entities;
using UnityEngine;

public struct Spell4013PrefabCleanUpData : ICleanupComponentData, IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<GameObject> effectObject;

	public UnityObjectRef<GameObject> shadowObject;

	public bool Initialized;
}
