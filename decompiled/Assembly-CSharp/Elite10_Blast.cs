using Unity.Entities;
using UnityEngine;

public class Elite10_Blast : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public Elite10 master;

	[Header("自身数值")]
	public Vector3 diration;

	public float height;

	public float spawnBulletInterval;

	public int damage;

	public float knockBack;

	public float lifeTime;

	public float speed;

	private bool recycle;

	private float existTime;

	private float flyDistance;

	public Transform tsf_Bullet;

	public Transform tsf_Shadow;

	public ParticleSystem trailParticle;

	public ParticleSystem shadowParticle;

	public ParticleSystem shootParticle;

	public CapsuleCollider thisCollider;

	[Header("子弹")]
	public int bulletDamage;

	public float bulletLifeTime;

	public float bulletDistance;

	public float bulletSlowDownTime;

	private SpellSpawnParams ssp;

	public Entity thisEntity { get; set; }

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Start()
	{
		if (GameMgr.IsMobile_Static)
		{
			speed *= 0.9f;
			spawnBulletInterval *= 1.1f;
		}
	}

	public void Initialize(Vector3 diration, Elite10 master)
	{
		tsf_Bullet.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZPoint(diration)));
		tsf_Shadow.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZPoint(diration)));
		tsf_Shadow.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
		tsf_Bullet.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position) + new Vector3(0f, 0f, 0f - height));
		this.diration = diration;
		this.master = master;
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90211);
		UnitBase.UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Shooter = master.myPpt.myEntity;
		sSPModifier.Damage = bulletDamage;
		sSPModifier.Duration = bulletLifeTime;
		sSPModifier.ApplyToSSP(ref ssp);
		flyDistance = 0f;
		trailParticle.Play();
		shadowParticle.Play();
		existTime = 0f;
		recycle = false;
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterAoe, thisCollider);
	}

	private void ShootBullet()
	{
		SEMgr.Inst.elite10Shoot.PlaySE();
		shootParticle.Play();
		UnitBase.UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - height);
		sSPModifier.Float1 = bulletSlowDownTime;
		sSPModifier.Speed = bulletDistance / bulletSlowDownTime * 2f;
		sSPModifier.Float2 = sSPModifier.Speed;
		for (int i = 0; i < 2; i++)
		{
			sSPModifier.Direction = Tool2D.GetDir(diration, (i == 0) ? (-90) : 90);
			sSPModifier.ApplyToSSP(ref ssp);
			Elite10.Inst.ShootSpell(ssp);
		}
	}

	private void Update()
	{
		tsf_Shadow.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
		tsf_Bullet.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position) + new Vector3(0f, 0f, 0f - height));
		existTime += Time.deltaTime;
		base.transform.position += Time.deltaTime * diration * speed;
		flyDistance += Time.deltaTime * speed;
		if (existTime < lifeTime)
		{
			if (flyDistance > spawnBulletInterval)
			{
				flyDistance -= spawnBulletInterval;
				ShootBullet();
			}
		}
		else if (existTime < lifeTime + 2f && !recycle)
		{
			recycle = true;
			trailParticle.Stop();
			shadowParticle.Stop();
		}
		else if (existTime > lifeTime + 2f)
		{
			Elite10.MiniPool.RecycleGO(base.gameObject);
		}
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
		case 16777216u:
		{
			UnitDotsSyncSystem.ProcessHitSpell(other, damage, out var _);
			break;
		}
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
			if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(other))
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite10.Inst.myPpt.myEntity);
				info.damage = damage;
				info.knockbackForce = diration * knockBack;
				info.teammateTakeDamageRatio = 4f;
				UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			}
			break;
		}
	}
}
