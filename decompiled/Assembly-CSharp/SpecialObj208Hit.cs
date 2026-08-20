using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj208Hit : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.BoxCollider thisCollider;

	public int damage;

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
		if (other == PlayerMgr.Inst.PlayerEtt && !PlayerMgr.Inst.PlayerPpt.IsFly)
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
			info.damage = damage;
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
