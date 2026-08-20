using Unity.Entities;

internal class Monster317AuthoringBaker : Baker<Monster317Authoring>
{
	public override void Bake(Monster317Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster317_Dots component = new Monster317_Dots
		{
			relativeDistance = authoring.relativeDistance,
			spellSpeed = authoring.spellSpeed,
			shootCount = authoring.shootCount,
			shootInterval = authoring.shootInterval,
			shootDistanceInterval = authoring.shootDistanceInterval,
			isPattern2 = authoring.isPattern2
		};
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = new EndlessMonsterTag
		{
			dropCount = 1
		};
		AddComponent(entity, in component2);
		AddBuffer<Monster317_Aim>(entity);
	}
}
