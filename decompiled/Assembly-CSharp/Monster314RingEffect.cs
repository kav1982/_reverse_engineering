using Unity.Entities;
using UnityEngine;

public struct Monster314RingEffect : IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<GameObject> ringEffect;
}
