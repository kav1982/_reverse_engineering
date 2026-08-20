using Unity.Entities;

internal class Monster320AuthoringBaker : Baker<Monster320Authoring>
{
	public override void Bake(Monster320Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster320_Dots component = new Monster320_Dots
		{
			floatRootOriginPos = authoring.tsf_floatRoot.localPosition
		};
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = default(EndlessMonsterTag);
		AddComponent(entity, in component2);
	}
}
