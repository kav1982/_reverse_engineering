using Unity.Entities;

internal class GuideCampDestroyAuthoringBaker : Baker<GuideCampDestroyAuthoring>
{
	public override void Bake(GuideCampDestroyAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.NonUniformScale);
		GuideCampDestroyTag component = new GuideCampDestroyTag
		{
			isMobile = authoring.isMobile
		};
		AddComponent(entity, in component);
	}
}
