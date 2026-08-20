using Unity.Entities;
using UnityEngine.Scripting;

public class BoundaryT2RoomCtrller : IComponentData, IQueryTypeParameter
{
	public RoomController roomCtrller;

	public Vector2Data accessPositionL;

	public Vector2Data accessPositionR;

	[Preserve]
	public BoundaryT2RoomCtrller()
	{
	}
}
