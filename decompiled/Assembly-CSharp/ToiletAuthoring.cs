using Unity.Entities;
using UnityEngine;

public class ToiletAuthoring : MonoBehaviour
{
	private class Baker : Baker<ToiletAuthoring>
	{
		public override void Bake(ToiletAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Toilet component = default(Toilet);
			AddComponent(entity, in component);
		}
	}
}
