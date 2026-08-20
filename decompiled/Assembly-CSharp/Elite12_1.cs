using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;
using UnityEngine;

public class Elite12_1 : UnitBase, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		Follow,
		JumpCharge,
		JumpFly,
		JumpOnGround,
		JumpAgain,
		DashCharge,
		Dash,
		DashAfter,
		DashAgain,
		KnockGround,
		Fly,
		Drop,
		DropRecover,
		ChangeStage
	}

	[Space(50f)]
	[Header("状态")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("技能选择")]
	public float jumpChance;

	public float knockGroundChance;

	public float dashChance;

	public float flyChance;

	[Header("行动")]
	public VariableFloat actionInterval;

	public VariableFloat secondStageActionInterval;

	private float actionIntervalTimer;

	[Header("移动")]
	public VariableFloat idleTime;

	public VariableFloat moveTime;

	public VariableFloat moveDistance;

	public VariableFloat secondStageIdleTime;

	public float moveAngleRange;

	public float moveRotateSpeed;

	[Header("跳砸")]
	public Transform tsf_Motion;

	public float jumpKeepDistance;

	public float jumpTime;

	public float jumpForwardForce;

	public float jumpMinForwardForce;

	public ShockParam jumpShock;

	public float jumpOnGroundKnockbackRatio;

	public float jumpDamageRadius;

	public float jumpKnockBack;

	public int jumpDamage;

	public int secondStageJumpCount;

	public float jumpHorizontalOffset;

	private int secondStageJumpCounter;

	private Vector3 jumpForce;

	[Header("撞墙和落石")]
	public LineRenderer warningLine;

	public LineRenderer warningLine_H;

	public float warningLineDistance;

	public float dashZoneOutDistance;

	private bool stopAim;

	public int minRocksCount;

	public int rocksCount;

	public float rockKeepDistance;

	public List<Elite12_FallRock> rocks = new List<Elite12_FallRock>();

	public List<float> rocksAngle = new List<float>();

	public List<float> rocksAngleDelta = new List<float>();

	public Vector3 dashDirection;

	public float dashChaseTime;

	public float dashSpeedRatio;

	public float dashRotateSpeed;

	public Elite12_AttackZone attackZone;

	public float rockSpikeChance;

	public float secondStageRockSpikeChance;

	public ShockParam knockWallShock;

	public ShockParam knockWallContinueShock;

	public int secondStageDashCount;

	public float secondStageDashWaveInterval;

	public float secondStageDashWaveSpeed;

	private int secondStageDashCounter;

	private bool dashAgain;

	[Header("裂地冲击")]
	public float groundWaveCount;

	public float groundWaveAngle;

	public float groundWaveSpeed;

	public float secondStageGroundWaveCount;

	public ShockParam groundWaveShock;

	[Header("伙伴和二阶段")]
	public ParticleSystem shoutParticle;

	public ShockParam switchStageShock;

	public float skillRepeatChance;

	public bool changedStage2;

	public static Elite12_1 Inst;

	public Elite12_2 farMate;

	[Header("二阶段锤大地")]
	public VariableInt knockGroundRockCount;

	public float knockGroundRadius;

	public float knockGroundDamage;

	public float knockGroundKnockBack;

	public float knockGroundTime;

	[Header("二阶段地球上投")]
	public float flyDropKeepDistance;

	public float flyDropTime;

	public float flyDropRadius;

	public int flyDropDamage;

	public float flyDropKnockBack;

	public ParticleSystem jumpParticle;

	public ParticleSystem dropParticle;

	public ParticleSystem jumpParticle_H;

	public ParticleSystem dropParticle_H;

	public Shadow shadow;

	public float shadowFadeHeight;

	private float originShadowScale;

	private WarningArea warningArea;

	[Header("脚步声")]
	public float footStepInterval;

	public float dashFootStepInterval;

	private float footStepTimer;

	[Header("其他")]
	public Transform tsf_Model;

	public Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	public static MiniObjPool MiniPool;

	private List<MonsterState> skills = new List<MonsterState>
	{
		MonsterState.JumpCharge,
		MonsterState.KnockGround,
		MonsterState.DashCharge,
		MonsterState.Fly
	};

	private MonsterState lastSkill;

	private List<UnitDotsSyncSystem.DistanceHitResult> results = new List<UnitDotsSyncSystem.DistanceHitResult>();

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

	public Entity thisEntity { get; set; }

	public override void SingleInitialCallback()
	{
		if (MiniPool == null)
		{
			MiniPool = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool"), LevelMgr.Inst.CurrentRoomT).GetComponent<MiniObjPool>();
		}
		MiniPool.PreloadGO("Prefabs/EF/ef_Elite12_1_LandBig" + (GameMgr.IsChAge14_Static ? " H" : ""), 1f);
		MiniPool.PreloadGO("Prefabs/EF/ef_Elite12_1_Land" + (GameMgr.IsChAge14_Static ? " H" : ""), 1f);
		if (GameMgr.IsChAge14_Static)
		{
			base.SAnima.initialSkinName = base.SAnima.initialSkinName + "_HX";
		}
		if (GameMgr.IsMobile_Static)
		{
			flyDropRadius *= 0.85f;
		}
	}

	public override void Frame1InitialCallback()
	{
		for (int i = 0; i < rocksCount; i++)
		{
			float value = UnityEngine.Random.value;
			float num = Mathf.Lerp(0f, rockKeepDistance, Mathf.Pow(value, 0.5f));
			Vector3 vector = roomCenterPoint + num * Tool2D.GetDir(GetSortDir(), 5f);
			for (int j = 0; j < 30; j++)
			{
				if (Tool2D.PointOnNavMesh(vector))
				{
					break;
				}
				value = UnityEngine.Random.value;
				num = Mathf.Lerp(0f, rockKeepDistance, Mathf.Pow(value, 0.5f));
				vector = roomCenterPoint + num * Tool2D.GetDir(GetSortDir(), 5f);
			}
			if (!Tool2D.PointOnNavMesh(vector))
			{
				vector = Tool2D.GetNavMeshPointIngoreZ(roomCenterPoint, num, GetSortDir(), 5f);
			}
			Elite12_FallRock component = MiniPool.GetGO("Prefabs/SpecialObjs/111", vector).GetComponent<Elite12_FallRock>();
			component.Initialize(isStand: true, GeneralTool.ChanceResult(rockSpikeChance));
			rocks.Add(component);
		}
		originShadowScale = shadow.shadowScale;
	}

	public override void EveryInitialCallback()
	{
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.width;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height;
		actionInterval.RandomResult();
		MiniPool = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool"), LevelMgr.Inst.CurrentRoomT).GetComponent<MiniObjPool>();
		Inst = this;
		if (GameMgr.IsChAge14_Static)
		{
			warningLine = warningLine_H;
			jumpParticle = jumpParticle_H;
			dropParticle = dropParticle_H;
		}
		warningLine.enabled = false;
		warningLine.positionCount = 10;
		if (Elite12_2.Inst != null && Elite12_2.Inst.closeMate == null)
		{
			Elite12_2.Inst.closeMate = this;
			Inst.farMate = Elite12_2.Inst;
		}
	}

	private void CheckAction()
	{
		if (!farMate.gameObject.activeSelf && !changedStage2)
		{
			state = MonsterState.ChangeStage;
			changedStage2 = true;
			return;
		}
		actionIntervalTimer += Time.deltaTime;
		if (!(actionIntervalTimer >= actionInterval.result))
		{
			return;
		}
		actionIntervalTimer = 0f;
		actionInterval.RandomResult();
		int weightRandom = GeneralTool.GetWeightRandom(jumpChance, 0f, dashChance);
		if (changedStage2)
		{
			weightRandom = GeneralTool.GetWeightRandom(jumpChance, knockGroundChance, dashChance, flyChance);
		}
		while (lastSkill == skills[weightRandom])
		{
			weightRandom = GeneralTool.GetWeightRandom(jumpChance, 0f, dashChance);
			if (changedStage2)
			{
				weightRandom = GeneralTool.GetWeightRandom(jumpChance, knockGroundChance, dashChance, flyChance);
			}
		}
		state = skills[weightRandom];
		lastSkill = state;
		dashAgain = false;
	}

	private Vector3 GetSortDir()
	{
		if (rocks.Count < 3)
		{
			return Tool2D.GetDir();
		}
		rocks.Sort();
		rocksAngle.Clear();
		rocksAngleDelta.Clear();
		for (int i = 0; i < rocks.Count; i++)
		{
			float num = Tool2D.IgnoreZAngleWithSign(Vector3.up, rocks[i].transform.position - roomCenterPoint);
			if (num < 0f)
			{
				num += 360f;
			}
			rocksAngle.Add(num);
		}
		for (int j = 0; j < rocksAngle.Count; j++)
		{
			int num2 = j + 1;
			if (num2 >= rocksAngle.Count)
			{
				num2 = 0;
			}
			float num3 = rocksAngle[j] - rocksAngle[num2];
			if (num3 < 0f)
			{
				num3 += 360f;
			}
			rocksAngleDelta.Add(num3);
		}
		int index = 0;
		float num4 = 0f;
		for (int k = 0; k < rocksAngleDelta.Count; k++)
		{
			if (rocksAngleDelta[k] > num4)
			{
				index = k;
				num4 = rocksAngleDelta[k];
			}
		}
		return Tool2D.GetDir(Vector3.up, rocksAngle[index] - rocksAngleDelta[index] / 2f).normalized;
	}

	private void FootStep()
	{
		footStepTimer += Time.deltaTime;
		if ((state == MonsterState.Follow && footStepTimer > footStepInterval) || (state == MonsterState.Dash && footStepTimer > dashFootStepInterval))
		{
			footStepTimer = 0f;
			SEMgr.Inst.monster37_Step.PlaySE();
		}
	}

	protected override void SetFlip(float motionX)
	{
		tsf_Model.localScale = new Vector3((motionX > 0f) ? 1 : (-1), 1f, 1f);
	}

	public unsafe override void Update()
	{
		for (int num = rocks.Count - 1; num >= 0; num--)
		{
			if (!rocks[num].gameObject.activeSelf)
			{
				rocks.RemoveAt(num);
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
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
			}
			SetMove(Vector3.zero);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = MonsterState.Follow;
				}
				else
				{
					state = MonsterState.Idle;
				}
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				idleTime.RandomResult();
				base.Anima.Play("Idle");
				base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
			}
			SetMove(Vector3.zero);
			checkTargetIntervalTimer += Time.deltaTime;
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.Follow;
			}
			else
			{
				CheckAction();
			}
			break;
		case MonsterState.Follow:
			_ = ref varMgr.RegV3(0);
			if (changedState)
			{
				base.Anima.Play("Walk");
				base.SAnima.AnimationState.SetAnimation(0, "Move", loop: true);
				Vector3 dir = Tool2D.GetDir();
				GetNearestTarget();
				if (base.HaveTarget)
				{
					dir = Tool2D.GetDir(ToTargetDir(), (UnityEngine.Random.value - 0.5f) * moveAngleRange);
				}
				GetNavInfo(base.transform.position + dir * moveDistance.RandomResult());
				footStepTimer = footStepInterval;
			}
			FootStep();
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				state = MonsterState.Idle;
				break;
			}
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			CheckAction();
			break;
		case MonsterState.JumpCharge:
			if (changedState)
			{
				base.Anima.Play("Jump", 0, 0f);
				SEMgr.Inst.elite12_1Roar1.PlaySE();
				base.SAnima.AnimationState.SetAnimation(0, "JumpBefore", loop: false);
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDelta().x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.JumpAgain:
			if (changedState)
			{
				base.Anima.Play("JumpAgain", 0, 0f);
				SEMgr.Inst.elite12_1Roar1.PlaySE();
				base.SAnima.AnimationState.SetAnimation(0, "JumpAfter_2", loop: false);
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDelta().x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.JumpFly:
		{
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "Jump", loop: false);
				base.gameObject.layer = LayerMask.NameToLayer("Monster_Ghost");
				PhysicsCollider pc = GetComponentData<PhysicsCollider>();
				DTool.SetCollider(in pc, 8192u, DTool.GetCollidesWith(8192u));
				SetComponentData(pc);
				UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
				componentData3.CanTouch = false;
				componentData3.FlyRegister();
				componentData3.ImmuneKnockbackRegister();
				componentData3.IsVelocityDeclice = false;
				SetComponentData(componentData3);
				GetNearestTarget();
				Vector3 vector = (base.HaveTarget ? base.TargetPoint : (roomCenterPoint + Tool2D.GetDir() * UnityEngine.Random.Range(0f, 10f)));
				vector = base.transform.position + ToPointDir(vector) * Mathf.Clamp(ToPointDistance(vector - ToPointDir(vector) * jumpKeepDistance), jumpMinForwardForce * jumpTime, jumpForwardForce * jumpTime);
				vector = Tool2D.GetNavMeshPointIngoreZ(vector, 8);
				float num2 = ToPointDistance(vector) / jumpTime;
				jumpForce = num2 * ToPointDir(vector).normalized;
				base.Rigid.linearVelocity = jumpForce;
				SyncDotsVelocity();
				SetFlip(jumpForce.x);
			}
			base.CC_Self.center = new Vector3(0f, 0f, 0f - (tsf_Motion.position.y - base.transform.position.y));
			_ = tsf_Motion.position;
			_ = base.transform.position;
			PhysicsCollider componentData4 = GetComponentData<PhysicsCollider>();
			Unity.Physics.CapsuleCollider* colliderPtr = (Unity.Physics.CapsuleCollider*)componentData4.ColliderPtr;
			CapsuleGeometry geometry = colliderPtr->Geometry;
			float height = base.CC_Self.height;
			Vector3 center = base.CC_Self.center;
			geometry.Vertex0 = center - new Vector3(0f, 0f, height / 2f);
			geometry.Vertex1 = center + new Vector3(0f, 0f, height / 2f);
			geometry.Radius = base.CC_Self.radius;
			colliderPtr->Geometry = geometry;
			SetComponentData(componentData4);
			break;
		}
		case MonsterState.JumpOnGround:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "JumpAfter", loop: false);
				base.gameObject.layer = LayerMask.NameToLayer("Monster");
				UnitProperty_Dots componentData5 = GetComponentData<UnitProperty_Dots>();
				componentData5.CanTouch = true;
				componentData5.FlyUnregister();
				componentData5.ImmuneKnockbackUnregister();
				componentData5.IsVelocityDeclice = true;
				SetComponentData(componentData5);
				JumpKnockGround();
				PhysicsCollider pc2 = GetComponentData<PhysicsCollider>();
				Unity.Physics.CapsuleCollider* colliderPtr2 = (Unity.Physics.CapsuleCollider*)pc2.ColliderPtr;
				CapsuleGeometry geometry2 = colliderPtr2->Geometry;
				float height2 = base.CC_Self.height;
				Vector3 center2 = base.CC_Self.center;
				geometry2.Vertex0 = center2 - new Vector3(0f, 0f, height2 / 2f);
				geometry2.Vertex1 = center2 + new Vector3(0f, 0f, height2 / 2f);
				geometry2.Radius = base.CC_Self.radius;
				colliderPtr2->Geometry = geometry2;
				DTool.SetCollider(in pc2, 2048u, DTool.GetCollidesWith(2048u));
				SetComponentData(pc2);
				secondStageJumpCounter++;
				if (changedStage2 && secondStageJumpCounter < secondStageJumpCount && (GeneralTool.ChanceResult(skillRepeatChance) || secondStageJumpCounter == 2))
				{
					state = MonsterState.JumpAgain;
					break;
				}
			}
			base.transform.position = Tool2D.IgnoreZPoint(base.transform.position);
			SyncDotsPosition();
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.DashCharge:
		{
			if (changedState)
			{
				if (!dashAgain)
				{
					base.SAnima.AnimationState.SetAnimation(0, "DashBefore_2", loop: false);
					base.Anima.Play("DashCharge");
					dashAgain = true;
				}
				else
				{
					base.SAnima.AnimationState.SetAnimation(0, "DashAfter_2_2", loop: false);
					base.Anima.Play("DashAgain");
				}
				base.Anima.Play("DashCharge");
				warningLine.enabled = true;
				stopAim = false;
				dashDirection = Tool2D.GetDir(ToPointDir(roomCenterPoint), UnityEngine.Random.Range(-45, 45));
			}
			SetMove(Vector3.zero);
			GetNearestTargetPlayerFirst();
			if (base.HaveTarget && !stopAim)
			{
				dashDirection = ToTargetDir();
			}
			SetFlip(dashDirection.x);
			Vector3 position2 = base.transform.position;
			Vector3 b = position2 + dashDirection * warningLineDistance;
			for (int i = 0; i < warningLine.positionCount; i++)
			{
				Vector3 rootPoint = Vector3.Lerp(position2, b, (float)i / (float)(warningLine.positionCount - 1));
				warningLine.SetPosition(i, Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect));
			}
			break;
		}
		case MonsterState.Dash:
		{
			ref bool reference = ref varMgr.RegBool(0);
			ref float reference2 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				warningLine.enabled = false;
				attackZone.Damage();
				base.Anima.Play("Dash");
				base.SAnima.AnimationState.SetAnimation(0, "Dash_2", loop: true);
				GetNearestTargetPlayerFirst();
				base.Rigid.linearVelocity = dashDirection * base.MoveSpeed * dashSpeedRatio;
				SyncDotsVelocity();
				UnitProperty_Dots componentData6 = GetComponentData<UnitProperty_Dots>();
				componentData6.CanTouch = false;
				componentData6.IsVelocityDeclice = false;
				componentData6.ImmuneKnockbackRegister();
				SetComponentData(componentData6);
				footStepTimer = dashFootStepInterval;
			}
			if (changedStage2)
			{
				reference2 += dashSpeedRatio * base.MoveSpeed * Time.deltaTime;
				if (reference2 > secondStageDashWaveInterval)
				{
					reference2 -= secondStageDashWaveInterval;
					DashWave();
				}
			}
			FootStep();
			if (base.HaveTarget && !reference)
			{
				dashDirection = Tool2D.IgnoreZPoint(Vector3.RotateTowards(dashDirection, ToTargetDir(), dashSpeedRatio * base.MoveSpeed * dashRotateSpeed * (MathF.PI / 180f) * Time.deltaTime, 0f)).normalized;
				if (Vector3.Dot(ToTargetDir(), dashDirection) < 0f || stateExistTime > dashChaseTime)
				{
					reference = true;
				}
			}
			attackZone.transform.localPosition = dashDirection * dashZoneOutDistance;
			base.Rigid.linearVelocity = dashDirection * base.MoveSpeed * dashSpeedRatio;
			SyncDotsVelocity();
			SetFlip(base.Rigid.linearVelocity.x);
			break;
		}
		case MonsterState.DashAfter:
			if (changedState)
			{
				attackZone.NoDamage();
				base.Anima.Play("DashAfter");
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.CanTouch = true;
				componentData2.IsVelocityDeclice = true;
				componentData2.ImmuneKnockbackUnregister();
				SetComponentData(componentData2);
				base.SAnima.AnimationState.SetAnimation(0, "DashAfter", loop: false);
				secondStageDashCounter++;
				if (changedStage2 && secondStageDashCounter < secondStageDashCount && (GeneralTool.ChanceResult(skillRepeatChance) || secondStageJumpCounter == 2))
				{
					state = MonsterState.DashCharge;
					break;
				}
			}
			SetMove(Vector3.zero, isFlip: false);
			if (stateExistTime > 12f)
			{
				state = MonsterState.DashAfter;
			}
			break;
		case MonsterState.KnockGround:
			if (changedState)
			{
				SEMgr.Inst.elite12_1Roar3.PlaySE();
				base.Anima.Play("KnockGround");
				base.SAnima.AnimationState.SetAnimation(0, "KnockGround", loop: false);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Fly:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "Fly", loop: false);
				base.Anima.Play("Fly");
				base.gameObject.layer = LayerMask.NameToLayer("Monster_Ghost");
			}
			shadow.SetScale(originShadowScale * Mathf.Lerp(1f, 0f, (tsf_Motion.transform.position.y - base.transform.position.y) / shadowFadeHeight));
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Drop:
			if (changedState)
			{
				base.Anima.Play("Drop");
				base.SAnima.AnimationState.SetAnimation(0, "Drop", loop: false);
				Vector3 position = roomCenterPoint + Tool2D.GetDir() * UnityEngine.Random.Range(0f, 10f);
				base.transform.position = position;
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					base.transform.position = Tool2D.GetNavMeshPointIngoreZ(Tool2D.IgnoreZPoint(base.TargetPoint) + Tool2D.GetDir() * jumpKeepDistance, 8);
				}
				SyncDotsPosition();
				warningArea = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle" + (GameMgr.IsChAge14_Static ? " Purple" : ""), base.transform.position).GetComponent<WarningArea>();
				warningArea.Initialize(flyDropRadius, flyDropTime);
			}
			shadow.SetScale(originShadowScale * Mathf.Lerp(1f, 0f, (tsf_Motion.transform.position.y - base.transform.position.y) / shadowFadeHeight));
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.DropRecover:
			if (changedState)
			{
				shadow.shadowScale = originShadowScale;
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanTouch = true;
				componentData.InvincibleUnregister();
				SetComponentData(componentData);
				FlyDropGround();
				base.gameObject.layer = LayerMask.NameToLayer("Monster");
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.ChangeStage:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "Stage2Change", loop: false);
				secondStageActionInterval.RandomResult();
				secondStageIdleTime.RandomResult();
				base.Anima.Play("ChangeStage");
				actionInterval = secondStageActionInterval;
				idleTime = secondStageIdleTime;
			}
			SetMove(Vector3.zero);
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	void IDotsCollisionReceiver.OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}

	void IDotsCollisionReceiver.OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
		((IDotsCollisionReceiver)this).OnCollisionEnter_Dots(collision);
	}

	unsafe void IDotsCollisionReceiver.OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
		Entity otherEntity = collision.GetOtherEntity(myPpt.myEntity);
		if (state != MonsterState.Dash || stateExistTime < 0.5f)
		{
			return;
		}
		for (int i = 0; i < rocks.Count; i++)
		{
			if (otherEntity == rocks[i].thisEntity)
			{
				return;
			}
		}
		if (GetComponentData<PhysicsCollider>(otherEntity).ColliderPtr->GetCollisionFilter().BelongsTo == 256)
		{
			Vector3 vector = collision.CollisionDetails.FirstContactPosition;
			Vector3 normalized = (base.transform.position - vector).normalized;
			MiniPool.GetGO("Prefabs/EF/EF_Elite12_KnockWall" + (GameMgr.IsChAge14_Static ? " H" : ""), vector).GetComponent<Transform>().transform.localEulerAngles = Tool2D.GetEulerAngleByDir(normalized);
			GetNearestTargetPlayerFirst();
			state = MonsterState.DashAfter;
			SEMgr.Inst.monster37_KnockWall.PlaySE();
			SEMgr.Inst.elite12FallRock.PlaySE();
			CamController.Inst.SetShock(knockWallShock);
			StartCoroutine(SetAfterShock());
			GetFallRock();
		}
	}

	private IEnumerator SetAfterShock()
	{
		yield return new WaitForSeconds(knockWallShock.time);
		CamController.Inst.SetShock(knockWallContinueShock);
	}

	private void GetFallRock(int rockCount = -1)
	{
		int num = Mathf.Max(minRocksCount, rocksCount - rocks.Count);
		if (rockCount > 0)
		{
			num = Mathf.Max(rockCount, num);
		}
		for (int i = 0; i < num; i++)
		{
			float value = UnityEngine.Random.value;
			float num2 = Mathf.Lerp(0f, rockKeepDistance, Mathf.Pow(value, 0.5f));
			Vector3 vector = roomCenterPoint + num2 * Tool2D.GetDir(GetSortDir(), 5f);
			for (int j = 0; j < 30; j++)
			{
				if (Tool2D.PointOnNavMesh(vector))
				{
					break;
				}
				value = UnityEngine.Random.value;
				num2 = Mathf.Lerp(0f, rockKeepDistance, Mathf.Pow(value, 0.5f));
				vector = roomCenterPoint + num2 * Tool2D.GetDir(GetSortDir(), 5f);
			}
			if (!Tool2D.PointOnNavMesh(vector))
			{
				vector = Tool2D.GetNavMeshPointIngoreZ(roomCenterPoint, num2, GetSortDir(), 5f);
			}
			Elite12_FallRock component = MiniPool.GetGO("Prefabs/SpecialObjs/111", vector).GetComponent<Elite12_FallRock>();
			component.Initialize(isStand: false, GeneralTool.ChanceResult((!changedStage2) ? rockSpikeChance : secondStageRockSpikeChance));
			rocks.Add(component);
		}
	}

	private void JumpKnockGround()
	{
		CreateGroundWave();
		CamController.Inst.SetShock(jumpShock);
		SEMgr.Inst.monster26BigLand.PlaySE();
		JumpGroundDamage();
	}

	private void JumpGroundDamage(bool isFly = false)
	{
		Vector3 vector = ((tsf_Model.localScale.x < 0f) ? Vector3.left : Vector3.right);
		vector *= jumpHorizontalOffset;
		float radius = (isFly ? flyDropRadius : jumpDamageRadius);
		int num = (isFly ? flyDropDamage : jumpDamage);
		float num2 = (isFly ? flyDropKnockBack : jumpKnockBack);
		if (!isFly)
		{
			MiniPool.GetGO("Prefabs/EF/EF_Elite12_1_Land" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position + vector, 2f);
		}
		else
		{
			MiniPool.GetGO("Prefabs/EF/EF_Elite12_1_LandBig" + (GameMgr.IsChAge14_Static ? " H" : "") + (GameMgr.IsMobile_Static ? " M" : ""), base.transform.position + vector, 6f);
		}
		UnitDotsSyncSystem.GetCollidersInRange(Tool2D.IgnoreZPoint(base.transform.position) + vector, radius, GameConst.Filter_MonsterEffectBullet, results);
		for (int i = 0; i < results.Count; i++)
		{
			Entity entity = results[i].entity;
			_ = results[i];
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 256u:
			{
				for (int j = 0; j < rocks.Count; j++)
				{
					if (rocks[j].thisEntity == entity)
					{
						rocks[j].Die();
					}
				}
				break;
			}
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, num, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
					info.damage = num;
					info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(results[i].point, base.transform.position) * num2;
					info.teammateTakeDamageRatio = 4f;
					UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				}
				break;
			}
		}
	}

	private void DashWave()
	{
		SEMgr.Inst.elite12GroundWave.PlaySE();
		for (int i = 0; i < 2; i++)
		{
			Vector3 dir = Tool2D.GetDir(dashDirection, (i == 0) ? (-90) : 90);
			MiniPool.GetGO("Prefabs/EF/EF_Elite12_GroundWave" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position + dir.normalized).GetComponent<Elite12_GroundWave>().Initialize(dir, secondStageDashWaveSpeed);
		}
	}

	private void CreateGroundWave()
	{
		Vector3 vector = ((tsf_Model.localScale.x < 0f) ? Vector3.left : Vector3.right);
		vector *= jumpHorizontalOffset;
		Vector3 normalized = jumpForce.normalized;
		CamController.Inst.SetShock(groundWaveShock);
		if (changedStage2)
		{
			for (int i = 0; (float)i < secondStageGroundWaveCount; i++)
			{
				Vector3 dir = Tool2D.GetDir(normalized, (float)(i * 360) / secondStageGroundWaveCount);
				MiniPool.GetGO("Prefabs/EF/EF_Elite12_GroundWave" + (GameMgr.IsChAge14_Static ? " H" : ""), vector + base.transform.position + dir.normalized).GetComponent<Elite12_GroundWave>().Initialize(dir, groundWaveSpeed);
			}
		}
		else
		{
			for (int j = 0; (float)j < groundWaveCount; j++)
			{
				Vector3 dir2 = Tool2D.GetDir(normalized, (float)j * groundWaveAngle / (groundWaveCount - 1f) - groundWaveAngle / 2f);
				MiniPool.GetGO("Prefabs/EF/EF_Elite12_GroundWave" + (GameMgr.IsChAge14_Static ? " H" : ""), vector + base.transform.position + dir2.normalized).GetComponent<Elite12_GroundWave>().Initialize(dir2, groundWaveSpeed);
			}
		}
	}

	private void FlyDropGround()
	{
		SEMgr.Inst.monster26BigLand.PlaySE();
		SEMgr.Inst.elite12FallRock.PlaySE();
		CamController.Inst.SetShock(knockWallShock);
		StartCoroutine(SetAfterShock());
		GetFallRock(knockGroundRockCount.RandomResult());
		JumpGroundDamage(isFly: true);
		dropParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position);
		dropParticle.Play();
	}

	private void KnockGround()
	{
		SEMgr.Inst.monster26BigLand.PlaySE();
		SEMgr.Inst.elite12FallRock.PlaySE();
		CamController.Inst.SetShock(knockWallShock);
		GetFallRock(knockGroundRockCount.RandomResult());
		JumpGroundDamage();
	}

	public void KillAllPillar()
	{
		for (int i = 0; i < rocks.Count; i++)
		{
			if (rocks[i] != null)
			{
				rocks[i].Die();
			}
		}
	}

	protected override void BossDeadStay()
	{
		SEMgr.Inst.elite12_1Die.PlaySE();
		base.Anima.Play("Die");
		base.SAnima.AnimationState.SetAnimation(0, "Stage2Change", loop: false);
		base.SAnima.Update(0.15f);
		base.SAnima.timeScale = 0f;
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
		shoutParticle.Stop();
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (farMate.myPpt.AlreadyDead)
		{
			KillAllPillar();
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "ChangeStageFinish":
			state = MonsterState.Idle;
			break;
		case "Jump":
			state = MonsterState.JumpFly;
			break;
		case "JumpOnGround":
			state = MonsterState.JumpOnGround;
			break;
		case "JumpFinish":
			secondStageJumpCounter = 0;
			state = MonsterState.Follow;
			break;
		case "DashAimStop":
			stopAim = true;
			break;
		case "DashChargeFinish":
			state = MonsterState.Dash;
			break;
		case "FlyStart":
		{
			base.CC_Self.enabled = false;
			SetDotsCCEnable(isOpen: false);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = false;
			componentData.InvincibleRegister();
			SetComponentData(componentData);
			SEMgr.Inst.elite12Fly.PlaySE();
			SEMgr.Inst.elite12FlyJump.PlaySE();
			jumpParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position);
			jumpParticle.Play();
			break;
		}
		case "FlyFinish":
			state = MonsterState.Drop;
			break;
		case "Drop":
			state = MonsterState.DropRecover;
			break;
		case "DropFinish":
			state = MonsterState.Follow;
			break;
		case "DashShout":
			SEMgr.Inst.elite12_1Roar1.PlaySE();
			break;
		case "DashAfterFinish":
			secondStageDashCounter = 0;
			state = MonsterState.Idle;
			break;
		case "SwitchStageShock":
			shoutParticle.Play();
			CamController.Inst.SetShock(switchStageShock);
			break;
		case "ShoutStop":
			shoutParticle.Stop();
			break;
		case "SwitchStageShout":
			SEMgr.Inst.elite12_1SwitchStage.PlaySE();
			break;
		case "Stage2ChangeFinish":
			state = MonsterState.Follow;
			break;
		case "KnockGround":
			KnockGround();
			break;
		case "KnockGroundFinish":
			state = MonsterState.Follow;
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}
}
