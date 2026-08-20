using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SpecialObj17Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj17Authoring>
	{
		public override void Bake(SpecialObj17Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj17_Dots component = new SpecialObj17_Dots
			{
				daveOffset = authoring.daveOffset
			};
			AddComponent(entity, in component);
		}
	}

	public float3 daveOffset;
}
