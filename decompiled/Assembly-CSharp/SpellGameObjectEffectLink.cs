using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[InternalBufferCapacity(6)]
public struct SpellGameObjectEffectLink : IBufferElementData
{
	public UnityObjectRef<GameObject> GameObject;

	public FixedString32Bytes EffectName;
}
