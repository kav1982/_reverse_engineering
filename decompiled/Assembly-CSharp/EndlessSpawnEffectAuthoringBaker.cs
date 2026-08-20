using Unity.Entities;

internal class EndlessSpawnEffectAuthoringBaker : Baker<EndlessSpawnEffectAuthoring>
{
	public override void Bake(EndlessSpawnEffectAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.NonUniformScale);
		Entity entity2 = GetEntity(authoring.effect, TransformUsageFlags.NonUniformScale);
		Entity entity3 = GetEntity(authoring.scaleRoot, TransformUsageFlags.NonUniformScale);
		EndlessSpawnEffect component = new EndlessSpawnEffect
		{
			effectEntity = entity2,
			scaleRoot = entity3,
			showTime = authoring.showTime,
			stayTime = authoring.stayTime,
			fadeTime = authoring.fadeTime
		};
		AddComponent(entity, in component);
	}
}
