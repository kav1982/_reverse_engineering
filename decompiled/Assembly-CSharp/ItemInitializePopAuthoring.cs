using Unity.Entities;
using UnityEngine;

internal class ItemInitializePopAuthoring : MonoBehaviour
{
	private class ItemInitializePopAuthoringBaker : Baker<ItemInitializePopAuthoring>
	{
		public override void Bake(ItemInitializePopAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			ItemPopData component = new ItemPopData
			{
				MaxHeight = 1.6f
			};
			AddComponent(entity, in component);
		}
	}
}
