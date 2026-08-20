using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Curse_RevengeGhost : IComponentData, IQueryTypeParameter
{
	public Entity ett_Layer;

	public float checkTargetInterval;

	public float moveSpeed;

	public float rotateSpeed;

	public float rotateAccekeration;

	public float rotateMaxSpeed;

	public float knockback;

	public float damage;

	public bool isInitialized;

	public float duration;

	public UnityObjectRef<GameObject> efGO;

	public bool isHit;

	public float checkIntervalTimer;

	public Entity targetEtt;

	public float3 currentDir;

	public float durationTimer;
}
