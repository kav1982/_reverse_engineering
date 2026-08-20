using Unity.Entities;

internal class Monster325AuthoringBaker : Baker<Monster325Authoring>
{
	public override void Bake(Monster325Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster325_Dots component = new Monster325_Dots
		{
			idleTime = authoring.idleTime,
			randomWalkDistance = authoring.randomWalkDistance,
			randomWalkTime = authoring.randomWalkTime,
			moveSpeedRatio = authoring.speedRatio,
			randomWalkToTargetAngle = authoring.randomWalkToTargetAngle,
			closeRandomWalkTriggerDistance = authoring.closeRandomWalkTriggerDistance,
			closeRandomWalkDistance = authoring.closeRandomWalkDistance,
			closeRandomWalkToTargetAngle = authoring.closeRandomWalkToTargetAngle,
			state = Monster325State.BornIdle
		};
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = default(EndlessMonsterTag);
		AddComponent(entity, in component2);
	}
}
