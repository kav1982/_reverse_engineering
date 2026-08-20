using Unity.Entities;
using UnityEngine;

public class AllMixedEttAuthoring : MonoBehaviour
{
	private class Baker : Baker<AllMixedEttAuthoring>
	{
		public override void Bake(AllMixedEttAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AllMixedEttTag component = default(AllMixedEttTag);
			AddComponent(entity, in component);
			DynamicBuffer<MixedEttBED> dynamicBuffer = AddBuffer<MixedEttBED>(entity);
			for (int i = 0; i < authoring.prefabs.Length; i++)
			{
				dynamicBuffer.Add(new MixedEttBED
				{
					name = authoring.prefabs[i].name,
					ett = GetEntity(authoring.prefabs[i], TransformUsageFlags.Dynamic)
				});
			}
		}
	}

	public GameObject[] prefabs;
}
