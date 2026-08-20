using Unity.Entities;
using Unity.Mathematics;

public class Monster327_MissileBaker : Baker<Monster327_MissileAuthoring>
{
	public override void Bake(Monster327_MissileAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster327Missle_Dots component = new Monster327Missle_Dots
		{
			rotateRoot = ((authoring.rotateRoot == null) ? Entity.Null : GetEntity(authoring.rotateRoot, TransformUsageFlags.Dynamic)),
			rotateShadow = ((authoring.rotateShadow == null) ? Entity.Null : GetEntity(authoring.rotateShadow, TransformUsageFlags.Dynamic)),
			straightTime = authoring.straightTime,
			straightSpeed = authoring.straightSpeed,
			homingSpeed = authoring.homingSpeed,
			maxTurnAnglePerSecond = authoring.maxTurnAnglePerSecond,
			lifeTime = authoring.lifeTime,
			explosionEffectScale = authoring.explosionEffectScale,
			explosionColliderRadius = authoring.explosionColliderRadius,
			explosionTouchDuration = authoring.explosionTouchDuration,
			explosionOffset = authoring.explosionOffset
		};
		AddComponent(entity, in component);
		Monster327MissileLaunch_Dots component2 = new Monster327MissileLaunch_Dots
		{
			initialDirection = new float3(0f, 1f, 0f),
			target = Entity.Null,
			shooter = Entity.Null
		};
		AddComponent(entity, in component2);
		EndlessMonsterTag component3 = new EndlessMonsterTag
		{
			dropCount = 0
		};
		AddComponent(entity, in component3);
	}
}
