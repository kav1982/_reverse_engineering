using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss13Stage3FollowMissile : UnitBase
{
	public static List<Boss13Stage3FollowMissile> followMissiles = new List<Boss13Stage3FollowMissile>();

	public float flyTime;

	private float flyTimer;

	public float flyXSpeed;

	private ParticleSystem bubbleEffect;

	public VariableFloat turnSpeed;

	public VariableFloat moveSpeed;

	public float damage;

	public float damageRadius;

	public float objDamageFactor;

	public float knockBack;

	public ShockParam shockParam;

	public Transform motion;

	public Transform particleRoot;

	public VariableFloat offsetX;

	public VariableFloat offsetY;

	public float shootStartHeight;

	public AnimationCurve heightCurve;

	private Vector3 lastFramePos;

	public float duration;

	public float durationTimer;

	public bool isTimeUp;

	public Animator anim;

	private Vector2 berlinSeed;

	public float shakeFrequency;

	public float shakeAmplitude;

	private Vector3 originModelLocalPosition;

	public float bangTime;

	public float bangTimer;

	public int bulletAmount;

	public float startRotateHeight;

	public float extraSpeed;

	public float extraSpeedDecay;

	private List<UnitDotsSyncSystem.DistanceHitResult> distanceHits = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public Entity thisEntity { get; set; }

	public bool onLand { get; set; }

	public Vector3 moveDir { get; set; }

	public override void EveryInitialCallback()
	{
		isTimeUp = false;
		originModelLocalPosition = motion.localPosition;
		berlinSeed = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		moveSpeed.RandomResult();
		turnSpeed.RandomResult();
		if (GameMgr.IsMobile_Static)
		{
			moveSpeed.result *= 0.9f;
			turnSpeed.result *= 0.9f;
		}
		offsetX.RandomResult();
		offsetY.RandomResult();
		onLand = false;
		flyTimer = 0f;
		lastFramePos = base.transform.position;
		shootStartHeight = base.transform.position.z;
		GetNearestTargetPlayerFirst();
		moveDir = Tool2D.GetDir();
		bubbleEffect = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13FollowMissileTrailStage3", particleRoot.position).GetComponent<ParticleSystem>();
		bubbleEffect.Clear();
		bubbleEffect.Play();
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		SetComponentData(componentData);
	}

	public override void Update()
	{
		base.Update();
		if (onLand)
		{
			moveDir = Tool2D.RotateTowardsAroundZAxis(moveDir, Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint + new Vector3(offsetX.result, offsetY.result, 0f), base.transform.position), turnSpeed.result * Time.deltaTime);
			extraSpeed -= Time.deltaTime * extraSpeedDecay;
			extraSpeed = Mathf.Max(extraSpeed, 0f);
			base.transform.position += moveDir * (extraSpeed + moveSpeed.result) * Time.deltaTime;
			motion.up = moveDir;
		}
		else
		{
			flyTimer += Time.deltaTime;
			base.transform.position += moveDir * flyXSpeed * Time.deltaTime;
			base.transform.position = Tool2D.IgnoreZPoint(base.transform.position, shootStartHeight * heightCurve.Evaluate(flyTimer / flyTime));
			Vector3 vector = base.transform.position - lastFramePos;
			motion.up = new Vector3(vector.x, vector.y - vector.z, 0f);
			lastFramePos = base.transform.position;
			if (flyTimer > flyTime)
			{
				onLand = true;
			}
		}
		if (bubbleEffect != null)
		{
			bubbleEffect.transform.position = particleRoot.position;
			bubbleEffect.transform.eulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngle(Vector3.up, moveDir) - 90f);
		}
		SyncDotsPosition();
		durationTimer += Time.deltaTime;
		if (durationTimer > duration && !isTimeUp)
		{
			durationTimer = 0f;
			isTimeUp = true;
			anim.Play("Flash");
		}
		if (isTimeUp)
		{
			bangTimer += Time.deltaTime;
			Vector2 vector2 = berlinSeed * bangTimer * shakeFrequency;
			float x = Mathf.PerlinNoise(vector2.x, vector2.y) - 0.5f;
			float y = Mathf.PerlinNoise(vector2.y, vector2.x) - 0.5f;
			motion.localPosition = originModelLocalPosition + new Vector3(x, y, 0f) * shakeAmplitude * bangTimer / bangTime;
			if (bangTimer > bangTime)
			{
				DotsAnnouncedDeath();
			}
		}
	}

	private void ExplodeOnce(Vector3 explodePoint)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13Explosion", explodePoint, 6f);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_Trace", explodePoint, 10f);
		CamController.Inst.SetShock(shockParam);
		SEMgr.Inst.boss13BigExplosion.PlaySE(SEPlayMode.Replay, 3, 0.2f);
		UnitDotsSyncSystem.GetCollidersInRange(explodePoint, damageRadius, GameConst.Filter_MonsterAoeUndiffer, distanceHits);
		for (int i = 0; i < distanceHits.Count; i++)
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13.Inst.myPpt.myEntity);
			Entity entity = distanceHits[i].entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
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
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		CamController.Inst.SetShock(shockParam);
		ExplodeOnce(base.transform.position);
		bangTimer = 0f;
		durationTimer = 0f;
		motion.localPosition = originModelLocalPosition;
		if (bubbleEffect != null && ObjPoolMgr.Inst.gameObject.activeInHierarchy)
		{
			bubbleEffect.Stop();
			ObjPoolMgr.Inst.RecycleGO(bubbleEffect.transform.gameObject, 1f);
			bubbleEffect = null;
		}
		followMissiles.Remove(this);
	}
}
