using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class EnterDoorDestroyAuthoring : MonoBehaviour
{
	private class Baker : Baker<EnterDoorDestroyAuthoring>
	{
		public override void Bake(EnterDoorDestroyAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			EnterDoorDestroy component = default(EnterDoorDestroy);
			AddComponent(entity, in component);
		}
	}
}
