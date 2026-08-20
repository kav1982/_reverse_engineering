using Unity.Entities;

internal class CampBonfireTagBaker : Baker<CampBonfireTag>
{
	public override void Bake(CampBonfireTag authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		CampBonfireTag_Dots component = default(CampBonfireTag_Dots);
		AddComponent(entity, in component);
	}
}
