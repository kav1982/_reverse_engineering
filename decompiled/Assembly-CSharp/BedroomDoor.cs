using Unity.Entities;

public struct BedroomDoor : IComponentData, IQueryTypeParameter
{
	public Entity ett_DoorOpen;

	public Entity ett_DoorClose;

	public bool isInitialized;
}
