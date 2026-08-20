using Unity.Entities;

public struct Monster315_Dots : IComponentData, IQueryTypeParameter
{
	public bool Initialized;

	public bool playerNear;

	public Entity shieldEntity;

	public Entity followEntity;
}
