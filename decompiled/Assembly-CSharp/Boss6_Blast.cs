using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Boss6_Blast : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public Boss6_Stage2 master;

	[Header("自身数值")]
	public Vector3 diration;

	public float height;

	public float spawnBulletInterval;

	public int damage;

	public float knockBack;

	public float lifeTime;

	public float speed;

	public UnityEngine.CapsuleCollider thisCollider;

	private bool recycle;

	private float existTime;

	public Transform tsf_Bullet;

	public SpriteRenderer sr_Bullet;

	public SpriteRenderer sr_Shadow;

	public Transform tsf_Shadow;

	public ParticleSystem trailParticle;

	public ParticleSystem shootParticle;

	public ParticleSystem explodeParticle;

	[Header("子弹")]
	public SpellInitialParameter sipBullet = new SpellInitialParameter();

	public VariableInt bulletDamage;

	public VariableFloat bulletLifeTime;

	public VariableFloat bulletDistance;

	public VariableFloat bulletSpeed;

	public float spellHeight;

	[Header("撞墙爆炸")]
	public ShockParam shock;

	public float explodeBulletGroupCount;

	public float explodeBulletCount;

	public float explodeBulletDamage;

	public float explodeBulletSpeed;

	public float explodeBulletSpeed1;

	public float explodeBulletLifeTime;

	public Entity thisEntity { get; set; }

	private void Start()
	{
		if (GameMgr.IsMobile_Static)
		{
			explodeBulletGroupCount -= 3f;
			speed *= 0.8f;
		}
	}

	public void Initialize(Vector3 diration, Boss6_Stage2 master)
	{
		thisCollider.enabled = true;
		sr_Bullet.enabled = true;
		sr_Shadow.enabled = true;
		tsf_Bullet.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZPoint(diration)));
		tsf_Shadow.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZPoint(diration)));
		tsf_Shadow.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
		tsf_Bullet.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position) + new Vector3(0f, 0f, 0f - height));
		this.diration = diration;
		this.master = master;
		shootParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position) + new Vector3(0f, 0f, -0.15f);
		shootParticle.Play();
		sipBullet.spelldataConfig = SpellConfig.GetConfigCopy(90201);
		sipBullet.ownerPpt = master.myPpt;
		trailParticle.Play();
		existTime = 0f;
		recycle = false;
		CollisionFilter filter_MonsterEffectBulletNoSpell = GameConst.Filter_MonsterEffectBulletNoSpell;
		filter_MonsterEffectBulletNoSpell.CollidesWith |= 65536u;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter_MonsterEffectBulletNoSpell, thisCollider);
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		tsf_Shadow.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
		tsf_Bullet.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position) + new Vector3(0f, 0f, 0f - height));
		existTime += Time.deltaTime;
		if (existTime < lifeTime)
		{
			base.transform.position += Time.deltaTime * diration * speed;
		}
		else if (existTime < lifeTime + 2f && !recycle)
		{
			recycle = true;
			trailParticle.Stop();
		}
		if (existTime > lifeTime + 2f)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	private void Explode()
	{
		sr_Shadow.enabled = false;
		sr_Bullet.enabled = false;
		trailParticle.Stop();
		thisCollider.enabled = false;
		SEMgr.Inst.monster34Explosion.PlaySE();
		SEMgr.Inst.boss6_Explode.PlaySE();
		explodeParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position);
		explodeParticle.Play();
		CamController.Inst.SetShock(shock);
		float value = Random.value;
		for (int i = 0; (float)i < explodeBulletGroupCount; i++)
		{
			for (int j = 0; (float)j < explodeBulletCount; j++)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_SimpleBullet" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position).GetComponent<Boss6_Bullet>().InitializeSimple(Tool2D.GetDir(360f / explodeBulletGroupCount * ((float)i + value)), Mathf.Lerp(explodeBulletSpeed, explodeBulletSpeed1, (float)j / (explodeBulletCount - 1f)), explodeBulletLifeTime);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_SimpleBullet" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position).GetComponent<Boss6_Bullet>().InitializeSimple(Tool2D.GetDir(360f / explodeBulletGroupCount * ((float)i + 0.5f + value)), Mathf.Lerp(explodeBulletSpeed, explodeBulletSpeed1, (float)j / (explodeBulletCount - 1f)) - 2f, explodeBulletLifeTime);
			}
		}
		existTime = lifeTime;
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (existTime >= lifeTime)
		{
			return;
		}
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		switch (layer)
		{
		case 256u:
		case 65536u:
			Explode();
			break;
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss6_Stage2.Inst.myPpt.myEntity);
			info.damage = damage;
			info.knockbackForce = diration * knockBack;
			info.teammateTakeDamageRatio = 4f;
			if (layer == 131072)
			{
				info.damage = 999999f;
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
