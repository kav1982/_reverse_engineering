using System;
using System.Collections.Generic;
using UnityEngine;

public class Spell4019BiAnLethalBlade : SpellBase
{
	public enum FallBladePullBackState
	{
		FlyingUp,
		BackToOwner
	}

	public enum BladeState
	{
		Follow,
		Shoot,
		OnGround,
		FlyingBack
	}

	public static List<Spell4019BiAnLethalBlade> shootingBladesList = new List<Spell4019BiAnLethalBlade>();

	public static List<Spell4019BiAnLethalBlade> followOwnerBladesList = new List<Spell4019BiAnLethalBlade>();

	public static List<Spell4019BiAnLethalBlade> onGroundBladesList = new List<Spell4019BiAnLethalBlade>();

	public static List<Spell4019BiAnLethalBlade> PickingBladesList = new List<Spell4019BiAnLethalBlade>();

	private Transform followTransform;

	public Transform bladeFollowCenter;

	private AutoWandData autoWand;

	public float bladeGroupRadiu;

	public float bladeGroupHeightRatio;

	private Vector3 inBladeGroupPosition;

	public Vector3 lookUpBladeGroupBasePos;

	public Vector3 lookDownBladeGroupBasePos;

	public Vector3 lookLeftBladeGroupBasePos;

	public Vector3 lookRightBladeGroupBasePos;

	public Vector3 LUGroupBasePos;

	public Vector3 LDGroupBasePos;

	public Vector3 RUGroupBasePos;

	public Vector3 RDGroupBasePos;

	public float BladeFollowOwnerSpeedLerpRatio;

	public Collider HitBox;

	public float baseSpeed;

	public Vector3 bladeFollowHeightShift;

	public VariableFloat shootDefaultTime;

	private float currentShootTimer;

	private float currentShootDuration;

	public float baseGroundBladePickUpRange;

	private float groundBladePickUpRange;

	public float OutMapAutoPickUpTime;

	private float outMapAutoPickUpTimer;

	public float normalBackToNormalRange;

	public float longBackToFollowStateRange;

	public float chaseMouseSpeedRatio;

	public float AttackPullBackChaseMouseTime;

	private float attackPullBackChaseMouseTimer;

	private float attackPullBackChaseEnemyTimer;

	private float defaultChaseTargetAngleSpeed;

	public float AttackPullBackChaseTimerToAngleSpeedRatio;

	public float SpeedToRotateRadiuChangeRatio;

	private float defaultRotateRadiu;

	public float AttackPullBackNormalSpeedRatio;

	public float AttackPullBackChaseEnemySpeedRatio;

	public float AttackPullBackChaseMouseSpeedRatio;

	public float AttackPullBackRotationSpeedRatio;

	private bool isAttackPullBack;

	private SpellSpecialMovementType originMovement;

	public AnimationCurve AttackPullBackMouseLerpCurve;

	public AnimationCurve OnGroundShakeCurve;

	public VariableFloat OngroundBladeBaseAngle;

	public VariableFloat OngroundBladeShakeAngleRange;

	public VariableFloat OngroundBladeShakeDurationRange;

	private float OngroundBladeTargetAngle;

	private float OnGroundBladeShakeAngle;

	private float OngroundBladeLerpTotalTime;

	private float OngroundBladeLerpTimer;

	private Transform BladeFrontRotateTrans;

	private Transform BladeBackRotateTrans;

	private Vector3 lastFrameOwnerPosition;

	public float LRMoveMaxAngleShift;

	public float UMoveMaxAngleShift;

	public float OwnerMoveBladeAngleTinyShiftLerpSpeed;

	public float MoveMinAngleRatio;

	public float PlayerMoveNormalSpeed;

	public Transform ShadowTrans;

	public SpriteRenderer ShadowSprite;

	private static readonly int BladeShadowWIdthRatio = Shader.PropertyToID("_BladeShadowWIdthRatio");

	private static readonly int RotateAngle = Shader.PropertyToID("_RotateAngle");

	public Transform BladeHeightTrans;

	public Vector3 BladeShootHeight;

	public Vector3 BladeIdleHeight;

	public Vector3 BladeOnGroundHeight;

	public Vector3 NormalColliderSize;

	public Vector3 PullBackColliderSize;

	private int normalReboundTime;

	private float afterShootUnlimitedReboundTimer;

	public float afterShootUnlimitedReboundDuration;

	private float afterFallShootClearTrailTimer;

	private float fallPullBackTimer;

	private FallBladePullBackState fallbladePullState;

	public VariableFloat FallBladePullBackFlyingUpBaseSpeed;

	private float fallBladePullBackFlyingUpSpeed;

	public float FallBladePullBackWaitInterval;

	public float FallBladePullBackExtraIntervalPerBlade;

	public float FallBladePullBackToOwnerInitialHeight;

	public float FallBladePullBackToOwnerSpeed;

	private int initialHalfLifeTeleportCount;

	public BladeState currentState { get; private set; }

	protected override float FallInitialHeight => 12f;

	protected override float FallingReboundForce => base.FallingReboundForce * 0.7f;

	protected override float InFallingReboundingGravity => base.InFallingReboundingGravity * 2.5f;

	public override void InitializeCallback()
	{
		base.InitializeCallback();
		followTransform = null;
		autoWand = null;
		defaultChaseTargetAngleSpeed = spellFollowTargetRotateSpeed;
		defaultRotateRadiu = base.spellAroundOwnerRadius;
		EnterState(BladeState.Follow);
		groundBladePickUpRange = Mathf.Max(1f, baseGroundBladePickUpRange * base.radiusRatio * base.finalRadiusRatio);
		spellFollowMouseLerp *= chaseMouseSpeedRatio;
		originMovement = base.currentSpellMovement;
		ResetBladeInGroupPosition();
		BladeFrontRotateTrans = null;
		BladeBackRotateTrans = null;
		base.spellCfg.abilityType = SpellAbilityType.BiAnLethalBlade;
		base.CanBeCapture = false;
		normalReboundTime = base.rebounceTime;
		initialHalfLifeTeleportCount = halfLifeTeleportCount;
		enableNormalTransform = !base.SIP.spellIsFall;
		base.transform.localScale = Vector3.one * base.spellVolumeRatio;
		if (base.endThunderHitPercent > 0f && base.ColorType == SpellColorType.Player)
		{
			base.ColorType = SpellColorType.Thunder;
		}
	}

	private void ResetBladeInGroupPosition()
	{
		inBladeGroupPosition = Tool2D.IgnoreZPoint(UnityEngine.Random.insideUnitCircle) * bladeGroupRadiu;
		inBladeGroupPosition = new Vector3(inBladeGroupPosition.x, inBladeGroupPosition.y * bladeGroupHeightRatio, inBladeGroupPosition.z);
	}

	private void OnDisable()
	{
		RemoveBladeFromAllList();
	}

	private void UpdateIfOutMapState()
	{
		Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(base.transform.position, 8);
		if (outMapAutoPickUpTimer >= OutMapAutoPickUpTime)
		{
			outMapAutoPickUpTimer = 0f;
			NormalPickUp();
		}
		if ((navMeshPointIngoreZ - base.transform.position).sqrMagnitude > 0.2f)
		{
			outMapAutoPickUpTimer += Time.deltaTime;
		}
	}

	private void RemoveBladeFromAllList()
	{
		shootingBladesList.Remove(this);
		followOwnerBladesList.Remove(this);
		onGroundBladesList.Remove(this);
		PickingBladesList.Remove(this);
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		lastFrameOwnerPosition = GetOwnerBasePoint();
	}

	public override void Update()
	{
		base.Update();
		if (!BladeBackRotateTrans)
		{
			BladeBackRotateTrans = ((Spell4019Effect)EffectBase).GetBladeBackTrasnform();
		}
		if (!BladeFrontRotateTrans)
		{
			BladeFrontRotateTrans = ((Spell4019Effect)EffectBase).GetBladeFrontTrasnform();
		}
		switch (currentState)
		{
		case BladeState.Follow:
			UpdateBladeRotateAngle(idleLookDown: true);
			UpdateBladeFollowOwnerPositionV2();
			UpdateOwnerMoveTinyAngleShiftState();
			break;
		case BladeState.Shoot:
			if (!base.SIP.spellIsFall)
			{
				currentShootTimer -= Time.deltaTime;
			}
			if (currentShootTimer <= 0f)
			{
				EnterState(BladeState.OnGround);
			}
			break;
		case BladeState.OnGround:
			OnGroundCloseOwnerAutoPickUp();
			UpdateBladeShakeRotateAngle();
			break;
		case BladeState.FlyingBack:
			CheckFlyBackToOwnerDistance();
			LowFrameForcePullBack();
			break;
		}
		UpdateFallBladeAttackPullBackState();
		UpdateFallBladeAttackPullBackPos();
		UpdateBladeDirection();
		HitBox.transform.right = base.Direction;
		UpdateShadow();
		UpdateBladeSpritreTransHeight();
		afterShootUnlimitedReboundTimer -= Time.deltaTime;
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		if (!base.SIP.spellIsFall)
		{
			tsf_Layer.localPosition = new Vector3(0f, 0f, tsf_Layer.localPosition.z);
		}
		if (afterFallShootClearTrailTimer > 0f)
		{
			afterFallShootClearTrailTimer -= Time.deltaTime;
			if (afterFallShootClearTrailTimer < 0f)
			{
				((Spell4019Effect)EffectBase).TrailSwitch(toggle: true);
				((Spell4019Effect)EffectBase).TrailParticleToggle(toggle: true);
			}
		}
	}

	private void UpdateFallBladeAttackPullBackState()
	{
		if (!base.SIP.spellIsFall || currentState != BladeState.FlyingBack || !isAttackPullBack)
		{
			return;
		}
		fallPullBackTimer -= Time.deltaTime;
		switch (fallbladePullState)
		{
		case FallBladePullBackState.FlyingUp:
			if (fallPullBackTimer <= 0f)
			{
				fallbladePullState = FallBladePullBackState.BackToOwner;
				base.transform.position = base.transform.position.IgnoreZ() + new Vector3(0f, 0f, 0f - FallBladePullBackToOwnerInitialHeight);
				((Spell4019Effect)EffectBase).TrailSwitch(toggle: false);
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case FallBladePullBackState.BackToOwner:
			break;
		}
	}

	private void UpdateFallBladeAttackPullBackPos()
	{
		if (base.SIP.spellIsFall && currentState == BladeState.FlyingBack && isAttackPullBack)
		{
			switch (fallbladePullState)
			{
			case FallBladePullBackState.FlyingUp:
				base.transform.position += new Vector3(0f, 0f, fallBladePullBackFlyingUpSpeed * Time.deltaTime);
				break;
			case FallBladePullBackState.BackToOwner:
				base.transform.position = Tool2D.IgnoreZPoint(GetBladeInGroupToOwnerPosition()) + new Vector3(0f, 0f, base.transform.position.z + FallBladePullBackToOwnerSpeed * Time.deltaTime);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}

	private void UpdateBladeDirection()
	{
		if (base.SIP.spellIsFall)
		{
			switch (currentState)
			{
			case BladeState.Follow:
			case BladeState.OnGround:
				bladeFollowCenter.right = Tool2D.IgnoreZPoint(base.Direction);
				break;
			case BladeState.Shoot:
			{
				float z = Vector2.SignedAngle(to: new Vector2(base.Direction.x * base.CurrentSpeed, base.CurrentUpSpeed + base.Direction.y * base.CurrentSpeed), from: Vector2.right);
				Quaternion rotation = Quaternion.Euler(0f, 0f, z);
				bladeFollowCenter.rotation = rotation;
				break;
			}
			case BladeState.FlyingBack:
				if (isAttackPullBack)
				{
					switch (fallbladePullState)
					{
					case FallBladePullBackState.FlyingUp:
						bladeFollowCenter.right = new Vector3(0f, 90f, 0f);
						break;
					case FallBladePullBackState.BackToOwner:
						bladeFollowCenter.right = new Vector3(0f, -90f, 0f);
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
				else
				{
					bladeFollowCenter.right = Tool2D.IgnoreZPoint(base.Direction);
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		else
		{
			bladeFollowCenter.right = Tool2D.IgnoreZPoint(base.Direction);
		}
	}

	private void UpdateBladeSpritreTransHeight()
	{
		switch (currentState)
		{
		case BladeState.Follow:
			BladeHeightTrans.localPosition = BladeIdleHeight;
			break;
		case BladeState.Shoot:
			BladeHeightTrans.localPosition = BladeShootHeight;
			break;
		case BladeState.OnGround:
			BladeHeightTrans.localPosition = BladeOnGroundHeight;
			break;
		case BladeState.FlyingBack:
			BladeHeightTrans.localPosition = BladeIdleHeight;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void UpdateShadow()
	{
		switch (currentState)
		{
		case BladeState.Follow:
		{
			Vector3 right = (BladeBackRotateTrans ? BladeBackRotateTrans : base.transform).right;
			ShadowTrans.localPosition = new Vector3(right.x / 2f, 0f, 1.05f);
			ShadowSprite.material.SetFloat(BladeShadowWIdthRatio, Mathf.Abs(right.x));
			ShadowSprite.material.SetFloat(RotateAngle, (right.x >= 0f) ? 0f : 180f);
			break;
		}
		case BladeState.Shoot:
			ShadowTrans.localPosition = new Vector3(0f, 0f, 1.05f);
			ShadowSprite.material.SetFloat(BladeShadowWIdthRatio, 1f);
			ShadowSprite.material.SetFloat(RotateAngle, Tool2D.GetDegree(base.Direction) + 90f);
			break;
		case BladeState.FlyingBack:
			ShadowTrans.localPosition = new Vector3(0f, 0f, 1.05f);
			if (base.SIP.spellIsFall)
			{
				ShadowSprite.material.SetFloat(BladeShadowWIdthRatio, 0.1f);
			}
			else
			{
				ShadowSprite.material.SetFloat(BladeShadowWIdthRatio, 1f);
			}
			ShadowSprite.material.SetFloat(RotateAngle, Tool2D.GetDegree(base.Direction) + 90f);
			break;
		case BladeState.OnGround:
		{
			Vector3 position = (BladeFrontRotateTrans ? BladeFrontRotateTrans : base.transform).position;
			ShadowTrans.position = new Vector3(position.x, position.y, 1.05f);
			ShadowSprite.material.SetFloat(BladeShadowWIdthRatio, 0.2f);
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void MovementToggle(bool toggle)
	{
		enableFollowTarget = toggle;
		enableFollowMouse = toggle;
		base.enableAroundPlayer = toggle;
	}

	private void OnGroundCloseOwnerAutoPickUp()
	{
		if (!(Tool2D.IgnoreZDistanceSqr(base.transform.position, GetOwnerBasePoint()) > groundBladePickUpRange * groundBladePickUpRange))
		{
			NormalPickUp();
		}
	}

	private void UpdateBladeShakeRotateAngle()
	{
		if ((bool)BladeFrontRotateTrans && currentState != BladeState.FlyingBack)
		{
			float z = 0f - BladeFrontRotateTrans.parent.eulerAngles.z + OngroundBladeTargetAngle + OnGroundShakeCurve.Evaluate(Mathf.Clamp(OngroundBladeLerpTimer / OngroundBladeLerpTotalTime, 0f, 1f)) * OnGroundBladeShakeAngle;
			BladeFrontRotateTrans.localEulerAngles = new Vector3(0f, 0f, z);
			((Spell4019Effect)EffectBase).SetBladeHiddenData(toggle: true, BladeFrontRotateTrans.position.y);
			OngroundBladeLerpTimer += Time.deltaTime;
		}
	}

	private void NormalPickUp()
	{
		rigid.linearVelocity = Vector3.zero;
		isAttackPullBack = false;
		enableNormalTransform = true;
		if (currentState == BladeState.OnGround)
		{
			MakeFallingGroundDamageToAround();
		}
		BladeRegister(BladeState.FlyingBack);
		MovementToggle(toggle: false);
		base.currentSpellMovement = SpellSpecialMovementType.Normal;
		sc_Rebound.enabled = false;
		HitBox.enabled = false;
		EnterState(BladeState.FlyingBack);
		PlaySE("PickUp");
	}

	public override void OnCollisionEnter(Collision collision)
	{
		if (!isThroughWall && base.currentSpellMovement != SpellSpecialMovementType.Rotation && collision.gameObject.CompareTag("Wall"))
		{
			if (afterShootUnlimitedReboundTimer <= 0f)
			{
				base.rebounceTime--;
				currentShootTimer += 1f;
			}
			if (createReboundEffect && base.currentSpellMovement != SpellSpecialMovementType.Rotation)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_TransparentRebound", Tool2D.GetLayerPoint(base.transform), 1f).transform.localScale = Vector3.one * base.transform.localScale.x * base.ColliderRadius * 0.8f;
			}
			if (base.rebounceTime < 0)
			{
				currentShootTimer = UnityEngine.Random.Range(0.01f, 0.03f);
			}
		}
	}

	protected override void OnHitWallAndSolidObj(Collider col)
	{
		if (!isAttackPullBack && !isThroughWall && base.currentSpellMovement != SpellSpecialMovementType.Rotation)
		{
			_ = base.rebounceTime;
			_ = 0;
		}
	}

	private void FallAttackPickUp()
	{
		rigid.linearVelocity = Vector3.zero;
		enableNormalTransform = false;
		if (currentState == BladeState.OnGround)
		{
			MakeFallingGroundDamageToAround();
		}
		BladeRegister(BladeState.FlyingBack);
		MovementToggle(toggle: false);
		isAttackPullBack = true;
		EnterState(BladeState.FlyingBack);
		base.spellCfg.gravity = 0f;
		base.CurrentUpSpeed = 0f;
		fallbladePullState = FallBladePullBackState.FlyingUp;
		fallBladePullBackFlyingUpSpeed = 0f - FallBladePullBackFlyingUpBaseSpeed.RandomResult();
		fallPullBackTimer = UnityEngine.Random.Range(FallBladePullBackWaitInterval, FallBladePullBackWaitInterval + 0.1f + Mathf.Min((float)base.shooterWand.passiveBiAnBlade.Count * FallBladePullBackExtraIntervalPerBlade, 0.5f));
	}

	public void AttackPickUp()
	{
		if (currentState == BladeState.Follow || currentState == BladeState.FlyingBack)
		{
			return;
		}
		if (base.SIP.spellIsFall)
		{
			FallAttackPickUp();
			return;
		}
		HitBox.enabled = true;
		rigid.linearVelocity = Vector3.zero;
		BladeRegister(BladeState.FlyingBack);
		MovementToggle(toggle: true);
		isAttackPullBack = true;
		EnterState(BladeState.FlyingBack);
		switch (base.currentSpellMovement)
		{
		case SpellSpecialMovementType.ChaseEnemy:
			spellFollowTargetPpt = ownerPpt;
			base.Direction = Tool2D.GetDir((base.transform.position - GetOwnerBasePoint()).normalized, UnityEngine.Random.Range(-90f, 90f));
			break;
		case SpellSpecialMovementType.Rotation:
			base.spellAroundOwnerRadius = Tool2D.IgnoreZDistance(base.transform.position, GetOwnerBasePoint());
			base.spellAroundOwnerCurrentAngle = Tool2D.GetDegree(base.transform.position - GetOwnerBasePoint());
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case SpellSpecialMovementType.Normal:
		case SpellSpecialMovementType.ChaseMouse:
		case SpellSpecialMovementType.ChaseOwner:
			break;
		}
	}

	public override void SpellAroundPlayer()
	{
		if (currentState == BladeState.FlyingBack)
		{
			base.spellAroundOwnerRadius = Mathf.Max(base.spellAroundOwnerRadius - base.CurrentSpeed * SpeedToRotateRadiuChangeRatio * Time.deltaTime, 0.1f);
		}
		else if (base.SIP.spellIsFall)
		{
			base.spellAroundOwnerRadius = defaultRotateRadiu;
		}
		else
		{
			base.spellAroundOwnerRadius = Mathf.Min(defaultRotateRadiu, base.spellAroundOwnerRadius + base.CurrentSpeed * SpeedToRotateRadiuChangeRatio * Time.deltaTime);
		}
		float num = 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / base.CurrentSpeed) * Time.deltaTime;
		base.spellAroundOwnerCurrentAngle += num;
		float num2 = base.spellAroundOwnerCurrentAngle;
		if (float.IsNaN(num2) || float.IsInfinity(num2) || float.IsNegativeInfinity(num2))
		{
			num2 = UnityEngine.Random.Range(0f, 360f);
		}
		base.Direction = Tool2D.GetDir(num2 + 90f);
		Vector3 v = GetOwnerCenterPoint() + Tool2D.GetDir(num2) * base.spellAroundOwnerRadius;
		base.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
		SpellAroundPlayerUpdateMoveTrigger(num);
	}

	protected override void SpellFollowTarget()
	{
		Vector3 point = (spellFollowTargetPpt ? spellFollowTargetPpt.transform.position : (base.transform.position + base.Direction));
		if (currentState == BladeState.FlyingBack)
		{
			attackPullBackChaseEnemyTimer += Time.deltaTime;
			point = GetOwnerCenterPoint();
		}
		spellFollowTargetRotateSpeed = ((currentState == BladeState.FlyingBack) ? (defaultChaseTargetAngleSpeed + attackPullBackChaseEnemyTimer * AttackPullBackChaseTimerToAngleSpeedRatio) : defaultChaseTargetAngleSpeed);
		if (base.SpellFollowHaveTarget)
		{
			base.Direction = Tool2D.DirMoveTowards(base.Direction, ToPointDir(point), base.CurrentSpeed * spellFollowTargetRotateSpeed * Time.deltaTime);
			rigid.linearVelocity = base.Direction * base.CurrentSpeed;
		}
		else
		{
			spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
		}
	}

	protected override void SpellFollowMouse()
	{
		if (currentState != BladeState.FlyingBack)
		{
			base.SpellFollowMouse();
			return;
		}
		attackPullBackChaseMouseTimer += Time.deltaTime;
		Vector3 point = Vector3.Lerp(PlayerMgr.Inst.GetMousePoint(base.transform.position.z), GetOwnerCenterPoint(), AttackPullBackMouseLerpCurve.Evaluate(Mathf.Min(1f, attackPullBackChaseMouseTimer / AttackPullBackChaseMouseTime)));
		Vector3 linearVelocity = Vector3.Lerp(rigid.linearVelocity, ToPointDir(point) * base.CurrentSpeed, base.CurrentSpeed * Time.deltaTime * spellFollowMouseLerp);
		rigid.linearVelocity = linearVelocity;
		base.Direction = linearVelocity.normalized;
	}

	protected override void SpellNormalTransform()
	{
		base.SpellNormalTransform();
		switch (currentState)
		{
		case BladeState.FlyingBack:
			base.Direction = (GetOwnerCenterPoint() - base.transform.position).normalized;
			rigid.linearVelocity = base.Direction * base.CurrentSpeed;
			break;
		case BladeState.Shoot:
			base.Direction = rigid.linearVelocity.normalized;
			break;
		}
	}

	protected override bool HalfLifeRandomTeleportRequirementCheck()
	{
		if (halfLifeTeleportCount > 0)
		{
			return currentShootTimer <= currentShootDuration / 2f;
		}
		return false;
	}

	protected override bool HalfLifeRandomTeleport()
	{
		if (currentState != BladeState.Shoot || halfLifeTeleportCount <= 0)
		{
			return false;
		}
		bool num = base.HalfLifeRandomTeleport();
		if (num)
		{
			currentShootTimer += 2f;
			currentShootDuration = currentShootTimer;
		}
		return num;
	}

	public void SetBladeData(float wandMaxMana, float manaToDamageRatio)
	{
		base.spellCfg.damage = Mathf.Ceil(manaToDamageRatio * base.damageRatio * base.finalDamageRatio * (base.spellCfg.isSplitSpell ? 0.35f : 1f));
	}

	public override void PoolRecycle()
	{
		spellEndTeleport = false;
		base.endThunderHitPercent = 0f;
		base.PoolRecycle();
	}

	private void EnterState(BladeState state)
	{
		switch (state)
		{
		case BladeState.Follow:
			BladeRegister(BladeState.Follow);
			HitBox.enabled = false;
			((BoxCollider)HitBox).size = NormalColliderSize;
			sc_Rebound.enabled = false;
			rigid.linearVelocity = Vector3.zero;
			MovementToggle(toggle: false);
			UpdateBladeRotateAngle(idleLookDown: true);
			UpdateBladeFollowOwnerPositionV2(isInstanceMove: true);
			base.CurrentSpeed = 0f;
			currentShootTimer = shootDefaultTime.RandomResult();
			currentShootDuration = currentShootTimer;
			outMapAutoPickUpTimer = 0f;
			attackPullBackChaseMouseTimer = 0f;
			attackPullBackChaseEnemyTimer = 0f;
			if ((bool)BladeFrontRotateTrans)
			{
				BladeFrontRotateTrans.localEulerAngles = Vector3.zero;
			}
			((Spell4019Effect)EffectBase).SetBladeHiddenData(toggle: false);
			BladeHeightTrans.localPosition = BladeIdleHeight;
			((Spell4019Effect)EffectBase).LightSaberSwitch(toggle: false);
			if ((bool)BladeFrontRotateTrans)
			{
				BladeFrontRotateTrans.localPosition = new Vector3(0.43f, 0f, 0f);
			}
			((Spell4019Effect)EffectBase).TrailParticleToggle(toggle: false);
			break;
		case BladeState.Shoot:
			if (!base.SIP.spellIsFall)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform) + new Vector3(0f, 0f, -0.3f);
				BladeHeightTrans.localPosition = BladeShootHeight;
			}
			((Spell4019Effect)EffectBase).TrailSwitch(toggle: true);
			((Spell4019Effect)EffectBase).TrailParticleToggle(toggle: true);
			((Spell4019Effect)EffectBase).LightSaberSwitch(toggle: true);
			break;
		case BladeState.OnGround:
		{
			rigid.linearVelocity = Vector3.zero;
			sc_Rebound.enabled = false;
			HitBox.enabled = false;
			MovementToggle(toggle: false);
			BladeRegister(BladeState.OnGround);
			float num = ((base.Direction.x >= 0f) ? 1 : (-1));
			if (base.SIP.spellIsFall)
			{
				num = UnityEngine.Random.Range(-0.5f, 0.5f);
			}
			OngroundBladeTargetAngle = 270f + OngroundBladeBaseAngle.RandomResult() * num;
			OnGroundBladeShakeAngle = OngroundBladeShakeAngleRange.RandomResult();
			OngroundBladeLerpTotalTime = OngroundBladeShakeDurationRange.RandomResult();
			OngroundBladeLerpTimer = 0f;
			UpdateBladeShakeRotateAngle();
			BladeHeightTrans.localPosition = BladeIdleHeight;
			if ((bool)BladeFrontRotateTrans)
			{
				BladeFrontRotateTrans.localPosition = Vector3.zero;
			}
			((Spell4019Effect)EffectBase).TrailSwitch(toggle: false);
			((Spell4019Effect)EffectBase).LightSaberSwitch(toggle: false);
			((Spell4019Effect)EffectBase).TrailParticleToggle(toggle: false);
			if (UnityEngine.Random.Range(0f, 1f) < base.endTHunderHitChance)
			{
				EndThunderAttackCheck(thunderOnly: false);
			}
			base.transform.position = Tool2D.GetNavMeshPointIngoreZ(base.transform.position);
			if (base.SIP.spellIsFall)
			{
				base.CurrentUpSpeed = 0f;
				base.Height = 0f;
			}
			break;
		}
		case BladeState.FlyingBack:
			if (isAttackPullBack)
			{
				switch (base.currentSpellMovement)
				{
				case SpellSpecialMovementType.Normal:
					base.CurrentSpeed *= AttackPullBackNormalSpeedRatio;
					break;
				case SpellSpecialMovementType.ChaseEnemy:
				case SpellSpecialMovementType.ChaseOwner:
					base.CurrentSpeed *= AttackPullBackChaseEnemySpeedRatio;
					break;
				case SpellSpecialMovementType.ChaseMouse:
					base.CurrentSpeed *= AttackPullBackChaseMouseSpeedRatio;
					break;
				case SpellSpecialMovementType.Rotation:
					base.CurrentSpeed *= AttackPullBackRotationSpeedRatio;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			halfLifeTeleportCount = initialHalfLifeTeleportCount;
			BladeFrontRotateTrans.localEulerAngles = Vector3.zero;
			((Spell4019Effect)EffectBase).SetBladeHiddenData(toggle: false);
			ResetBladeInGroupPosition();
			BladeHeightTrans.localPosition = BladeShootHeight;
			if ((bool)BladeFrontRotateTrans)
			{
				BladeFrontRotateTrans.localPosition = new Vector3(0.43f, 0f, 0f);
			}
			((BoxCollider)HitBox).size = PullBackColliderSize;
			((Spell4019Effect)EffectBase).TrailSwitch(toggle: true);
			((Spell4019Effect)EffectBase).LightSaberSwitch(toggle: true, instanceChange: true);
			((Spell4019Effect)EffectBase).TrailParticleToggle(toggle: true);
			break;
		default:
			throw new ArgumentOutOfRangeException("state", state, null);
		}
	}

	public void BladeRegister(BladeState state)
	{
		RemoveBladeFromAllList();
		switch (state)
		{
		case BladeState.Follow:
			followOwnerBladesList.Add(this);
			currentState = BladeState.Follow;
			break;
		case BladeState.Shoot:
			shootingBladesList.Add(this);
			currentState = BladeState.Shoot;
			break;
		case BladeState.OnGround:
			shootingBladesList.Add(this);
			currentState = BladeState.OnGround;
			break;
		case BladeState.FlyingBack:
			PickingBladesList.Add(this);
			currentState = BladeState.FlyingBack;
			break;
		default:
			throw new ArgumentOutOfRangeException("state", state, null);
		}
	}

	protected override void UpdateUpSpeedWithFalling()
	{
		if (base.InFallRebounding && currentState == BladeState.Shoot)
		{
			base.CurrentUpSpeed -= Time.deltaTime * InFallingReboundingGravity;
		}
	}

	private void UpdateBladeFollowOwnerPositionV2(bool isInstanceMove = false)
	{
		Vector3 bladeInGroupToOwnerPosition = GetBladeInGroupToOwnerPosition();
		float t = (isInstanceMove ? 1f : BladeFollowOwnerSpeedLerpRatio);
		base.transform.position = Vector3.Lerp(base.transform.position, bladeInGroupToOwnerPosition, t);
		if (Tool2D.IgnoreZDistanceSqr(base.transform.position, bladeInGroupToOwnerPosition) <= 0.010000001f)
		{
			if (isAttackPullBack)
			{
				isAttackPullBack = false;
				PlaySE("PickUp");
			}
			((Spell4019Effect)EffectBase).TrailSwitch(toggle: false);
		}
	}

	private Vector3 GetBladeInGroupToOwnerPosition()
	{
		return GetOwnerBasePoint() + GetGroupLookBasePosition() + inBladeGroupPosition;
	}

	private Vector3 GetOwnerCenterPoint()
	{
		return GetOwnerBasePoint() + bladeFollowHeightShift;
	}

	private Vector3 GetGroupBasePosition(Vector3 dir = default(Vector3), bool fourDirOnly = false)
	{
		if (fourDirOnly && (dir == default(Vector3) || dir == Vector3.zero))
		{
			return Vector3.zero;
		}
		Vector3 obj = ((dir == default(Vector3)) ? Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.GetMousePoint(), GetOwnerBasePoint() + bladeFollowHeightShift) : dir);
		float x = obj.x;
		float y = obj.y;
		if (Mathf.Abs(x) >= Mathf.Abs(y))
		{
			if (x >= 0f)
			{
				if (!fourDirOnly)
				{
					return lookRightBladeGroupBasePos;
				}
				return new Vector3(1f, 0f, 0f);
			}
			if (!fourDirOnly)
			{
				return lookLeftBladeGroupBasePos;
			}
			return new Vector3(-1f, 0f, 0f);
		}
		if (y >= 0f)
		{
			if (!fourDirOnly)
			{
				return lookUpBladeGroupBasePos;
			}
			return new Vector3(0f, 1f, 0f);
		}
		if (!fourDirOnly)
		{
			return lookDownBladeGroupBasePos;
		}
		return new Vector3(0f, -1f, 0f);
	}

	private Vector3 GetGroupLookBasePosition()
	{
		Vector3 vector = Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.GetMousePoint(), GetOwnerBasePoint());
		float x = vector.x;
		float y = vector.y;
		if (x >= 0f)
		{
			if (y > 0f)
			{
				return LUGroupBasePos;
			}
			return LDGroupBasePos;
		}
		if (y > 0f)
		{
			return RUGroupBasePos;
		}
		return RDGroupBasePos;
	}

	private void LowFrameForcePullBack()
	{
		if (Tool2D.IgnoreZDistanceSqr(base.transform.position, GetOwnerBasePoint()) <= base.CurrentSpeed * base.CurrentSpeed * Time.deltaTime * Time.deltaTime && base.currentSpellMovement != SpellSpecialMovementType.Rotation)
		{
			base.transform.position = GetOwnerBasePoint() + new Vector3(0f, 0.5f, 0f);
			EnterState(BladeState.Follow);
		}
	}

	private void CheckFlyBackToOwnerDistance()
	{
		Vector3 ownerCenterPoint = GetOwnerCenterPoint();
		SpellSpecialMovementType spellSpecialMovementType = base.currentSpellMovement;
		float num = ((spellSpecialMovementType == SpellSpecialMovementType.Rotation || spellSpecialMovementType == SpellSpecialMovementType.ChaseMouse) ? longBackToFollowStateRange : normalBackToNormalRange);
		if ((base.SIP.spellIsFall && fallbladePullState == FallBladePullBackState.BackToOwner && base.transform.position.z >= -1f) || (!base.SIP.spellIsFall && Tool2D.IgnoreZDistanceSqr(base.transform.position, ownerCenterPoint) <= num) || (base.SIP.spellIsFall && !isAttackPullBack && Tool2D.IgnoreZDistanceSqr(base.transform.position, ownerCenterPoint) <= num))
		{
			base.transform.position = ownerCenterPoint;
			EnterState(BladeState.Follow);
		}
	}

	private Vector3 GetOwnerBasePoint()
	{
		if (!followTransform)
		{
			return ownerPpt.transform.position;
		}
		return Tool2D.IgnoreZPoint(followTransform.position);
	}

	private void UpdateBladeRotateAngle(bool idleLookDown = false)
	{
		if (idleLookDown)
		{
			base.Direction = new Vector3(0f, -1f, 0f);
		}
		else if (autoWand == null)
		{
			base.Direction = Tool2D.IgnoreZV2ToV1Normal(Tool2D.IgnoreZPoint(PlayerMgr.Inst.GetMousePoint()), ownerPpt.transform.position);
		}
		else
		{
			base.Direction = Tool2D.IgnoreZV2ToV1Normal(Tool2D.IgnoreZPoint(PlayerMgr.Inst.GetMousePoint()), Tool2D.IgnoreZPoint(autoWand.wandObject.transform.position));
		}
	}

	private void UpdateOwnerMoveTinyAngleShiftState()
	{
		if ((bool)BladeBackRotateTrans)
		{
			float num = Tool2D.IgnoreZDistanceSqr(GetOwnerBasePoint(), lastFrameOwnerPosition);
			Vector3 vector = ((num <= 0.000225f) ? Vector3.zero : Tool2D.IgnoreZPoint(GetOwnerBasePoint() - lastFrameOwnerPosition).normalized);
			Vector3 groupBasePosition = GetGroupBasePosition(vector, fourDirOnly: true);
			float num2 = Mathf.Clamp(num / (PlayerMoveNormalSpeed * PlayerMoveNormalSpeed / 100f * Time.deltaTime), 1f, 3f);
			lastFrameOwnerPosition = GetOwnerBasePoint();
			if (vector == Vector3.zero || (groupBasePosition.x == 0f && groupBasePosition.y < 0f))
			{
				BladeBackRotateTrans.right = Tool2D.DirMoveTowardsTargetInMinAnlgeClockWiseSmoothLerp(BladeBackRotateTrans.right, new Vector3(0f, -1f, 0f), OwnerMoveBladeAngleTinyShiftLerpSpeed);
			}
			else if (groupBasePosition.x > 0f)
			{
				float num3 = 1f - (MoveMinAngleRatio + (1f - MoveMinAngleRatio) * (bladeGroupRadiu + inBladeGroupPosition.x) / (bladeGroupRadiu * 2f));
				Vector3 dir = Tool2D.GetDir(new Vector3(0f, -1f, 0f), (0f - LRMoveMaxAngleShift) * num3 * num2);
				BladeBackRotateTrans.right = Tool2D.DirMoveTowardsTargetInMinAnlgeClockWiseSmoothLerp(BladeBackRotateTrans.right, dir, OwnerMoveBladeAngleTinyShiftLerpSpeed);
			}
			else if (groupBasePosition.x < 0f)
			{
				float num4 = 1f - (MoveMinAngleRatio + (1f - MoveMinAngleRatio) * (bladeGroupRadiu - inBladeGroupPosition.x) / (bladeGroupRadiu * 2f));
				Vector3 dir2 = Tool2D.GetDir(new Vector3(0f, -1f, 0f), LRMoveMaxAngleShift * num4 * num2);
				BladeBackRotateTrans.right = Tool2D.DirMoveTowardsTargetInMinAnlgeClockWiseSmoothLerp(BladeBackRotateTrans.right, dir2, OwnerMoveBladeAngleTinyShiftLerpSpeed);
			}
			else if (groupBasePosition.y > 0f)
			{
				float num5 = inBladeGroupPosition.x / bladeGroupRadiu;
				Vector3 dir3 = Tool2D.GetDir(new Vector3(0f, -1f, 0f), UMoveMaxAngleShift * num5 * num2);
				BladeBackRotateTrans.right = Tool2D.DirMoveTowardsTargetInMinAnlgeClockWiseSmoothLerp(BladeBackRotateTrans.right, dir3, OwnerMoveBladeAngleTinyShiftLerpSpeed);
			}
		}
	}

	public Spell4019BiAnLethalBlade TryShootBiAnBlade(Wand shooterWand, Vector3 shootDir = default(Vector3))
	{
		if (followOwnerBladesList.Count > 0)
		{
			foreach (Spell4019BiAnLethalBlade followOwnerBlades in followOwnerBladesList)
			{
				if (followOwnerBlades.shooterWand == shooterWand)
				{
					followOwnerBlades.ShootBiAnBlade(shootDir);
					return followOwnerBlades;
				}
			}
		}
		return null;
	}

	protected override void OnFallingGroundTryReboundOrRecycle()
	{
		if (!OnFallingGroundTryRebound())
		{
			EnterState(BladeState.OnGround);
		}
	}

	private void FallShootBiAnBlade(Vector3 overrideShootDir)
	{
		enableFollowTarget = true;
		UpdateBladeRotateAngle();
		afterShootUnlimitedReboundTimer = afterShootUnlimitedReboundDuration;
		if (overrideShootDir != default(Vector3))
		{
			base.Direction = overrideShootDir.normalized;
		}
		base.Direction = Tool2D.GetDir(base.Direction, UnityEngine.Random.Range(0f - base._angle, base._angle));
		if (base.SIP.reverseDirection)
		{
			base.Direction *= -1f;
		}
		SpellEffectBase effectBase = EffectBase;
		Vector3? position = tsf_Layer.position + new Vector3(0f, 0f, -0.3f);
		effectBase.ManualCreateEffect("FallFlyUp", null, position);
		base.penetrateTime = 100;
		base.rebounceTime = normalReboundTime;
		currentShootTimer = 1000f;
		base.spellCfg.speed = (baseSpeed + base.bonusSpeed) * base.speedRatio * base.finalSpeedRatio;
		base.CurrentSpeed = base.spellCfg.speed;
		if (base.currentSpellMovement == SpellSpecialMovementType.ChaseEnemy)
		{
			spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
		}
		if (!PlayerMgr.Inst.ItemCtrller.relic_SpellThroughWall)
		{
			sc_Rebound.enabled = true;
		}
		HitBox.enabled = true;
		base.currentSpellMovement = originMovement;
		base.Direction = base.Direction.normalized;
		BladeBackRotateTrans.localEulerAngles = Vector3.zero;
		PlaySE("StartShoot");
		base.spellAroundOwnerRadius = longBackToFollowStateRange;
		BladeRegister(BladeState.Shoot);
		InitSpeedAndPositionWithFall();
		EnterState(BladeState.Shoot);
		((Spell4019Effect)EffectBase).TrailSwitch(toggle: false);
		((Spell4019Effect)EffectBase).TrailParticleToggle(toggle: false);
		ApplySpeedToVelocity();
		MovementToggle(toggle: true);
		afterFallShootClearTrailTimer = 0.03f;
	}

	protected override void InitSpeedAndPositionWithFall()
	{
		Vector3 target = ((base.currentSpellMovement == SpellSpecialMovementType.Rotation) ? base.transform.position : PlayerMgr.Inst.GetMousePoint());
		base.SIP.finalShootSpatialInfo = ShootSpellSpatialInfo.ToPoint(base.transform.position.IgnoreZ(), target);
		base.InitSpeedAndPositionWithFall();
	}

	private void ShootBiAnBlade(Vector3 overrideShootDir)
	{
		if (base.SIP.spellIsFall)
		{
			FallShootBiAnBlade(overrideShootDir);
			return;
		}
		enableFollowTarget = true;
		UpdateBladeRotateAngle();
		afterShootUnlimitedReboundTimer = afterShootUnlimitedReboundDuration;
		if (overrideShootDir != default(Vector3))
		{
			base.Direction = overrideShootDir.normalized;
		}
		base.Direction = Tool2D.GetDir(base.Direction, UnityEngine.Random.Range(0f - base._angle, base._angle));
		if (base.SIP.reverseDirection)
		{
			base.Direction *= -1f;
		}
		base.penetrateTime = 100;
		base.rebounceTime = normalReboundTime;
		currentShootTimer = (shootDefaultTime.RandomResult() + base.bonusDuration / 2f) * base.finalDurationRatio;
		base.spellCfg.speed = (baseSpeed + base.bonusSpeed) * base.speedRatio * base.finalSpeedRatio;
		base.CurrentSpeed = base.spellCfg.speed;
		if (base.currentSpellMovement == SpellSpecialMovementType.ChaseEnemy)
		{
			spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
		}
		if (base.SIP.speedDecreaseRatio < 1f)
		{
			currentShootTimer += GeneralTool.GetSpellSpeedToBonusDuration(base.spellCfg.speed / base.SIP.speedDecreaseRatio, base.SIP.speedDecreaseRatio, base.SIP.speedDecreaseTransIntoDurationRatio);
		}
		if (!PlayerMgr.Inst.ItemCtrller.relic_SpellThroughWall)
		{
			sc_Rebound.enabled = true;
		}
		HitBox.enabled = true;
		MovementToggle(toggle: true);
		base.currentSpellMovement = originMovement;
		base.Direction = base.Direction.normalized;
		ApplySpeedToVelocity();
		BladeBackRotateTrans.localEulerAngles = Vector3.zero;
		PlaySE("StartShoot");
		base.spellAroundOwnerRadius = longBackToFollowStateRange;
		BladeRegister(BladeState.Shoot);
		EnterState(BladeState.Shoot);
	}
}
