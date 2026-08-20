using Unity.Entities;
using UnityEngine;

public class Guide2EttAuthoring : MonoBehaviour
{
	private class Baker : Baker<Guide2EttAuthoring>
	{
		public override void Bake(Guide2EttAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Guide2Ett component = new Guide2Ett
			{
				ett_Door_T3_Guide = GetEntity(authoring.ett_Door_T3_Guide, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Door_T3_Guide;
}
