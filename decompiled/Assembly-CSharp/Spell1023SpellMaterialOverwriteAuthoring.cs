using Unity.Entities;
using UnityEngine;

internal class Spell1023SpellMaterialOverwriteAuthoring : MonoBehaviour
{
	private class Spell1023SpellMaterialOverwriteAuthoringBaker : Baker<Spell1023SpellMaterialOverwriteAuthoring>
	{
		public override void Bake(Spell1023SpellMaterialOverwriteAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1023SpellMaterialProperty component = default(Spell1023SpellMaterialProperty);
			AddComponent(entity, in component);
			Spell1023SpellMaterialOnGround component2 = default(Spell1023SpellMaterialOnGround);
			AddComponent(entity, in component2);
			Spell1023SpellMaterialOnGroundHideHeight component3 = default(Spell1023SpellMaterialOnGroundHideHeight);
			AddComponent(entity, in component3);
		}
	}
}
