using System.Collections.Generic;
using UnityEngine;

public class Monster105 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		Attack,
		TP,
		Dead
	}

	[Header("待机和随机移动")]
	public VariableFloat idleTime;

	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	public float checkInterval;

	public float checkIntervalTimer;

	[Header("瞬移")]
	public VariableFloat tpCount;

	public int tpCounter;

	[Header("攻击")]
	public bool isAttacking;

	public VariableFloat attackOffset;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public VariableFloat attackCD;

	public float attackCDTimer;

	public Transform attackParticle;

	[Header("翻转")]
	public SpriteRenderer bodyRender;

	public Sprite frontSprite;

	public Sprite backSprite;

	[Header("尸体图片")]
	public Sprite corpseSprite;

	public Shadow shadow;

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private SpellSpawnParams SipBulletSsp;

	public int type;

	public int circleBulletAmount;

	public int bulletsPerSideSquare = 8;

	public int bulletsPerSideCross;

	public int bulletsPerSideTriangle;

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
		state = MonsterState.BornIdle;
		attackCD.RandomResult();
		isAttacking = false;
		base.CC_Self.enabled = true;
		tpCount.RandomResult();
		tpCounter = 0;
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
			SetMove(Vector3.zero);
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.Attack;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				idleTime.RandomResult();
			}
			SetMove(Vector3.zero);
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.Attack;
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.Anima.Play("Idle");
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			attackCDTimer += Time.deltaTime;
			if (attackCDTimer > attackCD.result && !isAttacking)
			{
				base.Anima.Play("Attack");
				isAttacking = true;
			}
			if ((float)tpCounter >= tpCount.result)
			{
				state = MonsterState.TP;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.TP:
			if (changedState)
			{
				base.Anima.Play("TP");
				tpCounter = 0;
				tpCount.RandomResult();
				attackCDTimer = 3f;
			}
			break;
		case MonsterState.Dead:
			break;
		}
	}

	public void Attack()
	{
		SEMgr.Inst.monster12Land.PlaySE();
		tpCounter++;
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in SipBulletSsp);
		sSPModifier.Direction = Tool2D.GetDir();
		if (!base.HaveTarget)
		{
			GetNearestTarget();
		}
		if (base.HaveTarget)
		{
			sSPModifier.Direction = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position), attackOffset.RandomResult());
		}
		List<GameObject> bullets = new List<GameObject>();
		type = Random.Range(0, 4);
		switch (type)
		{
		case 0:
		{
			for (int l = 0; l < circleBulletAmount; l++)
			{
				sSPModifier.SpawnPosition = new Vector3(0f, 0f, 0f - spellHeight) + base.transform.position;
				sSPModifier.ApplyToSSP(ref SipBulletSsp);
				ShootSpell(SipBulletSsp);
			}
			break;
		}
		case 1:
		{
			for (int j = 0; j < bulletsPerSideCross * 4 + 1; j++)
			{
				sSPModifier.SpawnPosition = new Vector3(0f, 0f, 0f - spellHeight) + base.transform.position;
				sSPModifier.ApplyToSSP(ref SipBulletSsp);
				ShootSpell(SipBulletSsp);
			}
			break;
		}
		case 2:
		{
			for (int k = 0; k < bulletsPerSideSquare * 4; k++)
			{
				sSPModifier.SpawnPosition = new Vector3(0f, 0f, 0f - spellHeight) + base.transform.position;
				sSPModifier.ApplyToSSP(ref SipBulletSsp);
				ShootSpell(SipBulletSsp);
			}
			break;
		}
		case 3:
		{
			for (int i = 0; i < bulletsPerSideTriangle * 3; i++)
			{
				sSPModifier.SpawnPosition = new Vector3(0f, 0f, 0f - spellHeight) + base.transform.position;
				sSPModifier.ApplyToSSP(ref SipBulletSsp);
				ShootSpell(SipBulletSsp);
			}
			break;
		}
		}
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster105_BulletsMgr", base.transform.position - new Vector3(0f, 0f, spellHeight)).GetComponent<Monster105_BulletsMgr>().Init(sSPModifier.Direction, type, bullets, base.transform.position);
		isAttacking = false;
		attackCDTimer = 0f;
		attackCD.RandomResult();
	}

	protected override void SetFlip(float motionX)
	{
		if (base.HaveTarget && !isAttacking)
		{
			if (base.TargetPoint.x < base.transform.position.x)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
				attackParticle.localScale = new Vector3(-1f, 1f, 1f);
			}
			else
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
				attackParticle.localScale = Vector3.one;
			}
			if (base.TargetPoint.y > base.transform.position.y)
			{
				bodyRender.sprite = backSprite;
			}
			else
			{
				bodyRender.sprite = frontSprite;
			}
		}
	}

	public override void AfterTakeDamage(TakeDamageInfo info)
	{
		if (!isAttacking)
		{
			SEMgr.Inst.monster12Land.PlaySE();
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		SEMgr.Inst.monster12Land.PlaySE();
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Attack":
			Attack();
			break;
		case "TP":
		{
			Vector3 zero = Vector3.zero;
			zero = ((LevelMgr.Inst.CurrentRoomCfg.themeType != RoomThemeType.Theme6_Chapter3 && LevelMgr.Inst.CurrentRoomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1) ? (LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(Random.Range((float)(-LevelMgr.Inst.CurrentRoomCtrller.roomCfg.width) / 2f, (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.width / 2f), Random.Range((float)(-LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height) / 2f, (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height / 2f), 0f)) : (LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(Random.Range((float)(-LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width) / 2f, (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width / 2f), Random.Range((float)(-LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height) / 2f, (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height / 2f), 0f)));
			zero = LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(zero);
			base.transform.position = zero;
			break;
		}
		case "TPBack":
			base.CC_Self.enabled = true;
			myPpt.InvincibleUnregister();
			state = MonsterState.Idle;
			shadow.ShadowGO.SetActive(value: true);
			break;
		case "SetInvincible":
			base.CC_Self.enabled = false;
			myPpt.InvincibleRegister();
			shadow.ShadowGO.SetActive(value: false);
			break;
		case "Dead":
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_DeadPermanent_Monster102", base.transform.position).GetComponent<Corpse>().sr.sprite = corpseSprite;
			myPpt.AnnouncedDeath();
			break;
		}
	}
}
