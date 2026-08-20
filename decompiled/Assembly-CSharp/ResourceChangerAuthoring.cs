using Unity.Entities;
using UnityEngine;

public class ResourceChangerAuthoring : MonoBehaviour
{
	private class Baker : Baker<ResourceChangerAuthoring>
	{
		public override void Bake(ResourceChangerAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			ResourceChanger_Dots component = default(ResourceChanger_Dots);
			AddComponent(entity, in component);
		}
	}
}
