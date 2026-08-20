using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Boss4 : UnitBase
{
	public enum UnitState
	{
		BornIdle,
		RandomFly,
		CurseAttack,
		Rotate,
		ReverseBeamWarning,
		ReverseBeaming,
		ReverseBeamAfter,
		ChangeToStage2,
		ScreamBefore,
		ScreamDuration,
		Stage3FlyToCenter,
		Stage3Bellow,
		Stage3ReverseBeamBefore,
		Stage3ReverseBeam,
		Stage3CurseAttack,
		Stage3SpitLeech
	}

	[Space(50f)]
	public Transform tsf_Motion;

	public Transform tsf_HairParent;

	public Transform tsf_Hover;

	public float initialHeight;

	public VariableFloat trunInterval;

	public VariableFloat actionInterval;

	[Header("Hover")]
	public float hoverSpeed;

	public float hoverScale;

	[Header("Hair")]
	public Boss4_Hair hair;

	public int hairCount;

	[Header("EyeControl")]
	public VariableFloat eyeRepositionInterval;

	public VariableFloat eyeRepositionRange;

	public float eyeRepositionLerp;

	[Header("Wing")]
	public Boss4_Wing wingL;

	public Boss4_Wing wingR;

	[Header("CurseAttack")]
	public Transform tsf_CurseAttackEFCharge;

	public Transform tsf_CurseAttackEFChargeFinish;

	public VariableInt curseAttackStage1Count;

	public VariableInt curseAttackStage2Count;

	public float curseAttackAngle;

	public float curseAttackEFOffset;

	public float curseAttackRecoil;

	[Header("RotateLeech")]
	[Range(0f, 1f)]
	public float rotateLeechChance;

	public int rotateLeechID;

	public int rotateChildCount;

	public float rotateChildOffset;

	public float rotateChildHight;

	[Header("ReverseBeam")]
	public VariableInt actionTimeToReverseBeam;

	public Boss4_ReverseBeam pfb_ReverseBeam;

	public float reverseBeamHeightLerp;

	public float reverseBeamBeforeThreshold;

	public float reverseBeamDuration;

	public float reverseBeamRotateSpeed;

	public float reverseBeamMoveRatio;

	[Header("ChangeState")]
	public float changeStateFlyHeight;

	public float stage2ShockDuration;

	public float shockRadius;

	public float shockSpeed;

	public float shockRadiusIntence;

	public float shockSpeedIntence;

	public float stage3ShoutWaveRadius;

	public int stage3ShoutWaveDamage;

	public float stage3ShoutWaveInterval;

	public float stage3ShoutWaveKnockback;

	public ParticleSystem stage3ShoutEF;

	[Range(0f, 1f)]
	[Header("Stage2")]
	public float stage2HPRatio;

	public float stage2ChangeTime;

	public float stage2FlyHeight;

	public float stage2MoveRatio;

	[Header("SummonTentacle")]
	public int summonTentacleID;

	public VariableFloat summonTentacleInterval;

	public VariableInt summonTentacleCount;

	public int summonTentacleMaxCount;

	[Header("Scream")]
	[Range(0f, 1f)]
	public float screamChance;

	public float screamBeforeTime;

	public float screamMoveRatio;

	public float screamHoverSpeed;

	public float screamDuration;

	public float screamFlyHeight;

	public float screamShootInterval;

	public int screamShootCount;

	public float screamSpellDuration;

	public int screamSpellDamage;

	[Range(0f, 1f)]
	[Header("Stage3")]
	public float stage3HPRatio;

	public Vector3 stage3BleedingOffset;

	public float stage3BleedingInterval;

	public float stage3BleedingDuration;

	public float stage3BellowDuration;

	public float stage3FlyHeight;

	[Header("Stage3CurseAttack")]
	[Range(0f, 1f)]
	public float stage3CurseAttackChance;

	public VariableInt stage3CurseAttackCount;

	public float stage3CurseAttackOffset;

	[Header("SpitLeech")]
	[Range(0f, 1f)]
	public float spitLeechChance;

	public int spitLeechID;

	public VariableInt spitLeechCount;

	public float spitLeechSelfHeight;

	public float spitLeechHeight;

	public int stage3LeechMaxCount;

	[Header("HairBlink")]
	public VariableFloat hairBlinkInterval;

	[Header("SE")]
	public AudioSource as_BodyLoop;

	public AudioSource as_BeamCharge;

	public AudioSource as_BeamDuration;

	public AudioSource as_Scream;

	public AudioSource as_Bellow;

	public AudioSource as_Bellow2;

	public AudioSource as_Bellow3;

	public AudioSource as_ToStage2;

	private List<Boss4_Leech> allLeeches = new List<Boss4_Leech>();

	private List<Boss4_Tentacle> allTentacles = new List<Boss4_Tentacle>();

	private SpellSpawnParams ssp;

	private float dynamicMoveRatio = 1f;

	public StateVariableMgr varMgr = new StateVariableMgr();

	public UnitState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private Boss4_Hair[] hairs;

	private Boss4_Wing[] wings;

	private Vector3 randomFlyDir;

	private float flyHeight;

	private float currentHoverSpeed;

	private int actionTimer;

	private float actionIntervalTimer;

	private Boss4_ReverseBeam reverseBeam;

	private float hoverTimer;

	private Vector2 eyeTargetPoint;

	private Vector2 eyeCurrentPoint;

	private float eyeRepositionIntervalTimer;

	private float summonTentacleIntervalTimer;

	private int summonTentacleCounter;

	private int stage3LeechMaxCounter;

	private float hairBlinkIntervalTimer;

	[Header("和谐模式")]
	public List<AnimationClip> harmonyAnimations = new List<AnimationClip>();

	public SpriteRenderer SR_Eyeball1;

	public SpriteRenderer SR_BG;

	public SpriteRenderer SR_EyeSocket;

	public Sprite sprite_Eyeball1H;

	public Sprite sprite_BGH;

	public Sprite sprite_EyeSocketH;

	public Boss4Stage BossStage { get; private set; }

	public MiniObjPool MiniPool { get; private set; }

	public Vector3 ReverseBeamDir { get; private set; }

	private float FinalMoveSpeed => base.MoveSpeed * dynamicMoveRatio;

	public UnitState state
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
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		as_BodyLoop.volume = DataMgr.settingData.GetFinalSound();
		as_BeamCharge.volume = DataMgr.settingData.GetFinalSound();
		as_BeamDuration.volume = DataMgr.settingData.GetFinalSound();
		as_Scream.volume = DataMgr.settingData.GetFinalSound();
		as_Bellow.volume = DataMgr.settingData.GetFinalSound();
		as_Bellow2.volume = DataMgr.settingData.GetFinalSound();
		as_Bellow3.volume = DataMgr.settingData.GetFinalSound();
		as_ToStage2.volume = DataMgr.settingData.GetFinalSound();
		wings[0].as_WingFlap.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Start()
	{
		MiniPool = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool"), base.transform.parent).GetComponent<MiniObjPool>();
		hairs = new Boss4_Hair[hairCount];
		for (int i = 0; i < hairCount; i++)
		{
			hairs[i] = UnityEngine.Object.Instantiate(hair, tsf_HairParent);
			hairs[i].Initialize(this);
		}
		wings = new Boss4_Wing[4];
		wings[0] = UnityEngine.Object.Instantiate(wingL, tsf_HairParent).GetComponent<Boss4_Wing>();
		wings[1] = UnityEngine.Object.Instantiate(wingL, tsf_HairParent).GetComponent<Boss4_Wing>();
		wings[2] = UnityEngine.Object.Instantiate(wingR, tsf_HairParent).GetComponent<Boss4_Wing>();
		wings[3] = UnityEngine.Object.Instantiate(wingR, tsf_HairParent).GetComponent<Boss4_Wing>();
		wings[0].Initialize(this, Tool2D.GetDir(70f), isLeft: true, isIndex0: true);
		wings[1].Initialize(this, Tool2D.GetDir(110f), isLeft: true, isIndex0: false);
		wings[2].Initialize(this, Tool2D.GetDir(240f), isLeft: false, isIndex0: false);
		wings[3].Initialize(this, Tool2D.GetDir(290f), isLeft: false, isIndex0: false);
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f - initialHeight);
		reverseBeam = UnityEngine.Object.Instantiate(pfb_ReverseBeam, base.transform.position, Quaternion.identity, base.transform.parent);
		reverseBeam.Initialize(this);
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90042);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Duration = screamSpellDuration;
		sSPModifier.Damage = screamSpellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		flyHeight = initialHeight;
		currentHoverSpeed = hoverSpeed;
		trunInterval.RandomResult();
		actionInterval.RandomResult();
		actionTimeToReverseBeam.RandomResult();
		eyeRepositionInterval.RandomResult();
		summonTentacleInterval.RandomResult();
		hairBlinkInterval.RandomResult();
		SoundVolumeChange();
		allLeeches.Clear();
		allTentacles.Clear();
		if (GameMgr.IsHarmony_Static)
		{
			AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(base.Anima.runtimeAnimatorController);
			base.Anima.runtimeAnimatorController = animatorOverrideController;
			for (int j = 0; j < harmonyAnimations.Count; j++)
			{
				string text = harmonyAnimations[j].name.Substring(0, harmonyAnimations[j].name.Length - 2);
				if (animatorOverrideController[text] != null)
				{
					animatorOverrideController[text] = harmonyAnimations[j];
				}
			}
			SR_BG.sprite = sprite_BGH;
			SR_Eyeball1.sprite = sprite_Eyeball1H;
			SR_EyeSocket.sprite = sprite_EyeSocketH;
		}
		if (GameMgr.IsMobile_Static)
		{
			screamShootCount -= 2;
			curseAttackStage2Count.value1 -= 2;
			curseAttackStage2Count.value2 -= 2;
			ref int value = ref stage3CurseAttackCount.value1;
			value = value;
			stage3CurseAttackCount.value2--;
			reverseBeamRotateSpeed *= 0.9f;
		}
		SyncDotsPosition();
	}

	private void CheckAction()
	{
		actionIntervalTimer += Time.deltaTime;
		if (!(actionIntervalTimer >= actionInterval.result))
		{
			return;
		}
		actionIntervalTimer = 0f;
		actionInterval.RandomResult();
		if (actionTimer >= actionTimeToReverseBeam.result)
		{
			actionTimer = 0;
			if (BossStage == Boss4Stage.Stage3)
			{
				state = UnitState.Stage3ReverseBeamBefore;
			}
			else
			{
				state = UnitState.ReverseBeamWarning;
			}
			return;
		}
		actionTimer++;
		switch (BossStage)
		{
		case Boss4Stage.Stage1:
			if (UnityEngine.Random.value <= rotateLeechChance)
			{
				state = UnitState.Rotate;
			}
			else
			{
				state = UnitState.CurseAttack;
			}
			break;
		case Boss4Stage.Stage2:
			if (UnityEngine.Random.value <= screamChance)
			{
				state = UnitState.ScreamBefore;
			}
			else
			{
				state = UnitState.CurseAttack;
			}
			break;
		case Boss4Stage.Stage3:
		{
			float num = UnityEngine.Random.value;
			if (stage3LeechMaxCounter >= stage3LeechMaxCount)
			{
				num = UnityEngine.Random.Range(spitLeechChance, 1f);
			}
			if (num < spitLeechChance)
			{
				state = UnitState.Stage3SpitLeech;
			}
			else if (num <= spitLeechChance + stage3CurseAttackChance)
			{
				state = UnitState.Stage3CurseAttack;
			}
			else
			{
				state = UnitState.ScreamBefore;
			}
			break;
		}
		default:
			Debug.LogError(BossStage);
			break;
		}
	}

	private void CheckChangeStage()
	{
		switch (BossStage)
		{
		case Boss4Stage.Stage1:
			if (base.CurrentHPRatio <= stage2HPRatio)
			{
				state = UnitState.ChangeToStage2;
			}
			break;
		case Boss4Stage.Stage2:
			if (base.CurrentHPRatio <= stage3HPRatio)
			{
				state = UnitState.Stage3FlyToCenter;
			}
			break;
		default:
			Debug.LogError(BossStage);
			break;
		case Boss4Stage.Stage3:
			break;
		}
	}

	public override void Update()
	{
		if (base.deadStayed)
		{
			tsf_HairParent.localPosition = tsf_Motion.localPosition + tsf_Hover.localPosition + myPpt.Tsf_BeHit.localPosition;
			tsf_HairParent.rotation = tsf_Motion.rotation;
			tsf_HairParent.localScale = tsf_Motion.localScale;
			return;
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
		if (state == UnitState.ReverseBeamWarning || state == UnitState.ReverseBeaming || state == UnitState.ReverseBeamAfter || (BossStage == Boss4Stage.Stage3 && state != UnitState.ScreamBefore && state != UnitState.ScreamDuration))
		{
			tsf_Hover.localPosition = Vector3.Lerp(tsf_Hover.localPosition, Vector3.zero, reverseBeamHeightLerp * Time.deltaTime);
			hoverTimer = 0f;
		}
		else
		{
			hoverTimer += currentHoverSpeed * Time.deltaTime;
			tsf_Hover.localPosition = new Vector3(0f, Mathf.Sin(hoverTimer) * hoverScale, 0f);
		}
		if (state == UnitState.RandomFly && BossStage != Boss4Stage.Stage3)
		{
			eyeRepositionIntervalTimer += Time.deltaTime;
			if (eyeRepositionIntervalTimer >= eyeRepositionInterval.result)
			{
				eyeRepositionIntervalTimer = 0f;
				eyeRepositionInterval.RandomResult();
				eyeTargetPoint = Tool2D.GetDir() * eyeRepositionRange.RandomResult();
			}
		}
		eyeCurrentPoint = Vector3.Lerp((Vector3)eyeCurrentPoint, (Vector3)eyeTargetPoint, eyeRepositionLerp * Time.deltaTime);
		base.Anima.SetFloat("EyeBallX", eyeCurrentPoint.x);
		base.Anima.SetFloat("EyeBallY", eyeCurrentPoint.y);
		base.transform.position = Vector3.Lerp(base.transform.position, Tool2D.IgnoreZPoint(base.transform, 0f - flyHeight), reverseBeamHeightLerp * Time.deltaTime);
		SyncDotsPosition();
		if (BossStage != 0 && summonTentacleCounter < summonTentacleMaxCount)
		{
			summonTentacleIntervalTimer += Time.deltaTime;
			if (summonTentacleIntervalTimer >= summonTentacleInterval.result)
			{
				summonTentacleIntervalTimer = 0f;
				summonTentacleInterval.RandomResult();
				summonTentacleCount.RandomResult();
				summonTentacleCounter += summonTentacleCount.result;
				RoomConfig roomCfg = LevelMgr.Inst.CurrentRoomCtrller.roomCfg;
				for (int i = 0; i < summonTentacleCount.result; i++)
				{
					Vector3 point = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(UnityEngine.Random.Range(-roomCfg.theme6Width / 2, roomCfg.theme6Width / 2), UnityEngine.Random.Range(-roomCfg.theme6Height / 2, roomCfg.theme6Height / 2), 0f);
					ClearDeadSummonsInList();
					Boss4_Tentacle component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + summonTentacleID, point).GetComponent<Boss4_Tentacle>();
					allTentacles.Add(component);
					component.SetMother(this);
				}
			}
		}
		if (BossStage == Boss4Stage.Stage3 && state == UnitState.RandomFly)
		{
			hairBlinkIntervalTimer += Time.deltaTime;
			if (hairBlinkIntervalTimer >= hairBlinkInterval.result)
			{
				hairBlinkIntervalTimer = 0f;
				hairBlinkInterval.RandomResult();
				for (int j = 0; j < hairs.Length; j++)
				{
					hairs[j].Blink();
				}
			}
		}
		tsf_HairParent.localPosition = tsf_Motion.localPosition + tsf_Hover.localPosition + myPpt.Tsf_BeHit.localPosition;
		tsf_HairParent.rotation = tsf_Motion.rotation;
		tsf_HairParent.localScale = tsf_Motion.localScale;
		switch (state)
		{
		case UnitState.BornIdle:
			SetMove(Vector3.zero);
			if (stateExistTime >= 0.5f)
			{
				state = UnitState.RandomFly;
			}
			break;
		case UnitState.RandomFly:
			if (changedState)
			{
				switch (BossStage)
				{
				case Boss4Stage.Stage1:
					flyHeight = initialHeight;
					dynamicMoveRatio = 1f;
					break;
				case Boss4Stage.Stage2:
					flyHeight = stage2FlyHeight;
					dynamicMoveRatio = stage2MoveRatio;
					break;
				case Boss4Stage.Stage3:
					flyHeight = stage3FlyHeight;
					dynamicMoveRatio = stage2MoveRatio;
					break;
				default:
					Debug.LogError(BossStage);
					flyHeight = initialHeight;
					break;
				}
				if (BossStage == Boss4Stage.Stage3)
				{
					base.Anima.SetTrigger("EyeStage3Idle");
				}
				currentHoverSpeed = hoverSpeed;
				randomFlyDir = Tool2D.GetDir();
			}
			SetMove(randomFlyDir * FinalMoveSpeed);
			if (stateExistTime >= trunInterval.result)
			{
				trunInterval.RandomResult();
				randomFlyDir = Tool2D.GetDir();
			}
			CheckAction();
			CheckChangeStage();
			break;
		case UnitState.CurseAttack:
			if (changedState)
			{
				base.Anima.SetTrigger("BodyCurseAttack2");
				SEMgr.Inst.boss4_CurseCharge.PlaySE();
			}
			SetMove(Vector3.zero);
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				eyeTargetPoint = ToTargetDir();
				tsf_CurseAttackEFCharge.position = Tool2D.GetLayerPoint(base.transform.position + tsf_Motion.localPosition + (Vector3)eyeTargetPoint * curseAttackEFOffset);
				tsf_CurseAttackEFChargeFinish.position = tsf_CurseAttackEFCharge.position;
			}
			break;
		case UnitState.Rotate:
			if (changedState)
			{
				eyeRepositionIntervalTimer = 0f;
				eyeTargetPoint.x = 0f;
				eyeTargetPoint.y = 0f;
				if (UnityEngine.Random.Range(0, 2) == 0)
				{
					base.Anima.SetTrigger("BodyRotateL");
				}
				else
				{
					base.Anima.SetTrigger("BodyRotateR");
				}
				SEMgr.Inst.boss4_RotateLeech.PlaySE();
			}
			SetMove(randomFlyDir * FinalMoveSpeed);
			break;
		case UnitState.ReverseBeamWarning:
			if (changedState)
			{
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					ReverseBeamDir = ToTargetDir();
					eyeTargetPoint = ReverseBeamDir;
				}
				base.Anima.SetTrigger("BodyReverseBeamWarning");
				reverseBeam.Warning();
				flyHeight = 0f;
				dynamicMoveRatio = reverseBeamMoveRatio;
				as_BeamCharge.Play();
			}
			SetMove(ToPointDir(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint) * FinalMoveSpeed);
			if ((base.transform.position - Tool2D.IgnoreZPoint(base.transform)).sqrMagnitude <= reverseBeamBeforeThreshold * reverseBeamBeforeThreshold)
			{
				state = UnitState.ReverseBeaming;
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				ReverseBeamDir = Tool2D.DirMoveTowards(ReverseBeamDir, ToTargetDir(), reverseBeamRotateSpeed * Time.deltaTime);
				eyeTargetPoint = ReverseBeamDir;
			}
			break;
		case UnitState.ReverseBeaming:
			if (changedState)
			{
				base.Anima.SetTrigger("BodyReverseBeam");
				reverseBeam.Open();
				as_BeamCharge.Stop();
				as_BeamDuration.Play();
			}
			SetMove(ToPointDir(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint) * FinalMoveSpeed);
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				ReverseBeamDir = Tool2D.DirMoveTowards(ReverseBeamDir, ToTargetDir(), reverseBeamRotateSpeed * Time.deltaTime);
				eyeTargetPoint = ReverseBeamDir;
			}
			if (stateExistTime >= reverseBeamDuration)
			{
				state = UnitState.ReverseBeamAfter;
			}
			break;
		case UnitState.ReverseBeamAfter:
			if (changedState)
			{
				reverseBeam.Close();
				base.Anima.SetTrigger("BodyIdle");
				switch (BossStage)
				{
				case Boss4Stage.Stage1:
					flyHeight = initialHeight;
					break;
				case Boss4Stage.Stage2:
					flyHeight = stage2FlyHeight;
					break;
				default:
					Debug.LogError(BossStage);
					flyHeight = initialHeight;
					break;
				}
			}
			SetMove(randomFlyDir * FinalMoveSpeed);
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				ReverseBeamDir = Tool2D.DirMoveTowards(ReverseBeamDir, ToTargetDir(), reverseBeamRotateSpeed * Time.deltaTime);
				eyeTargetPoint = ReverseBeamDir;
			}
			base.transform.position = Vector3.Lerp(base.transform.position, Tool2D.IgnoreZPoint(base.transform, 0f - initialHeight), reverseBeamHeightLerp * Time.deltaTime);
			if ((base.transform.position - Tool2D.IgnoreZPoint(base.transform, 0f - initialHeight)).sqrMagnitude <= reverseBeamBeforeThreshold * reverseBeamBeforeThreshold)
			{
				state = UnitState.RandomFly;
			}
			break;
		case UnitState.ChangeToStage2:
			if (changedState)
			{
				BossStage = Boss4Stage.Stage2;
				for (int n = 0; n < hairs.Length; n++)
				{
					hairs[n].ChangeStageStart();
				}
				for (int num = 0; num < wings.Length; num++)
				{
					wings[num].Appear();
				}
				base.Anima.SetTrigger("EyeStage2");
				eyeTargetPoint = Vector2.zero;
				flyHeight = changeStateFlyHeight;
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.InvincibleRegister();
				SetComponentData(componentData);
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
			}
			SetMove(ToPointDir(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint) * FinalMoveSpeed);
			if (stateExistTime >= stage2ChangeTime)
			{
				for (int num2 = 0; num2 < hairs.Length; num2++)
				{
					hairs[num2].ChangeStageEnd();
				}
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.InvincibleUnregister();
				SetComponentData(componentData2);
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				state = UnitState.RandomFly;
			}
			break;
		case UnitState.ScreamBefore:
			if (changedState)
			{
				eyeRepositionIntervalTimer = 0f;
				eyeTargetPoint.x = 0f;
				eyeTargetPoint.y = 0f;
				for (int k = 0; k < wings.Length; k++)
				{
					wings[k].SetQuickMotion();
				}
				currentHoverSpeed = screamHoverSpeed;
				dynamicMoveRatio = screamMoveRatio;
				flyHeight = screamFlyHeight;
			}
			SetMove(ToPointDir(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint) * FinalMoveSpeed);
			if (stateExistTime >= screamBeforeTime)
			{
				state = UnitState.ScreamDuration;
			}
			break;
		case UnitState.ScreamDuration:
		{
			ref float reference2 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				if (BossStage == Boss4Stage.Stage3)
				{
					base.Anima.SetTrigger("EyeStage3Scream");
				}
				reference2 = screamShootInterval;
			}
			SetMove(Vector3.zero);
			reference2 += Time.deltaTime;
			if (reference2 >= screamShootInterval)
			{
				reference2 = 0f;
				float num4 = UnityEngine.Random.Range(0f, 360f);
				UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
				for (int num5 = 0; num5 < screamShootCount; num5++)
				{
					sSPModifier.Direction = Tool2D.GetDir(num4 + (float)(360 / screamShootCount * num5));
					sSPModifier.SpawnPosition = Tool2D.IgnoreZPoint(base.transform.position);
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
				}
				as_Scream.Play();
			}
			if (stateExistTime >= screamDuration)
			{
				for (int num6 = 0; num6 < wings.Length; num6++)
				{
					wings[num6].SetNormalMotion();
				}
				state = UnitState.RandomFly;
			}
			break;
		}
		case UnitState.Stage3FlyToCenter:
		{
			ref float reference3 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				CamController.Inst.SetShock(shockRadius, shockSpeed, stage3BleedingDuration);
				BossStage = Boss4Stage.Stage3;
				eyeTargetPoint = Vector2.zero;
				base.Anima.SetTrigger("EyeStage2Dead");
				flyHeight = changeStateFlyHeight;
				UnitProperty_Dots componentData4 = GetComponentData<UnitProperty_Dots>();
				componentData4.InvincibleRegister();
				SetComponentData(componentData4);
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
			}
			SetMove(ToPointDir(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint) * FinalMoveSpeed);
			reference3 += Time.deltaTime;
			if (reference3 >= stage3BleedingInterval)
			{
				reference3 = 0f;
				if (!GameMgr.IsHarmony_Static)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss4_Bleeding", base.transform.position + new Vector3(UnityEngine.Random.Range(0f - stage3BleedingOffset.x, stage3BleedingOffset.x), UnityEngine.Random.Range(0f - stage3BleedingOffset.y, stage3BleedingOffset.y), stage3BleedingOffset.z), 1f);
				}
				SEMgr.Inst.bossDeadShow.PlaySE();
			}
			if (stateExistTime >= stage3BleedingDuration)
			{
				state = UnitState.Stage3Bellow;
				stage3ShoutEF.Play();
			}
			break;
		}
		case UnitState.Stage3Bellow:
		{
			ref float reference = ref varMgr.RegFloat(0);
			if (changedState)
			{
				CamController.Inst.SetShock(shockRadiusIntence, shockSpeedIntence, stage3BellowDuration);
				base.Anima.SetTrigger("EyeStage3Bellow");
				for (int num3 = 0; num3 < hairs.Length; num3++)
				{
					hairs[num3].GrowEye();
				}
				as_Bellow.Play();
			}
			reference += Time.deltaTime;
			if (reference > stage3ShoutWaveInterval)
			{
				reference = 0f;
				ShoutDamage();
			}
			if (stateExistTime >= stage3BellowDuration)
			{
				stage3ShoutEF.Stop();
				UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
				componentData3.InvincibleUnregister();
				SetComponentData(componentData3);
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				state = UnitState.RandomFly;
			}
			break;
		}
		case UnitState.Stage3ReverseBeamBefore:
			if (changedState)
			{
				for (int num7 = 0; num7 < hairs.Length; num7++)
				{
					hairs[num7].ReverseBeamWarning();
				}
				as_Bellow3.Play();
			}
			SetMove(Vector3.zero);
			if (stateExistTime >= reverseBeamDuration)
			{
				state = UnitState.Stage3ReverseBeam;
			}
			break;
		case UnitState.Stage3ReverseBeam:
			if (changedState)
			{
				for (int l = 0; l < hairs.Length; l++)
				{
					hairs[l].ReverseBeaming();
				}
				reverseBeam.Open();
				as_BeamDuration.Play();
			}
			SetMove(Vector3.zero);
			if (stateExistTime >= reverseBeamDuration)
			{
				for (int m = 0; m < hairs.Length; m++)
				{
					hairs[m].ReverseBeamFinish();
				}
				reverseBeam.Close();
				state = UnitState.RandomFly;
			}
			break;
		case UnitState.Stage3CurseAttack:
			if (changedState)
			{
				base.Anima.SetTrigger("EyeStage3CurseAttack");
				flyHeight = 0f;
			}
			SetMove(Vector3.zero);
			break;
		case UnitState.Stage3SpitLeech:
			if (changedState)
			{
				base.Anima.SetTrigger("EyeStage3SpitLeech");
				flyHeight = spitLeechSelfHeight;
			}
			SetMove(Vector3.zero);
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public void ShoutDamage()
	{
		for (int i = 0; i < LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList.Count; i++)
		{
			Entity entity = LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList[i];
			if (UnitDotsSyncSystem.EntityIsValid(entity))
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(GetComponentData<LocalTransform>(entity).Position, base.transform.position) * stage3ShoutWaveKnockback;
				info.damage = stage3ShoutWaveDamage;
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
			}
		}
		for (int j = 0; j < LevelMgr.Inst.CurrentRoomCtrller.TeammateNotAttackEttList.Count; j++)
		{
			Entity entity2 = LevelMgr.Inst.CurrentRoomCtrller.TeammateNotAttackEttList[j];
			if (UnitDotsSyncSystem.EntityIsValid(entity2))
			{
				TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
				info2.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(GetComponentData<LocalTransform>(entity2).Position, base.transform.position) * stage3ShoutWaveKnockback;
				info2.damage = stage3ShoutWaveDamage;
				UnitDotsSyncSystem.AddTakeDamageRequest(entity2, info2);
			}
		}
	}

	protected override void BossDeadStay()
	{
		base.Anima.SetTrigger("Die");
		if (BossStage == Boss4Stage.Stage3)
		{
			base.Anima.SetTrigger("DieStage3");
		}
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
		if (state == UnitState.ReverseBeamWarning || state == UnitState.ReverseBeaming || state == UnitState.ReverseBeamAfter || (BossStage == Boss4Stage.Stage3 && state != UnitState.ScreamBefore && state != UnitState.ScreamDuration))
		{
			tsf_Hover.localPosition = Vector3.Lerp(tsf_Hover.localPosition, Vector3.zero, reverseBeamHeightLerp * Time.deltaTime);
		}
		else
		{
			hoverTimer += currentHoverSpeed * Time.deltaTime;
			tsf_Hover.localPosition = new Vector3(0f, Mathf.Sin(hoverTimer) * hoverScale, 0f);
		}
		base.Anima.SetFloat("EyeBallX", 0f);
		base.Anima.SetFloat("EyeBallY", 0f);
		tsf_HairParent.localPosition = tsf_Motion.localPosition + tsf_Hover.localPosition + myPpt.Tsf_BeHit.localPosition;
		tsf_HairParent.rotation = tsf_Motion.rotation;
		tsf_HairParent.localScale = tsf_Motion.localScale;
		for (int i = 0; i < wings.Length; i++)
		{
			wings[i].SetNormalMotion();
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
		case "SwitchingStage2":
			CamController.Inst.SetShock(shockRadius, shockSpeed, stage2ShockDuration);
			as_ToStage2.Play();
			break;
		case "RotateChild":
		{
			float num4 = UnityEngine.Random.Range(0f, 360f);
			for (int l = 0; l < rotateChildCount; l++)
			{
				Vector3 dir3 = Tool2D.GetDir(360f / (float)rotateChildCount * (float)l + num4);
				Vector3 point2 = base.transform.position + dir3 * rotateChildOffset;
				ClearDeadSummonsInList();
				Boss4_Leech component2 = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + rotateLeechID, point2).GetComponent<Boss4_Leech>();
				allLeeches.Add(component2);
				component2.Fly(this, dir3);
			}
			break;
		}
		case "RotateFinish":
			state = UnitState.RandomFly;
			break;
		case "CurseShoot":
		{
			curseAttackStage1Count.RandomResult();
			int num = BossStage switch
			{
				Boss4Stage.Stage1 => curseAttackStage1Count.RandomResult(), 
				Boss4Stage.Stage2 => curseAttackStage2Count.RandomResult(), 
				_ => curseAttackStage1Count.RandomResult(), 
			};
			float num2 = (float)(num - 1) * curseAttackAngle;
			GetNearestTargetPlayerFirst();
			Vector3 vector = (base.HaveTarget ? ToTargetDir() : Tool2D.GetDir());
			Vector3 point = Tool2D.IgnoreZPoint(base.transform.position);
			if (base.HaveTarget)
			{
				point += ToTargetDir() * curseAttackEFOffset;
			}
			for (int j = 0; j < num; j++)
			{
				Vector3 dir = Tool2D.GetDir(vector, (0f - num2) / 2f + num2 / (float)(num - 1) * (float)j);
				MiniPool.GetGO("Prefabs/EF/EF_Boss4_CurseAttack", point).GetComponent<Boss4_CurseAttack>().Initialize(this, dir);
			}
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.TakeKnockback(-vector * curseAttackRecoil);
			SetComponentData(componentData);
			SEMgr.Inst.boss4_CurseShoot.PlaySE();
			break;
		}
		case "CurseAttackFinish":
			state = UnitState.RandomFly;
			break;
		case "Stage3CurseAttackSE":
			as_Bellow.Play();
			break;
		case "Stage3CurseAttack":
		{
			stage3CurseAttackCount.RandomResult();
			float num3 = UnityEngine.Random.Range(0f, 360f);
			Vector3 vector2 = Tool2D.IgnoreZPoint(base.transform.position);
			for (int k = 0; k < stage3CurseAttackCount.result; k++)
			{
				Vector3 dir2 = Tool2D.GetDir(num3 + (float)(360 / stage3CurseAttackCount.result * k));
				MiniPool.GetGO("Prefabs/EF/EF_Boss4_CurseAttack", vector2 + dir2 * stage3CurseAttackOffset).GetComponent<Boss4_CurseAttack>().Initialize(this, dir2);
			}
			SEMgr.Inst.boss4_CurseShoot.PlaySE();
			break;
		}
		case "Stage3CurseAttackFinish":
		case "SpitLeechFinish":
			state = UnitState.RandomFly;
			break;
		case "SpitLeech":
		{
			spitLeechCount.RandomResult();
			for (int i = 0; i < spitLeechCount.result; i++)
			{
				ClearDeadSummonsInList();
				Boss4_Leech component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + spitLeechID, base.transform.position + new Vector3(0f, 0f, 0f - spitLeechHeight)).GetComponent<Boss4_Leech>();
				allLeeches.Add(component);
				component.Fly(this, Tool2D.GetDir(), isStage3: true);
			}
			as_Bellow2.Play();
			stage3LeechMaxCounter += spitLeechCount.result;
			break;
		}
		case "ToStage2Finish":
			base.Anima.SetTrigger("BodyIdle");
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	public override void Theme6Reposition(Vector3 changeValue)
	{
		base.Theme6Reposition(changeValue);
		for (int i = 0; i < hairs.Length; i++)
		{
			hairs[i].Theme6Reposition(changeValue);
		}
		for (int j = 0; j < wings.Length; j++)
		{
			wings[j].Theme6Reposition(changeValue);
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		UnityEngine.Object.Destroy(reverseBeam.gameObject);
		for (int i = 0; i < allTentacles.Count; i++)
		{
			if (allTentacles[i] != null && !allTentacles[i].myPpt.AlreadyDead)
			{
				allTentacles[i].DotsAnnouncedDeath();
			}
		}
		for (int j = 0; j < allLeeches.Count; j++)
		{
			if (allLeeches[j] != null && !allLeeches[j].myPpt.AlreadyDead)
			{
				allLeeches[j].DotsAnnouncedDeath();
			}
		}
	}

	private void ClearDeadSummonsInList()
	{
		for (int num = allLeeches.Count - 1; num >= 0; num--)
		{
			if (allLeeches[num].myPpt.AlreadyDead)
			{
				allLeeches.RemoveAt(num);
			}
		}
		for (int num2 = allTentacles.Count - 1; num2 >= 0; num2--)
		{
			if (allTentacles[num2].myPpt.AlreadyDead)
			{
				allTentacles.RemoveAt(num2);
			}
		}
	}

	public void TentacleDead()
	{
		summonTentacleCounter--;
	}

	public void Stage3LeechDead()
	{
		stage3LeechMaxCounter--;
	}
}
