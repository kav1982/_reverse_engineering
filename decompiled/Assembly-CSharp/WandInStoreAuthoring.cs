using Unity.Entities;
using UnityEngine;

public class WandInStoreAuthoring : MonoBehaviour
{
	private class Baker : Baker<WandInStoreAuthoring>
	{
		public override void Bake(WandInStoreAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			WandInStore component = default(WandInStore);
			AddComponent(entity, in component);
		}
	}
}
