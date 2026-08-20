using Unity.Entities;
using UnityEngine;

public class AccessTriggerGuideAuthoring : MonoBehaviour
{
	private class Baker : Baker<AccessTriggerGuideAuthoring>
	{
		public override void Bake(AccessTriggerGuideAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AccessTriggerGuide component = new AccessTriggerGuide
			{
				belongRoomtype = authoring.belongRoomtype,
				ett_TeleportPos = GetEntity(authoring.ett_TeleportPos, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public AccessTriggerGuideRoomType belongRoomtype;

	public GameObject ett_TeleportPos;
}
