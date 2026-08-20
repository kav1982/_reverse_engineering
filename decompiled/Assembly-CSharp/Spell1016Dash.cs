using System;
using System.Collections.Generic;
using UnityEngine;

public class Spell1016Dash : SpellBase, IOnLaunchFromUnitNotPlayer
{
	[Space(50f)]
	public ShockParam shockParam;

	public ShockParam fallShockParam;

	public float hitSlowTimeEffectDuration;

	public SphereCollider rebounceRange;

	public SphereCollider HitBoxRange;

	private float hitSlowTimeEffectTimer;

	private Vector3 velocityBeforeTimeSlow = Vector3.zero;

	private Vector3 AroundCenter = Vector3.zero;

	private static readonly List<SpellBase> spellPassengerList = new List<SpellBase>();

	private static readonly List<UnitProperty> unitPassengerList = new List<UnitProperty>();

	[HideInInspector]
	public UnitProperty currentUnitPassenger;

	[HideInInspector]
	public SpellBase currentSpellPassenger;

	private bool passengerAbord;

	private Transform passengerTransform;

	private bool mainDriver;

	public float reboundSize;

	[Min(0f)]
	public float chaseSpeedRatio;

	public float mouseSpeedRatio;

	public float rotateSpeedRatio;

	public float toMouseSpeedLerpDistanceThreshHold;

	private float toMouseLerpDistanceSpeedRatio = 1f;

	public AnimationCurve DurationTimeToOverheatCurve;

	private readonly List<GameObject> passengerVisualObject = new List<GameObject>();

	public float landForceRatio;

	private Vector3 rigidVelocityBeforeStop = Vector3.zero;

	private bool firstFramePass;

	public float playerControlSpeed;

	public static bool playerOnboard = false;

	public static Spell1016Dash playerDashSpell = null;

	public float hitboxRestartInterval;

	private float hitboxRestartTimer;

	public float afterHitWallControllabilityRecovyTime;

	private float controllabilityRecovyTimer;

	private bool castByPlayerOrTeammate;

	private float GetOverheatTime => DurationTimeToOverheatCurve.Evaluate(base.DurationTimer);

	public bool ignorePassenger { get; set; }

	public Vector3 independentRotateFollowPoint { get; set; } = Vector3.zero;


	protected override float FallingReboundForce => base.FallingReboundForce * 0.7f;

	public override void InitializeCallback()
	{
		currentUnitPassenger = null;
		currentSpellPassenger = null;
		passengerAbord = false;
		AroundCenter = Vector3.zero;
		passengerTransform = null;
		mainDriver = false;
		passengerVisualObject.Clear();
		ignorePassenger = false;
		firstFramePass = false;
		independentRotateFollowPoint = Vector3.zero;
		controllabilityRecovyTimer = 0f;
		base.shouldCameraShock = false;
		toMouseLerpDistanceSpeedRatio = 1f;
		castByPlayerOrTeammate = false;
		hitSlowTimeEffectTimer = 0f;
		velocityBeforeTimeSlow = Vector3.zero;
		if (!base.SIP.spellIsFall)
		{
			base.rebounceTime = 1000;
			base.penetrateTime = 1000;
		}
		rigidVelocityBeforeStop = Vector3.zero;
		if (ownerPpt != null && ownerPpt.unitCfg.unitType != 0)
		{
			base.shouldCameraShock = false;
		}
		hitboxRestartTimer = 0f;
		if (base.currentSpellMovement == SpellSpecialMovementType.ChaseOwner)
		{
			sc_Rebound.enabled = false;
		}
		if (IsSameCamp(UnitType.Player) && !base.SIP.spellIsFall)
		{
			PlayLoopSE("Loop", base.spellCfg.duration + base.SpellHoverTime);
		}
		if (!base.SIP.spellIsFall)
		{
			Abord();
		}
		else
		{
			ApplySpeedToVelocity();
		}
	}

	protected override void InitSpeedAndPosition()
	{
		base.spellCfg.speed = GetCurrentSpeed();
		base.InitSpeedAndPosition();
	}

	private float GetCurrentSpeed()
	{
		if (ownerPpt.unitCfg.unitType == UnitType.Player)
		{
			return (SpellConfig.dic[base.spellCfg.id].speed + base.bonusSpeed + PlayerMgr.Inst.PlayerPpt.unitCfg.moveSpeed) * base.speedRatio * base.finalSpeedRatio * MathF.Abs(PlayerMgr.Inst.PlayerPpt.MoveSpeedRatio);
		}
		return (SpellConfig.dic[base.spellCfg.id].speed + base.bonusSpeed) * base.speedRatio * base.finalSpeedRatio;
	}

	public override void OnFirstFrame()
	{
		if (!base.SIP.spellIsFall)
		{
			base.transform.localScale = Vector3.one;
			tsf_Layer.localScale = Vector3.one * base.spellCfg.radius;
		}
		rebounceRange.radius = base.spellCfg.radius * reboundSize;
		HitBoxRange.radius = base.spellCfg.radius;
	}

	public override void OnSecondFrame()
	{
		base.OnSecondFrame();
		firstFramePass = true;
	}

	private void Abord()
	{
		if (passengerAbord)
		{
			return;
		}
		passengerAbord = true;
		SpellSpecialMovementType spellSpecialMovementType = base.currentSpellMovement;
		if (spellSpecialMovementType == SpellSpecialMovementType.Rotation || spellSpecialMovementType == SpellSpecialMovementType.ChaseOwner)
		{
			rigid.linearVelocity = Vector3.zero;
		}
		else
		{
			rigid.linearVelocity = base.Direction.normalized * base.CurrentSpeed;
		}
		if (!ignorePassenger)
		{
			if (base.OwnerSpell != null && (!(base.SIP.ShootCause is ShootCause.BySpell bySpell) || !bySpell.ByTrigger))
			{
				if (!spellPassengerList.Contains(base.OwnerSpell) && !base.spellCfg.isSplitSpell)
				{
					currentSpellPassenger = base.OwnerSpell;
					spellPassengerList.Add(base.OwnerSpell);
					if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
					{
						AroundCenter = base.OwnerSpell.transform.position;
					}
					mainDriver = true;
					passengerTransform = base.OwnerSpell.transform;
					if (base.OwnerSpell.gameObject.activeSelf)
					{
						base.transform.position = Tool2D.GetNavMeshPointIngoreZ(base.OwnerSpell.transform.position);
					}
				}
				if (AroundCenter != Vector3.zero)
				{
					AroundCenter = base.OwnerSpell.transform.position;
				}
			}
			else if (base.OwnerSpell == null && !base.spellCfg.isSplitSpell)
			{
				if (!unitPassengerList.Contains(ownerPpt) && (!PlayerMgr.Inst.PlayerCtrller.isDashOverHeat || (ownerPpt != null && ownerPpt.unitCfg.unitType != 0)))
				{
					currentUnitPassenger = ownerPpt;
					unitPassengerList.Add(ownerPpt);
					if (ownerPpt != null && ownerPpt.unitCfg.unitType == UnitType.Player)
					{
						ownerPpt.SetUnitInvisibleState(state: true);
						PlayerMgr.Inst.SelectedWand.Display_Hide();
						PlayerMgr.Inst.PlayerCtrller.NonInteractiveRegister();
						PlayerMgr.Inst.inDashSpell = true;
						PlayerMgr.Inst.PlayerCtrller.ClosePlayerCollider();
						independentRotateFollowPoint = GetAroundTargetBasePoint() + new Vector3(0f, 0f, base.transform.position.z);
						CamController.Inst.dashCamFollowPoint = independentRotateFollowPoint;
						CamController.Inst.playerInRotateDash = base.currentSpellMovement == SpellSpecialMovementType.Rotation;
						base.shouldCameraShock = true;
						playerOnboard = true;
						playerDashSpell = this;
					}
					else if (ownerPpt != null && passengerVisualObject.Count > 0)
					{
						for (int i = 0; i < passengerVisualObject.Count; i++)
						{
							passengerVisualObject[i].SetActive(value: false);
						}
					}
					if (IsSameCamp(UnitType.Player))
					{
						castByPlayerOrTeammate = true;
						ownerPpt.InvincibleRegister();
					}
					ownerPpt.FlyRegister();
					if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
					{
						if (base.OwnerPoint == Vector3.zero)
						{
							AroundCenter = ownerPpt.transform.position;
						}
						EffectBase.ManualCreateEffect("Teleport");
						PlaySE("Teleport");
					}
					mainDriver = true;
					passengerTransform = ownerPpt.transform;
				}
				if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
				{
					if (base.OwnerPoint != Vector3.zero)
					{
						Vector3 ownerPoint = base.OwnerPoint;
						base.transform.position = Tool2D.IgnoreZPoint(ownerPoint, base.transform.position.z);
						if (mainDriver)
						{
							passengerTransform.position = base.transform.position;
						}
					}
				}
				else
				{
					base.transform.position = new Vector3(0f, 0f, base.transform.position.z) + Tool2D.GetNavMeshPointIngoreZ(ownerPpt.transform.position);
				}
			}
		}
		if (base.currentSpellMovement == SpellSpecialMovementType.ChaseOwner)
		{
			independentRotateFollowPoint = base.transform.position;
		}
	}

	public override void Update()
	{
		base.Update();
		hitboxRestartTimer += Time.deltaTime;
		if (!HitBoxRange.gameObject.activeInHierarchy)
		{
			HitBoxRange.gameObject.SetActive(value: true);
		}
		if (hitboxRestartTimer >= hitboxRestartInterval)
		{
			hitboxRestartTimer -= hitboxRestartInterval;
			HitBoxRange.gameObject.SetActive(value: false);
		}
		if (currentUnitPassenger != null && (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1) && (!(ownerPpt.tag != "Player") || base.currentSpellMovement != SpellSpecialMovementType.Rotation))
		{
			Vector3 chapter3RepositionChangeValue = LevelMgr.Inst.CurrentRoomCtrller.GetChapter3RepositionChangeValue(currentUnitPassenger.transform);
			if (chapter3RepositionChangeValue != Vector3.zero && currentUnitPassenger != null)
			{
				currentUnitPassenger.transform.position += chapter3RepositionChangeValue;
				base.transform.position = currentUnitPassenger.transform.position;
			}
		}
		if (currentUnitPassenger != null && currentUnitPassenger.unitCfg.unitType == UnitType.Player)
		{
			PlayerMgr.Inst.PlayerCtrller.CurrentMotion += new Vector3(0.01f * Time.deltaTime, 0f, 0f);
		}
		base.DurationTimer += Time.deltaTime;
		if (!(base.DurationTimer > base.spellCfg.duration) || base.SIP.spellIsFall)
		{
			return;
		}
		if (!base.isFlyFinish)
		{
			rigidVelocityBeforeStop = rigid.linearVelocity;
			base.isFlyFinish = true;
			rigid.linearVelocity = Vector3.zero;
			base.CurrentSpeed = 0f;
		}
		if (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)
		{
			base.SpellHoverTimer += Time.deltaTime;
			if (rigidVelocityBeforeStop != Vector3.zero)
			{
				rigidVelocityBeforeStop = Vector3.zero;
			}
			rigid.linearVelocity = Vector3.zero;
			if (base.SpellHoverTimer >= base.SpellHoverTime)
			{
				PoolRecycle();
			}
		}
		else
		{
			tsf_Layer.localScale = Vector3.one * (tsf_Layer.localScale.x - 5f * Time.deltaTime);
			if (tsf_Layer.localScale.x <= 0f)
			{
				PoolRecycle();
			}
			else
			{
				PoolRecycle();
			}
		}
	}

	public override void LateUpdate()
	{
		if (base.Direction == Vector3.zero && !base.isFlyFinish && base.currentSpellMovement != SpellSpecialMovementType.Rotation && base.currentSpellMovement != SpellSpecialMovementType.ChaseMouse)
		{
			base.Direction = Tool2D.IgnoreZPoint(UnityEngine.Random.insideUnitSphere, 0f).normalized;
			rigid.linearVelocity = base.Direction.normalized * base.spellCfg.speed;
		}
		if (!ignorePassenger && (base.DurationTimer < base.spellCfg.duration || (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)) && mainDriver && passengerTransform != null && (base.OwnerSpell == null || base.OwnerSpell.gameObject.activeSelf))
		{
			passengerTransform.position = base.transform.position;
		}
		if (!base.isFlyFinish)
		{
			if ((base.currentSpellMovement == SpellSpecialMovementType.Rotation || base.currentSpellMovement == SpellSpecialMovementType.ChaseOwner) && mainDriver && currentUnitPassenger != null && currentUnitPassenger.unitCfg.unitType == UnitType.Player)
			{
				PlayerController playerCtrller = PlayerMgr.Inst.PlayerCtrller;
				independentRotateFollowPoint += playerCtrller.CurrentMoveDir * playerCtrller.myPpt.MoveSpeed * Time.deltaTime * playerControlSpeed;
				CamController.Inst.dashCamFollowPoint = independentRotateFollowPoint;
			}
			else if (base.currentSpellMovement == SpellSpecialMovementType.Normal && mainDriver && currentUnitPassenger != null && currentUnitPassenger.unitCfg.unitType == UnitType.Player && controllabilityRecovyTimer >= afterHitWallControllabilityRecovyTime)
			{
				controllabilityRecovyTimer = Mathf.Min(1f, controllabilityRecovyTimer + afterHitWallControllabilityRecovyTime * Time.deltaTime);
				Vector2 inputWASD = ControlMgr.Inst.GetInputWASD();
				if (inputWASD != Vector2.zero)
				{
					base.Direction = inputWASD;
					rigid.linearVelocity = base.Direction * base.spellCfg.speed;
				}
			}
		}
		controllabilityRecovyTimer += Time.deltaTime;
		base.spellCfg.speed = GetCurrentSpeed();
		base.LateUpdate();
	}

	private void TryScreenShake()
	{
		if (base.shouldCameraShock)
		{
			CamController.Inst.SetShock(shockParam);
		}
	}

	protected override bool OnHitUnit(UnitProperty unit)
	{
		if (IsSameCamp(unit))
		{
			return false;
		}
		bool result = base.OnHitUnit(unit);
		hitboxRestartTimer = 0f;
		SlowTime();
		TryScreenShake();
		return result;
	}

	protected override bool OnHitDestructible(UnitProperty go)
	{
		bool result = base.OnHitDestructible(go);
		hitboxRestartTimer = 0f;
		SlowTime();
		TryScreenShake();
		return result;
	}

	protected override bool OnHitSpell(SpellBase spell)
	{
		bool result = base.OnHitSpell(spell);
		if (IsSameCamp(spell) || !(spell is Spell1002RollBall))
		{
			return result;
		}
		hitboxRestartTimer = 0f;
		TryScreenShake();
		SlowTime();
		return result;
	}

	protected override void OnHitWallAndSolidObj(Collider col)
	{
		base.OnHitWallAndSolidObj(col);
		if (col.CompareTag("SolidObj"))
		{
			controllabilityRecovyTimer = 0f;
			SlowTime();
			TryScreenShake();
		}
	}

	public override void TriggerIn(Collider other)
	{
		if (firstFramePass)
		{
			base.TriggerIn(other);
		}
	}

	public override void SpellAroundPlayer()
	{
		if (base.SIP.spellIsFall || base.SIP.tags.Contains(SpellTag.Twine))
		{
			base.SpellAroundPlayer();
		}
		else if (!base.isFlyFinish)
		{
			if (hitSlowTimeEffectTimer > 0f)
			{
				hitSlowTimeEffectTimer -= Time.deltaTime;
				rigid.linearVelocity = Vector3.zero;
				return;
			}
			Vector3 vector = ((base.OwnerSpell != null) ? base.OwnerPoint : (mainDriver ? ((!(independentRotateFollowPoint != Vector3.zero)) ? GetAroundTargetBasePoint() : independentRotateFollowPoint) : ((!(base.OwnerPoint != Vector3.zero)) ? AroundCenter : base.OwnerPoint)));
			float num = 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / GetCurrentSpeed()) * Time.deltaTime * rotateSpeedRatio;
			base.spellAroundOwnerCurrentAngle += num;
			Vector3 vector2 = vector + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
			base.Direction = ToPointDir(vector2);
			base.transform.position = vector2;
			SpellAroundPlayerUpdateMoveTrigger(num);
		}
	}

	protected override void SpellFollowMouse()
	{
		if (base.SIP.spellIsFall)
		{
			base.SpellFollowMouse();
		}
		else
		{
			if (base.isFlyFinish)
			{
				return;
			}
			if (UIMgr.Inst.InputType == PlayerInputType.Gamepad && playerDashSpell == this)
			{
				base.Direction = ControlMgr.Inst.GetInputWASD();
				rigid.linearVelocity = base.Direction * base.spellCfg.speed;
				return;
			}
			float num = Mathf.Clamp(Tool2D.IgnoreZDistance(PlayerMgr.Inst.GetMousePoint(base.transform.position.z), base.transform.position) / toMouseSpeedLerpDistanceThreshHold, 0.6f, 1f);
			if (hitSlowTimeEffectTimer > 0f)
			{
				hitSlowTimeEffectTimer -= Time.deltaTime;
				rigid.linearVelocity = Vector3.zero;
				if (velocityBeforeTimeSlow == Vector3.zero)
				{
					velocityBeforeTimeSlow = rigid.linearVelocity;
				}
				if (hitSlowTimeEffectTimer <= 0f)
				{
					rigid.linearVelocity = velocityBeforeTimeSlow;
					velocityBeforeTimeSlow = Vector3.zero;
				}
			}
			else
			{
				Vector3 mousePoint = PlayerMgr.Inst.GetMousePoint(base.transform.position.z);
				base.Direction = ToPointDir(mousePoint).normalized;
				rigid.linearVelocity = Vector3.Lerp(rigid.linearVelocity, base.Direction * GetCurrentSpeed() * toMouseLerpDistanceSpeedRatio * 1.2f, GetCurrentSpeed() * Time.deltaTime * mouseSpeedRatio * num);
			}
		}
	}

	protected override void SpellFollowTarget()
	{
		if (base.SIP.spellIsFall)
		{
			base.SpellFollowTarget();
		}
		else
		{
			if (base.isFlyFinish)
			{
				return;
			}
			float currentSpeed = GetCurrentSpeed();
			if (hitSlowTimeEffectTimer > 0f)
			{
				hitSlowTimeEffectTimer -= Time.deltaTime;
				rigid.linearVelocity = Vector3.zero;
				if (hitSlowTimeEffectTimer <= 0f)
				{
					rigid.linearVelocity = base.Direction * currentSpeed;
				}
			}
			else if (base.SpellFollowHaveTarget)
			{
				base.Direction = Tool2D.DirMoveTowards(base.Direction, ToPointDir(spellFollowTargetPpt.transform), currentSpeed * spellFollowTargetRotateSpeed * Time.deltaTime * chaseSpeedRatio);
				rigid.linearVelocity = base.Direction * currentSpeed;
			}
		}
	}

	protected override void SpellFollowOwner()
	{
		if (!base.isFlyFinish)
		{
			if (hitSlowTimeEffectTimer > 0f)
			{
				hitSlowTimeEffectTimer -= Time.deltaTime;
				rigid.linearVelocity = Vector3.zero;
				return;
			}
			rigid.linearVelocity = Vector3.zero;
			float t = Mathf.Abs(Mathf.Abs(Tool2D.IgnoreZAngleWithSign(base.Direction, ToPointDir(independentRotateFollowPoint))) - 90f) / 90f;
			base.Direction = Tool2D.DirMoveTowardsTargetInCounterClockWise(base.Direction, ToPointDir(independentRotateFollowPoint), base.CurrentSpeed * spellFollowTargetRotateSpeed * Time.fixedDeltaTime * rotateSpeedRatio);
			float num = 0.4f;
			base.transform.position += base.Direction * base.CurrentSpeed * Mathf.Lerp(1f - num, 1f + num, t) * Time.fixedDeltaTime * rotateSpeedRatio;
		}
	}

	protected override void SpellNormalTransform()
	{
		if (base.SIP.spellIsFall)
		{
			base.SpellNormalTransform();
			return;
		}
		float currentSpeed = GetCurrentSpeed();
		if (hitSlowTimeEffectTimer > 0f)
		{
			hitSlowTimeEffectTimer -= Time.deltaTime;
			rigid.linearVelocity = Vector3.zero;
			if (hitSlowTimeEffectTimer <= 0f)
			{
				rigid.linearVelocity = base.Direction * currentSpeed;
			}
		}
		else if (Mathf.Abs(rigid.linearVelocity.sqrMagnitude - currentSpeed * currentSpeed) > 0.5f)
		{
			rigid.linearVelocity = rigid.linearVelocity.normalized * currentSpeed;
		}
	}

	public override void PoolRecycle()
	{
		DashEnd();
		base.PoolRecycle();
	}

	private void DashEnd()
	{
		if (currentUnitPassenger != null && mainDriver && currentUnitPassenger.unitCfg.unitType == UnitType.Player)
		{
			if (PlayerMgr.Inst.SelectedWand != null)
			{
				PlayerMgr.Inst.SelectedWand.Display_Show(playChoiceAnimation: false);
			}
			PlayerMgr.Inst.PlayerCtrller.NonInteractiveUnregister();
			PlayerMgr.Inst.inDashSpell = false;
			PlayerMgr.Inst.PlayerCtrller.OpenPlayerCollider();
			playerOnboard = false;
			playerDashSpell = null;
			PlayerMgr.Inst.PlayerCtrller.DashOverHeatRegister(GetOverheatTime);
			Collider collider = GeneralTool.HaveCollider(Tool2D.IgnoreZPoint(ownerPpt.transform), ownerPpt.transform.localScale.x * ownerPpt.CC_Self.radius, "Abyss", "Abyss");
			if (!(base.OwnerSpell == null) || !(collider != null))
			{
				currentUnitPassenger.TakeKnockback(rigidVelocityBeforeStop * UnityEngine.Random.Range(0.3f, 0.35f));
			}
			if (base.spellCfg.int1 == 0)
			{
				EffectBase.ManualCreateEffect("DashWave");
			}
			PlaySE("Teleport");
		}
		if (currentUnitPassenger != null && mainDriver && passengerVisualObject != null)
		{
			for (int i = 0; i < passengerVisualObject.Count; i++)
			{
				passengerVisualObject[i].SetActive(value: true);
			}
			passengerVisualObject.Clear();
			currentUnitPassenger.TakeKnockback(rigid.linearVelocity * UnityEngine.Random.Range(0.3f, 0.35f));
			if (base.spellCfg.int1 == 0)
			{
				EffectBase.ManualCreateEffect("DashWave");
			}
			PlaySE("Teleport");
		}
		if (currentSpellPassenger != null)
		{
			spellPassengerList.Remove(base.OwnerSpell);
			currentSpellPassenger = null;
			passengerTransform = null;
			if (base.spellCfg.int1 == 0 || !(base.spellCfg.float1 > 0f) || !mainDriver)
			{
				return;
			}
			List<Collider> list = null;
			EffectBase.ManualCreateEffect("EndHit", base.spellCfg.radius * base.spellCfg.float1);
			PlaySE("Teleport");
			list = ((!IsSameCamp(UnitType.Player)) ? GeneralTool.GetCollidersByTagAndCheckCamp(base.transform.position, base.spellCfg.radius * base.spellCfg.float1, UnitType.Player, true, "Player", "Teammate", "RollBall", "Butterfly", "Spell") : GeneralTool.GetCollidersByTagAndCheckCamp(base.transform.position, base.spellCfg.radius * base.spellCfg.float1, UnitType.Player, false, "Monster", "RollBall", "Butterfly", "Spell"));
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].tag == "Spell")
				{
					SpellBase componentInParent = list[j].GetComponentInParent<SpellBase>();
					if (componentInParent.spellCfg.abilityType != SpellAbilityType.Dash)
					{
						componentInParent.PoolRecycle();
					}
				}
				else if (list[j].tag == "Player" || list[j].tag == "Teammate" || list[j].tag == "Monster")
				{
					UnitProperty component = list[j].GetComponent<UnitProperty>();
					if (component != null)
					{
						OutputDamage(component, new TakeDamageInfo
						{
							canRebound = false,
							knockbackForce = (component.transform.position - base.transform.position).normalized * base.spellCfg.knockback * landForceRatio
						});
					}
				}
				else if (list[j].tag == "RollBall" || list[j].tag == "Butterfly")
				{
					SpellBase componentInParent2 = list[j].GetComponentInParent<SpellBase>();
					if (!componentInParent2.IsSameCamp(this))
					{
						if (componentInParent2.spellCfg.abilityType == SpellAbilityType.Rollball)
						{
							((Spell1002RollBall)componentInParent2).TakeDamage(base.spellCfg.damage);
						}
						else if (componentInParent2.spellCfg.abilityType == SpellAbilityType.Butterfly)
						{
							((Spell1003Butterfly)componentInParent2).HitEFAndRecycle();
						}
						else
						{
							MonoBehaviour.print(componentInParent2.spellCfg.abilityType);
						}
					}
				}
				else
				{
					OutputDamage(list[j].gameObject, new TakeDamageInfo
					{
						canRebound = false
					});
				}
			}
		}
		else
		{
			if (!(currentUnitPassenger != null))
			{
				return;
			}
			unitPassengerList.Remove(currentUnitPassenger);
			currentUnitPassenger.FlyUnregister();
			if (castByPlayerOrTeammate)
			{
				currentUnitPassenger.InvincibleUnregister();
			}
			GeneralTool.HaveCollider(Tool2D.IgnoreZPoint(ownerPpt.transform), ownerPpt.transform.localScale.x * ownerPpt.CC_Self.radius, "Abyss", "Abyss");
			if (base.OwnerSpell == null && AroundCenter != Vector3.zero)
			{
				passengerTransform.position = Tool2D.GetNavMeshPointIngoreZ(base.transform.position);
				PlayerMgr.Inst.PlayerCtrller.CurrentMotion = Vector3.zero;
				base.Direction = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle + 90f).normalized;
				currentUnitPassenger.TakeKnockback(base.Direction.normalized * base.spellCfg.speed * UnityEngine.Random.Range(0.3f, 0.35f));
			}
			if (currentUnitPassenger.unitCfg.unitType == UnitType.Player)
			{
				currentUnitPassenger.SetUnitInvisibleState(state: false);
			}
			if (base.spellCfg.int1 != 0 && base.spellCfg.float1 > 0f && mainDriver && castByPlayerOrTeammate)
			{
				List<Collider> list2 = null;
				EffectBase.ManualCreateEffect("EndHit", base.spellCfg.radius * base.spellCfg.float1);
				PlaySE("Teleport");
				list2 = ((!IsSameCamp(UnitType.Player)) ? GeneralTool.GetCollidersByTagAndCheckCamp(base.transform.position, base.spellCfg.radius * base.spellCfg.float1, UnitType.Player, true, "Player", "Teammate", "RollBall", "Butterfly", "Destructible", "Brittleness", "Spell") : GeneralTool.GetCollidersByTagAndCheckCamp(base.transform.position, base.spellCfg.radius * base.spellCfg.float1, UnitType.Player, false, "Monster", "RollBall", "Butterfly", "Destructible", "Brittleness", "Spell"));
				for (int k = 0; k < list2.Count; k++)
				{
					if (list2[k].tag == "Spell")
					{
						SpellBase componentInParent3 = list2[k].GetComponentInParent<SpellBase>();
						if (componentInParent3.spellCfg.abilityType != SpellAbilityType.Dash)
						{
							componentInParent3.PoolRecycle();
						}
					}
					else if (list2[k].tag == "Player" || list2[k].tag == "Teammate" || list2[k].tag == "Monster")
					{
						UnitProperty component2 = list2[k].GetComponent<UnitProperty>();
						if (component2 != null)
						{
							OutputDamage(component2, new TakeDamageInfo
							{
								canRebound = false,
								knockbackForce = (component2.transform.position - base.transform.position).normalized * base.spellCfg.knockback * landForceRatio
							});
						}
					}
					else if (list2[k].tag == "RollBall" || list2[k].tag == "Butterfly")
					{
						SpellBase componentInParent4 = list2[k].GetComponentInParent<SpellBase>();
						if (!componentInParent4.IsSameCamp(this))
						{
							if (componentInParent4.spellCfg.abilityType == SpellAbilityType.Rollball)
							{
								((Spell1002RollBall)componentInParent4).TakeDamage(base.spellCfg.damage);
							}
							else if (componentInParent4.spellCfg.abilityType == SpellAbilityType.Butterfly)
							{
								((Spell1003Butterfly)componentInParent4).HitEFAndRecycle();
							}
							else
							{
								MonoBehaviour.print(componentInParent4.spellCfg.abilityType);
							}
						}
					}
					else
					{
						OutputDamage(list2[k].gameObject, new TakeDamageInfo
						{
							canRebound = false
						});
					}
				}
			}
			Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(currentUnitPassenger.transform.position);
			currentUnitPassenger.transform.position = navMeshPointIngoreZ;
			currentUnitPassenger = null;
			passengerTransform = null;
		}
	}

	private void SlowTime()
	{
		if (hitSlowTimeEffectTimer < hitSlowTimeEffectDuration)
		{
			hitSlowTimeEffectTimer = hitSlowTimeEffectDuration;
		}
	}

	public void Hijack()
	{
		if (base.OwnerSpell == null && currentUnitPassenger != null && currentUnitPassenger.unitCfg.unitType == UnitType.Player && mainDriver)
		{
			PlayerDashEnd();
			EffectBase.ManualCreateEffect("DashWave");
			PlaySE("Teleport");
		}
		else if (currentUnitPassenger != null && mainDriver && passengerVisualObject != null)
		{
			for (int i = 0; i < passengerVisualObject.Count; i++)
			{
				passengerVisualObject[i].SetActive(value: true);
			}
			passengerVisualObject.Clear();
			currentUnitPassenger.TakeKnockback(rigid.linearVelocity * UnityEngine.Random.Range(0.3f, 0.35f));
			EffectBase.ManualCreateEffect("DashWave");
			PlaySE("Teleport");
		}
		DashEnd();
	}

	public void SetPassengerVisualObject(GameObject target)
	{
		passengerVisualObject.Add(target);
	}

	public void PlayerDashEnd()
	{
		if (base.OwnerSpell == null && currentUnitPassenger != null && currentUnitPassenger.unitCfg.unitType == UnitType.Player && mainDriver)
		{
			if (PlayerMgr.Inst.SelectedWand != null)
			{
				PlayerMgr.Inst.SelectedWand.Display_Show(playChoiceAnimation: false);
			}
			PlayerMgr.Inst.PlayerCtrller.NonInteractiveUnregister();
			PlayerMgr.Inst.inDashSpell = false;
			PlayerMgr.Inst.PlayerCtrller.OpenPlayerCollider();
			currentUnitPassenger.FlyUnregister();
			currentUnitPassenger.InvincibleUnregister();
			playerOnboard = false;
			playerDashSpell = null;
			PlayerMgr.Inst.PlayerCtrller.DashOverHeatRegister(GetOverheatTime);
			Collider collider = GeneralTool.HaveCollider(Tool2D.IgnoreZPoint(ownerPpt.transform), ownerPpt.transform.localScale.x * ownerPpt.CC_Self.radius, "Abyss", "Abyss");
			if (!(base.OwnerSpell == null) || !(collider != null))
			{
				currentUnitPassenger.TakeKnockback(rigid.linearVelocity * UnityEngine.Random.Range(0.3f, 0.35f));
			}
			currentUnitPassenger.SetUnitInvisibleState(state: false);
			currentUnitPassenger.gameObject.transform.position = Tool2D.IgnoreZPoint(currentUnitPassenger.gameObject.transform.position);
			unitPassengerList.Remove(currentUnitPassenger);
			currentUnitPassenger = null;
		}
	}

	public void OnDisable()
	{
		if (currentUnitPassenger != null && mainDriver && passengerVisualObject != null)
		{
			for (int i = 0; i < passengerVisualObject.Count; i++)
			{
				passengerVisualObject[i].SetActive(value: true);
			}
			passengerVisualObject.Clear();
		}
		if (currentSpellPassenger != null)
		{
			spellPassengerList.Remove(base.OwnerSpell);
			currentSpellPassenger = null;
			passengerTransform = null;
		}
		else
		{
			if (!(currentUnitPassenger != null))
			{
				return;
			}
			if (base.OwnerSpell == null && currentUnitPassenger != null && currentUnitPassenger.unitCfg.unitType == UnitType.Player && mainDriver)
			{
				if (PlayerMgr.Inst.SelectedWand != null)
				{
					PlayerMgr.Inst.SelectedWand.Display_Show(playChoiceAnimation: false);
				}
				PlayerMgr.Inst.PlayerCtrller.NonInteractiveUnregister();
				PlayerMgr.Inst.inDashSpell = false;
				PlayerMgr.Inst.PlayerCtrller.OpenPlayerCollider();
				currentUnitPassenger.FlyUnregister();
				currentUnitPassenger.InvincibleUnregister();
				playerOnboard = false;
				playerDashSpell = null;
				PlayerMgr.Inst.PlayerCtrller.DashOverHeatRegister(GetOverheatTime);
				Collider collider = GeneralTool.HaveCollider(Tool2D.IgnoreZPoint(ownerPpt.transform), ownerPpt.transform.localScale.x * ownerPpt.CC_Self.radius, "Abyss", "Abyss");
				if (!(base.OwnerSpell == null) || !(collider != null))
				{
					currentUnitPassenger.TakeKnockback(rigid.linearVelocity * UnityEngine.Random.Range(0.3f, 0.35f));
				}
				currentUnitPassenger.SetUnitInvisibleState(state: false);
				unitPassengerList.Remove(currentUnitPassenger);
				currentUnitPassenger = null;
			}
			unitPassengerList.Remove(currentUnitPassenger);
			currentUnitPassenger = null;
			passengerTransform = null;
			ownerPpt.FlyUnregister();
			ownerPpt.InvincibleUnregister();
		}
	}

	private void HideAllTeammateOwner()
	{
		foreach (GameObject item in passengerVisualObject)
		{
			item.SetActive(value: false);
		}
	}

	public void OnLaunchFromUnitNotPlayer(UnitBase unit)
	{
		base.OwnerTsf = base.transform;
		base.OwnerPoint = base.transform.position;
		if (!base.SIP.spellIsFall)
		{
			if (unit is Teammate5 teammate)
			{
				SetPassengerVisualObject(teammate.visualObject.transform.Find("Motion").gameObject);
				SetPassengerVisualObject(teammate.visualObject.transform.Find("LoopVF").gameObject);
				SetPassengerVisualObject(teammate.visualObject.transform.Find("ProgressBar").gameObject);
				SetPassengerVisualObject(teammate.selfShadow.ShadowGO);
				HideAllTeammateOwner();
				independentRotateFollowPoint = teammate.transform.position;
				teammate.currentCastingSpell.Add((this, Teammate5.T5ShootPointPos.middle));
			}
			else if (unit is Teammate52 teammate2)
			{
				ownerPpt = teammate2.myPpt;
				independentRotateFollowPoint = teammate2.transform.position;
				SetPassengerVisualObject(teammate2.visualObject);
				SetPassengerVisualObject(teammate2.GetComponent<Shadow>().ShadowGO);
				HideAllTeammateOwner();
			}
		}
	}

	protected override void OnFallingGround()
	{
		SpellEffectBase effectBase = EffectBase;
		float? size = GetFallingGroundDamageRadius();
		effectBase.CreateSpriteEffect("Hit", null, null, size);
		EffectBase.ManualCreateEffect("EndHit", position: Tool2D.IgnoreZPoint(base.transform.position), scale: GetFallingGroundDamageRadius());
		EffectBase.PlayFallingGroundSound();
		MakeFallingGroundDamageToAround();
		CamController.Inst.SetShock(fallShockParam);
		OnFallingGroundTryReboundOrRecycle();
	}
}
