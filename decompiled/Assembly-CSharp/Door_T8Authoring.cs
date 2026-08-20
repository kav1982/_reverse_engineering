using Unity.Entities;
using UnityEngine;

public class Door_T8Authoring : MonoBehaviour
{
	private class Baker : Baker<Door_T8Authoring>
	{
		public override void Bake(Door_T8Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Door_T8_Dots component = new Door_T8_Dots
			{
				ThemeType = authoring.ThemeType,
				openDoorTime = authoring.openDoorTime
			};
			AddComponent(entity, in component);
		}
	}

	public RoomThemeType ThemeType;

	public float openDoorTime;
}
