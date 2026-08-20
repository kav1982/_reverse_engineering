using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Elite13_DamageZone : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public Elite13 master;

	public int damage;

	public float knockback;

	public UnityEngine.CapsuleCollider CC;

	public ShockParam shock;

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		CollisionFilter filter_MonsterAoeNoSpell = GameConst.Filter_MonsterAoeNoSpell;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter_MonsterAoeNoSpell, CC);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void Open()
	{
		CC.enabled = true;
	}

	public void Close()
	{
		CC.enabled = false;
	}

	private void OnTriggerEnter(UnityEngine.Collider other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			UnitProperty_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(other);
			SEMgr.Inst.monster37_KnockUnit.PlaySE();
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite13.Inst.myPpt.myEntity);
			info.damage = damage;
			info.ignorePlayerInvincibleFrame = true;
			LocalTransform componentData2 = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other);
			info.knockbackForce = (((Vector3)componentData2.Position - master.transform.position).normalized * 0.5f + master.myPpt.Rigid.linearVelocity.normalized * 0.5f) * knockback;
			if (componentData.unitCfg.unitType == UnitType.NotAttack)
			{
				info.damage = 99999f;
				info.ignoreFloatText = true;
			}
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			if (componentData.unitCfg.unitType != UnitType.Brittleness)
			{
				Elite13.MiniPool.GetGO("Prefabs/EF/EF_Elite13_Hit", componentData2.Position, 3f);
				SEMgr.Inst.spell1016Hit.PlaySE();
				CamController.Inst.SetShock(shock);
			}
			break;
		}
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
