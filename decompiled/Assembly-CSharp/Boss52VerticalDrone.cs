using System.Collections.Generic;
using DG.Tweening;
using Unity.Entities;
using UnityEngine;

public class Boss52VerticalDrone : MonoBehaviour
{
	private struct CVRH_RotateDroneData
	{
		public float RotateSpeed;

		public float StopRotateTime;
	}

	private struct RRV_RotateDroneData
	{
		public float RotateSpeed;

		public float RadiusChangeSpeed;

		public float CurrentRadius;

		public Vector3 StartPosition;

		public bool BurstShoot;
	}

	private struct FSTF_DroneData
	{
		public float TargetSpeed;

		public float SpeedStartLerpTime;

		public float SpeedLerpTime;
	}

	private struct RCV_DroneData
	{
		public Transform TargetCoreTrasnform;

		public bool IsChaseTarget;

		public float RotateSpeed;

		public float CurrentRotateAngle;

		public float CurrentRadius;

		public float TargetRadius;

		public float RadiusChangeSpeed;

		public bool IsClockwiseRotate;
	}

	private struct RCRV_DroneData
	{
		public Transform TargetTransform;

		public float TargetRadius;

		public float CurrentRadiusRatio;

		public float RotateAngleSpeed;

		public float CurrentAngle;

		public float RadiusLerpDuration;

		public float StartMoveDelayTimer;
	}

	private struct RRVLH_DroneData
	{
		public Transform TargetTransform;

		public float TargetRadius;

		public float CurrentRadiusRatio;

		public float RotateAngleSpeed;

		public float CurrentAngle;

		public float RadiusLerpDuration;

		public float StartMoveDelayTimer;
	}

	private struct DKCV_DroneData
	{
		public float MoveWaitTimer;

		public float MoveLerpDuration;

		public float MoveSpeedRatio;
	}

	private struct DBV_DroneData
	{
		public float BonusSpeed;

		public float BonusSpeedDecayTime;

		public float MoveWaitTimer;

		public float MoveLerpDuration;

		public float MoveSpeedRatio;
	}

	private struct SVWH_DroneData
	{
		public float AngleSpeed;
	}

	private bool isDroneLaserActive;

	private float droneDuration;

	private float moveSpeed;

	private bool isDroneFinish;

	private float hitDamage;

	public float LaserDamageInterval;

	private float laserDamageTimer;

	private float damageRange;

	public Transform GroundDamageAreaEffectTransform;

	public TrailRenderer GroundDamageAreaTrail;

	public TrailRenderer GroundDamageAreaShadowTrail;

	private List<Vector3> groundDamageAreaRecordPointList = new List<Vector3>();

	public float GroundDamageAreaPointRecordInterval;

	private float groundDamageAreaRecordTimer;

	private float groundDamageAreaExistTimer;

	private float groundDamageAreaDamage;

	private float groundDamageAreaDamageRange;

	private bool hasGroundDamageEffect;

	private int groundNodeListMaxCapability;

	public float GroundDamageAreaDamageInterval;

	public float GroundDamageAreaDamageTimer;

	public LineRenderer LaserRenderer;

	private float laserWidthRatio;

	private float laserMaxWidth;

	private bool autoEndRecycle;

	public Transform LaserShootPointParticleTransform;

	public Transform DroneTransform;

	private VerticalLaserDroneMotion motionType;

	public Transform GroundRangeEffect;

	public Transform GroundHitEffect;

	public LoopAudioController LoopAudioController;

	private CVRH_RotateDroneData CVRH_Data;

	private RRV_RotateDroneData RRV_Data;

	private FSTF_DroneData FSTF_Data;

	private RCV_DroneData RCV_Data;

	private RCRV_DroneData RCRV_Data;

	private RRVLH_DroneData RRVLH_Data;

	private DKCV_DroneData DKCV_Data;

	private DBV_DroneData DBV_Data;

	private SVWH_DroneData SVWH_Data;

	public static bool IsBoss52Dead;

	private bool shootByOtherSource;

	private Entity shooterEntity;

	[HideInInspector]
	public Vector3 currentMoveDirection { get; private set; }

	private bool isShooterValid
	{
		get
		{
			if (!shootByOtherSource)
			{
				return !IsBoss52Dead;
			}
			return shooterEntity != Entity.Null;
		}
	}

	private void OnEnable()
	{
		GroundHitEffect.gameObject.SetActive(value: false);
		GroundRangeEffect.gameObject.SetActive(value: false);
		LaserShootPointParticleTransform.gameObject.SetActive(value: false);
		GroundDamageAreaEffectTransform.gameObject.SetActive(value: false);
		DroneTransform.gameObject.SetActive(value: true);
		isDroneLaserActive = false;
		hasGroundDamageEffect = false;
		isDroneFinish = false;
		shootByOtherSource = false;
		shooterEntity = Entity.Null;
		GroundDamageAreaDamageTimer = 0f;
		groundDamageAreaRecordTimer = 0f;
		laserDamageTimer = 0f;
		GroundDamageAreaTrail.Clear();
		GroundDamageAreaShadowTrail.Clear();
		groundDamageAreaRecordPointList.Clear();
		LoopAudioController.LoopAudio.Stop();
		CVRH_Data = default(CVRH_RotateDroneData);
		RRV_Data = default(RRV_RotateDroneData);
		FSTF_Data = default(FSTF_DroneData);
		RCV_Data = default(RCV_DroneData);
		RCRV_Data = default(RCRV_DroneData);
		RRVLH_Data = default(RRVLH_DroneData);
		DKCV_Data = default(DKCV_DroneData);
		DBV_Data = default(DBV_DroneData);
		SVWH_Data = default(SVWH_DroneData);
	}

	private void OnDisable()
	{
		LaserRenderer.startWidth = 0f;
		laserDamageTimer = 0f;
		groundDamageAreaRecordPointList.Clear();
		isDroneLaserActive = false;
		hasGroundDamageEffect = false;
	}

	public void InitDroneData(float damage, float damageRange, float duration, float speed, Vector3 initialDir, float droneHeight = 2.5f, float groundDamageAreaExistTimer = 0f, float groundDamageAreaDamage = 0f, float groundDamageAreaRange = 0f, float laserWidth = 0.1f, float initialHeight = 0.2f, float initialHeightShiftTime = 0.6f, bool autoEndRecycle = true, VerticalLaserDroneMotion motionType = VerticalLaserDroneMotion.normal, bool playSE = true)
	{
		hitDamage = damage;
		this.damageRange = damageRange;
		moveSpeed = speed;
		droneDuration = duration;
		currentMoveDirection = initialDir.normalized;
		laserMaxWidth = laserWidth;
		this.autoEndRecycle = autoEndRecycle;
		this.motionType = motionType;
		GroundRangeEffect.transform.localScale = Vector3.one * (damageRange + 0.3f);
		this.groundDamageAreaExistTimer = groundDamageAreaExistTimer;
		this.groundDamageAreaDamage = groundDamageAreaDamage;
		groundDamageAreaDamageRange = groundDamageAreaRange;
		hasGroundDamageEffect = groundDamageAreaExistTimer > 0f && groundDamageAreaRange > 0f;
		GroundDamageAreaTrail.widthMultiplier = groundDamageAreaRange + 0.3f;
		GroundDamageAreaTrail.time = groundDamageAreaExistTimer;
		GroundDamageAreaShadowTrail.widthMultiplier = groundDamageAreaRange + 1f;
		GroundDamageAreaShadowTrail.time = groundDamageAreaExistTimer + 0.3f;
		groundNodeListMaxCapability = Mathf.FloorToInt(groundDamageAreaExistTimer / GroundDamageAreaPointRecordInterval);
		LoopAudioController.LoopAudio.gameObject.SetActive(playSE);
		GroundDamageAreaDamageTimer = 0f;
		groundDamageAreaRecordTimer = 0f;
		laserDamageTimer = 0f;
		GroundDamageAreaTrail.Clear();
		GroundDamageAreaShadowTrail.Clear();
		DroneOutInitHeight(initialHeight, droneHeight, initialHeightShiftTime);
	}

	private void DroneOutInitHeight(float initHeight, float targetHeight, float lerpTime, bool startLaserAfterDone = true)
	{
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f - initHeight);
		base.transform.DOLocalMoveZ(0f - targetHeight, lerpTime).onComplete = delegate
		{
			isDroneLaserActive = startLaserAfterDone || isDroneLaserActive;
			if (isDroneLaserActive)
			{
				GroundDamageAreaTrail.transform.position = Tool2D.IgnoreZPoint(GroundHitEffect, 1.05f);
				GroundDamageAreaShadowTrail.transform.position = Tool2D.IgnoreZPoint(GroundHitEffect, 1.0699999f);
				GroundHitEffect.transform.position = Tool2D.IgnoreZPoint(GroundHitEffect, 1.05f);
				GroundRangeEffect.transform.position = Tool2D.IgnoreZPoint(GroundRangeEffect, 1.05f);
				GroundHitEffect.gameObject.SetActive(value: true);
				GroundRangeEffect.gameObject.SetActive(value: true);
				if (LoopAudioController.LoopAudio.gameObject.activeInHierarchy)
				{
					LoopAudioController.LoopAudio.Play();
				}
				if (hasGroundDamageEffect)
				{
					GroundDamageAreaEffectTransform.gameObject.SetActive(value: true);
				}
			}
		};
	}

	private void Update()
	{
		UpdateDroneTimer();
		UpdateDroneMoveEffect();
		UpdateDroneLaserWidth();
		TryAttackTargetsInRange();
		UpdataGroundDamageAreaPoint();
		UpdateGroundDamageAreaState();
	}

	private void UpdateGroundDamageAreaState()
	{
		if (!hasGroundDamageEffect || groundDamageAreaRecordPointList.Count <= 0 || !isShooterValid)
		{
			return;
		}
		GroundDamageAreaDamageTimer += Time.deltaTime;
		if (GroundDamageAreaDamageTimer < GroundDamageAreaDamageInterval)
		{
			return;
		}
		GroundDamageAreaDamageTimer -= GroundDamageAreaDamageInterval;
		if (groundDamageAreaRecordPointList.Count == 1)
		{
			List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
			UnitDotsSyncSystem.GetCollidersInRange(base.transform.position.IgnoreZ(), groundDamageAreaDamageRange, GameConst.Filter_MonsterAoeNoSpell, list);
			for (int i = 0; i < list.Count; i++)
			{
				UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
				info.damage = groundDamageAreaDamage;
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(distanceHitResult.entity, info);
			}
			return;
		}
		for (int j = 0; j < groundDamageAreaRecordPointList.Count - 1; j++)
		{
			UnitDotsSyncSystem.RayCastHitResult[] array = UnitDotsSyncSystem.SphereCastAll(groundDamageAreaRecordPointList[j], groundDamageAreaRecordPointList[j + 1] - groundDamageAreaRecordPointList[j], groundDamageAreaDamageRange / 2f, (groundDamageAreaRecordPointList[j + 1] - groundDamageAreaRecordPointList[j]).magnitude + 0.1f, GameConst.Filter_Laser);
			List<Entity> list2 = new List<Entity>();
			for (int k = 0; k < array.Length; k++)
			{
				UnitDotsSyncSystem.RayCastHitResult rayCastHitResult = array[k];
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(rayCastHitResult.entity))
				{
					if (!list2.Contains(rayCastHitResult.entity))
					{
						list2.Add(rayCastHitResult.entity);
						TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
						info2.damage = groundDamageAreaDamage;
						UnitDotsSyncSystem.AddTakeDamageRequestEndless(rayCastHitResult.entity, info2);
					}
				}
				else
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster54_Hit_2", array[k].point, 3f);
				}
			}
		}
	}

	private void UpdataGroundDamageAreaPoint()
	{
		if (!hasGroundDamageEffect)
		{
			return;
		}
		groundDamageAreaRecordTimer += Time.deltaTime;
		if (!(groundDamageAreaRecordTimer < GroundDamageAreaPointRecordInterval))
		{
			groundDamageAreaRecordTimer -= GroundDamageAreaPointRecordInterval;
			if (droneDuration > 0f)
			{
				groundDamageAreaRecordPointList.Add(base.transform.position.IgnoreZ());
			}
			if ((groundDamageAreaRecordPointList.Count >= groundNodeListMaxCapability || droneDuration <= 0f) && groundDamageAreaRecordPointList.Count > 0)
			{
				groundDamageAreaRecordPointList.RemoveAt(0);
			}
		}
	}

	private void TryAttackTargetsInRange()
	{
		if (!isDroneLaserActive || !isShooterValid)
		{
			return;
		}
		laserDamageTimer -= Time.deltaTime;
		if (!(laserDamageTimer > 0f))
		{
			laserDamageTimer += LaserDamageInterval;
			List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
			UnitDotsSyncSystem.GetCollidersInRange(base.transform.position.IgnoreZ(), damageRange, GameConst.Filter_MonsterAoeNoSpell, list);
			for (int i = 0; i < list.Count; i++)
			{
				UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
				info.damage = hitDamage;
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(distanceHitResult.entity, info);
			}
		}
	}

	private void UpdateDroneMoveEffect()
	{
		if (isDroneFinish)
		{
			return;
		}
		switch (motionType)
		{
		case VerticalLaserDroneMotion.RRV_Rotate:
		{
			float z3 = base.transform.position.z;
			RRV_Data.CurrentRadius -= RRV_Data.RadiusChangeSpeed * Time.deltaTime;
			currentMoveDirection = Tool2D.GetDir(currentMoveDirection, RRV_Data.RotateSpeed * Time.deltaTime);
			base.transform.position = RRV_Data.StartPosition.IgnoreZ() + new Vector3(0f, 0f, z3) + currentMoveDirection * RRV_Data.CurrentRadius;
			break;
		}
		case VerticalLaserDroneMotion.FSTF_Move:
			if (FSTF_Data.SpeedStartLerpTime > 0f)
			{
				FSTF_Data.SpeedStartLerpTime -= Time.deltaTime;
				if (FSTF_Data.SpeedStartLerpTime <= 0f)
				{
					DOTween.To(() => moveSpeed, delegate(float x)
					{
						moveSpeed = x;
					}, FSTF_Data.TargetSpeed, FSTF_Data.SpeedLerpTime);
				}
			}
			base.transform.position += currentMoveDirection * moveSpeed * Time.deltaTime;
			break;
		case VerticalLaserDroneMotion.CVRH_Rotate:
			CVRH_Data.StopRotateTime -= Time.deltaTime;
			if (CVRH_Data.StopRotateTime > 0f)
			{
				currentMoveDirection = Tool2D.GetDir(currentMoveDirection, CVRH_Data.RotateSpeed * Time.deltaTime);
			}
			base.transform.position += currentMoveDirection * moveSpeed * Time.deltaTime;
			break;
		case VerticalLaserDroneMotion.RCV_Rotate:
			if (RCV_Data.IsChaseTarget && RCV_Data.TargetCoreTrasnform != null && RCV_Data.TargetCoreTrasnform.gameObject.activeInHierarchy)
			{
				RCV_Data.CurrentRadius = Mathf.Clamp(RCV_Data.CurrentRadius + RCV_Data.RadiusChangeSpeed * Time.deltaTime, 0f, RCV_Data.TargetRadius);
				RCV_Data.CurrentRotateAngle += RCV_Data.RotateSpeed * Time.deltaTime * (float)(RCV_Data.IsClockwiseRotate ? 1 : (-1));
				base.transform.position = RCV_Data.TargetCoreTrasnform.position.IgnoreZ() + new Vector3(0f, 0f, base.transform.position.z) + Tool2D.GetDir(RCV_Data.CurrentRotateAngle) * RCV_Data.CurrentRadius;
			}
			else
			{
				RCV_Data.IsChaseTarget = false;
			}
			break;
		case VerticalLaserDroneMotion.RCRV_Rotate:
		{
			float z2 = base.transform.position.z;
			float num2 = RCRV_Data.TargetRadius * RCRV_Data.CurrentRadiusRatio;
			RCRV_Data.StartMoveDelayTimer -= Time.deltaTime;
			if (RCRV_Data.TargetTransform != null && RCRV_Data.TargetTransform.gameObject.activeInHierarchy && droneDuration > 0f)
			{
				base.transform.position = (RCRV_Data.TargetTransform.position + Tool2D.GetDir(RCRV_Data.CurrentAngle) * num2).IgnoreZ() + new Vector3(0f, 0f, z2);
			}
			if (RCRV_Data.StartMoveDelayTimer <= 0f)
			{
				RCRV_Data.CurrentAngle += RCRV_Data.RotateAngleSpeed * Time.deltaTime;
			}
			break;
		}
		case VerticalLaserDroneMotion.RRVLH_Rotate:
		{
			float z = base.transform.position.z;
			float num = RRVLH_Data.TargetRadius * RRVLH_Data.CurrentRadiusRatio;
			RRVLH_Data.StartMoveDelayTimer -= Time.deltaTime;
			if (RRVLH_Data.TargetTransform != null && RRVLH_Data.TargetTransform.gameObject.activeInHierarchy && droneDuration > 0f)
			{
				base.transform.position = (RRVLH_Data.TargetTransform.position + Tool2D.GetDir(RRVLH_Data.CurrentAngle) * num).IgnoreZ() + new Vector3(0f, 0f, z);
			}
			if (RRVLH_Data.StartMoveDelayTimer <= 0f)
			{
				RRVLH_Data.CurrentAngle += RRVLH_Data.RotateAngleSpeed * Time.deltaTime;
			}
			break;
		}
		case VerticalLaserDroneMotion.DKCV_Move:
			if (DKCV_Data.MoveWaitTimer > 0f)
			{
				DKCV_Data.MoveWaitTimer -= Time.deltaTime;
				if (DKCV_Data.MoveWaitTimer <= 0f)
				{
					DOTween.To(() => DKCV_Data.MoveSpeedRatio, delegate(float x)
					{
						DKCV_Data.MoveSpeedRatio = x;
					}, 1f, DKCV_Data.MoveLerpDuration).SetEase(Ease.InSine);
				}
			}
			base.transform.position += currentMoveDirection * moveSpeed * DKCV_Data.MoveSpeedRatio * Time.deltaTime;
			break;
		case VerticalLaserDroneMotion.DBV_Move:
			if (DBV_Data.MoveWaitTimer > 0f)
			{
				DBV_Data.MoveWaitTimer -= Time.deltaTime;
				if (DBV_Data.MoveWaitTimer <= 0f)
				{
					DOTween.To(() => DBV_Data.MoveSpeedRatio, delegate(float x)
					{
						DBV_Data.MoveSpeedRatio = x;
					}, 1f, DBV_Data.MoveLerpDuration).SetEase(Ease.InSine);
					DOTween.To(() => DBV_Data.BonusSpeed, delegate(float x)
					{
						DBV_Data.BonusSpeed = x;
					}, 0f, DBV_Data.BonusSpeedDecayTime);
				}
			}
			base.transform.position += currentMoveDirection * (moveSpeed + DBV_Data.BonusSpeed) * DBV_Data.MoveSpeedRatio * Time.deltaTime;
			break;
		case VerticalLaserDroneMotion.SVWH_Rotate:
			currentMoveDirection = Tool2D.GetDir(currentMoveDirection, SVWH_Data.AngleSpeed * Time.deltaTime);
			base.transform.position += currentMoveDirection * moveSpeed * Time.deltaTime;
			break;
		default:
			base.transform.position += currentMoveDirection * moveSpeed * Time.deltaTime;
			break;
		}
	}

	private void UpdateDroneTimer()
	{
		if (isDroneLaserActive)
		{
			droneDuration -= Time.deltaTime;
			if ((droneDuration <= 0f && autoEndRecycle) || !isShooterValid)
			{
				GroundHitEffect.gameObject.SetActive(value: false);
				GroundRangeEffect.gameObject.SetActive(value: false);
				LaserShootPointParticleTransform.gameObject.SetActive(value: false);
				DroneTransform.gameObject.SetActive(value: false);
				isDroneLaserActive = false;
				isDroneFinish = true;
				LoopAudioController.LoopAudio.Stop();
				ObjPoolMgr.Inst.RecycleGO(base.gameObject, groundDamageAreaExistTimer);
			}
		}
	}

	private void UpdateDroneLaserWidth()
	{
		if (isDroneLaserActive)
		{
			laserWidthRatio = Mathf.Lerp(laserWidthRatio, 1f, 10f * Time.deltaTime);
		}
		else
		{
			laserWidthRatio = Mathf.Lerp(laserWidthRatio, 0f, 10f * Time.deltaTime);
		}
		LaserRenderer.startWidth = laserWidthRatio * laserMaxWidth;
		LaserRenderer.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position.IgnoreZ()) + Vector3.up * (DroneTransform.position.y - base.transform.position.y));
		LaserRenderer.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position.IgnoreZ()));
	}

	public void EndDroneAction()
	{
		droneDuration = 0f;
	}

	public void CVRH_InitData(float RotateSpeed, float StopRotateTime)
	{
		CVRH_Data = new CVRH_RotateDroneData
		{
			RotateSpeed = RotateSpeed,
			StopRotateTime = StopRotateTime
		};
	}

	public void RRV_InitData(float rotateSpeed, float radiusChangeSpeed, float radius, Vector3 startPos)
	{
		RRV_Data = new RRV_RotateDroneData
		{
			RotateSpeed = rotateSpeed,
			RadiusChangeSpeed = radiusChangeSpeed,
			CurrentRadius = radius,
			StartPosition = startPos,
			BurstShoot = false
		};
	}

	public void FSTF_InitData(float targetSpeed, float startLerpTime, float lerpTime)
	{
		FSTF_Data = new FSTF_DroneData
		{
			TargetSpeed = targetSpeed,
			SpeedStartLerpTime = startLerpTime,
			SpeedLerpTime = lerpTime
		};
	}

	public void RCV_InitData(Transform coreTransform, float rotateSpeed, float currentAngle, float targetRadius, float radiusChanseSpeed, bool isClockwiseRotate)
	{
		RCV_Data = new RCV_DroneData
		{
			TargetCoreTrasnform = coreTransform,
			RotateSpeed = rotateSpeed,
			CurrentRotateAngle = currentAngle,
			CurrentRadius = 0f,
			TargetRadius = targetRadius,
			RadiusChangeSpeed = radiusChanseSpeed,
			IsChaseTarget = true,
			IsClockwiseRotate = isClockwiseRotate
		};
	}

	public void RCRV_InitData(Transform followTargetTransform, float targetRadius, float radiusLerpDuration, float rotateAngleSpeed, float currentAngle, float StartMoveDelay, bool isClockwiseRotate)
	{
		RCRV_Data = new RCRV_DroneData
		{
			TargetTransform = followTargetTransform,
			TargetRadius = targetRadius,
			CurrentRadiusRatio = 0f,
			CurrentAngle = currentAngle,
			RotateAngleSpeed = rotateAngleSpeed * (float)(isClockwiseRotate ? 1 : (-1)),
			RadiusLerpDuration = radiusLerpDuration,
			StartMoveDelayTimer = StartMoveDelay
		};
		DOTween.To(() => RCRV_Data.CurrentRadiusRatio, delegate(float x)
		{
			RCRV_Data.CurrentRadiusRatio = x;
		}, 1f, radiusLerpDuration);
	}

	public void RRVLH_InitData(Transform followTargetTransform, float targetRadius, float radiusLerpDuration, float rotateAngleSpeed, float currentAngle, float StartMoveDelay, bool isClockwiseRotate)
	{
		RRVLH_Data = new RRVLH_DroneData
		{
			TargetTransform = followTargetTransform,
			TargetRadius = targetRadius,
			CurrentRadiusRatio = 0f,
			CurrentAngle = currentAngle,
			RotateAngleSpeed = rotateAngleSpeed * (float)(isClockwiseRotate ? 1 : (-1)),
			RadiusLerpDuration = radiusLerpDuration,
			StartMoveDelayTimer = StartMoveDelay
		};
		DOTween.To(() => RRVLH_Data.CurrentRadiusRatio, delegate(float x)
		{
			RRVLH_Data.CurrentRadiusRatio = x;
		}, 1f, radiusLerpDuration);
	}

	public void DKCV_InitData(float moveWaitDuration, float moveLerpDuration)
	{
		DKCV_Data = new DKCV_DroneData
		{
			MoveWaitTimer = moveWaitDuration,
			MoveLerpDuration = moveLerpDuration,
			MoveSpeedRatio = 0f
		};
	}

	public void DBV_InitData(float bonusSpeed, float bonusSpeedDecayTime, float moveWaitDuration, float moveLerpDuration)
	{
		DBV_Data = new DBV_DroneData
		{
			BonusSpeed = bonusSpeed,
			BonusSpeedDecayTime = bonusSpeedDecayTime,
			MoveWaitTimer = moveWaitDuration,
			MoveLerpDuration = moveLerpDuration,
			MoveSpeedRatio = 0f
		};
	}

	public void SVWH_InitData(float angleSpeed)
	{
		SVWH_Data = new SVWH_DroneData
		{
			AngleSpeed = angleSpeed
		};
	}

	public void ShootByOtherSource(Entity shooterEntity)
	{
		shootByOtherSource = true;
		this.shooterEntity = shooterEntity;
	}
}
