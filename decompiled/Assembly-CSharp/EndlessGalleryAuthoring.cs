using Unity.Entities;
using UnityEngine;

internal class EndlessGalleryAuthoring : MonoBehaviour
{
	private class EndlessGalleryAuthoringBaker : Baker<EndlessGalleryAuthoring>
	{
		public override void Bake(EndlessGalleryAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			EndlessGallery component = default(EndlessGallery);
			AddComponent(entity, in component);
		}
	}
}
