using Unity.Entities;
using UnityEngine;

public class GalleryAuthoring : MonoBehaviour
{
	private class Baker : Baker<GalleryAuthoring>
	{
		public override void Bake(GalleryAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Gallery_Dots component = default(Gallery_Dots);
			AddComponent(entity, in component);
		}
	}
}
