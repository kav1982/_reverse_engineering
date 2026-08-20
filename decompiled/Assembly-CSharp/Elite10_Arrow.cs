using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Elite10_Arrow : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Header("状态")]
	public bool flying = true;

	[Header("飞行")]
	private Vector3 startPoint;

	public float minDistance;

	public Vector3 targetDir;

	public VariableFloat flyDistance;

	public VariableFloat flyTime;

	private float flyTimer;

	private Vector3 targetPoint;

	public float startTimeFixDistance;

	public float flyTimeDistanceFix;

	[Header("飞行表现")]
	public ParticleSystem flyParticle;

	public ParticleSystem flyShadowParticle;

	public Transform tsf_Bullet;

	public Transform tsf_Shadow;

	public AnimationCurve arrowSpeedCurve;

	public float bulletHight;

	[Header("爆炸闪光")]
	public float shineSpeed;

	public SpriteRenderer thisRenderer;

	[Header("攻击")]
	public int damage;

	public float range;

	public float knockback;

	public UnityEngine.CapsuleCollider thisCollider;

	[Header("攻击表现")]
	public ShockParam shockParam;

	public ParticleSystem explodeParticle;

	private CollisionFilter Filter_MonsterAoeNoBrittleness = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 1073741824u,
		CollidesWith = 2228736u
	};

	private List<UnitDotsSyncSystem.DistanceHitResult> targetsInRange = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public Entity thisEntity { get; set; }

	public void Initialize(Vector3 targetDir, float range)
	{
		this.targetDir = targetDir.normalized;
		startPoint = Tool2D.IgnoreZPoint(base.transform.position);
		flying = true;
		targetPoint = startPoint + this.targetDir * Mathf.Max(minDistance, flyDistance.RandomResult() + range);
		flyTime.RandomResult();
		flyTimer = 0f;
		tsf_Bullet.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZPoint(targetDir)));
		tsf_Shadow.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZPoint(targetDir)));
		tsf_Bullet.gameObject.SetActive(value: true);
		tsf_Shadow.gameObject.SetActive(value: true);
		tsf_Shadow.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
		tsf_Bullet.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position) + new Vector3(0f, 0f, 0f - bulletHight));
		flyParticle.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position) + new Vector3(0f, 0f, 0f - bulletHight));
		flyShadowParticle.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
		flyParticle.Play();
		flyShadowParticle.Play();
		thisCollider.enabled = true;
		thisRenderer.material.SetFloat("_GlowColorTransparency", 0f);
		UnitPhysicsSyncSystem.RegisterReciever(this, Filter_MonsterAoeNoBrittleness, thisCollider);
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		tsf_Shadow.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
		tsf_Bullet.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position) + new Vector3(0f, 0f, 0f - bulletHight));
		if (flying)
		{
			flyTimer += Time.deltaTime;
			base.transform.position = Vector3.Lerp(startPoint, targetPoint, arrowSpeedCurve.Evaluate(flyTimer / flyTime.result));
			tsf_Shadow.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
			tsf_Bullet.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position) + new Vector3(0f, 0f, 0f - bulletHight));
			flyParticle.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position) + new Vector3(0f, 0f, 0f - bulletHight));
			flyShadowParticle.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
			if (flyTimer > flyTime.result - 0.8f)
			{
				thisRenderer.material.SetFloat("_GlowColorTransparency", 0.5f + Mathf.Sin((flyTimer - flyTime.result + 0.5f) * shineSpeed * MathF.PI * 2f - MathF.PI / 2f));
			}
			if (flyTimer > flyTime.result && flying)
			{
				flying = false;
				tsf_Bullet.gameObject.SetActive(value: false);
				tsf_Shadow.gameObject.SetActive(value: false);
				flyParticle.Stop();
				flyShadowParticle.Stop();
				Explode();
			}
		}
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		if (flying)
		{
			uint layer = UnitDotsSyncSystem.GetLayer(other);
			if (layer == 512 || layer == 131072 || layer == 2097152)
			{
				flying = false;
				tsf_Bullet.gameObject.SetActive(value: false);
				tsf_Shadow.gameObject.SetActive(value: false);
				flyParticle.Stop();
				flyShadowParticle.Stop();
				Explode();
				thisCollider.enabled = false;
			}
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}

	private void Explode()
	{
		SEMgr.Inst.elite10Explosion.PlaySE();
		CamController.Inst.SetShock(shockParam);
		explodeParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position);
		explodeParticle.Play();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, range, GameConst.Filter_MonsterAoe, targetsInRange);
		for (int i = 0; i < targetsInRange.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = targetsInRange[i];
			Entity entity = distanceHitResult.entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
			{
				if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(distanceHitResult.entity, out var result))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite10.Inst.myPpt.myEntity);
					info.damage = damage;
					if (result.unitCfg.unitType == UnitType.NotAttack)
					{
						info.damage = 999999f;
						info.ignoreFloatText = true;
					}
					info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, base.transform.position) * knockback;
					info.teammateTakeDamageRatio = 4f;
					UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				}
				break;
			}
			}
		}
	}
}
