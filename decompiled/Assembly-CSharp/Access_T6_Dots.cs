using Unity.Entities;

public struct Access_T6_Dots : IComponentData, IQueryTypeParameter
{
	public RoomThemeType themeType;

	public Entity ett_AccessTriggerT6;

	public Entity ett_PortalNormal;

	public Entity ett_PortalBoss;

	public Entity ett_Layer;

	public float openAnimaTime;

	public bool isInitialized;

	public UnityObjectRef<Access_T6Mono> accessT6Mono;

	public bool onOpenAnima;

	public float openAnimaTimer;

	public Entity createdAccessTriggerEtt;
}
