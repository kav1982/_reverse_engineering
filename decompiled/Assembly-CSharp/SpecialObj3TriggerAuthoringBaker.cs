using Unity.Entities;

internal class SpecialObj3TriggerAuthoringBaker : Baker<SpecialObj3TriggerAuthoring>
{
	public override void Bake(SpecialObj3TriggerAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Entity entity2 = GetEntity(authoring.reciever, TransformUsageFlags.Dynamic);
		SpecialObjTrigger_Dots component = new SpecialObjTrigger_Dots
		{
			reciever = entity2
		};
		AddComponent(entity, in component);
		SpecialObj3_TriggerTag component2 = default(SpecialObj3_TriggerTag);
		AddComponent(entity, in component2);
	}
}
