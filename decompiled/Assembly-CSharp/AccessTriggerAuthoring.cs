using Unity.Entities;
using UnityEngine;

public class AccessTriggerAuthoring : MonoBehaviour
{
	private class Baker : Baker<AccessTriggerAuthoring>
	{
		public override void Bake(AccessTriggerAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AccessTrigger component = default(AccessTrigger);
			AddComponent(entity, in component);
		}
	}
}
