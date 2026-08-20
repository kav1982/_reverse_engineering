using System;
using System.Collections.Generic;
using UnityEngine;

public class Boss54 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Move,
		Command,
		Directional,
		LaserCharge,
		Laser,
		LaserAfter,
		StraightBullet,
		Teleport,
		TeleportAfter,
		TrackBullet,
		DashPrepare,
		Dash,
		DashAfter,
		Dead
	}

	public StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("行动")]
	public VariableFloat IdleTime;

	public VariableFloat randomMoveRadius;

	public VariableFloat attackCD;

	public VariableFloat keepDistance;

	public VariableFloat normalSkillDuration;

	private bool inNormalSkill;

	private float normalSkillTimer;

	public float DirectionalChance;

	public float LaserChance;

	public float StraightChance;

	public float TrackChance;

	public float DashChance;

	[Header("二阶段")]
	public bool secondStageOverride;

	private float attackCDTimer;

	private MonsterState lastSkill;

	private MonsterState lastSingleState;

	private MonsterState nextSingleState;

	[Header("召唤")]
	public int maxSummonCount;

	public int minSummonCount;

	public int dashMinSummonCount;

	public VariableFloat singleSummonCount;

	public List<Boss54_Child> childList;

	public VariableFloat summonKeepDistance;

	[Header("命令")]
	public ParticleSystem commandParticle;

	[Header("测试")]
	public bool isTestingSingleSkill;

	public MonsterState testSingleSkill;

	[Header("传送")]
	public VariableFloat teleportKeepDistance;

	public float normalSkillTeleportChance;

	public float teleportAngleRange;

	[Header("多向攻击")]
	public float directionalBulletCount;

	public float directionalBulletSpeedDecrease;

	public float directionalBulletSpeed;

	public ParticleSystem bulletParticle;

	[Header("激光攻击")]
	public int laserCount;

	public float laserPredictTime;

	public float laserOffsetAngleRange;

	public ShockParam laserShockParam;

	private List<Boss54_Line> lasers = new List<Boss54_Line>();

	[Header("连续子弹攻击")]
	public float straightBulletWaveCount;

	public int straightBulletCount;

	public float straightBulletInterval;

	public float straightBulletSpeed;

	public float straightBulletOffset;

	public VariableInt blockSpellCount;

	private bool straightBulletShooting;

	private List<Vector3> straightBulletDirections = new List<Vector3>();

	[Header("创人")]
	public float dashSpeed;

	public float dashTime;

	public float dashRedirectDistance;

	public float dashRedirectAngleRange;

	public float dashRedirectPredictTime;

	public float dashWarningLineLength;

	public float dashBlockBulletInterval;

	public ParticleSystem dashParticle;

	public Boss54_DamageZone damageZone;

	public LineRenderer warningLine;

	private Vector3 dashDir;

	private float dashBlockBulletTimer;

	[Header("音效")]
	public AudioSource AS_Dash;

	public static Boss54 Inst;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

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

	public bool isSecondStage
	{
		get
		{
			UnitConfig unitCfg = GetComponentData<UnitProperty_Dots>(myPpt.myEntity).unitCfg;
			if (!secondStageOverride)
			{
				return unitCfg.currentHP / unitCfg.maxHP < 0.5f;
			}
			return true;
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
		AS_Dash.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
	}

	public override void EveryInitialCallback()
	{
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height;
		normalSkillDuration.RandomResult();
		Inst = this;
	}

	private Vector3 PointWithinRange(Vector3 startPoint)
	{
		return Tool2D.PointWithinRange(startPoint, roomCenterPoint, roomWidth - 3f, roomHeight - 3f);
	}

	private Vector3 GetRandomMovePoint()
	{
		GetNearestTargetPlayerFirst();
		if (base.HaveTarget)
		{
			return PointWithinRange(Tool2D.GetNavMeshPointIngoreZ(base.TargetPoint, keepDistance, -ToTargetDir(), 60f));
		}
		randomMoveRadius.RandomResult();
		return PointWithinRange(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
	}

	private void KeepMoving()
	{
		if (navInfo.allCornerArrived)
		{
			stateExistTime = 0f;
			GetNavInfo(GetRandomMovePoint());
		}
		else
		{
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			CheckNavInfo();
		}
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
		if (inNormalSkill)
		{
			normalSkillTimer += Time.deltaTime;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				GetNearestTargetPlayerFirst();
			}
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 0.5f)
			{
				state = MonsterState.RandomMove;
			}
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				base.Anima.Play("Move");
				GetNavInfo(GetRandomMovePoint());
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				GetNearestTargetPlayerFirst();
			}
			KeepMoving();
			TryAttack();
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			TryAttack();
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Move:
			if (changedState)
			{
				base.Anima.Play("Move");
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget(checkWall: true);
			}
			if (base.HaveTarget)
			{
				state = MonsterState.Idle;
			}
			else if (navInfo.allCornerArrived)
			{
				stateExistTime = 0f;
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			break;
		case MonsterState.Command:
			if (changedState)
			{
				base.Anima.Play("Command");
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Directional:
			if (changedState)
			{
				base.Anima.Play("Directional");
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.LaserCharge:
			if (changedState)
			{
				base.Anima.Play("LaserCharge");
			}
			SetMove(Vector3.zero, isFlip: false);
			SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPoint).x);
			break;
		case MonsterState.Laser:
			if (changedState)
			{
				base.Anima.Play("Laser");
				for (int i = 0; i < lasers.Count; i++)
				{
					lasers[i].SetWarningFinish();
				}
				SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPoint).x);
				CamController.Inst.SetShock(laserShockParam);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.LaserAfter:
			if (changedState)
			{
				base.Anima.Play("LaserAfter");
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.StraightBullet:
		{
			ref float reference = ref varMgr.RegFloat(0);
			ref int reference2 = ref varMgr.RegInt(0);
			if (changedState)
			{
				base.Anima.Play("Straight");
				reference2 = 0;
				straightBulletShooting = false;
				straightBulletDirections.Clear();
				float num = straightBulletOffset / straightBulletWaveCount;
				for (int j = 0; (float)j < straightBulletWaveCount; j++)
				{
					Vector3 dir = Tool2D.GetDir(ToPointDir(PlayerMgr.Inst.PlayerPoint), num * ((0f - straightBulletWaveCount) / 2f + (float)j + UnityEngine.Random.value - 0.5f));
					straightBulletDirections.Add(dir);
				}
				SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPoint).x);
				reference = straightBulletInterval;
			}
			if (straightBulletShooting)
			{
				reference += Time.deltaTime;
			}
			if (reference > straightBulletInterval && reference2 < straightBulletCount)
			{
				reference2++;
				reference -= straightBulletInterval;
				SEMgr.Inst.boss54Straight.PlaySE();
				bulletParticle.Play();
				for (int k = 0; k < straightBulletDirections.Count; k++)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_BigBullet", base.transform.position).GetComponent<Boss54Bullet>().Initialize(straightBulletDirections[k], straightBulletSpeed, myPpt.myEntity);
				}
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
		case MonsterState.TrackBullet:
			if (changedState)
			{
				base.Anima.Play("Track");
				SetFlip(ToPointDir(PlayerMgr.Inst.PlayerPoint).x);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Teleport:
			if (changedState)
			{
				base.Anima.Play("Teleport");
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.DashPrepare:
			if (changedState)
			{
				base.Anima.Play("DashCharge");
				GetNearestTargetPlayerFirst();
				dashDir = (base.HaveTarget ? ToTargetDir() : Tool2D.GetDir());
				warningLine.enabled = true;
				warningLine.positionCount = 10;
				SyncChildDashPrepare();
			}
			GetNearestTargetPlayerFirst();
			if (base.HaveTarget)
			{
				dashDir = ToTargetDir();
			}
			SetFlip(dashDir.x);
			SetDashWarningLine();
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Dash:
			if (changedState)
			{
				AS_Dash.Play();
				base.Anima.Play("Dash");
				warningLine.enabled = true;
				damageZone.Open();
				SetDashVelocity(dashDir);
				dashBlockBulletTimer = 0f;
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.CanTouch = false;
				componentData2.IsVelocityDeclice = false;
				componentData2.ImmuneKnockbackRegister();
				SetComponentData(componentData2);
				dashParticle.Play();
			}
			CheckDashOutOfRoom();
			KeepDashVelocity();
			SetFlip(dashDir.x);
			SetDashWarningLine();
			dashBlockBulletTimer += Time.deltaTime;
			if (dashBlockBulletTimer > dashBlockBulletInterval)
			{
				dashBlockBulletTimer -= dashBlockBulletInterval;
				ShootDashBlockBullet();
			}
			if (stateExistTime > dashTime)
			{
				state = MonsterState.DashAfter;
				SyncChildDashAfter();
			}
			break;
		case MonsterState.DashAfter:
			if (changedState)
			{
				AS_Dash.Stop();
				dashParticle.Stop();
				base.Anima.Play("DashAfter");
				warningLine.enabled = false;
				damageZone.Close();
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanTouch = true;
				componentData.IsVelocityDeclice = true;
				componentData.ImmuneKnockbackUnregister();
				SetComponentData(componentData);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Dead:
			if (changedState)
			{
				base.Anima.Play("Dead");
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.TeleportAfter:
			break;
		}
	}

	private void CheckChildIsAlive()
	{
		for (int num = childList.Count - 1; num >= 0; num--)
		{
			if (childList[num] == null || childList[num].myPpt.AlreadyDead)
			{
				childList.RemoveAt(num);
			}
		}
	}

	private void TryAttack()
	{
		attackCDTimer += Time.deltaTime;
		if (attackCDTimer > attackCD.result)
		{
			CheckChildIsAlive();
			attackCDTimer = 0f;
			attackCD.RandomResult();
			MonsterState monsterState = MonsterState.Idle;
			if ((childList.Count < minSummonCount || (inNormalSkill && normalSkillTimer > normalSkillDuration.result)) && lastSkill != MonsterState.Command && lastSkill != MonsterState.Teleport)
			{
				monsterState = MonsterState.Command;
				ClearChildSkill();
				normalSkillDuration.RandomResult();
				inNormalSkill = false;
				normalSkillTimer = 0f;
			}
			else if (lastSkill == MonsterState.Teleport || (lastSkill != MonsterState.Command && !GeneralTool.ChanceResult(normalSkillTeleportChance)))
			{
				monsterState = nextSingleState;
				ChildAttack();
			}
			else
			{
				monsterState = MonsterState.Teleport;
			}
			state = monsterState;
			lastSkill = monsterState;
		}
	}

	public void LaserFinish()
	{
		if (!myPpt.AlreadyDead && state == MonsterState.Laser)
		{
			state = MonsterState.LaserAfter;
		}
	}

	private void Summon()
	{
		CheckChildIsAlive();
		singleSummonCount.RandomResult();
		int num = (int)Mathf.Max(((nextSingleState == MonsterState.DashPrepare) ? dashMinSummonCount : minSummonCount) + 1 - childList.Count, singleSummonCount.result);
		if (childList.Count < num)
		{
			num += (int)singleSummonCount.result;
		}
		num = Mathf.Min(num, Mathf.Max(0, maxSummonCount - childList.Count - num));
		if (num > 0)
		{
			float num2 = 360f / (float)num;
			for (int i = 0; i < num; i++)
			{
				Vector3 dir = Tool2D.GetDir(num2 * ((float)(-num) / 2f + (float)i + UnityEngine.Random.value - 0.5f));
				Vector3 point = base.transform.position + dir * summonKeepDistance.RandomResult();
				Boss54_Child component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/505421", point).GetComponent<Boss54_Child>();
				childList.Add(component);
				SpecialObj301EndlessMonsterSpawner.Inst.SetPptFix(component.myPpt);
				component.Initialize();
			}
		}
	}

	private void DirectionalAttack()
	{
		bulletParticle.Play();
		for (int i = 0; (float)i < directionalBulletCount; i++)
		{
			float num = 360f / directionalBulletCount;
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_BigBullet", base.transform.position).GetComponent<Boss54Bullet>().Initialize(Tool2D.GetDir(num * (float)i), directionalBulletSpeed, myPpt.myEntity);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_BigBullet", base.transform.position).GetComponent<Boss54Bullet>().Initialize(Tool2D.GetDir(num * ((float)i + 0.5f)), directionalBulletSpeed - directionalBulletSpeedDecrease, myPpt.myEntity);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_BigBullet", base.transform.position).GetComponent<Boss54Bullet>().Initialize(Tool2D.GetDir(num * (float)i), directionalBulletSpeed - directionalBulletSpeedDecrease * 2f, myPpt.myEntity);
		}
	}

	private void ShootBlockBullet()
	{
		blockSpellCount.RandomResult();
		for (int i = 0; i < blockSpellCount.result; i++)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_BigBlockBullet", base.transform.position).GetComponent<Boss54Bullet>().InitializeBlock(myPpt.myEntity);
		}
	}

	private void ShootDashBlockBullet()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_BigBlockBulletSlow", base.transform.position).GetComponent<Boss54Bullet>().InitializeBlock(myPpt.myEntity);
	}

	private void CreateLaser()
	{
		lasers.Clear();
		float num = laserOffsetAngleRange / (float)laserCount;
		for (int i = 0; i < laserCount; i++)
		{
			Boss54_Line component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_BigLine", base.transform.position).GetComponent<Boss54_Line>();
			Vector3 point = PlayerMgr.Inst.PlayerPointIgnoreZ + PlayerMgr.Inst.PlayerCtrller.CurrentMotion * laserPredictTime;
			Vector3 dir = Tool2D.GetDir(ToPointDir(point), num * ((float)(-laserCount) / 2f + (float)i + UnityEngine.Random.value * 2f - 1f));
			component.Initialize(this, dir);
			lasers.Add(component);
		}
	}

	protected override void BossDeadStay()
	{
		state = MonsterState.Dead;
		damageZone.Close();
		CheckChildIsAlive();
		for (int i = 0; i < childList.Count; i++)
		{
			childList[i].myPpt.AnnouncedDeath_Dots();
		}
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
		warningLine.enabled = false;
		AS_Dash.Stop();
		dashParticle.Stop();
	}

	private void ChildAttack()
	{
		Boss54_Child.MonsterState skill = Boss54_Child.MonsterState.BornIdle;
		switch (nextSingleState)
		{
		case MonsterState.Directional:
			skill = Boss54_Child.MonsterState.Directional;
			break;
		case MonsterState.LaserCharge:
			skill = Boss54_Child.MonsterState.LaserCharge;
			break;
		case MonsterState.StraightBullet:
			skill = Boss54_Child.MonsterState.StraightBullet;
			break;
		case MonsterState.TrackBullet:
			skill = Boss54_Child.MonsterState.TrackBullet;
			break;
		}
		for (int i = 0; i < childList.Count; i++)
		{
			childList[i].SetSkill(skill);
		}
	}

	private void ClearChildSkill()
	{
		CheckChildIsAlive();
		for (int i = 0; i < childList.Count; i++)
		{
			childList[i].skillType = Boss54_Child.MonsterState.BornIdle;
		}
	}

	private void SyncChildDashPrepare()
	{
		CheckChildIsAlive();
		for (int i = 0; i < childList.Count; i++)
		{
			childList[i].SyncDashPrepare();
		}
	}

	private void SyncChildDash()
	{
		CheckChildIsAlive();
		for (int i = 0; i < childList.Count; i++)
		{
			childList[i].SyncDash();
		}
	}

	private void SyncChildDashAfter()
	{
		CheckChildIsAlive();
		for (int i = 0; i < childList.Count; i++)
		{
			childList[i].SyncDashAfter();
		}
	}

	private void SyncChildDashExit()
	{
		CheckChildIsAlive();
		for (int i = 0; i < childList.Count; i++)
		{
			childList[i].SyncDashExit();
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Attack":
			break;
		case "Summon":
			break;
		case "AttackFinish":
			state = MonsterState.RandomMove;
			break;
		case "SummonFinish":
			state = MonsterState.RandomMove;
			break;
		case "SummonSE":
			SEMgr.Inst.boss54Summon.PlaySE();
			break;
		case "Command":
			inNormalSkill = true;
			while (lastSingleState == nextSingleState)
			{
				switch (GeneralTool.GetWeightRandom(DirectionalChance, LaserChance, StraightChance, isSecondStage ? TrackChance : 0f, isSecondStage ? DashChance : 0f))
				{
				case 0:
					nextSingleState = MonsterState.Directional;
					break;
				case 1:
					nextSingleState = MonsterState.LaserCharge;
					break;
				case 2:
					nextSingleState = MonsterState.StraightBullet;
					break;
				case 3:
					nextSingleState = MonsterState.TrackBullet;
					break;
				case 4:
					nextSingleState = MonsterState.DashPrepare;
					break;
				}
			}
			if (isTestingSingleSkill)
			{
				nextSingleState = testSingleSkill;
			}
			lastSingleState = nextSingleState;
			Summon();
			commandParticle.Play();
			CheckChildIsAlive();
			attackCDTimer = attackCD.result / 2f;
			break;
		case "CommandFinish":
			attackCDTimer = attackCD.result;
			state = MonsterState.RandomMove;
			break;
		case "Directional":
			DirectionalAttack();
			break;
		case "DirectionalSE":
			SEMgr.Inst.boss54Directional.PlaySE();
			break;
		case "CreateLaser":
			CreateLaser();
			break;
		case "LaserChargeFinish":
			state = MonsterState.Laser;
			break;
		case "LaserFinish":
			state = MonsterState.LaserAfter;
			break;
		case "ShootStraightBullet":
			straightBulletShooting = true;
			ShootBlockBullet();
			break;
		case "ShootTrackBullet":
		{
			SEMgr.Inst.boss54Straight.PlaySE();
			bulletParticle.Play();
			Boss54Bullet component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_BigTrackBullet", base.transform.position).GetComponent<Boss54Bullet>();
			component.Initialize(GetTrackBulletDirection(), component.speed, myPpt.myEntity);
			break;
		}
		case "Teleport":
		{
			SEMgr.Inst.monster51_Teleport.PlaySE();
			GetNearestTargetPlayerFirst();
			Vector3 startPoint = roomCenterPoint + new Vector3((UnityEngine.Random.value - 0.5f) * roomWidth, (UnityEngine.Random.value - 0.5f) * roomHeight, 0f);
			if (base.HaveTarget)
			{
				startPoint = Tool2D.GetNavMeshPointIngoreZ(base.TargetPoint, teleportKeepDistance, ToTargetDir(), teleportAngleRange);
			}
			startPoint = Tool2D.GetNavMeshPointIngoreZ(startPoint);
			for (int i = 0; i < 2; i++)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss54_Teleport", (i == 0) ? base.transform.position : startPoint, 3f);
			}
			base.transform.position = startPoint;
			SyncDotsPosition();
			break;
		}
		case "TeleportFinish":
			attackCDTimer = attackCD.result;
			state = MonsterState.RandomMove;
			break;
		case "DashPrepareFinish":
			state = MonsterState.Dash;
			SyncChildDash();
			break;
		case "DashFinish":
			state = MonsterState.DashAfter;
			SyncChildDashAfter();
			break;
		case "DashAfterFinish":
			state = MonsterState.RandomMove;
			SyncChildDashExit();
			break;
		}
	}

	private Vector3 GetTrackBulletDirection()
	{
		Vector3 oldDir = Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position);
		float degree = ((UnityEngine.Random.value < 0.5f) ? UnityEngine.Random.Range(45f, 135f) : UnityEngine.Random.Range(225f, 315f));
		return Tool2D.GetDir(oldDir, degree).normalized;
	}

	private void SetDashVelocity(Vector3 dir)
	{
		dashDir = ((dir.sqrMagnitude > 0f) ? dir.normalized : Tool2D.GetDir());
		base.Rigid.linearVelocity = dashDir * dashSpeed;
		SyncDotsVelocity();
	}

	private void KeepDashVelocity()
	{
		float num = dashSpeed * dashSpeed;
		if (base.Rigid.linearVelocity.sqrMagnitude < num * 0.95f || base.Rigid.linearVelocity.sqrMagnitude > num * 1.05f)
		{
			SetDashVelocity((base.Rigid.linearVelocity.sqrMagnitude > 0f) ? base.Rigid.linearVelocity : dashDir);
		}
	}

	private void SetDashWarningLine()
	{
		for (int i = 0; i < warningLine.positionCount; i++)
		{
			Vector3 rootPoint = Vector3.Lerp(base.transform.position, base.transform.position + dashDir * dashWarningLineLength, (float)i / (float)(warningLine.positionCount - 1));
			warningLine.SetPosition(i, Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect));
		}
	}

	private void CheckDashOutOfRoom()
	{
		Vector3 vector = Tool2D.IgnoreZPoint(base.transform.position);
		Vector3 vector2 = Tool2D.PointWithinRange(vector, roomCenterPoint, roomWidth, roomHeight);
		if (!((vector2 - vector).sqrMagnitude < 0.0001f))
		{
			Vector3 zero = Vector3.zero;
			float num = roomWidth * 0.5f;
			float num2 = roomHeight * 0.5f;
			Vector3 vector3 = vector - roomCenterPoint;
			if (vector3.x > num)
			{
				zero += Vector3.left;
			}
			else if (vector3.x < 0f - num)
			{
				zero += Vector3.right;
			}
			if (vector3.y > num2)
			{
				zero += Vector3.down;
			}
			else if (vector3.y < 0f - num2)
			{
				zero += Vector3.up;
			}
			base.transform.position = vector2;
			SyncDotsPosition();
			GetNearestTargetPlayerFirst();
			if (base.HaveTarget && ToTargetDistance() >= dashRedirectDistance)
			{
				Vector3 point = PlayerMgr.Inst.PlayerPointIgnoreZ + PlayerMgr.Inst.PlayerCtrller.CurrentMotion * dashRedirectPredictTime;
				SetDashVelocity(Tool2D.GetDir(ToPointDir(point), UnityEngine.Random.Range(0f - dashRedirectAngleRange, dashRedirectAngleRange)));
			}
			else
			{
				SetDashVelocity(Vector3.Reflect(dashDir, zero.normalized));
			}
		}
	}
}
