using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Boss6_Cannon : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public Transform tsf_Layer;

	public float fallSpeed;

	[Header("Bullet")]
	public int damage;

	public int damageToDestructible;

	public float knockBack;

	public bool isExplode;

	public bool dropped;

	public Shadow shadow;

	public UnityEngine.CapsuleCollider thisCollider;

	[Header("爆炸")]
	public ParticleSystem trailParticle;

	public Transform tsf_Bullet;

	public int explodeBulletCount;

	public float bulletSpeed;

	public int bulletDamage;

	public int bulletLifeTime;

	public ShockParam shock;

	private float lifeTime;

	private Vector3 targetPoint;

	public Transform tsf_Warning;

	public Transform tsf_WarningCircle;

	private Vector3 startDir;

	private float duration = 10f;

	private float durationTimer;

	private float upSpeed;

	private float horizontalSpeed;

	private float gravity;

	private float bounceSpeed;

	private VariableFloat dropSpeed;

	private VariableFloat dropAngle;

	private float recycleTimer;

	public Entity thisEntity { get; set; }

	public void Start()
	{
		if (GameMgr.IsMobile_Static)
		{
			explodeBulletCount--;
		}
	}

	public void Initialize(Vector3 targetPoint, float upSpeed, float gravity, VariableFloat dropSpeed, VariableFloat dropAngle, float bounceSpeed, bool isExplode = false)
	{
		this.dropSpeed = dropSpeed;
		this.dropAngle = dropAngle;
		this.isExplode = isExplode;
		dropped = false;
		Vector3 vector = Tool2D.IgnoreZPoint(targetPoint - base.transform.position);
		this.bounceSpeed = bounceSpeed;
		startDir = vector.normalized;
		horizontalSpeed = GeneralTool.CannonSpeed(upSpeed, 0f - base.transform.position.z, gravity, vector.magnitude);
		lifeTime = vector.magnitude / horizontalSpeed;
		this.targetPoint = targetPoint;
		duration = 5f;
		durationTimer = 0f;
		this.upSpeed = upSpeed;
		this.gravity = gravity;
		if (isExplode)
		{
			tsf_WarningCircle.localScale = Vector3.one * durationTimer / lifeTime;
			tsf_Warning.gameObject.SetActive(value: true);
			tsf_Warning.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(targetPoint), LayerCorrectType.GroundEffect);
			tsf_Bullet.gameObject.SetActive(value: true);
			shadow.Show();
		}
		else
		{
			CollisionFilter filter_MonsterEffectBulletNoSpell = GameConst.Filter_MonsterEffectBulletNoSpell;
			filter_MonsterEffectBulletNoSpell.CollidesWith |= 65536u;
			UnitPhysicsSyncSystem.RegisterReciever(this, filter_MonsterEffectBulletNoSpell, thisCollider);
		}
		this.dropSpeed.RandomResult();
		this.dropAngle.RandomResult();
		recycleTimer = 0f;
	}

	private void OnDisable()
	{
		if (!isExplode)
		{
			UnitPhysicsSyncSystem.UnregisterReciever(this);
		}
	}

	private void Explode()
	{
		shadow.Hide();
		tsf_Warning.gameObject.SetActive(value: false);
		tsf_Bullet.gameObject.SetActive(value: false);
		trailParticle.Stop();
		SEMgr.Inst.monster34Explosion.PlaySE();
		SEMgr.Inst.boss6_Explode.PlaySE();
		float value = Random.value;
		CamController.Inst.SetShock(shock);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_BulletExplode" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position);
		for (int i = 0; i < explodeBulletCount; i++)
		{
			CamController.Inst.SetShock(shock);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_SimpleBullet" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position).GetComponent<Boss6_Bullet>().InitializeSimple(Tool2D.GetDir((float)(360 / explodeBulletCount) * ((float)i + value)), bulletSpeed, bulletLifeTime, useFakeHeight: false);
		}
	}

	private void Update()
	{
		if (dropped && isExplode)
		{
			recycleTimer += Time.deltaTime;
			if (recycleTimer >= 2f)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			return;
		}
		base.transform.position += startDir * Time.deltaTime * horizontalSpeed;
		if (gravity != 0f)
		{
			upSpeed += gravity * Time.deltaTime;
			base.transform.position -= new Vector3(0f, 0f, upSpeed * Time.deltaTime);
			if (isExplode)
			{
				tsf_WarningCircle.localScale = Vector3.one * durationTimer / lifeTime;
				tsf_Warning.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(targetPoint), LayerCorrectType.GroundEffect);
			}
			if (base.transform.position.z > 0f)
			{
				if (isExplode)
				{
					dropped = true;
					Explode();
					return;
				}
				upSpeed = bounceSpeed;
				if (!dropped)
				{
					SEMgr.Inst.boss6_BulletBounce.PlaySE();
					Vector3 vector = Tool2D.IgnoreZPoint(base.transform.position);
					Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(vector);
					if ((vector - navMeshPointIngoreZ).sqrMagnitude > 0.01f)
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite15_BulletHit" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position, 3f);
						ObjPoolMgr.Inst.RecycleGO(base.gameObject);
						return;
					}
					horizontalSpeed = dropSpeed.result;
					startDir = Tool2D.GetDir(startDir, dropAngle.result);
					dropped = true;
				}
			}
		}
		tsf_Layer.position = Tool2D.GetLayerPoint(base.transform.position);
		durationTimer += Time.deltaTime;
		if (durationTimer >= duration)
		{
			base.transform.localScale = Vector3.one * (base.transform.localScale.x - 5f * Time.deltaTime);
			if (base.transform.localScale.x <= 0f)
			{
				durationTimer = 0f;
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		switch (layer)
		{
		case 256u:
		case 65536u:
			if (dropped)
			{
				SEMgr.Inst.boss6_BulletBounce.PlaySE();
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite15_BulletHit" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position, 3f);
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			break;
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss6.Inst.myPpt.myEntity);
			info.damage = damage;
			info.knockbackForce = startDir.normalized * knockBack;
			info.teammateTakeDamageRatio = 4f;
			if (layer == 131072)
			{
				info.damage = damageToDestructible;
			}
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			if (layer != 32768)
			{
				SEMgr.Inst.elite11BulletHit.PlaySE();
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite15_BulletHit" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position, 3f);
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
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
