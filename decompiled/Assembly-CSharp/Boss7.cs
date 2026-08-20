using System.Collections.Generic;
using UnityEngine;

public class Boss7 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Move,
		Idle,
		RandomMove,
		Attack,
		KnockGround,
		TeleportQuick,
		TeleportBack,
		TeleportHide,
		TeleportShow,
		SummonSoul,
		SummonTrap,
		DashPrepare,
		Dash,
		Smash,
		SmashCancel,
		SummonLegion,
		SlashPrepare,
		Slash,
		SlashPrepareAgain,
		SlashAgain
	}

	[Header("状态")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("移动")]
	public float maxKeepDistance;

	public VariableFloat repositionRadius;

	public VariableFloat repositionTime;

	[Header("空闲")]
	public VariableFloat IdleTime;

	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	[Header("自身和对象池")]
	public static Boss7 Inst;

	public static MiniObjPool MiniPool;

	[Header("选技能")]
	public bool allowSkillRepeat;

	public float attackChance;

	public float summonTrapChance;

	public float teleportChance;

	public float slashChance;

	public VariableFloat ActCD;

	private float actCDTimer;

	[Header("普通攻击尝试")]
	public int bulletCount;

	public float bulletSpeed;

	public float bulletHeight;

	public float bulletAngle;

	[Header("近身伤害")]
	public float dashDamage;

	public float dashKnockBack;

	private List<UnitProperty> dashedPpts = new List<UnitProperty>();

	private List<float> dashedTimer = new List<float>();

	[Header("近战普通攻击")]
	public float attackDashSpeed;

	public AnimationCurve attackSpeedCurve;

	public float attackDashTime;

	public LineRenderer warningRenderer;

	private float attackDashTimer;

	private Vector3 attackDashDir;

	[Header("传送创人")]
	public VariableFloat teleportDistance;

	public VariableInt teleportAttackTimes;

	public Vector3 dashDir;

	public float dashSpeedRatio;

	public float dashRotateSpeed;

	public float dashTime;

	public ParticleSystem teleportParticle;

	public float teleportAngleRange;

	public Shadow shadow;

	public SpriteRenderer colorChangeReciever;

	public LineRenderer lr_DashWarning;

	private int teleportAttackTimesCounter;

	[Header("传送下砸")]
	public ShockParam smashParam;

	public float teleportSmashChance;

	public VariableFloat smashTeleportDistance;

	public float smashRadius;

	public float smashWarningTime;

	public ParticleSystem smashParticle;

	[Header("近战斩击")]
	public bool fakeFlipped;

	public Boss7_Sword sword;

	public Transform tsf_sprite;

	public bool showSword;

	public Transform tsf_ChestPoint;

	public Transform tsf_ChestPoint1;

	public float slashTime;

	public float slashSpeedRatio;

	public AnimationCurve slashSpeedCurve;

	public float slashMoveTime;

	public float slashAgainChance;

	private bool isSlashAgain;

	[Header("关于虚化")]
	public float hideDistance;

	public float hideCD;

	private float hideCDTimer;

	public float hideAlpha;

	private bool isHide;

	public float checkHideInterval;

	private float checkHideTime;

	public SpriteRenderer mainSprite;

	public SpriteRenderer borderSprite;

	[Header("陷阱攻击")]
	public int trapSummonCount;

	private List<Boss7_GhostTrap> horiontalTraps = new List<Boss7_GhostTrap>();

	private List<Boss7_GhostTrap> verticalTraps = new List<Boss7_GhostTrap>();

	private List<int> horizontalIndex = new List<int>();

	private List<int> verticalIndex = new List<int>();

	[HideInInspector]
	public float roomWidth;

	[HideInInspector]
	public float roomHeight;

	[HideInInspector]
	public Vector3 roomCenter;

	private Vector3 originPoint;

	private List<int> checkList = new List<int>();

	private MonsterState lastSkill;

	private MonsterState[] states = new MonsterState[4]
	{
		MonsterState.Attack,
		MonsterState.SummonTrap,
		MonsterState.TeleportQuick,
		MonsterState.SlashPrepare
	};

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

	public bool swordLocked => base.IsLocked;

	public override void EveryInitialCallback()
	{
		if (MiniPool == null)
		{
			MiniPool = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Mixed/MiniObjPool"), LevelMgr.Inst.CurrentRoomT).GetComponent<MiniObjPool>();
		}
		Inst = this;
		state = MonsterState.BornIdle;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height;
		roomHeight -= 1f;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.width;
		roomCenter = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomCenter.y -= 0.5f;
		originPoint = roomCenter - new Vector3(roomWidth / 2f, roomHeight / 2f, 0f);
		lr_DashWarning.positionCount = 10;
		lr_DashWarning.enabled = false;
		sword.gameObject.SetActive(value: true);
		sword.Initialize(this);
		sword.Update();
		showSword = false;
	}

	public override void SingleInitialCallback()
	{
		myPpt.RemoveSRFromArray(colorChangeReciever);
		sword = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss7_Sword", base.transform.position, base.transform).GetComponent<Boss7_Sword>();
	}

	private bool GetRandomTrap()
	{
		int num = -1;
		int num2 = -1;
		checkList.Clear();
		for (int i = 0; (float)i < roomHeight - 1f; i++)
		{
			bool flag = false;
			if (horizontalIndex.Contains(i))
			{
				flag = true;
			}
			if (!flag)
			{
				checkList.Add(i);
			}
		}
		if (checkList.Count > 0)
		{
			num = checkList[Random.Range(0, checkList.Count - 1)];
		}
		checkList.Clear();
		for (int j = 0; (float)j < roomWidth - 1f; j++)
		{
			bool flag2 = false;
			if (verticalIndex.Contains(j))
			{
				flag2 = true;
			}
			if (!flag2)
			{
				checkList.Add(j);
			}
		}
		if (checkList.Count > 0)
		{
			num2 = checkList[Random.Range(0, checkList.Count - 1)];
		}
		Vector3 vector = originPoint + new Vector3(0.5f, 0.5f, 0f);
		float num3 = GeneralTool.HalfChanceNPOne() * 0.5f;
		if (GeneralTool.ChanceResult(0.5f))
		{
			if (num2 != -1)
			{
				Vector3 vector2 = vector + new Vector3(num2, (roomHeight - 1f) * 0.5f, 0f);
				vector = vector2 + Vector3.up * (roomHeight - 1f) * num3;
				FinalSummon(num2, vector, Vector3.up * num3, vector2);
				verticalIndex.Add(num2);
				return true;
			}
			if (num != -1)
			{
				Vector3 vector2 = vector + new Vector3((roomWidth - 1f) * 0.5f, num, 0f);
				vector = vector2 + Vector3.right * (roomWidth - 1f) * num3;
				FinalSummon(num, vector, Vector3.right * num3, vector2);
				horizontalIndex.Add(num);
				return true;
			}
			return false;
		}
		if (num != -1)
		{
			Vector3 vector2 = vector + new Vector3((roomWidth - 1f) * 0.5f, num, 0f);
			vector = vector2 + Vector3.right * (roomWidth - 1f) * num3;
			FinalSummon(num, vector, Vector3.right * num3, vector2);
			horizontalIndex.Add(num);
			return true;
		}
		if (num2 != -1)
		{
			Vector3 vector2 = vector + new Vector3(num2, (roomHeight - 1f) * 0.5f, 0f);
			vector = vector2 + Vector3.up * (roomHeight - 1f) * num3;
			FinalSummon(num2, vector, Vector3.up * num3, vector2);
			verticalIndex.Add(num2);
			return true;
		}
		return false;
	}

	private void FinalSummon(int index, Vector3 finalPoint, Vector3 direction, Vector3 centerPoint)
	{
		Vector3 vector = new Vector3(2f, roomHeight, 0f);
		Vector3 vector2 = new Vector3(roomWidth, 2f, 0f);
		MiniPool.GetGO("Prefabs/EF/EF_Boss7_GhostTrap", finalPoint).GetComponent<Boss7_GhostTrap>().Initialize(TriggerScale: (direction.x != 0f) ? vector2 : vector, distance: (direction.x == 0f) ? (roomHeight - 1f) : (roomWidth - 1f), index: index, direction: -direction, triggerCenter: centerPoint);
	}

	public void TrapFadeReport(int index, bool isHorizontal)
	{
		if (isHorizontal)
		{
			horizontalIndex.Remove(index);
		}
		else
		{
			verticalIndex.Remove(index);
		}
	}

	private void TryAttack()
	{
		if (ActCD.result == 0f)
		{
			ActCD.RandomResult();
		}
		actCDTimer += Time.deltaTime;
		if (actCDTimer > ActCD.result)
		{
			ChooseSkill();
			actCDTimer = 0f;
			ActCD.RandomResult();
			teleportAttackTimes.RandomResult();
		}
	}

	private void ChooseSkill()
	{
		MonsterState monsterState2 = (lastSkill = (state = RandomSkill()));
	}

	private MonsterState RandomSkill()
	{
		int weightRandom = GeneralTool.GetWeightRandom(attackChance, summonTrapChance, teleportChance, slashChance);
		return states[weightRandom];
	}

	public bool CheckTargetNearby()
	{
		if (Tool2D.IgnoreZDistanceSqr(base.transform.position, PlayerMgr.Inst.PlayerCtrller.transform.position) < hideDistance * hideDistance)
		{
			return true;
		}
		return GeneralTool.HaveCollider(base.transform.position, hideDistance, new string[2] { "Teammate", "Player" }) != null;
	}

	protected override void SetFlip(float motionX)
	{
		base.SetFlip(motionX);
		fakeFlipped = myPpt.SR_Models[0].flipX;
	}

	public override void Update()
	{
		for (int num = dashedTimer.Count - 1; num >= 0; num--)
		{
			dashedTimer[num] -= Time.deltaTime;
			if (dashedTimer[num] < 0f)
			{
				dashedTimer.RemoveAt(num);
				dashedPpts.RemoveAt(num);
			}
		}
		myPpt.Color_NormalBody = colorChangeReciever.color;
		myPpt.ChangeAlpha(colorChangeReciever.color.a);
		shadow.SetTransparency(colorChangeReciever.color.a);
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
			_ = ref varMgr.RegBool(0);
			ref bool reference = ref varMgr.RegBool(1);
			ref float reference2 = ref varMgr.RegFloat(0);
			_ = ref varMgr.RegFloat(1);
			if (changedState)
			{
				base.Anima.Play("Move");
				repositionTime.RandomResult();
				if (base.HaveTarget)
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint, repositionRadius));
				}
				else
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, repositionRadius));
				}
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
			if (Mathf.Abs(base.transform.position.x - roomCenter.x) > roomWidth / 2f || Mathf.Abs(base.transform.position.y - roomCenter.y) > roomHeight / 2f)
			{
				state = MonsterState.TeleportBack;
				break;
			}
			if (ToTargetDistanceSqr() > maxKeepDistance * maxKeepDistance && !reference)
			{
				reference = true;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint, repositionRadius));
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			else
			{
				reference2 += Time.deltaTime;
				if (navInfo.allCornerArrived || reference2 > repositionTime.result)
				{
					repositionTime.RandomResult();
					reference2 = 0f;
					reference = false;
					GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint, repositionRadius, -ToTargetDir(), 60f));
				}
				else
				{
					Debug.DrawLine(base.transform.position, navInfo.ToGoPoint);
					SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
					CheckNavInfo();
				}
			}
			TryAttack();
			break;
		}
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				IdleTime.RandomResult();
			}
			if (stateExistTime > IdleTime.result)
			{
				state = MonsterState.RandomMove;
			}
			SetMove(Vector3.zero, isFlip: false);
			GetNearestTargetWithTimer();
			if (base.HaveTarget)
			{
				state = MonsterState.Move;
			}
			else
			{
				TryAttack();
			}
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				base.Anima.Play("Move");
				randomMoveRadius.RandomResult();
				randomMoveTime.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				GetNearestTarget(checkWall: true);
			}
			if (base.HaveTarget)
			{
				state = MonsterState.Move;
				break;
			}
			if (stateExistTime > randomMoveTime.result)
			{
				state = MonsterState.Idle;
				break;
			}
			if (navInfo.allCornerArrived)
			{
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			else
			{
				CheckNavInfo();
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			TryAttack();
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.Anima.Play("Attack");
				GetNearestTarget();
			}
			if (attackDashTimer < attackDashTime)
			{
				attackDashTimer += Time.deltaTime;
				SetMove(attackDashDir * attackDashSpeed * attackSpeedCurve.Evaluate(attackDashTimer / attackDashTime));
			}
			else
			{
				SetMove(Vector3.zero, isFlip: false);
			}
			break;
		case MonsterState.SummonTrap:
			if (changedState)
			{
				base.Anima.Play("SummonTrap");
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDelta().x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.TeleportQuick:
			if (changedState)
			{
				myPpt.CanTouch = false;
				base.Anima.Play("TeleportQuick");
				teleportAttackTimesCounter++;
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDelta().x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.TeleportBack:
			if (changedState)
			{
				myPpt.CanTouch = false;
				base.Anima.Play("TeleportBack");
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDelta().x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.SlashPrepare:
			if (changedState)
			{
				base.Anima.Play("SlashPrepare");
				if (!base.HaveTarget)
				{
					GetNearestTarget();
				}
				dashDir = Tool2D.GetDir(ToPointDir(roomCenter), Random.Range(-60, 60));
				if (base.HaveTarget)
				{
					dashDir = ToTargetDir();
				}
				sword.SlashAt(dashDir);
				isSlashAgain = GeneralTool.ChanceResult(slashAgainChance);
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				sword.SlashAim(ToTargetDir());
				SetFlip(ToTargetDelta().x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Slash:
			if (changedState)
			{
				base.Anima.Play("Slash");
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				SetFlip(dashDir.x);
			}
			if (sword.state == Boss7_Sword.SwordState.Recycle && isSlashAgain)
			{
				state = MonsterState.SlashPrepareAgain;
				break;
			}
			if (sword.state == Boss7_Sword.SwordState.Invisible)
			{
				state = MonsterState.Move;
				break;
			}
			base.transform.position += dashDir * base.MoveSpeed * slashSpeedRatio * slashSpeedCurve.Evaluate(stateExistTime / slashMoveTime) * Time.deltaTime;
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.SlashPrepareAgain:
			if (!changedState)
			{
				break;
			}
			if (changedState)
			{
				base.Anima.Play("SlashPrepareAgain");
				if (!base.HaveTarget)
				{
					GetNearestTarget();
				}
				dashDir = Tool2D.GetDir(ToPointDir(roomCenter), Random.Range(-60, 60));
				if (base.HaveTarget)
				{
					dashDir = ToTargetDir();
				}
				sword.SlashAt(dashDir);
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				sword.SlashAim(ToTargetDir());
				SetFlip(ToTargetDelta().x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.SlashAgain:
			if (changedState)
			{
				base.Anima.Play("Slash");
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				SetFlip(dashDir.x);
			}
			if (sword.state == Boss7_Sword.SwordState.Invisible)
			{
				state = MonsterState.Move;
				break;
			}
			base.transform.position += dashDir * base.MoveSpeed * slashSpeedRatio * slashSpeedCurve.Evaluate(stateExistTime / slashMoveTime) * Time.deltaTime;
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.DashPrepare:
		{
			if (changedState)
			{
				base.Anima.Play("DashPrepare");
				if (!base.HaveTarget)
				{
					GetNearestTarget();
				}
				dashDir = Tool2D.GetDir(ToPointDir(roomCenter), Random.Range(-60, 60));
				if (base.HaveTarget)
				{
					dashDir = ToTargetDir();
				}
				teleportParticle.Play();
				lr_DashWarning.enabled = true;
			}
			for (int i = 0; i < lr_DashWarning.positionCount; i++)
			{
				Vector3 rootPoint = Vector3.Lerp(base.transform.position, base.transform.position + dashDir * myPpt.unitCfg.moveSpeed * dashSpeedRatio * dashTime, (float)i / (float)(lr_DashWarning.positionCount - 1));
				lr_DashWarning.SetPosition(i, Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect));
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDelta().x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
		case MonsterState.Dash:
			if (changedState)
			{
				if (!base.HaveTarget)
				{
					GetNearestTarget();
				}
				if (base.HaveTarget)
				{
					dashDir = ToTargetDir();
				}
				base.Anima.Play("Dash");
				lr_DashWarning.enabled = false;
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				SetFlip(dashDir.x);
				dashDir = Tool2D.RotateTowardsAroundZAxis(dashDir, ToTargetDir(), Time.deltaTime * base.MoveSpeed * dashSpeedRatio * dashRotateSpeed);
			}
			if (stateExistTime > dashTime)
			{
				if (teleportAttackTimesCounter > teleportAttackTimes.result)
				{
					myPpt.CanTouch = true;
					teleportAttackTimesCounter = 0;
					state = MonsterState.Move;
				}
				else
				{
					state = MonsterState.TeleportQuick;
				}
			}
			else
			{
				SetMove(dashDir * base.MoveSpeed * dashSpeedRatio);
			}
			break;
		case MonsterState.Smash:
			if (changedState)
			{
				base.Anima.Play("Smash");
				teleportParticle.Play();
				MiniPool.GetGO("Prefabs/Mixed/WarningArea_Circle cyan", Tool2D.IgnoreZPoint(base.transform.position)).GetComponent<WarningArea>().Initialize(smashRadius, smashWarningTime);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.SmashCancel:
			if (changedState)
			{
				base.Anima.Play("SmashCancel");
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.KnockGround:
		case MonsterState.TeleportHide:
		case MonsterState.TeleportShow:
		case MonsterState.SummonSoul:
		case MonsterState.SummonLegion:
			break;
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (state != MonsterState.Dash)
		{
			return;
		}
		switch (other.gameObject.tag)
		{
		case "Player":
		case "Teammate":
		case "Destructible":
		case "Brittleness":
		{
			UnitProperty component = other.gameObject.GetComponent<UnitProperty>();
			if (!dashedPpts.Contains(component))
			{
				TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
				takeDamageInfo.knockbackForce = ((other.transform.position - base.transform.position).normalized * 0.5f + base.CurrentMotion * 0.5f) * dashKnockBack;
				takeDamageInfo.damage = dashDamage;
				if (other.gameObject.tag != "Player")
				{
					takeDamageInfo.damage *= 4f;
				}
				component.TakeDamage(dashDamage, AttackerType.NothingSpecial, takeDamageInfo);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch_Large", component.transform.position + Tool2D.GetDir() * Random.Range(0f, 0.2f) + new Vector3(0f, -1f, -0.5f), 1f);
				dashedPpts.Add(component);
				dashedTimer.Add(0.5f);
			}
			break;
		}
		}
	}

	private void KnockGround()
	{
		CamController.Inst.SetShock(smashParam);
		smashParticle.Play();
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(base.transform.position, smashRadius, "Destructible", "Spell", "RollBall", "Butterfly", "Brittleness", "Player", "Teammate");
		for (int i = 0; i < collidersByTag.Count; i++)
		{
			if (collidersByTag[i].tag == "Wall")
			{
				if (collidersByTag[i].gameObject.name == "111(Clone)")
				{
					collidersByTag[i].GetComponent<Elite12_FallRock>().Die();
				}
			}
			else if (collidersByTag[i].tag == "Spell" || collidersByTag[i].tag == "RollBall" || collidersByTag[i].tag == "Butterfly")
			{
				if (collidersByTag[i].gameObject.activeInHierarchy)
				{
					SpellBase componentInParent = collidersByTag[i].GetComponentInParent<SpellBase>();
					if (componentInParent.spellCfg.abilityType == SpellAbilityType.Rollball)
					{
						((Spell1002RollBall)componentInParent).TakeDamage(dashDamage);
					}
					else if (componentInParent.spellCfg.abilityType == SpellAbilityType.Butterfly)
					{
						((Spell1003Butterfly)componentInParent).HitEFAndRecycle();
					}
				}
			}
			else
			{
				TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
				takeDamageInfo.knockbackForce = Tool2D.IgnoreZPoint(collidersByTag[i].transform.position - base.transform.position).normalized * dashKnockBack;
				UnitProperty component = collidersByTag[i].GetComponent<UnitProperty>();
				if (component.unitCfg.unitType == UnitType.Player)
				{
					component.TakeDamage(dashDamage, null, takeDamageInfo);
				}
				else
				{
					component.TakeDamage((int)dashDamage, null, takeDamageInfo);
				}
			}
		}
	}

	private void Teleport()
	{
		bool num = GeneralTool.ChanceResult(teleportSmashChance);
		Vector3 targetPoint = roomCenter;
		GetNearestTarget();
		Vector3 vector = ToPointDir(targetPoint);
		if (base.HaveTarget)
		{
			targetPoint = base.TargetPoint;
			vector = ((!(targetPpt.PlayerCtrller != null)) ? targetPpt.UnitBas.CurrentMotion : targetPpt.PlayerCtrller.CurrentMotion);
		}
		if (num)
		{
			base.transform.position = Tool2D.GetNavMeshPointIngoreZ(targetPoint, smashTeleportDistance, vector.normalized, teleportAngleRange);
			state = MonsterState.Smash;
		}
		else
		{
			base.transform.position = Tool2D.GetNavMeshPointIngoreZ(targetPoint, teleportDistance, vector.normalized, teleportAngleRange);
			state = MonsterState.DashPrepare;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "TeleportParticle":
			break;
		case "AttackDash":
			GetNearestTarget();
			if (base.HaveTarget)
			{
				attackDashDir = ToTargetDir();
			}
			attackDashTimer = 0f;
			break;
		case "Attack":
		{
			for (int j = 0; j < bulletCount; j++)
			{
				Vector3 dir = Tool2D.GetDir(attackDashDir, bulletAngle * ((float)j / (float)bulletCount - 0.5f));
				MiniPool.GetGO("Prefabs/EF/EF_Boss7_Bullet", base.transform.position + Vector3.back * bulletHeight + dir * 0.5f).GetComponent<Boss7_Bullet>().Initialize(dir, myPpt, MiniPool);
			}
			break;
		}
		case "AttackFinish":
			state = MonsterState.Move;
			break;
		case "SummonTrap":
		{
			for (int i = 0; i < trapSummonCount; i++)
			{
				GetRandomTrap();
			}
			break;
		}
		case "SummonTrapFinish":
			state = MonsterState.Move;
			break;
		case "AttackTeleport":
		{
			Vector3 targetPoint = roomCenter;
			if (base.HaveTarget)
			{
				targetPoint = base.TargetPoint;
			}
			base.transform.position = Tool2D.GetNavMeshPointIngoreZ(targetPoint, ToPointDistance(targetPoint) + 3f, ToPointDir(targetPoint), 90f);
			teleportParticle.Play();
			break;
		}
		case "TeleportBack":
			base.transform.position = Tool2D.GetNavMeshPointIngoreZ(base.transform.position);
			if (ToPointDistance(roomCenter) > 1f)
			{
				base.transform.position += ToPointDir(roomCenter);
			}
			teleportParticle.Play();
			break;
		case "TeleportBackFinish":
			state = MonsterState.Move;
			break;
		case "Teleport":
			Teleport();
			break;
		case "Smash":
			KnockGround();
			break;
		case "SmashCancel":
			if (teleportAttackTimesCounter <= teleportAttackTimes.result)
			{
				teleportAttackTimesCounter++;
				state = MonsterState.SmashCancel;
			}
			break;
		case "SmashCancelFinish":
			Teleport();
			break;
		case "SmashFinish":
			myPpt.CanTouch = true;
			teleportAttackTimesCounter = 0;
			state = MonsterState.Move;
			break;
		case "DashPrepareFinish":
			state = MonsterState.Dash;
			break;
		case "SlashPrepareFinish":
			state = MonsterState.Slash;
			break;
		case "SlashPrapareAgainFinish":
			state = MonsterState.SlashAgain;
			break;
		}
	}
}
