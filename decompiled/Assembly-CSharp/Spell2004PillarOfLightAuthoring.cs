using Unity.Entities;
using UnityEngine;

public class Spell2004PillarOfLightAuthoring : MonoBehaviour
{
	private class Spell2004PillarOfLightBaker : Baker<Spell2004PillarOfLightAuthoring>
	{
		public override void Bake(Spell2004PillarOfLightAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell2004PillarOfLightData component = default(Spell2004PillarOfLightData);
			AddComponent(entity, in component);
			Spell2004PillarInitializeTag component2 = default(Spell2004PillarInitializeTag);
			AddComponent(entity, in component2);
			AddBuffer<Spell2004PillarBuffer>(entity);
			AddBuffer<Spell2004WallBuffer>(entity);
			SetComponentEnabled<Spell2004PillarInitializeTag>(entity, enabled: true);
		}
	}
}
