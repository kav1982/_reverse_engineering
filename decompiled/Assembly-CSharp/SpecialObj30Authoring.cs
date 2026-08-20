using Unity.Entities;
using UnityEngine;

public class SpecialObj30Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj30Authoring>
	{
		public override void Bake(SpecialObj30Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj30_Dots component = new SpecialObj30_Dots
			{
				ett_Center = GetEntity(authoring.ett_Center, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Center;
}
