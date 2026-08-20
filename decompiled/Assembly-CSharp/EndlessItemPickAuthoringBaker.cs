using Unity.Entities;

internal class EndlessItemPickAuthoringBaker : Baker<EndlessItemPickAuthoring>
{
	public override void Bake(EndlessItemPickAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		EndlessItemPick component = default(EndlessItemPick);
		AddComponent(entity, in component);
	}
}
