using Unity.Entities;
using UnityEngine;

internal class Spell1008ArcaneExplosionAuthoring : MonoBehaviour
{
	private class Spell1008ArcaneExplosionAuthoringBaker : Baker<Spell1008ArcaneExplosionAuthoring>
	{
		public override void Bake(Spell1008ArcaneExplosionAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1008ArcaneExplosionData component = new Spell1008ArcaneExplosionData
			{
				IsFullRangeDamage = false,
				HoverDamageTimer = 0f
			};
			AddComponent(entity, in component);
			AddComponent<Spell1008InitializedTag>(entity);
			AddComponent<Spell1008ReadyToDeathTag>(entity);
			SetComponentEnabled<Spell1008ReadyToDeathTag>(entity, enabled: false);
			SetComponentEnabled<Spell1008InitializedTag>(entity, enabled: true);
			AddBuffer<Spell1008HitTargetsData>(entity);
			AddBuffer<Spell1008HitExplosionEffectData>(entity);
			AddBuffer<Spell1008TakeDamageBuffer>(entity);
		}
	}
}
