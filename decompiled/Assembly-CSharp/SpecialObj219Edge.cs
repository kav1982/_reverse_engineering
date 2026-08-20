using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj219Edge : LayerCorrect, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public SpecialObj219 SpecialObj219;

	public UnityEngine.BoxCollider thisCollider;

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

	private void Update()
	{
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
		if (!SpecialObj219.IsComplete && other == PlayerMgr.Inst.PlayerEtt)
		{
			SpecialObj219.MoveOUT();
		}
	}
}
