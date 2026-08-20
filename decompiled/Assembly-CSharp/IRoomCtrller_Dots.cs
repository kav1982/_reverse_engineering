using Unity.Entities;
using Unity.Mathematics;

public struct IRoomCtrller_Dots : IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<RoomController> belongRoom;

	public bool onRoomFinish;

	public float3 roomFinishPos;

	public bool onRoomEnter;
}
