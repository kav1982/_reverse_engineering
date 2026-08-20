using Unity.Entities;
using UnityEngine;

public class AllSpecialObjEttAuthoring : MonoBehaviour
{
	private class Baker : Baker<AllSpecialObjEttAuthoring>
	{
		public override void Bake(AllSpecialObjEttAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AllSpecialObjEttTag component = default(AllSpecialObjEttTag);
			AddComponent(entity, in component);
			DynamicBuffer<SpecialObjEttBED> dynamicBuffer = AddBuffer<SpecialObjEttBED>(entity);
			for (int i = 0; i < authoring.prefabs.Length; i++)
			{
				dynamicBuffer.Add(new SpecialObjEttBED
				{
					id = int.Parse(authoring.prefabs[i].name),
					ett = GetEntity(authoring.prefabs[i], TransformUsageFlags.Dynamic)
				});
			}
		}
	}

	public GameObject[] prefabs;
}
