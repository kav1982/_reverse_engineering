using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Boss9_DecorationConvert : MonoBehaviour, IDotsPhysicsHolder, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider CC;

	public UnityEngine.BoxCollider BC;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 256u;
		collisionFilter.CollidesWith = 2228736u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, BC);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}
}
