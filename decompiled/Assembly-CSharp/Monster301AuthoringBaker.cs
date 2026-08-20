using Unity.Entities;

internal class Monster301AuthoringBaker : Baker<Monster301Authoring>
{
	public override void Bake(Monster301Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster301_Dots component = new Monster301_Dots
		{
			isMonster307 = authoring.isMonster307
		};
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = new EndlessMonsterTag
		{
			dropCount = 1
		};
		AddComponent(entity, in component2);
	}
}
