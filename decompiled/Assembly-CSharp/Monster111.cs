using UnityEngine;

public class Monster111 : UnitBase
{
	public enum MonsterState
	{
		Invisible,
		Appear,
		Move,
		RandomMove,
		Dead
	}

	public float shotInterval;

	public float shotIntervalTimer;

	public bool canAttack;

	public int bulletAmount;

	public int bulletAmountCounter;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public Transform bulletPivot;

	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	public Shadow shadow;

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private SpellSpawnParams SipBulletSsp;

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
		SipBulletSsp = UnitDotsSyncSystem.GetSpellPrototype(90281);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in SipBulletSsp);
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref SipBulletSsp);
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.Appear;
		myPpt.CC_Self.enabled = true;
		myPpt.CanTouch = true;
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
		if (base.CurrentMotion.y > 0f)
		{
			base.Anima.SetFloat("Flip", 1f);
		}
		else
		{
			base.Anima.SetFloat("Flip", 0f);
		}
		switch (state)
		{
		case MonsterState.Appear:
			if (changedState)
			{
				base.Anima.Play("Appear");
				canAttack = false;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Invisible:
			if (changedState)
			{
				base.Anima.Play("Invisible");
				canAttack = false;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Move:
			if (changedState)
			{
				base.Anima.Play("Move");
				canAttack = true;
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.RandomMove;
				break;
			}
			GetNavInfo(base.TargetPoint);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			if (base.CurrentMotion.x < 0f)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			CheckAndAttack();
			break;
		case MonsterState.RandomMove:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			if (changedState)
			{
				canAttack = false;
				base.Anima.Play("Move");
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				reference = base.transform.position + Tool2D.GetDir() * randomMoveRadius.result;
				GetNavInfo(reference);
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				reference = base.transform.position + Tool2D.GetDir() * randomMoveRadius.result;
				GetNavInfo(reference);
				break;
			}
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed, isFlip: false);
			if (base.CurrentMotion.x < 0f)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			break;
		}
		case MonsterState.Dead:
			if (changedState)
			{
				base.Anima.Play("Dead");
				canAttack = false;
			}
			SetMove(Vector3.zero);
			break;
		}
	}

	public void CheckAndAttack()
	{
		shotIntervalTimer += Time.deltaTime;
		if (canAttack && shotIntervalTimer > shotInterval)
		{
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in SipBulletSsp);
			sSPModifier.Direction = Tool2D.GetDir();
			if (base.HaveTarget)
			{
				sSPModifier.Direction = Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, bulletPivot.position);
			}
			else
			{
				sSPModifier.Direction = base.CurrentMotion.normalized;
			}
			sSPModifier.Speed = spellSpeed;
			sSPModifier.SpawnPosition = new Vector3(0f, -0.2f, 0f - spellHeight) + bulletPivot.position;
			sSPModifier.ApplyToSSP(ref SipBulletSsp);
			ShootSpell(SipBulletSsp);
			bulletAmountCounter++;
			shotIntervalTimer = 0f;
			if (bulletAmountCounter >= bulletAmount)
			{
				bulletAmountCounter = 0;
				canAttack = false;
				state = MonsterState.Invisible;
			}
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "ApperEnd":
			myPpt.InvincibleUnregister();
			myPpt.CC_Self.enabled = true;
			myPpt.CanTouch = true;
			state = MonsterState.Move;
			Debug.Log(1154753);
			break;
		case "InvisibleEnd":
		{
			state = MonsterState.Appear;
			shadow.ShadowGO.SetActive(value: true);
			Vector3 zero = Vector3.zero;
			zero = ((LevelMgr.Inst.CurrentRoomCfg.themeType != RoomThemeType.Theme6_Chapter3 && LevelMgr.Inst.CurrentRoomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1) ? (LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(Random.Range((float)(-LevelMgr.Inst.CurrentRoomCtrller.roomCfg.width) / 2f, (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.width / 2f), Random.Range((float)(-LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height) / 2f, (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height / 2f), 0f)) : (LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(Random.Range((float)(-LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width) / 2f, (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width / 2f), Random.Range((float)(-LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height) / 2f, (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height / 2f), 0f)));
			zero = LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(zero);
			base.transform.position = zero;
			break;
		}
		case "InvisibleStart":
			myPpt.InvincibleRegister();
			myPpt.CC_Self.enabled = false;
			myPpt.CanTouch = false;
			shadow.ShadowGO.SetActive(value: false);
			break;
		case "DeadEnd":
			myPpt.AnnouncedDeath();
			break;
		}
	}
}
