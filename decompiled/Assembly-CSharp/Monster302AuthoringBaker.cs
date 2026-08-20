using Unity.Entities;

internal class Monster302AuthoringBaker : Baker<Monster302Authoring>
{
	public override void Bake(Monster302Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster302_Dots component = new Monster302_Dots
		{
			aimTime = authoring.aimTime,
			MoveRange = new RandomFloat
			{
				value1 = authoring.MoveRange.value1,
				value2 = authoring.MoveRange.value2
			},
			AimRange = new RandomFloat
			{
				value1 = authoring.AimRange.value1,
				value2 = authoring.AimRange.value2
			}
		};
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = new EndlessMonsterTag
		{
			dropCount = 1
		};
		AddComponent(entity, in component2);
	}
}
