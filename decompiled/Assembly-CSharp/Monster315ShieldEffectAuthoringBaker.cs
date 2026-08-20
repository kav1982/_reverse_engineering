using Unity.Entities;

internal class Monster315ShieldEffectAuthoringBaker : Baker<Monster315ShieldEffectAuthoring>
{
	public override void Bake(Monster315ShieldEffectAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster315ShieldEffect component = new Monster315ShieldEffect
		{
			scaleRoot = GetEntity(authoring.ScaleRoot, TransformUsageFlags.Dynamic)
		};
		AddComponent(entity, in component);
	}
}
