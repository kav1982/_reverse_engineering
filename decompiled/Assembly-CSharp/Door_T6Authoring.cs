using Unity.Entities;
using UnityEngine;

public class Door_T6Authoring : MonoBehaviour
{
	private class Baker : Baker<Door_T6Authoring>
	{
		public override void Bake(Door_T6Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Door_T6_Dots component = new Door_T6_Dots
			{
				ett_Layer = GetEntity(authoring.ett_Layer, TransformUsageFlags.Dynamic),
				themeType = authoring.themeType,
				openDoorTime = authoring.openDoorTime
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Layer;

	public RoomThemeType themeType;

	public float openDoorTime;
}
