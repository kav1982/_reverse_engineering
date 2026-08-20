using Unity.Entities;

public struct Relic_GluttonousSnakeBody : IComponentData, IQueryTypeParameter
{
	public float knockback;

	public bool isInitialized;

	public float damage;

	public bool isHit;

	public int RelicID;
}
