using Unity.Entities;
using UnityEngine;

internal class Spell1012SpellMaterialAuthoring : MonoBehaviour
{
	private class Spell1012SpellMaterialAuthoringBaker : Baker<Spell1012SpellMaterialAuthoring>
	{
		public override void Bake(Spell1012SpellMaterialAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1012SpellMaterialProperty component = default(Spell1012SpellMaterialProperty);
			AddComponent(entity, in component);
		}
	}
}
