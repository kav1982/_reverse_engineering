using Unity.Entities;
using UnityEngine;

public class AllUnitEttAuthoring : MonoBehaviour
{
	private class Baker : Baker<AllUnitEttAuthoring>
	{
		public override void Bake(AllUnitEttAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AllUnitEttTag component = default(AllUnitEttTag);
			AddComponent(entity, in component);
			DynamicBuffer<UnitEttBED> dynamicBuffer = AddBuffer<UnitEttBED>(entity);
			GameObject[] prefabs = authoring.prefabs;
			foreach (GameObject gameObject in prefabs)
			{
				dynamicBuffer.Add(new UnitEttBED
				{
					id = int.Parse(gameObject.name),
					ett = GetEntity(gameObject, TransformUsageFlags.Dynamic)
				});
			}
		}
	}

	public GameObject[] prefabs;
}
