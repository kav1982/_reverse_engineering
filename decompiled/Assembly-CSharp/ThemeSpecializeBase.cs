using Unity.Entities;

public struct ThemeSpecializeBase : IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<RoomController> roomController;
}
