using System;
using UnityEngine;

public class Monster49 : UnitBase
{
	public enum UnitState
	{
		BornIdle,
		Idle,
		IdleWalk,
		ShootPrepare,
		Shooting,
		ShootStop,
		Trample
	}

	[Space(50f)]
	public VariableFloat idleTime;

	public VariableFloat idleWalkTime;

	public VariableFloat idleWalkRadius;

	[Header("Missile")]
	public VariableInt AttackTimes;

	private int attackCount;

	public VariableFloat shootInterval;

	public float doubleShootChance;

	public float missileHeight;

	public float missileOffsetX;

	[Header("Trample")]
	public VariableFloat trampleInterval;

	public float trampleTriggerDistance;

	public float trampleCenterOffsetX;

	public float trampleWarningRadius;

	public float trampleWarningTime;

	[Header("Sound")]
	public AudioSource as_Spit;

	public UnitState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private float idleTimer;

	private float walkTimer;

	private float shootIntervalTimer;

	private float trampleIntervalTimer;

	private WarningArea warningArea;

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

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		as_Spit.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void EveryInitialCallback()
	{
		idleTime.RandomResult();
		idleWalkTime.RandomResult();
		shootInterval.RandomResult();
		trampleInterval.RandomResult();
		state = UnitState.BornIdle;
		idleTimer = 0f;
		walkTimer = 0f;
		shootIntervalTimer = UnityEngine.Random.Range(0f, shootInterval.value2);
		trampleIntervalTimer = UnityEngine.Random.Range(0f, trampleInterval.value2);
		if (GameMgr.IsHarmony_Static)
		{
			base.SAnima.initialSkinName = "49_1";
		}
	}

	public override void SingleInitialCallback()
	{
		if (GameMgr.IsMobile_Static)
		{
			trampleWarningRadius *= 0.8f;
			doubleShootChance -= 0.2f;
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
		case UnitState.BornIdle:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "idle", loop: true);
				base.Anima.Play("Idle");
			}
			SetMove(Vector3.zero);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = UnitState.Idle;
			}
			break;
		case UnitState.Idle:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "idle", loop: true);
				base.Anima.Play("Idle");
			}
			SetMove(Vector3.zero);
			idleTimer += Time.deltaTime;
			if (idleTimer >= idleTime.result)
			{
				idleTimer = 0f;
				idleTime.RandomResult();
				state = UnitState.IdleWalk;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
			}
			break;
		case UnitState.IdleWalk:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "move", loop: true);
				base.Anima.Play("Walk");
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
			walkTimer += Time.deltaTime;
			if (walkTimer > idleWalkTime.result)
			{
				walkTimer = 0f;
				state = UnitState.Idle;
				base.Anima.Play("Idle");
			}
			break;
		case UnitState.ShootPrepare:
			if (changedState)
			{
				attackCount = 0;
				AttackTimes.RandomResult();
				base.SAnima.AnimationState.SetAnimation(0, "exhalation", loop: false);
				base.Anima.Play("ShootPrepare");
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case UnitState.Shooting:
			if (changedState)
			{
				attackCount++;
				base.SAnima.AnimationState.SetAnimation(0, "exhalationing", loop: false);
				base.Anima.Play("Shooting", 0, 0f);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case UnitState.ShootStop:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "exhalation_over", loop: false);
				base.Anima.Play("ShootStop");
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case UnitState.Trample:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "attach", loop: false);
				base.Anima.Play("Trample");
				GetNearestTarget();
				if (base.HaveTarget)
				{
					SetFlip(ToTargetDir().x);
				}
				warningArea = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle" + (GameMgr.IsChAge14_Static ? " purple" : ""), base.transform.position + new Vector3(base.IsFlipped ? (0f - trampleCenterOffsetX) : trampleCenterOffsetX, 0f, 0f)).GetComponent<WarningArea>();
				warningArea.Initialize(trampleWarningRadius, trampleWarningTime);
			}
			if (warningArea != null)
			{
				warningArea.transform.position = base.transform.position + new Vector3(base.IsFlipped ? (0f - trampleCenterOffsetX) : trampleCenterOffsetX, 0f, 0f);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		default:
			Debug.LogError(state);
			break;
		}
		if (state != UnitState.Idle && state != UnitState.IdleWalk)
		{
			return;
		}
		shootIntervalTimer += Time.deltaTime;
		if (shootIntervalTimer >= shootInterval.result)
		{
			shootIntervalTimer = 0f;
			shootInterval.RandomResult();
			state = UnitState.ShootPrepare;
			return;
		}
		trampleIntervalTimer += Time.deltaTime;
		if (trampleIntervalTimer >= trampleInterval.result)
		{
			trampleIntervalTimer = 0f;
			trampleInterval.RandomResult();
			state = UnitState.Trample;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Shoot":
			as_Spit.Play();
			if (UnityEngine.Random.Range(0f, 1f) < doubleShootChance)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster49_Missile", base.transform.position + new Vector3((float)(base.IsFlipped ? 1 : (-1)) * missileOffsetX, 0f, 0f - missileHeight)).GetComponent<Monster49_Missile>().master = this;
			}
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster49_Missile", base.transform.position + new Vector3((float)(base.IsFlipped ? 1 : (-1)) * missileOffsetX, 0f, 0f - missileHeight)).GetComponent<Monster49_Missile>().master = this;
			break;
		case "ShootPrepareFinish":
			state = UnitState.Shooting;
			break;
		case "ShootingFinish":
			if (attackCount < AttackTimes.result)
			{
				state = UnitState.Shooting;
			}
			else
			{
				state = UnitState.ShootStop;
			}
			break;
		case "ShootFinish":
		case "TrampleFinish":
			state = UnitState.Idle;
			break;
		case "Trample":
			ObjPoolMgr.Inst.RecycleGO(warningArea.gameObject);
			warningArea = null;
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/ef_Monster49_Trample", base.transform.position + new Vector3(base.IsFlipped ? (0f - trampleCenterOffsetX) : trampleCenterOffsetX, 0f, 0f)).GetComponent<Monster49_Trample>().master = this;
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (warningArea != null)
		{
			ObjPoolMgr.Inst.RecycleGO(warningArea.gameObject);
		}
	}
}
