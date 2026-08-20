using Unity.Entities;

public struct EndlessMonsterTag : IComponentData, IQueryTypeParameter
{
	public int dropCount;

	public bool has316Buff;

	public Entity buffEntity;
}
