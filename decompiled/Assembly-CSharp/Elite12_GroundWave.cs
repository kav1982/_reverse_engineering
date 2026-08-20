using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Elite12_GroundWave : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	private float speed;

	private Vector3 diration;

	public float knockBack;

	public int damage;

	private bool recycle;

	public CapsuleCollider thisCollider;

	public float lifeTime;

	private float lifeTimer;

	public ParticleSystem mainParticle;

	public ParticleSystem trailParticle;

	private bool frame1;

	public Entity thisEntity { get; set; }

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void Initialize(Vector3 diration, float speed)
	{
		recycle = false;
		this.diration = diration.normalized;
		this.speed = speed;
		thisCollider.enabled = true;
		lifeTimer = 0f;
		frame1 = false;
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterGroundWave, thisCollider);
	}

	private void Frame1Initialize()
	{
		mainParticle.Play();
		trailParticle.Play();
	}

	private void Die(bool playSound = true)
	{
		recycle = true;
		mainParticle.Stop();
		trailParticle.Stop();
		Elite12_1.MiniPool.RecycleGO(base.gameObject, 2f);
		thisCollider.enabled = false;
		if (playSound)
		{
			SEMgr.Inst.monster52_Hit.PlaySE();
		}
	}

	private void Update()
	{
		if (!frame1)
		{
			frame1 = true;
			Frame1Initialize();
		}
		thisCollider.center = -diration * thisCollider.radius;
		lifeTimer += Time.deltaTime;
		if (lifeTimer > lifeTime && !recycle)
		{
			Die(playSound: false);
		}
		if (!recycle)
		{
			base.transform.position += Time.deltaTime * diration * speed;
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		string text = "EF_Monster51_Hit";
		if (GameMgr.IsChAge14_Static)
		{
			text = "EF_Monster51_Hit_H";
		}
		float3 position = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position;
		switch (layer)
		{
		case 256u:
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, position, 3f);
			Die();
			for (int i = 0; i < Elite12_1.Inst.rocks.Count; i++)
			{
				if (Elite12_1.Inst.rocks[i].thisEntity == other)
				{
					Elite12_1.Inst.rocks[i].Die();
					break;
				}
			}
			break;
		}
		case 65536u:
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, position, 3f);
			Die();
			break;
		case 16777216u:
		{
			UnitDotsSyncSystem.ProcessHitSpell(other, damage, out var hitRollBall);
			if (hitRollBall)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, position, 3f);
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
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite12_1.Inst.myPpt.myEntity);
				info.damage = damage;
				info.knockbackForce = diration * knockBack;
				info.teammateTakeDamageRatio = 4f;
				if (result.unitCfg.unitType == UnitType.NotAttack)
				{
					info.damage = 99999f;
					info.ignoreFloatText = true;
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
				if (layer != 32768)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, position, 3f);
					SEMgr.Inst.monster52_Hit.PlaySE();
					Die();
				}
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
