using Unity.Entities;

public struct Spell1019BulletData : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ShootEntity;

	public float fallSpeed;
}
