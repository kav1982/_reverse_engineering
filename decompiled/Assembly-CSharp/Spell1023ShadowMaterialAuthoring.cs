using Unity.Entities;
using UnityEngine;

internal class Spell1023ShadowMaterialAuthoring : MonoBehaviour
{
	private class Spell1023ShadowMaterialAuthoringBaker : Baker<Spell1023ShadowMaterialAuthoring>
	{
		public override void Bake(Spell1023ShadowMaterialAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1023ShadowMaterialOverride component = default(Spell1023ShadowMaterialOverride);
			AddComponent(entity, in component);
		}
	}
}
