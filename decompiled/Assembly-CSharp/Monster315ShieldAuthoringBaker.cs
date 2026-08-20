using Unity.Entities;

internal class Monster315ShieldAuthoringBaker : Baker<Monster315ShieldAuthoring>
{
	public override void Bake(Monster315ShieldAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster315Shield_Dots component = new Monster315Shield_Dots
		{
			shieldInactive = false,
			ShieldOn = GetEntity(authoring.ShieldOn, TransformUsageFlags.Dynamic),
			ShieldOn1 = GetEntity(authoring.ShieldOn1, TransformUsageFlags.Dynamic)
		};
		AddComponent(entity, in component);
	}
}
