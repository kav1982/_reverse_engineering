using System;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Monster1 : UnitBase
{
	public enum UnitState
	{
		BornIdle,
		Idle,
		IdleWalk,
		MoveToTarget,
		Attack,
		PermanentAttack,
		LongAttackAfter,
		Fly,
		JumpPrepare,
		Jump
	}

	[Space(50f)]
	public VariableFloat idleTime;

	public VariableFloat idleWalkRadius;

	public float walkTime;

	public float getTargetDistance;

	[Range(0f, 1f)]
	public float walkToTargetChance;

	[Header("Leg")]
	public Monster1_Leg pfb_Monster1Leg;

	public Transform tsf_Motion;

	[Header("Eye")]
	public Transform tsf_Eye;

	public float eyeOffset;

	public float eyeMoveLerp;

	[Header("Pattern2 pattern3")]
	public AIPattern pattern;

	public float attackDistance;

	[Header("Pattern 4,5")]
	public float jumpSpeed;

	public float jumpUpSpeed;

	public float gravity;

	public VariableFloat jumpCheckTime;

	private float jumpCheckTimer;

	public float legReleaseHeight;

	public LayerMask jumpMask;

	private Vector3 targetLastPosition;

	[Header("Pattern 6")]
	public float bulletOffset;

	[Range(0f, 1f)]
	public float attackTargetChance;

	public float longAttackTime;

	public float longAttackMoveRatio;

	[Header("Spell")]
	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	[Header("安全模式")]
	public bool isSafeMode = true;

	public Animator Anima1;

	public Transform tsf_MotionH;

	[Header("状态机")]
	public UnitState _state;

	private float stateExistTime;

	private bool stateQuit;

	private bool changedState;

	[Header("和谐")]
	public MeshRenderer renderer_H;

	public UnityEngine.Material mainMaterial_H;

	public UnityEngine.Material material_H;

	private SpellSpawnParams ssp;

	private Monster1_Leg[] legs;

	public UnitState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
		}
	}

	public bool IsMove
	{
		get
		{
			if (state == UnitState.IdleWalk || state == UnitState.MoveToTarget || state == UnitState.PermanentAttack)
			{
				return true;
			}
			return false;
		}
	}

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
			tsf_MotionH.gameObject.SetActive(value: true);
			tsf_Motion.gameObject.SetActive(value: false);
		}
		else
		{
			tsf_MotionH.gameObject.SetActive(value: false);
			tsf_Motion.gameObject.SetActive(value: true);
		}
	}

	public override void SingleInitialCallback()
	{
		legs = new Monster1_Leg[6];
		for (int i = 0; i < legs.Length; i++)
		{
			legs[i] = UnityEngine.Object.Instantiate(pfb_Monster1Leg, base.transform);
			float degree = 0f;
			switch (i)
			{
			case 0:
				degree = 45f;
				break;
			case 1:
				degree = 90f;
				break;
			case 2:
				degree = 135f;
				break;
			case 3:
				degree = 225f;
				break;
			case 4:
				degree = 270f;
				break;
			case 5:
				degree = 300f;
				break;
			}
			legs[i].SingleInitial(this, Tool2D.GetDir(degree));
			if (GameMgr.IsHarmony_Static && material_H != null)
			{
				UnityEngine.Object.Destroy(legs[i].lr_Leg.material);
				legs[i].lr_Leg.material = material_H;
			}
		}
		if (GameMgr.IsHarmony_Static && renderer_H != null)
		{
			UnityEngine.Object.Destroy(renderer_H.material);
			renderer_H.material = mainMaterial_H;
		}
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Damage = spellDamage;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Shooter = myPpt.myEntity;
		if (pattern == AIPattern.Pattern5 || pattern == AIPattern.Pattern8)
		{
			ssp.ElementComponentData.VenomDuration = 3f;
			ssp.ElementComponentData.VenomApplyCount = 2f;
			sSPModifier.ColorType = SpellColorType.Venom;
		}
		sSPModifier.ApplyToSSP(ref ssp);
	}

	public override void EveryInitialCallback()
	{
		idleTime.RandomResult();
		state = UnitState.BornIdle;
		jumpCheckTimer = 0f;
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
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += Time.deltaTime;
		if (tsf_Eye != null)
		{
			float x = 0f;
			if (base.HaveTarget)
			{
				x = (Vector3.Angle(ToTargetDir(), Vector3.left) / 180f - 0.5f) * 2f * eyeOffset;
			}
			tsf_Eye.localPosition = Vector3.Lerp(tsf_Eye.localPosition, new Vector3(x, tsf_Eye.localPosition.y, tsf_Eye.localPosition.z), eyeMoveLerp * Time.deltaTime);
		}
		switch (state)
		{
		case UnitState.BornIdle:
			if (changedState)
			{
				Anima1.SetTrigger("Idle");
				base.Anima.SetTrigger("Idle");
			}
			SetMove(Vector3.zero);
			if (stateExistTime >= 0.5f)
			{
				state = UnitState.Idle;
			}
			break;
		case UnitState.Idle:
		{
			if (changedState)
			{
				Anima1.SetTrigger("Idle");
				base.Anima.SetTrigger("Idle");
			}
			SetMove(Vector3.zero);
			if (!(stateExistTime >= idleTime.result))
			{
				break;
			}
			idleTime.RandomResult();
			float value = UnityEngine.Random.value;
			switch (pattern)
			{
			case AIPattern.Pattern1:
			case AIPattern.Pattern4:
			case AIPattern.Pattern7:
				GetNearestTarget();
				if (base.HaveTarget && value <= walkToTargetChance && ToTargetDistanceSqr() < getTargetDistance * getTargetDistance)
				{
					state = UnitState.MoveToTarget;
				}
				else
				{
					state = UnitState.IdleWalk;
				}
				break;
			case AIPattern.Pattern2:
			case AIPattern.Pattern3:
			case AIPattern.Pattern5:
			case AIPattern.Pattern6:
			case AIPattern.Pattern8:
			case AIPattern.Pattern9:
				if (value <= walkToTargetChance)
				{
					GetNearestTarget();
					if (base.HaveTarget)
					{
						state = UnitState.MoveToTarget;
						break;
					}
					state = UnitState.IdleWalk;
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
				}
				else if (value < walkToTargetChance + attackTargetChance)
				{
					GetNearestTarget(checkWall: true);
					if (base.HaveTarget && ToTargetDistance() < attackDistance)
					{
						state = UnitState.Attack;
					}
					else
					{
						state = UnitState.IdleWalk;
					}
				}
				else
				{
					state = UnitState.IdleWalk;
				}
				break;
			default:
				Debug.LogError(pattern);
				break;
			}
			break;
		}
		case UnitState.IdleWalk:
			if (changedState)
			{
				Anima1.SetTrigger("Move");
				base.Anima.SetTrigger("Move");
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
			}
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			if (stateExistTime > walkTime)
			{
				state = UnitState.Idle;
			}
			break;
		case UnitState.MoveToTarget:
			if (changedState)
			{
				Anima1.SetTrigger("Move");
				base.Anima.SetTrigger("Move");
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = UnitState.IdleWalk;
				break;
			}
			GetNavInfo(base.TargetPoint);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			if (stateExistTime > walkTime)
			{
				state = UnitState.Idle;
			}
			else
			{
				JumpCheck();
			}
			break;
		case UnitState.Attack:
			if (changedState)
			{
				if (pattern == AIPattern.Pattern2 || pattern == AIPattern.Pattern5 || pattern == AIPattern.Pattern8)
				{
					Anima1.SetTrigger("Attack");
					base.Anima.SetTrigger("Attack");
				}
				else
				{
					Anima1.SetTrigger("LongAttackBefore");
					base.Anima.SetTrigger("LongAttackBefore");
				}
			}
			SetMove(Vector3.zero);
			break;
		case UnitState.PermanentAttack:
			if (changedState)
			{
				Anima1.SetTrigger("LongAttack");
				base.Anima.SetTrigger("LongAttack");
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget(checkWall: true);
			}
			if (!base.HaveTarget)
			{
				state = UnitState.LongAttackAfter;
				break;
			}
			if (base.HaveTarget)
			{
				GetNavInfo(base.TargetPoint);
			}
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * longAttackMoveRatio);
			if (stateExistTime >= longAttackTime)
			{
				state = UnitState.LongAttackAfter;
			}
			break;
		case UnitState.LongAttackAfter:
			if (changedState)
			{
				Anima1.SetTrigger("LongAttackOver");
				base.Anima.SetTrigger("LongAttackOver");
			}
			break;
		case UnitState.JumpPrepare:
			if (changedState)
			{
				Anima1.SetTrigger("Jump");
				base.Anima.SetTrigger("Jump");
			}
			SetMove(Vector3.zero);
			if (base.HaveTarget)
			{
				targetLastPosition = base.TargetPointIgnoreZ;
			}
			break;
		case UnitState.Jump:
			SetMove(Vector3.zero);
			if (base.transform.position.z < 0f - legReleaseHeight)
			{
				for (int j = 0; j < legs.Length; j++)
				{
					legs[j].SetJumpRelease();
				}
			}
			if (base.transform.position.z > 0f)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform);
				LocalTransform componentData2 = GetComponentData<LocalTransform>();
				componentData2.Position = base.transform.position;
				SetComponentData(componentData2);
				state = UnitState.Idle;
				JumpStop_Dots();
				for (int k = 0; k < legs.Length; k++)
				{
					legs[k].StopFly();
				}
			}
			break;
		case UnitState.Fly:
			if (base.transform.position.z > 0f)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform);
				LocalTransform componentData = GetComponentData<LocalTransform>();
				componentData.Position = base.transform.position;
				SetComponentData(componentData);
				state = UnitState.Idle;
				JumpStop_Dots();
				for (int i = 0; i < legs.Length; i++)
				{
					legs[i].StopFly();
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Attack":
			if (base.HaveTarget)
			{
				UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
				if (pattern == AIPattern.Pattern2 || pattern == AIPattern.Pattern3)
				{
					sSPModifier.Direction = ToTargetDir();
					sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
				}
				else if (pattern == AIPattern.Pattern5)
				{
					sSPModifier.Direction = ToTargetDir();
					sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
				}
				else if (pattern == AIPattern.Pattern8)
				{
					sSPModifier.Direction = ToTargetDir(-5f);
					sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
					sSPModifier.Direction = ToTargetDir(5f);
					sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
				}
				else if (pattern == AIPattern.Pattern6)
				{
					sSPModifier.Direction = ToTargetDir();
					sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight) + ToTargetDir(-90f) * bulletOffset;
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
					sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight) + ToTargetDir(90f) * bulletOffset;
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
				}
				else if (pattern == AIPattern.Pattern9)
				{
					sSPModifier.Direction = ToTargetDir();
					sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight) + ToTargetDir() * bulletOffset;
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
					sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight) + ToTargetDir(120f) * bulletOffset;
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
					sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight) + ToTargetDir(240f) * bulletOffset;
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
				}
			}
			break;
		case "AttackFinish":
			state = UnitState.Idle;
			break;
		case "LongAttackBeforeFinish":
			state = UnitState.PermanentAttack;
			break;
		case "LongAttackOverFinish":
			state = UnitState.Idle;
			break;
		case "JumpStarted":
		{
			Vector3 point = targetLastPosition;
			if (UnitDotsSyncSystem.Raycast(base.transform.position, targetLastPosition - base.transform.position, Vector3.Distance(targetLastPosition, base.transform.position), GameConst.Filter_Wall, out var result))
			{
				point = result.point;
			}
			NormalJump(Mathf.Min(Vector3.Distance(base.transform.position, point), jumpSpeed) * ToPointDir(point), jumpUpSpeed, gravity);
			break;
		}
		default:
			Debug.LogError(animaName);
			break;
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

	public void JumpCheck()
	{
		if (pattern == AIPattern.Pattern4 || pattern == AIPattern.Pattern5 || pattern == AIPattern.Pattern7 || pattern == AIPattern.Pattern8)
		{
			jumpCheckTimer += Time.deltaTime;
			if (jumpCheckTimer > jumpCheckTime.result)
			{
				jumpCheckTimer = 0f;
				jumpCheckTime.RandomResult();
				state = UnitState.JumpPrepare;
			}
		}
	}

	public void NormalJump(Vector3 forwardForce, float upForce, float gravity)
	{
		GetNearestTarget();
		Vector3 vector = ((!base.HaveTarget) ? Tool2D.GetNavMeshPointIngoreZ(base.transform.position + forwardForce.normalized) : Tool2D.GetNavMeshPointIngoreZ(base.transform.position + forwardForce));
		float num = GeneralTool.CannonSpeed(upForce, 0f, gravity, Vector3.Distance(base.transform.position, vector));
		base.Rigid.linearVelocity = ToPointDir(vector) * num;
		PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
		componentData.Linear = base.Rigid.linearVelocity;
		SetComponentData(componentData);
		JumpStart_Dots(upForce, gravity);
		state = UnitState.Jump;
		for (int i = 0; i < legs.Length; i++)
		{
			legs[i].SetJump();
		}
	}

	public void BornJump(Vector3 forwardForce, float upForce, float gravity)
	{
		base.Rigid.linearVelocity = forwardForce;
		PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
		componentData.Linear = base.Rigid.linearVelocity;
		SetComponentData(componentData);
		JumpStart_Dots(upForce, gravity);
		state = UnitState.Fly;
		for (int i = 0; i < legs.Length; i++)
		{
			legs[i].SetFly();
		}
	}
}
