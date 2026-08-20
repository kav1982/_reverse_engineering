using Unity.Entities;
using UnityEngine;

public class AutoDestroyAuthoring : MonoBehaviour
{
	private class Baker : Baker<AutoDestroyAuthoring>
	{
		public override void Bake(AutoDestroyAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AutoDestroy component = new AutoDestroy
			{
				duration = authoring.duration
			};
			AddComponent(entity, in component);
		}
	}

	public float duration;
}
