using Unity.Entities;
using UnityEngine;

internal class ItemDelayActiveTriggerAuthoring : MonoBehaviour
{
	private class ItemDelayActiveTriggerAuthoringBaker : Baker<ItemDelayActiveTriggerAuthoring>
	{
		public override void Bake(ItemDelayActiveTriggerAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			ItemDelayActiveTriggerData component = new ItemDelayActiveTriggerData
			{
				DelayTimer = authoring.DelayDuration
			};
			AddComponent(entity, in component);
		}
	}

	public float DelayDuration;
}
