using Unity.Entities;

internal class Monster319AuthoringBaker : Baker<Monster319Authoring>
{
	public override void Bake(Monster319Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster319_Dots component = new Monster319_Dots
		{
			floatRootOriginPos = authoring.tsf_floatRoot.localPosition,
			ballEntity = GetEntity(authoring.ball, TransformUsageFlags.NonUniformScale)
		};
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = default(EndlessMonsterTag);
		AddComponent(entity, in component2);
	}
}
