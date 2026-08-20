using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Boss13_FalculaTrigger : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider CC;

	public Boss13_FalculaHead head;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2097664u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		if (layer != 512 && layer != 2097152)
		{
			return;
		}
		if (other == PlayerMgr.Inst.PlayerEtt)
		{
			if (!head.hasHitted)
			{
				head.state = Boss13_FalculaHead.FalculaState.Hit;
				head.hasHitted = true;
			}
		}
		else
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13.Inst.myPpt.myEntity);
			info.damage = 999f;
			info.ignoreFloatText = true;
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}
}
