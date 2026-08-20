using System;
using UnityEngine;

public class Monster53 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Move,
		Attack
	}

	public AIPattern pattern;

	public StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("行动")]
	public float swingTimer;

	[Header("游动")]
	public VariableFloat amplitude;

	public VariableFloat frequency;

	public VariableFloat phase;

	private float offset;

	private bool reversed;

	private Vector3 CurrentDir;

	private float speedFix;

	[Header("叫声")]
	public VariableFloat ShoutInterval;

	public VariableFloat ShoutPitch;

	private float shoutTimer;

	[Header("高度")]
	public float baseHeight;

	public float extraHeight;

	public Transform tsf_Model;

	[Header("尾巴")]
	public int tentacleCount;

	public float tentacleAngle;

	public Monster53_Tentacle pfb_Tentacle;

	public Transform tsf_TentacleParent;

	public Transform tsf_Motion;

	public float tailInterval;

	private Monster53_Tentacle[] tentacles;

	[Header("母体")]
	public Monster53_Invisible master;

	[Header("影子")]
	public Shadow thisShadow;

	[Header("攻击")]
	public VariableInt bulletCount;

	public float attackOffset;

	public float spellHeight;

	public VariableFloat spellSpeed;

	public VariableFloat spellDuration;

	public VariableInt spellDamage;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public Transform tsf_Sprite;

	public Transform tsf_Firepoint;

	[Header("动画")]
	public Monster53_Anima anim;

	private SpellSpawnParams ssp;

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

	public override void SingleInitialCallback()
	{
		tentacles = new Monster53_Tentacle[tentacleCount];
		for (int i = 0; i < tentacleCount; i++)
		{
			tentacles[i] = UnityEngine.Object.Instantiate(pfb_Tentacle, tsf_TentacleParent.transform.position + new Vector3((0f - tailInterval) / 2f + tailInterval / (float)(tentacleCount - 1) * (float)i, 0f, 0f), Quaternion.identity, tsf_TentacleParent).GetComponent<Monster53_Tentacle>();
			tentacles[i].SingleInitial(this, (0f - tentacleAngle) / 2f + tentacleAngle / (float)tentacleCount * (float)i);
		}
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90201);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
	}

	public override void EveryInitialCallback()
	{
		ShoutInterval.RandomResult();
		shoutTimer = UnityEngine.Random.Range(0f, ShoutInterval.result);
		for (int i = 0; i < tentacles.Length; i++)
		{
			tentacles[i].EveryInitial();
		}
		CurrentDir = Tool2D.GetDir();
		tsf_Motion.up = CurrentDir;
		tsf_Motion.forward = Vector3.forward;
		tsf_TentacleParent.up = CurrentDir;
		reversed = GeneralTool.ChanceResult(0.5f);
		state = MonsterState.BornIdle;
	}

	public void Initialize(Monster53_Invisible master)
	{
		this.master = master;
		frequency.RandomResult();
		amplitude.RandomResult();
		phase.RandomResult();
	}

	public override void Update()
	{
		float num = baseHeight + master.GetHeightValue(base.transform.position) * extraHeight;
		tsf_TentacleParent.localPosition = new Vector3(0f, 0f, 0f - num);
		tsf_Model.localPosition = new Vector3(0f, num, 0f - num);
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		CurrentDir = base.CurrentMotion;
		tsf_Motion.up = CurrentDir;
		tsf_TentacleParent.up = CurrentDir;
		thisShadow.ShadowGO.transform.up = CurrentDir;
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
		swingTimer += Time.deltaTime;
		offset = Mathf.Sin(swingTimer * frequency.result * 2f * MathF.PI + phase.result) * amplitude.result * (float)((!reversed) ? 1 : (-1));
		Vector3 queuePosition = master.GetQueuePosition(master.GetIndex(this), offset);
		speedFix = Mathf.Lerp(0f, 1f, Mathf.Min(0.04f, ToPointDistanceSqr(queuePosition)) / 0.04f);
		SetMove(ToPointDir(queuePosition) * base.MoveSpeed * speedFix);
		Debug.DrawLine(queuePosition, base.transform.position);
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Born");
			}
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 1f)
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			shoutTimer += Time.deltaTime;
			if (shoutTimer > ShoutInterval.result)
			{
				shoutTimer = 0f;
				state = MonsterState.Attack;
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.Anima.Play("Attack");
			}
			break;
		case MonsterState.RandomMove:
		case MonsterState.Move:
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "AttackFinish"))
		{
			if (!(animaName == "Attack"))
			{
				return;
			}
			GetNearestTarget();
			bulletCount.RandomResult();
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight) + tsf_Firepoint.position - tsf_Model.position;
			for (int i = 0; i < bulletCount.result; i++)
			{
				sSPModifier.Speed = spellSpeed.RandomResult();
				sSPModifier.Duration = spellDuration.RandomResult();
				sSPModifier.Damage = spellDamage.RandomResult();
				sSPModifier.Direction = Tool2D.GetDir();
				sSPModifier.ApplyToSSP(ref ssp);
				ssp.MovementComponentData.IsIgnoreWall = true;
				ShootSpell(ssp);
				if (GeneralTool.ChanceResult(0.7f))
				{
					if (pattern == AIPattern.Pattern2)
					{
						SEMgr.Inst.monster53_ShoutBig.PlaySE(SEPlayMode.Replay, 3, 1f).pitch = ShoutPitch.RandomResult();
					}
					else
					{
						SEMgr.Inst.monster53_Shout.PlaySE(SEPlayMode.Replay, 3, 1f).pitch = ShoutPitch.RandomResult();
					}
				}
			}
		}
		else
		{
			state = MonsterState.Idle;
		}
	}
}
