using System.Collections.Generic;
using DG.Tweening;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Elite60Laser : MonoBehaviour
{
	private static readonly int TargetTimer = Shader.PropertyToID("_TargetTimer");

	private static readonly int CurrentTimer = Shader.PropertyToID("_CurrentTimer");

	private float delayShootTimer;

	private float delayLaserTimer;

	private float laserDuration;

	private float laserLength;

	private float laserWidth;

	private float laserDelayDestroyTimer;

	private Elite60LaserState currentState;

	private float laserDamage;

	private Vector3 currentDirection;

	private float chargeSEPlayInterval = 0.1f;

	private bool disableChargeSE;

	private float shootSEPlayInterval = 0.05f;

	private bool disableShootSE;

	private float damageCheckInterval = 0.2f;

	private bool laserDamageIsDps = true;

	private float damageCheckTimer;

	private float chaseTargetAngleSpeed;

	private float chaseTargetTimer;

	private Transform chaseTargetTransform;

	private Vector3 chaseTargetLastPosition;

	private bool chaseTargetRotateInClockwise;

	public GameObject WarningAreaObj;

	public SpriteRenderer OutlineSprite;

	public SpriteRenderer ProgressBarSprite;

	public LineRenderer LaserLineRenderer;

	public Vector3 laserOffset;

	public LineRenderer ShadowLineRenderer;

	public bool isShadow;

	public AnimationCurve LaserWidthCurve;

	private float laserShootWidthChangeTimer;

	public float laserShootWidthChargeTime;

	private float laserDurationTotal;

	public ParticleSystem chargeParticle;

	public ParticleSystem shootParticle;

	private float currentSpeed;

	private Vector3 moveDir;

	public static bool IsBoss52Dead = false;

	private bool shootByOtherSource;

	private Entity shooterEntity;

	private void OnEnable()
	{
		currentState = Elite60LaserState.None;
		WarningAreaObj.SetActive(value: false);
		laserShootWidthChangeTimer = 0f;
		currentDirection = Vector3.zero;
		currentSpeed = 0f;
		shootByOtherSource = false;
		shooterEntity = Entity.Null;
		damageCheckTimer = 0f;
	}

	private void OnDisable()
	{
		WarningAreaObj.SetActive(value: false);
		if (LaserLineRenderer != null)
		{
			LaserLineRenderer.startWidth = 0f;
		}
		if (ShadowLineRenderer != null)
		{
			ShadowLineRenderer.startWidth = 0f;
		}
	}

	public void InitDroneData(float delayShootTimer, float length, float width, float damage, Vector3 InitialDir, float damageCheckInterval, bool laserDamageIsDps, float delayLaserTimer = 0.7f, float afterShootDestroyTimer = 0.1f, float duration = 0.5f, float chaseTargetSpeed = 0f, float chaseTargetTimer = 0f, Transform chaseTargetTransform = null, Vector3 chaseTargetLastPosition = default(Vector3), float initialSpeed = 0f, float targetSpeed = 0f, float speedLerpDuration = 0f, Vector3 initialMoveDirection = default(Vector3), float chargeSEPlayInterval = 0.1f, float shootSEPlayInterval = 0.05f, bool disableChargeSE = false, bool disableShootSE = false, bool isShadow = false)
	{
		this.delayShootTimer = delayShootTimer;
		this.delayLaserTimer = delayLaserTimer;
		laserDamage = damage;
		laserLength = length;
		laserWidth = width;
		laserDuration = duration;
		laserDelayDestroyTimer = afterShootDestroyTimer;
		currentDirection = InitialDir.normalized;
		this.damageCheckInterval = Mathf.Max(damageCheckInterval, 0.01f);
		this.laserDamageIsDps = laserDamageIsDps;
		damageCheckTimer = this.damageCheckInterval;
		this.chargeSEPlayInterval = chargeSEPlayInterval;
		this.shootSEPlayInterval = shootSEPlayInterval;
		this.disableChargeSE = disableChargeSE;
		this.disableShootSE = disableShootSE;
		this.isShadow = isShadow;
		laserDurationTotal = duration;
		laserShootWidthChangeTimer = 0f;
		chaseTargetAngleSpeed = chaseTargetSpeed;
		this.chaseTargetTimer = chaseTargetTimer;
		this.chaseTargetTransform = chaseTargetTransform;
		this.chaseTargetLastPosition = chaseTargetLastPosition;
		chargeParticle.transform.localPosition = laserOffset;
		chargeParticle.gameObject.SetActive(value: false);
		shootParticle.transform.localPosition = laserOffset;
		shootParticle.gameObject.SetActive(value: false);
		if (chaseTargetTransform != null)
		{
			this.chaseTargetLastPosition = chaseTargetTransform.position;
		}
		if (chaseTargetLastPosition != default(Vector3))
		{
			chaseTargetRotateInClockwise = chaseTargetLastPosition.x >= base.transform.position.x;
		}
		if (initialMoveDirection != default(Vector3))
		{
			moveDir = initialMoveDirection.normalized;
			currentSpeed = initialSpeed;
			DOTween.To(() => currentSpeed, delegate(float x)
			{
				currentSpeed = x;
			}, targetSpeed, speedLerpDuration);
		}
		WarningAreaObj.transform.right = currentDirection;
		OutlineSprite.size = new Vector2(length, width);
		ProgressBarSprite.size = new Vector2(length, width);
		ProgressBarSprite.material.SetFloat(CurrentTimer, 0f);
		if (delayLaserTimer > 0f)
		{
			ProgressBarSprite.material.SetFloat(TargetTimer, delayLaserTimer);
		}
		Enterstate(Elite60LaserState.Idle);
	}

	private void Update()
	{
		if (moveDir != default(Vector3) && currentState != Elite60LaserState.AfterShoot)
		{
			base.transform.position += moveDir * currentSpeed * Time.deltaTime;
		}
		UpdateState();
	}

	private void UpdateState()
	{
		switch (currentState)
		{
		case Elite60LaserState.Idle:
			if (delayShootTimer > 0f)
			{
				delayShootTimer -= Time.deltaTime;
				if (delayShootTimer <= 0f)
				{
					Enterstate(Elite60LaserState.PreShoot);
				}
			}
			break;
		case Elite60LaserState.PreShoot:
			ProgressBarSprite.material.SetFloat(CurrentTimer, ProgressBarSprite.material.GetFloat(CurrentTimer) + Time.deltaTime);
			if (chaseTargetTimer > 0f && chaseTargetAngleSpeed > 0f && (chaseTargetTransform != null || chaseTargetLastPosition != default(Vector3)))
			{
				chaseTargetTimer -= Time.deltaTime;
				if (chaseTargetTransform != null && chaseTargetTransform.gameObject.activeInHierarchy)
				{
					chaseTargetLastPosition = chaseTargetTransform.position;
				}
				LockTargetRotate();
			}
			delayLaserTimer -= Time.deltaTime;
			if (delayLaserTimer <= 0f)
			{
				Enterstate(Elite60LaserState.Shooting);
			}
			break;
		case Elite60LaserState.Shooting:
			UpdateLaserVisualEffect();
			if (laserDuration > 0f)
			{
				damageCheckTimer += Time.deltaTime;
				if (damageCheckTimer >= damageCheckInterval)
				{
					damageCheckTimer = 0f;
					DamageCheck();
				}
				laserDuration -= Time.deltaTime;
				if (laserDuration <= 0f)
				{
					Enterstate(Elite60LaserState.AfterShoot);
				}
			}
			else
			{
				DamageCheck();
				Enterstate(Elite60LaserState.AfterShoot);
			}
			break;
		case Elite60LaserState.AfterShoot:
			UpdateLaserVisualEffect();
			break;
		case Elite60LaserState.None:
			break;
		}
	}

	private void UpdateLaserVisualEffect()
	{
		laserShootWidthChangeTimer += Time.deltaTime;
		float num = laserDurationTotal;
		if (num <= 0f)
		{
			num = Mathf.Max(laserShootWidthChargeTime * 2f, 0.1f);
		}
		float a = Mathf.Min(laserShootWidthChargeTime, num * 0.5f);
		a = Mathf.Max(a, 0.0001f);
		float value = 0f;
		if (laserShootWidthChangeTimer <= a)
		{
			value = laserShootWidthChangeTimer / a;
		}
		else if (laserShootWidthChangeTimer <= num - a)
		{
			value = 1f;
		}
		else if (laserShootWidthChangeTimer <= num)
		{
			value = (num - laserShootWidthChangeTimer) / a;
		}
		value = Mathf.Clamp01(value);
		LaserLineRenderer.startWidth = value * (laserWidth + 0.2f);
		LaserLineRenderer.SetPosition(0, laserOffset);
		LaserLineRenderer.SetPosition(1, currentDirection * laserLength + laserOffset);
		if (isShadow && ShadowLineRenderer != null)
		{
			ShadowLineRenderer.startWidth = value * (laserWidth + 0.2f);
			ShadowLineRenderer.SetPosition(0, Vector3.forward);
			ShadowLineRenderer.SetPosition(1, Vector3.forward + currentDirection * laserLength);
		}
	}

	private void LockTargetRotate()
	{
		if (!(chaseTargetLastPosition == default(Vector3)))
		{
			chaseTargetRotateInClockwise = Tool2D.IsClockWiseGapAngleSmaller(currentDirection, chaseTargetLastPosition - base.transform.position);
			Vector3 dir = Tool2D.IgnoreZV2ToV1Normal(chaseTargetLastPosition, base.transform.position);
			currentDirection = (chaseTargetRotateInClockwise ? Tool2D.DirMoveTowardsTargetInClockWiseSmoothLerp(currentDirection, dir, chaseTargetAngleSpeed * Time.deltaTime) : Tool2D.DirMoveTowardsTargetInCounterClockWiseSmoothLerp(currentDirection, dir, chaseTargetAngleSpeed * Time.deltaTime));
			WarningAreaObj.transform.right = currentDirection;
		}
	}

	private void Enterstate(Elite60LaserState newState)
	{
		currentState = newState;
		switch (newState)
		{
		case Elite60LaserState.PreShoot:
			if (delayLaserTimer > 0f)
			{
				WarningAreaObj.SetActive(value: true);
				chargeParticle.gameObject.SetActive(value: true);
				chargeParticle.Play(withChildren: true);
			}
			WarningAreaObj.transform.localScale = new Vector3(1f, 0f, 1f);
			DOTween.To(() => WarningAreaObj.transform.localScale, delegate(Vector3 x)
			{
				WarningAreaObj.transform.localScale = x;
			}, new Vector3(1f, 1f, 1f), 0.2f);
			if (!disableChargeSE)
			{
				SEMgr.Inst.boss52HDroneCharge.PlaySE(SEPlayMode.Replay, 3, chargeSEPlayInterval);
			}
			break;
		case Elite60LaserState.Shooting:
			if (!IsBoss52Dead || shootByOtherSource)
			{
				chargeParticle.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);
				shootParticle.gameObject.SetActive(value: true);
				shootParticle.Play(withChildren: true);
				DOTween.To(() => WarningAreaObj.transform.localScale, delegate(Vector3 x)
				{
					WarningAreaObj.transform.localScale = x;
				}, new Vector3(1f, 0f, 1f), 0.1f);
				laserShootWidthChangeTimer = 0f;
				damageCheckTimer = damageCheckInterval;
				if (!disableShootSE)
				{
					SEMgr.Inst.boss52HDroneShoot.PlaySE(SEPlayMode.Replay, 3, shootSEPlayInterval);
				}
			}
			break;
		case Elite60LaserState.AfterShoot:
			shootParticle.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);
			ObjPoolMgr.Inst.RecycleGO(base.gameObject, laserDelayDestroyTimer);
			break;
		case Elite60LaserState.None:
		case Elite60LaserState.Idle:
			break;
		}
	}

	public void ShootByOtherSource(Entity shooterEntity)
	{
		shootByOtherSource = true;
		this.shooterEntity = shooterEntity;
	}

	public void ForceUpdateCurrentDirection(Vector3 targetDirection)
	{
		currentDirection = targetDirection;
		WarningAreaObj.transform.right = currentDirection;
	}

	private void DamageCheck()
	{
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2097664u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitDotsSyncSystem.OverlapBox(base.transform.position + currentDirection * laserLength / 2f, new Vector3(laserLength / 2f, laserWidth / 2f, 1f), Quaternion.Euler(0f, 0f, Mathf.Atan2(currentDirection.y, currentDirection.x) * 57.29578f), filter, list);
		foreach (UnitDotsSyncSystem.DistanceHitResult item in list)
		{
			Entity entity = item.entity;
			Entity entity2 = shooterEntity;
			TakeDamageInfo_Dots takeDamageInfo_Dots = default(TakeDamageInfo_Dots);
			takeDamageInfo_Dots = ((!(entity2 != Entity.Null)) ? TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial) : TakeDamageInfo_Dots.NewInfo(entity2));
			takeDamageInfo_Dots.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(item.point, base.transform.position) * 3f;
			takeDamageInfo_Dots.damage = ((laserDamageIsDps && laserDuration > 0f) ? (laserDamage * damageCheckInterval) : laserDamage);
			UnitDotsSyncSystem.AddTakeDamageRequestEndless(entity, takeDamageInfo_Dots);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite60Hit", item.point).SetActive(value: true);
		}
	}
}
