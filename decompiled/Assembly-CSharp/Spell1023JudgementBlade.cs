using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Spell1023JudgementBlade : SpellBase, IOnLaunchFromSpellEventHandle, IOnSpellHijack
{
	[Serializable]
	public class LogicParameter
	{
		[Tooltip("\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd")]
		public Transform bladeCenter;

		[Tooltip("\ufffd\ufffd\ufffd\ufffdײ\ufffd\ufffd\ufffd\ufffd")]
		public BoxCollider hitbox;

		[Tooltip("\ufffd\ufffd\ufffd浯\ufffd\ufffd\ufffd\ufffd \ufffd\ufffd\ufffdл\ufffd\ufffd\ufffdʩ\ufffd\ufffd\ufffdߵĻ\ufffd\ufffd\ufffd\ufffd뾶")]
		public float normalMovementRotateRadiu;

		[Tooltip("\ufffd\ufffd\ufffd浯\ufffd\ufffd\ufffd\ufffd \ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\u0670ѽ\ufffd\ufffd\ufffdʼ\ufffd\ufffd\ufffdŰ뾶")]
		public float normalMovementIncreaseRotateRadiuThreshold;

		[Tooltip("\ufffd\ufffd\ufffd浯\ufffd\ufffd\ufffd\ufffd \ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\u05b5ʱÿ\ufffdѽ\ufffd\ufffd\ufffd\ufffdŵİ뾶\ufffd\ufffdС")]
		public float normalMovementIncreaseRotateRadiuPerBlade;

		[Tooltip("\ufffd\ufffd\ufffd浰\ufffd\ufffd\ufffd\ufffd \ufffd\ufffd\ufffdл\ufffd\ufffd\ufffdʩ\ufffd\ufffd\ufffdߵ\ufffd\ufffd\ufffdת\ufffdٶ\ufffd")]
		public float normalMovementRotateSpeed;

		[Tooltip("\ufffd\ufffd\ufffd浯\ufffd\ufffd\ufffd\ufffd \ufffd\ufffd\ufffd\ufffd\ufffd\ufffdʩ\ufffd\ufffd\ufffd\ufffd\ufffdƶ\ufffd\ufffdĲ\ufffd\u05b5\ufffdٶ\ufffd")]
		public float normalMovementChaseOwnerLerpSpeed;

		[Tooltip("\ufffd\ufffd\ufffd\ufffd״\u032c\ufffd\ufffd \ufffd\ufffd\ufffd\ufffd\ufffd\ufffdת\ufffdǶ\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\u02f5\ufffdʱ\ufffd\ufffd")]
		public float lockRotateDuration;

		[Tooltip("\ufffd\ufffd\ufffd\ufffd״\u032c\ufffd\ufffd \ufffd\ufffd\ufffd\ufffd\ufffd\ufffdת\ufffdǶ\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\u02f2\ufffd\u05b5\ufffdٶ\ufffd")]
		public float lockRotateLerpSpeed;

		[Tooltip("\ufffd\ufffd\ufffd\ufffd״\u032c\ufffd\ufffd \ufffd\ufffd\ufffdл\ufffd\ufffd\ufffd\ufffd\ufffdʱ\ufffd\ufffd")]
		public float lockPullBackDuration;

		[Tooltip("\ufffd\ufffd\ufffd\ufffd״\u032c\ufffd\ufffd \ufffd\ufffd\ufffdл\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd")]
		public AnimationCurve lockPullBackCurve;

		[Tooltip("\ufffd\ufffd\ufffd\ufffd״\u032c\ufffd\ufffd \ufffd\ufffd\ufffdл\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd")]
		public float lockPullBackDistance;

		[Tooltip("\ufffd\ufffd\ufffd\ufffd״\u032c\ufffd\ufffd \ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd״\u032c\ufffd\ufffd\ufffd\ufffd\ufffdʱ\ufffd\ufffd")]
		public float spawnDuration;

		[Tooltip("\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffdʱ\ufffd\ufffd\ufffd\ufffd\ufffdƫ\ufffdƽǶ\ufffd")]
		public Vector2 randomSpawnAngleShift;

		[Tooltip("\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffdʱ\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffdľ\ufffd\ufffd뱶\ufffd\ufffd")]
		public Vector2 randomSpawnDisRatio;

		[Tooltip("\ufffd\ufffd\ufffdҵ\ufffd\ufffd\ufffd\ufffd\u02fa\ufffdĶ\ufffd\ufffd\ufffd\ufffd\ufffd\ufffdʱ\ufffd\ufffd \ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\u05bd\ufffd\ufffd\ufffd\ufffd\ufffdת/\u05fc\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd/\ufffdշ\ufffd\ufffd\ufffd\ufffdڼ佣\ufffdͱ\ufffd\ufffd\ufffd\ufffdյ\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd")]
		public float afterDetectTargetBonusLifeTime;

		[Tooltip("\ufffd\ufffd\ufffd\ufffd\ufffd\ufffdʱ\ufffdľ\ufffd\ufffd\ufffd")]
		public Vector2 bladeSplitRadiuRatio;

		[Tooltip("\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffdλ\ufffd\ufffdƫ\ufffd\ufffd")]
		public Vector3 bladePosShift;

		[Tooltip("\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\u00b8\ufffd\ufffd\ufffd\ufffdٶ\ufffd")]
		public float bladeFloatSpeed;

		[Tooltip("\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\u00b8\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd")]
		public float bladeFloatDistance;

		[Tooltip("\ufffd\ufffd\ufffd\ufffd\u05fc\ufffd\ufffd\ufffd\ufffd\ufffd\ufffdʱҪ\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffdʵ\ufffdʸ߶\ufffd")]
		public Vector3 bladeBeforeShoot;
	}

	public enum BladeState
	{
		Spawn,
		DetectingTarget,
		LockingTarget,
		AfterShoot
	}

	private static readonly Dictionary<UnitProperty, List<Spell1023JudgementBlade>> _ownerData = new Dictionary<UnitProperty, List<Spell1023JudgementBlade>>();

	public LogicParameter param = new LogicParameter();

	private bool lockRotateInClockWise = true;

	private float lockDurationTimer;

	private Transform targetTransform;

	private Vector3 targetPosition = Vector3.zero;

	private float detectRange;

	private Vector3 stopPoint = Vector3.zero;

	private bool inQuery;

	private bool shootFromTrigger;

	private float ignoreWallDistance;

	private float lockRotateLerpSpeed;

	private float halfLifeDuration;

	private float halfLifeTimer;

	public float rotationRadiuShift;

	public float RedetectInterval;

	private float redetectTimer;

	public int OverSizeImmidiateLockThreshold;

	private List<Spell1023JudgementBlade> OwnerDataList
	{
		get
		{
			if (!_ownerData.ContainsKey(ownerPpt))
			{
				_ownerData.Add(ownerPpt, new List<Spell1023JudgementBlade>());
			}
			return _ownerData[ownerPpt];
		}
	}

	public BladeState currentState { get; private set; }

	protected override float FallInitialHeight => 4f;

	protected override float FallingReboundForce => base.FallingReboundForce * 0.6f;

	protected override float InFallingReboundingGravity => base.InFallingReboundingGravity * 3f;

	public override void InitializeCallback()
	{
		if (base.SIP.spellIsFall)
		{
			FallBladeInitialize();
		}
		else
		{
			BladeInitialize();
		}
		redetectTimer = 0f;
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			base.spellAroundOwnerRadius += UnityEngine.Random.Range(0f - rotationRadiuShift, rotationRadiuShift);
		}
	}

	public override void Update()
	{
		base.Update();
		switch (currentState)
		{
		case BladeState.Spawn:
			DetectEnemyOrTargetPointInRange();
			RotateAroundOwner();
			IdleFLoat();
			if (base.DurationTimer >= param.spawnDuration)
			{
				currentState = BladeState.DetectingTarget;
			}
			break;
		case BladeState.DetectingTarget:
			DetectEnemyOrTargetPointInRange();
			RotateAroundOwner();
			IdleFLoat();
			break;
		case BladeState.LockingTarget:
			if (base.DurationTimer >= param.spawnDuration)
			{
				if (lockDurationTimer >= param.lockRotateDuration + param.lockPullBackDuration)
				{
					StartShoot();
				}
				LockTargetRotate();
				if (base.currentSpellMovement == SpellSpecialMovementType.ChaseMouse && base.SIP.spellIsFall)
				{
					StartShoot();
				}
			}
			break;
		}
		UpdateLockPoint();
		AfterShootUpdateIgnoreWallDistance();
		UpdateBladeAfterShootDirection();
		param.bladeCenter.right = Tool2D.IgnoreZPoint(base.Direction);
		param.hitbox.transform.right = Tool2D.IgnoreZPoint(base.Direction);
		RotateShadow();
		base.DurationTimer += Time.deltaTime * GetLowFPSTimeScale(5f);
		if (base.DurationTimer > base.spellCfg.duration)
		{
			if (!base.isFlyFinish)
			{
				base.isFlyFinish = true;
				rigid.linearVelocity = Vector3.zero;
			}
			if (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)
			{
				base.SpellHoverTimer += Time.deltaTime * GetLowFPSTimeScale();
			}
			else
			{
				PoolRecycle();
			}
		}
	}

	private void IdleFLoat()
	{
		if (!inQuery)
		{
			param.bladeCenter.transform.localPosition = param.bladePosShift + new Vector3(0f, MathF.Sin(base.DurationTimer * param.bladeFloatSpeed) * param.bladeFloatDistance, 0f);
		}
	}

	private void AfterShootUpdateIgnoreWallDistance()
	{
		if (currentState == BladeState.AfterShoot)
		{
			ignoreWallDistance -= base.CurrentSpeed * Time.deltaTime;
		}
	}

	private void BladeInitialize()
	{
		targetPosition = Vector3.zero;
		enableFollowTarget = false;
		enableFollowMouse = false;
		stopPoint = Vector3.zero;
		param.hitbox.enabled = false;
		sc_Rebound.enabled = false;
		rigid.linearVelocity = Vector3.zero;
		currentState = BladeState.Spawn;
		lockDurationTimer = 0f;
		detectRange = base.spellCfg.float1 * Mathf.Max(1f, base.radiusRatio * base.finalRadiusRatio);
		if (!base.InitialParameter.equalScatter)
		{
			base.spellAroundOwnerCurrentAngle = UnityEngine.Random.Range(0f, 360f);
		}
		targetTransform = null;
		base.Direction = new Vector3(0f, -1f, 0f);
		param.bladeCenter.right = base.Direction;
		param.hitbox.transform.right = base.Direction;
		lockRotateInClockWise = true;
		lockRotateLerpSpeed = 0f;
		param.bladeCenter.localPosition = param.bladePosShift;
		ignoreWallDistance = param.hitbox.size.x * base.transform.localScale.x;
		inQuery = false;
		if (!base.spellCfg.isSplitSpell && !shootFromTrigger && !(base.InitialParameter.ShootCause is ShootCause.BySpell))
		{
			InsertBladeIntoOwnerBladesQuery();
			SetBladeInitialPosition();
		}
		DetectEnemyAroundSpawnPoint();
		ResetShadowRotate();
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			ShadowStartChangeShape();
		}
	}

	private void FallBladeInitialize()
	{
		targetPosition = Vector3.zero;
		enableFollowTarget = false;
		enableFollowMouse = false;
		stopPoint = Vector3.zero;
		param.hitbox.enabled = false;
		sc_Rebound.enabled = false;
		rigid.linearVelocity = Vector3.zero;
		currentState = BladeState.Spawn;
		lockDurationTimer = 0f;
		detectRange = base.spellCfg.float1 * Mathf.Max(1f, base.radiusRatio * base.finalRadiusRatio);
		if (!base.InitialParameter.equalScatter)
		{
			base.spellAroundOwnerCurrentAngle = UnityEngine.Random.Range(0f, 360f);
		}
		targetTransform = null;
		base.Direction = new Vector3(0f, -1f, 0f);
		param.bladeCenter.right = base.Direction;
		param.hitbox.transform.right = base.Direction;
		lockRotateInClockWise = true;
		lockRotateLerpSpeed = 0f;
		param.bladeCenter.localPosition = param.bladePosShift;
		base.CurrentUpSpeed = 0f;
		ignoreWallDistance = param.hitbox.size.x * base.transform.localScale.x;
		inQuery = false;
		if (!base.spellCfg.isSplitSpell && !shootFromTrigger)
		{
			_ = base.InitialParameter.ShootCause is ShootCause.BySpell;
		}
		if (!base.spellCfg.isSplitSpell && !shootFromTrigger && !(base.InitialParameter.ShootCause is ShootCause.BySpell))
		{
			_ = base.currentSpellMovement;
			_ = 3;
		}
		ResetShadowRotate();
	}

	protected override void UpdateSizeByDamageAndVolumeRatio()
	{
		if (SpellConfig.dic[base.spellCfg.id].damage != 0f)
		{
			float num = base.spellCfg.damage / base.level1Cfg.damage;
			float num2 = Mathf.Pow(num, 0.1f);
			if (Math.Abs(num - 1f) >= 0.01f)
			{
				base.transform.localScale = Vector3.one * num2 * base.spellVolumeRatio;
			}
			else
			{
				base.transform.localScale = Vector3.one * base.spellVolumeRatio;
			}
		}
	}

	protected override void UpdateUpSpeedWithFalling()
	{
		if (base.InFallRebounding && currentState == BladeState.AfterShoot)
		{
			base.CurrentUpSpeed -= Time.deltaTime * InFallingReboundingGravity;
		}
	}

	private void SetBladeInitialPosition()
	{
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			base.transform.position = Tool2D.IgnoreZPoint(GetAroundTargetBasePoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius);
			return;
		}
		float num = GetRotateAroundOwnerDistance() * UnityEngine.Random.Range(param.randomSpawnDisRatio.x, param.randomSpawnDisRatio.y);
		Vector3 vector = ((OwnerDataList.Count < 4) ? Tool2D.IgnoreZPoint(UnityEngine.Random.insideUnitSphere).normalized : Tool2D.IgnoreZPoint(Tool2D.GetDir(GetBladeRotateAroundOwnerDirection(), UnityEngine.Random.Range(param.randomSpawnAngleShift.x, param.randomSpawnAngleShift.y))));
		base.transform.position = Tool2D.GetNavMeshPointIngoreZ(base.transform.position + vector * num);
	}

	public void ShadowStartChangeShape()
	{
		((Spell1023Effect)EffectBase).ShadowStartChange();
	}

	public void InsertBladeIntoOwnerBladesQuery()
	{
		OwnerDataList.Insert(UnityEngine.Random.Range(0, OwnerDataList.Count), this);
		inQuery = true;
	}

	public void RemoveBladeFromOwnerBladesQuery()
	{
		if (OwnerDataList.Contains(this))
		{
			OwnerDataList.Remove(this);
		}
		inQuery = false;
	}

	private void UpdateBladeAfterShootDirection()
	{
		if (currentState == BladeState.AfterShoot)
		{
			base.Direction = rigid.linearVelocity.normalized;
		}
	}

	private void UpdateLockPoint()
	{
		if (currentState != BladeState.AfterShoot)
		{
			if (base.currentSpellMovement == SpellSpecialMovementType.ChaseMouse)
			{
				targetPosition = PlayerMgr.Inst.GetMousePoint(base.transform.position.z);
			}
			else if (targetTransform != null && targetTransform.gameObject.activeInHierarchy)
			{
				targetPosition = targetTransform.position;
			}
		}
	}

	private void RotateAroundOwner()
	{
		if (currentState == BladeState.DetectingTarget && base.currentSpellMovement != SpellSpecialMovementType.Rotation && inQuery)
		{
			Vector3 b = GetBladeAroundOwnerPoint() + GetBladeRotateAroundOwnerDirection() * GetRotateAroundOwnerDistance();
			base.transform.position = Vector3.Lerp(base.transform.position, b, param.normalMovementChaseOwnerLerpSpeed * PlayerMgr.Inst.PlayerDeltaTime);
		}
	}

	private float GetRotateAroundOwnerDistance()
	{
		return param.normalMovementRotateRadiu + Mathf.Max(0f, ((float)OwnerDataList.Count - param.normalMovementIncreaseRotateRadiuThreshold) * param.normalMovementIncreaseRotateRadiuPerBlade);
	}

	private Vector3 GetBladeRotateAroundOwnerDirection()
	{
		if (!inQuery)
		{
			return Vector3.zero;
		}
		return Tool2D.GetDir(Time.time * param.normalMovementRotateSpeed + 360f / (float)OwnerDataList.Count * (float)OwnerDataList.IndexOf(this));
	}

	private Vector3 GetBladeAroundOwnerPoint()
	{
		if (base.OwnerSpell != null)
		{
			if (isOwnerSpellValid())
			{
				base.OwnerPoint = base.OwnerSpell.transform.position;
				return base.OwnerSpell.transform.position;
			}
			return base.OwnerPoint;
		}
		return ownerPpt.GetSpellRotateFollowPoint();
	}

	private void DetectEnemyAroundSpawnPoint()
	{
		if (base.currentSpellMovement == SpellSpecialMovementType.ChaseMouse)
		{
			targetPosition = PlayerMgr.Inst.GetMousePoint(base.transform.position.z);
		}
		else
		{
			targetTransform = GetNearestEnemyTransformAroundTargetPoint(base.transform.position);
		}
		EnterLockState();
	}

	private void DetectEnemyOrTargetPointInRange()
	{
		redetectTimer += Time.deltaTime;
		if (!(redetectTimer <= RedetectInterval))
		{
			redetectTimer -= RedetectInterval;
			targetTransform = GetNearestEnemyTransformAroundTargetPoint(GetDetectCenterPoint());
			EnterLockState();
		}
	}

	private void EnterLockState()
	{
		if (!(targetTransform == null) || !(targetPosition == Vector3.zero))
		{
			if (targetTransform != null)
			{
				targetPosition = targetTransform.position;
			}
			stopPoint = base.transform.position;
			currentState = BladeState.LockingTarget;
			if (base.currentSpellMovement != SpellSpecialMovementType.Rotation)
			{
				ShadowStartChangeShape();
			}
			lockRotateInClockWise = targetPosition.x >= base.transform.position.x;
			ResetBladeRemainDuration();
			if (base.SIP.spellIsFall)
			{
				param.bladeCenter.transform.DOLocalMove(Vector3.zero, param.lockRotateDuration);
			}
			else
			{
				param.bladeCenter.transform.DOLocalMove(param.bladeBeforeShoot, param.lockRotateDuration);
			}
			float degree = Tool2D.GetDegree(base.Direction);
			float degree2 = Tool2D.GetDegree(Tool2D.IgnoreZV2ToV1Normal(targetPosition, base.transform.position));
			if (lockRotateInClockWise)
			{
				(float, float) tuple = default(Mathf).MoveTowardsAngleClockWiseReTurn2Angle(degree, degree2, param.lockRotateLerpSpeed * Time.deltaTime);
				lockRotateLerpSpeed = (tuple.Item1 + tuple.Item2) / 10f;
			}
			else
			{
				(float, float) tuple2 = default(Mathf).MoveTowardsAngleCounterClockWiseReTurn2Angle(degree, degree2, param.lockRotateLerpSpeed * Time.deltaTime);
				lockRotateLerpSpeed = (tuple2.Item1 + (360f - tuple2.Item2)) / 10f;
			}
			base.enableAroundPlayer = false;
			enableFollowTarget = false;
			enableFollowMouse = false;
		}
	}

	private Transform GetNearestEnemyTransformAroundTargetPoint(Vector3 point)
	{
		if (IsSameCamp(UnitType.Player))
		{
			float num = detectRange * detectRange;
			Transform result = null;
			for (int i = 0; i < LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts.Count; i++)
			{
				UnitProperty unitProperty = LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts[i];
				if (unitProperty.CanBeTarget)
				{
					float num2;
					if (unitProperty.CC_Self != null)
					{
						num2 = Mathf.Max(0f, Tool2D.IgnoreZDistance(point, unitProperty.transform.position + unitProperty.CC_Self.center) - unitProperty.CC_Self.radius);
						num2 *= num2;
					}
					else
					{
						num2 = Tool2D.IgnoreZDistanceSqr(point, unitProperty.transform.position);
					}
					if (num2 < num)
					{
						num = num2;
						result = unitProperty.transform;
					}
				}
			}
			return result;
		}
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(point, detectRange, "Player", "Teammate");
		GameObject minDistanceValidTarget = GetMinDistanceValidTarget(collidersByTag);
		if (minDistanceValidTarget == null)
		{
			return null;
		}
		return minDistanceValidTarget.transform;
	}

	private GameObject GetMinDistanceValidTarget(List<Collider> _hitTargets)
	{
		if (_hitTargets == null)
		{
			return null;
		}
		float num = float.PositiveInfinity;
		GameObject result = null;
		foreach (Collider _hitTarget in _hitTargets)
		{
			if (Tool2D.IgnoreZDistance(_hitTarget.transform.position, base.transform.position) <= num)
			{
				num = Tool2D.IgnoreZDistance(_hitTarget.transform.position, base.transform.position);
				result = _hitTarget.gameObject;
			}
		}
		return result;
	}

	private void ResetBladeRemainDuration()
	{
		if (!base.SIP.spellIsFall)
		{
			base.DurationTimer = base.spellCfg.duration - param.lockRotateDuration - param.lockPullBackDuration - param.afterDetectTargetBonusLifeTime;
			halfLifeTimer = 0f;
			halfLifeDuration = base.spellCfg.duration - base.DurationTimer;
			if (base.SIP.speedDecreaseRatio < 1f)
			{
				base.DurationTimer -= GeneralTool.GetSpellSpeedToBonusDuration(base.spellCfg.speed / base.SIP.speedDecreaseRatio, base.SIP.speedDecreaseRatio, base.SIP.speedDecreaseTransIntoDurationRatio);
			}
			if (base.DurationTimer < 0f)
			{
				base.spellCfg.duration -= base.DurationTimer;
				base.DurationTimer = 0f;
			}
		}
	}

	private Vector3 GetDetectCenterPoint()
	{
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation || base.spellCfg.isSplitSpell || base.InitialParameter.ShootCause is ShootCause.BySpell || base.SIP.spellIsFall)
		{
			return base.transform.position;
		}
		if (isOwnerSpellValid())
		{
			return base.OwnerSpell.transform.position;
		}
		if (base.currentSpellMovement != SpellSpecialMovementType.Rotation)
		{
			return ownerPpt.GetSpellRotateFollowPoint();
		}
		return base.transform.position;
	}

	public override void SpellAroundPlayer()
	{
		float num = 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / (base.CurrentSpeed / 2f)) * Time.deltaTime;
		base.spellAroundOwnerCurrentAngle += num;
		base.Direction = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle + 90f);
		Vector3 v = GetBladeAroundOwnerPoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
		base.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
	}

	protected override void SpellFollowMouse()
	{
		Vector3 mousePoint = PlayerMgr.Inst.GetMousePoint(base.transform.position.z);
		Vector3 linearVelocity = Vector3.Lerp(rigid.linearVelocity, ToPointDir(mousePoint) * base.CurrentSpeed, base.CurrentSpeed * Time.deltaTime * spellFollowMouseLerp);
		rigid.linearVelocity = linearVelocity;
		base.Direction = linearVelocity.normalized;
	}

	private void LockTargetRotate()
	{
		if (lockDurationTimer >= param.lockRotateDuration)
		{
			return;
		}
		Vector3 zero = Vector3.zero;
		if (base.SIP.spellIsFall)
		{
			base.Direction = Tool2D.IgnoreZV2ToV1Normal(targetPosition, base.transform.position);
			param.bladeCenter.right = base.Direction;
		}
		else
		{
			zero = Tool2D.IgnoreZV2ToV1Normal(targetPosition, base.transform.position);
			if (OwnerDataList.Count >= OverSizeImmidiateLockThreshold || base.spellCfg.isSplitSpell || GameMgr.Inst.GetFps() <= 20f)
			{
				base.Direction = zero;
			}
			else
			{
				base.Direction = (lockRotateInClockWise ? Tool2D.DirMoveTowardsTargetInClockWiseSmoothLerp(base.Direction, zero, lockRotateLerpSpeed * Time.deltaTime) : Tool2D.DirMoveTowardsTargetInCounterClockWiseSmoothLerp(base.Direction, zero, lockRotateLerpSpeed * Time.deltaTime));
			}
		}
		if (lockDurationTimer + Time.deltaTime >= param.lockRotateDuration && param.lockPullBackDuration > 0f)
		{
			LockTargetPullBack();
		}
		lockDurationTimer += Time.deltaTime;
	}

	public Vector3 GetFallingBladeToTargetDirection()
	{
		return Tool2D.IgnoreZV2ToV1Normal(targetPosition, base.transform.position + new Vector3(0f, base.Height, 0f));
	}

	private void RotateShadow()
	{
		if (currentState == BladeState.LockingTarget || currentState == BladeState.AfterShoot || base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			((Spell1023Effect)EffectBase).ShadowRotate(Tool2D.IgnoreZPoint(base.Direction));
		}
	}

	private void ResetShadowRotate()
	{
		((Spell1023Effect)EffectBase).ShadowRotate(Vector3.zero);
	}

	private void LockTargetPullBack()
	{
		base.transform.DOMove(stopPoint - base.Direction * param.lockPullBackDistance, param.lockPullBackDuration).SetEase(param.lockPullBackCurve);
	}

	protected override bool HalfLifeRandomTeleport()
	{
		if (currentState != BladeState.AfterShoot)
		{
			return false;
		}
		halfLifeTimer += Time.deltaTime;
		if (halfLifeTimer < halfLifeDuration / 2f)
		{
			return false;
		}
		bool num = base.HalfLifeRandomTeleport();
		if (num)
		{
			halfLifeDuration += 2f;
		}
		return num;
	}

	protected override void InitSpeedAndPosition()
	{
		if (!base.SIP.spellIsFall)
		{
			base.CurrentSpeed = base.spellCfg.speed;
			base.CurrentUpSpeed = base.spellCfg.upSpeed;
			return;
		}
		base.CurrentSpeed = base.spellCfg.speed;
		base.spellCfg.upSpeed = base.CurrentUpSpeed;
		base.Height = FallInitialHeight;
		if (base.SIP.finalShootSpatialInfo != null)
		{
			Vector3? target = base.SIP.finalShootSpatialInfo.Target;
			if (!target.HasValue)
			{
				Debug.LogWarning("\u05f9\ufffd䷨\ufffd\ufffdȴû\ufffdй\ufffd\ufffd\ufffdĿ\ufffd\ufffd\ufffd\ufffd\ufffd\ufffdϢ\ufffd\ufffd");
				return;
			}
			Vector3 direction = base.SIP.finalShootSpatialInfo.Direction;
			Vector3 vector = default(Vector3);
			vector = ((base.currentSpellMovement == SpellSpecialMovementType.ChaseMouse) ? (target.Value + -direction.normalized * (base.Height / GameConst.spellFallAngleTan)) : ((base.currentSpellMovement != SpellSpecialMovementType.Rotation) ? (target.Value + Tool2D.IgnoreZPoint(UnityEngine.Random.insideUnitSphere) * base._angle * 0.04f) : (GetBladeAroundOwnerPoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius)));
			Vector3 position = base.transform.position;
			position.x = vector.x;
			position.y = vector.y;
			base.transform.position = position;
		}
		if (base.currentSpellMovement != SpellSpecialMovementType.ChaseMouse || base.currentSpellMovement != SpellSpecialMovementType.Rotation)
		{
			base.CurrentUpSpeed = 0f;
		}
	}

	private void StartShoot()
	{
		enableFollowTarget = true;
		if (!base.SIP.spellIsFall)
		{
			param.hitbox.enabled = true;
		}
		if (base.rebounceTime > 0)
		{
			sc_Rebound.enabled = true;
		}
		rigid.linearVelocity = base.Direction * base.CurrentSpeed;
		Quaternion value = Quaternion.LookRotation(base.Direction) * Quaternion.Euler(0f, -90f, 0f);
		Quaternion value2 = Quaternion.LookRotation(GetFallingBladeToTargetDirection()) * Quaternion.Euler(0f, -90f, 0f);
		if (!base.SIP.spellIsFall)
		{
			SpellEffectBase effectBase = EffectBase;
			Quaternion? rotation = value;
			effectBase.CreateSpriteEffect("Shoot", null, rotation);
		}
		else
		{
			EffectBase.CreateSpriteEffect("Shoot", base.transform.position, value2);
		}
		EffectBase.ManualCreateEffect("Trail");
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			base.currentSpellMovement = SpellSpecialMovementType.Normal;
		}
		PlaySE("StartShoot");
		if (base.SIP.spellIsFall)
		{
			FallStartShoot();
		}
		currentState = BladeState.AfterShoot;
	}

	private void FallStartShoot()
	{
		float speed = base.spellCfg.speed;
		float num = Mathf.Sqrt(Mathf.Pow(Tool2D.IgnoreZDistance(base.transform.position, targetPosition), 2f) + Mathf.Pow(base.Height, 2f));
		float num2 = Mathf.Acos(base.Height / num) * 57.29578f;
		float currentSpeed = speed * Mathf.Sin(MathF.PI / 180f * num2);
		float num3 = speed * Mathf.Cos(MathF.PI / 180f * num2);
		base.CurrentSpeed = currentSpeed;
		rigid.linearVelocity = base.Direction * base.CurrentSpeed;
		base.CurrentUpSpeed = 0f - num3;
	}

	protected override void OnFallingGround()
	{
		MakeFallingGroundDamageToAround();
		OnFallingGroundTryReboundOrRecycle();
	}

	protected override void OnFallingGroundTryReboundOrRecycle()
	{
		if (!OnFallingGroundTryRebound())
		{
			base.isFlyFinish = true;
			base.CurrentSpeed *= 0.001f;
			base.CurrentUpSpeed *= 0.001f;
			rigid.linearVelocity = base.CurrentSpeed * base.Direction;
			base.DurationTimer = base.spellCfg.duration + 0.1f;
		}
	}

	public override void TriggerIn(Collider other)
	{
		UnitProperty component = other.GetComponent<UnitProperty>();
		switch (other.tag)
		{
		case "Player":
		case "Teammate":
			if (ownerPpt != null && ownerPpt.tag != "Player" && ownerPpt.tag != "Teammate")
			{
				OutputDamage(component);
				CreateHitEffectLookAt(base.Direction);
				PlaySE("Hit");
				TryRefractOrPenetrateOrRecycleOnHitTarget(other.gameObject);
			}
			break;
		case "Monster":
			if (ownerPpt != null && ownerPpt.gameObject.activeInHierarchy && ownerPpt.tag != "Monster")
			{
				OutputDamage(component);
				CreateHitEffectLookAt(base.Direction);
				PlaySE("Hit");
				TryRefractOrPenetrateOrRecycleOnHitTarget(other.gameObject);
			}
			break;
		case "Destructible":
			OutputDamage(other.gameObject);
			CreateHitEffectLookAt(base.Direction);
			PlaySE("Hit");
			TryRefractOrPenetrateOrRecycleOnHitTarget(other.gameObject);
			break;
		case "RollBall":
		{
			Spell1002RollBall componentInParent2 = other.GetComponentInParent<Spell1002RollBall>();
			if (!IsSameCamp(componentInParent2))
			{
				componentInParent2.TakeDamage(base.spellCfg.damage);
				TryRefractOrPenetrateOrRecycleOnHitTarget(other.gameObject);
			}
			break;
		}
		case "Butterfly":
		{
			Spell1003Butterfly componentInParent = other.GetComponentInParent<Spell1003Butterfly>();
			if (!IsSameCamp(componentInParent))
			{
				componentInParent.Break();
			}
			break;
		}
		case "Wall":
		case "SolidObj":
			if (!isThroughWall && base.currentSpellMovement != SpellSpecialMovementType.Rotation)
			{
				if (base.rebounceTime > 0)
				{
					base.rebounceTime--;
				}
				else if (!(ignoreWallDistance > 0f))
				{
					CreateHitEffectLookAt(base.Direction);
					HitEFAndRecycle();
				}
			}
			break;
		case "Brittleness":
			OutputDamage(other.gameObject, new TakeDamageInfo
			{
				isPlayHitSE = false
			});
			break;
		}
	}

	public void OnDisable()
	{
		if (inQuery)
		{
			RemoveBladeFromOwnerBladesQuery();
		}
	}

	protected override Vector3 GetSplitSpellPosition(Vector3 splitDirection)
	{
		float num = UnityEngine.Random.Range(param.bladeSplitRadiuRatio.x, param.bladeSplitRadiuRatio.y);
		return base.transform.position + splitDirection * num;
	}

	public override void PoolRecycle()
	{
		base.Direction = (inQuery ? GetBladeRotateAroundOwnerDirection() : Tool2D.IgnoreZPoint(UnityEngine.Random.insideUnitSphere).normalized);
		base.PoolRecycle();
		shootFromTrigger = false;
		RemoveBladeFromOwnerBladesQuery();
	}

	public void IOnLaunchFromSpellEventHandle(SpellBase ownerSpell, SlotData triggerOrNull)
	{
		shootFromTrigger = true;
	}

	public void IOnSpellHijack(UnitProperty targetPpt)
	{
		RemoveBladeFromOwnerBladesQuery();
		ownerPpt = targetPpt;
		base.DurationTimer = 0f;
		BladeInitialize();
	}
}
