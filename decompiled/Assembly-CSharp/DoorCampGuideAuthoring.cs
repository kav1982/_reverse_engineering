using Unity.Entities;
using UnityEngine;

public class DoorCampGuideAuthoring : MonoBehaviour
{
	private class Baker : Baker<DoorCampGuideAuthoring>
	{
		public override void Bake(DoorCampGuideAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			DoorCampGuide component = new DoorCampGuide
			{
				ett_Portal = GetEntity(authoring.ett_Portal, TransformUsageFlags.Dynamic),
				ett_CloseMask = GetEntity(authoring.ett_CloseMask, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Portal;

	public GameObject ett_CloseMask;
}
