using Unity.Entities;
using UnityEngine;

public class Door_T0Authoring : MonoBehaviour
{
	private class Baker : Baker<Door_T0Authoring>
	{
		public override void Bake(Door_T0Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Door_T0_Dots component = default(Door_T0_Dots);
			AddComponent(entity, in component);
		}
	}
}
