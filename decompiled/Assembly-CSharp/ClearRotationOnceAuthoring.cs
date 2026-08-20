using Unity.Entities;
using UnityEngine;

internal class ClearRotationOnceAuthoring : MonoBehaviour
{
	private class ClearRotationOnceAuthoringBaker : Baker<ClearRotationOnceAuthoring>
	{
		public override void Bake(ClearRotationOnceAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			ClearRotationOnceTag component = default(ClearRotationOnceTag);
			AddComponent(entity, in component);
			SetComponentEnabled<ClearRotationOnceTag>(entity, enabled: true);
		}
	}
}
