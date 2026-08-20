using Unity.Entities;

internal class Monster316BuffAuthoringBaker : Baker<Monster316BuffAuthoring>
{
	public override void Bake(Monster316BuffAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster316Buff_Dots component = new Monster316Buff_Dots
		{
			scaleRoot = GetEntity(authoring.scaleRoot, TransformUsageFlags.Dynamic)
		};
		AddComponent(entity, in component);
	}
}
