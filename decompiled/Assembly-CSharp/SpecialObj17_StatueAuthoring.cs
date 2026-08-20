using Unity.Entities;
using UnityEngine;

public class SpecialObj17_StatueAuthoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj17_StatueAuthoring>
	{
		public override void Bake(SpecialObj17_StatueAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj17_Statue_Dots component = new SpecialObj17_Statue_Dots
			{
				ett_Flip = GetEntity(authoring.ett_Flip, TransformUsageFlags.NonUniformScale)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Flip;
}
