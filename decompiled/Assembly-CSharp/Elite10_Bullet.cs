using System.Collections.Generic;
using UnityEngine;

public class Elite10_Bullet : MonoBehaviour
{
	[Header("状态")]
	public bool flying = true;

	[Header("飞行")]
	private Vector3 startPoint;

	public Vector3 targetPoint;

	public float flyTime;

	private float flyTimer;

	[Header("飞行表现")]
	public ParticleSystem flyParticle;

	public ParticleSystem flyShadowParticle;

	public Transform tsf_Bullet;

	public Transform tsf_Shadow;

	public Transform tsf_Warning;

	public Transform tsf_WarningCircle;

	[Header("攻击")]
	public int damage;

	public float range;

	public float knockback;

	[Header("攻击表现")]
	public ShockParam shockParam;

	public ParticleSystem explodeParticle;

	[Header("导弹")]
	public bool isMissile;

	public float bezierMidPercent;

	public VariableFloat bezierDistance;

	public VariableFloat missileAngle;

	public Vector3 midPoint;

	public AnimationCurve missileSpeedCurve;

	[Header("回收")]
	private float recycleTimer;

	private List<UnitDotsSyncSystem.DistanceHitResult> targetsInRange = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public void Initialize(float flyTime, Vector3 targetPoint, bool isMissile = false)
	{
		startPoint = base.transform.position;
		flying = true;
		this.targetPoint = targetPoint;
		this.flyTime = flyTime;
		flyTimer = 0f;
		recycleTimer = 0f;
		Vector3 layerPoint = Tool2D.GetLayerPoint(targetPoint - startPoint);
		Vector3 v = Tool2D.GetLayerPoint(targetPoint, LayerCorrectType.Shadow) - Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow);
		tsf_Bullet.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZPoint(layerPoint)));
		tsf_Shadow.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZPoint(v)));
		tsf_WarningCircle.localScale = Vector3.one * flyTimer / flyTime;
		tsf_Bullet.gameObject.SetActive(value: true);
		tsf_Shadow.gameObject.SetActive(value: true);
		tsf_Warning.gameObject.SetActive(value: true);
		tsf_Warning.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(targetPoint), LayerCorrectType.GroundEffect);
		tsf_Shadow.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
		tsf_Bullet.transform.position = Tool2D.GetLayerPoint(base.transform.position);
		flyParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position);
		flyShadowParticle.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
		flyParticle.Play();
		flyShadowParticle.Play();
		if (isMissile)
		{
			Vector3 vector = targetPoint - startPoint;
			Vector3 normalized = Vector3.Cross(vector, Tool2D.GetDir(Tool2D.IgnoreZPoint(vector), 90f)).normalized;
			midPoint = startPoint + (targetPoint - startPoint) * bezierMidPercent - Quaternion.AngleAxis(missileAngle.RandomResult(), vector) * normalized * bezierDistance.RandomResult();
		}
	}

	private void Update()
	{
		if (!flying)
		{
			recycleTimer += Time.deltaTime;
			if (recycleTimer > 1f)
			{
				Elite10.MiniPool.RecycleGO(base.gameObject);
			}
			return;
		}
		flyTimer += Time.deltaTime;
		if (!isMissile)
		{
			base.transform.position = Vector3.Lerp(startPoint, targetPoint, flyTimer / flyTime);
		}
		else
		{
			base.transform.position = GeneralTool.FreeBezierCurve(missileSpeedCurve.Evaluate(flyTimer / flyTime), startPoint, midPoint, targetPoint);
		}
		tsf_WarningCircle.localScale = Vector3.one * flyTimer / flyTime;
		tsf_Warning.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(targetPoint), LayerCorrectType.GroundEffect);
		tsf_Shadow.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
		tsf_Bullet.transform.position = Tool2D.GetLayerPoint(base.transform.position);
		flyParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position);
		flyShadowParticle.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
		tsf_Bullet.transform.eulerAngles = new Vector3(0f, 0f, 0f);
		Vector3 layerPoint = Tool2D.GetLayerPoint(targetPoint - startPoint);
		Vector3 v = Tool2D.GetLayerPoint(targetPoint, LayerCorrectType.Shadow) - Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow);
		if (!isMissile)
		{
			tsf_Bullet.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZPoint(layerPoint)));
			tsf_Shadow.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZPoint(v)));
		}
		else
		{
			Vector3 vector = GeneralTool.FreeBezierCurve(missileSpeedCurve.Evaluate(flyTimer + 0.01f / flyTime), startPoint, midPoint, targetPoint);
			layerPoint = Tool2D.GetLayerPoint(vector - base.transform.position);
			v = Tool2D.GetLayerPoint(vector, LayerCorrectType.Shadow) - Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow);
			tsf_Bullet.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZPoint(layerPoint)));
			tsf_Shadow.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZPoint(v)));
		}
		if (flyTimer > flyTime && flying)
		{
			flying = false;
			tsf_Bullet.gameObject.SetActive(value: false);
			tsf_Shadow.gameObject.SetActive(value: false);
			tsf_Warning.gameObject.SetActive(value: false);
			flyParticle.Stop();
			flyShadowParticle.Stop();
			Explode();
		}
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
			if (!UnitDotsSyncSystem.ProcessHitSpell(distanceHitResult.entity, damage, out var _) && UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(distanceHitResult.entity, out var result))
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
		}
	}
}
