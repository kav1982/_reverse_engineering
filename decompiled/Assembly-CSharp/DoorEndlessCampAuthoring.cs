using Unity.Entities;
using UnityEngine;

internal class DoorEndlessCampAuthoring : MonoBehaviour
{
	private class DoorEndlessCampAuthoringBaker : Baker<DoorEndlessCampAuthoring>
	{
		public override void Bake(DoorEndlessCampAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			DoorEndlessCamp component = default(DoorEndlessCamp);
			AddComponent(entity, in component);
		}
	}
}
