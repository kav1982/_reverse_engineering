using Unity.Entities;
using UnityEngine;

public class EntityRandomRotateAuthoring : MonoBehaviour
{
	private class Baker : Baker<EntityRandomRotateAuthoring>
	{
		public override void Bake(EntityRandomRotateAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			EntityRandomRotate component = default(EntityRandomRotate);
			AddComponent(entity, in component);
		}
	}
}
