using Unity.Entities;
using UnityEngine;

public class Curse_RevengeGhostAuthoring : MonoBehaviour
{
	private class Baker : Baker<Curse_RevengeGhostAuthoring>
	{
		public override void Bake(Curse_RevengeGhostAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Curse_RevengeGhost component = new Curse_RevengeGhost
			{
				ett_Layer = GetEntity(authoring.ett_Layer, TransformUsageFlags.Dynamic),
				checkTargetInterval = authoring.checkTargetInterval,
				moveSpeed = authoring.moveSpeed,
				rotateSpeed = authoring.rotateSpeed,
				rotateAccekeration = authoring.rotateAccekeration,
				rotateMaxSpeed = authoring.rotateMaxSpeed,
				knockback = authoring.knockback,
				damage = authoring.damage
			};
			AddComponent(entity, in component);
			IgnoreSpellHitTag component2 = default(IgnoreSpellHitTag);
			AddComponent(entity, in component2);
		}
	}

	public GameObject ett_Layer;

	public float checkTargetInterval;

	public float moveSpeed;

	public float rotateSpeed;

	public float rotateAccekeration;

	public float rotateMaxSpeed;

	public float knockback;

	public float damage;
}
