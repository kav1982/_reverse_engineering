using Unity.Entities;
using UnityEngine;

internal class Spell1008TrailMaterialAuthoring : MonoBehaviour
{
	private class Spell1008TrailMaterialAuthoringBaker : Baker<Spell1008TrailMaterialAuthoring>
	{
		public override void Bake(Spell1008TrailMaterialAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1008SpellMaterialProperty component = new Spell1008SpellMaterialProperty
			{
				Value = 1f
			};
			AddComponent(entity, in component);
		}
	}
}
