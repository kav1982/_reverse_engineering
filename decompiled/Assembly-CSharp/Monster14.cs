using System;
using Unity.Entities;
using UnityEngine;

public class Monster14 : UnitBase
{
	private enum UnitState
	{
		BornIdle,
		Idle,
		IdleWalk,
		EyeOpen,
		FollowTarget,
		MoveToTargetStop,
		AttackBefore,
		Attack,
		AttackIdle,
		Hanging,
		HangDown
	}

	[Space(50f)]
	public float checkTargetInterval;

	public float checkTargetDistance;

	public VariableFloat idleTime;

	public VariableFloat idleWalkRadius;

	public float idleWalkTime;

	[Header("Eye")]
	public Transform tsf_Eye2;

	public Transform tsf_Eye3;

	public float eyeOffset;

	public float eyeLerp;

	[Header("Leg")]
	public int legCount;

	public GameObject pfb_Leg;

	public Transform tsf_Motion;

	[Header("Follow")]
	public float followSpeedRatio;

	public VariableFloat followTimeToAction;

	[Header("Attack")]
	public float attackBeforeTime;

	public float attackBeforeMotionExtraHeight;

	public float attackAngle;

	public float attackRotateSpeed;

	public float attackInterval;

	public float attackMotionOffset;

	public float attackMotionLerp;

	public float attackIdleTime;

	[Header("Hang")]
	[Range(0f, 1f)]
	public float hangHpRatio;

	[Range(0f, 1f)]
	public float hangChange;

	public float hangSpeedRatio;

	public float hangHeight;

	public float hangBeforeTime;

	public float hangingTime;

	public float hangDownTime;

	public float hangAttackDistace;

	public float hangAttackInterval;

	[Header("Spell1")]
	public float spellExtraHeight;

	public float spellSpeed;

	public float spellUpSpeed;

	public float spellDuration;

	public float spellGravity;

	public int spellDamage;

	[Header("Spell2")]
	public float spell2ExtraHeight;

	public float spell2Speed;

	public float spell2UpSpeed;

	public float spell2Gravity;

	[Header("safe mode")]
	public MeshRenderer eye1;

	public MeshRenderer eye2;

	public MeshRenderer eye3;

	public Sprite eye1Origin;

	public Sprite eye2Origin;

	public Sprite eye3Origin;

	public Sprite eye1Safe;

	public Sprite eye2Safe;

	public Sprite eye3Safe;

	private SpellSpawnParams ssp1;

	private SpellSpawnParams ssp2;

	private Monster14_Leg[] legs;

	private UnitState state;

	private bool isEyeOpen;

	private float idleTimer;

	private float walkTimer;

	private float attackBeforeTimer;

	private float followToAttackTimer;

	private Vector3 attackDir;

	private bool attackSmallToLarge;

	private float attackAngleCounter;

	private float attackIntervalTimer;

	private float attackIdleTimer;

	private float hangTimer;

	private float hangAttackIntervalTimer;

	public float MoveRatio
	{
		get
		{
			if (state == UnitState.FollowTarget)
			{
				return followSpeedRatio;
			}
			if (state == UnitState.Hanging)
			{
				return hangSpeedRatio;
			}
			return 1f;
		}
	}

	public Vector3 OriginalMotionLocalPoint { get; private set; }

	public Vector3 CurrentMotionLocalPoint { get; private set; }

	public bool IsHang { get; private set; }

	private void OnEnable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Combine(EventMgr.SafeModeStateChange, new Action(SetSafeMode));
		SetSafeMode();
	}

	private void OnDisable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Remove(EventMgr.SafeModeStateChange, new Action(SetSafeMode));
	}

	public void SetSafeMode()
	{
		if (DataMgr.settingData.SafeMode)
		{
			eye1.material.SetTexture(GameConstManaged.shaderTextureIndex, eye1Safe.texture);
			eye2.material.SetTexture(GameConstManaged.shaderTextureIndex, eye2Safe.texture);
			eye3.material.SetTexture(GameConstManaged.shaderTextureIndex, eye3Safe.texture);
		}
		else
		{
			eye1.material.SetTexture(GameConstManaged.shaderTextureIndex, eye1Origin.texture);
			eye2.material.SetTexture(GameConstManaged.shaderTextureIndex, eye2Origin.texture);
			eye3.material.SetTexture(GameConstManaged.shaderTextureIndex, eye3Origin.texture);
		}
	}

	public override void SingleInitialCallback()
	{
		OriginalMotionLocalPoint = tsf_Motion.localPosition;
		legs = new Monster14_Leg[legCount];
		Debug.Log(legCount);
		for (int i = 0; i < legCount; i++)
		{
			legs[i] = UnityEngine.Object.Instantiate(pfb_Leg, base.transform.position, Quaternion.identity, base.transform).GetComponent<Monster14_Leg>();
			legs[i].SingleInitial(this);
		}
		ssp1 = UnitDotsSyncSystem.GetSpellPrototype(90011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp1);
		sSPModifier.Damage = spellDamage;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Gravity = 0f - spellGravity;
		sSPModifier.CurrentFallSpeed = 0f - spellUpSpeed;
		sSPModifier.Speed = spellSpeed;
		sSPModifier.SplitCount = 3;
		sSPModifier.SplitDamageRatio = 0.33f;
		sSPModifier.ApplyToSSP(ref ssp1);
		ssp2 = UnitDotsSyncSystem.GetSpellPrototype(90011);
		sSPModifier.Gravity = 0f - spell2Gravity;
		sSPModifier.CurrentFallSpeed = 0f - spell2UpSpeed;
		sSPModifier.Speed = spell2Speed;
		sSPModifier.ApplyToSSP(ref ssp2);
	}

	public override void EveryInitialCallback()
	{
		state = UnitState.BornIdle;
		isEyeOpen = false;
		idleTimer = 0f;
		walkTimer = 0f;
		attackBeforeTimer = 0f;
		followToAttackTimer = 0f;
		attackAngleCounter = 0f;
		attackIntervalTimer = 0f;
		attackIdleTimer = 0f;
		hangTimer = 0f;
		hangAttackIntervalTimer = 0f;
		myPpt.CC_Self.enabled = true;
		SetDotsCCEnable(isOpen: true);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = true;
		SetComponentData(componentData);
		IsHang = false;
		CurrentMotionLocalPoint = OriginalMotionLocalPoint;
		followTimeToAction.RandomResult();
		Debug.Log(legs.Length);
		for (int i = 0; i < legs.Length; i++)
		{
			legs[i].EveryInitial();
		}
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (isEyeOpen)
		{
			if (base.HaveTarget && state != UnitState.EyeOpen)
			{
				Vector3 vector = ToTargetDir() * eyeOffset;
				tsf_Eye2.localPosition = Vector3.Lerp(tsf_Eye2.localPosition, new Vector3(vector.x, vector.y, tsf_Eye2.localPosition.z), eyeLerp * Time.deltaTime);
				tsf_Eye3.localPosition = Vector3.Lerp(tsf_Eye3.localPosition, new Vector3(vector.x, vector.y, tsf_Eye3.localPosition.z), eyeLerp * Time.deltaTime);
			}
			else
			{
				tsf_Eye2.localPosition = Vector3.Lerp(tsf_Eye2.localPosition, new Vector3(0f, 0f, tsf_Eye2.localPosition.z), eyeLerp * Time.deltaTime);
				tsf_Eye3.localPosition = Vector3.Lerp(tsf_Eye3.localPosition, new Vector3(0f, 0f, tsf_Eye3.localPosition.z), eyeLerp * Time.deltaTime);
			}
		}
		tsf_Motion.localPosition = Tool2D.GetLayerPoint(CurrentMotionLocalPoint);
		switch (state)
		{
		case UnitState.BornIdle:
			SetMove(Vector3.zero);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = UnitState.Idle;
				idleTime.RandomResult();
			}
			break;
		case UnitState.Idle:
			SetMove(Vector3.zero);
			idleTimer += Time.deltaTime;
			if (idleTimer >= idleTime.result)
			{
				idleTimer = 0f;
				idleTime.RandomResult();
				state = UnitState.IdleWalk;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
			}
			CheckTarget();
			break;
		case UnitState.IdleWalk:
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			walkTimer += Time.deltaTime;
			if (walkTimer > idleWalkTime)
			{
				walkTimer = 0f;
				state = UnitState.Idle;
			}
			CheckTarget();
			break;
		case UnitState.EyeOpen:
			SetMove(Vector3.zero);
			break;
		case UnitState.FollowTarget:
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = UnitState.Idle;
				break;
			}
			GetNavInfo(base.TargetPoint);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * followSpeedRatio);
			followToAttackTimer += Time.deltaTime;
			if (!(followToAttackTimer >= followTimeToAction.result))
			{
				break;
			}
			followToAttackTimer = 0f;
			followTimeToAction.RandomResult();
			if (base.CurrentHPRatio <= hangHpRatio && UnityEngine.Random.value <= hangChange)
			{
				state = UnitState.Hanging;
				IsHang = true;
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
				UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
				componentData3.CanTouch = false;
				SetComponentData(componentData3);
			}
			else
			{
				state = UnitState.AttackBefore;
				CurrentMotionLocalPoint = OriginalMotionLocalPoint;
				for (int j = 0; j < legs.Length; j++)
				{
					legs[j].CanChangeState(canChange: false);
				}
			}
			break;
		case UnitState.AttackBefore:
			SetMove(Vector3.zero);
			CurrentMotionLocalPoint = Vector3.Lerp(CurrentMotionLocalPoint, OriginalMotionLocalPoint + new Vector3(0f, 0f, 0f - attackBeforeMotionExtraHeight), attackMotionLerp * Time.deltaTime);
			tsf_Motion.localPosition = Tool2D.GetLayerPoint(CurrentMotionLocalPoint);
			attackBeforeTimer += Time.deltaTime;
			if (!(attackBeforeTimer >= attackBeforeTime))
			{
				break;
			}
			attackBeforeTimer = 0f;
			if (base.HaveTarget)
			{
				state = UnitState.Attack;
				attackDir = ToTargetDir();
				attackSmallToLarge = ((UnityEngine.Random.Range(0, 2) == 0) ? true : false);
				if (attackSmallToLarge)
				{
					attackAngleCounter = (0f - attackAngle) / 2f;
				}
				else
				{
					attackAngleCounter = attackAngle / 2f;
				}
			}
			else
			{
				state = UnitState.Idle;
			}
			break;
		case UnitState.Attack:
		{
			SetMove(Vector3.zero);
			Vector3 dir = Tool2D.GetDir(attackDir, attackAngleCounter);
			Vector3 b = OriginalMotionLocalPoint + new Vector3(0f, 0f, 0f - attackBeforeMotionExtraHeight) + dir * attackMotionOffset;
			CurrentMotionLocalPoint = Vector3.Lerp(CurrentMotionLocalPoint, b, attackMotionLerp * Time.deltaTime);
			tsf_Motion.localPosition = Tool2D.GetLayerPoint(CurrentMotionLocalPoint);
			attackIntervalTimer += Time.deltaTime;
			if (attackIntervalTimer >= attackInterval)
			{
				attackIntervalTimer = 0f;
				UnitSpellModifier sSPModifier2 = UnitBase.GetSSPModifier(in ssp1);
				sSPModifier2.SpawnPosition = base.transform.position + CurrentMotionLocalPoint + new Vector3(0f, 0f, 0f - spellExtraHeight);
				sSPModifier2.Direction = dir;
				sSPModifier2.ApplyToSSP(ref ssp1);
				ShootSpell(ssp1);
			}
			if (attackSmallToLarge)
			{
				attackAngleCounter += attackRotateSpeed * Time.deltaTime;
				if (attackAngleCounter >= attackAngle / 2f)
				{
					state = UnitState.AttackIdle;
				}
			}
			else
			{
				attackAngleCounter -= attackRotateSpeed * Time.deltaTime;
				if (attackAngleCounter <= (0f - attackAngle) / 2f)
				{
					state = UnitState.AttackIdle;
				}
			}
			break;
		}
		case UnitState.AttackIdle:
			SetMove(Vector3.zero);
			CurrentMotionLocalPoint = Vector3.Lerp(CurrentMotionLocalPoint, OriginalMotionLocalPoint, attackMotionLerp * Time.deltaTime);
			tsf_Motion.localPosition = Tool2D.GetLayerPoint(CurrentMotionLocalPoint);
			attackIdleTimer += Time.deltaTime;
			if (attackIdleTimer >= attackIdleTime)
			{
				attackIdleTimer = 0f;
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = UnitState.FollowTarget;
				}
				else
				{
					state = UnitState.IdleWalk;
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
				}
				for (int i = 0; i < legs.Length; i++)
				{
					legs[i].CanChangeState(canChange: true);
				}
			}
			break;
		case UnitState.Hanging:
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				hangTimer = 0f;
				state = UnitState.HangDown;
				IsHang = false;
				myPpt.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanTouch = true;
				SetComponentData(componentData);
				break;
			}
			CurrentMotionLocalPoint = Vector3.Lerp(CurrentMotionLocalPoint, OriginalMotionLocalPoint + new Vector3(0f, 0f, 0f - hangHeight), attackMotionLerp * Time.deltaTime);
			checkTargetIntervalTimer += 1f;
			if (checkTargetIntervalTimer > 0.1f)
			{
				GetNavInfo(base.TargetPoint);
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(base.TargetPoint);
			}
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * hangSpeedRatio);
			hangAttackIntervalTimer += Time.deltaTime;
			if (hangAttackIntervalTimer >= hangAttackInterval && ToTargetDistanceSqr() < hangAttackDistace * hangAttackDistace)
			{
				hangAttackIntervalTimer = 0f;
				UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp2);
				sSPModifier.Speed = UnityEngine.Random.Range(0f, spell2Speed);
				sSPModifier.Direction = ToTargetDir();
				sSPModifier.SpawnPosition = base.transform.position + CurrentMotionLocalPoint + new Vector3(0f, 0f, 0f - spell2ExtraHeight);
				sSPModifier.ApplyToSSP(ref ssp2);
				ShootSpell(ssp2);
			}
			hangTimer += Time.deltaTime;
			if (hangTimer >= hangingTime)
			{
				hangTimer = 0f;
				state = UnitState.HangDown;
				IsHang = false;
				SetDotsCCEnable(isOpen: true);
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.CanTouch = true;
				SetComponentData(componentData2);
			}
			break;
		case UnitState.HangDown:
			SetMove(Vector3.zero);
			CurrentMotionLocalPoint = Vector3.Lerp(CurrentMotionLocalPoint, OriginalMotionLocalPoint, attackMotionLerp * Time.deltaTime);
			hangTimer += Time.deltaTime;
			if (hangTimer >= hangDownTime)
			{
				hangTimer = 0f;
				CurrentMotionLocalPoint = OriginalMotionLocalPoint;
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = UnitState.FollowTarget;
				}
				else
				{
					state = UnitState.IdleWalk;
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	private void CheckTarget()
	{
		checkTargetIntervalTimer += Time.deltaTime;
		if (!(checkTargetIntervalTimer >= checkTargetInterval))
		{
			return;
		}
		checkTargetIntervalTimer = 0f;
		GetNearestTarget();
		if (base.HaveTarget && ToTargetDistanceSqr() < checkTargetDistance * checkTargetDistance)
		{
			if (isEyeOpen)
			{
				state = UnitState.FollowTarget;
				return;
			}
			isEyeOpen = true;
			base.Anima.SetTrigger("EyeOpen");
			state = UnitState.EyeOpen;
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "EyeOpenFinish")
		{
			state = UnitState.FollowTarget;
		}
		else
		{
			Debug.LogError(animaName);
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (IsHang)
		{
			info.immuneDamage = true;
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if ((state == UnitState.BornIdle || state == UnitState.Idle || state == UnitState.IdleWalk) && info.attackerEntity != Entity.Null && GetComponentData<UnitProperty_Dots>().unitCfg.IsSameCamp(UnitType.Player))
		{
			targetEntity = info.attackerEntity;
			if (isEyeOpen)
			{
				state = UnitState.FollowTarget;
				return;
			}
			isEyeOpen = true;
			base.Anima.SetTrigger("EyeOpen");
			state = UnitState.EyeOpen;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Spell, 10201), base.transform.position);
		if (LevelMgr.Inst.CurrentRoomCtrller.IsFinish)
		{
			LevelMgr.Inst.CurrentRoomCtrller.AllAccessOpen();
		}
	}

	public override void Theme6Reposition(Vector3 changeValue)
	{
		base.Theme6Reposition(changeValue);
		for (int i = 0; i < legs.Length; i++)
		{
			legs[i].Theme6Reposition(changeValue);
		}
	}
}
