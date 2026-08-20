using Unity.Entities;
using UnityEngine;

public class SpecialObj2Authoring : MonoBehaviour
{
	private class SpecialObj2AuthoringBaker : Baker<SpecialObj2Authoring>
	{
		public override void Bake(SpecialObj2Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj2_Dots component = new SpecialObj2_Dots
			{
				radius = authoring.radius,
				checkInterval = authoring.checkInterval
			};
			AddComponent(entity, in component);
		}
	}

	public float radius;

	public float checkInterval;
}
