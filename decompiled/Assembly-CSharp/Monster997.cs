using UnityEngine;

public class Monster997 : UnitBase
{
	public enum MonsterState
	{
		Idle,
		RandomMove,
		Move,
		Attack,
		AttackIdle
	}

	public VariableFloat idleTime;

	public VariableFloat randomMoveTime;

	public float attackDistance;

	private int textCounter;

	private int textType;

	private float attackCDTimer;

	[Header("攻击")]
	public float spellDamage;

	public float spellGravity;

	public float spellUpSpeed;

	public float spellHeight;

	private SpellSpawnParams ssp;

	[Header("状态机")]
	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private Vector3 aimPoint;

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

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90011, UnitType.Teammate);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Damage = spellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.Gravity = 0f - spellGravity;
		sSPModifier.CurrentFallSpeed = 0f - spellUpSpeed;
		sSPModifier.ApplyToSSP(ref ssp);
		attackCDTimer = 0.8f;
	}

	private void TryAttack()
	{
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
		if (state != MonsterState.Attack)
		{
			attackCDTimer += Time.deltaTime;
		}
		switch (state)
		{
		case MonsterState.Idle:
			if (changedState)
			{
				idleTime.RandomResult();
				base.Anima.Play("Teammate1_Idle");
			}
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.RandomMove;
			}
			SetMove(Vector3.zero, isFlip: false);
			GetNearestTargetWithTimer();
			if (base.HaveTarget)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				randomMoveTime.RandomResult();
				base.Anima.Play("Teammate1_IdleWalk");
				GetNavInfo(base.transform.position + Tool2D.GetDir() * 5f);
			}
			if (stateExistTime > randomMoveTime.result)
			{
				state = MonsterState.Idle;
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				state = MonsterState.Idle;
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * 0.3f);
			}
			GetNearestTargetWithTimer();
			if (base.HaveTarget)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.Move:
		{
			if (changedState)
			{
				base.Anima.Play("Teammate1_Run");
				if (base.HaveTarget)
				{
					GetNavInfo(base.TargetPoint);
				}
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
				if (!base.HaveTarget)
				{
					state = MonsterState.Idle;
					break;
				}
				GetNavInfo(base.TargetPoint);
			}
			GetNavInfoWithTimer(base.TargetPoint);
			CheckNavInfo();
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			float num2 = ToTargetDistance();
			if (num2 < attackDistance && !UnitDotsSyncSystem.Raycast(new Ray(base.transform.position, ToTargetDir()), num2, GameConst.Filter_Wall))
			{
				state = MonsterState.AttackIdle;
			}
			break;
		}
		case MonsterState.Attack:
			if (changedState)
			{
				attackCDTimer = 0f;
				base.Anima.Play("Teammate1_Attack");
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			else
			{
				GetNearestTarget();
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.AttackIdle:
			if (changedState)
			{
				base.Anima.Play("Teammate1_Idle");
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
				float num = ToTargetDistance();
				if (num <= attackDistance && !UnitDotsSyncSystem.Raycast(new Ray(base.transform.position, ToTargetDir()), num, GameConst.Filter_Wall))
				{
					if (attackCDTimer > 0.8f)
					{
						state = MonsterState.Attack;
						aimPoint = base.TargetPoint;
					}
				}
				else
				{
					state = MonsterState.Move;
				}
			}
			else
			{
				GetNearestTarget();
				if (!base.HaveTarget)
				{
					state = MonsterState.Idle;
				}
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "Shoot"))
		{
			if (animaName == "AttackFinish")
			{
				state = MonsterState.AttackIdle;
			}
			else
			{
				Debug.LogError(animaName);
			}
			return;
		}
		ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>();
		Vector3 spawnPosition = base.transform.position - new Vector3(0f, 0f, spellHeight);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.SpawnPosition = spawnPosition;
		sSPModifier.Direction = ToPointDir(aimPoint);
		sSPModifier.Speed = GeneralTool.CannonSpeed(spellUpSpeed, 0f, spellGravity, ToPointDistance(aimPoint));
		ssp.ElementComponentData = default(SpellElementEffectComponentData);
		switch (textCounter)
		{
		case 0:
			sSPModifier.ColorType = SpellColorType.Fire;
			ssp.ElementComponentData = default(SpellElementEffectComponentData);
			ssp.ElementComponentData.FireBurnDuration = 3f;
			ssp.ElementComponentData.FireHpBurnPercent = 0.05f;
			break;
		case 1:
			sSPModifier.ColorType = SpellColorType.Mucus;
			ssp.ElementComponentData.MucusDuration = 3f;
			ssp.ElementComponentData.MucusMoveSpeedRatio = 0.6f;
			ssp.ElementComponentData.MucusSpellSpeedRatio = 0.7f;
			break;
		case 2:
			sSPModifier.ColorType = SpellColorType.Venom;
			ssp.ElementComponentData.VenomApplyCount = 1f;
			ssp.ElementComponentData.VenomDuration = 3f;
			break;
		case 3:
			sSPModifier.ColorType = SpellColorType.Thunder;
			ssp.ElementComponentData.ThunderHitRadius = 2.2f;
			ssp.ElementComponentData.ThunderHitDamageRatio = 2f;
			break;
		case 4:
			sSPModifier.ColorType = SpellColorType.Frozen;
			ssp.ElementComponentData.FrozenDuration = 1.5f;
			ssp.ElementComponentData.ThunderHitDamageRatio = 2f;
			break;
		case 5:
			sSPModifier.ColorType = SpellColorType.Void;
			ssp.ElementComponentData.VenomDuration = 3f;
			ssp.ElementComponentData.VoidExplosionHpDamageRatio = 0.4f;
			ssp.ElementComponentData.VoidInstantKillThreshold = 0.1f;
			break;
		}
		sSPModifier.ApplyToSSP(ref ssp);
		UnitDotsSyncSystem.ShootSpell(ssp);
		textCounter++;
		if (textCounter == 6)
		{
			textCounter = 0;
		}
	}
}
