using System;
using UnityEngine;

public class Elite1 : UnitBase
{
	private enum UnitState
	{
		BornIdle,
		IdleRun,
		Run,
		Attack
	}

	[Space(50f)]
	public float idleRunRotateSpeed;

	public float sideMoveSpeedRatio = 1f;

	public VariableFloat runTimeToChange;

	[Header("Attack")]
	public VariableFloat attackInterval;

	public GameObject pfb_SpiderEgg;

	[Range(0f, 1f)]
	public float chanceToAttackWeb;

	public float attackWebHeight;

	public float attackWebXOffset;

	public float attackWebUpSpeed;

	public float attackWebGravity;

	[Range(0f, 1f)]
	public float chanceToAttack2;

	[Header("Leg")]
	public GameObject pfb_Leg;

	public Transform tsf_Motion;

	[Header("Spell")]
	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	[Header("SafeMode")]
	public Vector3 SafeModeModelOffset;

	public SpriteRenderer bodySprite;

	public Sprite originSprite;

	public Sprite safeModeSprite;

	public bool isSafeMode = true;

	public Transform tsf_Model;

	private UnitState state;

	private Vector3 idleRunDir;

	private float runTimer;

	private float attackIntervalTimer;

	private bool isRunRight = true;

	private SpellSpawnParams ssp;

	public bool IsMove
	{
		get
		{
			if (state == UnitState.IdleRun || state == UnitState.Run)
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
			bodySprite.sprite = safeModeSprite;
			tsf_Model.localPosition = SafeModeModelOffset;
		}
		else
		{
			bodySprite.sprite = originSprite;
			tsf_Model.localPosition = Vector3.zero;
		}
	}

	public override void SingleInitialCallback()
	{
		runTimeToChange.RandomResult();
		attackInterval.RandomResult();
		for (int i = 0; i < 4; i++)
		{
			UnityEngine.Object.Instantiate(pfb_Leg, base.transform).GetComponent<Elite1_Leg>().Initialize(this, Tool2D.GetDir(45 + 30 * i));
		}
		for (int j = 0; j < 4; j++)
		{
			UnityEngine.Object.Instantiate(pfb_Leg, base.transform).GetComponent<Elite1_Leg>().Initialize(this, Tool2D.GetDir(225 + 30 * j));
		}
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
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
		switch (state)
		{
		case UnitState.BornIdle:
			SetMove(Vector3.zero, isFlip: false);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				base.Anima.SetTrigger("Run");
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					state = UnitState.Run;
					break;
				}
				idleRunDir = Tool2D.GetDir();
				state = UnitState.IdleRun;
			}
			break;
		case UnitState.IdleRun:
		{
			runTimer += Time.deltaTime;
			if (runTimer > runTimeToChange.result)
			{
				runTimer = 0f;
				isRunRight = !isRunRight;
				runTimeToChange.RandomResult();
			}
			idleRunDir = Tool2D.GetDir(idleRunDir, idleRunRotateSpeed * Time.deltaTime);
			Vector3 vector2 = idleRunDir * base.MoveSpeed;
			SetMove(isRunRight ? vector2 : (-vector2));
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				if (PlayerMgr.Inst.PlayerCtrller.IsVisible)
				{
					targetPpt = PlayerMgr.Inst.PlayerPpt;
				}
				else
				{
					GetNearestTarget();
				}
				if (base.HaveTarget)
				{
					state = UnitState.Run;
				}
			}
			break;
		}
		case UnitState.Run:
		{
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (!base.HaveTarget)
			{
				idleRunDir = Tool2D.GetDir();
				state = UnitState.IdleRun;
				break;
			}
			runTimer += Time.deltaTime;
			if (runTimer > runTimeToChange.result)
			{
				runTimer = 0f;
				isRunRight = !isRunRight;
				runTimeToChange.RandomResult();
			}
			Vector3 motion = ToTargetDir() * base.MoveSpeed;
			Vector3 vector = ToTargetDir(isRunRight ? 90 : (-90));
			motion += vector * base.MoveSpeed * sideMoveSpeedRatio;
			SetMove(motion);
			attackIntervalTimer += Time.deltaTime;
			if (attackIntervalTimer > attackInterval.result)
			{
				attackIntervalTimer = 0f;
				state = UnitState.Attack;
				attackInterval.RandomResult();
				if (UnityEngine.Random.value <= chanceToAttackWeb)
				{
					base.Anima.SetTrigger("AttackWeb");
				}
				else
				{
					base.Anima.SetTrigger("Attack");
				}
				SetFlip(ToTargetDir().x);
			}
			break;
		}
		case UnitState.Attack:
			SetMove(Vector3.zero);
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	protected override void BossDeadStay()
	{
		base.Anima.SetTrigger("Die");
		base.enabled = false;
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
	}

	public override void AnimaAction(string animaName)
	{
		if (base.deadStayed)
		{
			return;
		}
		switch (animaName)
		{
		case "Attack":
		{
			SEMgr.Inst.elite1Attack.PlaySE();
			if (!base.HaveTarget)
			{
				break;
			}
			float value = UnityEngine.Random.value;
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			if (value < chanceToAttack2)
			{
				for (int i = 0; i < 4; i++)
				{
					sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
					sSPModifier.Direction = ToTargetDir(-12 - 4 * i);
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
				}
				for (int j = 0; j < 4; j++)
				{
					sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
					sSPModifier.Direction = ToTargetDir(-12 + 4 * j);
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
				}
			}
			else
			{
				for (int k = 0; k < 5; k++)
				{
					sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
					sSPModifier.Direction = ToTargetDir(-12 + 4 * k);
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
				}
			}
			break;
		}
		case "AttackWeb":
			SEMgr.Inst.elite1Web.PlaySE();
			if (base.HaveTarget)
			{
				UnityEngine.Object.Instantiate(pfb_SpiderEgg, base.transform.position + new Vector3(base.IsFlipped ? (0f - attackWebXOffset) : attackWebXOffset, 0f, 0f - attackWebHeight), Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<SpiderEgg>().Initialize(myPpt, attackWebUpSpeed, attackWebGravity, base.TargetPoint);
			}
			break;
		case "AttackFinish":
			state = UnitState.Run;
			base.Anima.SetTrigger("Run");
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}
}
