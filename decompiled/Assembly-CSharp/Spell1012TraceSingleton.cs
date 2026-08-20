using Unity.Entities;

public struct Spell1012TraceSingleton : IComponentData, IQueryTypeParameter
{
	public float duration;

	public Entity ett_Root;

	public float timer;

	public bool recordRootEntity;
}
