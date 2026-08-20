using Unity.Entities;
using UnityEngine;

public class Relic_FollowGhostAuthoring : MonoBehaviour
{
	private class Baker : Baker<Relic_FollowGhostAuthoring>
	{
		public override void Bake(Relic_FollowGhostAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Relic_FollowGhost component = new Relic_FollowGhost
			{
				ett_Layer = GetEntity(authoring.ett_Layer, TransformUsageFlags.Dynamic),
				checkTargetInterval = authoring.checkTargetInterval,
				moveSpeed = authoring.moveSpeed,
				rotateSpeed = authoring.rotateSpeed,
				rotateAccekeration = authoring.rotateAccekeration,
				rotateMaxSpeed = authoring.rotateMaxSpeed,
				knockback = authoring.knockback
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Layer;

	public float checkTargetInterval;

	public float moveSpeed;

	public float rotateSpeed;

	public float rotateAccekeration;

	public float rotateMaxSpeed;

	public float knockback;
}
