using Unity.Entities;
using UnityEngine;

public class GuideRoomAuthoring : MonoBehaviour
{
	private class Baker : Baker<GuideRoomAuthoring>
	{
		public override void Bake(GuideRoomAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			GuideRoom component = default(GuideRoom);
			AddComponent(entity, in component);
		}
	}
}
