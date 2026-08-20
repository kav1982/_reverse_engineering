using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SODotsPhysicsHolder : MonoBehaviour, IDotsPhysicsHolder, IDotsPhysicsReciever
{
	public UnityEngine.Collider thisCollider;

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	public void Start()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 256u;
		collisionFilter.CollidesWith = DTool.GetCollidesWith(256u);
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	private void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}
}
