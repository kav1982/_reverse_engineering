using Unity.Entities;

public struct AccessCamp : IComponentData, IQueryTypeParameter
{
	public FourDir dir;

	public bool createdScarecrow;

	public bool needWaitAFrame;
}
