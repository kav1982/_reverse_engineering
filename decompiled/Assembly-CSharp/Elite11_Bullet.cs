using System;
using Unity.Entities;
using UnityEngine;

public class Elite11_Bullet : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public enum BulletMode
	{
		Straight,
		Rotate,
		RotateCenter,
		Wave,
		Lerp
	}

	public BulletMode mode;

	[Header("表现")]
	public ParticleSystem trailParticle;

	public ParticleSystem ExplodeParticle;

	public GameObject bulletHead;

	public Shadow thisShadow;

	public CapsuleCollider thisCollider;

	[Header("回收")]
	public float lifeTime;

	private float existTimer;

	private float recycleTimer;

	private bool recycle;

	[Header("数值")]
	public float speed;

	public int damage;

	public float knockBack;

	private Vector3 diration;

	private Vector3 verticalDiration;

	private Vector3 lastFramePosition;

	[Header("固定中心旋转")]
	public float verticalSpeed;

	private bool rotateRight;

	[Header("围绕某点旋转")]
	private float angleSpeed;

	private float radius;

	[Header("位置插值子弹")]
	public AnimationCurve lerpCurve;

	private Vector3 startPoint;

	private Vector3 endPoint;

	private bool frame1;

	private Vector3 center;

	public Entity thisEntity { get; set; }

	public void OnEnable()
	{
		trailParticle.Stop();
		trailParticle.Clear();
		recycle = false;
		existTimer = 0f;
		recycleTimer = 0f;
		frame1 = false;
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterEffectBulletNoSpell, thisCollider);
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void Initialize(BulletMode mode, Vector3 diration, float speed, float lifeTime, float verticalSpeed = 0f, bool rotateRight = false)
	{
		this.diration = diration.normalized;
		verticalDiration = Tool2D.GetDir(this.diration, 90f);
		this.mode = mode;
		this.speed = speed;
		this.lifeTime = lifeTime;
		this.verticalSpeed = verticalSpeed;
		this.rotateRight = rotateRight;
	}

	public void InitializeCenter(BulletMode mode, Vector3 rotateCenter, float speed, float radius, bool rotateRight)
	{
		diration = (base.transform.position - Elite11.Inst.transform.position).normalized;
		center = rotateCenter;
		this.mode = mode;
		this.speed = speed;
		this.radius = radius;
		this.rotateRight = rotateRight;
		lifeTime = radius * 2f * MathF.PI / speed;
		angleSpeed = 360f / lifeTime;
	}

	public void InitializeLerp(BulletMode mode, Vector3 startPoint, Vector3 endPoint, float lifeTime)
	{
		diration = (endPoint - startPoint).normalized;
		this.mode = mode;
		this.startPoint = startPoint;
		this.endPoint = endPoint;
		this.lifeTime = lifeTime;
	}

	public void Frame1Initialize()
	{
		bulletHead.SetActive(value: true);
		thisShadow.Show();
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
		if (!recycle)
		{
			switch (mode)
			{
			case BulletMode.Straight:
				base.transform.position += speed * diration * Time.deltaTime;
				break;
			case BulletMode.Rotate:
				diration = (base.transform.position - Elite11.elite11Position).normalized;
				verticalDiration = Tool2D.GetDir(diration, 90f);
				base.transform.position += speed * diration * Time.deltaTime + (rotateRight ? (0f - verticalSpeed) : verticalSpeed) * verticalDiration * Time.deltaTime;
				break;
			case BulletMode.RotateCenter:
				base.transform.position = center + Tool2D.GetDir(base.transform.position - center, (rotateRight ? angleSpeed : (0f - angleSpeed)) * Time.deltaTime);
				if (base.transform.position - lastFramePosition != Vector3.zero)
				{
					diration = (base.transform.position - lastFramePosition).normalized;
				}
				lastFramePosition = base.transform.position;
				break;
			case BulletMode.Lerp:
				base.transform.position = Vector3.Lerp(startPoint, endPoint, lerpCurve.Evaluate(existTimer / lifeTime));
				break;
			}
		}
		if (Elite11.Inst.myPpt.AlreadyDead)
		{
			recycle = true;
		}
		if (existTimer > lifeTime)
		{
			recycle = true;
		}
		if (recycle)
		{
			if (!trailParticle.isStopped)
			{
				thisShadow.Hide();
				trailParticle.Stop();
			}
			recycleTimer += Time.deltaTime;
			if (recycleTimer > 1f)
			{
				Elite11.MiniPool.RecycleGO(base.gameObject);
			}
		}
	}

	private void Explode()
	{
		ExplodeParticle.Play();
		SEMgr.Inst.elite11BulletHit.PlaySE();
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		if (recycle)
		{
			return;
		}
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite11.Inst.myPpt.myEntity);
		info.damage = damage;
		info.knockbackForce = diration * knockBack;
		info.teammateTakeDamageRatio = 4f;
		switch (layer)
		{
		case 256u:
			Explode();
			recycle = true;
			break;
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(other, out var result))
			{
				if (result.unitCfg.unitType == UnitType.NotAttack)
				{
					info.damage = 99999f;
					info.ignoreFloatText = true;
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
				if (layer != 32768)
				{
					Explode();
					recycle = true;
				}
			}
			break;
		}
		}
	}
}
