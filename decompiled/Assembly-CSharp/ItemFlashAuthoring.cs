using Unity.Entities;
using UnityEngine;

internal class ItemFlashAuthoring : MonoBehaviour
{
	private class ItemFlashAuthoringBaker : Baker<ItemFlashAuthoring>
	{
		public override void Bake(ItemFlashAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			ItemFlashProgressMaterialOverride component = default(ItemFlashProgressMaterialOverride);
			AddComponent(entity, in component);
		}
	}
}
