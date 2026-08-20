using Unity.Entities;
using UnityEngine;

public class FollowEntityAuthoring : MonoBehaviour
{
	private class Baker : Baker<FollowEntityAuthoring>
	{
		public override void Bake(FollowEntityAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			FollowEntity component = default(FollowEntity);
			AddComponent(entity, in component);
		}
	}
}
