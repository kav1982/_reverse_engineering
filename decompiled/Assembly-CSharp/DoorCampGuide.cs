using Unity.Entities;

public struct DoorCampGuide : IComponentData, IQueryTypeParameter
{
	public Entity ett_Portal;

	public Entity ett_CloseMask;

	public bool onHideMask;
}
