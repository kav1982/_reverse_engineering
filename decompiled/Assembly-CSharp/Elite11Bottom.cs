using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Elite11Bottom : MonoBehaviour, IDotsPhysicsHolder, IDotsPhysicsReciever
{
	public UnityEngine.Collider thisCollider;

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	public void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 65536u;
		collisionFilter.CollidesWith = DTool.GetCollidesWith(65536u);
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}
}
