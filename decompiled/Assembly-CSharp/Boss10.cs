using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;
using UnityEngine;

public class Boss10 : UnitBase, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Move,
		JumpCharge,
		Jump,
		JumpEnd,
		Uppercut,
		Summon,
		Heal,
		PostCastDelay,
		PrepareRam,
		Ram,
		RamVertigo,
		Crouch,
		Smash,
		FallTnt,
		ChangeStage,
		Dead,
		DeadWhiteScreen
	}

	public static Boss10 Inst;

	public List<Boss10TNT> tntList = new List<Boss10TNT>();

	public Transform bodyRotateTransform;

	public Transform outLineRotateTransform;

	public Transform shadowRotateTransform;

	public float rotateSpeed;

	public float targetAngle;

	private float nowAngle;

	public float dirOffset;

	public ParticleSystem downAttackEffect;

	public float fallTntHeight;

	public float fallTntAmount;

	public VariableFloat fallTntInterval;

	public bool isDying;

	public bool deathDelay;

	public Transform digDustEffectPivot;

	public Transform tsf_Model;

	public ShockParam smallShock;

	private bool dialogueActive;

	[Header("技能选择")]
	public float jumpChance;

	public float ramChance;

	public float throwChance;

	public float throwRockChance;

	public float summonChance;

	public float healChance;

	public float healHighChance;

	public float healLowChance;

	public float smashChance;

	[Header("转阶段")]
	public bool isStage2;

	public bool isChangingStage;

	public ParticleSystem roarParticleSystem;

	public ShockParam shockParam;

	public float force;

	public float summonReduceFactor;

	public bool isRoar;

	public ShockParam shockParam_Change;

	[Header("攻击")]
	public float attackCDTime;

	public float attackCDTimer;

	public int lastAttackType;

	public ShockParam shockParam_TNT;

	[Header("动画")]
	public Animator outLineAnima;

	public Animator shadowAnima;

	public SkinnedMeshRenderer[] skinnedMeshRenderers;

	public SpriteRenderer sprite;

	private float distanceToTarget;

	[Header("待机")]
	public VariableFloat idleTime;

	public float checkIntervalTime;

	public float checkIntervalTimer;

	[Header("追击")]
	public float chaseDistance;

	[Header("随机移动")]
	public VariableFloat randomMoveTime;

	public Vector3 randomMoveTarget;

	public VariableFloat randomMoveRadius;

	[Header("蓄力上挑")]
	public float uppercutChargeTime;

	public float uppercutChargeTimer;

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public Transform bulletPivot;

	public int uppercutBulletCount;

	public float uppercutBulletAngle;

	public float spellGravity;

	public VariableFloat spellUpspeed;

	public VariableFloat spellVerticalSpeed;

	public bool rotateRight;

	public ParticleSystem throwPrepareParticle;

	public ParticleSystem throwParticle;

	public float throwRockUpSpeed;

	public float throwRockMinDistance;

	public bool throwsRock;

	[Header("召唤")]
	public int tntCount;

	public int tntMaxAmount;

	public int secondStageTntCount;

	public int tntAroundPlayerCount;

	public int secondStageTntAroundPlayerCount;

	public VariableFloat tntAroundPlayerRange;

	public float summonFromBorderSize;

	public Transform summonParticlePivot;

	[Header("蓄力跃砸")]
	public int crushBulletCount;

	public float jumpChargeTime;

	private WarningArea warningCircle;

	public float delayTime;

	public Vector3 jumpPoint;

	public float areaMoveSpeed;

	public float jumpUpForceFactor;

	public float jumpUpforce;

	public float gravity;

	public ShockParam shockParam_Crush;

	public LayerMask attackMask;

	public float damageRadius;

	public ParticleSystem landParticleSystem;

	[Header("冲撞")]
	public float vertigoTime;

	public Vector3 ramDir;

	public Transform stunnedPivot;

	public GameObject stunnedEffect;

	public float ramSpeedFactor;

	public Transform fistEffectPivot;

	public Transform headPivot;

	public ParticleSystem footEffect;

	public ShockParam shockParam_Ram;

	public VariableFloat rockInterval;

	public float backTime;

	public float ramDamage;

	public float fallRockRounds;

	public float fallRockCount;

	public float fallRockChasePlayerCount;

	public float ramMinDistance;

	[Header("蜷缩")]
	public float crouchTime;

	public float cureCDTime;

	public float curePercentage;

	public int crouchNum;

	public GameObject crouchEffect;

	public GameObject crouchParticleObj;

	public ParticleSystem crouchHitEffect;

	public MeshRenderer shieldMesh;

	[Header("下砸")]
	public float smashRecoverTime;

	[Header("伤害传递")]
	public List<Entity> hitList = new List<Entity>();

	public float hitListClearTime;

	private float hitListClearTimer;

	public UnityEngine.BoxCollider[] boxColliders;

	private Boss10Collider boss10Collider;

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("影子")]
	public float shadowShowOffset;

	public float shadowHeightScale;

	public Vector2Int mrScale;

	public MeshRenderer shadow;

	private RTCamController camController;

	[Header("死亡特效")]
	public float deadExplosionDelay;

	public float deadExplosionInterval;

	public float deadExplosionRadius;

	private SpellSpawnParams ssp;

	private List<UnitDotsSyncSystem.DistanceHitResult> results = new List<UnitDotsSyncSystem.DistanceHitResult>();

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
			spellSpeed *= 0.9f;
			ramSpeedFactor *= 0.9f;
			tntCount--;
		}
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90431);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		RoomController currentRoomCtrller = LevelMgr.Inst.CurrentRoomCtrller;
		Vector2Int vector2Int = new Vector2Int(mrScale.x * currentRoomCtrller.pixelCountPerMeter, mrScale.y * currentRoomCtrller.pixelCountPerMeter);
		RenderTexture renderTexture = new RenderTexture(vector2Int.x, vector2Int.y, 0);
		camController = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/RTCam"), base.transform.parent).GetComponent<RTCamController>();
		camController.cam.transform.parent.position = Tool2D.IgnoreZPoint(base.transform.position, -2080f);
		camController.cam.targetTexture = renderTexture;
		camController.cam.farClipPlane = 1f;
		camController.cam.orthographicSize = (float)mrScale.y / 2f;
		camController.MaxFps = 60;
		myPpt.RemoveMRFromArray(shadow);
		shadow.transform.position = Tool2D.IgnoreZPoint(base.transform.position, 1.05f);
		shadow.transform.localScale = new Vector3(mrScale.x, (float)mrScale.y / 1.414f, 1f);
		shadow.material.SetTexture("_MainTex", renderTexture);
		shadow.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.4f));
	}

	public unsafe override void EveryInitialCallback()
	{
		Inst = this;
		state = MonsterState.BornIdle;
		isDying = false;
		deathDelay = false;
		crouchNum = 0;
		lastAttackType = -1;
		outLineAnima.speed = 1f;
		shadowAnima.speed = 1f;
		hitList.Clear();
		isStage2 = false;
		isChangingStage = false;
		Boss10Collider boss10Collider = (this.boss10Collider = ObjPoolMgr.Inst.GetGO("Prefabs/Units/501021", base.transform.position).GetComponent<Boss10Collider>());
		boss10Collider.Init(boxColliders[0]);
		boss10Collider.hitList = hitList;
		boss10Collider.master = this;
		PhysicsCollider componentData = GetComponentData<PhysicsCollider>();
		CollisionFilter collisionFilter = componentData.ColliderPtr->GetCollisionFilter();
		collisionFilter.BelongsTo = 4096u;
		collisionFilter.CollidesWith = 256u;
		componentData.ColliderPtr->SetCollisionFilter(collisionFilter);
		SetComponentData(componentData);
		shadow.transform.position = Tool2D.IgnoreZPoint(base.transform.position, 1.05f);
		camController.cam.transform.parent.position = Tool2D.IgnoreZPoint(base.transform.position, -2080f);
		MusicMgr.Inst.UpdateThemeMusic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
		componentData2.InvincibleRegister();
		componentData2.CanBeTarget = false;
		componentData2.CanTouch = false;
		SetComponentData(componentData2);
		this.boss10Collider.SetCanBeTarget(value: false);
		dialogueActive = false;
		healChance = healHighChance;
	}

	private void LateUpdate()
	{
		SkinnedMeshRenderer[] array = skinnedMeshRenderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].material.SetColor("_BaseColor", sprite.color);
		}
	}

	private void ChooseSkill()
	{
		attackCDTimer += Time.deltaTime;
		if (!(attackCDTimer > attackCDTime))
		{
			return;
		}
		attackCDTimer = 0f;
		bool flag = false;
		if (base.HaveTarget)
		{
			ToTargetDistance();
		}
		UnitDotsSyncSystem.RayCastHitResult result;
		bool flag2 = UnitDotsSyncSystem.Raycast(base.transform.position, Tool2D.GetDir(nowAngle), ramMinDistance, GameConst.Filter_Wall, out result);
		int num = 0;
		while (!flag)
		{
			num++;
			int num2 = ((!isStage2) ? GeneralTool.GetWeightRandom(jumpChance, ramChance, throwChance, summonChance, healChance) : GeneralTool.GetWeightRandom(jumpChance, ramChance, throwChance, summonChance, healChance, smashChance, throwRockChance));
			if (num2 == 4)
			{
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>(myPpt.myEntity);
				if (componentData.unitCfg.currentHP > componentData.unitCfg.maxHP * 0.7f)
				{
					num2 = UnityEngine.Random.Range(0, 4);
				}
			}
			throwsRock = false;
			if (num2 != lastAttackType)
			{
				switch (num2)
				{
				case 0:
					state = MonsterState.JumpCharge;
					flag = true;
					break;
				case 1:
					state = MonsterState.PrepareRam;
					if (!flag2)
					{
						flag = true;
					}
					break;
				case 2:
					state = MonsterState.Uppercut;
					if (!flag2)
					{
						flag = true;
					}
					break;
				case 3:
					state = MonsterState.Summon;
					if (tntList.Count < tntMaxAmount || num > 10)
					{
						flag = true;
					}
					break;
				case 4:
					state = MonsterState.Heal;
					crouchNum++;
					if (crouchNum >= 3)
					{
						healChance = healLowChance;
					}
					flag = true;
					break;
				case 5:
					state = MonsterState.Smash;
					flag = true;
					break;
				case 6:
					state = MonsterState.Uppercut;
					throwsRock = true;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				lastAttackType = num2;
			}
		}
	}

	public override void SetFrozen()
	{
		base.SetFrozen();
		SetAnimSpeed(base.Anima.speed);
	}

	public override void SetUnfrozen()
	{
		base.SetUnfrozen();
		SetAnimSpeed(base.Anima.speed);
	}

	public override void Update()
	{
		shadow.transform.position = Tool2D.IgnoreZPoint(base.transform.position, 1.05f);
		camController.cam.transform.parent.position = Tool2D.IgnoreZPoint(base.transform.position, -2080f);
		if (hitListClearTimer > hitListClearTime)
		{
			hitListClearTimer = 0f;
			hitList.Clear();
		}
		Debug.DrawLine(base.transform.position, base.transform.position + Tool2D.GetDir(nowAngle) * 20f, Color.blue);
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
		Quaternion to = Quaternion.Euler(0f, 0f - targetAngle + dirOffset, 0f);
		nowAngle = 0f - (bodyRotateTransform.localEulerAngles.y - dirOffset);
		MonsterState monsterState = state;
		if (monsterState == MonsterState.RandomMove || monsterState == MonsterState.Move || monsterState == MonsterState.PrepareRam || monsterState == MonsterState.JumpCharge || monsterState == MonsterState.Uppercut)
		{
			bodyRotateTransform.localRotation = Quaternion.RotateTowards(bodyRotateTransform.localRotation, to, rotateSpeed * Time.deltaTime);
			outLineRotateTransform.localRotation = bodyRotateTransform.localRotation;
			shadowRotateTransform.localRotation = bodyRotateTransform.localRotation;
		}
		shadowRotateTransform.position = Tool2D.IgnoreZPoint(Tool2D.IgnoreZPoint(base.transform.position), -2079f + shadowShowOffset * shadowHeightScale);
		boss10Collider.SyncPosition(base.transform.position, new Vector3(0f, 0f, nowAngle));
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				SetTriggers("Appear");
				targetAngle = Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position));
				Quaternion localRotation = Quaternion.Euler(0f, 0f - targetAngle + dirOffset, 0f);
				bodyRotateTransform.localRotation = localRotation;
				outLineRotateTransform.localRotation = localRotation;
				shadowRotateTransform.localRotation = localRotation;
			}
			if (DataMgr.selectedWorldData.daveFirstMeetBoss10)
			{
				if (Tool2D.IgnoreZDistanceSqr(base.transform.position, PlayerMgr.Inst.PlayerPoint) < 64f && !dialogueActive)
				{
					dialogueActive = true;
					GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(305, (Action)delegate
					{
						DataMgr.selectedWorldData.daveFirstMeetBoss10 = false;
						base.CC_Self.enabled = true;
						SetDotsCCEnable(isOpen: true);
						UnitProperty_Dots componentData13 = GetComponentData<UnitProperty_Dots>();
						componentData13.InvincibleUnregister();
						componentData13.CanBeTarget = true;
						componentData13.CanTouch = true;
						SetComponentData(componentData13);
						boss10Collider.SetCanBeTarget(value: true);
						state = MonsterState.Idle;
						GameUISingletonMono<UIBossHP>.ShowInit(myPpt.myEntity);
						GameUISingletonMono<UIBossShow>.ShowInit(myPpt.myEntity);
					});
				}
			}
			else
			{
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				UnitProperty_Dots componentData4 = GetComponentData<UnitProperty_Dots>();
				componentData4.InvincibleUnregister();
				componentData4.CanBeTarget = true;
				componentData4.CanTouch = true;
				SetComponentData(componentData4);
				boss10Collider.SetCanBeTarget(value: true);
				state = MonsterState.Idle;
				GameUISingletonMono<UIBossHP>.ShowInit(myPpt.myEntity);
				GameUISingletonMono<UIBossShow>.ShowInit(myPpt.myEntity);
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				UnitProperty_Dots componentData10 = GetComponentData<UnitProperty_Dots>(myPpt.myEntity);
				idleTime.RandomResult();
				if (componentData10.unitCfg.currentHP < componentData10.unitCfg.maxHP / 2f && !isStage2)
				{
					isStage2 = true;
					PlayAnims("Roar01");
					componentData10.InvincibleRegister();
					SetComponentData(componentData10, myPpt.myEntity);
					boss10Collider.SetCanBeTarget(value: true);
					isChangingStage = true;
					state = MonsterState.ChangeStage;
					break;
				}
			}
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.RandomMove;
				break;
			}
			SetMove(Vector3.zero);
			distanceToTarget = Mathf.MoveTowards(distanceToTarget, 0f, Time.deltaTime * 0.8f);
			SetFloats("DistanceToTarget", distanceToTarget);
			checkIntervalTimer += Time.deltaTime;
			if (checkIntervalTimer >= checkIntervalTime)
			{
				GetNearestTarget();
				checkTargetIntervalTimer = 0f;
				if (base.HaveTarget)
				{
					state = MonsterState.Move;
					break;
				}
			}
			ChooseSkill();
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				randomMoveTime.RandomResult();
				Vector3 dir = Tool2D.GetDir();
				targetAngle = Tool2D.IgnoreZAngleWithSign(Vector3.up, dir);
				randomMoveTarget = base.transform.position + dir * randomMoveRadius.RandomResult();
				GetNavInfo(randomMoveTarget);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>(myPpt.myEntity);
				if (componentData.unitCfg.currentHP < componentData.unitCfg.maxHP / 2f && !isStage2)
				{
					isStage2 = true;
					PlayAnims("Roar01");
					UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
					componentData2.InvincibleRegister();
					SetComponentData(componentData2);
					isChangingStage = true;
					state = MonsterState.ChangeStage;
					break;
				}
			}
			if (stateExistTime > randomMoveTime.result)
			{
				state = MonsterState.Idle;
				break;
			}
			distanceToTarget = Mathf.MoveTowards(distanceToTarget, 1f, Time.deltaTime * 0.8f);
			SetFloats("DistanceToTarget", distanceToTarget);
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				Vector3 dir2 = Tool2D.GetDir();
				targetAngle = Tool2D.IgnoreZAngleWithSign(Vector3.up, dir2);
				randomMoveTarget = base.transform.position + dir2 * randomMoveRadius.RandomResult();
				GetNavInfo(randomMoveTarget);
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			checkIntervalTimer += Time.deltaTime;
			if (checkIntervalTimer >= checkIntervalTime)
			{
				GetNearestTarget();
				checkTargetIntervalTimer = 0f;
				if (base.HaveTarget)
				{
					state = MonsterState.Move;
					break;
				}
			}
			ChooseSkill();
			break;
		case MonsterState.Move:
		{
			if (changedState)
			{
				UnitProperty_Dots componentData5 = GetComponentData<UnitProperty_Dots>(myPpt.myEntity);
				if (componentData5.unitCfg.currentHP < componentData5.unitCfg.maxHP / 2f && !isStage2)
				{
					isStage2 = true;
					PlayAnims("Roar01");
					componentData5 = GetComponentData<UnitProperty_Dots>();
					componentData5.InvincibleRegister();
					SetComponentData(componentData5);
					isChangingStage = true;
					state = MonsterState.ChangeStage;
					break;
				}
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
				break;
			}
			GetNavInfo(base.TargetPoint);
			targetAngle = Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position));
			float num = Tool2D.IgnoreZAngle(Tool2D.GetDir(targetAngle), Tool2D.GetDir(nowAngle));
			if (Tool2D.IgnoreZDistanceSqr(base.transform.position, base.TargetPoint) > chaseDistance * chaseDistance || num > 0f)
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				distanceToTarget = Mathf.MoveTowards(distanceToTarget, 1f, Time.deltaTime * 0.8f);
				SetFloats("DistanceToTarget", distanceToTarget);
			}
			else
			{
				SetMove(Vector3.zero);
				distanceToTarget = Mathf.MoveTowards(distanceToTarget, 0f, Time.deltaTime * 1.2f);
				SetFloats("DistanceToTarget", distanceToTarget);
			}
			ChooseSkill();
			break;
		}
		case MonsterState.JumpCharge:
			if (changedState)
			{
				if (warningCircle == null || !warningCircle.gameObject.activeInHierarchy)
				{
					SEMgr.Inst.boss10_CrushCharge.PlaySE();
					Vector3 playerPoint = PlayerMgr.Inst.PlayerPoint;
					playerPoint.x = Mathf.Clamp(PlayerMgr.Inst.PlayerPoint.x, LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x - (float)LevelMgr.Inst.CurrentRoomCfg.width / 2f + 4f, LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x + (float)LevelMgr.Inst.CurrentRoomCfg.width / 2f - 4f);
					playerPoint.y = Mathf.Clamp(PlayerMgr.Inst.PlayerPoint.y, LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y - (float)LevelMgr.Inst.CurrentRoomCfg.height / 2f + 4f, LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y + (float)LevelMgr.Inst.CurrentRoomCfg.height / 2f - 4f);
					warningCircle = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle" + (GameMgr.IsHarmony_Static ? " Purple" : ""), playerPoint).GetComponent<WarningArea>();
					warningCircle.Initialize(4f, jumpChargeTime + delayTime, zoomDirect: false);
					SetTriggers("HitTheWall");
				}
				footEffect.Play();
			}
			if (warningCircle != null)
			{
				Vector3 playerPoint2 = PlayerMgr.Inst.PlayerPoint;
				playerPoint2.x = Mathf.Clamp(PlayerMgr.Inst.PlayerPoint.x, LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x - (float)LevelMgr.Inst.CurrentRoomCfg.width / 2f + 4f, LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x + (float)LevelMgr.Inst.CurrentRoomCfg.width / 2f - 4f);
				playerPoint2.y = Mathf.Clamp(PlayerMgr.Inst.PlayerPoint.y, LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y - (float)LevelMgr.Inst.CurrentRoomCfg.height / 2f + 4f, LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y + (float)LevelMgr.Inst.CurrentRoomCfg.height / 2f - 4f);
				warningCircle.transform.position = Vector3.MoveTowards(warningCircle.transform.position, playerPoint2, areaMoveSpeed * Time.deltaTime);
				targetAngle = Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZV2ToV1Normal(warningCircle.transform.position, base.transform.position));
				warningCircle.tsf_Fill.localScale = Vector3.one * Mathf.Lerp(0f, 8f, stateExistTime / jumpChargeTime);
			}
			if (stateExistTime > jumpChargeTime - 0.1f)
			{
				SetTriggers("DurationEnd");
				jumpPoint = warningCircle.transform.position;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Jump:
			if (changedState)
			{
				UnitProperty_Dots componentData6 = GetComponentData<UnitProperty_Dots>();
				componentData6.CanTouch = false;
				SetComponentData(componentData6);
				Tool2D.IgnoreZDistance(base.transform.position, jumpPoint);
				NormalJump(jumpUpforce, gravity);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10_BigKnock", digDustEffectPivot.position, 7f);
				SEMgr.Inst.boss10_TNTUp.PlaySE();
				footEffect.Stop();
				ObjPoolMgr.Inst.RecycleGO(warningCircle.gameObject);
			}
			if (base.transform.position.z > 0f && base.isFalling)
			{
				SEMgr.Inst.boss10_CrushOnGround.PlaySE();
				JumpStop_Dots();
				UnitProperty_Dots componentData7 = GetComponentData<UnitProperty_Dots>(myPpt.myEntity);
				componentData7.CanTouch = true;
				SetComponentData(componentData7);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10_BigKnock", digDustEffectPivot.position, 7f);
				SetAnimSpeed(1f);
				DropDamage();
				state = MonsterState.JumpEnd;
			}
			break;
		case MonsterState.JumpEnd:
			if (changedState && deathDelay)
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(PlayerMgr.Inst.PlayerEtt);
				info.damage = 99f;
				info.ignoreFloatText = true;
				UnitDotsSyncSystem.AddTakeDamageRequest(myPpt.myEntity, info);
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Uppercut:
			if (changedState)
			{
				SetTriggers("RockSpread");
				SetTriggers("DurationEnd");
			}
			throwParticle.transform.position = bulletPivot.position;
			if (base.HaveTarget)
			{
				targetAngle = Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position));
			}
			else
			{
				targetAngle = Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position));
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.PostCastDelay:
			SetMove(Vector3.zero);
			break;
		case MonsterState.PrepareRam:
			if (changedState)
			{
				SetTriggers("Angry");
			}
			SetMove(Vector3.zero);
			if (base.HaveTarget)
			{
				targetAngle = Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position));
			}
			break;
		case MonsterState.Ram:
			if (changedState)
			{
				GetNearestTarget();
				if (base.HaveTarget)
				{
					ramDir = Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, base.transform.position);
				}
				else
				{
					targetAngle = Tool2D.IgnoreZAngleWithSign(to: ramDir = Tool2D.GetDir(), from: Vector3.up);
				}
				boss10Collider.boss10_AttackZone.attackedEtt.Clear();
				boss10Collider.ramCheckObj.SetActive(value: true);
				SetTriggers("RunToFront");
				UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
				componentData3.CanTouch = false;
				SetComponentData(componentData3);
				footEffect.Play();
			}
			distanceToTarget = Mathf.MoveTowards(distanceToTarget, 1f, Time.deltaTime);
			SetFloats("DistanceToTarget", distanceToTarget);
			SetMove(ramDir * base.MoveSpeed * ramSpeedFactor);
			break;
		case MonsterState.RamVertigo:
			if (changedState)
			{
				StartCoroutine(RockFall());
				stunnedEffect.SetActive(value: true);
				stunnedEffect.transform.position = stunnedPivot.position;
				boss10Collider.ramCheckObj.SetActive(value: false);
				SetTriggers("DurationEnd");
				footEffect.Stop();
			}
			distanceToTarget = Mathf.MoveTowards(distanceToTarget, 0f, Time.deltaTime * 0.8f);
			SetFloats("DistanceToTarget", distanceToTarget);
			if (stateExistTime > vertigoTime)
			{
				UnitProperty_Dots componentData8 = GetComponentData<UnitProperty_Dots>();
				componentData8.CanTouch = true;
				SetComponentData(componentData8);
				stunnedEffect.SetActive(value: false);
				state = MonsterState.Idle;
			}
			else
			{
				SetMove(Vector3.zero);
			}
			break;
		case MonsterState.Crouch:
		{
			ref float reference3 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				shieldMesh.material.SetFloat("_AlphaByY", 0f);
				crouchEffect.SetActive(value: true);
				crouchParticleObj.SetActive(value: true);
				UnitProperty_Dots componentData11 = GetComponentData<UnitProperty_Dots>();
				componentData11.InvincibleRegister();
				SetComponentData(componentData11);
			}
			if (stateExistTime > crouchTime)
			{
				if (shieldMesh.material.GetFloat("_AlphaByY") > 0f)
				{
					shieldMesh.material.SetFloat("_AlphaByY", shieldMesh.material.GetFloat("_AlphaByY") - Time.deltaTime);
					break;
				}
				UnitProperty_Dots componentData12 = GetComponentData<UnitProperty_Dots>();
				componentData12.InvincibleUnregister();
				SetComponentData(componentData12);
				PlayAnims("EyeDamagedEnd");
				reference3 = 0f;
				state = MonsterState.PostCastDelay;
				break;
			}
			if (shieldMesh.material.GetFloat("_AlphaByY") < 1f)
			{
				shieldMesh.material.SetFloat("_AlphaByY", shieldMesh.material.GetFloat("_AlphaByY") + Time.deltaTime);
			}
			reference3 += Time.deltaTime;
			if (reference3 > cureCDTime)
			{
				reference3 = 0f;
				float num2 = GetComponentData<UnitProperty_Dots>(myPpt.myEntity).unitCfg.maxHP * curePercentage;
				UnitDotsSyncSystem.UnitRecoveryHP(myPpt.myEntity, num2, UnitDotsSyncSystem.entityMgr, needTextFloat: false);
				QuickCreateSystem.Inst.CreateTextFloatVFX(num2, UITextFloatType.Recover, base.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * (float)UnityEngine.Random.Range(1, 5));
			}
			SetMove(Vector3.zero);
			break;
		}
		case MonsterState.Smash:
			if (changedState)
			{
				SetTriggers("Attack1");
				SetFloats("DistanceToTarget", 0f);
				targetAngle = 0f - (bodyRotateTransform.localEulerAngles.y - dirOffset);
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.FallTnt:
			if (changedState)
			{
				StartCoroutine(TNTFall());
			}
			if (stateExistTime > smashRecoverTime)
			{
				state = MonsterState.Idle;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Summon:
			if (changedState)
			{
				SetTriggers("Attack2");
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Heal:
			if (changedState)
			{
				SetTriggers("EyeDamaged");
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.ChangeStage:
			SetMove(Vector3.zero);
			if (isRoar)
			{
				UnitProperty_Dots componentData9 = GetComponentData<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt);
				componentData9.TakeKnockback(Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position) * force * Time.deltaTime);
				SetComponentData(componentData9, PlayerMgr.Inst.PlayerEtt);
			}
			if (isChangingStage)
			{
				throwParticle.transform.position = bulletPivot.position;
			}
			break;
		case MonsterState.Dead:
		{
			ref float reference2 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				if ((bool)warningCircle && warningCircle.gameObject.activeInHierarchy)
				{
					ObjPoolMgr.Inst.RecycleGO(warningCircle.gameObject);
				}
				PlayAnims("Die01");
				SEMgr.Inst.boss10_Die.PlaySE();
				footEffect.Stop();
			}
			if (stateExistTime > deadExplosionDelay)
			{
				reference2 += Time.deltaTime;
				if (reference2 > deadExplosionInterval)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10Explosion 1", Tool2D.GetLayerPoint(base.transform.position + Tool2D.GetDir() * UnityEngine.Random.value * deadExplosionRadius + Vector3.back * UnityEngine.Random.value * deadExplosionRadius), Quaternion.identity, Vector3.one * 1.5f, 4f);
					CamController.Inst.SetShock(smallShock);
					reference2 = 0f;
				}
			}
			SetMove(Vector3.zero);
			break;
		}
		case MonsterState.DeadWhiteScreen:
		{
			ref bool reference = ref varMgr.RegBool(0);
			if (stateExistTime > 1.7f)
			{
				foreach (Boss10TNT tnt in tntList)
				{
					tnt.disableDeadBoom = true;
					tnt.DotsAnnouncedDeath();
				}
				PlayAnims("Die01_Move 0");
			}
			if (stateExistTime > 4f && !reference)
			{
				reference = true;
				SEMgr.Inst.boss10_Escape.PlaySE();
			}
			SetMove(Vector3.zero);
			break;
		}
		}
	}

	public void OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
		if (state == MonsterState.Ram && UnitDotsSyncSystem.GetLayer(collision.GetOtherEntity(myPpt.myEntity)) == 256)
		{
			SEMgr.Inst.boss10_HitWall.PlaySE();
			if (isStage2)
			{
				DoubleArcBullet(crushBulletCount, base.transform.position);
			}
			CamController.Inst.SetShock(shockParam_Ram);
			state = MonsterState.RamVertigo;
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10_KnockWall", collision.CollisionDetails.FirstContactPosition, 3f).transform.up = collision.Normal;
		}
	}

	public void OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
		((IDotsCollisionReceiver)this).OnCollisionEnter_Dots(collision);
	}

	public void OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}

	public void NormalJump(float upForce, float gravity)
	{
		float num = GeneralTool.CannonSpeed(upForce, 0f, gravity, Vector3.Distance(base.transform.position, jumpPoint));
		float num2 = Vector3.Distance(base.transform.position, jumpPoint) / num;
		JumpStart_Dots(upForce, gravity);
		SetAnimSpeed(1f / num2);
		base.Rigid.linearVelocity = ToPointDir(jumpPoint) * num;
		SyncDotsVelocity();
	}

	public void UpperCutBulletAttack()
	{
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		CamController.Inst.SetShock(smallShock);
		SEMgr.Inst.boss10_UpperCutUp.PlaySE();
		if (!isStage2)
		{
			_ = base.transform.position + Tool2D.GetDir(Vector3.up, targetAngle) * 6f;
			if (base.HaveTarget)
			{
				_ = base.TargetPoint;
			}
			for (int i = 0; i < uppercutBulletCount; i++)
			{
				sSPModifier.Direction = Tool2D.GetDir(Tool2D.GetDir(Vector3.up, targetAngle - 50f), (float)i * uppercutBulletAngle / (float)uppercutBulletCount);
				sSPModifier.Speed = spellSpeed + 1f;
				sSPModifier.SpawnPosition = new Vector3(0f, 0f, 0f - spellHeight) + Tool2D.IgnoreZPoint(bulletPivot.position);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			for (int j = 0; j < uppercutBulletCount; j++)
			{
				sSPModifier.Direction = Tool2D.GetDir(Tool2D.GetDir(Vector3.up, targetAngle - 50f), (float)j * uppercutBulletAngle / (float)uppercutBulletCount);
				sSPModifier.Speed = spellSpeed - 1f;
				sSPModifier.SpawnPosition = new Vector3(0f, 0f, 0f - spellHeight) + Tool2D.IgnoreZPoint(bulletPivot.position);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			return;
		}
		if (throwsRock)
		{
			Boss10ParabolaTNT component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10ParabolaTnt", Tool2D.IgnoreZPoint(bulletPivot.position)).GetComponent<Boss10ParabolaTNT>();
			component.spriteRenderer.sprite = component.rockSprite;
			component.shootBullet = true;
			component.isTnt = false;
			if (base.HaveTarget)
			{
				component.Initialize(base.TargetPoint, throwRockUpSpeed, isTnt: false);
			}
			else
			{
				component.Initialize(PlayerMgr.Inst.PlayerPoint, throwRockUpSpeed, isTnt: false);
			}
			return;
		}
		for (int k = 0; k < uppercutBulletCount; k++)
		{
			sSPModifier.Direction = Tool2D.GetDir(Tool2D.GetDir(Vector3.up, targetAngle - 50f), (float)k * uppercutBulletAngle / (float)uppercutBulletCount);
			sSPModifier.Speed = spellSpeed + 1f;
			sSPModifier.SpawnPosition = new Vector3(0f, 0f, 0f - spellHeight) + Tool2D.IgnoreZPoint(bulletPivot.position);
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
		}
		for (int l = 0; l < uppercutBulletCount; l++)
		{
			sSPModifier.Direction = Tool2D.GetDir(Tool2D.GetDir(Vector3.up, targetAngle - 50f), (float)l * uppercutBulletAngle / (float)uppercutBulletCount);
			sSPModifier.Speed = spellSpeed - 1f;
			sSPModifier.SpawnPosition = new Vector3(0f, 0f, 0f - spellHeight) + Tool2D.IgnoreZPoint(bulletPivot.position);
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
		}
		for (int m = 0; m < uppercutBulletCount; m++)
		{
			sSPModifier.Direction = Tool2D.GetDir(Tool2D.GetDir(Vector3.up, targetAngle - 50f), (float)m * uppercutBulletAngle / (float)uppercutBulletCount);
			sSPModifier.Speed = spellSpeed - 3f;
			sSPModifier.SpawnPosition = new Vector3(0f, 0f, 0f - spellHeight) + Tool2D.IgnoreZPoint(bulletPivot.position);
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
		}
	}

	private void DropDamage()
	{
		landParticleSystem.Play();
		CamController.Inst.SetShock(shockParam_Crush);
		DoubleArcBullet(crushBulletCount, base.transform.position);
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, damageRadius, GameConst.Filter_MonsterAoeNoSpell, results);
		foreach (UnitDotsSyncSystem.DistanceHitResult result in results)
		{
			switch (UnitDotsSyncSystem.GetLayer(result.entity))
			{
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
			{
				TakeDamageInfo_Dots takeDamageInfo_Dots = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
				takeDamageInfo_Dots.teammateTakeDamageRatio = 3f;
				takeDamageInfo_Dots.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(result.point, base.transform.position).normalized * 5f;
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch_Large", result.point, 1f);
				break;
			}
			}
		}
	}

	public void DoubleArcBullet(int bulletCount, Vector3 bulletPivot, float offsetDistance = 2f)
	{
		Vector3 dir = Tool2D.GetDir();
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		for (int i = 0; i < bulletCount; i++)
		{
			sSPModifier.Direction = Tool2D.GetDir(dir, (float)i * 360f / (float)bulletCount);
			sSPModifier.Speed = spellSpeed + 1f;
			sSPModifier.SpawnPosition = new Vector3(0f, 0f, 0f - spellHeight) + bulletPivot + sSPModifier.Direction * offsetDistance;
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
		}
		dir = Tool2D.GetDir(dir, 180f / (float)bulletCount);
		for (int j = 0; j < bulletCount; j++)
		{
			sSPModifier.Direction = Tool2D.GetDir(dir, (float)j * 360f / (float)bulletCount);
			sSPModifier.Speed = spellSpeed - 1f;
			sSPModifier.SpawnPosition = new Vector3(0f, 0f, 0f - spellHeight) + Tool2D.IgnoreZPoint(bulletPivot) + sSPModifier.Direction * offsetDistance;
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
		}
	}

	private IEnumerator RockFall()
	{
		SEMgr.Inst.elite12FallRock.PlaySE();
		for (int i = 0; (float)i < fallRockRounds; i++)
		{
			for (int j = 0; (float)j < fallRockCount; j++)
			{
				float num = (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.width - summonFromBorderSize * 2f;
				float num2 = (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height - summonFromBorderSize * 2f;
				Vector3 centerPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
				Vector3 point = PlayerMgr.Inst.PlayerPoint + PlayerMgr.Inst.PlayerCtrller.CurrentMotion + Tool2D.GetDir() * UnityEngine.Random.Range(0f, 3f);
				point.x = Mathf.Clamp(point.x, centerPoint.x - num / 2f, centerPoint.x + num / 2f);
				point.y = Mathf.Clamp(point.y, centerPoint.y - num2 / 2f, centerPoint.y + num2 / 2f);
				if ((float)i < fallRockChasePlayerCount)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10_FallObj", point);
				}
				else
				{
					point = new Vector3(UnityEngine.Random.Range((0f - num) / 2f, num / 2f), UnityEngine.Random.Range((0f - num2) / 2f, num2 / 2f), 0f);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10_FallObj", centerPoint + point);
				}
				yield return new WaitForSeconds(rockInterval.RandomResult());
			}
		}
	}

	private IEnumerator TNTFall()
	{
		SEMgr.Inst.elite12FallRock.PlaySE();
		for (int i = 0; (float)i < fallTntAmount; i++)
		{
			Vector3 vector = PlayerMgr.Inst.PlayerPoint + PlayerMgr.Inst.PlayerCtrller.CurrentMotion + Tool2D.GetDir() * UnityEngine.Random.Range(0f, 3f) + new Vector3(0f, 0f, fallTntHeight);
			float num = (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.width - summonFromBorderSize * 2f;
			float num2 = (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height - summonFromBorderSize * 2f;
			Vector3 centerPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			vector = new Vector3(Mathf.Clamp(vector.x, centerPoint.x - num / 2f, centerPoint.x + num / 2f), Mathf.Clamp(vector.y, centerPoint.y - num2 / 2f, centerPoint.y + num2 / 2f), vector.z);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10ParabolaTnt", vector).GetComponent<Boss10ParabolaTNT>().Initialize(Tool2D.IgnoreZPoint(vector), 0f, isTnt: true);
			yield return new WaitForSeconds(fallTntInterval.RandomResult());
		}
	}

	public void SetTriggers(string triggerName)
	{
		outLineAnima.SetTrigger(triggerName);
		shadowAnima.SetTrigger(triggerName);
		base.Anima.SetTrigger(triggerName);
	}

	public void SetFloats(string floatName, float value)
	{
		outLineAnima.SetFloat(floatName, value);
		shadowAnima.SetFloat(floatName, value);
		base.Anima.SetFloat(floatName, value);
	}

	public void SetBools(string bollName, bool value)
	{
		outLineAnima.SetBool(bollName, value);
		shadowAnima.SetBool(bollName, value);
		base.Anima.SetBool(bollName, value);
	}

	public void PlayAnims(string animName)
	{
		outLineAnima.Play(animName);
		shadowAnima.Play(animName);
		base.Anima.Play(animName);
	}

	public void SetAnimSpeed(float speed)
	{
		outLineAnima.speed = speed;
		shadowAnima.speed = speed;
		base.Anima.speed = speed;
	}

	public override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		if (state == MonsterState.Jump)
		{
			deathDelay = true;
			info.stopAnnouncedDeath = true;
		}
		else
		{
			base.BeforeAnnouncedDeath_Dots(ref info);
		}
	}

	protected override void BossDeadStay()
	{
		if (!isDying)
		{
			StopAllCoroutines();
			LateUpdate();
			state = MonsterState.Dead;
			isDying = true;
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.BossDeadStay();
			SetComponentData(componentData);
			boss10Collider.SetCanBeTarget(value: false);
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		boss10Collider.DotsAnnouncedDeath();
		base.AfterDead(ref info);
		tsf_Model.gameObject.SetActive(value: false);
		base.transform.position += Tool2D.GetDir(nowAngle) * 4f;
		SyncDotsPosition();
		DataMgr.selectedWorldData.daveKilledBoss4 = true;
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		base.BeforeTakeDamage_Dots(ref info);
		if (crouchHitEffect.gameObject.activeInHierarchy)
		{
			crouchHitEffect.Play();
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "WhiteScreen":
			GameUISingletonMono<UIWhiteScreen>.ShowInit((1.5f, 1.2f, 0.5f));
			break;
		case "Dying":
			state = MonsterState.DeadWhiteScreen;
			break;
		case "Dead":
			DotsAnnouncedDeath();
			break;
		case "DigDustEffect":
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10DigDustEffect", Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(digDustEffectPivot.position)), 3f);
			break;
		}
		if (isDying)
		{
			return;
		}
		switch (animaName)
		{
		case "UpperCutBulletAttack":
			UpperCutBulletAttack();
			break;
		case "StopCrouchEffect":
			crouchParticleObj.SetActive(value: false);
			crouchEffect.SetActive(value: false);
			break;
		case "ThrowParticlePlay":
			throwParticle.Play();
			break;
		case "UpperCutFinish":
			state = MonsterState.Move;
			break;
		case "ReturnMove":
		{
			state = MonsterState.Move;
			UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
			componentData2.InvincibleUnregister();
			SetComponentData(componentData2);
			break;
		}
		case "Crush":
			state = MonsterState.Jump;
			break;
		case "Ram":
			state = MonsterState.Ram;
			break;
		case "CrushEnd":
			state = MonsterState.Idle;
			break;
		case "GenerateTNT":
			CamController.Inst.SetShock(shockParam_TNT);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10_BigKnock", summonParticlePivot.position, 7f);
			StartCoroutine(GenerateTnt());
			break;
		case "TNTDown":
			SEMgr.Inst.boss10_TNTDown.PlaySE();
			break;
		case "TNTUp":
			SEMgr.Inst.boss10_TNTUp.PlaySE();
			break;
		case "ChangeToCrouch":
			state = MonsterState.Crouch;
			break;
		case "Angry":
			SEMgr.Inst.boss10_Angry.PlaySE();
			break;
		case "PlayFistEffect":
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10_FistEffect", fistEffectPivot.position);
			break;
		case "UpperCutDownSound":
			CamController.Inst.SetShock(smallShock);
			SEMgr.Inst.boss10_UpperCutDown.PlaySE();
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10_DustDirtySoft", bulletPivot.position, 2f);
			break;
		case "FootSound":
			SEMgr.Inst.boss10_FootStep.PlaySE();
			break;
		case "RoarSound":
			SEMgr.Inst.boss10_Roar.PlaySE();
			break;
		case "Roar":
			isRoar = true;
			roarParticleSystem.Play();
			roarParticleSystem.transform.position = headPivot.position;
			CamController.Inst.SetShock(shockParam);
			break;
		case "EndRoar":
			isRoar = false;
			break;
		case "StartSwing":
			throwParticle.Play();
			break;
		case "RoarShake":
			CamController.Inst.SetShock(shockParam_Change);
			break;
		case "EndChangeStage":
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.InvincibleUnregister();
			SetComponentData(componentData);
			isStage2 = true;
			isChangingStage = false;
			PlayAnims("MoveToTarget");
			state = MonsterState.Move;
			break;
		}
		case "Smash":
		{
			SEMgr.Inst.boss10_CrushOnGround.PlaySE();
			downAttackEffect.Play();
			CamController.Inst.SetShock(shockParam_Ram);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10SmashEffect", bulletPivot.position, 3f);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10SmashTrace", bulletPivot.position, 10f).transform.localScale = new Vector3(3f, 3f, 3f);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10_BigKnock", bulletPivot.position, 7f);
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
			info.damage = UnityEngine.Random.Range(5, 11) * 10;
			foreach (Boss10TNT tnt in tntList)
			{
				UnitDotsSyncSystem.AddTakeDamageRequest(tnt.myPpt.myEntity, info);
			}
			state = MonsterState.FallTnt;
			break;
		}
		}
	}

	private IEnumerator GenerateTnt()
	{
		if (isStage2)
		{
			tntCount = secondStageTntCount;
			tntAroundPlayerCount = secondStageTntAroundPlayerCount;
		}
		float width = (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.width - summonFromBorderSize * 2f;
		float height = (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height - summonFromBorderSize * 2f;
		for (int i = 0; i < tntCount; i++)
		{
			Vector3 centerPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			Vector3 startPoint = centerPoint + new Vector3(UnityEngine.Random.Range((0f - width) / 2f, width / 2f), UnityEngine.Random.Range((0f - height) / 2f, height / 2f), 0f);
			startPoint = Tool2D.GetNavMeshPointIngoreZ(startPoint);
			if (i < tntAroundPlayerCount && base.HaveTarget)
			{
				startPoint = Tool2D.GetNavMeshPointIngoreZ(base.TargetPoint, tntAroundPlayerRange);
				startPoint.x = Mathf.Clamp(startPoint.x, centerPoint.x - width / 2f, centerPoint.x + width / 2f);
				startPoint.y = Mathf.Clamp(startPoint.y, centerPoint.y - height / 2f, centerPoint.y + height / 2f);
			}
			tntList.Add(ObjPoolMgr.Inst.GetGO("Prefabs/Units/501031", startPoint).GetComponent<Boss10TNT>());
			yield return new WaitForSeconds(0.1f);
		}
	}
}
