using Unity.Entities;
using UnityEngine;

public class Destructible1_T3Authoring : MonoBehaviour
{
	private class Baker : Baker<Destructible1_T3Authoring>
	{
		public override void Bake(Destructible1_T3Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Destructible1_T3_Dots component = new Destructible1_T3_Dots
			{
				ett_Anima = GetEntity(authoring.ett_Anima, TransformUsageFlags.Dynamic)
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

	public GameObject ett_Anima;
}
