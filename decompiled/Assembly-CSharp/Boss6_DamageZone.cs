using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Boss6_DamageZone : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public Boss6 master;

	public int damage;

	public float knockback;

	public UnityEngine.CapsuleCollider CC;

	public float delayTime;

	public float delayRecordInterval;

	public List<Vector3> delayRecordPoints = new List<Vector3>();

	private float delayTimer;

	private int recordPointsLength;

	public Entity thisEntity { get; set; }

	private void Start()
	{
		recordPointsLength = Mathf.CeilToInt(delayTime / delayRecordInterval);
		delayRecordPoints.Clear();
		delayRecordPoints.Add(base.transform.position);
	}

	private void Update()
	{
		delayTimer += Time.deltaTime;
		if (delayTimer > delayRecordInterval)
		{
			delayTimer -= delayRecordInterval;
			delayRecordPoints.Add(base.transform.position);
			if (delayRecordPoints.Count > recordPointsLength)
			{
				delayRecordPoints.RemoveAt(0);
			}
			CC.center = delayRecordPoints[0] - base.transform.position;
		}
	}

	public void Open()
	{
		CC.enabled = true;
	}

	public void Close()
	{
		CC.enabled = false;
	}

	public void OnEnable()
	{
		CollisionFilter filter_MonsterAoeNoSpell = GameConst.Filter_MonsterAoeNoSpell;
		filter_MonsterAoeNoSpell.CollidesWith |= 8192u;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter_MonsterAoeNoSpell, CC);
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		if (!UnitDotsSyncSystem.EntityIsValid(other))
		{
			return;
		}
		Vector3 vector = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position;
		TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(master.myPpt.myEntity);
		info.damage = damage;
		info.knockbackForce = (vector - master.transform.position).normalized * knockback;
		info.teammateTakeDamageRatio = 4f;
		switch (layer)
		{
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
			if (layer == 131072)
			{
				info.damage = 999999f;
				info.ignoreFloatText = true;
			}
			if (layer != 32768)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch_Large", vector + Tool2D.GetDir() * Random.Range(0f, 0.2f) + new Vector3(0f, -1f, -0.5f), 1f);
				SEMgr.Inst.monster37_KnockUnit.PlaySE();
			}
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			break;
		case 8192u:
			if (other != master.myPpt.myEntity)
			{
				info.damage = 999999f;
				info.ignoreFloatText = true;
				SEMgr.Inst.monster37_KnockUnit.PlaySE();
				UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			}
			break;
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
