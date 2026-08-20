using Unity.Entities;
using UnityEngine;

public class SpecialObj101CompoundAuthoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj101CompoundAuthoring>
	{
		public override void Bake(SpecialObj101CompoundAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj101Compound_Dots component = new SpecialObj101Compound_Dots
			{
				ett_CarpetLayer = GetEntity(authoring.ett_CarpetLayer, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_CarpetLayer;
}
