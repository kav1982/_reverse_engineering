using Unity.Entities;

internal class Monster315AuthoringBaker : Baker<Monster315Authoring>
{
	public override void Bake(Monster315Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster315_Dots component = default(Monster315_Dots);
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = default(EndlessMonsterTag);
		AddComponent(entity, in component2);
	}
}
