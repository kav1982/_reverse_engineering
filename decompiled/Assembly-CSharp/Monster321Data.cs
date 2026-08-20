using Unity.Entities;

public struct Monster321Data : IComponentData, IQueryTypeParameter
{
	public float ExplosionBaseDamage;

	public float ExplosionRange;

	public float CloseToTargetForceExplosionRange;

	public float PreExplosionDuration;

	public bool IsInitialized;
}
