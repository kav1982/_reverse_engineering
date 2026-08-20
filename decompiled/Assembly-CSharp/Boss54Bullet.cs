using Unity.Entities;
using UnityEngine;

public class Boss54Bullet : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Header("表现")]
	public ParticleSystem trailParticle;

	public float trailRecycleTime;

	public GameObject bulletHead;

	public GameObject shadow;

	public float bulletHeight;

	[Header("封锁子弹")]
	public bool isBlockSpell;

	public VariableFloat blockSpellStartSpeed;

	public VariableFloat blockSpellSlowDownTime;

	public VariableFloat blockSpellFinalSpeed;

	public VariableFloat blockSpellHeadRotateSpeed;

	private float blockSpellHeadRotateDir;

	[Header("跟踪子弹")]
	public bool isTracking;

	public float trackingRotateSpeed;

	public float canStopTrackingTime;

	public float mustStopTrackingTime;

	private bool tracking;

	[Header("控制")]
	private bool recycle;

	private bool hit;

	[Header("判断")]
	public CapsuleCollider triggerCollider;

	[Header("数值")]
	public float speed;

	private Vector3 direction;

	public int damage;

	public float knockBack;

	private float existTimer;

	private float recycleTimer;

	public VariableFloat lifeTime;

	private bool frame1;

	private Entity master;

	public Entity thisEntity { get; set; }

	public void Frame1Initialize()
	{
		bulletHead.SetActive(value: true);
		shadow.SetActive(value: true);
		trailParticle.Play();
	}

	public void Initialize(Vector3 direction, float speed, Entity master)
	{
		trailParticle.Stop();
		trailParticle.Clear();
		recycle = false;
		hit = false;
		existTimer = 0f;
		lifeTime.RandomResult();
		frame1 = false;
		this.speed = speed;
		this.direction = direction.normalized;
		this.master = master;
		shadow.transform.localScale = Vector3.one;
		bulletHead.transform.localScale = Vector3.one;
		bulletHead.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(direction));
		bulletHead.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight));
		recycleTimer = 0f;
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterAoeNoSpell, triggerCollider);
		tracking = true;
	}

	public void InitializeBlock(Entity master)
	{
		trailParticle.Stop();
		trailParticle.Clear();
		recycle = false;
		hit = false;
		existTimer = 0f;
		lifeTime.RandomResult();
		frame1 = false;
		blockSpellStartSpeed.RandomResult();
		blockSpellSlowDownTime.RandomResult();
		blockSpellFinalSpeed.RandomResult();
		blockSpellHeadRotateSpeed.RandomResult();
		blockSpellHeadRotateDir = GeneralTool.HalfChanceNPOne();
		speed = blockSpellStartSpeed.result;
		direction = Tool2D.GetDir();
		this.master = master;
		recycleTimer = 0f;
		shadow.transform.localScale = Vector3.one;
		bulletHead.transform.localScale = Vector3.one;
		bulletHead.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, direction));
		bulletHead.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight));
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterAoeNoSpell, triggerCollider);
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		if (!frame1)
		{
			frame1 = true;
			Frame1Initialize();
		}
		existTimer += Time.deltaTime;
		if (existTimer > lifeTime.result)
		{
			recycle = true;
		}
		if (isBlockSpell)
		{
			speed = Mathf.Lerp(blockSpellStartSpeed.result, blockSpellFinalSpeed.result, existTimer / blockSpellSlowDownTime.result);
			Vector3 localEulerAngles = bulletHead.transform.localEulerAngles;
			localEulerAngles.z += Mathf.Abs(blockSpellHeadRotateSpeed.result) * blockSpellHeadRotateDir * Time.deltaTime;
			bulletHead.transform.localEulerAngles = localEulerAngles;
		}
		else
		{
			if (isTracking && tracking)
			{
				Vector3 vector = Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position);
				direction = Tool2D.RotateTowardsAroundZAxis(direction, vector, Mathf.Abs(trackingRotateSpeed) * Time.deltaTime).normalized;
				if (canStopTrackingTime < existTimer && Vector3.Dot(direction, vector) < 0f)
				{
					tracking = false;
				}
				if (mustStopTrackingTime < existTimer)
				{
					tracking = false;
				}
			}
			bulletHead.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, direction));
		}
		base.transform.position += direction * speed * Time.deltaTime;
		bulletHead.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight));
		trailParticle.transform.position = bulletHead.transform.position + Vector3.forward * 0.01f;
		shadow.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.GroundEffect);
		if (!recycle)
		{
			return;
		}
		if (hit)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			return;
		}
		if (trailParticle.isPlaying)
		{
			trailParticle.Stop();
		}
		bulletHead.transform.localScale = Vector3.one * Mathf.Lerp(1f, 0f, recycleTimer * 4f);
		shadow.transform.localScale = bulletHead.transform.localScale;
		recycleTimer += Time.deltaTime;
		if (recycleTimer > trailRecycleTime)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (!frame1 || recycle)
		{
			return;
		}
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(other, out var result))
			{
				string text = "EF_Boss54_Hit";
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(master);
				info.damage = damage;
				info.knockbackForce = direction * knockBack;
				if (result.unitCfg.unitType == UnitType.NotAttack)
				{
					info.ignoreFloatText = true;
					info.damage = 99999f;
				}
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(other, info);
				if (result.unitCfg.unitType != UnitType.Brittleness)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight), 3f);
					SEMgr.Inst.boss54BulletHit.PlaySE();
					recycle = true;
					hit = true;
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
