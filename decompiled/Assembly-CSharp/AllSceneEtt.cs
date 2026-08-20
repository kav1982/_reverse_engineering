using Unity.Entities;

public struct AllSceneEtt : IComponentData, IQueryTypeParameter
{
	public Entity ett_OuterBoundary;

	public Entity ett_T8CliffCollider;
}
