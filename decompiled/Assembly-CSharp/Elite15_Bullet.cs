using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Elite15_Bullet : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public Transform tsf_Layer;

	public float targetHeight;

	public float fallSpeed;

	[Header("Bullet")]
	public int damage;

	public int damageToDestructible;

	public float knockBack;

	public VariableFloat asPitch;

	public UnityEngine.CapsuleCollider thisCollider;

	private Vector3 initialForce;

	private float duration = 10f;

	private float durationTimer;

	private float upSpeed;

	private float gravity;

	private float bounceRatio;

	private float deceleration;

	public Entity thisEntity { get; set; }

	public void OnEnable()
	{
		CollisionFilter filter_MonsterEffectBulletNoSpell = GameConst.Filter_MonsterEffectBulletNoSpell;
		filter_MonsterEffectBulletNoSpell.CollidesWith |= 4096u;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter_MonsterEffectBulletNoSpell, thisCollider);
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void Initialize(Vector3 initialForce)
	{
		this.initialForce = initialForce;
		duration = 10f;
		durationTimer = 0f;
		upSpeed = 0f;
		gravity = 0f;
		bounceRatio = 0f;
		deceleration = 0f;
	}

	public void Initialize(Vector3 initialForce, float upSpeed, float gravity, float bounceRatio)
	{
		this.initialForce = initialForce;
		duration = 10f;
		durationTimer = 0f;
		this.upSpeed = upSpeed;
		this.gravity = gravity;
		this.bounceRatio = bounceRatio;
		deceleration = 0f;
	}

	public void Initialize(Vector3 initialForce, float duration, float deceleration)
	{
		this.initialForce = initialForce;
		this.duration = duration;
		durationTimer = 0f;
		upSpeed = 0f;
		gravity = 0f;
		bounceRatio = 0f;
		this.deceleration = deceleration;
	}

	private void Update()
	{
		if (deceleration != 0f)
		{
			initialForce = Vector3.Lerp(initialForce, Vector3.zero, deceleration * Time.deltaTime);
		}
		base.transform.position += initialForce * Time.deltaTime;
		if (gravity != 0f)
		{
			upSpeed += gravity * Time.deltaTime;
			base.transform.position -= new Vector3(0f, 0f, upSpeed * Time.deltaTime);
			if (base.transform.position.z > 0f)
			{
				base.transform.IgnoreZPoint();
				upSpeed = (0f - upSpeed) * bounceRatio;
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite15_BulletBounce" + (GameMgr.IsHarmony_Static ? " H" : ""), Tool2D.GetLayerPoint(base.transform.position), 1f);
				SEMgr.Inst.elite15BulletBounce.PlaySE().pitch = asPitch.RandomResult();
			}
		}
		else if (base.transform.position.z < targetHeight)
		{
			base.transform.position += new Vector3(0f, 0f, fallSpeed * Time.deltaTime);
			if (base.transform.position.z >= targetHeight)
			{
				base.transform.IgnoreZPoint(targetHeight);
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
		{
			AudioSource audioSource = SEMgr.Inst.elite15BulletMiss.PlaySE(SEPlayMode.Replay, 3, 0.25f);
			if (audioSource != null)
			{
				audioSource.pitch = asPitch.RandomResult();
			}
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite15_BulletHit" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position, 3f);
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			break;
		}
		case 512u:
		case 4096u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite15.Inst.myPpt.myEntity);
			info.damage = damage;
			info.knockbackForce = initialForce.normalized * knockBack;
			info.teammateTakeDamageRatio = 4f;
			if (layer == 4096)
			{
				info.ignoreFloatText = true;
				info.damage = 9999999f;
			}
			if (!(other == Elite15.Inst.myPpt.myEntity))
			{
				if (layer == 131072)
				{
					info.damage = damageToDestructible;
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
				if (layer != 32768)
				{
					SEMgr.Inst.elite15BulletHit.PlaySE().pitch = asPitch.RandomResult();
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite15_BulletHit" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position, 3f);
					ObjPoolMgr.Inst.RecycleGO(base.gameObject);
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
