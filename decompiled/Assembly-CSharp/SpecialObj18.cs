using Unity.Entities;

public struct SpecialObj18 : IComponentData, IQueryTypeParameter
{
	public Entity ett_Normal;

	public Entity ett_Used;

	public bool isInitialized;

	public int useTime;

	public int useTimer;
}
