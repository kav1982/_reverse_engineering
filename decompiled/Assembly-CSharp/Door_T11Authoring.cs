using Unity.Entities;
using UnityEngine;

public class Door_T11Authoring : MonoBehaviour
{
	private class Baker : Baker<Door_T11Authoring>
	{
		public override void Bake(Door_T11Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Door_T11_Dots component = new Door_T11_Dots
			{
				openDoorTime = authoring.openDoorTime
			};
			AddComponent(entity, in component);
		}
	}

	public float openDoorTime;
}
