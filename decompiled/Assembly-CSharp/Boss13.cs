using System;
using Unity.Entities;
using Unity.Physics.Stateful;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Boss13 : UnitBase, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Move,
		Attack1Load,
		Attack1Aim,
		Attack1Fire,
		Attack2Aim,
		Attack2Fire,
		DeviceAim,
		DeviceFire,
		DeviceRepeatCD,
		DashPrepare,
		Dash,
		DashFinish,
		SubStrafeReady,
		SubStrafe,
		Dead,
		DeadAnimation
	}

	[Header("通用属性")]
	public VariableFloat attackCD;

	public float attackCDTimer;

	public Transform motion;

	public static Boss13 Inst;

	public TestController controller;

	[Header("技能选择")]
	public float grenadeChance;

	public float highGrenadeChance;

	public float dashChance;

	public float dashExtraChance;

	public float fallTorpedoChance;

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

	[Header("移动")]
	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	public VariableFloat keepDistance;

	public float keepDistanceAngle;

	private Vector3 randomMoveTarget;

	[Header("榴弹射击")]
	public Transform gunPivot;

	public Transform highPivot;

	public float grenadeLoadTime;

	public float upGrenadeLoadTime;

	public float upSpeed;

	public float upSpeedLow;

	public float redictMoveTime;

	public float fixDistance;

	public float grenadeAimTime;

	public float grenadeAttackTime;

	public float grenadeScatterRange;

	[Header("反弹榴弹")]
	public float reboundGrenadeCount;

	public float reboundGrenadeAngle;

	[Header("冲撞")]
	public float dashDamage = 10f;

	public float dashSpeed;

	public float dashChargeTime;

	public float dashTime;

	public float dashFinishTime;

	public VariableInt dashMaxTimes;

	public float dashAgainMinAngle;

	private int dashCounter;

	public LineRenderer dashWarningLine;

	public LineRenderer dashWarningLine_H;

	private Vector3 dashDir;

	private bool dashWall;

	public ParticleSystem dashEffect;

	public float dashBulletSpawnInterval;

	public float dashRotateSpeed;

	public Boss13DashDamageCheck dashDamageCheck;

	private Vector3 roomCenter;

	private float roomWidth;

	private float roomHeight;

	private Vector3 aimPoint;

	[Header("潜艇扫射")]
	public bool isStrafing;

	public float generatePosOffset;

	public Boss13FakeSub fakeSub;

	public float subStrafeReadyTime;

	[Header("取消闪红")]
	public SpriteRenderer[] cancelRedSprites;

	[Header("子弹参数")]
	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public int circleBulletCount;

	[Header("拖尾子弹")]
	public VariableInt headTrailBulletAmount;

	public int headTrailBulletCounter;

	public ParticleSystem fireEffect;

	public float fireKnockBack;

	[Header("影子")]
	public float shadowShowOffset;

	public float shadowHeightScale;

	public Vector2Int mrScale;

	public static RTCamController camController;

	public static Vector2Int shadowMRScale;

	public static RenderTexture shadowRT;

	[Header("死亡")]
	public float deadTime;

	private bool dialogueActive;

	private bool createdSub;

	public float deadAnimationTime;

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private SpellSpawnParams ssp;

	private int notDashSkillCount;

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
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
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
		RoomController currentRoomCtrller = LevelMgr.Inst.CurrentRoomCtrller;
		Vector2Int vector2Int = new Vector2Int(mrScale.x * currentRoomCtrller.pixelCountPerMeter, mrScale.y * currentRoomCtrller.pixelCountPerMeter);
		shadowRT = new RenderTexture(vector2Int.x, vector2Int.y, GraphicsFormat.R8G8B8A8_UNorm, GraphicsFormat.D16_UNorm);
		camController = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/RTCam"), base.transform.parent).GetComponent<RTCamController>();
		camController.cam.transform.parent.position = Tool2D.IgnoreZPoint(base.transform.position, -2080f);
		camController.cam.targetTexture = shadowRT;
		camController.cam.farClipPlane = 1f;
		camController.cam.orthographicSize = (float)mrScale.y / 2f;
		camController.MaxFps = 60;
		shadowMRScale = mrScale;
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		attackCD.RandomResult();
		Inst = this;
		MusicMgr.Inst.UpdateThemeMusic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.InvincibleRegister();
		componentData.CanBeTarget = false;
		componentData.CanTouch = false;
		SetComponentData(componentData);
		dialogueActive = false;
		createdSub = false;
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
				dashWall = true;
				PlayAnim("Idle");
				break;
			case 131072u:
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Inst.myPpt.myEntity);
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
		attackCDTimer += Time.deltaTime;
		if (!(attackCDTimer > attackCD.result))
		{
			return;
		}
		attackCDTimer = 0f;
		attackCD.RandomResult();
		bool flag = false;
		while (!flag)
		{
			int num = GeneralTool.GetWeightRandom(grenadeChance, dashChance + ((state == MonsterState.Move) ? dashExtraChance : 0f), highGrenadeChance, fallTorpedoChance);
			if (notDashSkillCount >= 2)
			{
				num = 1;
			}
			if (isStrafing && num == 1)
			{
				continue;
			}
			if (lastAttackType != num || (state == MonsterState.Move && lastAttackType == 1))
			{
				switch (num)
				{
				case 0:
					state = MonsterState.Attack1Load;
					break;
				case 1:
					state = MonsterState.DashPrepare;
					dashCounter = 0;
					notDashSkillCount = 0;
					dashMaxTimes.RandomResult();
					break;
				case 2:
					state = MonsterState.Attack2Aim;
					break;
				case 3:
					state = MonsterState.SubStrafeReady;
					break;
				}
				flag = true;
			}
			if (flag)
			{
				lastAttackType = num;
				if (num != 1 && num != 3)
				{
					notDashSkillCount++;
				}
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
				PlayAnim("Idle");
			}
			SetMove(Vector3.zero);
			if (controller.skipDaveDialogue)
			{
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				UnitProperty_Dots componentData5 = GetComponentData<UnitProperty_Dots>();
				componentData5.CanBeTarget = true;
				componentData5.CanTouch = true;
				componentData5.InvincibleUnregister();
				SetComponentData(componentData5);
				state = MonsterState.Move;
			}
			else
			{
				if (!(Tool2D.IgnoreZDistanceSqr(base.transform.position, PlayerMgr.Inst.PlayerPoint) < 25f) || dialogueActive)
				{
					break;
				}
				dialogueActive = true;
				if (DataMgr.selectedWorldData.daveFirstMeetBoss13)
				{
					GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(306, (Action)delegate
					{
						DataMgr.selectedWorldData.daveFirstMeetBoss13 = false;
						base.CC_Self.enabled = true;
						SetDotsCCEnable(isOpen: true);
						UnitProperty_Dots componentData7 = GetComponentData<UnitProperty_Dots>();
						componentData7.InvincibleUnregister();
						componentData7.CanBeTarget = true;
						componentData7.CanTouch = true;
						SetComponentData(componentData7);
						state = MonsterState.Move;
						GameUISingletonMono<UIBossHP>.ShowInit(myPpt.myEntity);
						GameUISingletonMono<UIBossShow>.ShowInit(myPpt.myEntity);
						dialogueActive = false;
					});
					Debug.Log("第一次");
				}
				else
				{
					Debug.Log("重复");
					if (DataMgr.selectedWorldData.IsDave)
					{
						GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(307, StartBoss);
					}
					else
					{
						StartBoss();
					}
				}
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
		case MonsterState.Attack1Load:
			if (changedState)
			{
				PlayAnim("UpReady");
				headTrailBulletAmount.RandomResult();
				aimPoint = GetInvisiblePoint(0.4f);
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
			else
			{
				GetNearestTargetPlayerFirst();
			}
			SetMove(Vector3.zero);
			SetFlip(ToPointDir(aimPoint).x);
			if (stateExistTime > grenadeLoadTime)
			{
				state = MonsterState.Attack1Aim;
			}
			break;
		case MonsterState.Attack1Aim:
			if (changedState)
			{
				PlayAnim("UpAim");
				headTrailBulletAmount.RandomResult();
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
				aimPoint = base.TargetPoint;
			}
			else
			{
				GetNearestTargetPlayerFirst();
			}
			if (stateExistTime > upGrenadeLoadTime)
			{
				state = MonsterState.Attack1Fire;
				break;
			}
			SetMove(Vector3.zero, isFlip: false);
			SetFlip(ToPointDir(aimPoint).x);
			break;
		case MonsterState.Attack1Fire:
			if (changedState)
			{
				SEMgr.Inst.boss13Stage1Shoot.PlaySE();
				PlayAnim("UpFire");
				GetNearestTargetPlayerFirst();
				_ = LevelMgr.Inst.CurrentRoomCtrller.roomCfg;
				_ = LevelMgr.Inst.CurrentRoomCtrller.roomCfg;
				_ = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
				if (base.HaveTarget)
				{
					if (targetEntity == PlayerMgr.Inst.PlayerEtt)
					{
						aimPoint = PlayerMgr.Inst.PlayerCtrller.CurrentMotion * redictMoveTime + PlayerMgr.Inst.PlayerPoint;
					}
					else
					{
						aimPoint = base.TargetPoint;
					}
				}
				aimPoint += Tool2D.GetDir() * UnityEngine.Random.value * grenadeScatterRange;
				aimPoint = LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(aimPoint);
				fireEffect.transform.position = highPivot.position;
				fireEffect.transform.localEulerAngles = new Vector3(0f, 0f, 30f * motion.localScale.x);
				fireEffect.Play();
				float num3 = highPivot.position.y - base.transform.position.y;
				Vector3 point = new Vector3(highPivot.position.x, base.transform.position.y, 0f - num3);
				Boss13_Grenade component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13Grenade", point).GetComponent<Boss13_Grenade>();
				component.isTypeOne = false;
				component.StartParabola(aimPoint, num3, upSpeed);
				headTrailBulletCounter++;
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.TakeKnockback(-ToPointDir(aimPoint) * fireKnockBack / myPpt.unitCfg.knockbackRatio);
				SetComponentData(componentData);
			}
			SetMove(Vector3.zero);
			SetFlip(ToPointDir(aimPoint).x);
			if (stateExistTime > grenadeAttackTime)
			{
				if (headTrailBulletCounter > headTrailBulletAmount.result)
				{
					state = MonsterState.RandomMove;
					headTrailBulletCounter = 0;
				}
				else
				{
					state = MonsterState.Attack1Aim;
					aimPoint = GetInvisiblePoint(0.4f);
				}
			}
			break;
		case MonsterState.Attack2Aim:
			if (changedState)
			{
				PlayAnim("Ready");
				aimPoint = GetInvisiblePoint(0.4f);
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
				aimPoint = base.TargetPoint;
			}
			if (stateExistTime > grenadeAimTime + grenadeLoadTime)
			{
				state = MonsterState.Attack2Fire;
				break;
			}
			SetMove(Vector3.zero);
			SetFlip(ToPointDir(aimPoint).x);
			break;
		case MonsterState.Attack2Fire:
			if (changedState)
			{
				SEMgr.Inst.boss13Stage1Shoot.PlaySE();
				PlayAnim("Fire");
				Vector3 vector4 = ToPointDir(aimPoint);
				if (base.HaveTarget)
				{
					vector4 = ((!(targetEntity == PlayerMgr.Inst.PlayerEtt)) ? Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position) : Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint + PlayerMgr.Inst.PlayerCtrller.CurrentMotion * redictMoveTime, base.transform.position));
				}
				fireEffect.transform.position = gunPivot.position;
				fireEffect.transform.localEulerAngles = new Vector3(0f, 0f, 90f * motion.localScale.x);
				fireEffect.Play();
				float num4 = gunPivot.position.y - base.transform.position.y;
				Vector3 vector5 = new Vector3(gunPivot.position.x, base.transform.position.y, 0f - num4);
				for (int j = 0; (float)j < reboundGrenadeCount; j++)
				{
					Boss13_Grenade component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13GrenadeRebound", vector5).GetComponent<Boss13_Grenade>();
					component2.isTypeOne = true;
					component2.StartParabola(vector5 + Tool2D.GetDir(degree: reboundGrenadeAngle * (-0.5f + (float)j / (reboundGrenadeCount - 1f)), oldDir: vector4) * fixDistance, num4, upSpeedLow);
				}
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.TakeKnockback(-vector4 * fireKnockBack / myPpt.unitCfg.knockbackRatio);
				SetComponentData(componentData2);
				SetFlip(vector4.x);
			}
			SetMove(Vector3.zero, isFlip: false);
			if (stateExistTime > 0.56f)
			{
				state = MonsterState.RandomMove;
			}
			break;
		case MonsterState.DashPrepare:
		{
			ref Vector3 reference2 = ref varMgr.RegV3(0);
			if (changedState)
			{
				dashWall = false;
				if (dashCounter == 0)
				{
					if (base.HaveTarget)
					{
						reference2 = -ToTargetDir();
					}
					else
					{
						reference2 = -ToPointDir(GetInvisiblePoint(0.4f));
					}
				}
				else
				{
					reference2 = -dashDir;
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
				if (dashCounter == 1 || Tool2D.IgnoreZAngle(ToTargetDir(), reference2) > dashAgainMinAngle)
				{
					dashDir = ToTargetDir();
				}
				else
				{
					float num = Mathf.Sign(Tool2D.IgnoreZAngleWithSign(reference2, ToTargetDir()));
					dashDir = Tool2D.GetDir(reference2, num * dashAgainMinAngle);
				}
			}
			else if (dashCounter != 1 && !(Tool2D.IgnoreZAngle(dashDir, reference2) > dashAgainMinAngle))
			{
				float num2 = Mathf.Sign(Tool2D.IgnoreZAngleWithSign(reference2, dashDir));
				dashDir = Tool2D.GetDir(reference2, num2 * dashAgainMinAngle);
			}
			for (int i = 0; i < dashWarningLine.positionCount; i++)
			{
				Vector3 rootPoint = Vector3.Lerp(base.transform.position, base.transform.position + dashDir * dashSpeed * dashTime, (float)i / (float)(dashWarningLine.positionCount - 1));
				rootPoint = Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect);
				dashWarningLine.SetPosition(i, rootPoint);
			}
			if (stateExistTime >= dashChargeTime)
			{
				state = MonsterState.Dash;
			}
			break;
		}
		case MonsterState.Dash:
		{
			ref float reference3 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				dashWarningLine.gameObject.SetActive(value: false);
				dashEffect.Play();
				dashEffect.transform.right = dashDir;
				PlayAnim("Dash");
				actionAnima.speed = 0f;
				GetNearestTargetPlayerFirst();
				dashDamageCheck.damageCheck = true;
				dashDamageCheck.hitEntities.Clear();
				UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
				componentData3.CanTouch = false;
				SetComponentData(componentData3);
				SEMgr.Inst.boss13Dash.PlaySE();
			}
			if (stateExistTime > dashTime)
			{
				if (dashCounter < dashMaxTimes.result)
				{
					actionAnima.speed = 1f;
					state = MonsterState.DashPrepare;
				}
				else
				{
					actionAnima.speed = 1f;
					state = MonsterState.DashFinish;
				}
				dashDamageCheck.damageCheck = false;
				UnitProperty_Dots componentData4 = GetComponentData<UnitProperty_Dots>();
				componentData4.CanTouch = true;
				SetComponentData(componentData4);
			}
			else if (!dashWall)
			{
				if (base.HaveTarget)
				{
					dashDir = Vector3.MoveTowards(dashDir, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position), dashRotateSpeed * Time.deltaTime);
				}
				SetMove(dashDir.normalized * dashSpeed);
				motion.localScale = new Vector3((0f - dashDir.x > 0f) ? 1 : (-1), 1f, 1f);
				reference3 += Time.deltaTime;
				if (reference3 > dashBulletSpawnInterval)
				{
					reference3 = 0f;
					float num5 = GeneralTool.HalfChanceNPOne();
					Vector3 dir2 = Tool2D.GetDir(dashDir, 90f * num5);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13DashBullet", base.transform.position + UnityEngine.Random.Range(0f, 0.1f) * dir2).GetComponent<Boss13DashBullet>().moveDir = Tool2D.GetDir(dir2, (float)UnityEngine.Random.Range(25, 45) * num5);
				}
				dashDamageCheck.dashDir = dashDir;
			}
			else
			{
				SetMove(Vector3.zero);
			}
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
		case MonsterState.SubStrafeReady:
			if (changedState)
			{
				PlayAnim("Call");
				SEMgr.Inst.boss13CallSub.PlaySE();
			}
			if (stateExistTime > subStrafeReadyTime)
			{
				state = MonsterState.SubStrafe;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.SubStrafe:
			if (changedState)
			{
				isStrafing = true;
				Vector3 dir = Tool2D.GetDir();
				Vector3 vector = PlayerMgr.Inst.PlayerPoint - dir * generatePosOffset;
				Vector3 vector2 = PlayerMgr.Inst.PlayerCtrller.CurrentMotion + vector;
				Vector3 vector3 = LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(vector2 + dir * 7.5f) - dir * 7.5f;
				fakeSub = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13FakeSub", vector3).GetComponent<Boss13FakeSub>();
				fakeSub.lookDir = dir;
				fakeSub.strafeGeneratePos = vector3;
				fakeSub.strafeMoveDir = dir;
				fakeSub.strafeAmount = 1;
				fakeSub.SetMode(0);
				state = MonsterState.Move;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Dead:
			if (changedState)
			{
				actionAnima.speed = 1f;
				PlayAnim("Dead");
				if ((bool)fakeSub && fakeSub.gameObject.activeInHierarchy)
				{
					fakeSub.state = Boss13FakeSub.SubState.FadeOut;
				}
			}
			SetMove(Vector3.zero);
			if (stateExistTime > deadTime && !createdSub)
			{
				createdSub = true;
				fakeSub = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13FakeSub").GetComponent<Boss13FakeSub>();
				fakeSub.SetMode(1);
			}
			break;
		case MonsterState.DeadAnimation:
		{
			ref bool reference = ref varMgr.RegBool(0);
			if (changedState)
			{
				PlayAnim("ChangeStage");
			}
			if (stateExistTime > 1.6f && !reference)
			{
				reference = true;
				SEMgr.Inst.boss13Dash.PlaySE();
			}
			if (stateExistTime > deadAnimationTime && !dialogueActive)
			{
				dialogueActive = true;
				ObjPoolMgr.Inst.GetGO("Prefabs/Units/501321", base.transform.position);
				DotsAnnouncedDeath();
				fakeSub.state = Boss13FakeSub.SubState.SwitchToStage2FadeOut;
			}
			SetMove(Vector3.zero);
			break;
		}
		case MonsterState.DeviceAim:
		case MonsterState.DeviceFire:
		case MonsterState.DeviceRepeatCD:
			break;
		}
		void StartBoss()
		{
			base.CC_Self.enabled = true;
			SetDotsCCEnable(isOpen: true);
			UnitProperty_Dots componentData6 = GetComponentData<UnitProperty_Dots>();
			componentData6.InvincibleUnregister();
			componentData6.CanBeTarget = true;
			componentData6.CanTouch = true;
			SetComponentData(componentData6);
			state = MonsterState.Move;
			GameUISingletonMono<UIBossHP>.ShowInit(myPpt.myEntity);
			GameUISingletonMono<UIBossShow>.ShowInit(myPpt.myEntity);
			dialogueActive = false;
		}
	}

	protected override void SetFlip(float motionX)
	{
		if (state == MonsterState.RandomMove || state == MonsterState.Move)
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
			actionAnima.Play(anim);
		}
	}
}
