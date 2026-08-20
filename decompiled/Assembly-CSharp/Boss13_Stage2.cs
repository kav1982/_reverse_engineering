using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics.Stateful;
using UnityEngine;

public class Boss13_Stage2 : UnitBase, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	public enum MonsterState
	{
		BornIdle,
		ShowIdle,
		Idle,
		RandomMove,
		Move,
		DashCharge,
		Dash,
		DashFinish,
		FollowMissileAim,
		FollowMissile,
		SubStrafeReady,
		CrossStrafe,
		MineCharge,
		Mine,
		FalculaCharge,
		FalculaStay,
		FalculaFail,
		Dead
	}

	[Header("通用属性")]
	public VariableFloat attackCD;

	public float attackCDTimer;

	public Transform motion;

	public static Boss13_Stage2 Inst;

	public bool isAttacking;

	public bool isDying;

	public float deadTime;

	private bool subActive;

	public TestController controller;

	public float fallTime;

	private bool dialogueActive;

	[Header("技能选择")]
	public float mineChance;

	public float dashChance;

	public float missileChance;

	public float areaMissileChance;

	public float falculaChance;

	public int lastAttackType;

	[Header("动画")]
	public GameObject moveAnimaObj;

	public GameObject actionAnimaObj;

	public Animator actionAnima;

	public Transform rotationParent;

	[Header("震屏")]
	public ShockParam shockParam;

	[Header("待机")]
	public VariableFloat idleTime;

	public float checkIntervalTime;

	public float checkIntervalTimer;

	[Header("随机移动")]
	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	private Vector3 randomMoveTarget;

	public VariableFloat keepDistance;

	public float keepDistanceAngle;

	[Header("冲锋")]
	public float dashSpeed;

	public float dashChargeTime;

	public float dashTime;

	private Vector3 dashDir;

	public LineRenderer dashWarningLine;

	public LineRenderer dashWarningLine_H;

	public ParticleSystem dashEffect;

	public int dashCounter;

	public float dashBulletSpawnInterval;

	public float dashBulletSpawnTimer;

	public float dashRotateSpeed;

	public float dashAgainMinAngle;

	public float dashFinishTime;

	public VariableInt dashMaxTimes;

	private bool dashWall;

	public Boss13DashDamageCheck dashDamageCheck;

	private Vector3 roomCenter;

	private float roomWidth;

	private float roomHeight;

	private Vector3 fakeTarget;

	private Vector3 noTargetDir;

	[Header("水雷")]
	public float mineChargeTime;

	public int mineCount;

	public int mineMaxAmount;

	public List<Boss13_Mine> boss13_Mines = new List<Boss13_Mine>();

	[Header("跟踪弹")]
	public float aimTime;

	public int missileAmount;

	public Transform missilePivot;

	public float missileOriginHeight;

	public ParticleSystem fireEffect;

	[Header("交叉扫射")]
	public bool isStrafing;

	public float generatePosOffset;

	public Boss13FakeSub fakeSub;

	public float subStrafeReadyTime;

	[Header("钩爪")]
	public Transform falculaPivot;

	public float falculaChargeTime;

	public float falculaFailTime;

	public float falculaAttackTime;

	public float hitDamage;

	public float hitKnockBack;

	private Boss13_FalculaHead falculaHead;

	[Header("取消闪红")]
	public SpriteRenderer[] cancelRedSprites;

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private Vector3 towardDir = Vector3.right;

	public Entity thisEntity { get; set; }

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
		if (GameMgr.IsMobile_Static)
		{
			dashTime *= 1.2f;
			dashBulletSpawnInterval *= 1.2f;
		}
		SpriteRenderer[] array = cancelRedSprites;
		foreach (SpriteRenderer srToRemove in array)
		{
			myPpt.RemoveSRFromArray(srToRemove);
		}
		if (GameMgr.IsChAge14_Static)
		{
			dashWarningLine = dashWarningLine_H;
		}
		dashWarningLine.widthMultiplier = base.CC_Self.radius * 2.5f;
		dashWarningLine.positionCount = 10;
	}

	public override void EveryInitialCallback()
	{
		PlayAnim("Appear1");
		state = MonsterState.BornIdle;
		attackCD.RandomResult();
		Inst = this;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		componentData.CanBeTarget = false;
		componentData.InvincibleRegister();
		SetComponentData(componentData);
		Boss13Stage3FollowMissile.followMissiles.Clear();
		subActive = false;
		dialogueActive = false;
		roomCenter = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height;
	}

	private Vector3 GetInvisiblePoint(float percent)
	{
		Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * UnityEngine.Random.Range(3, 7));
		return GetInvisiblePoint(navMeshPointIngoreZ, percent);
	}

	private Vector3 GetInvisiblePoint(Vector3 origin, float percent)
	{
		float num = roomWidth * percent;
		float num2 = roomHeight * percent;
		origin.x = Mathf.Clamp(origin.x, roomCenter.x - num, roomCenter.x + num);
		origin.y = Mathf.Clamp(origin.y, roomCenter.y - num2, roomCenter.y + num2);
		return origin;
	}

	public void OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
		if (state == MonsterState.Dash)
		{
			switch (UnitDotsSyncSystem.GetLayer(collision.GetOtherEntity(myPpt.myEntity)))
			{
			case 256u:
				_ = (Vector3)(-collision.GetNormalFrom(myPpt.myEntity));
				dashWall = true;
				PlayAnim("Idle");
				break;
			case 131072u:
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13.Inst.myPpt.myEntity);
				info.damage = 9999f;
				info.ignoreFloatText = true;
				UnitDotsSyncSystem.AddTakeDamageRequest(collision.GetOtherEntity(myPpt.myEntity), info);
				break;
			}
			}
		}
	}

	public void OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
	}

	public void OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}

	private void ChooseSkill()
	{
		if (!isAttacking)
		{
			attackCDTimer += Time.deltaTime;
		}
		if (!(attackCDTimer > attackCD.result))
		{
			return;
		}
		attackCDTimer = 0f;
		attackCD.RandomResult();
		bool flag = false;
		while (!flag)
		{
			int weightRandom = GeneralTool.GetWeightRandom(mineChance, dashChance, missileChance, areaMissileChance, falculaChance);
			if (isStrafing && (weightRandom == 1 || weightRandom == 4))
			{
				continue;
			}
			if (lastAttackType != weightRandom)
			{
				switch (weightRandom)
				{
				case 0:
					state = MonsterState.MineCharge;
					break;
				case 1:
					dashCounter = 0;
					state = MonsterState.DashCharge;
					dashMaxTimes.RandomResult();
					break;
				case 2:
					state = MonsterState.FollowMissileAim;
					break;
				case 3:
					state = MonsterState.SubStrafeReady;
					break;
				case 4:
					state = MonsterState.FalculaCharge;
					break;
				}
				flag = true;
			}
			if (flag)
			{
				lastAttackType = weightRandom;
			}
		}
	}

	private void SetMoveDirAndFlip(Vector3 targetPoint)
	{
		towardDir = Tool2D.RotateTowardsAroundZAxis(towardDir, Tool2D.IgnoreZV2ToV1Normal(targetPoint, base.transform.position), 540f * Time.deltaTime);
		if (Tool2D.IgnoreZAngleWithSign(Vector3.up, towardDir) < 0f)
		{
			motion.localScale = new Vector3(-1f, 1f, 1f);
			rotationParent.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			motion.localScale = new Vector3(1f, 1f, 1f);
			rotationParent.localScale = new Vector3(-1f, -1f, 1f);
		}
		rotationParent.right = towardDir;
	}

	private void SetMoveDirAndFlipImmediately(Vector3 targetPoint)
	{
		towardDir = Tool2D.IgnoreZV2ToV1Normal(targetPoint, base.transform.position);
		if (targetPoint.x > base.transform.position.x)
		{
			motion.localScale = new Vector3(-1f, 1f, 1f);
			rotationParent.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			motion.localScale = new Vector3(1f, 1f, 1f);
			rotationParent.localScale = new Vector3(-1f, -1f, 1f);
		}
		rotationParent.right = towardDir;
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		SyncDotsPosition();
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
				PlayAnim("Appear1");
				GetNearestTargetPlayerFirst();
				SEMgr.Inst.boss13DashDown.PlaySE();
			}
			SetMove(Vector3.zero);
			if (!(stateExistTime > fallTime) || dialogueActive)
			{
				break;
			}
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13_DustDirtySoft", base.transform.position, 1.5f);
			dialogueActive = true;
			PlayAnim("Idle");
			if (controller.skipDaveDialogue)
			{
				state = MonsterState.ShowIdle;
			}
			else if (DataMgr.selectedWorldData.IsDave)
			{
				GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(308, (Action)delegate
				{
					state = MonsterState.ShowIdle;
				});
			}
			else
			{
				state = MonsterState.ShowIdle;
			}
			break;
		case MonsterState.ShowIdle:
			if (changedState)
			{
				PlayAnim("Appear2");
				GetNearestTargetPlayerFirst();
			}
			SetMove(Vector3.zero);
			if (stateExistTime > 1f)
			{
				GameUISingletonMono<UIBossHP>.ShowInit(myPpt.myEntity);
				GameUISingletonMono<UIBossShow>.ShowInit(myPpt.myEntity);
				state = MonsterState.Idle;
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanTouch = true;
				componentData.CanBeTarget = true;
				componentData.InvincibleUnregister();
				SetComponentData(componentData);
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				PlayAnim("Idle");
				idleTime.RandomResult();
			}
			SetMove(Vector3.zero);
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.Move;
			}
			ChooseSkill();
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				PlayAnim("Move");
				randomMoveTarget = base.transform.position + Tool2D.GetDir() * randomMoveRadius.RandomResult();
				randomMoveTime.RandomResult();
				if (base.HaveTarget)
				{
					randomMoveTarget = Tool2D.GetNavMeshPoint(base.TargetPoint, keepDistance, -ToTargetDir(), keepDistanceAngle);
					GetNavInfo(randomMoveTarget);
				}
				else
				{
					randomMoveTarget = GetInvisiblePoint(base.transform.position + Tool2D.GetDir() * randomMoveRadius.RandomResult(), 0.4f);
					GetNavInfo(randomMoveTarget);
				}
				SetMoveDirAndFlipImmediately(navInfo.ToGoPoint);
			}
			if (stateExistTime > randomMoveTime.result)
			{
				if (base.HaveTarget && ToTargetDistanceSqr() > keepDistance.value2 * keepDistance.value2)
				{
					state = MonsterState.Move;
					break;
				}
				state = MonsterState.Idle;
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				if (base.HaveTarget)
				{
					if (ToTargetDistanceSqr() > keepDistance.value2 * keepDistance.value2)
					{
						state = MonsterState.Move;
						break;
					}
					randomMoveTarget = Tool2D.GetNavMeshPoint(base.TargetPoint, keepDistance, -ToTargetDir(), keepDistanceAngle);
					GetNavInfo(randomMoveTarget);
				}
				else
				{
					randomMoveTarget = base.transform.position + Tool2D.GetDir() * randomMoveRadius.RandomResult();
					GetNavInfo(randomMoveTarget);
				}
			}
			SetMoveDirAndFlip(navInfo.ToGoPoint);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			ChooseSkill();
			break;
		case MonsterState.Move:
			if (changedState)
			{
				PlayAnim("Move");
				if (base.HaveTarget)
				{
					GetNavInfo(base.TargetPoint);
					SetMoveDirAndFlipImmediately(navInfo.ToGoPoint);
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
			ChooseSkill();
			if (ToTargetDistance() > keepDistance.value2 * keepDistance.value2)
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				SetMoveDirAndFlip(navInfo.ToGoPoint);
			}
			else
			{
				state = MonsterState.RandomMove;
			}
			break;
		case MonsterState.DashCharge:
		{
			ref Vector3 reference3 = ref varMgr.RegV3(0);
			if (changedState)
			{
				dashWall = false;
				if (dashCounter == 0)
				{
					if (base.HaveTarget)
					{
						reference3 = -ToTargetDir();
					}
					else
					{
						reference3 = Tool2D.GetDir();
					}
				}
				else
				{
					reference3 = -dashDir;
				}
				dashCounter++;
				PlayAnim("DashCharge");
				GetNearestTargetPlayerFirst();
				dashDir = ToPointDir(GetInvisiblePoint(0.4f));
				dashWarningLine.gameObject.SetActive(value: true);
			}
			if (PlayerMgr.Inst.PlayerPoint.x < base.transform.position.x)
			{
				motion.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				motion.localScale = new Vector3(-1f, 1f, 1f);
			}
			SetMove(Vector3.zero);
			SetFlip(dashDir.x);
			if (base.HaveTarget)
			{
				if (dashCounter == 1 || Tool2D.IgnoreZAngle(ToTargetDir(), reference3) > dashAgainMinAngle)
				{
					dashDir = ToTargetDir();
				}
				else
				{
					float num = Mathf.Sign(Tool2D.IgnoreZAngleWithSign(reference3, ToTargetDir()));
					dashDir = Tool2D.GetDir(reference3, num * dashAgainMinAngle);
				}
			}
			else if (dashCounter != 1 && !(Tool2D.IgnoreZAngle(dashDir, reference3) > dashAgainMinAngle))
			{
				float num2 = Mathf.Sign(Tool2D.IgnoreZAngleWithSign(reference3, dashDir));
				dashDir = Tool2D.GetDir(reference3, num2 * dashAgainMinAngle);
			}
			for (int j = 0; j < dashWarningLine.positionCount; j++)
			{
				Vector3 rootPoint = Vector3.Lerp(base.transform.position, base.transform.position + dashDir * dashSpeed * dashTime, (float)j / (float)(dashWarningLine.positionCount - 1));
				rootPoint = Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect);
				dashWarningLine.SetPosition(j, rootPoint);
			}
			if (stateExistTime >= dashChargeTime)
			{
				dashWarningLine.gameObject.SetActive(value: false);
				dashEffect.Play();
				dashEffect.transform.right = dashDir;
				state = MonsterState.Dash;
			}
			break;
		}
		case MonsterState.Dash:
		{
			ref float reference5 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				SEMgr.Inst.boss13Dash.PlaySE();
				PlayAnim("Dash");
				actionAnima.speed = 0f;
				GetNearestTargetPlayerFirst();
				dashDamageCheck.damageCheck = true;
				dashDamageCheck.hitEntities.Clear();
				UnitProperty_Dots componentData4 = GetComponentData<UnitProperty_Dots>();
				componentData4.CanTouch = false;
				SetComponentData(componentData4);
			}
			if (stateExistTime > dashTime)
			{
				if (dashCounter < dashMaxTimes.result)
				{
					actionAnima.speed = 1f;
					state = MonsterState.DashCharge;
				}
				else
				{
					actionAnima.speed = 1f;
					state = MonsterState.DashFinish;
				}
				dashDamageCheck.damageCheck = false;
				UnitProperty_Dots componentData5 = GetComponentData<UnitProperty_Dots>();
				componentData5.CanTouch = true;
				SetComponentData(componentData5);
				break;
			}
			if (!dashWall)
			{
				if (base.HaveTarget)
				{
					dashDir = Vector3.MoveTowards(dashDir, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position), dashRotateSpeed * Time.deltaTime);
				}
				SetMove(dashDir.normalized * dashSpeed);
				motion.localScale = new Vector3((0f - dashDir.x > 0f) ? 1 : (-1), 1f, 1f);
				reference5 += Time.deltaTime;
				if (reference5 > dashBulletSpawnInterval)
				{
					reference5 = 0f;
					float num3 = GeneralTool.HalfChanceNPOne();
					Vector3 dir2 = Tool2D.GetDir(dashDir, 90f * num3);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13DashBullet", base.transform.position + UnityEngine.Random.Range(0f, 0.1f) * dir2).GetComponent<Boss13DashBullet>().moveDir = Tool2D.GetDir(dir2, (float)UnityEngine.Random.Range(25, 45) * num3);
				}
				dashDamageCheck.dashDir = dashDir;
			}
			else
			{
				SetMove(Vector3.zero);
			}
			SetFlip(dashDir.x);
			break;
		}
		case MonsterState.DashFinish:
			if (changedState)
			{
				actionAnima.speed = 1f;
			}
			SetMove(Vector3.zero);
			if (stateExistTime > dashFinishTime)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.FollowMissileAim:
			if (changedState)
			{
				SEMgr.Inst.boss13Stage2Aim.PlaySE();
				PlayAnim("MissileReady");
				if (PlayerMgr.Inst.PlayerPoint.x < base.transform.position.x)
				{
					motion.localScale = new Vector3(1f, 1f, 1f);
				}
				else
				{
					motion.localScale = new Vector3(-1f, 1f, 1f);
				}
			}
			SetMove(Vector3.zero);
			if (stateExistTime > aimTime)
			{
				state = MonsterState.FollowMissile;
			}
			break;
		case MonsterState.FollowMissile:
		{
			ref float reference = ref varMgr.RegFloat(0);
			ref int reference2 = ref varMgr.RegInt(0);
			if (changedState)
			{
				reference2 = 0;
				reference = 9f;
			}
			if (PlayerMgr.Inst.PlayerPoint.x < base.transform.position.x)
			{
				motion.localScale = new Vector3(1f, 1f, 1f);
				fireEffect.transform.localEulerAngles = new Vector3(0f, 0f, 30f);
			}
			else
			{
				motion.localScale = new Vector3(-1f, 1f, 1f);
				fireEffect.transform.localEulerAngles = new Vector3(0f, 0f, -30f);
			}
			reference += Time.deltaTime;
			if (reference > 0.8f)
			{
				if (reference2 < missileAmount)
				{
					PlayAnim("Missile");
					reference = 0f;
					reference2++;
					SEMgr.Inst.boss13Stage1Shoot.PlaySE();
					Boss13Stage3FollowMissile component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/501341", new Vector3(missilePivot.position.x, base.transform.position.y, 0f - (missilePivot.position.y - base.transform.position.y - missileOriginHeight))).GetComponent<Boss13Stage3FollowMissile>();
					Boss13Stage3FollowMissile.followMissiles.Add(component);
					fireEffect.Play();
				}
				else
				{
					state = MonsterState.RandomMove;
				}
			}
			break;
		}
		case MonsterState.SubStrafeReady:
			if (changedState)
			{
				PlayAnim("Call");
				SEMgr.Inst.boss13CallSub.PlaySE();
			}
			if (stateExistTime > subStrafeReadyTime)
			{
				state = MonsterState.CrossStrafe;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.CrossStrafe:
			if (changedState)
			{
				isStrafing = true;
				Vector3 dir = Tool2D.GetDir();
				Vector3 vector = PlayerMgr.Inst.PlayerPoint - dir * generatePosOffset;
				Vector3 vector2 = PlayerMgr.Inst.PlayerCtrller.CurrentMotion + vector;
				Vector3 vector3 = LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(vector2 + dir * 7.5f) - dir * 7.5f;
				Boss13FakeSub component3 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13FakeSub", vector3).GetComponent<Boss13FakeSub>();
				component3.lookDir = dir;
				component3.strafeGeneratePos = vector3;
				component3.strafeMoveDir = dir;
				component3.strafeAmount = 2;
				component3.SetMode(0);
				fakeSub = component3;
				state = MonsterState.Move;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.MineCharge:
			if (changedState)
			{
				PlayAnim("Mine");
				isAttacking = true;
				if (boss13_Mines.Count + mineCount > mineMaxAmount)
				{
					for (int k = 0; k < mineCount; k++)
					{
						boss13_Mines[0].StartDisappear();
					}
				}
			}
			if (PlayerMgr.Inst.PlayerPoint.x < base.transform.position.x)
			{
				motion.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				motion.localScale = new Vector3(-1f, 1f, 1f);
			}
			SetMove(Vector3.zero);
			if (stateExistTime > mineChargeTime)
			{
				state = MonsterState.Mine;
			}
			break;
		case MonsterState.Mine:
			if (changedState)
			{
				SEMgr.Inst.boss13MineShoot.PlaySE();
				for (int i = 0; i < mineCount; i++)
				{
					Vector3 zero = Vector3.zero;
					zero = ((LevelMgr.Inst.CurrentRoomCfg.themeType != RoomThemeType.Theme6_Chapter3 && LevelMgr.Inst.CurrentRoomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1) ? (LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(UnityEngine.Random.Range((float)(-LevelMgr.Inst.CurrentRoomCtrller.roomCfg.width) / 2f, (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.width / 2f), UnityEngine.Random.Range((float)(-LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height) / 2f, (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height / 2f), 0f)) : (LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(UnityEngine.Random.Range((float)(-LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width) / 2f, (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width / 2f), UnityEngine.Random.Range((float)(-LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height) / 2f, (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height / 2f), 0f)));
					zero = LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(zero);
					Boss13_Mine component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13Mine", base.transform.position - new Vector3(0f, 0f, 0.2f)).GetComponent<Boss13_Mine>();
					component2.StartParabola(zero, Mathf.Clamp(Tool2D.IgnoreZDistance(component2.transform.position, zero) * 0.8f, 6f, 9f));
					boss13_Mines.Add(component2);
				}
			}
			SetMove(Vector3.zero);
			if (stateExistTime > 1f)
			{
				state = MonsterState.Idle;
				isAttacking = false;
				SEMgr.Inst.boss13MineFloat.PlaySE();
			}
			break;
		case MonsterState.FalculaCharge:
			if (changedState)
			{
				SEMgr.Inst.boss13FalculaCharge.PlaySE();
				UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
				componentData3.CanTouch = false;
				SetComponentData(componentData3);
				PlayAnim("FalculaChargeFire");
			}
			if (stateExistTime > falculaChargeTime)
			{
				Vector3 vector4 = new Vector3(Inst.falculaPivot.position.x, Inst.transform.position.y, 0f - Inst.falculaPivot.position.y + Inst.transform.position.y);
				falculaHead = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13HarpoonsTip").GetComponent<Boss13_FalculaHead>();
				falculaHead.falculaMoveDir = Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, vector4);
				falculaHead.state = Boss13_FalculaHead.FalculaState.Out;
				falculaHead.transform.position = vector4;
				state = MonsterState.FalculaStay;
			}
			SetMove(Vector3.zero);
			SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPoint).x);
			break;
		case MonsterState.FalculaStay:
			SetMove(Vector3.zero);
			break;
		case MonsterState.FalculaFail:
		{
			ref bool reference4 = ref varMgr.RegBool(0);
			if (changedState)
			{
				PlayAnim("FalculaFail");
				reference4 = false;
			}
			if (stateExistTime > falculaAttackTime && !reference4)
			{
				reference4 = true;
				PlayerMgr.Inst.PlayerCtrller.StartMotion();
				isAttacking = false;
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13.Inst.myPpt.myEntity);
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position) * hitKnockBack;
				info.damage = hitDamage;
				info.teammateTakeDamageRatio = 4f;
				UnitDotsSyncSystem.AddTakeDamageRequest(PlayerMgr.Inst.PlayerEtt, info);
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.CanTouch = true;
				SetComponentData(componentData2);
			}
			if (stateExistTime > falculaFailTime)
			{
				state = MonsterState.Move;
			}
			break;
		}
		case MonsterState.Dead:
			SetMove(Vector3.zero, isFlip: false);
			if (changedState)
			{
				PlayAnim("Dead");
				if (falculaHead != null)
				{
					falculaHead.state = Boss13_FalculaHead.FalculaState.Hide;
					falculaHead.gameObject.SetActive(value: false);
					CamController.Inst.FocusRecover(0.5f);
					if (falculaHead.bindPlayer)
					{
						PlayerMgr.Inst.PlayerCtrller.StartMotion();
					}
				}
			}
			if (stateExistTime > deadTime && !subActive)
			{
				subActive = true;
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13FakeSub", base.transform.position).GetComponent<Boss13FakeSub>().SetMode(2);
			}
			break;
		}
	}

	public void FalculaEnd()
	{
		isAttacking = false;
		state = MonsterState.Move;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = true;
		SetComponentData(componentData);
	}

	protected override void SetFlip(float motionX)
	{
		if (state == MonsterState.RandomMove || state == MonsterState.FalculaStay || state == MonsterState.Move)
		{
			return;
		}
		if (base.HaveTarget)
		{
			if (base.TargetPoint.x < base.transform.position.x)
			{
				motion.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				motion.localScale = new Vector3(-1f, 1f, 1f);
			}
		}
		else if (!(Mathf.Abs(motionX) < 0.01f))
		{
			if (motionX < 0f)
			{
				motion.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				motion.localScale = new Vector3(-1f, 1f, 1f);
			}
		}
	}

	protected override void BossDeadStay()
	{
		dashWarningLine.gameObject.SetActive(value: false);
		if ((bool)fakeSub && fakeSub.gameObject.activeInHierarchy)
		{
			fakeSub.state = Boss13FakeSub.SubState.FadeOut;
			fakeSub.strafeAmount = 0;
		}
		state = MonsterState.Dead;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
	}

	public void PlayAnim(string anim)
	{
		if (anim == "Move")
		{
			moveAnimaObj.SetActive(value: true);
			actionAnimaObj.SetActive(value: false);
		}
		else
		{
			moveAnimaObj.SetActive(value: false);
			actionAnimaObj.SetActive(value: true);
			actionAnima.Play(anim, 0, 0f);
		}
	}
}
