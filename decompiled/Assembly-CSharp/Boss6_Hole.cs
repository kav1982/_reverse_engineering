using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Boss6_Hole : MonoBehaviour, IDotsPhysicsHolder, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider Collider;

	private bool abyssTagAdded;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1024u;
		collisionFilter.CollidesWith = 262144u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, Collider);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		if (UnitDotsSyncSystem.EntityIsValid(thisEntity) && !abyssTagAdded)
		{
			UnitDotsSyncSystem.entityMgr.AddComponent<AbyssTag>(thisEntity);
			abyssTagAdded = true;
		}
	}
}
