using Unity.Entities;

internal class Monster314AuthoringBaker : Baker<Monster314Authoring>
{
	public override void Bake(Monster314Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster314_Dots component = new Monster314_Dots
		{
			cureHpPercent = authoring.cureHpPercent,
			cureHpPercentTotal = authoring.cureHpPercentTotal,
			cureRadius = authoring.cureRadius
		};
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = new EndlessMonsterTag
		{
			dropCount = 1
		};
		AddComponent(entity, in component2);
		Monster314RingEffect component3 = default(Monster314RingEffect);
		AddComponent(entity, in component3);
	}
}
