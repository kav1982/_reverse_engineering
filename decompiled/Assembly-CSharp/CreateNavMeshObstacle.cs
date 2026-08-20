using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public struct CreateNavMeshObstacle : IComponentData, IQueryTypeParameter
{
	public bool isCamp;

	public bool isAbyss;

	public bool isT22Rock;

	public float extraAngle;

	public NavMeshObstacleShape shape;

	public float3 center;

	[Header("Box")]
	public float3 size;

	[Header("Capsule")]
	public float radius;

	public float height;

	public NavMeshObstacleCapsuleDirection Direction;

	public bool isInitialized;

	public bool onT6RockDestroyed;

	public bool chestDisable;

	public float3 lastPosition;
}
