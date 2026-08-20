using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Boss13_Mine : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider CC;

	public float damageRadius;

	public float damage;

	public float objDamageFactor;

	public float knockBack;

	public ShockParam shockParam;

	public Vector3 targetPosition;

	public float moveTime;

	public float moveSpeed;

	public bool moving;

	public bool bangBefore;

	public float bangTime;

	public float bangTimer;

	public Animator animator;

	public VariableFloat rotateSpeed;

	public float currentAngle;

	public Transform mine;

	private Vector2 berlinSeed;

	public float shakeFrequency;

	public float shakeAmplitude;

	private Vector3 originModelLocalPosition;

	public bool disappearing;

	public float disappearTime;

	public float disappeerTimer;

	private bool baseIsJumping;

	private float baseJumpUpForce;

	private float baseJumpGravity;

	private Vector3 moveDir;

	private float forwardSpeed;

	private float distance;

	public float gravity;

	public Rigidbody Rigid;

	public VariableFloat duration;

	public float durationTimer;

	public ParticleSystem mineShine;

	private List<UnitDotsSyncSystem.DistanceHitResult> distanceHits = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		moving = true;
		bangBefore = false;
		bangTimer = 0f;
		rotateSpeed.RandomResult();
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2228992u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
		originModelLocalPosition = mine.transform.localPosition;
		berlinSeed = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		duration.RandomResult();
		durationTimer = 0f;
	}

	private void OnDisable()
	{
		mine.transform.localPosition = originModelLocalPosition;
		disappearing = false;
		mineShine.Stop();
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		durationTimer += Time.deltaTime;
		if (durationTimer > duration.result && !bangBefore)
		{
			bangBefore = true;
			animator.Play("Flash");
			ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle" + (GameMgr.IsHarmony_Static ? " Purple" : ""), base.transform.position).GetComponent<WarningArea>().Initialize(damageRadius, bangTime);
		}
		currentAngle += rotateSpeed.result * Time.deltaTime;
		if (currentAngle > 360f)
		{
			currentAngle -= 360f;
		}
		else if (currentAngle < 0f)
		{
			currentAngle += 360f;
		}
		mine.eulerAngles = new Vector3(0f, 0f, currentAngle);
		if (baseIsJumping)
		{
			baseJumpUpForce += baseJumpGravity * Time.deltaTime;
			if (baseJumpUpForce != 0f)
			{
				base.transform.position -= new Vector3(0f, 0f, baseJumpUpForce * Time.deltaTime);
			}
		}
		if (bangBefore)
		{
			bangTimer += Time.deltaTime;
			Vector2 vector = berlinSeed * bangTimer * shakeFrequency;
			float x = Mathf.PerlinNoise(vector.x, vector.y) - 0.5f;
			float y = Mathf.PerlinNoise(vector.y, vector.x) - 0.5f;
			mine.localPosition = originModelLocalPosition + new Vector3(x, y, 0f) * shakeAmplitude * bangTimer / bangTime;
			if (bangTimer > bangTime)
			{
				bangBefore = false;
				ExplodeOnce(base.transform.position);
			}
		}
		if (base.transform.position.z > 0f && moving)
		{
			moving = false;
			animator.Play("Float");
			mineShine.Play();
			ParabolaStop();
			Rigid.linearVelocity = Vector3.zero;
		}
		if (disappearing)
		{
			disappeerTimer += Time.deltaTime;
			if (disappeerTimer > disappearTime)
			{
				Boss13_Stage2.Inst.boss13_Mines.Remove(this);
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
	}

	private void ExplodeOnce(Vector3 explodePoint)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13Explosion", explodePoint, Quaternion.identity, Vector3.one * 2f, 6f);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_Trace", explodePoint, 10f);
		CamController.Inst.SetShock(shockParam);
		SEMgr.Inst.monster34Explosion.PlaySE();
		UnitDotsSyncSystem.GetCollidersInRange(explodePoint, damageRadius, GameConst.Filter_MonsterAoeUndiffer, distanceHits);
		for (int i = 0; i < distanceHits.Count; i++)
		{
			Entity entity = distanceHits[i].entity;
			uint layer = UnitDotsSyncSystem.GetLayer(entity);
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13.Inst.myPpt.myEntity);
			switch (layer)
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				break;
			}
			case 512u:
			case 2097152u:
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, explodePoint) * knockBack;
				info.damage = damage;
				info.teammateTakeDamageRatio = 4f;
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				break;
			case 32768u:
			case 131072u:
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, explodePoint) * knockBack;
				info.damage = damage * objDamageFactor;
				info.ignoreFloatText = true;
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				break;
			}
		}
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
	}

	public void OnTriggerStay_Dots(Entity other)
	{
		if (!bangBefore)
		{
			uint layer = UnitDotsSyncSystem.GetLayer(other);
			if ((layer == 512 || layer == 2097152) && !moving && !disappearing)
			{
				bangBefore = true;
				animator.Play("Flash");
				SEMgr.Inst.boss13MineTrigger.PlaySE();
				ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle" + (GameMgr.IsHarmony_Static ? " Purple" : ""), base.transform.position).GetComponent<WarningArea>().Initialize(damageRadius, bangTime);
			}
		}
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}

	public void StartParabola(Vector3 landPoint, float upForce)
	{
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, -0.1f);
		moveDir = Tool2D.IgnoreZV2ToV1Normal(landPoint, base.transform.position);
		distance = Vector3.Distance(base.transform.position, landPoint);
		forwardSpeed = GeneralTool.CannonSpeed(upForce, 0f, gravity, distance);
		Rigid.linearVelocity = moveDir * forwardSpeed;
		ParabolaStart(upForce, gravity);
	}

	public void ParabolaStart(float upForce, float gravity)
	{
		if (!baseIsJumping)
		{
			baseIsJumping = true;
			baseJumpUpForce = upForce;
			baseJumpGravity = gravity;
		}
	}

	public void ParabolaStop()
	{
		if (baseIsJumping)
		{
			baseIsJumping = false;
			baseJumpUpForce = 0f;
			baseJumpGravity = 0f;
		}
	}

	public void StartDisappear()
	{
		disappeerTimer = 0f;
		disappearing = true;
		animator.Play("Disappear");
	}
}
