using Unity.Entities;

public struct Door_T8_Dots : IComponentData, IQueryTypeParameter
{
	public RoomThemeType ThemeType;

	public float openDoorTime;

	public bool isInitialized;

	public UnityObjectRef<Door_T8Mono> doorT8Mono;

	public float openDoorTimer;
}
