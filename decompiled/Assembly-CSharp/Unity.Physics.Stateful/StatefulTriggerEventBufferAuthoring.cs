using Unity.Entities;
using UnityEngine;

namespace Unity.Physics.Stateful;

public class StatefulTriggerEventBufferAuthoring : MonoBehaviour
{
	private class Baker : Baker<StatefulTriggerEventBufferAuthoring>
	{
		public override void Bake(StatefulTriggerEventBufferAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddBuffer<StatefulTriggerEvent>(entity);
		}
	}
}
