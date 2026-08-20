using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Relic_FollowGhost : IComponentData, IQueryTypeParameter
{
	public Entity ett_Layer;

	public float checkTargetInterval;

	public float moveSpeed;

	public float rotateSpeed;

	public float rotateAccekeration;

	public float rotateMaxSpeed;

	public float knockback;

	public bool isInitialized;

	public UnityObjectRef<GameObject> efGO;

	public float damage;

	public int RelicId;

	public bool isHit;

	public float checkIntervalTimer;

	public Entity targetEtt;

	public float3 currentDir;
}
