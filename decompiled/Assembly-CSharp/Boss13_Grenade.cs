using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;
using UnityEngine;

public class Boss13_Grenade : MonoBehaviour, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider CC;

	public Transform spriteTransform;

	public Rigidbody Rigid;

	public bool isTypeOne;

	[Header("反弹参数")]
	public int reboundTimer;

	public int reboundTime;

	public float reboundHeight;

	private bool baseIsJumping;

	private float baseJumpUpForce;

	private float baseJumpGravity;

	private Vector3 moveDir;

	private float forwardSpeed;

	private float distance;

	public float gravity;

	[Header("爆炸参数")]
	public ShockParam shockParam;

	public float knockBack;

	public float explosionRadius;

	public int boomDamage;

	public float objDamageFactor;

	private bool mobileNerfed;

	private List<UnitDotsSyncSystem.DistanceHitResult> distanceHits = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2228992u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		reboundTimer = 0;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		if (baseIsJumping)
		{
			baseJumpUpForce += baseJumpGravity * Time.deltaTime;
			if (baseJumpUpForce != 0f)
			{
				base.transform.position -= new Vector3(0f, 0f, baseJumpUpForce * Time.deltaTime);
			}
		}
		spriteTransform.right = new Vector3(Rigid.linearVelocity.x, Rigid.linearVelocity.y + baseJumpUpForce, 0f);
		if (!(base.transform.position.z > 0f))
		{
			return;
		}
		ParabolaStop();
		if (isTypeOne)
		{
			reboundTimer++;
			ExplodeOnce(base.transform.position);
			if (reboundTimer >= reboundTime)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
				return;
			}
			Vector3 vector = Tool2D.IgnoreZPoint(base.transform.position) + moveDir * distance;
			StartParabola(vector, 0f, Mathf.Clamp(Tool2D.IgnoreZDistance(base.transform.position, vector), 4f, 5f));
		}
		else
		{
			ExplodeOnce(base.transform.position);
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	public void StartParabola(Vector3 landPoint, float height, float upForce)
	{
		if (!mobileNerfed && GameMgr.IsMobile_Static)
		{
			mobileNerfed = true;
			gravity *= 0.9f;
		}
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f - height);
		moveDir = Tool2D.IgnoreZV2ToV1Normal(landPoint, base.transform.position);
		distance = Tool2D.IgnoreZDistance(landPoint, base.transform.position);
		forwardSpeed = GeneralTool.CannonSpeed(upForce, height, gravity, distance);
		Rigid.linearVelocity = moveDir * forwardSpeed;
		ParabolaStart(upForce, gravity);
		if (!isTypeOne)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle" + (GameMgr.IsHarmony_Static ? " Purple" : ""), landPoint).GetComponent<WarningArea>().Initialize(explosionRadius, Tool2D.IgnoreZDistance(landPoint, base.transform.position) / forwardSpeed);
		}
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

	public void OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
		if (UnitDotsSyncSystem.GetLayer(collision.GetOtherEntity(thisEntity)) == 256 && isTypeOne)
		{
			ParabolaStop();
			ExplodeOnce(base.transform.position);
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	public void OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
	}

	public void OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}

	private Vector3 CalculatePerfectReflection(Vector3 velocity, Vector3 normal)
	{
		moveDir = Vector3.Reflect(moveDir, normal).normalized;
		return moveDir * forwardSpeed;
	}

	private void ExplodeOnce(Vector3 explodePoint)
	{
		if (!isTypeOne)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13ExplosionLarge", explodePoint, 3f);
			SEMgr.Inst.boss13BigExplosion.PlaySE(SEPlayMode.Replay, 3, 0.2f);
		}
		else
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13Explosion", explodePoint, 3f).transform.localScale = new Vector3(explosionRadius / 2f + 0.3f, explosionRadius / 2f + 0.3f, 1f);
			SEMgr.Inst.boss13SmallExplosion.PlaySE(SEPlayMode.Replay, 3, 0.2f);
		}
		CamController.Inst.SetShock(shockParam);
		UnitDotsSyncSystem.GetCollidersInRange(explodePoint, explosionRadius, GameConst.Filter_MonsterAoeUndiffer, distanceHits);
		for (int i = 0; i < distanceHits.Count; i++)
		{
			Entity entity = distanceHits[i].entity;
			uint layer = UnitDotsSyncSystem.GetLayer(entity);
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13.Inst.myPpt.myEntity);
			switch (layer)
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, boomDamage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 2097152u:
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, explodePoint) * knockBack;
				info.damage = boomDamage;
				info.teammateTakeDamageRatio = 4f;
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				break;
			case 131072u:
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, explodePoint) * knockBack;
				info.damage = (float)boomDamage * objDamageFactor;
				info.ignoreFloatText = true;
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				break;
			}
		}
	}
}
