using Unity.Entities;
using UnityEngine;

public class ItemDropAuthoring : MonoBehaviour
{
	private class Baker : Baker<ItemDropAuthoring>
	{
		public override void Bake(ItemDropAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			ItemDrop component = default(ItemDrop);
			AddComponent(entity, in component);
		}
	}
}
