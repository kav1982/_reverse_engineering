using Unity.Entities;
using UnityEngine;

public class DoorCampAuthoring : MonoBehaviour
{
	private class Baker : Baker<DoorCampAuthoring>
	{
		public override void Bake(DoorCampAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			DoorCamp_Dots component = default(DoorCamp_Dots);
			AddComponent(entity, in component);
		}
	}
}
