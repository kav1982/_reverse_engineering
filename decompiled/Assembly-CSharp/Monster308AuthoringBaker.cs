using Unity.Entities;

internal class Monster308AuthoringBaker : Baker<Monster308Authoring>
{
	public override void Bake(Monster308Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster308_Dots component = new Monster308_Dots
		{
			warningEntity = GetEntity(authoring.Warning, TransformUsageFlags.Dynamic)
		};
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = new EndlessMonsterTag
		{
			dropCount = 4
		};
		AddComponent(entity, in component2);
		AddBuffer<Monster308_AttackedEtt>(entity);
	}
}
