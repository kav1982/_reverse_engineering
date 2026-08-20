using Unity.Entities;

public struct SpecialObj103 : IComponentData, IQueryTypeParameter
{
	public SO103Type type;

	public float spaceX;

	public float spaceY;

	public int waitFrame;

	public bool alreadyHandleEnterRoom;
}
