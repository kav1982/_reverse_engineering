using Unity.Entities;

internal class Monster309AuthoringBaker : Baker<Monster309Authoring>
{
	public override void Bake(Monster309Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster309_Dots component = new Monster309_Dots
		{
			pattern = authoring.pattern
		};
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = new EndlessMonsterTag
		{
			dropCount = 4
		};
		AddComponent(entity, in component2);
	}
}
