using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Monster37_AttackZone : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public Monster37 master;

	public int damage;

	public float knockback;

	public CapsuleCollider CC;

	public float damageSpeedFix;

	public ParticleSystem attackZoneParticle;

	public Transform particleRoot;

	private bool repositionParticleActive;

	public int DestructibleDamage;

	private Vector3 originOffset;

	private int frameCounter;

	public Entity thisEntity { get; set; }

	private void Start()
	{
		originOffset = base.transform.localPosition;
	}

	private void OnEnable()
	{
		frameCounter = 0;
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		if (frameCounter < 2)
		{
			frameCounter++;
			if (frameCounter >= 2)
			{
				UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterEffectBulletNoSpell, CC);
			}
		}
		if (master.currentSpeed / master.myPpt.unitCfg.moveSpeed > damageSpeedFix)
		{
			if (!attackZoneParticle.isPlaying)
			{
				attackZoneParticle.Play();
			}
			CC.enabled = true;
		}
		else
		{
			if (attackZoneParticle.isPlaying)
			{
				attackZoneParticle.Stop();
			}
			CC.enabled = false;
		}
		if (master.moveDiration.x > 0f)
		{
			base.transform.localPosition = originOffset;
			particleRoot.localPosition = -base.transform.localPosition;
			particleRoot.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			base.transform.localPosition = -originOffset;
			particleRoot.localPosition = -base.transform.localPosition;
			particleRoot.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	public void BeforeReposotion()
	{
		repositionParticleActive = attackZoneParticle.isPlaying;
		attackZoneParticle.Stop();
	}

	public void AfterReposotion()
	{
		if (repositionParticleActive)
		{
			attackZoneParticle.Play();
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		Vector3 vector = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position;
		switch (layer)
		{
		case 256u:
		{
			SEMgr.Inst.monster37_KnockWall.PlaySE();
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster37_Hit" + (GameMgr.IsHarmony_Static ? " H" : ""), vector, Quaternion.Euler(0f, (!(master.moveDiration.x > 0f)) ? 180 : 0, 0f), 3f);
			if (UnitDotsSyncSystem.TryGetComponent<CreateNavMeshObstacle>(other, out var result))
			{
				result.onT6RockDestroyed = true;
				UnitDotsSyncSystem.SetComponentData(result, other);
				if (result.isT22Rock)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_DeadPermanent_Quartz_Green", vector);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Quartz_Green", vector, 2f);
				}
				else
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_DeadPermanent_Quartz", vector);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Quartz", vector, 2f);
				}
			}
			master.currentSpeed = 0f;
			break;
		}
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(master.myPpt.myEntity);
			info.damage = damage;
			info.knockbackForce = ((vector - master.transform.position).normalized * 0.5f + master.moveDiration.normalized * 0.5f) * knockback;
			info.teammateTakeDamageRatio = 2f;
			if (layer == 131072)
			{
				info.damage = 999999f;
				info.ignoreFloatText = true;
			}
			if (layer != 32768)
			{
				SEMgr.Inst.monster37_KnockUnit.PlaySE();
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster37_Hit" + (GameMgr.IsHarmony_Static ? " H" : ""), vector, Quaternion.Euler(0f, (!(master.moveDiration.x > 0f)) ? 180 : 0, 0f), 3f);
				master.currentSpeed = 0f;
			}
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
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
