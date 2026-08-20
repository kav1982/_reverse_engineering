using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public struct Spell1026ShiningStarData : IComponentData, IQueryTypeParameter
{
	public bool RecordColliderType;

	public bool ResetColliderType;

	public CollisionResponsePolicy Collider1Type;

	public CollisionResponsePolicy Collider2Type;

	public UnityObjectRef<GameObject> ChargePrefab;

	public UnityObjectRef<GameObject> ChargeShinePrefab;

	public float baseCritical;

	public int CurStage;

	public bool ResizeSpellEffect;

	public bool ReadyToShoot;
}
