using Unity.Entities;
using UnityEngine;

public class SpecialObj16Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj16Authoring>
	{
		public override void Bake(SpecialObj16Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj16_Dots component = new SpecialObj16_Dots
			{
				ett_Big = GetEntity(authoring.ett_Big, TransformUsageFlags.Dynamic),
				ett_Small = GetEntity(authoring.ett_Small, TransformUsageFlags.Dynamic),
				scale = authoring.scale
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Big;

	public GameObject ett_Small;

	public VariableFloat scale;
}
