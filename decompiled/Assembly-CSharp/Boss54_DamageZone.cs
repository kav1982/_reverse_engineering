using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Boss54_DamageZone : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnitBase master;

	public int damage;

	public float knockback;

	public CapsuleCollider CC;

	public ShockParam shock;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterAoeNoSpell, CC);
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

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
			DoDamage(other);
			break;
		}
	}

	private void DoDamage(Entity other)
	{
		UnitProperty_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(other);
		LocalTransform componentData2 = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other);
		TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(master.myPpt.myEntity);
		info.damage = damage;
		info.knockbackForce = ((Vector3)componentData2.Position - master.transform.position).normalized * knockback;
		if (componentData.unitCfg.unitType == UnitType.NotAttack)
		{
			info.damage = 99999f;
			info.ignoreFloatText = true;
		}
		UnitDotsSyncSystem.AddTakeDamageRequestEndless(other, info);
		if (componentData.unitCfg.unitType != UnitType.Brittleness)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster37_Hit" + (GameMgr.IsChAge14_Static ? " H" : ""), componentData2.Position, 3f);
			SEMgr.Inst.monster37_KnockUnit.PlaySE();
			CamController.Inst.SetShock(shock);
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
