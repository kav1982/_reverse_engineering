using Unity.Entities;
using Unity.Physics;

public class SpecialObj223Block : SpecialObj223CarBlocksMono, IDotsPhysicsHolder, IDotsPhysicsReciever
{
	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	private void Start()
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
