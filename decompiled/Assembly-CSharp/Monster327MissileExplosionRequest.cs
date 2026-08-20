using Unity.Entities;
using Unity.Mathematics;

public struct Monster327MissileExplosionRequest
{
	public Entity missileEntity;

	public Entity shooter;

	public float3 position;

	public float effectScale;

	public float explosionColliderRadius;

	public bool dontCreateDeadEF;

	public bool kill;
}
