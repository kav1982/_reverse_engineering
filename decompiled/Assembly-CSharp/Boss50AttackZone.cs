using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Boss50AttackZone : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.BoxCollider BC;

	public List<Entity> attackedEtt = new List<Entity>();

	public float ramKnockBackForce;

	public float damage;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2228736u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, BC);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		if (Boss50.Inst.state != Boss50.MonsterState.Charge)
		{
			return;
		}
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 131072u:
		{
			TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(Boss50.Inst.myPpt.myEntity);
			info2.damage = damage * 10f;
			UnitDotsSyncSystem.AddTakeDamageRequestEndless(other, info2);
			break;
		}
		case 512u:
		case 2097152u:
			if (!attackedEtt.Contains(other))
			{
				attackedEtt.Add(other);
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss50.Inst.myPpt.myEntity);
				info.damage = damage;
				Vector3 vector = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position;
				Vector3 normalized = (Boss50.Inst.moveDir + Tool2D.IgnoreZV2ToV1Normal(vector, Boss50.Inst.transform.position)).normalized;
				info.knockbackForce = normalized * ramKnockBackForce;
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(other, info);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch_Large", Tool2D.GetLayerPoint(vector), Quaternion.Euler(new Vector3(0f, 0f, Tool2D.IgnoreZAngle(Vector3.up, normalized) - 90f)), Vector3.one, 3f);
			}
			break;
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}
}
