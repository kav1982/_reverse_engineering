using System.Collections.Generic;
using Unity.Physics;
using UnityEngine;

public class Elite12_2 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Move,
		MeteorBefore,
		Meteor,
		MeteorAfter,
		MeteorLine,
		MeteorLineContinue,
		MeteorLineAfter,
		ChangeStage,
		Teleport,
		TeleportAfter,
		BulletBefore,
		BulletContinue,
		BulletAfter,
		Drone,
		DroneContinue,
		DroneAfter
	}

	[Header("状态")]
	[Space(50f)]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("行动")]
	public VariableFloat keepDistance;

	public VariableFloat walkRepositionTime;

	public VariableFloat idleTime;

	public VariableFloat walkTime;

	public VariableFloat walkRadius;

	public VariableFloat actionInterval;

	public VariableFloat secondStageActionInterval;

	private float actionIntervalTimer;

	[Header("陨石")]
	public float meteorFlyTime;

	public VariableFloat meteorRadius;

	public VariableFloat meteorRadiusNoTarget;

	public VariableFloat meteorShootInterval;

	public VariableInt meteorShootCount;

	public VariableFloat secondStageMeteorShootInterval;

	public VariableInt secondStageMeteorShootCount;

	public float meteoriteHeight;

	public float action1AfterTime;

	public VariableFloat predictTime;

	public Transform tsf_FirePoint;

	public ParticleSystem shootParticle;

	public ParticleSystem shootParticle_H;

	[Header("一条陨石")]
	public int meteorLineCount;

	public int secondStageMeteorLineCount;

	public float meteorLineAngle;

	public float meteorLineMinAngle;

	public float meteorLineMinDistance;

	public float meteorLineMaxDistance;

	public float secondStageMeteorLineAngle;

	public VariableInt multiMeteorLineCount;

	public VariableInt secondStageMultiMeteorLineCount;

	public List<Elite12_LineMeteor> processingLines = new List<Elite12_LineMeteor>();

	private int meteorLineCounter;

	[Header("圆圈冲击波")]
	public float CircleBulletCount;

	public float CircleBulletSpeed;

	public float CircleBulletDamage;

	public float CirclelBulletLifeTime;

	[Header("二阶段防御无人机")]
	public Transform tsf_DronePoint;

	public float droneHeight;

	public float droneLaunchCD;

	public float droneTime;

	[Header("传送躲避")]
	public VariableFloat teleportDistance;

	public float teleportChance;

	public float teleportStartDistance;

	public ParticleSystem teleportParticle;

	public float teleportAngleRange;

	[Header("二阶段")]
	public ParticleSystem portalParticle;

	public ParticleSystem portalParticle_H;

	public ParticleSystem shoutParticle;

	public ShockParam switchStageShock;

	public bool changedStage2;

	public static Elite12_2 Inst;

	public Elite12_1 closeMate;

	[Header("技能概率")]
	public float meteorChance;

	public float lineMeteorChance;

	public float droneChance;

	[Header("其他")]
	public Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	private Vector3 nowFaceDir;

	private List<MonsterState> skills = new List<MonsterState>
	{
		MonsterState.MeteorBefore,
		MonsterState.MeteorLine,
		MonsterState.Drone
	};

	private MonsterState lastSkill;

	private MonsterState lastNoTeleportSkill;

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
			meteorShootInterval.value1 *= 1.1f;
			meteorShootInterval.value2 *= 1.1f;
			meteorShootCount.value1 = Mathf.CeilToInt((float)meteorShootCount.value1 * 0.9f);
			meteorShootCount.value2 = Mathf.CeilToInt((float)meteorShootCount.value2 * 0.9f);
			secondStageMeteorShootInterval.value1 *= 1.15f;
			secondStageMeteorShootInterval.value2 *= 1.15f;
			secondStageMeteorShootCount.value1 = Mathf.CeilToInt((float)secondStageMeteorShootCount.value1 * 0.9f);
			secondStageMeteorShootCount.value2 = Mathf.CeilToInt((float)secondStageMeteorShootCount.value2 * 0.9f);
		}
	}

	public override void EveryInitialCallback()
	{
		actionInterval.RandomResult();
		Inst = this;
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.width;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height;
		if (Elite12_1.Inst != null && Elite12_1.Inst.farMate == null)
		{
			Elite12_1.Inst.farMate = this;
			Inst.closeMate = Elite12_1.Inst;
		}
		if (GameMgr.IsHarmony_Static)
		{
			portalParticle = portalParticle_H;
			shootParticle = shootParticle_H;
		}
		if (GameMgr.IsHarmony_Static)
		{
			base.SAnima.initialSkinName += "_HX";
			base.SAnima.Initialize(overwrite: true);
		}
	}

	public override void Frame1InitialCallback()
	{
	}

	private void CheckAction()
	{
		if (!changedStage2 && !closeMate.gameObject.activeSelf)
		{
			actionInterval = secondStageActionInterval;
			state = MonsterState.ChangeStage;
			changedStage2 = true;
			return;
		}
		actionIntervalTimer += Time.deltaTime;
		if (!(actionIntervalTimer >= actionInterval.result))
		{
			return;
		}
		GetNearestTargetPlayerFirst();
		if (changedStage2 && ((base.HaveTarget && ToTargetDistance() < teleportStartDistance) || !base.HaveTarget) && lastSkill != MonsterState.Teleport)
		{
			if (GeneralTool.ChanceResult(teleportChance))
			{
				state = MonsterState.Teleport;
			}
			lastSkill = state;
			return;
		}
		actionIntervalTimer = 0f;
		actionInterval.RandomResult();
		int weightRandom = GeneralTool.GetWeightRandom(1f, 1f, changedStage2 ? 1 : 0);
		if (changedStage2)
		{
			while (lastNoTeleportSkill == skills[weightRandom])
			{
				weightRandom = GeneralTool.GetWeightRandom(meteorChance, lineMeteorChance, changedStage2 ? droneChance : 0f);
			}
		}
		else
		{
			while (lastSkill == skills[weightRandom])
			{
				weightRandom = GeneralTool.GetWeightRandom(meteorChance, lineMeteorChance, changedStage2 ? droneChance : 0f);
			}
		}
		state = skills[weightRandom];
		lastSkill = state;
		lastNoTeleportSkill = state;
	}

	public override void Update()
	{
		for (int num = processingLines.Count - 1; num >= 0; num--)
		{
			if (!processingLines[num].gameObject.activeSelf)
			{
				processingLines.RemoveAt(num);
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
			if (stateExistTime >= 0.5f)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
				idleTime.RandomResult();
			}
			CheckAction();
			SetMove(Vector3.zero);
			if (stateExistTime >= idleTime.result)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				base.Anima.Play("Walk");
				base.SAnima.AnimationState.SetAnimation(0, "Move", loop: true);
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, walkRadius));
				walkTime.RandomResult();
			}
			CheckAction();
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, walkRadius));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			if (stateExistTime >= walkTime.result)
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.Move:
		{
			ref bool reference4 = ref varMgr.RegBool(0);
			ref float reference5 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				walkRepositionTime.RandomResult();
				keepDistance.RandomResult();
				base.Anima.Play("Walk");
				base.SAnima.AnimationState.SetAnimation(0, "Move", loop: true);
				walkTime.RandomResult();
				if (base.HaveTarget)
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint, keepDistance));
				}
				else
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, keepDistance));
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
			CheckAction();
			if ((ToTargetDistanceSqr() < keepDistance.value1 * keepDistance.value1 || ToTargetDistanceSqr() > keepDistance.value2 * keepDistance.value2) && !reference4)
			{
				reference4 = true;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint, keepDistance));
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				break;
			}
			reference5 += Time.deltaTime;
			if (navInfo.allCornerArrived || reference5 > walkRepositionTime.result)
			{
				walkRepositionTime.RandomResult();
				reference5 = 0f;
				reference4 = false;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint, keepDistance, -ToTargetDir(), 60f));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			break;
		}
		case MonsterState.MeteorBefore:
			if (changedState)
			{
				base.Anima.Play("Meteor");
				base.SAnima.AnimationState.SetAnimation(0, "BulletBefore", loop: false);
				GetNearestTargetPlayerFirst();
				nowFaceDir = ToPointDir(roomCenterPoint);
			}
			SetMove(Vector3.zero, isFlip: false);
			if (base.HaveTarget)
			{
				nowFaceDir = ToTargetDir();
			}
			SetFlip(nowFaceDir.x);
			break;
		case MonsterState.Meteor:
		{
			ref float reference = ref varMgr.RegFloat(0);
			ref int reference2 = ref varMgr.RegInt(0);
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "Bullet", loop: true);
				SEMgr.Inst.elite12_2Roar1.PlaySE();
				if (changedStage2)
				{
					meteorShootInterval = secondStageMeteorShootInterval;
					meteorShootCount = secondStageMeteorShootCount;
				}
				meteorShootInterval.RandomResult();
				meteorShootCount.RandomResult();
			}
			if (base.HaveTarget)
			{
				nowFaceDir = ToTargetDir();
			}
			SetFlip(nowFaceDir.x);
			SetMove(Vector3.zero, isFlip: false);
			reference += Time.deltaTime;
			if (!(reference >= meteorShootInterval.result))
			{
				break;
			}
			reference = 0f;
			meteorShootInterval.RandomResult();
			GetNearestTargetPlayerFirst();
			if (base.HaveTarget)
			{
				Vector3 targetPoint = base.TargetPoint;
				predictTime.RandomResult();
				if (GetComponentData<UnitProperty_Dots>(targetEntity).unitCfg.unitType == UnitType.Player)
				{
					targetPoint += PlayerMgr.Inst.PlayerCtrller.CurrentMotion * predictTime.result;
				}
				ShootSingleMeteor(Tool2D.GetNavMeshPointIngoreZ(targetPoint, meteorRadius, 8));
			}
			else
			{
				ShootSingleMeteor(Tool2D.GetNavMeshPointIngoreZ(PlayerMgr.Inst.PlayerPointIgnoreZ, meteorRadiusNoTarget, 8));
			}
			reference2++;
			if (reference2 >= meteorShootCount.result)
			{
				state = MonsterState.MeteorAfter;
			}
			break;
		}
		case MonsterState.MeteorAfter:
			if (changedState)
			{
				base.Anima.Play("Idle");
				base.SAnima.AnimationState.SetAnimation(0, "BulletAfter", loop: false);
			}
			SetMove(Vector3.zero, isFlip: false);
			if (stateExistTime > action1AfterTime)
			{
				state = MonsterState.Move;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, walkRadius));
			}
			break;
		case MonsterState.MeteorLine:
			if (changedState)
			{
				base.Anima.Play("MeteorLine", 0, 0f);
				base.SAnima.AnimationState.SetAnimation(0, "BulletBefore", loop: false);
				SEMgr.Inst.elite12_2Roar2.PlaySE();
				if (changedStage2)
				{
					meteorLineCount = secondStageMeteorLineCount;
					meteorLineAngle = secondStageMeteorLineAngle;
					multiMeteorLineCount = secondStageMultiMeteorLineCount;
				}
				multiMeteorLineCount.RandomResult();
				nowFaceDir = Tool2D.GetDir(ToPointDir(roomCenterPoint), Random.Range(-90, 90));
			}
			SetMove(Vector3.zero);
			if (base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			break;
		case MonsterState.MeteorLineContinue:
			if (changedState)
			{
				base.Anima.Play("MeteorLineContinue");
				base.SAnima.AnimationState.SetAnimation(0, "Bullet", loop: true);
				CreateMeteorLine();
			}
			if (stateExistTime > 1.5f && meteorLineCounter < meteorLineCount)
			{
				stateExistTime = 0f;
				meteorLineCounter++;
				if (meteorLineCounter < meteorLineCount)
				{
					nowFaceDir = Tool2D.GetDir(ToPointDir(roomCenterPoint), Random.Range(-90, 90));
					CreateMeteorLine();
				}
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
				nowFaceDir = ToTargetDir();
			}
			else
			{
				SetFlip(nowFaceDir.x);
			}
			if (processingLines.Count <= 0 && meteorLineCounter >= meteorLineCount)
			{
				meteorLineCounter = 0;
				state = MonsterState.MeteorLineAfter;
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.MeteorLineAfter:
			if (changedState)
			{
				base.Anima.Play("MeteorLineAfter");
				base.SAnima.AnimationState.SetAnimation(0, "BulletAfter", loop: false);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Teleport:
		{
			if (changedState)
			{
				teleportParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position);
				teleportParticle.Play();
				base.Anima.Play("Teleport");
				base.SAnima.AnimationState.SetAnimation(0, "Teleport", loop: false);
			}
			PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
			componentData.Linear = Vector3.zero;
			SetComponentData(componentData);
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
		case MonsterState.Drone:
			if (changedState)
			{
				SEMgr.Inst.elite12_2Roar3.PlaySE().pitch = Random.Range(0.9f, 1.1f);
				base.Anima.Play("Drone");
				base.SAnima.AnimationState.SetAnimation(0, "DroneBefore", loop: false);
				GetNearestTarget();
				nowFaceDir = Tool2D.GetDir(ToPointDir(roomCenterPoint), Random.Range(-45, 45));
				SetFlip(nowFaceDir.x);
			}
			SetMove(Vector3.zero, isFlip: false);
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDelta().x);
				nowFaceDir = ToTargetDir();
			}
			break;
		case MonsterState.DroneContinue:
		{
			ref float reference3 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				base.Anima.Play("DroneContinue");
				base.SAnima.AnimationState.SetAnimation(0, "Drone", loop: true);
				GetNearestTarget();
			}
			reference3 += Time.deltaTime;
			if (reference3 > droneLaunchCD)
			{
				reference3 -= droneLaunchCD;
				ShootDrone(nowFaceDir);
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDelta().x);
				nowFaceDir = ToTargetDir();
			}
			if (stateExistTime > droneTime)
			{
				state = MonsterState.DroneAfter;
			}
			SetMove(Vector3.zero);
			break;
		}
		case MonsterState.DroneAfter:
			if (changedState)
			{
				base.Anima.Play("DroneAfter");
				base.SAnima.AnimationState.SetAnimation(0, "DroneAfter", loop: false);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.ChangeStage:
			if (changedState)
			{
				base.Anima.Play("ChangeStage");
				base.SAnima.AnimationState.SetAnimation(0, "Stage2Change", loop: false);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.TeleportAfter:
		case MonsterState.BulletBefore:
		case MonsterState.BulletContinue:
		case MonsterState.BulletAfter:
			break;
		}
	}

	private void ShootDrone(Vector3 basicAimDir)
	{
		Elite12_Drone component = Elite12_1.MiniPool.GetGO("Prefabs/EF/EF_Elite12_Drone" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position + Vector3.right * (tsf_DronePoint.position.x - base.transform.position.x) * ((myPpt.MR_Models[0].transform.localScale.x > 0f) ? 1 : (-1))).GetComponent<Elite12_Drone>();
		GetNearestTarget();
		component.Initialize(this, targetEntity, Tool2D.GetDir(basicAimDir, Random.Range(-45, 45)));
	}

	public void ShootSingleMeteor(Vector3 pos)
	{
		SEMgr.Inst.elite15Action2SHoot.PlaySE(SEPlayMode.Replay, 3, 0.2f);
		Vector3 vector = new Vector3((tsf_FirePoint.position.x - base.transform.position.x) * (float)((myPpt.MR_Models[0].transform.localScale.x > 0f) ? 1 : (-1)) + base.transform.position.x, base.transform.position.y, 0f - (tsf_FirePoint.position.y - base.transform.position.y));
		Elite12_1.MiniPool.GetGO("Prefabs/EF/EF_Elite12_Meteorite" + (GameMgr.IsHarmony_Static ? " H" : ""), vector).GetComponent<Elite12_Meteorite>().Initialize(Elite12_1.MiniPool, vector, pos, meteorFlyTime);
		shootParticle.transform.position = Tool2D.GetLayerPoint(vector);
		shootParticle.Play();
	}

	public void CreateMeteorLine()
	{
		multiMeteorLineCount.RandomResult();
		Vector3 oldDir = nowFaceDir;
		GetNearestTarget();
		if (base.HaveTarget)
		{
			predictTime.RandomResult();
			Vector3 targetPoint = base.TargetPoint;
			if (GetComponentData<UnitProperty_Dots>(targetEntity).unitCfg.unitType == UnitType.Player)
			{
				targetPoint += PlayerMgr.Inst.PlayerCtrller.CurrentMotion * predictTime.result;
			}
			oldDir = ToPointDir(targetPoint);
		}
		for (int i = 0; i < multiMeteorLineCount.result; i++)
		{
			float num = Random.Range(-0.5f, 0.5f);
			if (!changedStage2)
			{
				Tool2D.GetDir(oldDir, 0.2f * meteorLineAngle);
				num = Random.Range(-0.1f, -0.1f);
			}
			float num2 = meteorLineAngle;
			if (!changedStage2 && base.HaveTarget)
			{
				num2 = Mathf.Lerp(meteorLineAngle, meteorLineMinAngle, (ToTargetDistance() - meteorLineMinDistance) / (meteorLineMaxDistance - meteorLineMinDistance));
			}
			Vector3 dir = Tool2D.GetDir(oldDir, ((float)i + num) * num2 / (float)(multiMeteorLineCount.result - 1) - num2 / 2f);
			Elite12_LineMeteor component = Elite12_1.MiniPool.GetGO("Prefabs/EF/EF_Elite12_LineMeteor", base.transform.position).GetComponent<Elite12_LineMeteor>();
			component.Initialize(base.transform.position, dir);
			processingLines.Add(component);
		}
	}

	protected override void BossDeadStay()
	{
		base.BossDeadStay();
		SEMgr.Inst.elite12_2Die.PlaySE();
		base.Anima.Play("Die");
		base.SAnima.AnimationState.SetAnimation(0, "BulletAfter", loop: false);
		base.SAnima.Update(0.6f);
		base.SAnima.timeScale = 0f;
		base.enabled = false;
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: true);
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (closeMate.myPpt.AlreadyDead)
		{
			closeMate.KillAllPillar();
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "MeteorBeforeFinish":
			state = MonsterState.Meteor;
			break;
		case "MeteorLine":
			state = MonsterState.MeteorLineContinue;
			break;
		case "MeteorLineFinish":
			state = MonsterState.Move;
			break;
		case "Teleport":
		{
			SEMgr.Inst.monster51_Teleport.PlaySE();
			GetNearestTargetPlayerFirst();
			Vector3 startPoint = roomCenterPoint + new Vector3((Random.value - 0.5f) * roomWidth, (Random.value - 0.5f) * roomHeight, 0f);
			if (base.HaveTarget)
			{
				startPoint = Tool2D.GetNavMeshPointIngoreZ(base.TargetPoint, teleportDistance, ToTargetDir(), teleportAngleRange);
			}
			startPoint = Tool2D.GetNavMeshPointIngoreZ(startPoint);
			for (int i = 0; i < 2; i++)
			{
				Elite12_1.MiniPool.GetGO("Prefabs/EF/EF_Elite12_Teleport" + (GameMgr.IsHarmony_Static ? " H" : ""), (i == 0) ? base.transform.position : startPoint, 3f);
			}
			base.transform.position = startPoint;
			SyncDotsPosition();
			break;
		}
		case "TeleportFinish":
			CheckAction();
			break;
		case "ChangeStageShout":
			shoutParticle.Play();
			portalParticle.Play();
			CamController.Inst.SetShock(switchStageShock);
			SEMgr.Inst.elite12_2SwitchStage.PlaySE();
			break;
		case "ShoutStop":
			portalParticle.Stop();
			shoutParticle.Stop();
			break;
		case "ChangeStageFinish":
			state = MonsterState.Move;
			break;
		case "BulletStart":
			state = MonsterState.BulletContinue;
			break;
		case "BulletFinish":
			state = MonsterState.Move;
			break;
		case "DroneStart":
			state = MonsterState.DroneContinue;
			break;
		case "DroneFinish":
			state = MonsterState.Move;
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}
}
