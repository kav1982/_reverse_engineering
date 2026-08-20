using Unity.Entities;
using UnityEngine;

public class Spell1008ArcaneExplosionHitAuthoring : MonoBehaviour
{
	public class Baker : Baker<Spell1008ArcaneExplosionHitAuthoring>
	{
		public override void Bake(Spell1008ArcaneExplosionHitAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddBuffer<Spell1008HitTargetsData>(entity);
			AddComponent<EnterDoorDestroy>(entity);
		}
	}
}
