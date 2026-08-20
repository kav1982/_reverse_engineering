using Unity.Entities;
using UnityEngine;

public class Curse_RandomBombAuthoring : MonoBehaviour
{
	private class Baker : Baker<Curse_RandomBombAuthoring>
	{
		public override void Bake(Curse_RandomBombAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Curse_RandomBomb_Dots component = new Curse_RandomBomb_Dots
			{
				explosionDelay = authoring.explosionDelay,
				explosionRadius = authoring.explosionRadius,
				explosionKnockback = authoring.explosionKnockback,
				explosionDamage = authoring.explosionDamage
			};
			AddComponent(entity, in component);
		}
	}

	public float explosionDelay;

	public float explosionRadius;

	public float explosionKnockback;

	public float explosionDamage;
}
