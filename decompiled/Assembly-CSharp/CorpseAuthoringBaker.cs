using Unity.Entities;

internal class CorpseAuthoringBaker : Baker<CorpsePhysicsAuthoring>
{
	public override void Bake(CorpsePhysicsAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		CorpsePhysics_Dots component = default(CorpsePhysics_Dots);
		AddComponent(entity, in component);
	}
}
