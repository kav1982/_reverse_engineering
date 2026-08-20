using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Elite10_DamageZone : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public Elite10 master;

	public int damage;

	public float knockback;

	public CapsuleCollider CC;

	public ParticleSystem attackZoneParticle;

	public ParticleSystem attackZoneParticle_H;

	public ShockParam bigShock;

	public ShockParam shock;

	private float shockTimer;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterAoeNoSpell, CC);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		attackZoneParticle.transform.position = Tool2D.GetLayerPoint(master.transform.position) + Vector3.forward;
	}

	public void Open()
	{
		CC.enabled = true;
		CamController.Inst.SetShock(bigShock);
		if (GameMgr.IsChAge14_Static)
		{
			attackZoneParticle = attackZoneParticle_H;
		}
		attackZoneParticle.Play();
		SEMgr.Inst.monster26BigLand.PlaySE();
		SEMgr.Inst.elite10Scratch.PlaySE();
	}

	public void Close()
	{
		CC.enabled = false;
		attackZoneParticle.Stop();
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}

	public void OnTriggerEnter_Dots(Entity other)
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
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite10.Inst.myPpt.myEntity);
			info.damage = damage;
			info.ignorePlayerInvincibleFrame = true;
			LocalTransform componentData2 = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other);
			info.knockbackForce = ((Vector3)componentData2.Position - master.transform.position).normalized * 0.5f + master.dashDir.normalized * 0.5f * knockback;
			if (componentData.unitCfg.unitType == UnitType.NotAttack)
			{
				info.damage = 99999f;
				info.ignoreFloatText = true;
			}
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			if (componentData.unitCfg.unitType != UnitType.Brittleness)
			{
				Elite10.MiniPool.GetGO("Prefabs/EF/EF_Monster37_Hit" + (GameMgr.IsChAge14_Static ? " H" : ""), componentData2.Position, Quaternion.Euler(0f, (!(master.dashDir.x > 0f)) ? 180 : 0, 0f), 3f);
			}
			break;
		}
		}
	}
}
