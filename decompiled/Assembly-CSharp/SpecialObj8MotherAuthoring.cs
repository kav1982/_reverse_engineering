using Unity.Entities;
using UnityEngine;

public class SpecialObj8MotherAuthoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj8MotherAuthoring>
	{
		public override void Bake(SpecialObj8MotherAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj8Mother component = default(SpecialObj8Mother);
			AddComponent(entity, in component);
			DynamicBuffer<SpecialObj8EachThemeBED> dynamicBuffer = AddBuffer<SpecialObj8EachThemeBED>(entity);
			GameObject[] allSO8s = authoring.allSO8s;
			foreach (GameObject authoring2 in allSO8s)
			{
				dynamicBuffer.Add(new SpecialObj8EachThemeBED
				{
					ett = GetEntity(authoring2, TransformUsageFlags.Dynamic)
				});
			}
		}
	}

	public GameObject[] allSO8s;
}
