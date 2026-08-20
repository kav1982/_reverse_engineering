using Unity.Entities;
using UnityEngine;

public class Boss51Bullet : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Header("表现")]
	public ParticleSystem trailParticle;

	public ParticleSystem ExplodeParticle;

	public Transform tsf_bulletHead;

	public Shadow shadow;

	public SpriteRenderer SR_Bullet;

	public Collider thisCollider;

	public Transform bulletFire;

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

	public float rotationOffset;

	[Header("抛物线版本")]
	public float gravity;

	public ParticleSystem groundParticle;

	public VariableFloat dropBurnTime;

	private Vector3 direction;

	private bool dropped;

	private bool isParabola;

	public float upSpeed;

	private bool frame1;

	public Entity thisEntity { get; set; }

	public void InitializeSimple(Vector3 direction, float speed)
	{
		this.direction = direction.normalized;
		bulletFire.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, -direction));
		this.speed = speed;
		if (GameMgr.IsMobile_Static)
		{
			this.speed *= 0.8f;
		}
		isParabola = false;
	}

	public void InitializePara(Vector3 direction, float speed, float time)
	{
		this.direction = direction.normalized;
		upSpeed = GeneralTool.CannonInitialSpeed(0f - base.transform.position.z, gravity, time);
		this.speed = speed;
		isParabola = true;
		dropBurnTime.RandomResult();
	}

	public void OnEnable()
	{
		trailParticle.Stop();
		trailParticle.Clear();
		bulletFire.gameObject.SetActive(value: true);
		hit = false;
		recycle = false;
		existTimer = 0f;
		recycleTimer = 0f;
		frame1 = false;
		if (SR_Bullet != null)
		{
			SR_Bullet.enabled = true;
		}
		dropped = false;
		tsf_bulletHead.transform.localScale = Vector3.one;
		thisCollider.enabled = true;
		tsf_bulletHead.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Coordinate);
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterEffectBulletNoSpell, thisCollider);
		shadow.Show();
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void Frame1Initialize()
	{
		tsf_bulletHead.gameObject.SetActive(value: true);
		trailParticle.Play();
	}

	private void Update()
	{
		if (!frame1)
		{
			frame1 = true;
			Frame1Initialize();
		}
		if (!recycle)
		{
			if (isParabola)
			{
				if (!dropped)
				{
					base.transform.position += direction * speed * Time.deltaTime;
					upSpeed += Time.deltaTime * gravity;
					base.transform.position += upSpeed * Vector3.back * Time.deltaTime;
					tsf_bulletHead.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Coordinate);
					Vector3 vector = direction * speed + Vector3.up * upSpeed;
					bulletFire.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, -vector));
					if (upSpeed < 0f && base.transform.position.z > 0f)
					{
						dropped = true;
						trailParticle.Stop();
						SR_Bullet.enabled = false;
						shadow.Hide();
						SEMgr.Inst.boss13Stage3BulletHit.PlaySE();
						groundParticle.Play();
						ExplodeParticle.Play();
					}
				}
				else
				{
					if (Boss51.Inst.myPpt.AlreadyDead)
					{
						existTimer = dropBurnTime.result;
					}
					existTimer += Time.deltaTime;
					if (existTimer > dropBurnTime.result)
					{
						recycle = true;
						groundParticle.Stop();
					}
				}
			}
			else
			{
				existTimer += Time.deltaTime;
				tsf_bulletHead.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Coordinate);
				base.transform.position += speed * direction * Time.deltaTime;
				if (existTimer > lifeTime)
				{
					recycle = true;
				}
			}
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
		if (hit || dropped)
		{
			if (SR_Bullet != null)
			{
				SR_Bullet.enabled = false;
			}
		}
		else
		{
			tsf_bulletHead.transform.localScale = Vector3.one * (0.5f - Mathf.Max(0f, recycleTimer)) / 0.5f;
		}
		recycleTimer += Time.deltaTime;
		if (recycleTimer > 1.5f)
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
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		switch (layer)
		{
		case 256u:
		case 65536u:
			recycle = true;
			hit = true;
			ExplodeParticle.Play();
			SEMgr.Inst.boss13Stage3BulletHit.PlaySE();
			break;
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss51.Inst.myPpt.myEntity);
			info.damage = damage;
			info.knockbackForce = direction.normalized * knockBack;
			if (layer == 131072)
			{
				info.damage = 999999f;
				info.ignoreFloatText = true;
			}
			if (layer != 32768)
			{
				recycle = true;
				hit = true;
				if (!dropped)
				{
					ExplodeParticle.Play();
					SEMgr.Inst.boss13Stage3BulletHit.PlaySE();
				}
				else
				{
					groundParticle.Stop();
				}
			}
			SEMgr.Inst.elite9Burn.PlaySE();
			UnitDotsSyncSystem.AddTakeDamageRequestEndless(other, info);
			shadow.Hide();
			bulletFire.gameObject.SetActive(value: false);
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
