using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Monster52_BladeWave : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
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

	public UnitProperty masterPpt;

	public Entity thisEntity { get; set; }

	public void Initialize(Vector3 diration, float speed, UnitProperty masterPpt)
	{
		recycle = false;
		this.diration = diration.normalized;
		this.speed = speed;
		thisCollider.enabled = true;
		lifeTimer = 0f;
		frame1 = false;
		this.masterPpt = masterPpt;
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterGroundWave, thisCollider);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
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
		Invoke("Fade", 3f);
		thisCollider.enabled = false;
		if (playSound)
		{
			SEMgr.Inst.monster52_Hit.PlaySE();
		}
	}

	private void Fade()
	{
		ObjPoolMgr.Inst.RecycleGO(base.gameObject, 2f);
	}

	private void Update()
	{
		if (!frame1)
		{
			frame1 = true;
			Frame1Initialize();
		}
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
		if (recycle)
		{
			return;
		}
		string text = "EF_Monster51_Hit";
		if (GameMgr.IsChAge14_Static)
		{
			text = "EF_Monster51_Hit_H";
		}
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 256u:
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, Tool2D.IgnoreZPoint(base.transform.position), 3f);
			Die();
			break;
		case 16777216u:
		{
			if (UnitDotsSyncSystem.ProcessHitSpell(other, damage, out var hitRollBall) && hitRollBall)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, Tool2D.IgnoreZPoint(base.transform.position), 3f);
			}
			break;
		}
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			if (!UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(other, out var result))
			{
				break;
			}
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(masterPpt.myEntity);
			info.damage = damage;
			info.knockbackForce = diration * knockBack;
			info.teammateTakeDamageRatio = 3f;
			if (result.unitCfg.unitType == UnitType.NotAttack)
			{
				info.damage *= 6f;
				if (result.unitCfg.currentHP > info.damage)
				{
					Die();
				}
			}
			if (result.unitCfg.unitType != UnitType.Brittleness)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position, 3f);
				SEMgr.Inst.monster52_Hit.PlaySE();
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
