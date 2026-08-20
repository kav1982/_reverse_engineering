using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Elite14_WallTrigger : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public Elite14_Stage2 bigMaster;

	public Elite14_Child master;

	public UnityEngine.CapsuleCollider CC;

	public Entity thisEntity { get; set; }

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (master != null)
		{
			master.Trigger(other);
		}
		if (bigMaster != null)
		{
			bigMaster.Trigger(other);
		}
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnEnable()
	{
		CollisionFilter filter_MonsterEffectBulletNoSpell = GameConst.Filter_MonsterEffectBulletNoSpell;
		filter_MonsterEffectBulletNoSpell.CollidesWith |= 65536u;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter_MonsterEffectBulletNoSpell, CC);
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}
}
