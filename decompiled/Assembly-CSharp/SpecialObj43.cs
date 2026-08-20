using Unity.Entities;

public struct SpecialObj43 : IComponentData, IQueryTypeParameter
{
	public Entity ett_WallLeft;

	public Entity ett_AccessLeft;

	public Entity ett_ColliderMiddleLeft;

	public Entity ett_WallRight;

	public Entity ett_AccessRight;

	public Entity ett_ColliderMiddleRight;

	public bool waitOneFrame;
}
