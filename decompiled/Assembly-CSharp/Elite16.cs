using System.Collections.Generic;
using UnityEngine;

public class Elite16 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Move,
		Idle,
		RandomMove,
		CometDash,
		MeteorCrashBefore,
		MeteorCrash,
		MeteorCrashAfter,
		GravityVortex,
		LaserJail,
		satelliteSpiral,
		StarRain
	}

	[Header("状态")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("移动")]
	public VariableFloat keepDistanceWithPlayer;

	public float keepDistanceWithBorder;

	[Header("空闲")]
	public VariableFloat IdleTime;

	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	[Header("自身和对象池")]
	public static Elite16 Inst;

	public static MiniObjPool MiniPool;

	[Header("选技能")]
	public bool allowSkillRepeat;

	public float meteorCrashChance;

	public float cometDashChance;

	public float gravityVortexChance;

	public float slashChance;

	public VariableFloat ActCD;

	private float actCDTimer;

	[Header("近身伤害")]
	public float dashDamage;

	public float dashKnockBack;

	private List<UnitProperty> dashedPpts = new List<UnitProperty>();

	private List<float> dashedTimer = new List<float>();

	[Header("流星冲击")]
	public VariableInt MeteorCount;

	public VariableFloat MeteorInterval;

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
			varMgr.Clear();
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
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			SetMove(Vector3.zero, isFlip: false);
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.Move:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			if (changedState)
			{
				base.Anima.Play("Move");
			}
			if (changedState)
			{
				base.Anima.SetTrigger("Idle");
				reference = Tool2D.GetDir() * keepDistanceWithPlayer.RandomResult();
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.RandomMove;
				break;
			}
			GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint + reference));
			if (navInfo.allCornerArrived)
			{
				reference = Tool2D.GetDir() * keepDistanceWithPlayer.RandomResult();
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			SetFlip(ToTargetDir().x);
			break;
		}
		case MonsterState.RandomMove:
			if (changedState)
			{
				base.Anima.Play("Move");
			}
			if (changedState)
			{
				base.Anima.SetTrigger("Idle");
				base.SAnima.AnimationState.SetAnimation(0, "Elite6_Idle", loop: true);
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, keepDistanceWithPlayer));
			}
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, keepDistanceWithPlayer));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					state = MonsterState.Move;
				}
			}
			break;
		case MonsterState.Idle:
			_ = changedState;
			break;
		case MonsterState.MeteorCrashBefore:
			if (changedState)
			{
				base.Anima.Play("MeteorCrashBefore");
			}
			break;
		case MonsterState.MeteorCrash:
			_ = ref varMgr.RegFloat(0);
			if (changedState)
			{
				base.Anima.Play("MeteorCrash");
			}
			break;
		case MonsterState.MeteorCrashAfter:
			if (changedState)
			{
				base.Anima.Play("MeteorCrashAfter");
			}
			break;
		case MonsterState.CometDash:
			break;
		}
	}
}
