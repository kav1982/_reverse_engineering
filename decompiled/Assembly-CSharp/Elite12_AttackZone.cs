using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Elite12_AttackZone : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public Elite12_1 master;

	public int damage;

	public float attackInterval;

	public float knockback;

	public CapsuleCollider CC;

	public ShockParam shock;

	public ParticleSystem dustParticle;

	public ParticleSystem dustParticle_H;

	private List<Entity> attackedEntities = new List<Entity>();

	private List<float> attackedIntervals = new List<float>();

	public Entity thisEntity { get; set; }

	private void Start()
	{
		if (GameMgr.IsChAge14_Static)
		{
			dustParticle = dustParticle_H;
		}
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void OnEnable()
	{
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterEffectBulletNoSpell, CC);
	}

	private void Update()
	{
		for (int num = attackedEntities.Count - 1; num >= 0; num--)
		{
			attackedIntervals[num] -= Time.deltaTime;
			if (attackedIntervals[num] <= 0f)
			{
				attackedIntervals.RemoveAt(num);
				attackedEntities.RemoveAt(num);
			}
		}
	}

	public void Damage()
	{
		CC.enabled = true;
		dustParticle.Play();
	}

	public void NoDamage()
	{
		CC.enabled = false;
		dustParticle.Stop();
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
		((IDotsTriggerReceiver)this).OnTriggerEnter_Dots(other);
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (attackedEntities.Contains(other))
		{
			return;
		}
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		float3 position = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position;
		switch (layer)
		{
		case 256u:
		{
			for (int i = 0; i < Elite12_1.Inst.rocks.Count; i++)
			{
				if (Elite12_1.Inst.rocks[i].thisEntity == other)
				{
					Vector3 to = (Vector3)position - master.transform.position;
					Elite12_1.MiniPool.GetGO("Prefabs/EF/EF_Elite12_Hit" + (GameMgr.IsChAge14_Static ? " H" : ""), position, Quaternion.Euler(new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, to) + 90f)), 3f);
					Elite12_1.Inst.rocks[i].Die();
					CamController.Inst.SetShock(shock);
					SEMgr.Inst.monster37_KnockWall.PlaySE(SEPlayMode.Replay, 1);
					break;
				}
			}
			break;
		}
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(other, out var result))
			{
				Vector3 vector = Tool2D.IgnoreZV2ToV1Normal(position, base.transform.position) * 0.5f + master.dashDirection.normalized * 0.5f;
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite12_1.Inst.myPpt.myEntity);
				info.damage = damage;
				info.knockbackForce = vector * knockback;
				info.teammateTakeDamageRatio = 4f;
				if (result.unitCfg.unitType == UnitType.NotAttack)
				{
					info.damage = 99999f;
					info.ignoreFloatText = true;
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
				if (layer != 32768)
				{
					Elite12_1.MiniPool.GetGO("Prefabs/EF/EF_Elite12_Hit" + (GameMgr.IsChAge14_Static ? " H" : ""), position, Quaternion.Euler(new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, vector) + 90f)), 3f);
					attackedEntities.Add(other);
					attackedIntervals.Add(attackInterval);
					SEMgr.Inst.monster37_KnockUnit.PlaySE();
				}
			}
			break;
		}
		}
	}
}
