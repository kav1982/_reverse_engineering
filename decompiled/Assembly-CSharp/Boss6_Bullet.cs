using System;
using Unity.Entities;
using UnityEngine;

public class Boss6_Bullet : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public enum BulletMode
	{
		Straight,
		RotateToward,
		Wave,
		Acclerate,
		GetBack
	}

	public BulletMode mode;

	[Header("表现")]
	public ParticleSystem trailParticle;

	public ParticleSystem ExplodeParticle;

	public Transform tsf_bulletHead;

	public Transform tsf_shadowRoot;

	public SpriteRenderer SR_Shadow;

	public SpriteRenderer SR_Bullet;

	public Collider thisCollider;

	public float bulletHeight;

	public float bulletZOffset;

	[Header("回收")]
	public float lifeTime;

	private float existTimer;

	private float recycleTimer;

	private bool recycle;

	private bool hit;

	[Header("数值")]
	public float speed;

	public int damage;

	public float knockBack;

	private Vector3 direction;

	[Header("旋转直到对齐子弹")]
	public float rotateSpeed;

	private float deltaAngle;

	private float targetAngle;

	private bool rotateRight;

	public float allowAngle;

	private bool aligned;

	private Vector3 targetDir;

	[Header("普通旋转子弹")]
	public float maxRotateAngle;

	[Header("波浪子弹")]
	public float frequency;

	public float amplitude;

	private float startPhase;

	private float sinTimer;

	private Vector3 waveSpeedDir;

	private bool frame1;

	private bool useFakeHeight = true;

	public Entity thisEntity { get; set; }

	public void InitializeSimple(Vector3 direction, float speed, float lifeTime, bool useFakeHeight = true)
	{
		this.direction = direction.normalized;
		mode = BulletMode.Straight;
		this.speed = speed;
		this.lifeTime = lifeTime;
		this.useFakeHeight = useFakeHeight;
		tsf_bulletHead.transform.eulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, direction));
		tsf_shadowRoot.transform.eulerAngles = tsf_bulletHead.transform.eulerAngles;
		if (GameMgr.IsMobile_Static)
		{
			this.speed *= 0.8f;
		}
	}

	public void InitializeWave(Vector3 direction, float speed, float frequency, float waveAmplitude, float startPhase, float lifeTime)
	{
		this.direction = direction.normalized;
		mode = BulletMode.Wave;
		this.speed = speed;
		amplitude = waveAmplitude;
		this.lifeTime = lifeTime;
		this.frequency = frequency;
		this.startPhase = startPhase;
		sinTimer = 0f;
		tsf_bulletHead.transform.localScale = Vector3.one;
		tsf_bulletHead.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight), LayerCorrectType.Coordinate) + new Vector3(0f, 0f, 0f - bulletZOffset);
		trailParticle.Play();
		if (GameMgr.IsMobile_Static)
		{
			this.speed *= 0.8f;
		}
	}

	public void InitializeRotate(Vector3 direction, float speed, float targetAngle, float deltaAngle, float rotateSpeed, float lifeTime)
	{
		this.direction = direction.normalized;
		mode = BulletMode.RotateToward;
		this.speed = speed;
		this.rotateSpeed = rotateSpeed;
		this.deltaAngle = deltaAngle;
		this.targetAngle = targetAngle;
		aligned = false;
		if (GameMgr.IsMobile_Static)
		{
			this.speed *= 0.8f;
		}
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
		if (SR_Bullet != null)
		{
			SR_Bullet.enabled = true;
		}
		if (SR_Shadow != null)
		{
			SR_Shadow.enabled = true;
		}
		tsf_bulletHead.transform.localScale = Vector3.one;
		tsf_shadowRoot.transform.localScale = Vector3.one;
		thisCollider.enabled = true;
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterEffectBulletNoSpell, thisCollider);
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void Frame1Initialize()
	{
		tsf_bulletHead.gameObject.SetActive(value: true);
		tsf_shadowRoot.gameObject.SetActive(value: true);
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
		tsf_bulletHead.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight), LayerCorrectType.Coordinate);
		tsf_shadowRoot.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow);
		if (useFakeHeight)
		{
			tsf_bulletHead.transform.position += new Vector3(0f, 0f, (0f - bulletZOffset) * 0.01f);
		}
		if (!recycle)
		{
			switch (mode)
			{
			case BulletMode.Straight:
				base.transform.position += speed * direction * Time.deltaTime;
				break;
			case BulletMode.RotateToward:
				base.transform.position += speed * direction * Time.deltaTime;
				if (!aligned)
				{
					deltaAngle = Mathf.MoveTowards(deltaAngle, 0f, rotateSpeed * Time.deltaTime);
					direction = Tool2D.GetDir(targetAngle + deltaAngle);
					tsf_bulletHead.transform.eulerAngles = new Vector3(0f, 0f, targetAngle + deltaAngle);
					tsf_shadowRoot.transform.eulerAngles = tsf_bulletHead.transform.eulerAngles;
				}
				break;
			case BulletMode.Wave:
				sinTimer += Time.deltaTime * frequency * 2f * MathF.PI;
				waveSpeedDir = Tool2D.GetDir(direction, Mathf.Cos(sinTimer + startPhase) * amplitude);
				base.transform.position += Time.deltaTime * waveSpeedDir * speed;
				tsf_bulletHead.transform.eulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, waveSpeedDir));
				tsf_shadowRoot.transform.eulerAngles = tsf_bulletHead.transform.eulerAngles;
				break;
			}
		}
		if (existTimer > lifeTime)
		{
			recycle = true;
		}
		if (!recycle)
		{
			return;
		}
		if (!trailParticle.isStopped)
		{
			trailParticle.Stop();
			thisCollider.enabled = false;
		}
		if (hit)
		{
			if (SR_Bullet != null)
			{
				SR_Bullet.enabled = false;
			}
			if (SR_Shadow != null)
			{
				SR_Shadow.enabled = false;
			}
		}
		else
		{
			tsf_bulletHead.transform.localScale = Vector3.one * (0.5f - recycleTimer) / 0.5f;
			tsf_shadowRoot.transform.localScale = tsf_bulletHead.transform.localScale;
		}
		recycleTimer += Time.deltaTime;
		if (recycleTimer > 0.5f)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (recycle || hit)
		{
			return;
		}
		recycle = true;
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		switch (layer)
		{
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss6.Inst.myPpt.myEntity);
			info.damage = damage;
			info.knockbackForce = direction.normalized * knockBack;
			if (mode == BulletMode.Wave)
			{
				info.knockbackForce = waveSpeedDir * knockBack;
			}
			info.teammateTakeDamageRatio = 4f;
			if (layer == 131072)
			{
				info.damage = 999999f;
			}
			if (layer != 32768)
			{
				hit = true;
				ExplodeParticle.Play();
				SEMgr.Inst.elite11BulletHit.PlaySE();
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
