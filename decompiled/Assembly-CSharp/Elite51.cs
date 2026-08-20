using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Entities;
using Unity.Physics.Stateful;
using UnityEngine;

public class Elite51 : UnitBase, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	public enum Elite51LockerState
	{
		FindingTarget,
		Locking,
		AfterLock
	}

	public enum Elite51State
	{
		BornIdle,
		MoveToTarget,
		DashExplosion,
		LockMissile
	}

	private Elite51State state;

	private Vector3 moveDir;

	public float RotateSpeed;

	public Transform RotateTransform;

	public float SkillCastBaseInterval;

	private float skillCastTimer;

	public float BornIdleDuration;

	private float idleTimer;

	public float CloseToTargetRange;

	public float CloseToTargetAngleSpeedRatio;

	public float CloseToTargetAngleSpeedLerpRatio;

	private float angleSpeedRatio = 1f;

	private UIEndlessEliteHpBar HpBar;

	public int ShootMissileRequirement;

	private int dashCastCounter;

	public List<ParticleSystem> DashChargeParticles;

	public List<ParticleSystem> DashParticles;

	public List<TrailRenderer> DashTrails;

	[Header("高度浮动相关")]
	public Transform BaseHeightTransform;

	public float NormalHeight;

	public float ShootingMissileHeight;

	public float HeightShiftTime;

	public float InitialHeight;

	[Header("轰炸冲锋")]
	public float DashSpeed;

	public float DashChargeTime;

	public float ExplosionRange;

	public float ExplosionDamage;

	public float DashShootInterval;

	private float dashShootTimer;

	public float DashTime;

	public float DashOutBorderStopRange;

	public float DashRotateSpeed;

	public float DashStopLockTime;

	private float dashChargeTimer;

	private float dashTimer;

	public LineRenderer DashWarningArea;

	public float WarningAreaMaxDistance;

	[Header("导弹锁定")]
	public float LockDuration;

	public float MissileDamage;

	public float MissileSpeed;

	public float MissileRange;

	public float MissileExistDuration;

	public float MissileShootTime;

	public float MissileAfterShootStartMoveDuration;

	public float MissileBeforeMoveSpeed;

	public float MissileStartMoveLerpSpeed;

	private float MissileTimer;

	public Transform MissileShootTransform;

	public float MissileShootBasePointShiftDistance;

	public float MissileChaseAngleSpeed;

	public float MissileCloseToTargetExplosionRangeRatio;

	public float MissilePreExplosionDuration;

	public float ShootMissileSkillDuration;

	public float MissileShootInterval;

	public int MissileShootCount;

	private int MissileShootCounter;

	[Header("锁定器参数")]
	private bool isStartLock;

	private Elite51_MissileLocker locker;

	private Vector3 lockerCurrentPosition;

	private Vector3 lockerMotion;

	public float LockerMoveLerpSpeed;

	public float LockerMaxMoveSpeed;

	public float LockerMinLockDuration;

	private float LockingTimer;

	public float EnterLockStateDistance;

	private Elite51LockerState LockerState;

	public float MinLockwarningDistance;

	public Entity thisEntity { get; set; }

	public override void SingleInitialCallback()
	{
		HpBar = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIEndlessEliteHpBar"), myPpt.tsf_Layer.position + new Vector3(0f, myPpt.unitCfg.relicShowHPUIHight - 0.2f, 0f) * myPpt.tsf_Layer.lossyScale.y, Quaternion.identity, myPpt.tsf_Layer).GetComponent<UIEndlessEliteHpBar>();
		HpBar.Initialize(this);
	}

	private void OnEnable()
	{
		foreach (TrailRenderer dashTrail in DashTrails)
		{
			dashTrail.Clear();
		}
	}

	private void OnDisable()
	{
		foreach (ParticleSystem dashParticle in DashParticles)
		{
			dashParticle.Stop();
		}
		foreach (ParticleSystem dashChargeParticle in DashChargeParticles)
		{
			dashChargeParticle.Stop();
		}
		foreach (TrailRenderer dashTrail in DashTrails)
		{
			dashTrail.emitting = false;
			dashTrail.Clear();
		}
		if (locker != null && locker.gameObject.activeInHierarchy)
		{
			locker.LockEnd();
		}
	}

	public override void EveryInitialCallback()
	{
		state = Elite51State.BornIdle;
		dashChargeTimer = 0f;
		dashTimer = 0f;
		skillCastTimer = 0f;
		DashWarningArea.transform.localScale = new Vector3(1f, 0f, 1f);
		idleTimer = 0f;
		dashShootTimer = 0f;
		dashCastCounter = 0;
		moveDir = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(base.transform.position, PlayerMgr.Inst.PlayerPoint), UnityEngine.Random.Range(-30f, 30f));
		HpBar.gameObject.SetActive(value: true);
		angleSpeedRatio = 1f;
		foreach (TrailRenderer dashTrail in DashTrails)
		{
			dashTrail.Clear();
			dashTrail.emitting = true;
		}
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		Vector3 to = Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position);
		switch (state)
		{
		case Elite51State.BornIdle:
			idleTimer += Time.deltaTime;
			if (idleTimer >= BornIdleDuration)
			{
				state = Elite51State.MoveToTarget;
			}
			break;
		case Elite51State.MoveToTarget:
			if (Tool2D.IgnoreZDistance(base.transform.position, PlayerMgr.Inst.PlayerPoint) <= CloseToTargetRange)
			{
				angleSpeedRatio = Mathf.Lerp(angleSpeedRatio, CloseToTargetAngleSpeedRatio, CloseToTargetAngleSpeedLerpRatio * Time.deltaTime);
			}
			else
			{
				angleSpeedRatio = Mathf.Lerp(angleSpeedRatio, 1f, CloseToTargetAngleSpeedLerpRatio * Time.deltaTime);
			}
			moveDir = Tool2D.RotateTowardsAroundZAxis(moveDir, to, RotateSpeed * Time.deltaTime * angleSpeedRatio);
			SetMove(base.MoveSpeed * moveDir, instantLerp: true);
			skillCastTimer += Time.deltaTime;
			if (skillCastTimer >= SkillCastBaseInterval)
			{
				skillCastTimer = 0f;
				CastSkill();
			}
			break;
		case Elite51State.DashExplosion:
			if (dashChargeTimer <= DashChargeTime)
			{
				if (dashChargeTimer == 0f)
				{
					foreach (ParticleSystem dashChargeParticle in DashChargeParticles)
					{
						dashChargeParticle.Play();
					}
					SEMgr.Inst.elite51DashCharge.PlaySE();
					BaseHeightTransform.DOLocalMove(new Vector3(0f, NormalHeight, 0f), HeightShiftTime);
				}
				dashChargeTimer += Time.deltaTime;
				DashWarningArea.startWidth = Mathf.Lerp(DashWarningArea.startWidth, 3.5f, 8f * Time.deltaTime);
				if (dashChargeTimer >= DashChargeTime)
				{
					foreach (ParticleSystem dashParticle in DashParticles)
					{
						dashParticle.Play();
					}
				}
				if (dashChargeTimer <= DashStopLockTime)
				{
					moveDir = Tool2D.RotateTowardsAroundZAxis(moveDir, to, DashRotateSpeed * Time.deltaTime);
				}
				SetMove(Vector3.zero);
				DashWarningArea.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position.IgnoreZ(), LayerCorrectType.GroundEffect));
				DashWarningArea.SetPosition(1, Tool2D.GetLayerPoint((base.transform.position + moveDir * (dashChargeTimer / DashChargeTime) * WarningAreaMaxDistance).IgnoreZ(), LayerCorrectType.GroundEffect));
				break;
			}
			DashWarningArea.startWidth = Mathf.Lerp(DashWarningArea.startWidth, 0f, 15f * Time.deltaTime);
			if (DashWarningArea.startWidth < 0.1f)
			{
				DashWarningArea.startWidth = 0f;
			}
			SetMove(moveDir * DashSpeed, instantLerp: true);
			dashShootTimer += Time.deltaTime;
			if (dashShootTimer >= DashShootInterval)
			{
				dashShootTimer -= DashShootInterval;
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite50_Cannon_Big", base.transform.position).GetComponent<Monster309_Cannon>().InitializeCannon(base.transform.position, base.transform.position + moveDir * 0.5f, 0.7f, myPpt.myEntity, buffed: false);
			}
			dashTimer += Time.deltaTime;
			if (!(dashTimer >= DashTime))
			{
				break;
			}
			state = Elite51State.MoveToTarget;
			skillCastTimer = -0.5f;
			foreach (ParticleSystem dashParticle2 in DashParticles)
			{
				dashParticle2.Stop();
			}
			break;
		case Elite51State.LockMissile:
			SetMove(Vector3.zero);
			moveDir = Tool2D.RotateTowardsAroundZAxis(moveDir, to, DashRotateSpeed * Time.deltaTime);
			if (!isStartLock)
			{
				isStartLock = true;
				locker = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite51_MissileLocker", base.transform.position).GetComponent<Elite51_MissileLocker>();
				lockerCurrentPosition = PlayerMgr.Inst.PlayerPoint + Tool2D.IgnoreZV2ToV1Normal(base.transform.position, PlayerMgr.Inst.PlayerPoint) * 5f;
				locker.UpdateTransform(lockerCurrentPosition);
				locker.LockStart();
				foreach (TrailRenderer dashTrail in DashTrails)
				{
					dashTrail.emitting = false;
				}
				BaseHeightTransform.DOLocalMove(new Vector3(0f, ShootingMissileHeight, 0f), HeightShiftTime).SetEase(Ease.InOutQuad);
			}
			MissileTimer += Time.deltaTime;
			if (MissileTimer >= MissileShootTime && MissileShootCounter < MissileShootCount)
			{
				MissileTimer -= MissileShootInterval;
				Vector3 dir = Tool2D.GetDir(moveDir, -90f);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite51_Missile", MissileShootTransform.position + dir * MissileShootBasePointShiftDistance).GetComponent<Elite51_Missile>().InitData(moveDir, dir, lockerCurrentPosition, MissileBeforeMoveSpeed, MissileSpeed, MissileStartMoveLerpSpeed, MissileAfterShootStartMoveDuration, MissileChaseAngleSpeed, MissileExistDuration, MissileDamage, MissileRange, MissilePreExplosionDuration, MissileCloseToTargetExplosionRangeRatio);
				dir = Tool2D.GetDir(moveDir, 90f);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite51_Missile", MissileShootTransform.position + dir * MissileShootBasePointShiftDistance).GetComponent<Elite51_Missile>().InitData(moveDir, dir, lockerCurrentPosition, MissileBeforeMoveSpeed + (float)MissileShootCounter * 0.5f, MissileSpeed, MissileStartMoveLerpSpeed, MissileAfterShootStartMoveDuration, MissileChaseAngleSpeed, MissileExistDuration, MissileDamage, MissileRange, MissilePreExplosionDuration, MissileCloseToTargetExplosionRangeRatio);
				SEMgr.Inst.elite51Launch.PlaySE();
				MissileShootCounter++;
			}
			if (MissileShootCounter >= MissileShootCount && MissileTimer >= 1f && locker != null)
			{
				locker.LockEnd();
				locker = null;
			}
			if (!(MissileTimer >= ShootMissileSkillDuration))
			{
				break;
			}
			state = Elite51State.MoveToTarget;
			skillCastTimer = -0.5f;
			foreach (TrailRenderer dashTrail2 in DashTrails)
			{
				dashTrail2.emitting = true;
			}
			BaseHeightTransform.DOLocalMove(new Vector3(0f, NormalHeight, 0f), HeightShiftTime);
			break;
		}
		RotateTransform.right = moveDir;
	}

	private void LateUpdate()
	{
		if (!(locker != null))
		{
			return;
		}
		float num = Tool2D.IgnoreZDistance(lockerCurrentPosition, PlayerMgr.Inst.PlayerPoint);
		switch (LockerState)
		{
		case Elite51LockerState.FindingTarget:
		{
			Vector3 b = Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, lockerCurrentPosition) * LockerMaxMoveSpeed;
			lockerMotion = Vector3.Lerp(lockerMotion, b, LockerMoveLerpSpeed * Time.deltaTime);
			lockerCurrentPosition += lockerMotion * Time.deltaTime;
			if (Tool2D.IgnoreZDistance(lockerCurrentPosition, PlayerMgr.Inst.PlayerPoint) <= MinLockwarningDistance / 2f)
			{
				LockerState = Elite51LockerState.Locking;
			}
			break;
		}
		case Elite51LockerState.Locking:
			lockerMotion = Vector3.Lerp(lockerMotion, Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, lockerCurrentPosition) * LockerMaxMoveSpeed, LockerMoveLerpSpeed * Time.deltaTime);
			lockerCurrentPosition += lockerMotion * Time.deltaTime;
			if (LockingTimer >= LockerMinLockDuration)
			{
				LockerState = Elite51LockerState.AfterLock;
				LockingTimer = 0f;
			}
			break;
		case Elite51LockerState.AfterLock:
			lockerMotion = Vector3.Lerp(lockerMotion, Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, lockerCurrentPosition) * LockerMaxMoveSpeed / 2f, LockerMoveLerpSpeed * Time.deltaTime);
			lockerCurrentPosition += lockerMotion * Time.deltaTime;
			LockingTimer += Time.deltaTime;
			if (LockingTimer >= 0.5f && Tool2D.IgnoreZDistance(lockerCurrentPosition, PlayerMgr.Inst.PlayerPoint) >= MinLockwarningDistance + 0.2f)
			{
				LockerState = Elite51LockerState.FindingTarget;
				LockingTimer = 0f;
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		locker.UpdateTransform(lockerCurrentPosition);
		float num2 = 0f;
		if (num <= EnterLockStateDistance)
		{
			num2 = 1f - num / EnterLockStateDistance;
		}
		locker.UpdateLockProgress(num2, num2 > 0f);
		Elite51_Missile.AngleChaseSpeedRatio = num2;
	}

	public void SetMove(Vector3 motion, bool instantLerp = false, float motionLerp = 0f)
	{
		float num = ((motionLerp > 0f) ? motionLerp : moveLerp);
		base.CurrentMotion = Tool2D.IgnoreZPoint(base.CurrentMotion);
		base.CurrentMotion = Vector3.Lerp(base.CurrentMotion, motion, instantLerp ? 1f : (num * Time.deltaTime));
	}

	private void CastSkill()
	{
		if (dashCastCounter < ShootMissileRequirement)
		{
			state = Elite51State.DashExplosion;
			dashTimer = 0f;
			dashChargeTimer = 0f;
			DashWarningArea.startWidth = 0f;
			dashShootTimer = DashShootInterval - 0.1f;
			dashCastCounter++;
		}
		else
		{
			state = Elite51State.LockMissile;
			MissileTimer = 0f;
			MissileShootCounter = 0;
			isStartLock = false;
			locker = null;
			lockerMotion = Vector3.zero;
			LockingTimer = 0f;
			LockerState = Elite51LockerState.FindingTarget;
			dashCastCounter = 0;
		}
	}

	public void OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
	}

	public void OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
	}

	public void OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}
}
