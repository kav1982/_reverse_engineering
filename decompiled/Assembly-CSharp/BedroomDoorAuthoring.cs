using Unity.Entities;
using UnityEngine;

public class BedroomDoorAuthoring : MonoBehaviour
{
	private class Baker : Baker<BedroomDoorAuthoring>
	{
		public override void Bake(BedroomDoorAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			BedroomDoor component = new BedroomDoor
			{
				ett_DoorOpen = GetEntity(authoring.ett_DoorOpen, TransformUsageFlags.Dynamic),
				ett_DoorClose = GetEntity(authoring.ett_DoorClose, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_DoorOpen;

	public GameObject ett_DoorClose;
}
