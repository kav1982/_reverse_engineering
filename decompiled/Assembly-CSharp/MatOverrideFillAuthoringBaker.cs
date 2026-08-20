using Unity.Entities;

internal class MatOverrideFillAuthoringBaker : Baker<MatOverrideFillAuthoring>
{
	public override void Bake(MatOverrideFillAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		MatOverrideFill component = new MatOverrideFill
		{
			fill = 1f
		};
		AddComponent(entity, in component);
	}
}
