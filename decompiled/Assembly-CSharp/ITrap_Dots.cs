using Unity.Entities;

public struct ITrap_Dots : IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<RoomController> belongRoom;

	public bool onInvalid;
}
