using Unity.Entities;
using Unity.Mathematics;

public class Monster327AuthoringBaker : Baker<Monster327Authoring>
{
	public override void Bake(Monster327Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		float3 @float = math.normalizesafe(authoring.defaultTurretDirection, new float3(0f, 1f, 0f));
		Monster327_Dots component = new Monster327_Dots
		{
			missilePrefab = ((authoring.missilePrefab == null) ? Entity.Null : GetEntity(authoring.missilePrefab, TransformUsageFlags.Dynamic)),
			turretRoot = ((authoring.turretRoot == null) ? Entity.Null : GetEntity(authoring.turretRoot, TransformUsageFlags.Dynamic)),
			leftMuzzle = ((authoring.leftMuzzle == null) ? Entity.Null : GetEntity(authoring.leftMuzzle, TransformUsageFlags.Dynamic)),
			rightMuzzle = ((authoring.rightMuzzle == null) ? Entity.Null : GetEntity(authoring.rightMuzzle, TransformUsageFlags.Dynamic)),
			turretRotateSpeed = math.max(0f, authoring.turretRotateSpeed),
			maxFireAngleError = math.clamp(authoring.maxFireAngleError, 0f, 180f),
			turretDirection = @float,
			lockedFireDirection = @float,
			missileSpawnYOffset = authoring.missileSpawnYOffset,
			firstFireDelay = authoring.firstFireDelay,
			fireInterval = authoring.fireInterval,
			missilesPerVolley = authoring.missilesPerVolley,
			missileFireInterval = authoring.missileFireInterval,
			afterVolleyLockTime = math.max(0f, authoring.afterVolleyLockTime),
			fireTimer = math.max(0.01f, authoring.fireInterval) - math.max(0f, authoring.firstFireDelay)
		};
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = default(EndlessMonsterTag);
		AddComponent(entity, in component2);
	}
}
