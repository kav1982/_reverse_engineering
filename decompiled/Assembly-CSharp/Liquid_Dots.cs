using Unity.Entities;

public struct Liquid_Dots : IComponentData, IQueryTypeParameter
{
	public Entity imageEntity;

	public Entity colliderEntity;
}
