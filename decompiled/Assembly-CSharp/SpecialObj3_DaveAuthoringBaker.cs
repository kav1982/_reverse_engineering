using Unity.Entities;

internal class SpecialObj3_DaveAuthoringBaker : Baker<SpecialObj3_DaveAuthoring>
{
	public override void Bake(SpecialObj3_DaveAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Entity entity2 = GetEntity(authoring.tsf_Layer, TransformUsageFlags.Dynamic);
		SpecialObj3_Dave_Dots component = new SpecialObj3_Dave_Dots
		{
			layerEntity = entity2
		};
		AddComponent(entity, in component);
	}
}
