using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Elite14_Stage2 : UnitBase
{
	public enum MonsterState
	{
		Hide,
		BornIdle,
		RandomMove,
		Idle,
		Move,
		SideSlashChase,
		SideSlash,
		SlashDashPrepare,
		SlashDash,
		ShadowAttackChase,
		ShadowAttackShow,
		ShadowAttack,
		ShadowAttackHide,
		Buff
	}

	[Header("状态")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("出场")]
	public Transform tsf_Model;

	public Shadow thisShadow;

	[Header("移动")]
	public float maxKeepDistance;

	public VariableFloat repositionRadius;

	public VariableFloat repositionTime;

	[Header("空闲")]
	public VariableFloat IdleTime;

	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	[Header("选技能")]
	public bool allowSkillRepeat;

	public float shadowAttackChance;

	public float slashDashChance;

	public float sideSlashChance;

	public float buffChance;

	public VariableFloat ActCD;

	private float actCDTimer;

	[Header("冲刺破碎斩")]
	public LineRenderer lr_DashWarning;

	public ParticleSystem slashParticle;

	public ParticleSystem slashParticle1;

	public VariableFloat dashSlashAngleConstraint;

	public float slashDashKeepDistance;

	public float slashDashRotateSpeed;

	public float slashDashSpeedRatio;

	public float slashDashTime;

	public float delaySlashDistanceInterval;

	public VariableInt delaySlashCount;

	public VariableFloat delaySlashDistanceRange;

	public VariableFloat delaySlashDistanceAngle;

	public float dashSlashForwardOffset;

	public ParticleSystem slashDashParticle;

	private Vector3 dashDir;

	[Header("冲刺斩创人")]
	public float dashDamage;

	public float dashKnockBack;

	[Header("近战斩击追逐")]
	public float sideSlashChaseSpeedRatio;

	public float sideSlashChaseTime;

	[Header("近战斩击")]
	public VariableFloat sideSlashDistanceRange;

	public float sideSlashIdealRange;

	public float sideSlashFromPointDistance;

	public VariableInt sideSlashWavesCount;

	public float sideSlashAngleRange;

	public VariableFloat sideSlashStartDistance;

	private float sideSlashFromLeft;

	private float sideSlashRotateRight;

	private bool sideSlashFinished;

	private bool sideSlashNeedFlip;

	private bool sideSlashLocked;

	[Header("Buff")]
	public ParticleSystem buffParticle;

	public int bornSummonCount;

	public int forceSummonCount;

	public ShockParam buffShock;

	[Header("撞墙判定")]
	public UnityEngine.Collider wallTrigger;

	private List<Entity> dashedEntities = new List<Entity>();

	private List<float> dashedTimer = new List<float>();

	[Header("瞬身斩")]
	public VariableInt shadowAttackCount;

	public float shadowAttackInterval;

	public SpriteRenderer colorChangeAnimaReciever;

	public float shadowAttackPredictTime;

	public ParticleSystem shadowHideParticle;

	public float shadowAttackDistanceRange;

	public float shadowAttackAwailableDistance;

	private Vector3 lastShadowAttackPoint;

	public ShockParam shadowAttackShowShock;

	[Header("表现")]
	public Shadow shadow;

	[Header("对象池")]
	public static MiniObjPool MiniPool;

	public static Elite14_Stage2 Inst;

	private MonsterState lastSkill;

	private List<Elite14_Shadow> shadows = new List<Elite14_Shadow>();

	private List<float> shadowsAngle = new List<float>();

	private List<float> shadowsAngleDelta = new List<float>();

	private List<int> slashMatsType = new List<int> { 0, 1, 2 };

	private bool dashSlashFromLeft;

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
		}
	}

	private void ChooseSkill()
	{
		MonsterState monsterState = RandomSkill();
		if (!allowSkillRepeat)
		{
			while ((monsterState == lastSkill && monsterState != MonsterState.SideSlashChase) || (monsterState == MonsterState.SlashDash && base.HaveTarget && ToTargetDistanceSqr() < slashDashKeepDistance * slashDashKeepDistance) || (monsterState == MonsterState.ShadowAttackHide && !base.HaveTarget))
			{
				monsterState = RandomSkill();
			}
		}
		state = monsterState;
		lastSkill = monsterState;
	}

	private MonsterState RandomSkill()
	{
		int weightRandom = GeneralTool.GetWeightRandom(shadowAttackChance, slashDashChance, sideSlashChance, buffChance);
		return (new MonsterState[4]
		{
			MonsterState.ShadowAttackChase,
			MonsterState.SlashDashPrepare,
			MonsterState.SideSlashChase,
			MonsterState.Buff
		})[weightRandom];
	}

	public override void SingleInitialCallback()
	{
		myPpt.RemoveSRFromArray(colorChangeAnimaReciever);
	}

	public override void EveryInitialCallback()
	{
		if (MiniPool == null)
		{
			MiniPool = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool"), LevelMgr.Inst.CurrentRoomT).GetComponent<MiniObjPool>();
		}
		Inst = this;
		state = MonsterState.Hide;
		tsf_Model.gameObject.SetActive(value: false);
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanBeTarget = false;
		componentData.CanTouch = false;
		componentData.showAffect = false;
		componentData.InvincibleRegister();
		SetComponentData(componentData);
		thisShadow.Hide();
		GameUISingletonMono<UIBossHP>.HideIfInited();
		if (wallTrigger != null)
		{
			wallTrigger.enabled = false;
		}
		lr_DashWarning.positionCount = 10;
		lr_DashWarning.enabled = false;
	}

	protected override void SetFlip(float motionX)
	{
		Vector3 localScale = tsf_Model.localScale;
		localScale.x = Mathf.Abs(localScale.x) * Mathf.Sign(motionX);
		tsf_Model.localScale = localScale;
	}

	public override void Update()
	{
		myPpt.Color_NormalBody = colorChangeAnimaReciever.color;
		myPpt.ChangeAlpha(colorChangeAnimaReciever.color.a);
		shadow.SetTransparency(colorChangeAnimaReciever.color.a);
		for (int num = dashedTimer.Count - 1; num >= 0; num--)
		{
			dashedTimer[num] -= Time.deltaTime;
			if (dashedTimer[num] < 0f)
			{
				dashedTimer.RemoveAt(num);
				dashedEntities.RemoveAt(num);
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
		case MonsterState.Hide:
			if (changedState)
			{
				tsf_Model.gameObject.SetActive(value: false);
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.CanBeTarget = false;
				componentData2.CanTouch = false;
				componentData2.showAffect = false;
				SetComponentData(componentData2);
				thisShadow.Hide();
			}
			if (stateExistTime > Elite14.Inst.summonDelayTime)
			{
				state = MonsterState.BornIdle;
			}
			break;
		case MonsterState.BornIdle:
			if (changedState)
			{
				tsf_Model.gameObject.SetActive(value: true);
				base.Anima.Play("Idle");
				base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				UnitProperty_Dots componentData7 = GetComponentData<UnitProperty_Dots>();
				componentData7.CanBeTarget = true;
				componentData7.showAffect = true;
				componentData7.CanTouch = true;
				componentData7.InvincibleUnregister();
				SetComponentData(componentData7);
				thisShadow.Show();
				GameUISingletonMono<UIBossHP>.ShowInit(myPpt.myEntity);
				Elite14.Inst.ForceSummon(bornSummonCount);
			}
			if (stateExistTime > 0.5f)
			{
				Elite14.Inst.state = Elite14.MonsterState.Summon;
				state = MonsterState.Move;
			}
			break;
		case MonsterState.Move:
		{
			_ = ref varMgr.RegBool(0);
			ref bool reference3 = ref varMgr.RegBool(1);
			ref float reference4 = ref varMgr.RegFloat(0);
			_ = ref varMgr.RegFloat(1);
			if (changedState)
			{
				base.Anima.Play("Move");
				base.SAnima.AnimationState.SetAnimation(0, "Move", loop: true);
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
			if (ToTargetDistanceSqr() > maxKeepDistance * maxKeepDistance && !reference3)
			{
				reference3 = true;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint, repositionRadius));
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			else
			{
				reference4 += Time.deltaTime;
				if (navInfo.allCornerArrived || reference4 > repositionTime.result)
				{
					repositionTime.RandomResult();
					reference4 = 0f;
					reference3 = false;
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
				base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
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
				base.SAnima.AnimationState.SetAnimation(0, "Move", loop: true);
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
		case MonsterState.SideSlashChase:
		{
			if (changedState)
			{
				base.Anima.Play("SideSlashChase");
				base.SAnima.AnimationState.SetAnimation(0, "SideSlashChase", loop: true);
				GetNearestTargetPlayerFirst();
				sideSlashFromLeft = GeneralTool.HalfChanceNPOne();
				sideSlashRotateRight = GeneralTool.HalfChanceNPOne();
				UnitProperty_Dots componentData4 = GetComponentData<UnitProperty_Dots>();
				componentData4.ImmuneMucusRegister();
				SetComponentData(componentData4);
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.SideSlash;
				break;
			}
			if (stateExistTime > sideSlashChaseTime)
			{
				state = MonsterState.SideSlash;
				break;
			}
			Vector3 point = base.TargetPoint - ToTargetDir() * sideSlashIdealRange + Tool2D.GetDir(ToTargetDir(), 90f * sideSlashRotateRight) * 1f;
			SetMove(ToPointDir(point) * base.MoveSpeed * sideSlashChaseSpeedRatio);
			if (Mathf.Abs(ToPointDistance(base.TargetPoint) - sideSlashIdealRange) < 0.5f)
			{
				state = MonsterState.SideSlash;
			}
			break;
		}
		case MonsterState.SideSlash:
			if (changedState)
			{
				SEMgr.Inst.elite14SideSlashShout.PlaySE();
				if (sideSlashFromLeft > 0f)
				{
					base.Anima.Play("SideSlash L", 0, 0f);
					base.SAnima.AnimationState.SetAnimation(0, "SideSlash_L", loop: false);
				}
				else
				{
					base.Anima.Play("SideSlash R", 0, 0f);
					base.SAnima.AnimationState.SetAnimation(0, "SideSlash_R", loop: false);
				}
				sideSlashFinished = false;
				sideSlashLocked = false;
				if (!base.HaveTarget)
				{
					base.CurrentMotion = ToPointDir(Elite14.Inst.roomCenterPoint, Random.Range(-30, 30));
				}
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				Vector3 point2 = base.TargetPoint - ToTargetDir() * sideSlashIdealRange + Tool2D.GetDir(ToTargetDir(), 90f * sideSlashRotateRight) * 1f;
				dashDir = ToTargetDir();
				SetMove(ToPointDir(point2) * base.MoveSpeed * sideSlashChaseSpeedRatio);
				if (sideSlashLocked)
				{
					SetFlip(ToTargetDelta().x);
				}
				else
				{
					sideSlashNeedFlip = ToTargetDelta().x > 0f == !myPpt.SR_Models[0].flipX;
				}
			}
			else
			{
				SetMove(base.CurrentMotion.normalized * base.MoveSpeed * sideSlashChaseSpeedRatio);
			}
			break;
		case MonsterState.SlashDashPrepare:
		{
			if (changedState)
			{
				lr_DashWarning.enabled = true;
				SEMgr.Inst.elite14SlashDashShout.PlaySE();
				base.Anima.Play("SlashDashPrepare");
				base.SAnima.AnimationState.SetAnimation(0, "SlashDashParpare", loop: false);
				dashDir = ToPointDir(Elite14.Inst.roomCenterPoint, Random.Range(-30, 30));
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDelta().x);
			}
			if (base.HaveTarget)
			{
				dashDir = ToTargetDir();
			}
			for (int i = 0; i < lr_DashWarning.positionCount; i++)
			{
				Vector3 rootPoint = Vector3.Lerp(base.transform.position, base.transform.position + dashDir * myPpt.unitCfg.moveSpeed * slashDashSpeedRatio * slashDashTime, (float)i / (float)(lr_DashWarning.positionCount - 1));
				lr_DashWarning.SetPosition(i, Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect));
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
		case MonsterState.SlashDash:
		{
			ref Vector3 reference5 = ref varMgr.RegV3(0);
			ref Vector3 reference6 = ref varMgr.RegV3(1);
			ref bool reference7 = ref varMgr.RegBool(0);
			ref float reference8 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				lr_DashWarning.enabled = false;
				slashDashParticle.Play();
				UnitProperty_Dots componentData5 = GetComponentData<UnitProperty_Dots>();
				componentData5.CanTouch = false;
				componentData5.ImmuneMucusRegister();
				componentData5.ImmuneKnockbackRegister();
				SetComponentData(componentData5);
				wallTrigger.enabled = true;
				base.gameObject.layer = LayerMask.NameToLayer("Monster_Ghost");
				PhysicsCollider pc = GetComponentData<PhysicsCollider>();
				DTool.SetCollider(in pc, 2048u, DTool.GetCollidesWith(8192u));
				SetComponentData(pc);
				reference5 = dashDir;
				GetNearestTargetPlayerFirst();
				base.Anima.Play("SlashDash");
				base.SAnima.AnimationState.SetAnimation(0, "SlashDash", loop: true);
				reference6 = base.transform.position;
				reference7 = GeneralTool.ChanceResult(0.5f);
			}
			reference8 += Time.deltaTime;
			if (reference8 > 0.2f)
			{
				if (reference7)
				{
					slashParticle.Play();
				}
				else
				{
					slashParticle1.Play();
				}
				reference7 = !reference7;
				reference8 -= 0.2f;
			}
			if ((base.transform.position - reference6).sqrMagnitude > delaySlashDistanceInterval * delaySlashDistanceInterval)
			{
				reference6 = base.transform.position;
				delaySlashCount.RandomResult();
				for (int j = 0; j < delaySlashCount.result; j++)
				{
					CreateSingleSlash();
				}
			}
			if (base.HaveTarget)
			{
				reference5 = Tool2D.RotateTowardsAroundZAxis(reference5, ToTargetDir(), Time.deltaTime * base.MoveSpeed * slashDashRotateSpeed);
			}
			SetMove(reference5 * base.MoveSpeed * slashDashSpeedRatio);
			if (stateExistTime > slashDashTime)
			{
				wallTrigger.enabled = false;
				base.gameObject.layer = LayerMask.NameToLayer("Monster");
				PhysicsCollider pc2 = GetComponentData<PhysicsCollider>();
				DTool.SetCollider(in pc2, 2048u, DTool.GetCollidesWith(2048u));
				SetComponentData(pc2);
				slashDashParticle.Stop();
				UnitProperty_Dots componentData6 = GetComponentData<UnitProperty_Dots>();
				componentData6.CanTouch = true;
				componentData6.ImmuneKnockbackUnregister();
				componentData6.ImmuneMucusUnregister();
				SetComponentData(componentData6);
				state = MonsterState.Move;
			}
			break;
		}
		case MonsterState.Buff:
			if (changedState)
			{
				base.Anima.Play("Buff");
				base.SAnima.AnimationState.SetAnimation(0, "Buff", loop: false);
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
		case MonsterState.ShadowAttackChase:
			if (changedState)
			{
				base.Anima.Play("SideSlashChase");
				base.SAnima.AnimationState.SetAnimation(0, "SideSlashChase", loop: true);
				GetNearestTargetPlayerFirst();
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.ImmuneMucusRegister();
				SetComponentData(componentData);
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.ShadowAttackHide;
				break;
			}
			if (stateExistTime > sideSlashChaseTime)
			{
				state = MonsterState.ShadowAttackHide;
				break;
			}
			SetMove(ToTargetDir() * base.MoveSpeed * sideSlashChaseSpeedRatio);
			if (ToTargetDistanceSqr() < shadowAttackAwailableDistance * shadowAttackAwailableDistance)
			{
				state = MonsterState.ShadowAttackHide;
			}
			break;
		case MonsterState.ShadowAttackHide:
			if (changedState)
			{
				base.Anima.Play("ShadowAttackHide");
				base.SAnima.AnimationState.SetAnimation(0, "ShadowAttackHide", loop: false);
				SEMgr.Inst.elite14ShadowHide.PlaySE();
				SEMgr.Inst.elite14ShadowAttackShout.PlaySE();
				UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
				componentData3.ImmuneMucusUnregister();
				SetComponentData(componentData3);
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
		case MonsterState.ShadowAttack:
		{
			_ = ref varMgr.RegV3(0);
			ref float reference = ref varMgr.RegFloat(0);
			ref int reference2 = ref varMgr.RegInt(0);
			if (changedState)
			{
				shadowAttackCount.RandomResult();
				lastShadowAttackPoint = base.transform.position;
				shadows.Clear();
			}
			SetMove(Vector3.zero, isFlip: false);
			reference += Time.deltaTime;
			if (reference > shadowAttackInterval)
			{
				reference -= shadowAttackInterval;
				reference2++;
				CreateSingleShadowAttack();
				if (reference2 > shadowAttackCount.result)
				{
					state = MonsterState.ShadowAttackShow;
				}
			}
			break;
		}
		case MonsterState.ShadowAttackShow:
			_ = ref varMgr.RegV3(0);
			if (changedState)
			{
				base.transform.position = lastShadowAttackPoint;
				SyncDotsPosition();
				base.Anima.Play("ShadowAttackShow");
			}
			break;
		}
	}

	private void CreateSingleShadowAttack()
	{
		float value = Random.value;
		float num = Mathf.Lerp(0f, shadowAttackDistanceRange, Mathf.Pow(value, 1f));
		GetNearestTargetPlayerFirst();
		Vector3 vector;
		if (base.HaveTarget)
		{
			vector = base.TargetPoint;
			if (targetEntity == PlayerMgr.Inst.PlayerEtt)
			{
				vector = Tool2D.IgnoreZPoint(vector + PlayerMgr.Inst.PlayerCtrller.CurrentMotion * shadowAttackPredictTime);
				vector = Tool2D.GetNavMeshPointIngoreZ(vector);
			}
		}
		else
		{
			vector = Tool2D.GetNavMeshPointIngoreZ(lastShadowAttackPoint);
		}
		lastShadowAttackPoint = vector;
		Elite14_Shadow component = Elite14.MiniPool.GetGO("Prefabs/EF/EF_Elite14_Shadow", vector).GetComponent<Elite14_Shadow>();
		Vector3 sortDir = GetSortDir();
		component.Initialize(sortDir, vector + Tool2D.GetDir(sortDir, 90f) * num);
		shadows.Add(component);
	}

	private Vector3 GetSortDir()
	{
		if (shadows.Count < 3)
		{
			return Tool2D.GetDir();
		}
		shadows.Sort();
		shadowsAngle.Clear();
		shadowsAngleDelta.Clear();
		for (int i = 0; i < shadows.Count; i++)
		{
			float num = Tool2D.IgnoreZAngleWithSign(Vector3.up, shadows[i].direction);
			if (num < 0f)
			{
				num += 360f;
			}
			shadowsAngle.Add(num);
		}
		for (int j = 0; j < shadowsAngle.Count; j++)
		{
			int num2 = j + 1;
			if (num2 >= shadowsAngle.Count)
			{
				num2 = 0;
			}
			float num3 = shadowsAngle[j] - shadowsAngle[num2];
			if (num3 < 0f)
			{
				num3 += 360f;
			}
			shadowsAngleDelta.Add(num3);
		}
		int index = 0;
		float num4 = 0f;
		for (int k = 0; k < shadowsAngleDelta.Count; k++)
		{
			if (shadowsAngleDelta[k] > num4)
			{
				index = k;
				num4 = shadowsAngleDelta[k];
			}
		}
		return Tool2D.GetDir(Vector3.up, shadowsAngle[index] - shadowsAngleDelta[index] / 2f).normalized;
	}

	private void CreateSideSlash(Vector3 attackPoint)
	{
		float num = 0f;
		num = ((tsf_Model.localScale.x > 0f != sideSlashFromLeft > 0f) ? 1f : (-1f));
		Vector3 vector = attackPoint - base.transform.position;
		vector = vector.normalized * Mathf.Clamp(vector.magnitude, sideSlashDistanceRange.value1, sideSlashDistanceRange.value2);
		Vector3 normalized = Tool2D.GetDir(vector, 90f * num).normalized;
		Vector3 vector2 = base.transform.position + normalized * Mathf.Pow(sideSlashFromPointDistance * sideSlashFromPointDistance - vector.sqrMagnitude, 0.5f);
		Vector3 oldDir = base.transform.position + vector - vector2;
		sideSlashWavesCount.RandomResult();
		GeneralTool.RandomizeList(slashMatsType);
		for (int i = 0; i < sideSlashWavesCount.result; i++)
		{
			Vector3 dir = Tool2D.GetDir(oldDir, sideSlashAngleRange * ((float)i / (float)(sideSlashWavesCount.result - 1) - 0.5f));
			Vector3 point = vector2 + dir.normalized * (dir.magnitude - sideSlashStartDistance.RandomResult());
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite14_DelaySlash", point).GetComponent<Elite14_DelaySlash>().Initialize(dir, isSideSlash: true, slashMatsType[i]);
		}
		Debug.DrawLine(vector2, base.transform.position, Color.blue, 1f);
		Debug.DrawLine(vector2, base.transform.position + vector, Color.red, 1f);
	}

	private void CreateSingleSlash()
	{
		dashSlashFromLeft = !dashSlashFromLeft;
		Vector3 dir = Tool2D.GetDir(base.CurrentMotion.normalized, ((!dashSlashFromLeft) ? 1 : (-1)) * Random.Range(60, 120));
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite14_DelaySlash", base.transform.position + dashSlashForwardOffset * base.CurrentMotion.normalized + dir * delaySlashDistanceRange.RandomResult()).GetComponent<Elite14_DelaySlash>().Initialize(Tool2D.GetDir(dir, delaySlashDistanceAngle.RandomResult()), isSideSlash: false, Random.Range(0, 2));
	}

	public void Trigger(Entity other)
	{
		if (state != MonsterState.SlashDash)
		{
			return;
		}
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		Vector3 vector = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position;
		switch (layer)
		{
		case 256u:
		case 65536u:
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = true;
			componentData.ImmuneKnockbackUnregister();
			wallTrigger.enabled = false;
			slashDashParticle.Stop();
			base.gameObject.layer = LayerMask.NameToLayer("Monster");
			PhysicsCollider pc = GetComponentData<PhysicsCollider>();
			DTool.SetCollider(in pc, 2048u, DTool.GetCollidesWith(2048u));
			SetComponentData(pc);
			base.transform.position -= base.CurrentMotion * 0.1f;
			SyncDotsPosition();
			base.Rigid.linearVelocity = Vector3.zero;
			SyncDotsVelocity();
			state = MonsterState.Move;
			break;
		}
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
			if (!dashedEntities.Contains(other))
			{
				dashedEntities.Add(other);
				dashedTimer.Add(0.5f);
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
				info.damage = dashDamage;
				info.knockbackForce = ((vector - base.transform.position).normalized * 0.5f + base.CurrentMotion * 0.5f) * dashKnockBack;
				info.teammateTakeDamageRatio = 4f;
				if (layer == 131072)
				{
					info.damage = 999999f;
					info.ignoreFloatText = true;
				}
				if (layer != 32768)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch_Large", vector + Tool2D.GetDir() * Random.Range(0f, 0.2f) + new Vector3(0f, -1f, -0.5f), 1f);
					dashedEntities.Add(other);
					dashedTimer.Add(0.5f);
					SEMgr.Inst.monster37_KnockUnit.PlaySE();
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			}
			break;
		}
	}

	protected override void BossDeadStay()
	{
		base.Anima.Play("Die");
		base.SAnima.AnimationState.SetAnimation(0, "Die", loop: false);
		base.SAnima.Update(2f);
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
		Elite14.Inst.DotsAnnouncedDeath();
		lr_DashWarning.enabled = false;
		SEMgr.Inst.elite14Die.PlaySE();
	}

	public void Buff()
	{
		for (int i = 0; i < Elite14.Inst.children.Count; i++)
		{
			Elite14.Inst.children[i].Buff();
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (base.deadStayed)
		{
			return;
		}
		switch (animaName)
		{
		case "SideSlashLock":
			sideSlashLocked = true;
			break;
		case "SideSlash":
		{
			SEMgr.Inst.elite14Slash.PlaySE().pitch = Random.Range(0.9f, 1.1f);
			Vector3 attackPoint = ((!base.HaveTarget) ? (base.transform.position + base.CurrentMotion.normalized * sideSlashIdealRange) : base.TargetPoint);
			if (sideSlashFromLeft > 0f == !myPpt.SR_Models[0].flipX)
			{
				slashParticle.Play();
			}
			else
			{
				slashParticle1.Play();
			}
			CreateSideSlash(attackPoint);
			break;
		}
		case "SideSlashFinish":
			if (sideSlashFinished)
			{
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.ImmuneMucusUnregister();
				SetComponentData(componentData);
				state = MonsterState.Move;
				break;
			}
			sideSlashLocked = false;
			sideSlashFromLeft *= -1f;
			if (sideSlashFromLeft > 0f)
			{
				base.Anima.Play("SideSlash L", 0, 0f);
				base.SAnima.AnimationState.SetAnimation(0, "SideSlash_L", loop: false);
			}
			else
			{
				base.Anima.Play("SideSlash R", 0, 0f);
				base.SAnima.AnimationState.SetAnimation(0, "SideSlash_R", loop: false);
			}
			sideSlashFinished = true;
			break;
		case "SlashSound":
			SEMgr.Inst.elite14Slash.PlaySE().pitch = Random.Range(0.9f, 1.1f);
			break;
		case "SlashDashPrepareFinish":
			state = MonsterState.SlashDash;
			break;
		case "BuffShout":
			SEMgr.Inst.elite14Buff.PlaySE();
			break;
		case "BuffSummon":
			Elite14.Inst.ForceSummon(forceSummonCount);
			break;
		case "BuffStart":
			CamController.Inst.SetShock(buffShock);
			buffParticle.Play();
			Buff();
			break;
		case "Buff":
			Buff();
			break;
		case "BuffFinish":
			state = MonsterState.Move;
			break;
		case "ShadowAttackHide":
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = false;
			componentData.CanBeTarget = false;
			componentData.showAffect = false;
			componentData.InvincibleRegister();
			SetComponentData(componentData);
			base.CC_Self.enabled = false;
			SetDotsCCEnable(isOpen: false);
			break;
		}
		case "ShadowHideParticlePlay":
			shadowHideParticle.Play();
			break;
		case "ShadowAttackSound":
			SEMgr.Inst.elite14ShadowHide.PlaySE();
			break;
		case "ShadowAttackShowAnimation":
			base.SAnima.AnimationState.SetAnimation(0, "ShadowAttackShow", loop: false);
			break;
		case "ShadowAttackShowShock":
			CamController.Inst.SetShock(shadowAttackShowShock);
			break;
		case "ShadowAttackShow":
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = true;
			componentData.CanBeTarget = true;
			componentData.showAffect = true;
			componentData.InvincibleUnregister();
			SetComponentData(componentData);
			base.CC_Self.enabled = true;
			SetDotsCCEnable(isOpen: true);
			break;
		}
		case "ShadowAttackHideFinish":
			state = MonsterState.ShadowAttack;
			break;
		case "ShadowAttackShowFinish":
			state = MonsterState.Move;
			break;
		}
	}
}
