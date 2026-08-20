using Unity.Entities;
using UnityEngine;

public class ShadowAuthoring : MonoBehaviour
{
	private class Baker : Baker<ShadowAuthoring>
	{
		public override void Bake(ShadowAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Shadow_Dots component = new Shadow_Dots
			{
				ett_Shadow = GetEntity(authoring.ett_Shadow, TransformUsageFlags.Dynamic),
				shadowScale = authoring.shadowScale,
				updateEveryFrame = authoring.updateEveryFrame
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Shadow;

	public float shadowScale = 1f;

	public bool updateEveryFrame;
}
