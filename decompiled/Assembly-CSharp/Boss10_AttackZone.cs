using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Boss10_AttackZone : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.BoxCollider BC;

	public List<Entity> attackedEtt = new List<Entity>();

	public float ramKnockBackForce;

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
		if (Boss10.Inst.state != Boss10.MonsterState.Ram)
		{
			return;
		}
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 131072u:
		{
			TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(Boss10.Inst.myPpt.myEntity);
			info2.damage = Boss10.Inst.ramDamage * 10f;
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info2);
			break;
		}
		case 512u:
		case 2097152u:
			if (!attackedEtt.Contains(other))
			{
				attackedEtt.Add(other);
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss10.Inst.myPpt.myEntity);
				info.damage = Boss10.Inst.ramDamage;
				Vector3 vector = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position;
				Vector3 normalized = (Boss10.Inst.ramDir + Tool2D.IgnoreZV2ToV1Normal(vector, Boss10.Inst.transform.position)).normalized;
				info.knockbackForce = normalized * ramKnockBackForce;
				UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster37_Hit" + (GameMgr.IsChAge14_Static ? " H" : ""), Tool2D.GetLayerPoint(vector), Quaternion.Euler(new Vector3(0f, 0f, Tool2D.IgnoreZAngle(Vector3.up, normalized) - 90f)), new Vector3(2f, 2f, 1f), 3f);
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
