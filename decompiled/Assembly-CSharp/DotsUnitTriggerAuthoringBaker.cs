using Unity.Entities;
using Unity.Physics.Stateful;

internal class DotsUnitTriggerAuthoringBaker : Baker<DotsUnitTriggerAuthoring>
{
	public override void Bake(DotsUnitTriggerAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.NonUniformScale);
		AddBuffer<StatefulTriggerEvent>(entity);
		AddComponent<DotsUnitTrigger>(entity);
		AddComponent<IgnoreSpellHitTag>(entity);
	}
}
