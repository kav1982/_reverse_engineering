using Unity.Entities;

internal class Monster306AuthoringBaker : Baker<Monster306Authoring>
{
	public override void Bake(Monster306Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Entity entity2 = GetEntity(authoring.turret, TransformUsageFlags.Dynamic);
		Entity entity3 = GetEntity(authoring.turretBack, TransformUsageFlags.Dynamic);
		Monster306_Dots component = new Monster306_Dots
		{
			turretRotateAngle = authoring.turretRotateAngle,
			turretEntity = entity2,
			turretBackEntity = entity3,
			turretRotateSpeed = authoring.turretRotateSpeed,
			turretRotateInterval = authoring.turretRotateInterval,
			shootInterval = authoring.shootInterval,
			shootCount = authoring.shootCount,
			attackRange = authoring.attackRange,
			attackCD = authoring.attackCD
		};
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = new EndlessMonsterTag
		{
			dropCount = 4
		};
		AddComponent(entity, in component2);
	}
}
