using Unity.Entities;

public struct AccessTrigger : IComponentData, IQueryTypeParameter
{
	public bool initialized;

	public FourDir Dir;
}
