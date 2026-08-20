using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class CreateNavMeshObstacleAuthoring : MonoBehaviour
{
	private class Baker : Baker<CreateNavMeshObstacleAuthoring>
	{
		public override void Bake(CreateNavMeshObstacleAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			CreateNavMeshObstacle component = new CreateNavMeshObstacle
			{
				isCamp = authoring.isCamp,
				isAbyss = authoring.isAbyss,
				isT22Rock = authoring.isT22Rock,
				extraAngle = authoring.extraAngle,
				shape = authoring.shape,
				center = authoring.center,
				size = authoring.size,
				radius = authoring.radius,
				height = authoring.height,
				Direction = authoring.Direction
			};
			AddComponent(entity, in component);
		}
	}

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
}
