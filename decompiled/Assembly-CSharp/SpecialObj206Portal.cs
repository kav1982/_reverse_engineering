using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj206Portal : LayerCorrect, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	private Vector3 nextPoint;

	public UnityEngine.BoxCollider thisCollider;

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	public void Initialize(Vector3 next)
	{
		tsf_Layer.transform.localPosition = new Vector3(0f, 0f, 1.12f);
		nextPoint = next;
	}

	public override void OnEnable()
	{
		base.OnEnable();
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 67108864u;
		collisionFilter.CollidesWith = 512u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void SetNextPoint(Vector3 next)
	{
		nextPoint = next;
	}

	private void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (other == PlayerMgr.Inst.PlayerEtt)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/Item/Curse_InjuredRandomPoint", PlayerMgr.Inst.PlayerT.position, 2f);
			ObjPoolMgr.Inst.GetGO("Prefabs/Item/Curse_InjuredRandomPoint", nextPoint, 2f);
			PlayerMgr.Inst.SetPlayerPoint(nextPoint);
			PlayerMgr.Inst.ItemCtrller.ItemPointerToPlayer();
			SEMgr.Inst.curseInjuredRandomPoint.PlaySE();
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
