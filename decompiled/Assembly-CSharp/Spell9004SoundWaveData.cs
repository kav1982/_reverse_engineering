using Unity.Entities;
using UnityEngine;

public struct Spell9004SoundWaveData : IComponentData, IQueryTypeParameter
{
	public float width;

	public UnityObjectRef<GameObject> SpellObj;

	public UnityObjectRef<Material> BulletMaterial;

	public bool InitOver;
}
