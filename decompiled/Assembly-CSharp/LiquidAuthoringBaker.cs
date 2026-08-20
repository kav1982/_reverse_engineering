using Unity.Entities;

internal class LiquidAuthoringBaker : Baker<LiquidAuthoring>
{
	public override void Bake(LiquidAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.NonUniformScale);
		Entity entity2 = GetEntity(authoring.image, TransformUsageFlags.NonUniformScale);
		if (authoring.noCollider)
		{
			Liquid_Dots component = new Liquid_Dots
			{
				imageEntity = entity2
			};
			AddComponent(entity, in component);
		}
		else
		{
			Entity entity3 = GetEntity(authoring.thisCollider, TransformUsageFlags.NonUniformScale);
			Liquid_Dots component = new Liquid_Dots
			{
				imageEntity = entity2,
				colliderEntity = entity3
			};
			AddComponent(entity, in component);
		}
	}
}
