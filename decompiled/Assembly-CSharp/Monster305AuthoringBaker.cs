using Unity.Entities;

internal class Monster305AuthoringBaker : Baker<Monster305Authoring>
{
	public override void Bake(Monster305Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster305_Dots component = default(Monster305_Dots);
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = new EndlessMonsterTag
		{
			dropCount = 1
		};
		AddComponent(entity, in component2);
	}
}
