using Unity.Entities;

internal class Monster316AuthoringBaker : Baker<Monster316Authoring>
{
	public override void Bake(Monster316Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster316_Dots component = default(Monster316_Dots);
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = new EndlessMonsterTag
		{
			dropCount = 1
		};
		AddComponent(entity, in component2);
		Monster316RingEffect component3 = default(Monster316RingEffect);
		AddComponent(entity, in component3);
	}
}
