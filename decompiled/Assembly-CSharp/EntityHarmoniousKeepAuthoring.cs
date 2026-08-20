using Unity.Entities;
using UnityEngine;

public class EntityHarmoniousKeepAuthoring : MonoBehaviour
{
	private class Baker : Baker<EntityHarmoniousKeepAuthoring>
	{
		public override void Bake(EntityHarmoniousKeepAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			EntityHarmoniousKeep component = new EntityHarmoniousKeep
			{
				ett_Normal = GetEntity(authoring.ett_Normal, TransformUsageFlags.Dynamic),
				ett_H = GetEntity(authoring.ett_H, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Normal;

	public GameObject ett_H;
}
