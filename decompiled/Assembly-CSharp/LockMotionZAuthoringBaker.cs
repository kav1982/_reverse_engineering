using Unity.Entities;

internal class LockMotionZAuthoringBaker : Baker<LockMotionZAuthoring>
{
	public override void Bake(LockMotionZAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		LockMotionZ component = default(LockMotionZ);
		AddComponent(entity, in component);
	}
}
