using Unity.Entities;

public struct LockMotionZ : IComponentData, IQueryTypeParameter
{
	public float beforePhysicsZ;
}
