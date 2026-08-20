using Unity.Entities;
using UnityEngine;

public class Boss50Bullet : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Header("表现")]
	public ParticleSystem trailParticle;

	public ParticleSystem ExplodeParticle;

	public Transform tsf_bulletHead;

	public Shadow shadow;

	public SpriteRenderer SR_Bullet;

	private float rotateDir;

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

	private bool frame1;

	public Entity thisEntity { get; set; }

	public void InitializeSimple(Vector3 direction, float speed)
	{
		this.direction = direction.normalized;
		tsf_bulletHead.transform.eulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.right, direction));
		this.speed = speed;
		if (GameMgr.IsMobile_Static)
		{
			this.speed *= 0.8f;
		}
	}

	public void OnEnable()
	{
		rotateDir = GeneralTool.HalfChanceNPOne();
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
		tsf_bulletHead.transform.localScale = Vector3.one;
		thisCollider.enabled = true;
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
		existTimer += Time.deltaTime;
		tsf_bulletHead.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight), LayerCorrectType.Coordinate);
		if (!recycle)
		{
			base.transform.position += speed * direction * Time.deltaTime;
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
			shadow.Hide();
		}
		if (hit)
		{
			if (SR_Bullet != null)
			{
				SR_Bullet.enabled = false;
			}
		}
		else
		{
			tsf_bulletHead.transform.localScale = Vector3.one * (0.5f - recycleTimer) / 0.5f;
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
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss50.Inst.myPpt.myEntity);
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
				ExplodeParticle.Play();
				SEMgr.Inst.spell9046Hit.PlaySE();
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
