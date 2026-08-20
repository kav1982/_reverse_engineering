using Unity.Entities;
using Unity.Mathematics;

public struct Monster327_Dots : IComponentData, IQueryTypeParameter
{
	public Entity missilePrefab;

	public Entity turretRoot;

	public Entity leftMuzzle;

	public Entity rightMuzzle;

	public float turretRotateSpeed;

	public float maxFireAngleError;

	public float missileSpawnYOffset;

	public float3 turretDirection;

	public float3 lockedFireDirection;

	public float firstFireDelay;

	public float fireInterval;

	public float fireTimer;

	public int missilesPerVolley;

	public float missileFireInterval;

	public float afterVolleyLockTime;

	public float afterVolleyLockTimer;

	public float missileFireTimer;

	public int missilesFiredInVolley;

	public int nextTubeIndex;

	public bool isFiringVolley;

	public bool isAfterVolleyLocked;

	public Entity volleyTarget;
}
