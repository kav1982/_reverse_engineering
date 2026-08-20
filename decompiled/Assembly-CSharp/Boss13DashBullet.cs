using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Boss13DashBullet : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider CC;

	public VariableFloat moveSpeed;

	public Vector3 moveDir;

	public float damageMin;

	public float damageMax;

	public float damage;

	public float knockbackForce;

	public VariableFloat duration;

	public float scaleMin;

	public float scaleMax;

	public float scale;

	public float durationTimer;

	public float zoomer;

	public float zoomSpeed;

	public Transform layer;

	public float rotateSpeed;

	public Transform motion;

	public Shadow shadow;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2231040u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
		moveSpeed.RandomResult();
		duration.RandomResult();
		zoomer = 1f;
		motion.localScale = new Vector3(1f, 1f, 1f);
		CC.enabled = true;
		durationTimer = 0f;
		float value = Random.value;
		scale = scaleMin + (scaleMax - scaleMin) * value;
		base.transform.localScale = new Vector3(scale, scale, 1f);
		shadow.SetScale(CC.radius * 2f);
		damage = damageMin + (damageMax - damageMin) * value;
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		base.transform.position += moveDir * moveSpeed.result * Time.deltaTime;
		durationTimer += Time.deltaTime;
		motion.eulerAngles += new Vector3(0f, 0f, rotateSpeed * Time.deltaTime);
		if (durationTimer > duration.result)
		{
			CC.enabled = false;
			zoomer -= Time.deltaTime * zoomSpeed;
			motion.localScale = new Vector3(zoomer, zoomer, 1f);
			shadow.SetScale(CC.radius * 2f * zoomer);
			if (zoomer < 0f)
			{
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
			info.teammateTakeDamageRatio = 4f;
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13DashBulletHit", base.transform.position, 2f).transform.localScale = new Vector3(scale, scale, 1f);
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			break;
		}
		case 256u:
		case 131072u:
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13DashBulletHit", base.transform.position, 2f).transform.localScale = new Vector3(scale, scale, 1f);
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
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
