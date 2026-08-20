using Unity.Entities;
using UnityEngine;

public class ItemAuthoring : MonoBehaviour
{
	private class Baker : Baker<ItemAuthoring>
	{
		public override void Bake(ItemAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Item component = new Item
			{
				storeBaseOffsetY = authoring.storeBaseOffsetY,
				priceFactor = 1f
			};
			AddComponent(entity, in component);
			AddComponentObject(entity, new WandConfigComponent());
		}
	}

	public float storeBaseOffsetY;
}
