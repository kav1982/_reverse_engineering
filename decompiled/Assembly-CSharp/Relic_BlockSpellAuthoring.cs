using Unity.Entities;
using UnityEngine;

public class Relic_BlockSpellAuthoring : MonoBehaviour
{
	private class Baker : Baker<Relic_BlockSpellAuthoring>
	{
		public override void Bake(Relic_BlockSpellAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Relic_BlockSpell component = new Relic_BlockSpell
			{
				damage = authoring.damage,
				knockback = authoring.knockback,
				moveLerp = authoring.moveLerp
			};
			AddComponent(entity, in component);
			AddComponent<IgnorePlayerSpellHitTag>(entity);
		}
	}

	public float damage;

	public float knockback;

	public float moveLerp;
}
