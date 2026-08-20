using Spine;
using Unity.Physics;
using UnityEngine;

public class Monster52 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		JumpPrepare,
		Jump,
		Attack,
		MoveToTarget
	}

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("舌头")]
	public Monster51_Tongue tongue;

	[Header("随机移动")]
	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	[Header("站立")]
	public VariableFloat idleTime;

	[Header("索敌")]
	public float sight;

	public VariableFloat rotateCloseAngleRange;

	[Header("跳跃")]
	public float jumpPrepareTime;

	public VariableFloat jumpTime;

	public VariableFloat maxJumpDistance;

	public float gravity;

	public float jumpKeepDistance;

	private Vector3 jumpDiration;

	[Header("攻击")]
	public VariableFloat attackCD;

	public float attackSpeed;

	private float attackCDTimer;

	public float attackExtraOffset;

	public ShockParam shockParam;

	public float waveOffsetX;

	[Header("墙壁检查")]
	private Vector3 blockPoint;

	[Header("二模式")]
	public float multiAttackChance;

	public AIPattern pattern;

	public float attackAngle;

	public int multiAttackTime;

	private int multiAttackTimer;

	private Vector3 _destnation;

	public MonsterState state
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

	private Vector3 destination
	{
		get
		{
			if (base.HaveTarget)
			{
				return base.TargetPointIgnoreZ;
			}
			return _destnation;
		}
	}

	private void GetDistnation()
	{
		Vector3 a = LevelMgr.Inst.CurrentRoomCtrller.RoomScale;
		Vector3 vector = (_destnation = Tool2D.IgnoreZPoint(Tool2D.GetNavMeshPoint(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + Vector3.Scale(a, new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f)))));
	}

	protected override void SetFlip(float motionX)
	{
		for (int i = 0; i < myPpt.SR_Models.Length; i++)
		{
			myPpt.SR_Models[i].flipX = motionX < 0f;
			if (myPpt.SR_Models[i] != tongue.mainRenderer)
			{
				myPpt.SR_Models[i].material.SetFloat(GameConstManaged.shaderFlipXIndex, (!myPpt.SR_Models[i].flipX) ? 1 : (-1));
			}
		}
		base.SAnima.transform.localScale = new Vector3(Mathf.Abs(base.SAnima.transform.localScale.x) * (float)((!(motionX <= 0f)) ? 1 : (-1)), base.SAnima.transform.localScale.y, base.SAnima.transform.localScale.z);
	}

	public bool WallBlocked(bool usePosition = false, Vector3 startPosition = default(Vector3), Vector3 endPosition = default(Vector3))
	{
		Vector3 vector = base.transform.position;
		Vector3 position = destination;
		if (usePosition)
		{
			vector = startPosition;
			position = endPosition;
		}
		UnityEngine.Ray ray = new UnityEngine.Ray(vector, Tool2D.IgnoreZV2ToV1Normal(destination, vector));
		if (UnitDotsSyncSystem.Raycast(ray, 999f, GameConst.Filter_Wall, out var result))
		{
			blockPoint = Tool2D.IgnoreZPoint(result.point);
			if (ToPointDistanceSqr(position) < (ray.origin - result.point).sqrMagnitude)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public override void SingleInitialCallback()
	{
		if (GameMgr.IsMobile_Static)
		{
			maxJumpDistance.value1 *= 0.7f;
			maxJumpDistance.value2 *= 0.7f;
		}
	}

	public override void EveryInitialCallback()
	{
		base.SAnima.timeScale = 1f;
		base.SAnima.AnimationState.Data.DefaultMix = 0f;
		base.SAnima.AnimationState.SetAnimation(0, "idle", loop: true);
		base.SAnima.Update(1f);
		base.SAnima.skeleton.UpdateWorldTransform(Skeleton.Physics.None);
		base.SAnima.LateUpdate();
		tongue.Allmove();
		base.gameObject.layer = LayerMask.NameToLayer("Monster");
		multiAttackTimer = 0;
		state = MonsterState.BornIdle;
		stateExistTime = 0f;
		attackCDTimer = Random.Range(0f, attackCD.value2);
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
		if (base.HaveTarget)
		{
			attackCDTimer += Time.deltaTime;
		}
		else
		{
			attackCDTimer += Time.deltaTime / 2f;
		}
		if (base.HaveTarget)
		{
			WallBlocked();
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.SAnima.AnimationState.Data.DefaultMix = 0f;
				base.SAnima.AnimationState.SetAnimation(0, "idle", loop: true);
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster52_Idle");
				}
				else
				{
					base.Anima.Play("Monster52_Idle 1");
				}
			}
			if (stateExistTime > 0.5f)
			{
				base.SAnima.AnimationState.Data.DefaultMix = 0.1f;
				state = MonsterState.MoveToTarget;
			}
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "move", loop: true);
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster52_Move");
				}
				else
				{
					base.Anima.Play("Monster52_Move 1");
				}
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			if (stateExistTime > randomMoveTime.result)
			{
				state = MonsterState.Idle;
				break;
			}
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			if (attackCDTimer > attackCD.result)
			{
				attackCDTimer = 0f;
				state = MonsterState.JumpPrepare;
			}
			break;
		case MonsterState.MoveToTarget:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "move", loop: true);
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster52_Move");
				}
				else
				{
					base.Anima.Play("Monster52_Move 1");
				}
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				maxJumpDistance.RandomResult();
				rotateCloseAngleRange.RandomResult();
				rotateCloseAngleRange.result *= GeneralTool.HalfChanceNPOne();
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
			}
			else if (WallBlocked())
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			else if (ToTargetDistanceSqr() > maxJumpDistance.result * maxJumpDistance.result || attackCDTimer < attackCD.result)
			{
				GetNavInfo(base.TargetPoint);
				SetMove(Tool2D.GetDir(ToPointDir(navInfo.ToGoPoint), rotateCloseAngleRange.result) * base.MoveSpeed);
			}
			else if (base.HaveTarget && ToTargetDistanceSqr() < maxJumpDistance.result * maxJumpDistance.result && attackCDTimer > attackCD.result)
			{
				attackCDTimer = 0f;
				state = MonsterState.JumpPrepare;
			}
			else
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "idle", loop: true);
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster52_Idle");
				}
				else
				{
					base.Anima.Play("Monster52_Idle 1");
				}
				idleTime.RandomResult();
			}
			if (!base.HaveTarget && checkTargetIntervalTimer > 0.2f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget(checkWall: true);
				if (base.HaveTarget && ToTargetDistanceSqr() > sight * sight)
				{
					targetPpt = null;
				}
			}
			if (base.HaveTarget)
			{
				state = MonsterState.MoveToTarget;
				break;
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.RandomMove;
			}
			else
			{
				SetMove(Vector3.zero, isFlip: false);
			}
			break;
		case MonsterState.JumpPrepare:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "aim", loop: false);
				if (Random.Range(0f, 1f) < multiAttackChance)
				{
					multiAttackTimer = 0;
				}
				else
				{
					multiAttackTimer = multiAttackTime;
				}
				attackCD.RandomResult();
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster52_JumpPrepare");
				}
				else
				{
					base.Anima.Play("Monster52_JumpPrepare 1");
				}
			}
			if (stateExistTime > jumpPrepareTime)
			{
				state = MonsterState.Jump;
				break;
			}
			SetMove(Vector3.zero);
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			break;
		case MonsterState.Jump:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "attack", loop: false);
				multiAttackTimer++;
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster52_Jump");
				}
				else
				{
					base.Anima.Play("Monster52_Jump 1");
				}
				GetNearestTarget();
				NormalJump();
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.CanTouch = false;
				SetComponentData(componentData2);
				SetMove(Vector3.zero, isFlip: false);
				base.gameObject.layer = LayerMask.NameToLayer("Monster_Ghost");
				if (base.HaveTarget)
				{
					SetFlip(base.Rigid.linearVelocity.x);
				}
			}
			base.CurrentMotion = Vector3.zero;
			if (base.transform.position.z > 0f)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform);
				state = MonsterState.Attack;
				JumpStop_Dots();
				UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
				componentData3.CanTouch = true;
				SetComponentData(componentData3);
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.Play("Monster52_Attack");
				}
				else
				{
					base.Anima.Play("Monster52_Attack 1");
				}
				base.Rigid.linearVelocity = Vector3.zero;
				PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
				componentData.Linear = base.Rigid.linearVelocity;
				SetComponentData(componentData);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
	}

	public void NormalJump()
	{
		jumpTime.RandomResult();
		GetNearestTarget();
		Vector3 vector;
		if (base.HaveTarget)
		{
			vector = ((!WallBlocked()) ? base.TargetPointIgnoreZ : blockPoint);
		}
		else
		{
			vector = base.transform.position + Tool2D.GetDir() * 3f;
			if (WallBlocked(usePosition: true, base.transform.position, vector))
			{
				vector = blockPoint;
			}
		}
		float num = (0f - jumpTime.result) * gravity / 2f;
		float num2 = Mathf.Min((vector - base.transform.position).magnitude, maxJumpDistance.result);
		if (num2 > jumpKeepDistance)
		{
			num2 -= jumpKeepDistance;
		}
		Vector3 startPoint = base.transform.position + (vector - base.transform.position).normalized * num2;
		startPoint = Tool2D.GetNavMeshPointIngoreZ(startPoint);
		float num3 = GeneralTool.CannonSpeed(num, 0f, gravity, Vector3.Distance(base.transform.position, startPoint));
		jumpDiration = (startPoint - base.transform.position).normalized;
		base.Rigid.linearVelocity = ToPointDir(startPoint) * num3;
		PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
		componentData.Linear = base.Rigid.linearVelocity;
		SetComponentData(componentData);
		JumpStart_Dots(num, gravity);
		SetFlip(jumpDiration.x);
	}

	public override void Theme6Reposition(Vector3 changeValue)
	{
		tongue.LockMotion();
		base.Theme6Reposition(changeValue);
		tongue.UnlockMotion();
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "AttackFinish":
			state = MonsterState.Idle;
			break;
		case "AttackCancel":
			if (multiAttackTimer < multiAttackTime)
			{
				state = MonsterState.Jump;
			}
			break;
		case "Shock":
			CamController.Inst.SetShock(shockParam);
			break;
		case "Attack":
		{
			Vector3 vector = jumpDiration;
			Vector3 vector2 = base.transform.position + new Vector3(waveOffsetX, 0f, 0f) * Mathf.Sign(jumpDiration.x);
			if (base.HaveTarget)
			{
				float num = Tool2D.IgnoreZAngleWithSign(ToTargetDir(), jumpDiration);
				vector = ((!(Mathf.Abs(num) < attackExtraOffset)) ? Tool2D.GetDir(vector, (num > 0f) ? (0f - attackExtraOffset) : attackExtraOffset) : Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, vector2));
			}
			string text = "EF_Monster52_BladeWave";
			if (GameMgr.IsChAge14_Static)
			{
				text = "EF_Monster52_BladeWave_H";
			}
			SEMgr.Inst.monster52_Attack.PlaySE();
			if (pattern == AIPattern.Pattern2)
			{
				for (int i = 0; i < 3; i++)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, vector2).GetComponent<Monster52_BladeWave>().Initialize(Tool2D.GetDir(vector, (float)(i - 1) * attackAngle), attackSpeed, myPpt);
				}
			}
			else
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, vector2).GetComponent<Monster52_BladeWave>().Initialize(vector, attackSpeed, myPpt);
			}
			base.gameObject.layer = LayerMask.NameToLayer("Monster_Fly");
			break;
		}
		}
	}
}
