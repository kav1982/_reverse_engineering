using Unity.Entities;

internal class SpecialObj3AuthoringBaker : Baker<SpecialObj3Authoring>
{
	public override void Bake(SpecialObj3Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Entity entity2 = GetEntity(authoring.tsf_Layer, TransformUsageFlags.Dynamic);
		Entity entity3 = GetEntity(authoring.tsf_Mat, TransformUsageFlags.Dynamic);
		Entity triggerEntity = Entity.Null;
		if (authoring.pattern == SO3Pattern.Trigger)
		{
			triggerEntity = GetEntity(authoring.pattern3Trigger, TransformUsageFlags.Dynamic);
		}
		SpecialObj3_Dots component = new SpecialObj3_Dots
		{
			pattern = authoring.pattern,
			layerEntity = entity2,
			matEntity = entity3,
			triggerEntity = triggerEntity,
			changedState = true,
			stateQuit = true
		};
		AddComponent(entity, in component);
	}
}
