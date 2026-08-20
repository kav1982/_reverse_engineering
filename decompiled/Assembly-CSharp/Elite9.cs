using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.VFX;

public class Elite9 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		ArmTest,
		Idle,
		MoveAway,
		CloseMove,
		Mine,
		Charge,
		LaserAttack,
		Rest,
		Slash,
		DoubleSlashPrepare,
		DoubleSlashChase,
		DoubleSlash,
		SwitchStage,
		BeforeKnock,
		Knock
	}

	[Header("技能选择")]
	public float slashChance;

	public float mineChance;

	public float laserChance;

	public float knockChance;

	public float doubleSlashChance;

	public float canSlashDistance;

	private MonsterState lastSkill;

	public VariableFloat attackCD;

	private float attackCDTimer;

	private bool stageSwitched;

	[Header("近战")]
	public float slashBulletCount;

	public float slashBulletAngle;

	public VariableFloat slashBulletAngleOffset;

	public int spellDamage;

	public float spellSpeed;

	public float spellDuration;

	public float spellHeight;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public VariableFloat maxSlashTime;

	public VariableFloat secondStageMaxSlashTime;

	private float slashCount;

	public float slashDeacclerateSpeed;

	public float slashBulletTime;

	private bool slashSpeedNormal;

	private bool slashRotate;

	public float bulletDistance;

	public ShockParam slashShockParam;

	[Header("双刀")]
	public float doubleSlashAngle;

	public float doubleSlashChaseTime;

	public float doubleSlashDirationChangeSpeed;

	public float doubleSlashTargetDistance;

	public float doubleSlashTargetAngle;

	public float doubleSlashSpeedFix;

	public float doubleSlashDistance;

	public bool doubleSlashAimStop;

	public ParticleSystem hintParticle;

	public ShockParam doubleSlashShockParam;

	private bool slashAgain = true;

	[Header("充能")]
	public float chargeTime;

	private float chargeTimer;

	public ParticleSystem chargeParticle;

	private float moveSpeedLerped;

	[Header("激光和灼烧地面")]
	public float laserAttackRadius;

	public int laserAttackDamage;

	public ParticleSystem attackParticle;

	public VariableFloat attackAngleOffset;

	public float attackDistance;

	private float nowAttackDistance;

	private float nowAttackDistanceModular;

	public float attackCloseDistance;

	public float attackRange;

	public Vector3 attackingPoint;

	public float laserSpeed;

	public LineRenderer lr_Laser;

	public ParticleSystem laserParticle;

	public Transform tsf_Head;

	private Elite9_HotGround currentLaserGround;

	private float laserShakeTimer;

	public float laserShakeTime;

	public Vector3 attackDiration;

	public float laserMoveSpeedFix;

	private Vector3 attackDirationHorizontal;

	public float horizontalMinDistance;

	private bool isHorizontal;

	public VariableFloat HorizontalLaserOffset;

	private Vector3 laserHorizonRootPoint;

	private float laserHorizontalRight;

	private bool laserOutOfCliff;

	private List<Entity> laserAttackPpt = new List<Entity>();

	[Header("近距离")]
	public float closeSpeedFixer;

	public float stopMoveRange;

	public float closeMoveFixAngle;

	private float closeMoveRight;

	public float closeDistance;

	public bool needRediration;

	public Vector3 moveDiration = Vector3.right;

	public float dirationChangeSpeed;

	public float cliffCheckRange;

	public float dirationChangeSpeedClose;

	private Vector3 cliffMoveDiration;

	public bool closeToCliff;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	private Vector3 noTargetPoint;

	[Header("摇摆")]
	public float moveAngleAmplitude;

	public float moveAngleFrequency;

	private float nowMoveAngle;

	[Header("地雷")]
	public int mineCount;

	public VariableFloat mineRange;

	public int extraMineCount;

	public int secondStageMineCount;

	public int secondStageExtraMineCount;

	public float maxMineCount;

	public float secondStageMaxMineCount;

	public List<Elite9_Mine> mines = new List<Elite9_Mine>();

	[Header("冷却")]
	public float bubbleCountPerMeter;

	public VisualEffect ve_Bubble;

	public VisualEffect ve_Bubble_H;

	public ParticleSystem restParticle;

	public float restTime;

	private float restTimer;

	[Header("体节")]
	public GameObject bodyPfb;

	public int bodyCount;

	public int notTailCount;

	public LineRenderer lr_bodyConnector;

	public LineRenderer lr_bodyConnectorShadow;

	public LineRenderer lr_Tail;

	public LineRenderer lr_TailShadow;

	public LineRenderer lr_upperBodyConnector;

	public LineRenderer lr_upperBodyConnectorShadow;

	public List<Elite9_BodyLerp> upperbody = new List<Elite9_BodyLerp>();

	public List<Elite9_Body> bodys = new List<Elite9_Body>();

	public Transform tsf_HeadShadow;

	public bool headUp;

	[Header("砸地")]
	public float knockTime;

	private float knockTimer;

	public float knockSpeedFix;

	public VariableInt knockBulletCount;

	public VariableFloat knockBulletSpeed;

	public ShockParam knockParam;

	public int spellDamage2;

	public float spellSpeed2;

	public float spellDuration2;

	public float spellHeight2;

	public ParticleSystem shoutParticle;

	[Header("手臂")]
	public List<Elite9_Arm> arms = new List<Elite9_Arm>();

	private bool rightArmMove = true;

	[Header("伤害共享")]
	public List<Entity> hitList = new List<Entity>();

	private List<Elite9_BodyInvisible> invisibleBodys = new List<Elite9_BodyInvisible>();

	public List<int> invisibleBodyIndex = new List<int>();

	public float hitListClearTime;

	private float hitListClearTimer;

	[Header("音效")]
	public AudioSource as_LaserLoop;

	public AudioSource as_LaserEnd;

	public AudioSource as_LaserCharge;

	public static Elite9 Inst;

	public static MiniObjPool MiniPool;

	[Header("状态")]
	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("和谐模式")]
	public UnityEngine.Material mt_bodyConnector_H;

	public UnityEngine.Material mt_Tail_H;

	public UnityEngine.Material mt_UpperBody_H;

	public UnityEngine.Material mt_bodyConnector_H_S;

	public UnityEngine.Material mt_Tail_H_S;

	public UnityEngine.Material mt_UpperBody_H_S;

	public UnityEngine.Material mt_Laser_H;

	public ParticleSystem chargeParticle_H;

	public ParticleSystem attackParticle_H;

	public ParticleSystem hintParticle_H;

	public ParticleSystem laserParticle_H;

	private List<MonsterState> skills = new List<MonsterState>
	{
		MonsterState.Slash,
		MonsterState.DoubleSlashPrepare,
		MonsterState.Charge,
		MonsterState.BeforeKnock
	};

	private List<UnitDotsSyncSystem.DistanceHitResult> laserHitTargets = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private List<UnitDotsSyncSystem.DistanceHitResult> bladeHitTargets = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private bool secondStage
	{
		get
		{
			if (EntityIsValid(myPpt.myEntity))
			{
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				return componentData.unitCfg.currentHP <= componentData.unitCfg.maxHP * 0.5f;
			}
			return false;
		}
	}

	public float moveFixAngle => Tool2D.IgnoreZAngleWithSign(base.transform.position - bodys[1].transform.position, moveDiration);

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
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		as_LaserLoop.volume = DataMgr.settingData.GetFinalSound();
		as_LaserEnd.volume = DataMgr.settingData.GetFinalSound();
		as_LaserCharge.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		Elite9_Body front = null;
		lr_bodyConnector.positionCount = notTailCount;
		lr_bodyConnectorShadow.positionCount = notTailCount;
		lr_Tail.positionCount = bodyCount - notTailCount + 1;
		lr_TailShadow.positionCount = bodyCount - notTailCount + 1;
		lr_upperBodyConnector.positionCount = upperbody.Count;
		lr_upperBodyConnectorShadow.positionCount = upperbody.Count;
		for (int i = 0; i < bodyCount; i++)
		{
			Elite9_Body component = UnityEngine.Object.Instantiate(bodyPfb, LevelMgr.Inst.CurrentRoomT).GetComponent<Elite9_Body>();
			component.transform.position = base.transform.position;
			if (i < notTailCount)
			{
				component.Initialize(this, front);
			}
			else
			{
				component.Initialize(this, front, isTail: true);
			}
			front = component;
			bodys.Add(component);
		}
		arms[0].SingleInitial(this, rightLeg: true);
		arms[1].SingleInitial(this, rightLeg: false);
		for (int j = 0; j < invisibleBodyIndex.Count; j++)
		{
			Elite9_BodyInvisible component2 = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + 300921, Tool2D.GetNavMeshPointIngoreZ(bodys[j].transform.position)).GetComponent<Elite9_BodyInvisible>();
			invisibleBodys.Add(component2);
			component2.target = bodys[invisibleBodyIndex[j]];
			component2.master = this;
			component2.hitList = hitList;
		}
		headUp = false;
		if (GameMgr.IsMobile_Static)
		{
			maxMineCount *= 0.8f;
			extraMineCount -= 2;
			slashBulletCount -= 1f;
			slashBulletAngleOffset.value1 = -10f;
			slashBulletAngleOffset.value1 = 10f;
			secondStageMaxSlashTime.value1 = 3f;
			secondStageMaxSlashTime.value2 = 4f;
			knockBulletCount.value1 -= 2;
			knockBulletCount.value2 -= 2;
		}
	}

	public override void EveryInitialCallback()
	{
		Inst = this;
		if (MiniPool == null)
		{
			MiniPool = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool"), LevelMgr.Inst.CurrentRoomT).GetComponent<MiniObjPool>();
		}
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height;
		if (GameMgr.IsHarmony_Static)
		{
			UnityEngine.Object.Destroy(lr_bodyConnector.material);
			UnityEngine.Object.Destroy(lr_bodyConnectorShadow.material);
			UnityEngine.Object.Destroy(lr_Tail.material);
			UnityEngine.Object.Destroy(lr_TailShadow.material);
			UnityEngine.Object.Destroy(lr_upperBodyConnector.material);
			UnityEngine.Object.Destroy(lr_upperBodyConnectorShadow.material);
			UnityEngine.Object.Destroy(lr_Laser.material);
			lr_bodyConnector.material = mt_bodyConnector_H;
			lr_bodyConnectorShadow.material = mt_bodyConnector_H_S;
			lr_Tail.material = mt_Tail_H;
			lr_TailShadow.material = mt_Tail_H_S;
			lr_upperBodyConnector.material = mt_UpperBody_H;
			lr_upperBodyConnectorShadow.material = mt_UpperBody_H_S;
			lr_Laser.material = mt_Laser_H;
			hintParticle = hintParticle_H;
			laserParticle = laserParticle_H;
			attackParticle = attackParticle_H;
			chargeParticle = chargeParticle_H;
			ve_Bubble = ve_Bubble_H;
		}
	}

	private void MuteAllMines()
	{
		for (int i = 0; i < mines.Count; i++)
		{
			if (!mines[i].startExplode && !mines[i].muted && mines[i].dropped)
			{
				mines[i].ManualMute();
			}
		}
	}

	private void ChooseSkill(bool fromMoveAway = false)
	{
		if (!stageSwitched && secondStage)
		{
			attackCDTimer = 0f;
			stageSwitched = true;
			state = MonsterState.BeforeKnock;
			lastSkill = state;
			return;
		}
		float num = Vector3.Distance(base.transform.position, PlayerMgr.Inst.PlayerCtrller.transform.position);
		if (stageSwitched && UnityEngine.Random.Range(0f, 1f) < knockChance && lastSkill != MonsterState.BeforeKnock)
		{
			attackCDTimer = 0f;
			state = MonsterState.BeforeKnock;
			lastSkill = state;
			return;
		}
		float num2 = GeneralTool.GetWeightRandomCompletion(slashChance, doubleSlashChance, laserChance);
		if (num2 == 0f)
		{
			if ((closeToCliff || fromMoveAway) && lastSkill != MonsterState.Slash)
			{
				attackCDTimer = 0f;
				state = skills[0];
				lastSkill = state;
			}
		}
		else if (num2 == 1f)
		{
			if (num > canSlashDistance && lastSkill != MonsterState.DoubleSlashPrepare)
			{
				attackCDTimer = 0f;
				state = skills[1];
				lastSkill = state;
			}
		}
		else if (num > canSlashDistance && num2 == 2f && lastSkill != MonsterState.Charge)
		{
			attackCDTimer = 0f;
			state = skills[2];
			lastSkill = state;
		}
		else if (lastSkill != MonsterState.Mine)
		{
			attackCDTimer = 0f;
			state = MonsterState.Mine;
			lastSkill = state;
		}
	}

	public override void Update()
	{
		hitListClearTimer += Time.deltaTime;
		if (hitListClearTimer > hitListClearTime)
		{
			hitListClearTimer = 0f;
			hitList.Clear();
		}
		for (int i = 0; i < notTailCount; i++)
		{
			lr_bodyConnector.SetPosition(i, Tool2D.GetLayerPoint(bodys[i].transform.position + new Vector3(0f, 0f, 0f - bodys[i].rootHeight)));
			lr_bodyConnectorShadow.SetPosition(i, Tool2D.GetLayerPoint(bodys[i].transform.position, LayerCorrectType.Shadow));
		}
		for (int j = notTailCount - 1; j < bodyCount; j++)
		{
			lr_Tail.SetPosition(j - notTailCount + 1, Tool2D.GetLayerPoint(bodys[j].transform.position + new Vector3(0f, 0f, 0f - bodys[j].rootHeight)));
			lr_TailShadow.SetPosition(j - notTailCount + 1, Tool2D.GetLayerPoint(bodys[j].transform.position, LayerCorrectType.Shadow));
		}
		for (int k = 0; k < upperbody.Count; k++)
		{
			lr_upperBodyConnector.SetPosition(k, Tool2D.GetLayerPoint(upperbody[k].truePosition));
			lr_upperBodyConnectorShadow.SetPosition(k, Tool2D.IgnoreZPoint(upperbody[k].truePosition, 1.05f));
		}
		if (lr_bodyConnector.startColor != myPpt.BaseColor)
		{
			lr_bodyConnector.startColor = myPpt.BaseColor;
			lr_bodyConnector.endColor = myPpt.BaseColor;
			lr_Tail.startColor = myPpt.BaseColor;
			lr_Tail.endColor = myPpt.BaseColor;
			lr_upperBodyConnector.startColor = myPpt.BaseColor;
			lr_upperBodyConnector.endColor = myPpt.BaseColor;
		}
		tsf_HeadShadow.position = Tool2D.IgnoreZPoint(upperbody[5].truePosition, 1.05f);
		ve_Bubble.transform.position = Vector3.zero;
		for (int num = mines.Count - 1; num >= 0; num--)
		{
			if (mines[num].exploded || mines[num].muted)
			{
				mines.RemoveAt(num);
			}
		}
		if ((float)mines.Count > maxMineCount)
		{
			for (int l = 0; (float)l < (float)mines.Count - maxMineCount; l++)
			{
				if (!mines[l].startExplode && !mines[l].muted && mines[l].dropped)
				{
					mines[l].ManualMute();
				}
			}
		}
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
		Debug.DrawRay(base.transform.position, moveDiration * 10f, Color.cyan);
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				bornIdleTimer = 0f;
			}
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = MonsterState.CloseMove;
			}
			break;
		case MonsterState.MoveAway:
			if (changedState)
			{
				if (base.HaveTarget)
				{
					noTargetPoint = new Vector3(Mathf.Clamp(base.transform.position.x, roomCenterPoint.x - roomWidth / 2f + (cliffCheckRange + 2f), roomCenterPoint.x + roomWidth / 2f - (cliffCheckRange + 2f)), Mathf.Clamp(base.transform.position.y, roomCenterPoint.y - roomHeight / 2f + (cliffCheckRange + 2f), roomCenterPoint.y + roomHeight / 2f - (cliffCheckRange + 2f)), 0f);
				}
				GetNavInfo(noTargetPoint);
			}
			moveDiration = Tool2D.IgnoreZPoint(Vector3.RotateTowards(moveDiration, ToPointDir(navInfo.ToGoPoint), dirationChangeSpeedClose * (MathF.PI / 180f) * Time.deltaTime, 0f)).normalized;
			SetMove(moveDiration * base.MoveSpeed);
			CheckNeedRediration();
			if (!closeToCliff)
			{
				ChooseSkill(fromMoveAway: true);
			}
			break;
		case MonsterState.CloseMove:
			if (changedState)
			{
				attackCD.RandomResult();
				base.Anima.Play("Elite9_Move");
				CheckNeedRediration();
				if (!closeToCliff)
				{
					closeMoveRight = ((!((double)UnityEngine.Random.Range(0f, 1f) > 0.5)) ? 1 : (-1));
				}
				noTargetPoint = Tool2D.GetNavMeshPointIngoreZ(roomCenterPoint + new Vector3(UnityEngine.Random.Range((0f - roomWidth) / 3f, roomWidth / 3f), UnityEngine.Random.Range((0f - roomHeight) / 3f, roomHeight / 3f), 0f));
			}
			SetDirationLerp();
			SetMove(moveDiration * base.MoveSpeed);
			CheckNeedRediration();
			attackCDTimer += Time.deltaTime;
			if (attackCDTimer > attackCD.result)
			{
				if (closeToCliff)
				{
					state = MonsterState.MoveAway;
				}
				else
				{
					ChooseSkill();
				}
			}
			break;
		case MonsterState.SwitchStage:
			if (changedState)
			{
				base.Anima.Play("Elite9_SwitchStage");
				closeMoveRight = ((!((double)UnityEngine.Random.Range(0f, 1f) > 0.5)) ? 1 : (-1));
				noTargetPoint = Tool2D.GetNavMeshPointIngoreZ(roomCenterPoint + new Vector3(UnityEngine.Random.Range((0f - roomWidth) / 3f, roomWidth / 3f), UnityEngine.Random.Range((0f - roomHeight) / 3f, roomHeight / 3f), 0f));
				arms[0].state = Elite9_Arm.LegState.Prepare;
				arms[1].state = Elite9_Arm.LegState.Prepare;
			}
			SetDirationLerp();
			SetMove(moveDiration * base.MoveSpeed);
			break;
		case MonsterState.Slash:
			if (changedState)
			{
				if (!base.HaveTarget)
				{
					GetNearestTargetPlayerFirst();
				}
				if (!base.HaveTarget)
				{
					state = MonsterState.CloseMove;
					break;
				}
				slashCount = 0f;
				maxSlashTime.RandomResult();
				secondStageMaxSlashTime.RandomResult();
				base.Anima.Play("Elite9_SlashPrepare");
				arms[0].state = Elite9_Arm.LegState.BeforeAttack;
				arms[1].state = Elite9_Arm.LegState.BeforeAttack;
				slashSpeedNormal = false;
				slashRotate = true;
			}
			if (base.HaveTarget && slashRotate)
			{
				GetNavInfo(base.TargetPoint);
				moveDiration = Tool2D.IgnoreZPoint(Vector3.RotateTowards(moveDiration, ToPointDir(navInfo.ToGoPoint), dirationChangeSpeed * (MathF.PI / 180f) * Time.deltaTime, 0f)).normalized;
			}
			if (!slashSpeedNormal)
			{
				SetMove(moveDiration * base.MoveSpeed * laserMoveSpeedFix);
			}
			else
			{
				SetMove(moveDiration * base.MoveSpeed);
			}
			break;
		case MonsterState.DoubleSlashPrepare:
			if (changedState)
			{
				doubleSlashAimStop = false;
				if (!base.HaveTarget)
				{
					GetNearestTargetPlayerFirst();
				}
				if (!base.HaveTarget)
				{
					state = MonsterState.CloseMove;
					break;
				}
				base.Anima.Play("Elite9_DoubleSlashPrepare");
				arms[0].state = Elite9_Arm.LegState.BeforeAttack;
				arms[1].state = Elite9_Arm.LegState.BeforeAttack;
				slashSpeedNormal = false;
			}
			if (base.HaveTarget && !doubleSlashAimStop)
			{
				GetNavInfo(base.TargetPoint);
				moveDiration = Tool2D.IgnoreZPoint(Vector3.RotateTowards(moveDiration, ToPointDir(navInfo.ToGoPoint), dirationChangeSpeedClose * (MathF.PI / 180f) * Time.deltaTime, 0f)).normalized;
			}
			if (!slashSpeedNormal)
			{
				SetMove(moveDiration * base.MoveSpeed * laserMoveSpeedFix);
			}
			else
			{
				SetMove(moveDiration * base.MoveSpeed);
			}
			break;
		case MonsterState.DoubleSlashChase:
		{
			if (changedState)
			{
				base.Anima.Play("Elite9_DoubleSlashChase");
				slashSpeedNormal = true;
			}
			bool flag = false;
			if (base.HaveTarget)
			{
				GetNavInfo(base.TargetPoint);
				moveDiration = Tool2D.IgnoreZPoint(Vector3.RotateTowards(moveDiration, ToPointDir(navInfo.ToGoPoint), doubleSlashDirationChangeSpeed * (MathF.PI / 180f) * Time.deltaTime, 0f)).normalized;
				Vector3 dir = Tool2D.GetDir(moveDiration, moveFixAngle);
				flag = Tool2D.IgnoreZAngle(base.TargetPoint - Tool2D.IgnoreZPoint(upperbody[upperbody.Count - 2].truePosition), dir) < doubleSlashAngle / 2f && ToTargetDistanceSqr() < doubleSlashTargetDistance * doubleSlashTargetDistance;
			}
			CheckNeedRediration();
			if (flag || stateExistTime > doubleSlashChaseTime || (closeToCliff && needRediration))
			{
				state = MonsterState.DoubleSlash;
			}
			SetMove(moveDiration * base.MoveSpeed * doubleSlashSpeedFix);
			break;
		}
		case MonsterState.DoubleSlash:
			if (changedState)
			{
				base.Anima.Play("Elite9_DoubleSlash");
				slashRotate = true;
			}
			SetMove(moveDiration * base.MoveSpeed);
			break;
		case MonsterState.Mine:
			if (changedState)
			{
				base.Anima.Play("Elite9_MineAttack");
				arms[0].state = Elite9_Arm.LegState.Prepare;
				arms[1].state = Elite9_Arm.LegState.Prepare;
			}
			SetDirationLerp();
			moveSpeedLerped = Mathf.Lerp(base.MoveSpeed * laserMoveSpeedFix, base.MoveSpeed, stateExistTime / 1f);
			SetMove(moveDiration * moveSpeedLerped);
			break;
		case MonsterState.Charge:
			if (changedState)
			{
				as_LaserCharge.Play();
				base.Anima.Play("Elite9_LaserAttack");
				if (!base.HaveTarget)
				{
					GetNearestTargetPlayerFirst();
				}
				arms[0].state = Elite9_Arm.LegState.Prepare;
				arms[1].state = Elite9_Arm.LegState.Prepare;
				chargeParticle.Play();
				chargeTimer = 0f;
			}
			if (base.HaveTarget)
			{
				GetNavInfo(base.TargetPoint);
				moveDiration = Tool2D.IgnoreZPoint(Vector3.RotateTowards(moveDiration, Tool2D.IgnoreZPoint(ToPointDir(navInfo.ToGoPoint)), dirationChangeSpeedClose * (MathF.PI / 180f) * Time.deltaTime, 0f)).normalized;
			}
			SetMove(moveDiration * base.MoveSpeed * laserMoveSpeedFix);
			chargeTimer += Time.deltaTime;
			if (chargeTimer > chargeTime)
			{
				as_LaserCharge.Stop();
				chargeParticle.Stop();
				state = MonsterState.LaserAttack;
			}
			break;
		case MonsterState.LaserAttack:
			if (changedState)
			{
				laserAttackPpt.Clear();
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					attackAngleOffset.RandomResult();
					attackDiration = Tool2D.GetDir(ToPointDir(base.TargetPoint), attackAngleOffset.result).normalized;
				}
				else
				{
					attackDiration = moveDiration.normalized;
				}
				nowAttackDistance = 0f;
				if (stageSwitched)
				{
					isHorizontal = UnityEngine.Random.Range(0f, 1f) > 0.5f;
				}
				laserHorizontalRight = ((!(UnityEngine.Random.Range(0f, 1f) > 0.5f)) ? 1 : (-1));
				laserOutOfCliff = false;
				if (isHorizontal)
				{
					if (base.HaveTarget)
					{
						if (ToTargetDistance() < attackCloseDistance)
						{
							laserHorizonRootPoint = base.transform.position + horizontalMinDistance * attackDiration;
						}
						else
						{
							laserHorizonRootPoint = base.TargetPointIgnoreZ;
						}
					}
					else
					{
						laserHorizonRootPoint = base.transform.position + horizontalMinDistance * attackDiration;
					}
					attackingPoint = laserHorizonRootPoint - Tool2D.GetDir(attackDiration, 90f) * attackDistance / 2f * laserHorizontalRight;
					attackDirationHorizontal = Tool2D.GetDir(attackDiration, 90f * laserHorizontalRight);
					if (Mathf.Abs(roomCenterPoint.x - laserHorizonRootPoint.x) > roomWidth / 2f || Mathf.Abs(roomCenterPoint.y - laserHorizonRootPoint.y) > roomHeight / 2f)
					{
						isHorizontal = false;
					}
				}
				if (!isHorizontal)
				{
					attackingPoint = base.transform.position + attackDiration * attackCloseDistance;
					string text = "EF_Elite9_HotGround";
					if (GameMgr.IsHarmony_Static)
					{
						text = "EF_Elite9_HotGround_H";
					}
					currentLaserGround = MiniPool.GetGO("Prefabs/EF/" + text, attackingPoint, 40f).GetComponent<Elite9_HotGround>();
					currentLaserGround.fadeSpeed = laserSpeed;
					currentLaserGround.master = this;
					CamController.Inst.SetShock(0.05f, 3f, laserShakeTime);
					LaserAttackOnce(attackingPoint);
				}
				laserParticle.transform.position = Tool2D.GetLayerPoint(attackingPoint);
				as_LaserLoop.Play();
				base.Anima.Play("Elite9_LaserAttacking");
				attackParticle.Play();
				laserParticle.Play();
			}
			moveDiration = Tool2D.IgnoreZPoint(Vector3.RotateTowards(moveDiration, attackDiration, dirationChangeSpeed * (MathF.PI / 180f) * Time.deltaTime, 0f)).normalized;
			SetMove(moveDiration * base.MoveSpeed * laserMoveSpeedFix);
			if (isHorizontal)
			{
				if (currentLaserGround == null)
				{
					string text2 = "EF_Elite9_HotGround";
					if (GameMgr.IsHarmony_Static)
					{
						text2 = "EF_Elite9_HotGround_H";
					}
					if (Mathf.Abs(roomCenterPoint.x - attackingPoint.x) < roomWidth / 2f - 0.5f && Mathf.Abs(roomCenterPoint.y - attackingPoint.y) < roomHeight / 2f - 0.5f)
					{
						currentLaserGround = MiniPool.GetGO("Prefabs/EF/" + text2, attackingPoint, 40f).GetComponent<Elite9_HotGround>();
						currentLaserGround.fadeSpeed = laserSpeed;
						currentLaserGround.master = this;
					}
				}
				attackingPoint += attackDirationHorizontal * laserSpeed * Time.deltaTime;
				nowAttackDistance += laserSpeed * Time.deltaTime;
				nowAttackDistanceModular += laserSpeed * Time.deltaTime;
				if ((double)nowAttackDistanceModular > 0.1)
				{
					nowAttackDistanceModular = 0f;
					if (!laserOutOfCliff && currentLaserGround != null)
					{
						LaserAttackOnce(attackingPoint);
					}
				}
				if (currentLaserGround != null)
				{
					CollisionFilter collisionFilter = default(CollisionFilter);
					collisionFilter.BelongsTo = 1073741824u;
					collisionFilter.CollidesWith = 65536u;
					collisionFilter.GroupIndex = 0;
					CollisionFilter filter = collisionFilter;
					UnityEngine.Ray ray = default(UnityEngine.Ray);
					ray.direction = attackDirationHorizontal;
					ray.origin = attackingPoint;
					if (UnitDotsSyncSystem.Raycast(ray, 0.2f, filter))
					{
						laserOutOfCliff = true;
					}
					if (!laserOutOfCliff && currentLaserGround != null)
					{
						currentLaserGround.transform.position = attackingPoint;
					}
				}
			}
			else
			{
				attackingPoint += attackDiration * laserSpeed * Time.deltaTime;
				nowAttackDistance += laserSpeed * Time.deltaTime;
				nowAttackDistanceModular += laserSpeed * Time.deltaTime;
				if (nowAttackDistanceModular > laserAttackRadius * 2f && currentLaserGround != null)
				{
					nowAttackDistanceModular = 0f;
					if (!laserOutOfCliff)
					{
						LaserAttackOnce(attackingPoint);
					}
				}
				CollisionFilter collisionFilter = default(CollisionFilter);
				collisionFilter.BelongsTo = 1073741824u;
				collisionFilter.CollidesWith = 65536u;
				collisionFilter.GroupIndex = 0;
				CollisionFilter filter2 = collisionFilter;
				UnityEngine.Ray ray = default(UnityEngine.Ray);
				ray.direction = attackDiration;
				ray.origin = attackingPoint;
				if (UnitDotsSyncSystem.Raycast(ray, 0.2f, filter2))
				{
					laserOutOfCliff = true;
				}
				if (!laserOutOfCliff)
				{
					currentLaserGround.transform.position = attackingPoint;
				}
			}
			laserParticle.transform.position = Tool2D.GetLayerPoint(attackingPoint);
			lr_Laser.SetPosition(0, tsf_Head.position);
			lr_Laser.SetPosition(1, Tool2D.GetLayerPoint(attackingPoint));
			if (nowAttackDistance > attackDistance)
			{
				as_LaserLoop.Stop();
				attackParticle.Stop();
				laserParticle.Stop();
				state = MonsterState.Rest;
				break;
			}
			laserShakeTimer += Time.deltaTime;
			if (laserShakeTimer > laserShakeTime)
			{
				laserShakeTimer = 0f;
				CamController.Inst.SetShock(0.05f, 3f, laserShakeTime);
			}
			break;
		case MonsterState.Rest:
			if (changedState)
			{
				currentLaserGround = null;
				as_LaserEnd.Play();
				base.Anima.Play("Elite9_LaserAttackFinish");
				restTimer = 0f;
				restParticle.Play();
				ve_Bubble.gameObject.SetActive(value: false);
				arms[0].state = Elite9_Arm.LegState.AfterPrepare;
				arms[1].state = Elite9_Arm.LegState.AfterPrepare;
				ve_Bubble.SetFloat("Count", Tool2D.IgnoreZDistance(lr_Laser.GetPosition(0), lr_Laser.GetPosition(1)) * bubbleCountPerMeter);
				ve_Bubble.SetVector3("Position0", lr_Laser.GetPosition(0));
				ve_Bubble.SetVector3("Position1", lr_Laser.GetPosition(1));
				if (!GameMgr.IsMobile_Static)
				{
					ve_Bubble.gameObject.SetActive(value: true);
				}
				lr_Laser.SetPosition(0, tsf_Head.position);
				lr_Laser.SetPosition(1, tsf_Head.position);
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (!base.HaveTarget)
			{
				SetMove(moveDiration * base.MoveSpeed * laserMoveSpeedFix);
			}
			else
			{
				nowMoveAngle += moveAngleFrequency * MathF.PI * 2f * Time.deltaTime;
				GetNavInfo(base.TargetPoint - ToTargetDir() * closeDistance + Tool2D.GetDir(ToTargetDir(), 90f * closeMoveRight).normalized);
				moveDiration = Tool2D.IgnoreZPoint(Vector3.RotateTowards(moveDiration, ToPointDir(navInfo.ToGoPoint), dirationChangeSpeed * (MathF.PI / 180f) * Time.deltaTime, 0f)).normalized;
				SetMove(moveDiration * base.MoveSpeed * laserMoveSpeedFix);
			}
			restTimer += Time.deltaTime;
			if (restTimer > restTime)
			{
				restParticle.Stop();
				state = MonsterState.CloseMove;
			}
			break;
		case MonsterState.BeforeKnock:
			if (changedState)
			{
				base.Anima.Play("Elite9_BeforeKnock");
				noTargetPoint = Tool2D.GetNavMeshPointIngoreZ(roomCenterPoint + new Vector3(UnityEngine.Random.Range((0f - roomWidth) / 3f, roomWidth / 3f), UnityEngine.Random.Range((0f - roomHeight) / 3f, roomHeight / 3f), 0f));
			}
			SetDirationLerp();
			SetMove(moveDiration * base.MoveSpeed * knockSpeedFix);
			break;
		case MonsterState.Knock:
			if (changedState)
			{
				base.Anima.Play("Elite9_Knock");
			}
			SetDirationLerp();
			SetMove(moveDiration * knockSpeedFix * base.MoveSpeed);
			break;
		case MonsterState.ArmTest:
		case MonsterState.Idle:
			break;
		}
	}

	private void SetDirationLerp(bool forceNoTarget = false)
	{
		if (!base.HaveTarget)
		{
			GetNearestTargetPlayerFirst();
		}
		if (base.HaveTarget && !forceNoTarget)
		{
			GetNavInfo(base.TargetPoint - ToTargetDir() * closeDistance + Tool2D.GetDir(ToTargetDir(), 90f * closeMoveRight).normalized * 2f);
			if (needRediration)
			{
				Debug.DrawLine(base.transform.position, base.TargetPointIgnoreZ - ToTargetDir() * closeDistance + Tool2D.GetDir(ToTargetDir(), 90f * closeMoveRight).normalized * 2f, Color.cyan);
			}
			Debug.DrawRay(base.transform.position, cliffMoveDiration * 10f, Color.black);
			Debug.DrawLine(base.transform.position, base.TargetPointIgnoreZ - ToTargetDir() * closeDistance + Tool2D.GetDir(ToTargetDir(), 90f * closeMoveRight).normalized * 2f, Color.red);
		}
		else
		{
			GetNavInfo(noTargetPoint - (noTargetPoint - base.transform.position).normalized * closeDistance + Tool2D.GetDir(noTargetPoint - base.transform.position, 90f * closeMoveRight).normalized * 2f);
		}
		CheckNeedRediration();
		if (needRediration)
		{
			moveDiration = Tool2D.RotateTowardsAroundZAxis(moveDiration, cliffMoveDiration, dirationChangeSpeedClose * Time.deltaTime).normalized;
		}
		else
		{
			moveDiration = Tool2D.RotateTowardsAroundZAxis(moveDiration, ToPointDir(navInfo.ToGoPoint), dirationChangeSpeed * Time.deltaTime).normalized;
		}
	}

	public void CheckNeedRediration()
	{
		if (Mathf.Abs(roomCenterPoint.x - base.transform.position.x) > roomWidth / 2f - cliffCheckRange || Mathf.Abs(roomCenterPoint.y - base.transform.position.y) > roomHeight / 2f - cliffCheckRange)
		{
			closeToCliff = true;
		}
		else
		{
			closeToCliff = false;
		}
		needRediration = false;
		cliffMoveDiration = Vector3.zero;
		if (!closeToCliff)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		if (Mathf.Abs(roomCenterPoint.x + roomWidth / 2f - base.transform.position.x) < cliffCheckRange)
		{
			flag4 = true;
		}
		if (Mathf.Abs(roomCenterPoint.x - roomWidth / 2f - base.transform.position.x) < cliffCheckRange)
		{
			flag3 = true;
		}
		if (Mathf.Abs(roomCenterPoint.y - roomHeight / 2f - base.transform.position.y) < cliffCheckRange)
		{
			flag2 = true;
		}
		if (Mathf.Abs(roomCenterPoint.y + roomHeight / 2f - base.transform.position.y) < cliffCheckRange)
		{
			flag = true;
		}
		bool flag5 = (flag && flag4) || (flag4 && flag2) || (flag2 && flag3) || (flag3 && flag);
		if (closeMoveRight > 0f)
		{
			if (flag4 && !flag2)
			{
				cliffMoveDiration = Vector3.down;
			}
			else if (flag && !flag4)
			{
				cliffMoveDiration = Vector3.right;
			}
			else if (flag3 && !flag)
			{
				cliffMoveDiration = Vector3.up;
			}
			else if (flag2 && !flag3)
			{
				cliffMoveDiration = Vector3.left;
			}
			Debug.DrawRay(base.transform.position, ToPointDir(navInfo.ToGoPoint) * 10f, Color.white);
			needRediration = Tool2D.IgnoreZAngleWithSign(ToPointDir(navInfo.ToGoPoint), cliffMoveDiration) < 0f || flag5;
		}
		else
		{
			if (flag4 && !flag)
			{
				cliffMoveDiration = Vector3.up;
			}
			else if (flag && !flag3)
			{
				cliffMoveDiration = Vector3.left;
			}
			else if (flag3 && !flag2)
			{
				cliffMoveDiration = Vector3.down;
			}
			else if (flag2 && !flag4)
			{
				cliffMoveDiration = Vector3.right;
			}
			Debug.DrawRay(base.transform.position, ToPointDir(navInfo.ToGoPoint) * 10f, Color.white);
			needRediration = Tool2D.IgnoreZAngleWithSign(ToPointDir(navInfo.ToGoPoint), cliffMoveDiration) > 0f || flag5;
		}
	}

	private void LaserAttackOnce(Vector3 explodePoint)
	{
		UnitDotsSyncSystem.GetCollidersInRange(explodePoint, laserAttackRadius, GameConst.Filter_Friendly, laserHitTargets);
		for (int i = 0; i < laserHitTargets.Count; i++)
		{
			Entity entity = laserHitTargets[i].entity;
			if (!laserAttackPpt.Contains(entity))
			{
				laserAttackPpt.Add(entity);
				currentLaserGround.AddAttackedPpt(entity);
				MiniPool.GetGO("Prefabs/EF/EF_Elite9_Hit" + (GameMgr.IsHarmony_Static ? "_H" : ""), laserHitTargets[i].point, 2f);
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
				info.ignorePlayerInvincibleFrame = true;
				info.teammateTakeDamageRatio = 3f;
				info.damage = laserAttackDamage;
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
			}
		}
	}

	private void BladeAttackOnce()
	{
		UnitDotsSyncSystem.GetCollidersInRange(Tool2D.IgnoreZPoint(upperbody[upperbody.Count - 2].truePosition), doubleSlashDistance, GameConst.Filter_Friendly, bladeHitTargets);
		Vector3 dir = Tool2D.GetDir(moveDiration, moveFixAngle);
		for (int i = 0; i < bladeHitTargets.Count; i++)
		{
			if (!(Tool2D.IgnoreZAngle(bladeHitTargets[i].point - Tool2D.IgnoreZPoint(upperbody[upperbody.Count - 2].truePosition), dir) < slashBulletAngle / 2f))
			{
				continue;
			}
			Entity entity = bladeHitTargets[i].entity;
			if (!laserAttackPpt.Contains(entity))
			{
				if (!GameMgr.IsHarmony_Static)
				{
					MiniPool.GetGO("Prefabs/EF/EF_Elite9_BladeHit", bladeHitTargets[i].point, 2f);
				}
				else
				{
					MiniPool.GetGO("Prefabs/EF/EF_Elite9_BladeHit_H", bladeHitTargets[i].point, 2f);
				}
				laserAttackPpt.Add(entity);
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
				info.ignorePlayerInvincibleFrame = true;
				info.teammateTakeDamageRatio = 3f;
				info.damage = laserAttackDamage;
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
			}
		}
	}

	private void BladeAttackDouble()
	{
		UnitDotsSyncSystem.GetCollidersInRange(Tool2D.IgnoreZPoint(upperbody[upperbody.Count - 2].truePosition), doubleSlashDistance, GameConst.Filter_Friendly, bladeHitTargets);
		Vector3 dir = Tool2D.GetDir(moveDiration, moveFixAngle);
		for (int i = 0; i < bladeHitTargets.Count; i++)
		{
			if (!(Tool2D.IgnoreZAngle(bladeHitTargets[i].point - Tool2D.IgnoreZPoint(upperbody[upperbody.Count - 2].transform.position), dir) < doubleSlashAngle / 2f))
			{
				continue;
			}
			Entity entity = bladeHitTargets[i].entity;
			if (!laserAttackPpt.Contains(entity))
			{
				if (!GameMgr.IsHarmony_Static)
				{
					MiniPool.GetGO("Prefabs/EF/EF_Elite9_BladeHit", bladeHitTargets[i].point, 2f);
				}
				else
				{
					MiniPool.GetGO("Prefabs/EF/EF_Elite9_BladeHit_H", bladeHitTargets[i].point, 2f);
				}
				laserAttackPpt.Add(entity);
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
				info.ignorePlayerInvincibleFrame = true;
				info.teammateTakeDamageRatio = 3f;
				info.damage = laserAttackDamage;
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
			}
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (info.spell.Entity != Entity.Null)
		{
			if (!hitList.Contains(info.spell.Entity))
			{
				hitList.Add(info.spell.Entity);
			}
			else
			{
				info.immuneDamage = true;
			}
		}
	}

	protected override void BossDeadStay()
	{
		myPpt.ChangeColor(myPpt.Color_NormalBody);
		lr_bodyConnector.startColor = Color.white;
		lr_bodyConnector.endColor = Color.white;
		lr_Tail.startColor = Color.white;
		lr_Tail.endColor = Color.white;
		lr_upperBodyConnector.startColor = Color.white;
		lr_upperBodyConnector.endColor = Color.white;
		base.BossDeadStay();
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		MuteAllMines();
		for (int i = 0; i < invisibleBodys.Count; i++)
		{
			invisibleBodys[i].DotsAnnouncedDeath();
		}
		for (int num = bodys.Count - 1; num >= 0; num--)
		{
			UnityEngine.Object.Destroy(bodys[num].gameObject);
		}
		for (int num2 = arms.Count - 1; num2 >= 0; num2--)
		{
			UnityEngine.Object.Destroy(arms[num2].gameObject);
		}
	}

	private IEnumerator ShootBullet()
	{
		slashBulletAngleOffset.RandomResult();
		for (int i = 0; (float)i < slashBulletCount; i++)
		{
			float num = slashBulletAngle / (slashBulletCount - 1f) * (float)i - slashBulletAngle / 2f + slashBulletAngleOffset.result;
			sipBullet.shootDirection = Tool2D.GetDir(moveDiration, (rightArmMove ? (0f - num) : num) + moveFixAngle).normalized;
			string text = "EF_Elite9_BladeWave";
			if (GameMgr.IsHarmony_Static)
			{
				text = "EF_Elite9_BladeWave_H";
			}
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, Tool2D.IgnoreZPoint(base.transform.position) + sipBullet.shootDirection * bulletDistance).GetComponent<Elite9_BladeWaves>().Initialize(sipBullet.shootDirection, myPpt);
			yield return new WaitForSeconds(slashBulletTime / slashBulletCount);
		}
	}

	public void GroundKnock(Vector3 position)
	{
		Invoke("KnockShockDelay", 0.1f);
		MiniPool.GetGO("Prefabs/EF/EF_Elite9_GroundTrace", position, 15f);
		CamController.Inst.SetShock(knockParam);
		slashBulletAngleOffset.RandomResult();
		knockBulletCount.RandomResult();
		float num = UnityEngine.Random.Range(0f, 360f);
		string text = "EF_Elite9_BladeWaveVertical";
		if (GameMgr.IsHarmony_Static)
		{
			text = "EF_Elite9_BladeWaveVertical_H";
		}
		for (int i = 0; i < knockBulletCount.result; i++)
		{
			float num2 = (float)(360 / knockBulletCount.result * i) - slashBulletAngle / 2f + slashBulletAngleOffset.result;
			Vector3 normalized = Tool2D.GetDir(moveDiration, num2 + num).normalized;
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, Tool2D.IgnoreZPoint(position) + normalized * 0.5f).GetComponent<Elite9_BladeWaves>().Initialize(normalized, myPpt);
		}
	}

	private void KnockShockDelay()
	{
		SEMgr.Inst.elite9Knock.PlaySE();
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "HeadUp":
			headUp = true;
			break;
		case "HeadDown":
			headUp = false;
			break;
		case "ArmMove":
			if (rightArmMove)
			{
				arms[0].state = Elite9_Arm.LegState.Lift;
				rightArmMove = false;
			}
			else
			{
				arms[1].state = Elite9_Arm.LegState.Lift;
				rightArmMove = true;
			}
			break;
		case "Mine":
		{
			GetNearestTargetPlayerFirst();
			SEMgr.Inst.elite9Vomit.PlaySE();
			string text = "EF_Elite9_Mine";
			if (GameMgr.IsHarmony_Static)
			{
				text = "EF_Elite9_Mine_H";
			}
			if (base.HaveTarget)
			{
				for (int i = 0; i < mineCount; i++)
				{
					mineRange.RandomResult();
					Elite9_Mine component = MiniPool.GetGO("Prefabs/EF/" + text, base.transform.position - new Vector3(0f, 0f, tsf_Head.position.y - base.transform.position.y)).GetComponent<Elite9_Mine>();
					component.master = this;
					mines.Add(component);
					component.SetTarget(Tool2D.GetNavMeshPointIngoreZ(base.TargetPoint + Tool2D.GetDir() * mineRange.result, 0f), tsf_Head.position.y - base.transform.position.y);
				}
			}
			else
			{
				for (int j = 0; j < mineCount; j++)
				{
					mineRange.RandomResult();
					Elite9_Mine component2 = MiniPool.GetGO("Prefabs/EF/" + text, base.transform.position - new Vector3(0f, 0f, tsf_Head.position.y - base.transform.position.y)).GetComponent<Elite9_Mine>();
					component2.master = this;
					mines.Add(component2);
					component2.SetTarget(Tool2D.GetNavMeshPointIngoreZ(roomCenterPoint + new Vector3(UnityEngine.Random.Range((0f - roomWidth) / 2f, roomWidth / 2f), UnityEngine.Random.Range((0f - roomHeight) / 2f, roomHeight / 2f), 0f)), tsf_Head.position.y - base.transform.position.y);
				}
			}
			for (int k = 0; k < extraMineCount; k++)
			{
				Elite9_Mine component3 = MiniPool.GetGO("Prefabs/EF/" + text, base.transform.position - new Vector3(0f, 0f, tsf_Head.position.y - base.transform.position.y)).GetComponent<Elite9_Mine>();
				component3.master = this;
				mines.Add(component3);
				component3.SetTarget(Tool2D.GetNavMeshPointIngoreZ(roomCenterPoint + new Vector3(UnityEngine.Random.Range((0f - roomWidth) / 2f, roomWidth / 2f), UnityEngine.Random.Range((0f - roomHeight) / 2f, roomHeight / 2f), 0f)), tsf_Head.position.y - base.transform.position.y);
			}
			break;
		}
		case "MineFinish":
			state = MonsterState.CloseMove;
			break;
		case "ArmDrop":
			arms[0].state = Elite9_Arm.LegState.AfterPrepare;
			arms[1].state = Elite9_Arm.LegState.AfterPrepare;
			break;
		case "DoubleAttackHint":
			SEMgr.Inst.elite9BeforeSlash.PlaySE();
			hintParticle.Play();
			break;
		case "DoubleAttackPrepareFinish":
			state = MonsterState.DoubleSlashChase;
			break;
		case "DoubleSlashStart":
			SEMgr.Inst.elite9DoubleSlash.PlaySE();
			arms[0].state = Elite9_Arm.LegState.Attack;
			arms[1].state = Elite9_Arm.LegState.Attack;
			break;
		case "DoubleSlashAimStop":
			doubleSlashAimStop = true;
			break;
		case "DoubleAttack":
			CamController.Inst.SetShock(doubleSlashShockParam);
			BladeAttackDouble();
			break;
		case "DoubleAttackAgain":
			if (stageSwitched)
			{
				if (slashAgain)
				{
					slashAgain = false;
					state = MonsterState.DoubleSlashPrepare;
				}
				else
				{
					slashAgain = true;
					state = MonsterState.CloseMove;
				}
			}
			break;
		case "BeforeKnockStart":
			arms[0].state = Elite9_Arm.LegState.BeforeKnock;
			arms[1].state = Elite9_Arm.LegState.BeforeKnock;
			break;
		case "BeforeKnockFinish":
			knockTimer = 0f;
			state = MonsterState.Knock;
			break;
		case "KnockStart":
			knockTimer += 1f;
			if (rightArmMove)
			{
				arms[0].state = Elite9_Arm.LegState.Knock;
			}
			else
			{
				arms[1].state = Elite9_Arm.LegState.Knock;
			}
			break;
		case "KnockLift":
			if (knockTimer >= knockTime)
			{
				state = MonsterState.CloseMove;
			}
			else if (rightArmMove)
			{
				rightArmMove = false;
				if (arms[1].state != Elite9_Arm.LegState.LockHeight)
				{
					arms[1].state = Elite9_Arm.LegState.BeforeKnock;
				}
			}
			else if (!rightArmMove)
			{
				rightArmMove = true;
				if (arms[0].state != Elite9_Arm.LegState.LockHeight)
				{
					arms[0].state = Elite9_Arm.LegState.BeforeKnock;
				}
			}
			break;
		case "KnockFinish":
			if (knockTimer >= knockTime)
			{
				state = MonsterState.CloseMove;
			}
			else
			{
				base.Anima.Play("Elite9_Knock", 0, 0f);
			}
			break;
		case "ShoutDone":
			shoutParticle.Stop();
			break;
		case "ShoutFinish":
			state = MonsterState.CloseMove;
			break;
		case "Shout":
			SEMgr.Inst.elite9Shout.PlaySE();
			shoutParticle.Play();
			break;
		case "DoubleAttackFinish":
			if (stageSwitched)
			{
				if (slashAgain)
				{
					slashAgain = false;
					state = MonsterState.DoubleSlashPrepare;
				}
				else
				{
					slashAgain = true;
					state = MonsterState.CloseMove;
				}
			}
			else
			{
				state = MonsterState.CloseMove;
			}
			break;
		case "BladeAttack":
			CamController.Inst.SetShock(slashShockParam);
			BladeAttackOnce();
			break;
		case "BladeWave":
			StartCoroutine(ShootBullet());
			break;
		case "SlashPrepareFinish":
			base.Anima.Play("Elite9_Slash");
			break;
		case "SlashStart":
			SEMgr.Inst.elite9Slash.PlaySE();
			slashSpeedNormal = false;
			if (rightArmMove)
			{
				arms[0].state = Elite9_Arm.LegState.Attack;
				if (arms[1].state != Elite9_Arm.LegState.BeforeAttack && arms[1].state != Elite9_Arm.LegState.LockHeight && arms[1].state != Elite9_Arm.LegState.AfterAttack)
				{
					arms[1].state = Elite9_Arm.LegState.AfterAttack;
				}
				rightArmMove = false;
			}
			else
			{
				arms[1].state = Elite9_Arm.LegState.Attack;
				if (arms[0].state != Elite9_Arm.LegState.BeforeAttack && arms[0].state != Elite9_Arm.LegState.LockHeight && arms[0].state != Elite9_Arm.LegState.AfterAttack)
				{
					arms[0].state = Elite9_Arm.LegState.AfterAttack;
				}
				rightArmMove = true;
			}
			slashCount += 1f;
			break;
		case "SlashAnimaEnd":
		{
			slashSpeedNormal = true;
			bool flag = false;
			if (state == MonsterState.Slash)
			{
				if (stageSwitched)
				{
					if (slashCount >= secondStageMaxSlashTime.result)
					{
						flag = true;
					}
				}
				else if (slashCount >= maxSlashTime.result)
				{
					flag = true;
				}
			}
			if (flag)
			{
				base.Anima.Play("Elite9_SlashFinish");
			}
			else
			{
				base.Anima.Play("Elite9_Slash", 0, 0f);
			}
			break;
		}
		case "SlashArmFinish":
			slashSpeedNormal = true;
			arms[0].state = Elite9_Arm.LegState.AttackBack;
			arms[1].state = Elite9_Arm.LegState.AttackBack;
			break;
		case "SlashFinishAnimaEnd":
			state = MonsterState.CloseMove;
			break;
		}
	}
}
