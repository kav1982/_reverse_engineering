using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class Boss5 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Ground,
		Bubble,
		Side,
		Knock,
		BeforeTornado,
		Tornado,
		AfterTornado,
		Rest,
		Shield,
		SecondStage,
		ThirdStage,
		SecondStageStart,
		SecondStageGround,
		SecondStageRest,
		SecondStageFinish
	}

	public static Boss5 Inst;

	[Header("技能概率")]
	public float groundChance;

	public float sideChance;

	public float knockChance;

	public float tornadoChance;

	public float bubbleChance;

	public float shieldChance;

	[Header("状态机")]
	public StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("召唤乱创的小怪")]
	public List<Boss5_Child> allChildren = new List<Boss5_Child>();

	public int childCount;

	public float childInterval;

	public float childStateTime;

	public VariableFloat childStartDiration;

	public ParticleSystem tornadoParticle;

	[Header("地板攻击")]
	public Boss5_Ground groundAttack;

	public float chargeTime;

	public float attackInterval;

	public float attackTimes;

	[Header("头部水弹攻击适配")]
	public Boss5_MainAttack headAttack;

	public Vector3 targetDir;

	[Header("眼睛看人")]
	public bool staring;

	public Transform centerEyeTransform;

	public float centerEyeOffset;

	public float eyeLerp;

	public Transform leftEyeTransform;

	public Transform rightEyeTransform;

	public float sideEyeOffset;

	public float eyeBlinkInterval;

	private float eyeBlinkTimer;

	public bool canBlink;

	public float BlinkChance;

	[Header("休息和选技能")]
	public float restTime;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	[Header("汲取水弹")]
	public List<Boss5_Bubble> allBubbles = new List<Boss5_Bubble>();

	public ParticleSystem bubbleParticle;

	private bool bubbling;

	public int bubbleWaveTimes;

	private int bubbleWaveCount;

	public int bubbleCount;

	public float bubbleMinAngle;

	public float noBubbleAngle;

	public float healBubbleChance;

	public VariableFloat nextBubbleAngleRange;

	public int maxHealBubbleCount;

	public float bubbleInterval;

	public float bubbleStartDistance;

	public Transform mouthTransform;

	[Header("侧面拍地板")]
	public List<float> sidePointsY = new List<float>();

	public List<Vector3> leftPoints = new List<Vector3>();

	public List<Vector3> rightPoints = new List<Vector3>();

	public float sideAttackTimes;

	private float sideAttackCount;

	public float sideAttackInterval;

	public float singleSideAttackInterval;

	[Header("双手拍地板")]
	public Boss5_Hand leftHand;

	public Boss5_Hand rightHand;

	public Transform leftHandTsf;

	public Transform rightHandTsf;

	public int knockGroundTimes;

	[Header("护盾")]
	public int maxShieldTime;

	private int shieldTimesCounter;

	public float shieldExistTime;

	public float shieldExistTimer;

	public SpriteRenderer shieldRenderer;

	public Transform shieldTransform;

	private float shieldMaxScale;

	private float shieldNowScale;

	private bool shieldCreated;

	public int animationCount;

	public float shieldGrowTime;

	public float animationDuration;

	public float animationInterval;

	public AnimationCurve shieldGrowCurve;

	public ParticleSystem shieldParticle;

	public ParticleSystem shieldRingParticle;

	public ParticleSystem shieldBreakParticle;

	public bool shieldProtected;

	public List<Boss5_Rune> runes = new List<Boss5_Rune>();

	public float shieldWaveLerp;

	public float shieldHitNoiseSpeed;

	public float shieldHitNoiseIntensity;

	public float shieldHitNoiseDensity;

	private float shieldOriginNoiseSpeed;

	private float shieldOriginNoiseIntensity;

	private float shieldOriginNoiseDensity;

	private float shieldNowNoiseSpeed;

	private float shieldNowNoiseIntensity;

	private float shieldNowNoiseDensity;

	[Header("新二阶段柱子")]
	public bool useNewSecondStage;

	public float tentacleAttackInterval;

	private List<Boss5_LargeTentacle> allLargeTentacles = new List<Boss5_LargeTentacle>();

	public BoxCollider boxCollider;

	[Header("二，三阶段")]
	public float secondStageRatio;

	public float thirdStageRatio;

	public bool isSecondStage;

	public bool isThirdStage;

	public bool stageTentacleShow;

	public bool stageTentacleShowDone;

	public float stageTentacleGrowTime;

	public SpriteRenderer headRenderer;

	private float stageHeadBlend;

	public Boss5_HairMgr hairManager;

	[Header("双手")]
	public SkeletonAnimation leftHandAnima;

	public SkeletonAnimation rightHandAnima;

	[Header("初始位置修正")]
	public float transformYOffset;

	[Header("音效")]
	public AudioSource as_Suck;

	public AudioSource as_ShieldCharge;

	public AudioSource as_Tornado;

	private MonsterState lastSkill;

	private MonsterState foreLastSkill;

	private MonsterState nextSkill;

	private bool canShield;

	private float lastBubbleAngle;

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
		as_Suck.volume = DataMgr.settingData.GetFinalSound();
		as_ShieldCharge.volume = DataMgr.settingData.GetFinalSound();
		as_Tornado.volume = DataMgr.settingData.GetFinalSound();
	}

	private void RandomResort()
	{
		leftPoints.Clear();
		rightPoints.Clear();
		GeneralTool.RandomizeList(sidePointsY);
		for (int i = 0; i < sidePointsY.Count / 2; i++)
		{
			leftPoints.Add(new Vector3(roomCenterPoint.x - roomWidth / 2f, sidePointsY[i], 0f));
		}
		for (int j = sidePointsY.Count / 2; j < sidePointsY.Count; j++)
		{
			rightPoints.Add(new Vector3(roomCenterPoint.x + roomWidth / 2f, sidePointsY[j], 0f));
		}
	}

	public override void SingleInitialCallback()
	{
		base.SingleInitialCallback();
		leftHand = ObjPoolMgr.Inst.GetGO("Prefabs/Units/500521").GetComponent<Boss5_Hand>();
		rightHand = ObjPoolMgr.Inst.GetGO("Prefabs/Units/500521").GetComponent<Boss5_Hand>();
		leftHand.transform.position = Tool2D.IgnoreZPoint(leftHandTsf.position);
		leftHand.originPosition = Tool2D.IgnoreZPoint(leftHandTsf.position);
		rightHand.transform.position = Tool2D.IgnoreZPoint(rightHandTsf.position);
		rightHand.originPosition = Tool2D.IgnoreZPoint(rightHandTsf.position);
		rightHand.isRight = true;
		rightHand.otherHand = leftHand;
		leftHand.otherHand = rightHand;
		Boss5_Hand.knockTimes = knockGroundTimes;
		headAttack.anima = base.Anima;
		for (int i = 0; i < 4; i++)
		{
			runes.Add(ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss5_Rune", base.transform.position).GetComponent<Boss5_Rune>());
			runes[i].Initialize(i + 1);
			runes[i].Mute();
		}
		if (GameMgr.IsMobile_Static)
		{
			noBubbleAngle += 15f;
			singleSideAttackInterval *= 1.4f;
			knockGroundTimes--;
			Boss5_Hand.knockTimes = knockGroundTimes;
			sideAttackTimes -= 1f;
		}
	}

	public override void EveryInitialCallback()
	{
		Inst = this;
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height;
		base.transform.position -= new Vector3(0f, transformYOffset, 0f);
		SyncDotsPosition();
		sidePointsY.Clear();
		targetDir = new Vector3(0f, -1f, 0f);
		for (int i = 0; (float)i < roomHeight; i++)
		{
			sidePointsY.Add(roomCenterPoint.y - roomHeight / 2f + 0.5f + (float)i);
		}
		allChildren.Clear();
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		SetComponentData(componentData);
		shieldMaxScale = shieldTransform.localScale.x;
		shieldOriginNoiseSpeed = shieldRenderer.material.GetFloat("_NoiseSpeed");
		shieldOriginNoiseIntensity = shieldRenderer.material.GetFloat("_NoiseIntensity");
		shieldOriginNoiseDensity = shieldRenderer.material.GetFloat("_NoiseDensity");
		shieldNowNoiseIntensity = shieldOriginNoiseIntensity;
		shieldNowNoiseSpeed = shieldOriginNoiseSpeed;
		shieldNowNoiseDensity = shieldOriginNoiseDensity;
		state = MonsterState.BornIdle;
	}

	private void ChooseSkill()
	{
		bool flag = false;
		while (!flag)
		{
			int num = (isThirdStage ? ((!useNewSecondStage) ? GeneralTool.GetWeightRandom(bubbleChance, sideChance, groundChance, tornadoChance, knockChance, 0f) : GeneralTool.GetWeightRandom(bubbleChance, sideChance, 0f, tornadoChance, knockChance, 0f)) : (isSecondStage ? GeneralTool.GetWeightRandom(bubbleChance, sideChance, groundChance, tornadoChance, knockChance, 0f) : ((!useNewSecondStage) ? GeneralTool.GetWeightRandom(bubbleChance, 0f, groundChance, tornadoChance, 0f, 0f) : GeneralTool.GetWeightRandom(bubbleChance, 0f, groundChance, tornadoChance, 0f, 0f))));
			if (!useNewSecondStage && isThirdStage && lastSkill != MonsterState.Shield && !shieldProtected && canShield && shieldTimesCounter < maxShieldTime)
			{
				shieldTimesCounter++;
				nextSkill = MonsterState.Shield;
				canShield = false;
			}
			else
			{
				switch (num)
				{
				case 0:
					nextSkill = MonsterState.Bubble;
					break;
				case 1:
					nextSkill = MonsterState.Side;
					break;
				case 2:
					nextSkill = MonsterState.Ground;
					break;
				case 3:
					nextSkill = MonsterState.BeforeTornado;
					break;
				case 4:
					nextSkill = MonsterState.Knock;
					break;
				}
			}
			if (nextSkill == lastSkill)
			{
				continue;
			}
			if (isThirdStage)
			{
				if (foreLastSkill != lastSkill)
				{
					foreLastSkill = lastSkill;
					lastSkill = nextSkill;
					state = nextSkill;
					flag = true;
				}
			}
			else
			{
				foreLastSkill = nextSkill;
				lastSkill = nextSkill;
				state = nextSkill;
				flag = true;
			}
		}
		if (isThirdStage && !shieldProtected && nextSkill != MonsterState.Shield)
		{
			canShield = true;
		}
	}

	private void CheckChangeStage()
	{
		if (base.CurrentHPRatio < secondStageRatio && !isSecondStage)
		{
			isSecondStage = true;
			if (useNewSecondStage)
			{
				state = MonsterState.SecondStageStart;
			}
			else
			{
				state = MonsterState.SecondStage;
			}
		}
		else if (base.CurrentHPRatio < thirdStageRatio && !isThirdStage)
		{
			isSecondStage = true;
			isThirdStage = true;
			state = MonsterState.ThirdStage;
		}
	}

	public override void Update()
	{
		if (base.HaveTarget)
		{
			targetDir = ToTargetDir();
		}
		for (int num = allChildren.Count - 1; num >= 0; num--)
		{
			if (allChildren[num].myPpt.AlreadyDead)
			{
				allChildren.RemoveAt(num);
			}
		}
		for (int num2 = allBubbles.Count - 1; num2 >= 0; num2--)
		{
			if (allBubbles[num2].dying)
			{
				allBubbles.RemoveAt(num2);
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
		float num3 = ((!base.HaveTarget || !staring) ? ((90f / 180f - 0.5f) * 2f) : ((Vector3.Angle(ToTargetDir(), Vector3.left) / 180f - 0.5f) * 2f));
		centerEyeTransform.localPosition = Vector3.Lerp(centerEyeTransform.localPosition, new Vector3(num3 * centerEyeOffset, 0f, centerEyeTransform.localPosition.z), eyeLerp * Time.deltaTime);
		leftEyeTransform.localPosition = Vector3.Lerp(leftEyeTransform.localPosition, new Vector3(num3 * sideEyeOffset, 0f, leftEyeTransform.localPosition.z), eyeLerp * Time.deltaTime);
		rightEyeTransform.localPosition = Vector3.Lerp(rightEyeTransform.localPosition, new Vector3(num3 * sideEyeOffset, 0f, rightEyeTransform.localPosition.z), eyeLerp * Time.deltaTime);
		eyeBlinkTimer += Time.deltaTime;
		if (eyeBlinkTimer > eyeBlinkInterval)
		{
			eyeBlinkTimer = 0f;
			if (UnityEngine.Random.Range(0f, 1f) < BlinkChance && canBlink)
			{
				base.Anima.Play("Boss5_EyeBlink", 1, 0f);
			}
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				canBlink = true;
				staring = true;
				base.Anima.Play("Boss5_Idle");
				rightHandAnima.AnimationState.SetAnimation(0, "idle2", loop: true);
				leftHandAnima.AnimationState.SetAnimation(0, "idle2", loop: true);
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.Rest;
			}
			break;
		case MonsterState.Rest:
		{
			ref float reference4 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				canBlink = true;
				staring = true;
				base.Anima.Play("Boss5_Idle");
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			reference4 += Time.deltaTime;
			if (reference4 > 0.2f)
			{
				reference4 -= 0.2f;
				CheckChangeStage();
			}
			if (restTime < stateExistTime)
			{
				ChooseSkill();
			}
			break;
		}
		case MonsterState.Ground:
			if (changedState)
			{
				canBlink = false;
				staring = true;
				groundAttack.Attack();
				headAttack.freeAttack = true;
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (groundAttack.state == Boss5_Ground.GroundState.Rest)
			{
				BubbleAllDie();
				headAttack.freeAttack = false;
			}
			if (!headAttack.freeAttack && headAttack.state == Boss5_MainAttack.HeadState.Rest)
			{
				state = MonsterState.Rest;
			}
			break;
		case MonsterState.Bubble:
		{
			ref float reference3 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				canBlink = false;
				staring = true;
				bubbling = false;
				bubbleWaveCount = 0;
				base.Anima.Play("Boss5_BeforeBubble");
			}
			reference3 += Time.deltaTime;
			if (bubbling && reference3 > bubbleInterval)
			{
				bubbleWaveCount++;
				reference3 = 0f;
				CreateBubble();
				if (bubbleWaveCount >= bubbleWaveTimes)
				{
					bubbling = false;
					bubbleParticle.Stop();
					base.Anima.Play("Boss5_AfterBubble");
					as_Suck.DOFade(0f, 1f);
					BubbleAllDie();
				}
			}
			break;
		}
		case MonsterState.BeforeTornado:
			if (changedState)
			{
				canBlink = false;
				staring = false;
				base.Anima.Play("Boss5_BeforeTornado");
			}
			break;
		case MonsterState.Tornado:
			if (changedState)
			{
				base.Anima.Play("Boss5_Tornado");
				canBlink = true;
				staring = true;
				tornadoParticle.transform.position = Tool2D.GetLayerPoint(roomCenterPoint + new Vector3(0f, roomHeight / 2f - 1f, 0f));
				tornadoParticle.Play();
				as_Tornado.Play();
				as_Tornado.volume = 0f;
				as_Tornado.DOFade(DataMgr.settingData.GetFinalSound(), 1f);
				StartCoroutine(SummonChild());
			}
			if (!tornadoParticle.isPlaying && allChildren.Count == 0)
			{
				KillAllChild();
				state = MonsterState.AfterTornado;
			}
			else if (stateExistTime > childStateTime)
			{
				KillAllChild();
				state = MonsterState.AfterTornado;
			}
			break;
		case MonsterState.AfterTornado:
			if (changedState)
			{
				base.Anima.Play("Boss5_AfterTornado");
			}
			break;
		case MonsterState.SecondStageStart:
			if (changedState)
			{
				base.Anima.Play("Boss5_Hide");
			}
			break;
		case MonsterState.SecondStageGround:
		{
			ref int reference = ref varMgr.RegInt(0);
			ref float reference2 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				reference = 0;
				base.Anima.Play("Boss5_Hiding");
				groundAttack.Attack();
			}
			if (groundAttack.state == Boss5_Ground.GroundState.Rest)
			{
				state = MonsterState.SecondStageRest;
				break;
			}
			bool flag = true;
			for (int i = 0; i < 4; i++)
			{
				if (!allLargeTentacles[i].myPpt.AlreadyDead)
				{
					flag = false;
				}
			}
			if (flag)
			{
				state = MonsterState.SecondStageFinish;
				break;
			}
			if (stateExistTime > 1.4f)
			{
				reference2 += Time.deltaTime;
			}
			if (!(reference2 > tentacleAttackInterval))
			{
				break;
			}
			reference2 = 0f;
			while (allLargeTentacles[reference].myPpt.AlreadyDead)
			{
				reference++;
				if (reference >= 4)
				{
					reference = 0;
				}
			}
			allLargeTentacles[reference].Attack();
			reference++;
			if (reference >= 4)
			{
				reference = 0;
			}
			break;
		}
		case MonsterState.SecondStageRest:
		{
			if (changedState)
			{
				base.Anima.Play("Boss5_Hiding");
			}
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.SecondStageGround;
				break;
			}
			bool flag2 = true;
			for (int j = 0; j < 4; j++)
			{
				if (!allLargeTentacles[j].myPpt.AlreadyDead)
				{
					flag2 = false;
				}
			}
			if (flag2)
			{
				state = MonsterState.SecondStageFinish;
			}
			break;
		}
		case MonsterState.SecondStageFinish:
			if (changedState)
			{
				base.Anima.Play("Boss5_Show");
				groundAttack.state = Boss5_Ground.GroundState.Rest;
			}
			break;
		case MonsterState.SecondStage:
			if (changedState)
			{
				canBlink = false;
				staring = false;
				base.Anima.Play("Boss5_ChangeStage");
				stageTentacleShow = true;
			}
			if (stateExistTime > stageTentacleGrowTime)
			{
				state = MonsterState.Rest;
			}
			break;
		case MonsterState.Side:
			if (changedState)
			{
				canBlink = true;
				staring = true;
				base.Anima.Play("Boss5_SideStart");
				RandomResort();
				sideAttackCount = 1f;
				StartCoroutine(SideAttack());
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (stateExistTime > sideAttackInterval)
			{
				if (sideAttackCount > sideAttackTimes - 1f)
				{
					base.Anima.Play("Boss5_SideFinish");
					stageTentacleShowDone = true;
				}
				else
				{
					stateExistTime = 0f;
					sideAttackCount += 1f;
					StartCoroutine(SideAttack());
				}
			}
			break;
		case MonsterState.Knock:
			if (changedState)
			{
				base.Anima.Play("Boss5_Idle");
				canBlink = true;
				staring = true;
				Boss5_Hand.knockCount = 0f;
				leftHand.freeAct = true;
				rightHand.freeAct = true;
				return;
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (leftHand.state == Boss5_Hand.HandState.Rest && rightHand.state == Boss5_Hand.HandState.Rest)
			{
				leftHand.state = Boss5_Hand.HandState.Hide;
				rightHand.state = Boss5_Hand.HandState.Hide;
			}
			if (leftHand.state == Boss5_Hand.HandState.Invisible && rightHand.state == Boss5_Hand.HandState.Invisible)
			{
				state = MonsterState.Rest;
			}
			break;
		case MonsterState.ThirdStage:
			if (changedState)
			{
				stageTentacleShow = true;
				canBlink = false;
				staring = false;
				base.Anima.Play("Boss5_ChangeStage3");
			}
			break;
		case MonsterState.Shield:
			if (changedState)
			{
				canBlink = false;
				staring = false;
				shieldProtected = false;
				shieldNowScale = 0f;
				shieldTransform.localScale = Vector3.one * shieldNowScale;
				shieldExistTimer = 0f;
				shieldCreated = false;
				base.Anima.Play("Boss5_Shield");
			}
			if (shieldCreated && !shieldProtected)
			{
				shieldNowScale += Time.deltaTime / shieldGrowTime * shieldMaxScale;
				shieldTransform.localScale = Vector3.one * shieldNowScale;
			}
			if (shieldNowScale > shieldMaxScale && !shieldProtected)
			{
				SEMgr.Inst.boss5_ShieldProtected.PlaySE();
				SEMgr.Inst.boss5_ShieldDrop.PlaySE();
				SEMgr.Inst.boss5_RuneShow.PlaySE();
				as_ShieldCharge.Stop();
				shieldProtected = true;
				shieldParticle.Stop();
				shieldRingParticle.Play();
				shieldBreakParticle.Play();
			}
			break;
		}
		shieldNowNoiseDensity = Mathf.Lerp(shieldNowNoiseDensity, shieldOriginNoiseDensity, shieldWaveLerp * Time.deltaTime);
		shieldNowNoiseSpeed = Mathf.Lerp(shieldNowNoiseSpeed, shieldOriginNoiseSpeed, shieldWaveLerp * Time.deltaTime);
		shieldNowNoiseIntensity = Mathf.Lerp(shieldNowNoiseIntensity, shieldOriginNoiseIntensity, shieldWaveLerp * Time.deltaTime);
		shieldRenderer.sharedMaterial.SetFloat("_NoiseSpeed", shieldNowNoiseSpeed);
		shieldRenderer.sharedMaterial.SetFloat("_NoiseIntensity", shieldNowNoiseIntensity);
		shieldRenderer.sharedMaterial.SetFloat("_NoiseDensity", shieldNowNoiseDensity);
		if (shieldProtected)
		{
			shieldExistTimer += Time.deltaTime;
			bool flag3 = true;
			for (int k = 0; k < runes.Count; k++)
			{
				if (!runes[k].bulletReached)
				{
					flag3 = false;
				}
			}
			if (shieldExistTimer > shieldExistTime)
			{
				flag3 = true;
			}
			if (flag3)
			{
				for (int l = 0; l < runes.Count; l++)
				{
					runes[l].BulletReset();
				}
				shieldProtected = false;
				shieldBreakParticle.Play();
			}
		}
		if (!shieldProtected && state != MonsterState.Shield)
		{
			shieldNowScale = 0f;
			shieldTransform.localScale = Vector3.one * shieldNowScale;
			shieldRingParticle.Stop();
		}
	}

	private void LateUpdate()
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		myPpt.MR_Models[0].GetPropertyBlock(materialPropertyBlock);
		if (materialPropertyBlock.GetColor("_Color") != myPpt.BaseColor)
		{
			materialPropertyBlock.SetColor("_Color", myPpt.BaseColor);
			for (int i = 0; i < myPpt.MR_Models.Length; i++)
			{
				myPpt.MR_Models[i].SetPropertyBlock(materialPropertyBlock);
			}
		}
	}

	public override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		base.BeforeAnnouncedDeath_Dots(ref info);
		LateUpdate();
	}

	public void ShieldHit()
	{
		SEMgr.Inst.boss5_ShieldDrop.PlaySE();
		SEMgr.Inst.boss5_ShieldHit.PlaySE();
		shieldNowNoiseSpeed = shieldHitNoiseSpeed;
		shieldNowNoiseIntensity = shieldHitNoiseIntensity;
		shieldNowNoiseDensity = shieldHitNoiseDensity;
		shieldBreakParticle.Play();
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
	}

	private void CreateBubble()
	{
		float num = 180f / ((float)bubbleCount - 1f);
		float num3;
		float num2;
		if (lastBubbleAngle == 0f)
		{
			num2 = UnityEngine.Random.Range(bubbleMinAngle - noBubbleAngle / 2f, 180f - noBubbleAngle / 2f - bubbleMinAngle);
			num3 = num2;
		}
		else
		{
			nextBubbleAngleRange.RandomResult();
			num2 = lastBubbleAngle + nextBubbleAngleRange.result * (float)((UnityEngine.Random.Range(0f, 1f) > 0.5f) ? 1 : (-1));
			num3 = lastBubbleAngle * 2f - num2;
			if (Mathf.Abs(num2 - 90f) > 90f - bubbleMinAngle)
			{
				float num4 = num2;
				num2 = num3;
				num3 = num4;
			}
		}
		lastBubbleAngle = num2;
		float num5 = 0f;
		bool flag = Mathf.Abs(num3 - 90f) > 90f - bubbleMinAngle;
		if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
		{
			flag = false;
		}
		num2 -= noBubbleAngle % num;
		for (int i = 0; i < bubbleCount; i++)
		{
			if (flag)
			{
				if ((num5 < num2 || num5 > num2 + noBubbleAngle) && (num5 < num3 || num5 > num3 + noBubbleAngle))
				{
					Boss5_Bubble component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss5_Bubble", Tool2D.IgnoreZPoint(mouthTransform.position + Tool2D.GetDir(Vector3.up, 90f + num5) * bubbleStartDistance)).GetComponent<Boss5_Bubble>();
					component.Initialize(mouthTransform.position);
					allBubbles.Add(component);
				}
			}
			else if (num5 < num2 || num5 > num2 + noBubbleAngle)
			{
				Boss5_Bubble component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss5_Bubble", Tool2D.IgnoreZPoint(mouthTransform.position + Tool2D.GetDir(Vector3.up, 90f + num5) * bubbleStartDistance)).GetComponent<Boss5_Bubble>();
				component2.Initialize(mouthTransform.position);
				allBubbles.Add(component2);
			}
			num5 += num;
		}
	}

	private IEnumerator SummonChild()
	{
		yield return new WaitForSeconds(childInterval);
		for (int i = 0; i < childCount; i++)
		{
			childStartDiration.RandomResult();
			Boss5_Child component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + 500551, roomCenterPoint + new Vector3(0f, roomHeight / 2f - 1f, 0f)).GetComponent<Boss5_Child>();
			component.moveDiration = Tool2D.GetDir(Vector3.up, childStartDiration.result);
			allChildren.Add(component);
			component.master = this;
			yield return new WaitForSeconds(childInterval);
		}
		tornadoParticle.Stop();
		as_Tornado.DOFade(0f, 1f);
	}

	private IEnumerator SideAttack()
	{
		yield return new WaitForSeconds(1f);
		RandomResort();
		for (int i = 0; i < sidePointsY.Count / 2 + 1; i++)
		{
			if (i < leftPoints.Count)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss5_WaveMaker", leftPoints[i] - new Vector3(1f, 0f, 0f), 6f).GetComponent<Boss5_WaveMaker>().Initialize(leftPoints[i], towardsRight: true);
			}
			if (i < rightPoints.Count)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss5_WaveMaker", rightPoints[i] + new Vector3(1f, 0f, 0f), 6f).GetComponent<Boss5_WaveMaker>().Initialize(rightPoints[i], towardsRight: false);
			}
			yield return new WaitForSeconds(singleSideAttackInterval);
		}
	}

	private void KillAllChild()
	{
		for (int i = 0; i < allChildren.Count; i++)
		{
			allChildren[i].ForceKill();
		}
		allChildren.Clear();
	}

	public void BubbleAllDie()
	{
		for (int num = allBubbles.Count - 1; num >= 0; num--)
		{
			if (allBubbles[num] != null)
			{
				allBubbles[num].Die();
			}
			allBubbles.RemoveAt(num);
		}
	}

	protected override void BossDeadStay()
	{
		base.Anima.Play("Die");
		SEMgr.Inst.elite10Dead.PlaySE();
		base.enabled = false;
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		myPpt.ChangeColor(myPpt.Color_NormalBody);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		StopAllCoroutines();
		rightHand.DotsAnnouncedDeath();
		leftHand.DotsAnnouncedDeath();
		KillAllChild();
		BubbleAllDie();
		groundAttack.state = Boss5_Ground.GroundState.Rest;
		headAttack.state = Boss5_MainAttack.HeadState.Rest;
		for (int i = 0; i < 4; i++)
		{
			runes[i].Mute();
		}
		for (int j = 0; j < allLargeTentacles.Count; j++)
		{
			allLargeTentacles[j].DotsAnnouncedDeath();
		}
		base.AfterDead(ref info);
		base.transform.position = roomCenterPoint;
		SyncDotsPosition();
	}

	public override void AnimaAction(string animaName)
	{
		if (base.deadStayed)
		{
			return;
		}
		headAttack.AnimaAction(animaName);
		switch (animaName)
		{
		case "TentacleSummon":
		{
			for (int j = 0; j < 4; j++)
			{
				Boss5_LargeTentacle component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/500553", roomCenterPoint + new Vector3((float)((j / 2 != 1) ? 1 : (-1)) * (roomWidth / 2f - 1f), (float)((j % 2 != 1) ? 1 : (-1)) * (roomHeight / 2f), 0f)).GetComponent<Boss5_LargeTentacle>();
				allLargeTentacles.Add(component);
			}
			break;
		}
		case "HideDone":
		{
			boxCollider.enabled = false;
			SetDotsCCEnable(isOpen: false);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanBeTarget = false;
			SetComponentData(componentData);
			state = MonsterState.SecondStageGround;
			break;
		}
		case "ShowDone":
		{
			isSecondStage = true;
			isThirdStage = true;
			boxCollider.enabled = true;
			SetDotsCCEnable(isOpen: true);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanBeTarget = true;
			SetComponentData(componentData);
			state = MonsterState.ThirdStage;
			break;
		}
		case "BubbleStart":
			base.Anima.Play("Boss5_Bubble");
			bubbling = true;
			bubbleParticle.Play();
			as_Suck.Play();
			as_Suck.volume = 0f;
			as_Suck.DOFade(DataMgr.settingData.GetFinalSound(), 1f);
			break;
		case "BubbleFinish":
			state = MonsterState.Rest;
			base.Anima.Play("Boss5_Idle");
			break;
		case "SecondStageFinish":
			stageTentacleShowDone = true;
			state = MonsterState.Rest;
			stateExistTime = restTime;
			foreLastSkill = lastSkill;
			lastSkill = state;
			break;
		case "ThirdStageColorChange":
			hairManager.stageColorChange = true;
			break;
		case "ThirdStageFinish":
			stageTentacleShowDone = true;
			if (!useNewSecondStage)
			{
				state = MonsterState.Shield;
			}
			else
			{
				state = MonsterState.Knock;
			}
			foreLastSkill = lastSkill;
			lastSkill = state;
			break;
		case "BeforeTornadoFinish":
			state = MonsterState.Tornado;
			break;
		case "TornadoFinish":
			state = MonsterState.Rest;
			break;
		case "SideStartFinish":
			base.Anima.Play("Boss5_Side");
			stageTentacleShowDone = false;
			break;
		case "SideFinish":
			state = MonsterState.Rest;
			break;
		case "ShieldFinish":
			state = MonsterState.Rest;
			break;
		case "ShieldCreate":
			shieldParticle.Play();
			as_ShieldCharge.Play();
			shieldCreated = true;
			break;
		case "Shout":
			SEMgr.Inst.boss5_Shout.PlaySE();
			break;
		case "ThirdStage":
			SEMgr.Inst.boss5_ThirdStage.PlaySE();
			break;
		case "OpenEye":
			base.Anima.Play("Boss5_EyeOpen", 1);
			break;
		case "CloseEye":
			base.Anima.Play("Boss5_EyeClose", 1);
			break;
		case "EyeCloseFinish":
			base.Anima.Play("Boss5_EyeClosing", 1);
			break;
		case "EyeOpenFinish":
			base.Anima.Play("Boss5_EyeOpening", 1);
			break;
		case "EyeSwitch":
			hairManager.ChangeEye();
			break;
		case "RuneShow":
		{
			SEMgr.Inst.boss5_Rune.PlaySE();
			Boss5_Rune.Rearrange();
			for (int i = 0; i < runes.Count; i++)
			{
				runes[i].Initialize(i + 1);
			}
			Boss5_Rune.order = 1;
			break;
		}
		}
	}
}
