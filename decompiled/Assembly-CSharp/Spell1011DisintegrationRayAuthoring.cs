using Unity.Entities;
using UnityEngine;

public class Spell1011DisintegrationRayAuthoring : MonoBehaviour
{
	private class Spell1011DisintegrationRayAuthoringBaker : Baker<Spell1011DisintegrationRayAuthoring>
	{
		public override void Bake(Spell1011DisintegrationRayAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1011DisintegrationRayData component = default(Spell1011DisintegrationRayData);
			AddComponent(entity, in component);
			AddBuffer<DisintegrationRayBodyPoint>(entity);
			AddBuffer<DisintegrationRayMousePoint>(entity);
			AddBuffer<DisintegrationRayHoverFallGroundPoint>(entity);
		}
	}
}
