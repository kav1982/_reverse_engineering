using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss6_Stage2 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		BeforeHeadBullet,
		HeadBullet,
		AfterHeadBullet,
		SwitchBefore,
		SwitchAfter,
		ExplodeBullet,
		ExplodeBulletAfter,
		HeadDashBefore,
		HeadDash,
		HeadDashAfter,
		Blast,
		BlastReposition,
		Summon,
		SideAttack,
		GetDown,
		GetUp,
		StaticHide,
		FreeMoveEnter,
		FreeMove,
		FreeMoveLeave,
		FreeHide,
		FreeMoveCenter,
		FreeRoundEnter,
		FreeRound,
		FreeRoundLeave,
		Dead
	}

	private enum FreeMoveMode
	{
		Round,
		Horizontal,
		Vertical
	}

	[Header("状态")]
	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private float stateExistTime;

	[Header("通用身体设置")]
	public float extraViewScale;

	public float extraViewConstraint;

	public float fakeBodyHeight;

	public Transform tsf_Body;

	public Transform tsf_BodyShadow;

	public Transform tsf_Neck;

	public float neckOffsetRange;

	public Transform tsf_HeadCenterPoint;

	public Transform tsf_headAttack;

	public SpriteRenderer SR_Shadow;

	public SpriteRenderer SR_Head;

	public Color ShadowColor;

	public float attackBodyHeight;

	public float bodyInterval;

	public int bodyCount;

	public float legWaveAngle;

	public float legWaveAngle1;

	public float legWaveAngle1Offset;

	public float legWaveOffset;

	public float legWaveSpeed;

	public List<Entity> hitList = new List<Entity>();

	public List<float> hitTime = new List<float>();

	public List<Entity> instantHitList = new List<Entity>();

	public List<float> instantHitCount = new List<float>();

	public List<Boss6_Body> bodys = new List<Boss6_Body>();

	public float normalSpellClearTime;

	public float breakerClearTime;

	public float pierceSpellClearTime;

	public float infinitePierceSpellClearTime;

	private float hitListClearTimer;

	[Header("头设置")]
	public Boss6_Face face;

	public float staticHeadRotateSpeed;

	public float staticHeadRotateRange;

	public float staticHeadRotateSlowDownRange;

	public AnimationCurve headAngleBlendCurve;

	public int headAngleBlendRange;

	[Header("固定模式身体设置")]
	public float staticBodyAngle;

	public float staticBodyAngleSpeed;

	public float staticBodyPhaseOffset;

	public float staticDampTime;

	private Vector3 staticDampSpeed;

	public float staticMaxDampSpeed;

	public float staticExtraDampSpeed;

	public float staticMaxDistanceCrossCenter;

	public float staticMinDistanceFromBorder;

	public float staticExpectedDistance;

	public float staticNoTargetOffset;

	public float staticWaveSpeed;

	public float staticWaveRange;

	public float staticBodyDampTime;

	public float staticBodyStrength;

	public float staticBodyConstraintAngle;

	public float staticBodyStrengthBlendIndex;

	public AnimationCurve staticBodyStrengthCurve;

	private Vector3[] nodePoints;

	private Vector3[] nodeSpeed;

	public Vector3 staticFaceDirection;

	private Vector3 noTargetAlignOffset;

	[Header("选技能")]
	public VariableFloat actCD;

	public VariableInt oneSideUseSkillTime;

	private MonsterState thisSideState;

	private float oneSideUseSkillCount;

	private float actCDTimer;

	private float extraIdleTimer;

	[Header("技能结束额外cd")]
	public float headBulletRecoverTime;

	public float explodeBulletRecoverTime;

	public float headDashRecoverTime;

	public float aimBulletRecoverTime;

	public float blastRecoverTime;

	[Header("攻击动作衔接")]
	public float getDownTime;

	public float getUpTime;

	public AnimationCurve getDownCurve;

	private MonsterState afterGetDownState;

	[Header("入场和换边")]
	public ParticleSystem ShootParticle;

	public ShockParam bornIdleShock;

	public ParticleSystem SideShootParticle;

	public ShockParam sideShock;

	public float switchSideWaitTime;

	[Header("头喷子弹")]
	public float headAttackRounds;

	public float headAttackAngleRange;

	public float headAttackPrepareTime;

	public float headAttackTime;

	public AnimationCurve prepareHeadAngleCurve;

	public AnimationCurve attackHeadAngleCurve;

	public float headAttackBackwardDistance;

	private float headAttackRoundsCounter;

	private float headAttackStartLeft;

	public float attackHeadHorizontalOffset;

	public ParticleSystem headAttackParticle;

	public ParticleSystem headAttackParticle_H;

	private Vector3 attackStartPoint;

	public float headBulletInterval;

	public float headBulletLifeTime;

	public VariableFloat headBulletSpeed;

	public VariableInt headBulletDamage;

	public VariableFloat headBulletOffsetRange;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	[Header("分裂弹")]
	public int explodeBulletRounds;

	public int splitTimes;

	public AnimationCurve explodeBulletBackwardCurve;

	public float explodeBulletBackwardDistance;

	public float explodeBulletSpeed;

	public float explodeBulletFromBorder;

	public ParticleSystem explodeBulletChargeParticle;

	public ParticleSystem explodeBulletShootParticle;

	public ParticleSystem explodeBulletChargeParticle_H;

	public ParticleSystem explodeBulletShootParticle_H;

	private int explodeBulletRoundsCounter;

	[Header("头槌冲撞")]
	public float headDashRounds;

	public float headDashprepareExtraDistance;

	public AnimationCurve headDashPrepareCurve;

	public AnimationCurve headDashOutCurve;

	public AnimationCurve headDashBackCurve;

	public float headDashAfterDistance;

	public float headDashBulletInterval;

	public float headDashBulletDelayTime;

	private float headDashRoundsCounter;

	[Header("拉线封锁子弹")]
	public VariableInt blastBulletRounds;

	public VariableInt blastBulletCount;

	public float blastAngleRange;

	public float blastRepositionTime;

	public VariableFloat BlastAngleOffsetRange;

	public float minBlastAngleInterval;

	private float nowBlastAngle;

	private float lastBlastAngle;

	private float blastBulletRoundsCounter;

	[Header("召唤干扰小弟")]
	public int maxChildCount;

	public List<Boss6_LongChild> children;

	[Header("游行模式身体设置")]
	public float enterLerpDistance;

	public AnimationCurve enterLerpCurve;

	public float idleBodyHeight;

	public Vector3 freeMoveDirection;

	public List<Vector3> recordPoints = new List<Vector3>();

	public float recordPointInterval;

	private float headFromFirstRecordPoint;

	[Header("游行")]
	public float enterDistanceX;

	public float enterDistanceY;

	public float enterSpeedRatio;

	public float leaveSpeedRatio;

	public float waveSpeed;

	public float waveAngle;

	public float extraMoveSpeedRatio;

	private FreeMoveMode nowFreeMode;

	public LineRenderer waveWarningLine;

	public LineRenderer waveWarningLine_H;

	[Header("游行攻击")]
	public float freeMoveBodyWidth;

	private float roomWidth;

	private float roomHeight;

	private Vector3 roomCenter;

	public float waveBulletDelayRange;

	public float waveBulletOffsetRange;

	public int waveBulletGroupCount;

	public VariableInt waveBulletGroupCountVertical;

	public float waveBulletRoundsInterval;

	public float waveBulletSpeed;

	public float waveBulletLifeTime;

	public VariableInt waveBulletCount;

	public float waveBulletFrequency;

	public float waveBulletAmplitude;

	public float waveBulletInterval;

	[Header("中间游行攻击")]
	public float backBulletInterval;

	public float backBulletSpeed;

	public float backBulletAccleration;

	public float startBackDistance;

	public float rotateBulletInterval;

	public float rotateBulletSpeed;

	public float rotateBulletRotateSpeed;

	private Vector3 warningStartPoint;

	private Vector3 warningEndPoint;

	[Header("圆环游行")]
	private bool roundMoveClockWise;

	public float rotateRadius;

	public float roundSpeedRatio;

	public float roundLeaveSpeedRatio;

	public float roundLeaveTime;

	public VariableInt roundBulletGroupCount;

	public VariableInt roundBulletCount;

	public float roundBulletRoundsInterval;

	public float roundBulletInterval;

	public float roundBulletDelayRange;

	public float roundBulletOffsetRange;

	public float roundBulletDurationTime;

	public float roundBulletFrequency;

	public float roundBulletAmplitude;

	public float roundBulletSpeed;

	public VariableFloat roundBulletAngleRange;

	private float rotateAngleSpeed;

	private float nowRotateAngle;

	private float startRotateAngle;

	private float bodyAllAngle;

	[Header("死亡放烟花")]
	public float deadExplodeTime;

	public float deadExplodeDelayTime;

	private float deadExplodeTimer;

	private bool startDeadExplode;

	public ShockParam deadShock;

	private bool bossDeadStay;

	[Header("音效")]
	public AudioSource as_Wave;

	public AudioSource as_Wave1;

	public static Boss6_Stage2 Inst;

	public DynamicBuffer<TakeDamageInfo_Dots> takeDamageInfoBuffer;

	private List<MonsterState> skills = new List<MonsterState>
	{
		MonsterState.BeforeHeadBullet,
		MonsterState.ExplodeBullet,
		MonsterState.Blast,
		MonsterState.SideAttack
	};

	private List<MonsterState> simpleSkills = new List<MonsterState>
	{
		MonsterState.Blast,
		MonsterState.SideAttack
	};

	private MonsterState lastState;

	private List<FreeMoveMode> freeModes = new List<FreeMoveMode>
	{
		FreeMoveMode.Round,
		FreeMoveMode.Horizontal,
		FreeMoveMode.Vertical
	};

	private float nowAngle;

	private float nowHandAngle;

	private bool upsideDown;

	private bool waveOffset;

	public bool isFreeMode
	{
		get
		{
			if (state != MonsterState.FreeMoveEnter && state != MonsterState.FreeMove && state != MonsterState.FreeMoveLeave)
			{
				return state == MonsterState.FreeHide;
			}
			return true;
		}
	}

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
		as_Wave.volume = DataMgr.settingData.GetFinalSound();
		as_Wave1.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		CamController.Inst.SetSpecificConstraint(extraViewConstraint);
		face.Initialize();
		if (GameMgr.IsMobile_Static)
		{
			waveBulletDelayRange *= 1.25f;
			waveBulletRoundsInterval *= 1.25f;
			waveBulletInterval *= 1.25f;
			waveBulletGroupCount--;
			roundBulletRoundsInterval *= 1.25f;
			roundBulletInterval *= 1.25f;
			explodeBulletRounds--;
			headBulletInterval *= 1.25f;
			roundBulletGroupCount.value1--;
			roundBulletGroupCount.value2--;
		}
	}

	public override void EveryInitialCallback()
	{
		Inst = this;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y;
		roomCenter = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		myPpt.unitCfg.unitType = UnitType.Boss;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.unitCfg.unitType = UnitType.Boss;
		SetComponentData(componentData);
		SR_Shadow.color = ShadowColor;
		myPpt.RemoveSRFromArray(SR_Shadow);
		recordPoints.Clear();
		int num = Mathf.CeilToInt((float)(bodyCount + 3) * bodyInterval / recordPointInterval);
		for (int i = 0; i < num; i++)
		{
			recordPoints.Add(base.transform.position);
		}
		bodys.Clear();
		for (int j = 0; j < bodyCount; j++)
		{
			Boss6_Body component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + 500622, base.transform.position).GetComponent<Boss6_Body>();
			bodys.Add(component);
			component.master = this;
			component.hitList = hitList;
			if (j == bodyCount - 1)
			{
				component.SetTail(isTail: true);
			}
			else
			{
				component.SetTail(isTail: false);
			}
		}
		tsf_Body.localPosition = new Vector3(0f, attackBodyHeight, 0f - attackBodyHeight - fakeBodyHeight);
		if (GeneralTool.ChanceResult(0.5f))
		{
			freeMoveDirection = new Vector3(GeneralTool.HalfChanceNPOne(), 0f, 0f);
		}
		else
		{
			freeMoveDirection = new Vector3(0f, GeneralTool.HalfChanceNPOne(), 0f);
		}
		actCD.RandomResult();
		nowFreeMode = FreeMoveMode.Round;
		state = MonsterState.FreeMoveEnter;
		GameUISingletonMono<UIBossHP>.ShowInit(myPpt.myEntity);
		if (GameMgr.IsChAge14_Static)
		{
			explodeBulletChargeParticle = explodeBulletChargeParticle_H;
			explodeBulletShootParticle = explodeBulletShootParticle_H;
			headAttackParticle = headAttackParticle_H;
			waveWarningLine = waveWarningLine_H;
		}
		waveWarningLine.positionCount = 10;
		takeDamageInfoBuffer = UnitDotsSyncSystem.entityMgr.GetBuffer<TakeDamageInfo_Dots>(myPpt.myEntity);
	}

	public override void Frame1InitialCallback()
	{
		base.Frame1InitialCallback();
	}

	public void TryAct()
	{
		actCDTimer += Time.deltaTime;
		if (actCDTimer > actCD.result)
		{
			ChooseSkill();
			actCDTimer = 0f;
			actCD.RandomResult();
		}
	}

	public void ChooseSkill()
	{
		if (oneSideUseSkillTime.result == 0)
		{
			oneSideUseSkillTime.RandomResult();
		}
		oneSideUseSkillCount += 1f;
		if (oneSideUseSkillCount > (float)oneSideUseSkillTime.result)
		{
			oneSideUseSkillCount = 0f;
			oneSideUseSkillTime.RandomResult();
			thisSideState = MonsterState.SwitchBefore;
			state = thisSideState;
			lastState = state;
			return;
		}
		thisSideState = skills[GeneralTool.GetWeightRandom(1f, 1f, 1f, 0f)];
		if (thisSideState == lastState)
		{
			for (int i = 0; i < 30; i++)
			{
				thisSideState = skills[GeneralTool.GetWeightRandom(1f, 1f, 1f, 0f)];
				if (thisSideState != lastState)
				{
					break;
				}
			}
		}
		if (simpleSkills.Contains(thisSideState))
		{
			lastState = thisSideState;
			afterGetDownState = thisSideState;
			state = MonsterState.GetDown;
		}
		else
		{
			state = thisSideState;
			lastState = state;
		}
	}

	public void ChooseSideSkill()
	{
		if (!isFreeMode)
		{
			state = MonsterState.FreeMoveEnter;
			FreeMoveMode freeMoveMode;
			for (freeMoveMode = freeModes[GeneralTool.GetWeightRandom(1f, 1f, 1f)]; freeMoveMode == nowFreeMode; freeMoveMode = freeModes[GeneralTool.GetWeightRandom(1f, 1f, 1f)])
			{
			}
			nowFreeMode = freeMoveMode;
		}
		else
		{
			GetNearestTargetPlayerFirst();
			float x;
			float y;
			if (base.HaveTarget)
			{
				x = base.TargetPoint.x;
				y = base.TargetPoint.y;
			}
			else
			{
				x = roomCenter.x;
				y = roomCenter.y;
			}
			if (staticFaceDirection.y != 0f)
			{
				staticFaceDirection = Vector3.right * GeneralTool.HalfChanceNPOne();
				base.transform.position = new Vector3(roomCenter.x - Mathf.Sign(staticFaceDirection.x) * (roomWidth / 2f + enterDistanceX), y, 0f);
			}
			else
			{
				staticFaceDirection = Vector3.up * GeneralTool.HalfChanceNPOne();
				base.transform.position = new Vector3(x, roomCenter.y - Mathf.Sign(staticFaceDirection.y) * (roomHeight / 2f + enterDistanceY), 0f);
			}
			StaticBodyInitialize();
			SetAfterAttack(switchSideWaitTime);
			SEMgr.Inst.boss6_Stage2Show.PlaySE();
		}
		actCDTimer = 0f;
	}

	private void SetAfterAttack(float time, bool getUp = false)
	{
		extraIdleTimer = time;
		state = MonsterState.Idle;
		if (getUp)
		{
			state = MonsterState.GetUp;
		}
	}

	public void SummonAllChild()
	{
		int num = maxChildCount - children.Count;
		for (int i = 0; i < num; i++)
		{
			Boss6_LongChild component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/500651", Vector3.zero).GetComponent<Boss6_LongChild>();
			component.Initialize(enterDistanceX, enterDistanceY);
			children.Add(component);
		}
	}

	public Vector3 SharedTargetPos()
	{
		Vector3 result = Vector3.zero;
		if (base.HaveTarget)
		{
			result = base.TargetPoint;
		}
		return result;
	}

	public Vector3 GetPositionByBody(Boss6_Body body)
	{
		float num = ((float)(bodys.IndexOf(body) + 1) - 0.5f) * bodyInterval;
		int num2 = Mathf.CeilToInt((num - headFromFirstRecordPoint) / recordPointInterval);
		if (num2 <= 0)
		{
			num2 = 1;
		}
		float num3 = headFromFirstRecordPoint + (float)num2 * recordPointInterval - num;
		return recordPoints[num2] - (recordPoints[num2] - recordPoints[num2 - 1]).normalized * num3;
	}

	private void ResetFreeBody(Vector3 position, Vector3 diration)
	{
		base.transform.position = position;
		SyncDotsPosition();
		for (int i = 0; i < recordPoints.Count; i++)
		{
			recordPoints[i] = base.transform.position - diration * i * recordPointInterval;
		}
		for (int j = 0; j < bodyCount; j++)
		{
			bodys[j].state = Boss6_Body.MonsterState.FreeFollow;
		}
	}

	private void SetHeadRotate(float angle)
	{
		float num = Tool2D.IgnoreZAngleWithSign(Vector3.up, staticFaceDirection) + angle;
		float z = tsf_Body.eulerAngles.z;
		float num2 = Mathf.Abs(z - num);
		float z2 = Mathf.MoveTowardsAngle(z, num, Mathf.Lerp(0f, 1f, num2 / staticHeadRotateSlowDownRange) * staticHeadRotateSpeed * Time.deltaTime);
		tsf_Body.eulerAngles = new Vector3(0f, 0f, z2);
	}

	private void SetNeckRotate()
	{
		Vector3 to = bodys[0].tsf_Body.transform.up + tsf_Body.up;
		float z = Tool2D.IgnoreZAngleWithSign(Vector3.up, to);
		tsf_Neck.eulerAngles = new Vector3(0f, 0f, z);
	}

	private void SetAllHeight(float height)
	{
		if (isFreeMode)
		{
			tsf_Body.localPosition = new Vector3(0f, height, (0f - height - fakeBodyHeight) * 0.01f);
		}
		else
		{
			tsf_Body.localPosition = new Vector3(0f, height, (0f - height - fakeBodyHeight) * 0.01f) + Tool2D.GetDir(staticFaceDirection, 90f) * neckOffsetRange * Tool2D.IgnoreZAngleWithSign(staticFaceDirection, tsf_Body.up) / 90f;
		}
		for (int i = 0; i < bodyCount; i++)
		{
			bodys[i].tsf_Body.localPosition = new Vector3(0f, height, (0f - height - fakeBodyHeight) * 0.01f);
		}
	}

	private Vector3 GetNoTargetPoint()
	{
		Vector3 vector;
		if (staticFaceDirection == Vector3.down)
		{
			vector = new Vector3(base.transform.position.x + (float)UnityEngine.Random.Range(-1, 1) * staticNoTargetOffset, roomCenter.y + UnityEngine.Random.value * roomWidth / 2f);
			vector.y = Mathf.Clamp(vector.y, roomCenter.y - staticMaxDistanceCrossCenter, roomCenter.y + roomHeight / 2f - staticMinDistanceFromBorder);
		}
		else if (staticFaceDirection == Vector3.up)
		{
			vector = new Vector3(base.transform.position.x + (float)UnityEngine.Random.Range(-1, 1) * staticNoTargetOffset, roomCenter.y - UnityEngine.Random.value * roomWidth / 2f);
			vector.y = Mathf.Clamp(vector.y, roomCenter.y - roomHeight / 2f + staticMinDistanceFromBorder, roomCenter.y + staticMaxDistanceCrossCenter);
		}
		else if (staticFaceDirection == Vector3.left)
		{
			vector = new Vector3(roomCenter.x + UnityEngine.Random.value * roomWidth / 2f, base.transform.position.y + (float)UnityEngine.Random.Range(-1, 1) * staticNoTargetOffset);
			vector.x = Mathf.Clamp(vector.x, roomCenter.x - staticMaxDistanceCrossCenter, roomCenter.x + roomWidth / 2f - staticMinDistanceFromBorder);
		}
		else
		{
			vector = new Vector3(roomCenter.x + UnityEngine.Random.value * roomWidth / 2f, base.transform.position.y + (float)UnityEngine.Random.Range(-1, 1) * staticNoTargetOffset);
			vector.x = Mathf.Clamp(vector.x, roomCenter.x - roomWidth / 2f + staticMinDistanceFromBorder, roomCenter.x + staticMaxDistanceCrossCenter);
		}
		vector.x = Mathf.Clamp(vector.x, roomCenter.x - roomWidth / 2f, roomCenter.x + roomWidth / 2f);
		vector.y = Mathf.Clamp(vector.y, roomCenter.y - roomHeight / 2f, roomCenter.y + roomHeight / 2f);
		return vector + staticFaceDirection * staticExpectedDistance;
	}

	private void KeepHeadDistanceFromPoint(Vector3 manualTargetPoint, bool headWaving = true, bool allowBackward = false)
	{
		Vector3 target = manualTargetPoint - staticFaceDirection * staticExpectedDistance;
		if (headWaving)
		{
			target += Mathf.Sin(Time.time * staticWaveSpeed * (MathF.PI / 180f)) * staticWaveRange * ((staticFaceDirection.y != 0f) ? Vector3.right : Vector3.up);
		}
		if (allowBackward)
		{
			if (staticFaceDirection == Vector3.down)
			{
				target.y = Mathf.Max(target.y, roomCenter.y - staticMaxDistanceCrossCenter);
			}
			else if (staticFaceDirection == Vector3.up)
			{
				target.y = Mathf.Min(target.y, roomCenter.y + staticMaxDistanceCrossCenter);
			}
			else if (staticFaceDirection == Vector3.left)
			{
				target.x = Mathf.Max(target.x, roomCenter.x - staticMaxDistanceCrossCenter);
			}
			else
			{
				target.x = Mathf.Min(target.x, roomCenter.x + staticMaxDistanceCrossCenter);
			}
		}
		else if (staticFaceDirection == Vector3.down)
		{
			target.y = Mathf.Clamp(target.y, roomCenter.y - staticMaxDistanceCrossCenter, roomCenter.y + roomHeight / 2f - staticMinDistanceFromBorder);
		}
		else if (staticFaceDirection == Vector3.up)
		{
			target.y = Mathf.Clamp(target.y, roomCenter.y - roomHeight / 2f + staticMinDistanceFromBorder, roomCenter.y + staticMaxDistanceCrossCenter);
		}
		else if (staticFaceDirection == Vector3.left)
		{
			target.x = Mathf.Clamp(target.x, roomCenter.x - staticMaxDistanceCrossCenter, roomCenter.x + roomWidth / 2f - staticMinDistanceFromBorder);
		}
		else
		{
			target.x = Mathf.Clamp(target.x, roomCenter.x - roomWidth / 2f + staticMinDistanceFromBorder, roomCenter.x + staticMaxDistanceCrossCenter);
		}
		base.transform.position = Vector3.SmoothDamp(base.transform.position, target, ref staticDampSpeed, staticDampTime, staticMaxDampSpeed);
		SyncDotsPosition();
	}

	private void KeepHeadDistanceFromTarget(bool headWaving = true)
	{
		Vector3 target = (base.HaveTarget ? (base.TargetPoint - staticFaceDirection * staticExpectedDistance) : noTargetAlignOffset);
		if (headWaving)
		{
			target += Mathf.Sin(Time.time * staticWaveSpeed * (MathF.PI / 180f)) * staticWaveRange * ((staticFaceDirection.y != 0f) ? Vector3.right : Vector3.up);
		}
		if (staticFaceDirection == Vector3.down)
		{
			target.y = Mathf.Clamp(target.y, roomCenter.y - staticMaxDistanceCrossCenter, roomCenter.y + roomHeight / 2f - staticMinDistanceFromBorder);
		}
		else if (staticFaceDirection == Vector3.up)
		{
			target.y = Mathf.Clamp(target.y, roomCenter.y - roomHeight / 2f + staticMinDistanceFromBorder, roomCenter.y + staticMaxDistanceCrossCenter);
		}
		else if (staticFaceDirection == Vector3.left)
		{
			target.x = Mathf.Clamp(target.x, roomCenter.x - staticMaxDistanceCrossCenter, roomCenter.x + roomWidth / 2f - staticMinDistanceFromBorder);
		}
		else
		{
			target.x = Mathf.Clamp(target.x, roomCenter.x - roomWidth / 2f + staticMinDistanceFromBorder, roomCenter.x + staticMaxDistanceCrossCenter);
		}
		base.transform.position = Vector3.SmoothDamp(base.transform.position, target, ref staticDampSpeed, staticDampTime, staticMaxDampSpeed);
		SyncDotsPosition();
	}

	private void StaticBodyInitialize()
	{
		nodePoints = new Vector3[bodys.Count];
		nodeSpeed = new Vector3[bodys.Count];
		float num = staticBodyAngle * Mathf.Sin(Time.time * staticBodyAngleSpeed * (MathF.PI / 180f) - 0.5f * staticBodyPhaseOffset);
		float num2 = Tool2D.IgnoreZAngleWithSign(Vector3.down, staticFaceDirection);
		Vector3 dir = Tool2D.GetDir(num + num2);
		Vector3 vector = Tool2D.IgnoreZPoint(base.transform.position) + bodyInterval * dir * 0.5f;
		float b = Tool2D.IgnoreZAngleWithSign(staticFaceDirection, tsf_Body.up);
		nowHandAngle += Time.deltaTime * legWaveSpeed * (MathF.PI / 180f);
		for (int i = 0; i < bodyCount; i++)
		{
			float degree = Mathf.Lerp(staticBodyAngle * Mathf.Sin(Time.time * staticBodyAngleSpeed * (MathF.PI / 180f) + (float)(i + 1) * staticBodyPhaseOffset * (MathF.PI / 180f)), b, headAngleBlendCurve.Evaluate(i / headAngleBlendRange)) + num2;
			bodys[i].transform.position = vector;
			dir = Tool2D.GetDir(degree);
			vector += bodyInterval * dir;
			nodePoints[i] = vector;
			nodeSpeed[i] = Vector3.zero;
			bodys[i].SyncDotsPositionSafe();
		}
		for (int j = 0; j < bodyCount; j++)
		{
			Vector3 vector2 = ((j == 0) ? base.transform.position : bodys[j - 1].transform.position);
			Vector3 vector3 = ((j == bodys.Count - 1) ? bodys[j].transform.position : bodys[j + 1].transform.position);
			Vector3 dir2 = vector2 - vector3;
			bodys[j].SetDir(dir2);
			bodys[j].SetCoverDir(vector2 - bodys[j].transform.position);
			float rotateAngle = legWaveAngle * Mathf.Sin(nowHandAngle + (float)j * (MathF.PI / 180f) * legWaveOffset);
			float rotateAngle2 = legWaveAngle1 * Mathf.Sin(nowHandAngle + legWaveAngle1Offset * (MathF.PI / 180f) + (float)j * (MathF.PI / 180f) * legWaveOffset);
			bodys[j].SetHandDir(rotateAngle, rotateAngle2);
			bodys[j].SetColor(myPpt.BaseColor);
		}
		SetNeckRotate();
		SyncDotsPosition();
	}

	private void SetStaticBody(bool autolHeadRotation = true, bool bodyStrengthFade = false)
	{
		float headRotate = staticBodyAngle * Mathf.Sin(Time.time * staticBodyAngleSpeed * (MathF.PI / 180f) - 0.5f * staticBodyPhaseOffset);
		if (autolHeadRotation)
		{
			SetHeadRotate(headRotate);
		}
		for (int i = 0; i < bodyCount; i++)
		{
			Vector3 vector;
			Vector3 vector2;
			float num2;
			float num;
			switch (i)
			{
			case 0:
			{
				vector = base.transform.position;
				Vector3 to = nodePoints[i] - base.transform.position;
				float f = Tool2D.IgnoreZAngleWithSign(-staticFaceDirection, -tsf_Body.up);
				f = Mathf.Sign(f) * 90f * staticBodyStrengthCurve.Evaluate(Mathf.Abs(f) / 90f);
				vector2 = ((!bodyStrengthFade) ? (-tsf_Body.up.normalized * bodyInterval / 2f) : (Tool2D.GetDir(-staticFaceDirection, f).normalized * bodyInterval / 2f));
				num = 0f;
				num2 = bodyInterval / 2f;
				break;
			}
			case 1:
			{
				vector = nodePoints[i - 1];
				Vector3 to = nodePoints[i] - nodePoints[i - 1];
				vector2 = nodePoints[i] - base.transform.position;
				num = Tool2D.IgnoreZAngleWithSign(vector2, to);
				num2 = bodyInterval;
				break;
			}
			default:
			{
				vector = nodePoints[i - 1];
				Vector3 to = nodePoints[i] - nodePoints[i - 1];
				vector2 = nodePoints[i] - nodePoints[i - 2];
				num = Tool2D.IgnoreZAngleWithSign(vector2, to);
				num2 = bodyInterval;
				break;
			}
			}
			headRotate = staticBodyAngle * Mathf.Sin(Time.time * staticBodyAngleSpeed * (MathF.PI / 180f) + (float)(i + 1) * staticBodyPhaseOffset * (MathF.PI / 180f));
			num -= headRotate;
			nodePoints[i] = Vector3.SmoothDamp(nodePoints[i], vector + Tool2D.GetDir(vector2, num * (1f - staticBodyStrength)) * num2, ref nodeSpeed[i], staticBodyDampTime);
			nodePoints[i] = vector + (nodePoints[i] - vector).normalized * num2;
			if (i != 0)
			{
				Debug.DrawLine(nodePoints[i], nodePoints[i - 1]);
			}
			else
			{
				Debug.DrawLine(nodePoints[i], base.transform.position);
			}
			bodys[i].transform.position = nodePoints[i];
			bodys[i].SyncDotsPositionSafe();
		}
		nowHandAngle += Time.deltaTime * legWaveSpeed * (MathF.PI / 180f);
		for (int j = 0; j < bodyCount; j++)
		{
			Vector3 obj = ((j == 0) ? base.transform.position : bodys[j - 1].transform.position);
			Vector3 vector3 = ((j == bodys.Count - 1) ? bodys[j].transform.position : bodys[j + 1].transform.position);
			Vector3 dir = obj - vector3;
			bodys[j].SetDir(dir);
			float rotateAngle = legWaveAngle * Mathf.Sin(nowHandAngle + (float)j * (MathF.PI / 180f) * legWaveOffset);
			float rotateAngle2 = legWaveAngle1 * Mathf.Sin(nowHandAngle + legWaveAngle1Offset * (MathF.PI / 180f) + (float)j * (MathF.PI / 180f) * legWaveOffset);
			bodys[j].SetHandDir(rotateAngle, rotateAngle2);
			bodys[j].SetColor(myPpt.BaseColor);
		}
		SetNeckRotate();
	}

	public void FreeMove(float speedRatio = 1f)
	{
		if (nowFreeMode != 0)
		{
			Vector3 vector = Time.deltaTime * freeMoveDirection * speedRatio * extraMoveSpeedRatio * base.MoveSpeed;
			base.transform.position += vector;
			for (int i = 0; i < recordPoints.Count - 1; i++)
			{
				recordPoints[i] += vector;
			}
		}
		nowAngle += Time.deltaTime * waveSpeed * (MathF.PI / 180f) * speedRatio;
		if (nowAngle > MathF.PI * 2f)
		{
			nowAngle -= MathF.PI * 2f;
		}
		float num = Mathf.Sin(nowAngle) * waveAngle;
		if (nowFreeMode == FreeMoveMode.Round)
		{
			num *= 0.5f;
		}
		Vector3 normalized = Tool2D.GetDir(freeMoveDirection, num).normalized;
		base.transform.position += normalized * speedRatio * base.MoveSpeed * Time.deltaTime;
		tsf_Body.transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, normalized));
		float magnitude = (base.transform.position - recordPoints[0]).magnitude;
		if (headFromFirstRecordPoint > magnitude)
		{
			headFromFirstRecordPoint += 0.02f;
			base.transform.position = recordPoints[0] + (base.transform.position - recordPoints[0]).normalized * headFromFirstRecordPoint;
		}
		else
		{
			headFromFirstRecordPoint = magnitude;
		}
		while (headFromFirstRecordPoint > recordPointInterval)
		{
			for (int num2 = recordPoints.Count - 1; num2 > 0; num2--)
			{
				recordPoints[num2] = recordPoints[num2 - 1];
			}
			recordPoints[0] = recordPoints[0] + (base.transform.position - recordPoints[0]).normalized * recordPointInterval;
			headFromFirstRecordPoint -= recordPointInterval;
		}
		nowHandAngle += Time.deltaTime * legWaveSpeed * (MathF.PI / 180f);
		for (int j = 0; j < bodyCount; j++)
		{
			Vector3 positionByBody = GetPositionByBody(bodys[j]);
			bodys[j].transform.position = positionByBody;
			bodys[j].SyncDotsPositionSafe();
		}
		for (int k = 0; k < bodyCount; k++)
		{
			Vector3 vector2 = ((k == 0) ? base.transform.position : bodys[k - 1].transform.position);
			Vector3 vector3 = ((k == bodys.Count - 1) ? bodys[k].transform.position : bodys[k + 1].transform.position);
			Vector3 dir = vector2 - vector3;
			bodys[k].SetDir(dir);
			bodys[k].SetCoverDir(vector2 - bodys[k].transform.position);
			float rotateAngle = legWaveAngle * Mathf.Sin(nowHandAngle + (float)k * (MathF.PI / 180f) * legWaveOffset);
			float rotateAngle2 = legWaveAngle1 * Mathf.Sin(nowHandAngle + legWaveAngle1Offset * (MathF.PI / 180f) + (float)k * (MathF.PI / 180f) * legWaveOffset);
			bodys[k].SetHandDir(rotateAngle, rotateAngle2);
			bodys[k].SetColor(myPpt.BaseColor);
		}
		SyncDotsPosition();
	}

	private Vector3 GetBodyFirePoint(float percent)
	{
		Vector3 result = default(Vector3);
		Vector3 vector = (isFreeMode ? freeMoveDirection : staticFaceDirection);
		float num = ((!(vector == Vector3.up) && !(vector == Vector3.down)) ? Mathf.Abs(base.transform.position.x - (roomCenter.x + vector.x * (percent - 0.5f) * roomWidth)) : Mathf.Abs(base.transform.position.y - (roomCenter.y + vector.y * (percent - 0.5f) * roomHeight)));
		float num2 = 0f;
		Vector3 a = base.transform.position;
		for (int i = 0; i < bodys.Count; i++)
		{
			Vector3 position = bodys[i].transform.position;
			float num3 = ((vector == Vector3.up) ? (0f - (position.y - base.transform.position.y)) : ((vector == Vector3.down) ? (position.y - base.transform.position.y) : ((!(vector == Vector3.left)) ? (0f - (position.x - base.transform.position.x)) : (position.x - base.transform.position.x))));
			if (num2 <= num && num <= num3)
			{
				return Vector3.Lerp(a, position, (num - num2) / (num3 - num2));
			}
			num2 = num3;
			a = position;
		}
		return result;
	}

	private bool CheckCanFireRound(float percent)
	{
		bool result = false;
		float num = nowRotateAngle - bodyAllAngle * percent;
		if (num < 360f && num > 0f)
		{
			result = true;
		}
		return result;
	}

	private Vector3 GetBodyFirePointRound(float percent)
	{
		float num = percent * bodyInterval * (float)(bodyCount - 1);
		int num2 = 0;
		for (int i = 0; i < bodyCount; i++)
		{
			if (num < bodyInterval)
			{
				break;
			}
			num -= bodyInterval;
			num2++;
		}
		if (num2 == bodyCount - 1)
		{
			return bodys[num2].transform.position;
		}
		return bodys[num2].transform.position + (bodys[num2 + 1].transform.position - bodys[num2].transform.position) * num / bodyInterval;
	}

	public override void Update()
	{
		if (myPpt.unitCfg.currentHP <= 0f && state != MonsterState.Dead)
		{
			state = MonsterState.Dead;
			base.Anima.Play("Show");
		}
		if (state == MonsterState.Dead)
		{
			myPpt.ClearBurnState();
			myPpt.ClearFrozenState();
			myPpt.ClearMucusState();
			myPpt.ClearVenomState();
			myPpt.ClearBurnState();
			myPpt.ClearVoidState();
		}
		for (int num = children.Count - 1; num >= 0; num--)
		{
			if (children[num].myPpt.AlreadyDead)
			{
				children.RemoveAt(num);
			}
		}
		for (int num2 = instantHitList.Count - 1; num2 >= 0; num2--)
		{
			int index = hitList.IndexOf(instantHitList[num2]);
			instantHitList.RemoveAt(num2);
			instantHitCount.RemoveAt(num2);
			hitTime.RemoveAt(index);
			hitList.RemoveAt(index);
		}
		for (int num3 = hitTime.Count - 1; num3 >= 0; num3--)
		{
			hitTime[num3] -= Time.deltaTime;
			if (hitTime[num3] < 0f)
			{
				hitTime.RemoveAt(num3);
				hitList.RemoveAt(num3);
			}
		}
		if (isFreeMode)
		{
			base.CC_Self.center = Vector3.zero;
		}
		else
		{
			base.CC_Self.center = Tool2D.IgnoreZPoint(tsf_HeadCenterPoint.position - base.transform.position);
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
		tsf_BodyShadow.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow);
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				face.SetClose();
				base.transform.position = roomCenter + new Vector3(0f, roomHeight / 2f + enterDistanceY, 0f);
				tsf_Body.eulerAngles = new Vector3(0f, 0f, 180f);
				staticFaceDirection = Vector3.down;
				StaticBodyInitialize();
				SetStaticBody();
				SetAllHeight(idleBodyHeight);
			}
			KeepHeadDistanceFromPoint(roomCenter);
			SetAllHeight(idleBodyHeight);
			SetStaticBody();
			if (stateExistTime > 4f)
			{
				staticFaceDirection = Vector3.down;
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.SwitchBefore:
		{
			ref Vector3 reference7 = ref varMgr.RegV3(0);
			if (changedState)
			{
				if (staticFaceDirection.y != 0f)
				{
					reference7 = new Vector3(base.transform.position.x, roomCenter.y - staticFaceDirection.y * (roomHeight / 2f + enterDistanceY), 0f);
				}
				else
				{
					reference7 = new Vector3(roomCenter.x - staticFaceDirection.x * (roomWidth / 2f + enterDistanceX), base.transform.position.y, 0f);
				}
			}
			KeepHeadDistanceFromPoint(reference7, headWaving: true, allowBackward: true);
			SetStaticBody();
			SetAllHeight(idleBodyHeight);
			if ((base.transform.position + staticFaceDirection * staticExpectedDistance - reference7).sqrMagnitude < Mathf.Pow(staticExpectedDistance + 1f, 2f))
			{
				ChooseSideSkill();
			}
			break;
		}
		case MonsterState.Idle:
			if (changedState)
			{
				tsf_Body.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, staticFaceDirection));
				tsf_Body.gameObject.SetActive(value: true);
				for (int i = 0; i < bodys.Count; i++)
				{
					bodys[i].state = Boss6_Body.MonsterState.Idle;
				}
				base.Anima.Play("Idle");
				GetNearestTargetPlayerFirst();
				_ = PlayerMgr.Inst.PlayerCtrller.transform.position;
				noTargetAlignOffset = GetNoTargetPoint();
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			SetAllHeight(idleBodyHeight);
			KeepHeadDistanceFromTarget();
			SetStaticBody();
			if (extraIdleTimer > 0f)
			{
				extraIdleTimer -= Time.deltaTime;
				stateExistTime = 0f;
			}
			else
			{
				TryAct();
			}
			break;
		case MonsterState.GetDown:
			if (changedState)
			{
				base.Anima.Play("Idle");
				GetNearestTargetPlayerFirst();
				_ = PlayerMgr.Inst.PlayerCtrller.transform.position;
				noTargetAlignOffset = GetNoTargetPoint();
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			SetAllHeight(Mathf.Lerp(idleBodyHeight, attackBodyHeight, getDownCurve.Evaluate(stateExistTime / getDownTime)));
			KeepHeadDistanceFromTarget();
			SetStaticBody(autolHeadRotation: false);
			SetHeadRotate(0f);
			if (stateExistTime > getDownTime)
			{
				state = afterGetDownState;
			}
			break;
		case MonsterState.GetUp:
			if (changedState)
			{
				base.Anima.Play("Idle");
				GetNearestTargetPlayerFirst();
				_ = PlayerMgr.Inst.PlayerCtrller.transform.position;
				noTargetAlignOffset = GetNoTargetPoint();
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			SetAllHeight(Mathf.Lerp(attackBodyHeight, idleBodyHeight, getDownCurve.Evaluate(stateExistTime / getDownTime)));
			KeepHeadDistanceFromTarget();
			SetStaticBody(autolHeadRotation: false);
			SetHeadRotate(0f);
			if (stateExistTime > getDownTime)
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.BeforeHeadBullet:
		{
			if (changedState)
			{
				SEMgr.Inst.boss6_Roar1.PlaySE();
				face.SetClose();
				if (base.HaveTarget)
				{
					attackStartPoint = base.TargetPoint;
				}
				else
				{
					_ = PlayerMgr.Inst.PlayerCtrller.transform.position;
					attackStartPoint = GetNoTargetPoint();
				}
				headAttackStartLeft = GeneralTool.HalfChanceNPOne();
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				attackStartPoint = base.TargetPoint;
			}
			SetAllHeight(Mathf.Lerp(attackBodyHeight, idleBodyHeight, 1f - stateExistTime / headAttackPrepareTime));
			float num7 = headAttackStartLeft * attackHeadHorizontalOffset * (0f - prepareHeadAngleCurve.Evaluate(stateExistTime / headAttackPrepareTime));
			KeepHeadDistanceFromPoint(attackStartPoint + num7 * Tool2D.GetDir(staticFaceDirection, 90f), headWaving: false);
			SetStaticBody(autolHeadRotation: false, bodyStrengthFade: true);
			float num8 = headAttackStartLeft * headAttackAngleRange * prepareHeadAngleCurve.Evaluate(stateExistTime / headAttackPrepareTime);
			num8 += Tool2D.IgnoreZAngleWithSign(Vector3.down, staticFaceDirection);
			tsf_Body.eulerAngles = new Vector3(0f, 0f, 180f + num8);
			if (stateExistTime > headAttackPrepareTime)
			{
				state = MonsterState.HeadBullet;
			}
			break;
		}
		case MonsterState.HeadBullet:
		{
			_ = ref varMgr.RegV3(0);
			ref float reference = ref varMgr.RegFloat(0);
			if (changedState)
			{
				face.SetOpenContinue();
				headAttackRoundsCounter += 1f;
			}
			SetAllHeight(attackBodyHeight);
			float num4 = stateExistTime / headAttackTime;
			float num5 = headAttackStartLeft * headAttackAngleRange * attackHeadAngleCurve.Evaluate(num4);
			num5 += Tool2D.IgnoreZAngleWithSign(Vector3.down, staticFaceDirection);
			tsf_Body.eulerAngles = new Vector3(0f, 0f, 180f + num5);
			reference += Time.deltaTime;
			if (reference > headBulletInterval)
			{
				reference -= headBulletInterval;
				ShootHeadBullet(Tool2D.GetDir(180f + num5));
			}
			float num6 = headAttackStartLeft * attackHeadHorizontalOffset * attackHeadAngleCurve.Evaluate(num4);
			KeepHeadDistanceFromPoint(attackStartPoint + num6 * Tool2D.GetDir(staticFaceDirection, 90f) + staticFaceDirection * (0f - Mathf.Lerp(0f, headAttackBackwardDistance, num4)), headWaving: false);
			SetStaticBody(autolHeadRotation: false, bodyStrengthFade: true);
			if (stateExistTime > headAttackTime)
			{
				if (headAttackRoundsCounter >= headAttackRounds)
				{
					state = MonsterState.AfterHeadBullet;
					headAttackRoundsCounter = 0f;
				}
				else
				{
					state = MonsterState.HeadBullet;
				}
			}
			break;
		}
		case MonsterState.AfterHeadBullet:
		{
			if (changedState)
			{
				face.SetIdle();
			}
			float num12 = headAttackStartLeft * headAttackAngleRange * prepareHeadAngleCurve.Evaluate(1f - stateExistTime / headAttackPrepareTime);
			num12 += Tool2D.IgnoreZAngleWithSign(Vector3.down, staticFaceDirection);
			tsf_Body.eulerAngles = new Vector3(0f, 0f, 180f + num12);
			SetAllHeight(Mathf.Lerp(attackBodyHeight, idleBodyHeight, stateExistTime / headAttackPrepareTime));
			KeepHeadDistanceFromPoint(attackStartPoint);
			SetStaticBody(autolHeadRotation: false, bodyStrengthFade: true);
			if (stateExistTime > headAttackPrepareTime)
			{
				SetAfterAttack(headBulletRecoverTime);
			}
			break;
		}
		case MonsterState.Summon:
			if (changedState)
			{
				base.Anima.Play("Summon");
			}
			KeepHeadDistanceFromPoint(roomCenter);
			SetAllHeight(idleBodyHeight);
			SetStaticBody();
			break;
		case MonsterState.ExplodeBullet:
		{
			ref float reference2 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				if (explodeBulletRoundsCounter == 0)
				{
					SEMgr.Inst.boss6_Roar2.PlaySE();
				}
				SEMgr.Inst.boss6_ExplodeBulletCharge.PlaySE();
				face.SetClose();
				explodeBulletRoundsCounter++;
				base.Anima.Play("ExplodeBullet");
				reference2 = base.Anima.GetCurrentAnimatorClipInfo(0)[0].clip.length;
				explodeBulletChargeParticle.Play();
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					attackStartPoint = base.TargetPoint;
				}
				else
				{
					Vector3 position = PlayerMgr.Inst.PlayerCtrller.transform.position;
					attackStartPoint = position + Tool2D.GetDir() * staticNoTargetOffset;
				}
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				attackStartPoint = base.TargetPoint;
			}
			KeepHeadDistanceFromPoint(attackStartPoint);
			SetAllHeight(Mathf.Lerp(idleBodyHeight, attackBodyHeight, stateExistTime / reference2));
			SetStaticBody(autolHeadRotation: false);
			SetHeadRotate(0f);
			break;
		}
		case MonsterState.ExplodeBulletAfter:
		{
			ref float reference8 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				SEMgr.Inst.boss6_ExplodeBulletShoot.PlaySE();
				face.SetOpen();
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_ExplodeBullet" + (GameMgr.IsChAge14_Static ? " H" : ""), Tool2D.IgnoreZPoint(explodeBulletChargeParticle.transform.position)).GetComponent<Boss6_ExplodeBullet>().Initialize(staticFaceDirection, explodeBulletSpeed, splitTimes);
				explodeBulletChargeParticle.Stop();
				explodeBulletShootParticle.Play();
				base.Anima.Play("ExplodeBulletAfter");
				base.Anima.Update(0.1f);
				reference8 = base.Anima.GetCurrentAnimatorClipInfo(0)[0].clip.length;
				attackStartPoint = base.transform.position + staticFaceDirection * staticExpectedDistance;
			}
			SetAllHeight(Mathf.Lerp(attackBodyHeight, idleBodyHeight, stateExistTime / reference8));
			KeepHeadDistanceFromPoint(attackStartPoint + staticFaceDirection * (0f - Mathf.Lerp(0f, explodeBulletBackwardDistance, explodeBulletBackwardCurve.Evaluate(stateExistTime / reference8))), headWaving: false);
			SetStaticBody(autolHeadRotation: false);
			SetHeadRotate(0f);
			if (stateExistTime > reference8)
			{
				if (explodeBulletRoundsCounter >= explodeBulletRounds)
				{
					explodeBulletRoundsCounter = 0;
					SetAfterAttack(explodeBulletRecoverTime);
				}
				else
				{
					state = MonsterState.ExplodeBullet;
				}
			}
			break;
		}
		case MonsterState.Blast:
			if (changedState)
			{
				if (blastBulletRoundsCounter == 0f)
				{
					SEMgr.Inst.boss6_Roar3.PlaySE();
				}
				face.SetClose();
				nowBlastAngle = blastAngleRange * (UnityEngine.Random.value - 0.5f);
				while (Mathf.Abs(lastBlastAngle - nowBlastAngle) < minBlastAngleInterval)
				{
					nowBlastAngle = blastAngleRange * (UnityEngine.Random.value - 0.5f);
				}
				lastBlastAngle = nowBlastAngle;
				blastBulletRounds.RandomResult();
				base.Anima.Play("Blast", 0, 0f);
				GetNoTargetPoint();
			}
			KeepHeadDistanceFromTarget();
			SetStaticBody(autolHeadRotation: false);
			SetHeadRotate(nowBlastAngle);
			break;
		case MonsterState.BlastReposition:
		{
			ref Vector3 reference6 = ref varMgr.RegV3(0);
			if (changedState)
			{
				reference6 = base.transform.position;
				base.Anima.Play("Idle");
			}
			KeepHeadDistanceFromTarget();
			SetStaticBody(autolHeadRotation: false);
			if (stateExistTime > blastRepositionTime)
			{
				state = MonsterState.Blast;
			}
			break;
		}
		case MonsterState.FreeMoveEnter:
		{
			if (changedState)
			{
				SEMgr.Inst.boss6_Stage2FreeShow.PlaySE();
				as_Wave.Play();
				as_Wave1.Play();
				if (nowFreeMode != FreeMoveMode.Horizontal)
				{
					freeMoveDirection = Vector3.up * GeneralTool.HalfChanceNPOne();
				}
				else
				{
					freeMoveDirection = Vector3.right * GeneralTool.HalfChanceNPOne();
				}
				if (nowFreeMode == FreeMoveMode.Round)
				{
					float x = GeneralTool.HalfChanceNPOne() * rotateRadius;
					base.transform.position = new Vector3(x, (0f - freeMoveDirection.y) * (roomHeight / 2f + enterDistanceY) + roomCenter.y, 0f);
					warningStartPoint = new Vector3(x, roomCenter.y - Mathf.Sign(freeMoveDirection.y) * (roomHeight / 2f - freeMoveBodyWidth), 0f);
					warningEndPoint = new Vector3(x, roomCenter.y + Mathf.Sign(freeMoveDirection.y) * (roomHeight / 2f - freeMoveBodyWidth), 0f);
					roundMoveClockWise = Tool2D.IgnoreZAngleWithSign(freeMoveDirection, ToPointDir(roomCenter)) < 0f;
					rotateAngleSpeed = 360f / (MathF.PI * 2f * rotateRadius / (myPpt.MoveSpeed * roundSpeedRatio));
					nowRotateAngle = 0f;
					startRotateAngle = ((base.transform.position.x > roomCenter.x) ? (-90) : 90);
					bodyAllAngle = (float)bodyCount * (bodyInterval - 1f) / (MathF.PI * 2f * rotateRadius) * 360f;
				}
				else if (nowFreeMode == FreeMoveMode.Vertical)
				{
					base.transform.position = new Vector3(roomCenter.x, roomCenter.y - freeMoveDirection.y * (roomHeight / 2f + enterDistanceY), 0f);
					warningStartPoint = new Vector3(base.transform.position.x, roomCenter.y + roomHeight / 2f, 0f);
					warningEndPoint = new Vector3(base.transform.position.x, roomCenter.y - roomHeight / 2f, 0f);
				}
				else
				{
					float num9 = GeneralTool.HalfChanceNPOne() * (roomHeight / 2f - freeMoveBodyWidth);
					base.transform.position = new Vector3((0f - freeMoveDirection.x) * (roomWidth / 2f + enterDistanceX), roomCenter.y + num9, 0f);
					warningStartPoint = new Vector3(roomCenter.x + roomWidth / 2f, base.transform.position.y, 0f);
					warningEndPoint = new Vector3(roomCenter.x - roomWidth / 2f, base.transform.position.y, 0f);
				}
				for (int j = 0; j < waveWarningLine.positionCount; j++)
				{
					waveWarningLine.SetPosition(j, Tool2D.GetLayerPoint(Vector3.Lerp(warningStartPoint, warningEndPoint, (float)j / (float)(waveWarningLine.positionCount - 1)), LayerCorrectType.GroundEffectLow));
				}
				if (nowFreeMode != 0)
				{
					waveWarningLine.enabled = true;
				}
				else
				{
					waveWarningLine.enabled = false;
				}
				tsf_Body.gameObject.SetActive(value: true);
				for (int k = 0; k < bodys.Count; k++)
				{
					bodys[k].state = Boss6_Body.MonsterState.FreeFollow;
				}
				ResetFreeBody(base.transform.position, freeMoveDirection);
			}
			float num10 = 0f;
			if (nowFreeMode == FreeMoveMode.Round)
			{
				if (freeMoveDirection.y > 0.01f)
				{
					num10 = 0f - (base.transform.position.y - roomCenter.y);
				}
				else if (freeMoveDirection.y < -0.01f)
				{
					num10 = base.transform.position.y - roomCenter.y;
				}
			}
			else if (freeMoveDirection.x > 0.01f)
			{
				num10 = 0f - (base.transform.position.x - (roomCenter.x + roomWidth / 2f));
			}
			else if (freeMoveDirection.x < -0.01f)
			{
				num10 = base.transform.position.x - (roomCenter.x - roomWidth / 2f);
			}
			else if (freeMoveDirection.y > 0.01f)
			{
				num10 = 0f - (base.transform.position.y - (roomCenter.y + roomHeight / 2f));
			}
			else if (freeMoveDirection.y < -0.01f)
			{
				num10 = base.transform.position.y - (roomCenter.y - roomHeight / 2f);
			}
			SetAllHeight(Mathf.Lerp(attackBodyHeight, idleBodyHeight, enterLerpCurve.Evaluate(num10 / enterLerpDistance)));
			FreeMove(Mathf.Lerp((nowFreeMode == FreeMoveMode.Round) ? roundSpeedRatio : 1f, enterSpeedRatio, enterLerpCurve.Evaluate(num10 / enterLerpDistance)));
			if (num10 < 0f)
			{
				state = MonsterState.FreeMove;
			}
			break;
		}
		case MonsterState.FreeMove:
		{
			ref float reference3 = ref varMgr.RegFloat(0);
			ref float reference4 = ref varMgr.RegFloat(1);
			ref float reference5 = ref varMgr.RegFloat(2);
			_ = ref varMgr.RegInt(0);
			if (changedState)
			{
				waveWarningLine.enabled = false;
				reference4 = backBulletInterval;
				reference3 = waveBulletRoundsInterval;
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (nowFreeMode == FreeMoveMode.Round)
			{
				reference5 += Time.deltaTime;
				if (reference5 > roundBulletRoundsInterval)
				{
					reference5 -= roundBulletRoundsInterval;
					ShootAllRoundBullet();
				}
			}
			else if (nowFreeMode == FreeMoveMode.Vertical)
			{
				reference4 += Time.deltaTime;
				if (reference4 > rotateBulletInterval)
				{
					reference4 -= rotateBulletInterval;
					ShootFreeMoveRotateBullet();
				}
			}
			else
			{
				reference3 += Time.deltaTime;
				if (reference3 > waveBulletRoundsInterval)
				{
					reference3 -= waveBulletRoundsInterval;
					ShootAllWaveBullet();
				}
			}
			if (nowFreeMode == FreeMoveMode.Round)
			{
				nowRotateAngle += Time.deltaTime * rotateAngleSpeed;
				freeMoveDirection = Tool2D.GetDir(startRotateAngle + (Mathf.Min(nowRotateAngle, 360f) + 90f) * (float)((!roundMoveClockWise) ? 1 : (-1)));
				if (nowRotateAngle > 360f + bodyAllAngle)
				{
					state = MonsterState.FreeMoveLeave;
				}
			}
			FreeMove((nowFreeMode == FreeMoveMode.Round) ? roundSpeedRatio : 1f);
			if (nowFreeMode != 0 && ((Mathf.Abs(bodys[bodys.Count - 1].transform.position.x - roomCenter.x) < roomWidth / 2f && freeMoveDirection.x != 0f) || (Mathf.Abs(bodys[bodys.Count - 1].transform.position.y - roomCenter.y) < roomHeight / 2f && freeMoveDirection.y != 0f)))
			{
				state = MonsterState.FreeMoveLeave;
			}
			break;
		}
		case MonsterState.FreeMoveLeave:
		{
			if (changedState)
			{
				waveWarningLine.enabled = false;
				for (int m = 0; m < bodys.Count; m++)
				{
					bodys[m].state = Boss6_Body.MonsterState.FreeFollow;
				}
			}
			float num11 = 0f;
			Vector3 position2 = bodys[bodys.Count - 1].transform.position;
			if (freeMoveDirection.x > 0.01f)
			{
				num11 = position2.x - (roomCenter.x - roomWidth / 2f);
			}
			else if (freeMoveDirection.x < -0.01f)
			{
				num11 = 0f - (position2.x - (roomCenter.x + roomWidth / 2f));
			}
			else if (freeMoveDirection.y > 0.01f)
			{
				num11 = position2.y - (roomCenter.y - roomHeight / 2f);
			}
			else if (freeMoveDirection.y < -0.01f)
			{
				num11 = 0f - (position2.y - (roomCenter.y + roomHeight / 2f));
			}
			if (nowFreeMode == FreeMoveMode.Round)
			{
				SetAllHeight(Mathf.Lerp(attackBodyHeight, idleBodyHeight, enterLerpCurve.Evaluate(stateExistTime / roundLeaveTime)));
				FreeMove(Mathf.Lerp((nowFreeMode == FreeMoveMode.Round) ? roundSpeedRatio : 1f, roundLeaveSpeedRatio, enterLerpCurve.Evaluate(stateExistTime / roundLeaveTime)));
			}
			else
			{
				SetAllHeight(Mathf.Lerp(attackBodyHeight, idleBodyHeight, enterLerpCurve.Evaluate(num11 / enterLerpDistance)));
				FreeMove(Mathf.Lerp((nowFreeMode == FreeMoveMode.Round) ? roundSpeedRatio : 1f, leaveSpeedRatio, enterLerpCurve.Evaluate(num11 / enterLerpDistance)));
			}
			if ((Mathf.Abs(position2.x - roomCenter.x) > roomWidth / 2f + enterDistanceX && Mathf.Sign(position2.x - roomCenter.x) == Mathf.Sign(freeMoveDirection.x)) || (Mathf.Abs(position2.y - roomCenter.y) > enterDistanceY + roomHeight / 2f && Mathf.Sign(position2.y - roomCenter.y) == Mathf.Sign(freeMoveDirection.y)))
			{
				state = MonsterState.FreeHide;
			}
			break;
		}
		case MonsterState.FreeHide:
			if (changedState)
			{
				as_Wave.Stop();
				tsf_Body.gameObject.SetActive(value: false);
				for (int l = 0; l < bodys.Count; l++)
				{
					bodys[l].state = Boss6_Body.MonsterState.FreeHide;
				}
			}
			SetMove(Vector3.zero, isFlip: false);
			ChooseSideSkill();
			break;
		case MonsterState.Dead:
			if (changedState)
			{
				base.Anima.Play("Show");
			}
			deadExplodeTimer += Time.deltaTime;
			if (!startDeadExplode && deadExplodeTimer > deadExplodeDelayTime)
			{
				deadExplodeTimer -= deadExplodeDelayTime;
				startDeadExplode = true;
			}
			if (!startDeadExplode || !(deadExplodeTimer > deadExplodeTime))
			{
				break;
			}
			CamController.Inst.SetShock(deadShock);
			deadExplodeTimer -= deadExplodeTime;
			if (bodys.Count > 0)
			{
				bodys[bodys.Count - 1].DotsAnnouncedDeath();
				bodys.RemoveAt(bodys.Count - 1);
				break;
			}
			DotsAnnouncedDeath();
			if (LevelMgr.Inst.CurrentRoomCtrller.IsFinish)
			{
				MusicMgr.Inst.UpdateThemeMusic();
				SEMgr.Inst.bossFinish.PlaySE();
			}
			break;
		case MonsterState.SwitchAfter:
		case MonsterState.HeadDashBefore:
		case MonsterState.HeadDash:
		case MonsterState.HeadDashAfter:
		case MonsterState.SideAttack:
		case MonsterState.StaticHide:
		case MonsterState.FreeMoveCenter:
		case MonsterState.FreeRoundEnter:
		case MonsterState.FreeRound:
		case MonsterState.FreeRoundLeave:
			break;
		}
	}

	private void ShootFreeMoveRotateBullet()
	{
		float num = waveBulletGroupCountVertical.RandomResult();
		waveOffset = !waveOffset;
		_ = (waveOffset ? 0f : 0.5f) / num;
		for (int i = 0; (float)i < num; i++)
		{
			_ = UnityEngine.Random.value / num;
			_ = waveBulletOffsetRange;
			float num2 = (float)i / num;
			if (num2 > 1f)
			{
				num2 -= 1f;
			}
			Vector3 bodyFirePoint = GetBodyFirePoint(num2);
			StartCoroutine(ShootSingleRotateBullet(bodyFirePoint, (upsideDown ? (num - (float)i) : ((float)i)) / num * rotateBulletInterval));
		}
		upsideDown = !upsideDown;
	}

	private IEnumerator ShootSingleRotateBullet(Vector3 startPoint, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		SEMgr.Inst.boss6_SideRotateBulletShoot.PlaySE();
		Vector3 point = Tool2D.IgnoreZPoint(startPoint + Vector3.left * freeMoveBodyWidth);
		Vector3 point2 = Tool2D.IgnoreZPoint(startPoint + Vector3.right * freeMoveBodyWidth);
		Boss6_Bullet component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_RotateBullet" + (GameMgr.IsChAge14_Static ? " H" : ""), point).GetComponent<Boss6_Bullet>();
		Boss6_Bullet component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_RotateBullet" + (GameMgr.IsChAge14_Static ? " H" : ""), point2).GetComponent<Boss6_Bullet>();
		bool flag = startPoint.y < roomCenter.y;
		float targetAngle = ((!flag) ? 180 : 0);
		float rotateSpeed = 90f / (Mathf.Abs(roomCenter.y - startPoint.y) * MathF.PI / 2f / rotateBulletSpeed) / 2f;
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_ShootSideBullet1" + (GameMgr.IsChAge14_Static ? " H" : ""), component.tsf_bulletHead.transform.position, 2f);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_ShootSideBullet1" + (GameMgr.IsChAge14_Static ? " H" : ""), component2.tsf_bulletHead.transform.position, 2f);
		component.InitializeRotate(Vector3.left, rotateBulletSpeed, targetAngle, flag ? 90 : (-90), rotateSpeed, waveBulletLifeTime);
		component2.InitializeRotate(Vector3.right, rotateBulletSpeed, targetAngle, flag ? (-90) : 90, rotateSpeed, waveBulletLifeTime);
	}

	private void ShootHeadBullet(Vector3 dir)
	{
		SEMgr.Inst.boss6_BulletShoot.PlaySE(SEPlayMode.Replay, 3, 0.2f);
		headAttackParticle.Play();
		Vector3 dir2 = Tool2D.GetDir(dir, headBulletOffsetRange.RandomResult());
		float speed = headBulletSpeed.RandomResult();
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_SimpleBullet" + (GameMgr.IsChAge14_Static ? " H" : ""), Tool2D.IgnoreZPoint(tsf_headAttack.position)).GetComponent<Boss6_Bullet>().InitializeSimple(dir2, speed, headBulletLifeTime);
	}

	public void ShootAllWaveBullet()
	{
		float num = ((freeMoveDirection.x != 0f) ? waveBulletGroupCount : waveBulletGroupCountVertical.RandomResult());
		waveOffset = !waveOffset;
		float num2 = (waveOffset ? 0f : 0.5f) / num;
		for (int i = 0; (float)i < num; i++)
		{
			float num3 = UnityEngine.Random.value / num * waveBulletOffsetRange;
			float num4 = (float)i / num + num3 + num2;
			if (num4 > 1f)
			{
				num4 -= 1f;
			}
			Vector3 bodyFirePoint = GetBodyFirePoint(num4);
			StartCoroutine(ShootSingleWaveBullet(bodyFirePoint, UnityEngine.Random.value * waveBulletDelayRange));
		}
	}

	public IEnumerator ShootSingleWaveBullet(Vector3 startPoint, float delayTime)
	{
		bool shootLeft = Tool2D.IgnoreZAngleWithSign(freeMoveDirection, base.transform.position - roomCenter) < 0f;
		yield return new WaitForSeconds(delayTime);
		Vector3 leftDir = Tool2D.GetDir(freeMoveDirection, 90f);
		Vector3 rightDir = Tool2D.GetDir(freeMoveDirection, -90f);
		Vector3 leftStartPoint = Tool2D.IgnoreZPoint(startPoint + leftDir * freeMoveBodyWidth);
		Vector3 rightStartPoint = Tool2D.IgnoreZPoint(startPoint + rightDir * freeMoveBodyWidth);
		float bulletCount = waveBulletCount.RandomResult();
		float startPhase = (float)((!(UnityEngine.Random.value > 0.5f)) ? 1 : 0) * MathF.PI;
		SEMgr.Inst.boss6_SideBulletShoot.PlaySE(SEPlayMode.Replay, 3, 0.6f);
		for (int i = 0; (float)i < bulletCount; i++)
		{
			if (shootLeft)
			{
				Boss6_Bullet component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_WaveBullet" + (GameMgr.IsChAge14_Static ? " H" : ""), leftStartPoint).GetComponent<Boss6_Bullet>();
				component.InitializeWave(leftDir, waveBulletSpeed, waveBulletFrequency, waveBulletAmplitude, startPhase, waveBulletLifeTime);
				if (i == 0)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_ShootSideBullet" + (GameMgr.IsChAge14_Static ? " H" : ""), component.transform.position + new Vector3(0f, 0f, 0.001f) + leftDir * 0.3f, 2f);
				}
			}
			else
			{
				Boss6_Bullet component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_WaveBullet" + (GameMgr.IsChAge14_Static ? " H" : ""), rightStartPoint).GetComponent<Boss6_Bullet>();
				component2.InitializeWave(rightDir, waveBulletSpeed, waveBulletFrequency, waveBulletAmplitude, startPhase, waveBulletLifeTime);
				if (i == 0)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_ShootSideBullet" + (GameMgr.IsChAge14_Static ? " H" : ""), component2.transform.position + new Vector3(0f, 0f, 0.001f) + rightDir * 0.3f, 2f);
				}
			}
			yield return new WaitForSeconds(waveBulletInterval);
		}
	}

	private void ShootAllRoundBullet()
	{
		float num = roundBulletGroupCount.RandomResult();
		float num2 = UnityEngine.Random.value / num;
		for (int i = 0; (float)i < num; i++)
		{
			float num3 = (float)i / num + num2;
			if (num3 > 1f)
			{
				num3 -= 1f;
			}
			StartCoroutine(ShootSingleRoundBullet(num3, UnityEngine.Random.value * roundBulletDelayRange));
		}
	}

	private IEnumerator ShootSingleRoundBullet(float percent, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		if (!CheckCanFireRound(percent))
		{
			yield break;
		}
		Vector3 bodyFirePointRound = GetBodyFirePointRound(percent);
		Vector3 normalized = (roomCenter - bodyFirePointRound).normalized;
		Vector3 finalShootPoint = bodyFirePointRound + normalized * freeMoveBodyWidth;
		Vector3 finalShootDir = Tool2D.GetDir(normalized, roundBulletAngleRange.RandomResult());
		float startPhase = (float)((!(UnityEngine.Random.value > 0.5f)) ? 1 : 0) * MathF.PI;
		int roundBulletCount = this.roundBulletCount.RandomResult();
		SEMgr.Inst.boss6_SideBulletShoot.PlaySE(SEPlayMode.Replay, 3, 0.6f);
		for (int i = 0; i < roundBulletCount; i++)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_WaveBullet" + (GameMgr.IsChAge14_Static ? " H" : ""), finalShootPoint).GetComponent<Boss6_Bullet>().InitializeWave(finalShootDir, roundBulletSpeed, roundBulletFrequency, roundBulletAmplitude, startPhase, waveBulletLifeTime);
			if (i == 0)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_ShootSideBullet" + (GameMgr.IsChAge14_Static ? " H" : ""), finalShootPoint + new Vector3(0f, 0f, 0.001f), 2f);
			}
			yield return new WaitForSeconds(roundBulletInterval);
		}
	}

	private void ShootBlastBullet()
	{
		blastBulletCount.RandomResult();
		SEMgr.Inst.boss6_Cannon.PlaySE();
		SEMgr.Inst.boss6_Cannon1.PlaySE();
		if (blastBulletCount.result == 1)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_Blast" + (GameMgr.IsChAge14_Static ? " H" : ""), Tool2D.IgnoreZPoint(tsf_headAttack.position)).GetComponent<Boss6_Blast>().Initialize(Tool2D.GetDir(staticFaceDirection, nowBlastAngle), this);
		}
	}

	public override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		GameUISingletonMono<UIBossHP>.HideIfInited();
		explodeBulletChargeParticle.Stop();
		explodeBulletChargeParticle.Clear();
		info.stopAnnouncedDeath = true;
		if (bossDeadStay)
		{
			return;
		}
		state = MonsterState.Dead;
		base.Anima.Play("Show");
		bossDeadStay = true;
		base.Rigid.isKinematic = true;
		SyncDotsPosition();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		face.SetClose();
		myPpt.ChangeColor(Color.white);
		for (int i = 0; i < bodys.Count; i++)
		{
			if (isFreeMode)
			{
				bodys[i].state = Boss6_Body.MonsterState.FreeFollow;
			}
			bodys[i].SetColor(myPpt.BaseColor);
		}
		for (int j = 0; j < bodys.Count; j++)
		{
			bodys[j].SetDead();
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		CamController.Inst.ClearSpecificConstraint();
		CamController.Inst.ClearExtraCameraFocusRequirement();
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (state == MonsterState.BornIdle || state == MonsterState.Dead)
		{
			info.immuneDamage = true;
			return;
		}
		Entity entity = info.spell.Entity;
		if (!(entity != Entity.Null))
		{
			return;
		}
		SpellAbilityType abilityType = info.spell.Config.AbilityType;
		if (abilityType == SpellAbilityType.PullForceCrystal)
		{
			return;
		}
		if (!hitList.Contains(info.spell.Entity))
		{
			switch (abilityType)
			{
			case SpellAbilityType.RuneHammer:
				if (info.spell.Movement.Type == SpellSpecialMovementType.ChaseMouse)
				{
					hitTime.Add(0.45f);
					hitList.Add(entity);
				}
				else
				{
					hitList.Add(entity);
					hitTime.Add(0.45f);
				}
				break;
			case SpellAbilityType.BiAnLethalBlade:
				hitList.Add(entity);
				hitTime.Add(infinitePierceSpellClearTime);
				break;
			case SpellAbilityType.MagicBreaker:
				hitTime.Add(breakerClearTime);
				hitList.Add(entity);
				break;
			case SpellAbilityType.Dash:
				hitTime.Add(0.3f);
				hitList.Add(entity);
				break;
			case SpellAbilityType.ArcaneExplosion:
				hitTime.Add(0.35f);
				hitList.Add(entity);
				break;
			case SpellAbilityType.DimensionTraveller:
				hitTime.Add(0.3f);
				hitList.Add(entity);
				break;
			case SpellAbilityType.Laser:
			case SpellAbilityType.DeathAdder:
			case SpellAbilityType.LaserBeam:
				hitList.Add(entity);
				hitTime.Add(0.01f);
				instantHitList.Add(entity);
				instantHitCount.Add(1f);
				break;
			default:
				if (info.spell.Config.Penetrate.Calculate() > 1)
				{
					hitList.Add(entity);
					hitTime.Add(pierceSpellClearTime);
				}
				else
				{
					hitList.Add(entity);
					hitTime.Add(0.01f);
				}
				break;
			case SpellAbilityType.DisintegrationRay:
			case SpellAbilityType.ThunderAura:
			case SpellAbilityType.DragonBreath:
				hitList.Add(entity);
				hitTime.Add(normalSpellClearTime);
				break;
			}
			return;
		}
		switch (abilityType)
		{
		case SpellAbilityType.Laser:
		case SpellAbilityType.LaserBeam:
		{
			if (!info.spell.Movement.IsFallSpell)
			{
				break;
			}
			int num = instantHitList.IndexOf(entity);
			if (num >= 0)
			{
				instantHitCount[num]++;
				if (instantHitCount[num] <= (float)info.spell.Movement.ReboundCount)
				{
					return;
				}
			}
			break;
		}
		default:
		{
			if (UnitDotsSyncSystem.TryGetComponent<SpellConfigComponentData>(info.spell.Entity, out var result))
			{
				result.Penetrate.Base++;
				SetComponentData(result, info.spell.Entity);
			}
			break;
		}
		case SpellAbilityType.SnakeWalk:
		case SpellAbilityType.DisintegrationRay:
		case SpellAbilityType.Dash:
		case SpellAbilityType.ThunderAura:
		case SpellAbilityType.DragonBreath:
		case SpellAbilityType.SuperNova:
			break;
		}
		info.immuneDamage = true;
	}

	public override void AnimaAction(string animaName)
	{
		if (bossDeadStay && animaName != "ShoutParticlePlay" && animaName != "ShoutParticleStop")
		{
			return;
		}
		switch (animaName)
		{
		case "DashShoot":
			break;
		case "Blast":
			face.SetOpen();
			ShootBlastBullet();
			break;
		case "BlastFinish":
			blastBulletRoundsCounter += 1f;
			if (blastBulletRoundsCounter >= (float)blastBulletRounds.result)
			{
				blastBulletRoundsCounter = 0f;
				SetAfterAttack(blastRecoverTime, getUp: true);
			}
			else
			{
				state = MonsterState.BlastReposition;
			}
			break;
		case "DashBeforeFinish":
			state = MonsterState.HeadDash;
			break;
		case "DashFinish":
			state = MonsterState.HeadDashAfter;
			break;
		case "DashAfterFinish":
			if (headDashRoundsCounter >= headDashRounds)
			{
				headDashRoundsCounter = 0f;
				SetAfterAttack(headDashRecoverTime);
			}
			else
			{
				state = MonsterState.HeadDashBefore;
			}
			break;
		case "ShoutParticlePlay":
			SEMgr.Inst.boss6_Dead.PlaySE();
			face.SetOpenContinue();
			ShootParticle.Play();
			CamController.Inst.SetShock(bornIdleShock);
			break;
		case "ShoutParticleStop":
			face.SetIdle();
			ShootParticle.Stop();
			break;
		case "ShootExplodeBullet":
			state = MonsterState.ExplodeBulletAfter;
			break;
		case "SideShout":
			SideShootParticle.Play();
			CamController.Inst.SetShock(sideShock);
			break;
		case "SideShoutParticleStop":
			SideShootParticle.Stop();
			break;
		}
	}
}
