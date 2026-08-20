using Unity.Entities;

public struct SpecialObj44 : IComponentData, IQueryTypeParameter
{
	public Entity ett_LeftWall;

	public Entity ett_LeftAccess;

	public Entity ett_LeftColliderMiddle;

	public Entity ett_RightWall;

	public Entity ett_RightAccess;

	public Entity ett_RightColliderMiddle;

	public bool isInitialized;
}
