using Unity.Entities;

internal class Monster313AuthoringBaker : Baker<Monster313Authoring>
{
	public override void Bake(Monster313Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster313_Dots component = default(Monster313_Dots);
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = default(EndlessMonsterTag);
		AddComponent(entity, in component2);
	}
}
