using Unity.Entities;
using UnityEngine;

public class EntityRandomFlipAuthoring : MonoBehaviour
{
	private class Baker : Baker<EntityRandomFlipAuthoring>
	{
		public override void Bake(EntityRandomFlipAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			EntityRandomFlip component = default(EntityRandomFlip);
			AddComponent(entity, in component);
		}
	}
}
