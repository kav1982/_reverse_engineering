using System;
using Unity.Entities;
using UnityEngine;

public class Monster319_Bullet : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Header("表现")]
	public Transform tsf_Bullet;

	public ParticleSystem trailParticle;

	public ParticleSystem ExplodeParticle;

	public Shadow shadow;

	public Collider thisCollider;

	[Header("状态")]
	public float lifeTime;

	private float existTimer;

	private float recycleTimer;

	private bool recycle;

	private bool hit;

	private bool buffed;

	private Entity owner;

	[Header("旋转")]
	public bool isRotate;

	public float rotateSpeedRatio;

	private Vector3 initialPoint;

	[Header("爆炸")]
	public VariableFloat explodeDelay;

	public bool isExplode;

	public int explodeBulletCount;

	public AudioSource as_Charge;

	public ParticleSystem chargeParticle;

	private bool chargeStarted;

	[Header("数值")]
	public float speed;

	public int damage;

	public float knockBack;

	public bool isPenetrate;

	private float rotateDegree;

	private Vector3 direction;

	private bool frame1;

	public Entity thisEntity { get; set; }

	public void Initialize(Vector3 direction, Entity owner, bool buffed)
	{
		this.direction = direction.normalized;
		this.owner = owner;
		this.buffed = buffed;
		if (GameMgr.IsMobile_Static)
		{
			speed *= 0.8f;
		}
	}

	public void InitializeRotate(Vector3 direction, Entity owner, bool buffed, bool clockWise, float rotateSpeed = 90f)
	{
		this.direction = direction.normalized;
		this.owner = owner;
		this.buffed = buffed;
		if (GameMgr.IsMobile_Static)
		{
			speed *= 0.8f;
		}
		rotateDegree = (clockWise ? rotateSpeed : (0f - rotateSpeed));
		initialPoint = base.transform.position;
	}

	public void InitializeExplode(Vector3 direction, Entity owner, bool buffed)
	{
		this.direction = direction.normalized;
		this.owner = owner;
		this.buffed = buffed;
		if (GameMgr.IsMobile_Static)
		{
			speed *= 0.8f;
		}
		explodeDelay.RandomResult();
		chargeParticle.Clear();
		chargeParticle.Stop();
		chargeStarted = false;
	}

	public void OnEnable()
	{
		trailParticle.Stop();
		trailParticle.Clear();
		hit = false;
		recycle = false;
		existTimer = 0f;
		recycleTimer = 0f;
		frame1 = false;
		thisCollider.enabled = true;
		tsf_Bullet.localScale = Vector3.one;
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterEffectBulletNoSpell, thisCollider);
		EventMgr.EndlessStageClear = (Action)Delegate.Combine(EventMgr.EndlessStageClear, new Action(Stop));
		shadow.Show();
		if (!isExplode && !isRotate)
		{
			SEMgr.Inst.monster319_Shoot.PlaySE().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
		}
		if (isExplode)
		{
			EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
			SoundVolumeChange();
		}
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
		EventMgr.EndlessStageClear = (Action)Delegate.Remove(EventMgr.EndlessStageClear, new Action(Stop));
		if (isExplode)
		{
			EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		}
	}

	private void SoundVolumeChange()
	{
		as_Charge.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Stop()
	{
		recycle = true;
	}

	public void Frame1Initialize()
	{
		trailParticle.Play();
	}

	private void Update()
	{
		if (!frame1)
		{
			frame1 = true;
			Frame1Initialize();
		}
		existTimer += Time.deltaTime;
		if (isExplode)
		{
			if (existTimer > explodeDelay.result - 1f && !chargeStarted)
			{
				chargeStarted = true;
				chargeParticle.Play();
				as_Charge.Play();
			}
			if (existTimer > explodeDelay.result && !recycle)
			{
				recycle = true;
				Explode();
			}
		}
		if (existTimer > lifeTime)
		{
			recycle = true;
		}
		if (recycle)
		{
			if (isExplode)
			{
				chargeParticle.Clear();
				chargeParticle.Stop();
				as_Charge.Stop();
			}
			if (!hit)
			{
				tsf_Bullet.localScale = Vector3.one * Mathf.Clamp01(1f - recycleTimer * 3f);
			}
			if (!trailParticle.isStopped && hit)
			{
				trailParticle.Stop();
			}
			if (thisCollider.enabled)
			{
				thisCollider.enabled = false;
				shadow.Hide();
			}
			recycleTimer += Time.deltaTime;
			if (recycleTimer > 2f)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
		else
		{
			base.transform.position += Time.deltaTime * direction * speed;
			if (isRotate)
			{
				Vector3 vector = Tool2D.IgnoreZV2ToV1Normal(base.transform.position, initialPoint);
				Vector3 dir = Tool2D.GetDir(vector, rotateDegree);
				direction = vector + dir * rotateSpeedRatio;
				direction = direction.normalized;
			}
		}
	}

	private void Explode()
	{
		hit = true;
		ExplodeParticle.Play();
		chargeParticle.Stop();
		as_Charge.Stop();
		SEMgr.Inst.elite53Shoot.PlaySE();
		Vector3 dir = Tool2D.GetDir();
		float num = 360f / (float)explodeBulletCount;
		for (int i = 0; i < explodeBulletCount; i++)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster319_Bullet", Tool2D.IgnoreZPoint(base.transform.position, -0.9f)).GetComponent<Monster319_Bullet>().Initialize(Tool2D.GetDir(dir, num * ((float)i + UnityEngine.Random.Range(0f, 1f))), owner, buffed: false);
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (recycle || hit)
		{
			return;
		}
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		switch (layer)
		{
		case 256u:
		case 65536u:
			recycle = true;
			hit = true;
			ExplodeParticle.Play();
			SEMgr.Inst.monster319_Hit.PlaySE();
			break;
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(owner);
			info.damage = damage;
			if (buffed)
			{
				info.damage *= 1f;
			}
			info.knockbackForce = direction.normalized * knockBack;
			if (layer == 131072)
			{
				info.damage = 999999f;
				info.ignoreFloatText = true;
			}
			if (layer != 32768 && !isPenetrate)
			{
				recycle = true;
				hit = true;
				ExplodeParticle.Play();
				SEMgr.Inst.monster319_Hit.PlaySE();
			}
			if (layer != 32768 && isExplode)
			{
				Explode();
			}
			UnitDotsSyncSystem.AddTakeDamageRequestEndless(other, info);
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
