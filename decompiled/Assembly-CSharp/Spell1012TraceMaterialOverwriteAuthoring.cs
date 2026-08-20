using Unity.Entities;
using UnityEngine;

internal class Spell1012TraceMaterialOverwriteAuthoring : MonoBehaviour
{
	private class Spell1012TraceMaterialOverwriteAuthoringBaker : Baker<Spell1012TraceMaterialOverwriteAuthoring>
	{
		public override void Bake(Spell1012TraceMaterialOverwriteAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1012MatOverrider component = new Spell1012MatOverrider
			{
				Progress = 0f
			};
			AddComponent(entity, in component);
		}
	}
}
