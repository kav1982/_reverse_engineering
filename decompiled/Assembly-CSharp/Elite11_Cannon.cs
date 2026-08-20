using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Elite11_Cannon : MonoBehaviour
{
	[Header("发射")]
	public bool dropped;

	private float horizontalSpeed;

	public VariableFloat upSpeed;

	private float nowUpSpeed;

	public float gravity;

	private Vector3 diration;

	public Shadow shadow;

	private float flyTime;

	[Header("伤害")]
	public float range;

	public int damage;

	public bool exploded;

	public float knockback;

	public Transform tsf_warningCircle;

	public Transform tsf_WarningScale;

	public MeshRenderer mr_bullet;

	public VariableFloat rotateSpeed;

	private float rotateRight;

	public ParticleSystem trailParticle;

	public ParticleSystem explodeParticle;

	public ShockParam shockParam;

	private Vector3 targetPoint;

	[Header("污染")]
	public bool canCorrupt;

	public float corruptTime;

	public int corruptDamage;

	public float checkInterval;

	public float corruptInterval;

	public ParticleSystem corruptionParticle;

	public ParticleSystem corruptionSurfaceParticle;

	private float corruptTimer;

	public List<Entity> attackedEntities = new List<Entity>();

	private List<float> attackedEntitiesCD = new List<float>();

	private float generalTimer;

	private bool frame1Initialized;

	private List<UnitDotsSyncSystem.DistanceHitResult> targetsInRange = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public void SetTarget(Vector3 targetPoint, float initialHeight)
	{
		nowUpSpeed = upSpeed.RandomResult();
		horizontalSpeed = GeneralTool.CannonSpeed(nowUpSpeed, initialHeight, gravity, (Tool2D.IgnoreZPoint(base.transform.position) - targetPoint).magnitude);
		flyTime = GeneralTool.CannonLandTime(nowUpSpeed, initialHeight, gravity);
		diration = Tool2D.IgnoreZPoint(targetPoint - base.transform.position).normalized;
		attackedEntities.Clear();
		attackedEntitiesCD.Clear();
		dropped = false;
		canCorrupt = false;
		shadow.Show();
		tsf_WarningScale.transform.position = Tool2D.GetLayerPoint(targetPoint, LayerCorrectType.GroundEffect);
		tsf_WarningScale.gameObject.SetActive(value: true);
		this.targetPoint = targetPoint;
		generalTimer = 0f;
		tsf_warningCircle.transform.localScale = Vector3.one * generalTimer / flyTime;
		rotateRight = ((!GeneralTool.ChanceResult(0.5f)) ? 1 : (-1));
		rotateSpeed.RandomResult();
		mr_bullet.enabled = true;
		frame1Initialized = false;
	}

	private void OnFrame1()
	{
		trailParticle.Play();
	}

	private void Update()
	{
		for (int num = attackedEntitiesCD.Count - 1; num >= 0; num--)
		{
			attackedEntitiesCD[num] -= Time.deltaTime;
			if (attackedEntitiesCD[num] < 0f)
			{
				attackedEntitiesCD.RemoveAt(num);
				attackedEntities.RemoveAt(num);
			}
		}
		if (!frame1Initialized)
		{
			frame1Initialized = true;
			OnFrame1();
		}
		generalTimer += Time.deltaTime;
		if (!dropped)
		{
			nowUpSpeed += gravity * Time.deltaTime;
			base.transform.position += new Vector3(0f, 0f, (0f - nowUpSpeed) * Time.deltaTime) + diration * horizontalSpeed * Time.deltaTime;
			tsf_warningCircle.transform.localScale = Vector3.one * generalTimer / flyTime;
			mr_bullet.transform.localEulerAngles = new Vector3(0f, 0f, mr_bullet.transform.localEulerAngles.z + rotateRight * rotateSpeed.result * Time.deltaTime);
			if (base.transform.position.z > 0f)
			{
				tsf_WarningScale.gameObject.SetActive(value: false);
				dropped = true;
				canCorrupt = true;
				generalTimer = 0f;
				Explode();
				trailParticle.Stop();
				explodeParticle.Play();
				corruptionParticle.Play();
				corruptionSurfaceParticle.Play();
				mr_bullet.enabled = false;
			}
		}
		if (dropped && canCorrupt)
		{
			corruptTimer += Time.deltaTime;
			if (corruptTimer > checkInterval)
			{
				corruptTimer = 0f;
				Corruption();
			}
			if (generalTimer > corruptTime || Elite11.Inst.myPpt.AlreadyDead)
			{
				canCorrupt = false;
				generalTimer = 0f;
				corruptionParticle.Stop();
				corruptionSurfaceParticle.Stop();
			}
		}
		if (dropped && !canCorrupt && generalTimer > 3f)
		{
			Elite11.MiniPool.RecycleGO(base.gameObject);
		}
		trailParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position);
		explodeParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position);
		corruptionSurfaceParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position) + new Vector3(0f, 0f, -2f);
		corruptionParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.GroundEffect);
		tsf_WarningScale.transform.position = Tool2D.GetLayerPoint(targetPoint, LayerCorrectType.GroundEffectLow);
	}

	private void Corruption()
	{
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, range, GameConst.Filter_MonsterAoeNoSpell, targetsInRange);
		for (int i = 0; i < targetsInRange.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = targetsInRange[i];
			if (!attackedEntities.Contains(distanceHitResult.entity))
			{
				attackedEntities.Add(distanceHitResult.entity);
				attackedEntitiesCD.Add(corruptInterval);
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite11.Inst.myPpt.myEntity);
				info.damage = corruptDamage;
				info.teammateTakeDamageRatio = 4f;
				UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
			}
		}
	}

	private void Explode()
	{
		shadow.Hide();
		SEMgr.Inst.monster34Explosion.PlaySE();
		CamController.Inst.SetShock(shockParam);
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
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite11.Inst.myPpt.myEntity);
					info.damage = damage;
					info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, base.transform.position) * knockback;
					info.teammateTakeDamageRatio = 4f;
					UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				}
				break;
			}
		}
	}
}
