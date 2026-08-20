using Unity.Entities;
using UnityEngine;

public class EntityRandomKeepAuthoring : MonoBehaviour
{
	private class Baker : Baker<EntityRandomKeepAuthoring>
	{
		public override void Bake(EntityRandomKeepAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			DynamicBuffer<EntityRandomKeepEttBED> dynamicBuffer = AddBuffer<EntityRandomKeepEttBED>(entity);
			for (int i = 0; i < authoring.etts.Length; i++)
			{
				if (authoring.etts[i] == null)
				{
					Debug.LogError(authoring.transform?.ToString() + " 有entity为null");
					continue;
				}
				dynamicBuffer.Add(new EntityRandomKeepEttBED
				{
					ett = GetEntity(authoring.etts[i], TransformUsageFlags.Dynamic)
				});
			}
			EntityRandomKeep component = default(EntityRandomKeep);
			AddComponent(entity, in component);
		}
	}

	public GameObject[] etts;
}
