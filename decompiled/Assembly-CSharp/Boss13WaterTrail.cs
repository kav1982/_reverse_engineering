using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Boss13WaterTrail : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.BoxCollider BC;

	public float waterForce;

	public float summonReduceFactor;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2097664u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, BC);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void OnTriggerStay_Dots(Entity other)
	{
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		if (layer == 512 || layer == 2097152)
		{
			if (PlayerMgr.Inst.PlayerEtt == other)
			{
				UnitProperty_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(other);
				componentData.TakeKnockback(-base.transform.right * waterForce * Time.deltaTime);
				UnitDotsSyncSystem.SetComponentData(componentData, other);
			}
			else
			{
				UnitProperty_Dots componentData2 = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(other);
				componentData2.TakeKnockback(-base.transform.right * waterForce * Time.deltaTime / summonReduceFactor);
				UnitDotsSyncSystem.SetComponentData(componentData2, other);
			}
		}
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}
}
