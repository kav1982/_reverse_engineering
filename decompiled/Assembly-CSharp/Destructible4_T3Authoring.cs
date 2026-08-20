using Unity.Entities;
using UnityEngine;

public class Destructible4_T3Authoring : MonoBehaviour
{
	private class Baker : Baker<Destructible4_T3Authoring>
	{
		public override void Bake(Destructible4_T3Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Destructible4_T3_Dots component = new Destructible4_T3_Dots
			{
				ett_Fruit = GetEntity(authoring.ett_Fruit, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
			DynamicBuffer<EntityBED1> dynamicBuffer = AddBuffer<EntityBED1>(entity);
			for (int i = 0; i < authoring.ett_MRs.Length; i++)
			{
				dynamicBuffer.Add(new EntityBED1
				{
					ett = GetEntity(authoring.ett_MRs[i], TransformUsageFlags.Dynamic)
				});
			}
		}
	}

	public GameObject[] ett_MRs;

	public GameObject ett_Fruit;
}
