using Unity.Entities;
using UnityEngine;

namespace Unity.Physics.Stateful;

public class StatefulCollisionEventBufferAuthoring : MonoBehaviour
{
	private class StatefulCollisionEventBufferBaker : Baker<StatefulCollisionEventBufferAuthoring>
	{
		public override void Bake(StatefulCollisionEventBufferAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			if (authoring.CalculateDetails)
			{
				StatefulCollisionEventDetails statefulCollisionEventDetails = default(StatefulCollisionEventDetails);
				statefulCollisionEventDetails.CalculateDetails = authoring.CalculateDetails;
				StatefulCollisionEventDetails component = statefulCollisionEventDetails;
				AddComponent(entity, in component);
			}
			AddBuffer<StatefulCollisionEvent>(entity);
		}
	}

	[Tooltip("If selected, the details will be calculated in collision event dynamic buffer of this entity")]
	public bool CalculateDetails;
}
