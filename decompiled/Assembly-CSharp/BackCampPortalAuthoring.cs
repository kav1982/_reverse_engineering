using Unity.Entities;
using UnityEngine;

public class BackCampPortalAuthoring : MonoBehaviour
{
	private class Baker : Baker<BackCampPortalAuthoring>
	{
		public override void Bake(BackCampPortalAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			BackCampPortal component = default(BackCampPortal);
			AddComponent(entity, in component);
		}
	}
}
