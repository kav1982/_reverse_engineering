using Unity.Entities;

public struct Relic_BlockSpell : IComponentData, IQueryTypeParameter
{
	public float damage;

	public float knockback;

	public float moveLerp;

	public bool isHit;
}
