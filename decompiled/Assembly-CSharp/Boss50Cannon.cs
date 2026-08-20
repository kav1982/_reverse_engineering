using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Boss50Cannon : MonoBehaviour
{
	[Header("炮弹")]
	public float cannonSpeed;

	public float damage;

	public float knockBack;

	public float damageRadius;

	public Transform tsf_bullet;

	public SpriteRenderer sr_Bullet;

	public ShockParam shootShockParam;

	public ShockParam explodeShockParam;

	public Shadow thisShadow;

	public ParticleSystem trailParticle;

	private float existTimer;

	private float lifeTime;

	private float startHeight;

	private float height;

	private Vector3 targetPos;

	private Vector3 startPos;

	private Vector3 dir;

	[Header("制导炮弹")]
	public bool isMissile;

	public VariableFloat missileChaseStartTime;

	public float missileChaseTime;

	public float missileBeforeExplosionTime;

	public float missileStartSpeed;

	public float missileMaxSpeed;

	public float missileAccleration;

	private Vector3 missileVelocity;

	private Entity targetEntity;

	private bool chasing;

	private WarningArea warningArea;

	[Header("破片")]
	public float bulletCount;

	public VariableFloat bulletSpeed;

	public VariableFloat bulletSpeed2;

	public float bulletLifeTime;

	public VariableFloat bulletDamage;

	public float bulletHeight;

	private List<UnitDotsSyncSystem.DistanceHitResult> distanceHits = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public bool exploded { get; private set; }

	public void InitializeMissile(Vector3 startDir, float height, Entity targetEntity)
	{
		exploded = false;
		existTimer = 0f;
		lifeTime = missileChaseTime + missileBeforeExplosionTime;
		missileVelocity = startDir * missileStartSpeed;
		this.height = height;
		tsf_bullet.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.right, missileVelocity));
		this.targetEntity = targetEntity;
		chasing = true;
		warningArea = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle").GetComponent<WarningArea>();
		warningArea.Initialize(damageRadius, lifeTime, zoomDirect: false);
		missileChaseStartTime.RandomResult();
		InitializeCommon();
	}

	public void InitializeCommon()
	{
		sr_Bullet.enabled = true;
		thisShadow.Show();
		CamController.Inst.SetShock(shootShockParam);
		trailParticle.Play();
	}

	public void Initialize(Vector3 targetPos, float height)
	{
		this.targetPos = targetPos;
		startPos = base.transform.position;
		exploded = false;
		existTimer = 0f;
		dir = Tool2D.IgnoreZV2ToV1Normal(targetPos, base.transform.position);
		startHeight = height;
		lifeTime = Tool2D.IgnoreZDistance(targetPos, base.transform.position) / cannonSpeed;
		tsf_bullet.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.right, dir));
		InitializeCommon();
	}

	private void Update()
	{
		existTimer += Time.deltaTime;
		if (!exploded)
		{
			if (isMissile)
			{
				warningArea.tsf_Fill.localScale = Vector3.one * existTimer / lifeTime * damageRadius * 2f;
				warningArea.transform.position = Tool2D.IgnoreZPoint(base.transform.position);
				base.transform.position += missileVelocity * Time.deltaTime;
				tsf_bullet.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.right, missileVelocity));
				tsf_bullet.transform.position = Tool2D.GetLayerPoint(base.transform.position + Vector3.back * height);
				if (existTimer > missileChaseTime)
				{
					chasing = false;
				}
				if (chasing && existTimer > missileChaseStartTime.result)
				{
					if (UnitDotsSyncSystem.EntityIsValid(targetEntity))
					{
						targetPos = UnitDotsSyncSystem.GetComponentData<LocalTransform>(targetEntity).Position;
					}
					else
					{
						chasing = false;
						existTimer = missileChaseTime;
					}
					missileVelocity += Time.deltaTime * missileAccleration * Tool2D.IgnoreZV2ToV1Normal(targetPos, base.transform.position);
					missileVelocity = missileVelocity.normalized * Mathf.Min(missileVelocity.magnitude, missileMaxSpeed);
				}
			}
			else
			{
				base.transform.position = Vector3.Lerp(startPos, targetPos, existTimer / lifeTime);
				height = Mathf.Lerp(startHeight, 0f, existTimer / lifeTime);
				tsf_bullet.transform.position = Tool2D.GetLayerPoint(base.transform.position - Vector3.forward * height);
			}
		}
		if (existTimer > lifeTime && !exploded)
		{
			if (warningArea != null)
			{
				ObjPoolMgr.Inst.RecycleGO(warningArea.gameObject);
				warningArea = null;
			}
			exploded = true;
			ObjPoolMgr.Inst.RecycleGO(base.gameObject, 4f);
			DealDamage();
			sr_Bullet.enabled = false;
			thisShadow.Hide();
			trailParticle.Stop();
		}
	}

	private void DealDamage()
	{
		SpellSpawnParams ssp = UnitDotsSyncSystem.GetSpellPrototype(90461);
		UnitBase.UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Duration = bulletLifeTime;
		sSPModifier.Shooter = Boss50.Inst.myPpt.myEntity;
		sSPModifier.SpawnPosition = base.transform.position - Vector3.forward * bulletHeight;
		sSPModifier.ApplyToSSP(ref ssp);
		float num = 360f / bulletCount;
		for (int i = 0; (float)i < bulletCount; i++)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss50_Bullet", base.transform.position).GetComponent<Boss50Bullet>().InitializeSimple(Tool2D.GetDir(num * ((float)i + Random.Range(0f, 1f))), bulletSpeed.RandomResult());
		}
		for (int j = 0; (float)j < bulletCount; j++)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss50_Bullet", base.transform.position).GetComponent<Boss50Bullet>().InitializeSimple(Tool2D.GetDir(num * ((float)j + Random.Range(0f, 1f))), bulletSpeed2.RandomResult());
		}
		CamController.Inst.SetShock(explodeShockParam);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss50ExplosionLarge", base.transform.position, 3f);
		SEMgr.Inst.boss50CannonExplode.PlaySE(SEPlayMode.Replay, 3, 0.2f);
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, damageRadius, GameConst.Filter_MonsterAoeUndiffer, distanceHits);
		for (int k = 0; k < distanceHits.Count; k++)
		{
			Entity entity = distanceHits[k].entity;
			uint layer = UnitDotsSyncSystem.GetLayer(entity);
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss50.Inst.myPpt.myEntity);
			switch (layer)
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 2097152u:
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[k].point, base.transform.position) * knockBack;
				info.damage = damage;
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(entity, info);
				break;
			case 131072u:
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[k].point, base.transform.position) * knockBack;
				info.damage = damage * 9999f;
				info.ignoreFloatText = true;
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(entity, info);
				break;
			}
		}
	}
}
