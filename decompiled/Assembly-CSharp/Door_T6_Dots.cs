using Unity.Entities;

public struct Door_T6_Dots : IComponentData, IQueryTypeParameter
{
	public Entity ett_Layer;

	public RoomThemeType themeType;

	public float openDoorTime;

	public bool isInitialized;

	public UnityObjectRef<Door_SpineMono> doorSpineMono;

	public float openDoorTimer;
}
