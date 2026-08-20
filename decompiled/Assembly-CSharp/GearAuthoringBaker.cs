using Unity.Entities;

internal class GearAuthoringBaker : Baker<GearAuthoring>
{
	public override void Bake(GearAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Gear_Dots component = new Gear_Dots
		{
			price = 1,
			normalGear = GetEntity(authoring.normalGear, TransformUsageFlags.Dynamic),
			doubleGear = GetEntity(authoring.doubleGear, TransformUsageFlags.Dynamic)
		};
		AddComponent(entity, in component);
	}
}
