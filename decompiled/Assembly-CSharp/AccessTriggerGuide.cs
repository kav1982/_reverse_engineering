using Unity.Entities;

public struct AccessTriggerGuide : IComponentData, IQueryTypeParameter
{
	public AccessTriggerGuideRoomType belongRoomtype;

	public Entity ett_TeleportPos;
}
