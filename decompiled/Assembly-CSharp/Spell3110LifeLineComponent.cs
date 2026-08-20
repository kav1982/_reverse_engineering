using Unity.Entities;

public struct Spell3110LifeLineComponent : IComponentData, IQueryTypeParameter
{
	public Entity line;

	public Entity shadow;

	public Entity fire;

	public Entity linkTarget1;

	public Entity linkTarget2;

	public Entity tie1;

	public Entity tie2;

	public TakeDamageInfo_Dots damageInfo;

	public float damageIntervalTimer;

	public float distancePocess;
}
