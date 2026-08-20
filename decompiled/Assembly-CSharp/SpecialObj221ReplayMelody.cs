using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;
using UnityEngine;

public class SpecialObj221ReplayMelody : LayerCorrect, IDotsCollisionReceiver, IDotsPhysicsReciever, IDotsTriggerReceiver
{
	public GameObject Outline;

	public SpecialObj221 specialObj221;

	public UnityEngine.CapsuleCollider thisTrigger;

	public UnityEngine.CapsuleCollider thisCollider;

	public Entity thisEntity { get; set; }

	public void Start()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 67108864u;
		collisionFilter.CollidesWith = 512u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisTrigger);
	}

	public void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (!specialObj221.IsComplete && other == PlayerMgr.Inst.PlayerEtt)
		{
			Outline.SetActive(value: true);
			if (!specialObj221.IsComplete)
			{
				specialObj221.ReplayMelody();
			}
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
		Outline.SetActive(value: false);
	}

	void IDotsCollisionReceiver.OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
	}

	void IDotsCollisionReceiver.OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
	}

	void IDotsCollisionReceiver.OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}
}
