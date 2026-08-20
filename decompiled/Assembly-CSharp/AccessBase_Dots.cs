using Unity.Entities;

public struct AccessBase_Dots : IComponentData, IQueryTypeParameter
{
	public Entity ett_AccessTriggerLR;

	public Entity ett_AccessTriggerUD;

	public Entity ett_Anima;

	public Entity triggerEntity;

	public FourDir Dir;

	public RoomType roomType;

	public RoomThemeType themeType;

	public bool needKey;

	public bool alreadyUseKey;

	public bool onOpen;

	public bool onOpenDirect;

	public bool onClose;

	public bool onCloseDirect;
}
