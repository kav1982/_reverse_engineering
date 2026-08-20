using System;
using UnityEngine;

public class Monster36 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		Move,
		GetIntoRange,
		Attack,
		SideStep,
		Escape
	}

	[Header("移动")]
	public float actChanceRecovery;

	public VariableFloat randomFlyRadius;

	public float chaseTargetChance;

	public float theme6MoveTimeLimit;

	[Header("攻击")]
	public float spellSpeed;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public VariableFloat spellSpeedFix;

	public VariableFloat launchAngle;

	public float spellDuration;

	public int spellDamage;

	public float spellHeight;

	public float bulletCount;

	public VariableFloat bulletRotateSpeed;

	public VariableFloat attackPointOffset;

	private Vector3 attackPoint;

	public float attackChance;

	public float closeRange;

	public float moveOffsetAngle;

	public float escapeDistance;

	public VariableFloat escapeAngle;

	public VariableFloat attackRadius;

	[Header("翅膀")]
	public float wingInterval;

	private float wingTimer;

	public SpriteRenderer wingRenderer;

	public SpriteRenderer wingForeRenderer;

	private bool isFirstWing;

	public Sprite wing1;

	public Sprite wing2;

	public Sprite wing3;

	public Sprite wing4;

	private Vector3 toGoPoint;

	[Header("二模式")]
	public float multiAttackTime;

	public float multiAttackTimer;

	private float rotateRight;

	public float sideStepAngle;

	[Header("音效")]
	public AudioSource as_Fly;

	[Header("状态机")]
	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	public AIPattern pattern;

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
		}
	}

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundChange));
		SoundChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundChange));
	}

	private void SoundChange()
	{
		as_Fly.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		navAreaMask = 32;
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90111);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		if (GameMgr.IsMobile_Static)
		{
			bulletCount *= 0.6f;
		}
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
	}

	protected override void SetFlip(float motionX)
	{
		for (int i = 0; i < myPpt.SR_Models.Length; i++)
		{
			myPpt.SR_Models[i].flipX = motionX < 0f;
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
		wingTimer += Time.deltaTime;
		if (wingTimer > wingInterval)
		{
			wingTimer = 0f;
			isFirstWing = !isFirstWing;
			if (isFirstWing)
			{
				wingRenderer.sprite = wing1;
				wingForeRenderer.sprite = wing3;
			}
			else
			{
				wingRenderer.sprite = wing2;
				wingForeRenderer.sprite = wing4;
			}
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				bornIdleTimer = 0f;
				base.Anima.Play("Monster36_Idle");
				base.CurrentMotion = Vector3.zero;
			}
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 0.5f)
			{
				state = MonsterState.Move;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Move:
			if (changedState)
			{
				randomFlyRadius.RandomResult();
				base.Anima.Play("Monster36_Move");
				GetNearestTarget();
				if (UnityEngine.Random.Range(0f, 1f) < chaseTargetChance && base.HaveTarget)
				{
					GetNavInfo(Tool2D.GetNavMeshPointIngoreZ(base.transform.position + ToTargetDir() * randomFlyRadius.result, navAreaMask));
					theme6MoveTimeLimit = ToPointDistance(navInfo.ToGoPoint) / base.MoveSpeed;
				}
				else
				{
					GetNavInfo(Tool2D.GetNavMeshPointIngoreZ(base.transform.position, randomFlyRadius, navAreaMask));
					theme6MoveTimeLimit = ToPointDistance(navInfo.ToGoPoint) / base.MoveSpeed;
				}
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived || (stateExistTime > theme6MoveTimeLimit && (LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)))
			{
				state = MonsterState.Idle;
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Monster36_Idle");
			}
			SetMove(Vector3.zero);
			checkTargetIntervalTimer += Time.deltaTime;
			if ((double)checkTargetIntervalTimer >= 0.2)
			{
				GetNearestTarget();
			}
			if (!(Time.time % actChanceRecovery < 0.1f))
			{
				break;
			}
			if (base.HaveTarget)
			{
				if (ToTargetDistanceSqr() < closeRange * closeRange)
				{
					if ((float)UnityEngine.Random.Range(0, 1) < attackChance)
					{
						state = MonsterState.GetIntoRange;
					}
				}
				else
				{
					state = MonsterState.Move;
				}
			}
			else
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.GetIntoRange:
			if (changedState)
			{
				base.Anima.Play("Monster36_Move");
				if (!base.HaveTarget)
				{
					state = MonsterState.Idle;
					break;
				}
				toGoPoint = Tool2D.GetNavMeshPointIngoreZ(base.TargetPoint, attackRadius, -ToTargetDir(), moveOffsetAngle, navAreaMask);
				GetNavInfo(toGoPoint);
				theme6MoveTimeLimit = ToPointDistance(navInfo.ToGoPoint) / base.MoveSpeed;
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived || (stateExistTime > theme6MoveTimeLimit && (LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)))
			{
				multiAttackTimer = 1f;
				state = MonsterState.Attack;
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.Anima.Play("Monster36_Attack");
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.SideStep:
			if (changedState)
			{
				base.Anima.Play("Monster36_Move");
				multiAttackTimer += 1f;
				if (!base.HaveTarget)
				{
					state = MonsterState.Idle;
					break;
				}
				rotateRight = ((!((double)UnityEngine.Random.Range(0, 1) > 0.5)) ? 1 : (-1));
				toGoPoint = base.TargetPointIgnoreZ + Tool2D.GetDir(base.transform.position - base.TargetPoint, rotateRight * sideStepAngle);
				GetNavInfo(toGoPoint);
				theme6MoveTimeLimit = ToPointDistance(navInfo.ToGoPoint) / base.MoveSpeed;
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived || (stateExistTime > theme6MoveTimeLimit && (LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)))
			{
				state = MonsterState.Attack;
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			break;
		case MonsterState.Escape:
			if (changedState)
			{
				base.Anima.Play("Monster36_Move");
				if (!base.HaveTarget)
				{
					state = MonsterState.Idle;
					break;
				}
				escapeAngle.RandomResult();
				toGoPoint = base.transform.position + Tool2D.GetDir(base.transform.position - base.TargetPoint, escapeAngle.result).normalized * escapeDistance;
				GetNavInfo(toGoPoint);
				theme6MoveTimeLimit = ToPointDistance(navInfo.ToGoPoint) / base.MoveSpeed;
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived || (stateExistTime > theme6MoveTimeLimit && (LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)))
			{
				state = MonsterState.Idle;
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			break;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (pattern == AIPattern.Pattern2)
		{
			if (base.HaveTarget)
			{
				attackPoint = base.TargetPointIgnoreZ;
			}
			else
			{
				attackRadius.RandomResult();
				attackPoint = base.transform.position + Tool2D.GetDir() * attackRadius.result;
			}
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
			for (int i = 0; (float)i < bulletCount; i++)
			{
				bulletRotateSpeed.RandomResult();
				attackPointOffset.RandomResult();
				launchAngle.RandomResult();
				spellSpeedFix.RandomResult();
				Vector3 vector = Tool2D.GetDir() * attackPointOffset.result;
				sSPModifier.Speed = spellSpeed * spellSpeedFix.result;
				Vector3 vector2 = attackPoint + vector;
				sSPModifier.Float1 = vector2.x;
				sSPModifier.Float2 = vector2.y;
				sSPModifier.Float3 = bulletRotateSpeed.result;
				sSPModifier.Direction = Tool2D.GetDir(base.transform.position - attackPoint, launchAngle.result).normalized;
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "Attack"))
		{
			if (animaName == "AttackFinish")
			{
				state = MonsterState.Idle;
			}
			return;
		}
		SEMgr.Inst.monster36Attack.PlaySE();
		if (base.HaveTarget)
		{
			attackPoint = base.TargetPointIgnoreZ;
		}
		else
		{
			attackRadius.RandomResult();
			attackPoint = base.transform.position + Tool2D.GetDir() * attackRadius.result;
		}
		if (pattern == AIPattern.Pattern1)
		{
			for (int i = 0; (float)i < bulletCount; i++)
			{
				bulletRotateSpeed.RandomResult();
				attackPointOffset.RandomResult();
				launchAngle.RandomResult();
				spellSpeedFix.RandomResult();
				Vector3 vector = Tool2D.GetDir() * attackPointOffset.result;
				Vector3 vector2 = attackPoint + vector;
				UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
				sSPModifier.Speed = spellSpeed * spellSpeedFix.result;
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.Direction = Tool2D.GetDir(Tool2D.IgnoreZPoint(base.transform.position - attackPoint), launchAngle.result).normalized;
				sSPModifier.Float1 = vector2.x;
				sSPModifier.Float2 = vector2.y;
				sSPModifier.Float3 = bulletRotateSpeed.result;
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
				sSPModifier.Direction = Tool2D.GetDir(base.transform.position - attackPoint, 0f - launchAngle.result).normalized;
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
		}
		else
		{
			for (int j = 0; (float)j < bulletCount; j++)
			{
				bulletRotateSpeed.RandomResult();
				attackPointOffset.RandomResult();
				launchAngle.RandomResult();
				spellSpeedFix.RandomResult();
				Vector3 vector3 = Tool2D.GetDir() * attackPointOffset.result;
				Vector3 vector4 = attackPoint + vector3;
				sipBullet.shootDirection = Tool2D.GetDir(base.transform.position - attackPoint, launchAngle.result).normalized;
				UnitSpellModifier sSPModifier2 = UnitBase.GetSSPModifier(in ssp);
				sSPModifier2.Speed = spellSpeed * spellSpeedFix.result;
				sSPModifier2.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier2.Direction = Tool2D.GetDir(Tool2D.IgnoreZPoint(base.transform.position - attackPoint), launchAngle.result).normalized;
				sSPModifier2.Float1 = vector4.x;
				sSPModifier2.Float2 = vector4.y;
				sSPModifier2.Float3 = bulletRotateSpeed.result;
				sSPModifier2.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
		}
	}
}
