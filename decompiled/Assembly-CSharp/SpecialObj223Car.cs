using Unity.Entities;
using Unity.Physics;

public class SpecialObj223Car : SpecialObj223CarBlocksMono, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	private void Start()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 67108864u;
		collisionFilter.CollidesWith = 512u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	private void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (other == PlayerMgr.Inst.PlayerEtt && specialObjCar != null && !cantInteract)
		{
			level.TriggerCar(this);
		}
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}
}
