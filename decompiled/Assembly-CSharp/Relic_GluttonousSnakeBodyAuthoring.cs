using Unity.Entities;
using UnityEngine;

public class Relic_GluttonousSnakeBodyAuthoring : MonoBehaviour
{
	private class Baker : Baker<Relic_GluttonousSnakeBodyAuthoring>
	{
		public override void Bake(Relic_GluttonousSnakeBodyAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Relic_GluttonousSnakeBody component = new Relic_GluttonousSnakeBody
			{
				knockback = authoring.knockback
			};
			AddComponent(entity, in component);
			SpellConfigComponentData component2 = default(SpellConfigComponentData);
			AddComponent(entity, in component2);
			SpellMovementComponentData component3 = default(SpellMovementComponentData);
			AddComponent(entity, in component3);
		}
	}

	public float knockback;
}
