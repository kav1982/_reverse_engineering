using Unity.Entities;

public struct Spell3007LightningChainEffect : IComponentData, IQueryTypeParameter
{
	public Entity SourceEntity;

	public Entity TargetEntity;

	public float Damage;

	public int PenetrateCount;

	public float DamageTimer;
}
