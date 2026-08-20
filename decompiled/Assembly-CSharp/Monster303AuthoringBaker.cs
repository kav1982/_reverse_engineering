using Unity.Entities;

internal class Monster303AuthoringBaker : Baker<Monster303Authoring>
{
	public override void Bake(Monster303Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster303_Dots component = new Monster303_Dots
		{
			warningEntity = GetEntity(authoring.Warning, TransformUsageFlags.Dynamic)
		};
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = new EndlessMonsterTag
		{
			dropCount = 1
		};
		AddComponent(entity, in component2);
	}
}
