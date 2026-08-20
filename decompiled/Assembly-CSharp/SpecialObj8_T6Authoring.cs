using Unity.Entities;
using UnityEngine;

public class SpecialObj8_T6Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj8_T6Authoring>
	{
		public override void Bake(SpecialObj8_T6Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj8_T6_Dots component = new SpecialObj8_T6_Dots
			{
				isLargeAbyss = authoring.isLargeAbyss
			};
			AddComponent(entity, in component);
		}
	}

	public bool isLargeAbyss;
}
