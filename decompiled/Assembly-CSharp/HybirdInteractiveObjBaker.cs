using Unity.Entities;

internal class HybirdInteractiveObjBaker : Baker<HybirdInteractiveObjAuthoring>
{
	public override void Bake(HybirdInteractiveObjAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		InteractiveObjRef component = default(InteractiveObjRef);
		AddComponent(entity, in component);
	}
}
