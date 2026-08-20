using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Boss13ReboundBullet : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider CC;

	public Transform tsf_Layer;

	public float targetHeight;

	public float fallSpeed;

	[Header("Bullet")]
	public int damage;

	public int damageToDestructible;

	public float knockbackForce;

	public VariableFloat asPitch;

	public bool beCatched;

	public Boss13BulletRotateZone UAV;

	private Vector3 initialForce;

	private float duration = 10f;

	private float durationTimer;

	private float upSpeed;

	private float gravity;

	private float bounceRatio;

	private float deceleration;

	public Entity thisEntity { get; set; }

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

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2231040u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
		beCatched = false;
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		if (beCatched)
		{
			return;
		}
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
				if (upSpeed < 2f)
				{
					upSpeed = 2f;
				}
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite15_BulletBounce" + (GameMgr.IsChAge14_Static ? " H" : ""), Tool2D.GetLayerPoint(base.transform.position), 1f);
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

	public void OnTriggerEnter_Dots(Entity other)
	{
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 512u:
		case 2097152u:
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13.Inst.myPpt.myEntity);
			info.damage = damage;
			info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position, base.transform.position) * knockbackForce;
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			break;
		}
		case 256u:
			if (!beCatched)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			break;
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}
}
