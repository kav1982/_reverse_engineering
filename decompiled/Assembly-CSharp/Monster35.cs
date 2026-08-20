using System;
using System.Collections.Generic;
using Unity.Transforms;
using UnityEngine;

public class Monster35 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		IdleWalk,
		RunToTarget,
		Shoot,
		UnderGround,
		Hide,
		Show,
		Jump
	}

	public float bornIdleTime;

	[Header("骨头球")]
	public GameObject boneBallPrefab;

	public float ballHeight;

	public float actionInterval;

	[Header("尖刺攻击")]
	public VariableFloat spikeRange;

	public float spikeLines;

	public float spikeSpeed;

	public float spikeDeltaDistance;

	public GameObject wavePrefab;

	public float spikeChance;

	private List<Monster35_SpikeWave> spikeWavePool = new List<Monster35_SpikeWave>();

	[Header("地下潜行")]
	public int barrierId;

	private Vector3 fakeTarget;

	private Vector3 underGroundDiration;

	public float underGroundRotateSpeed;

	public int underGroundCount;

	public int MaxUnderGroundCount;

	public float underGroundDistance;

	public float dirtDistance;

	public Vector3 lastDirtPosition;

	public ParticleSystem dirtParticle;

	public ParticleSystem moveParticle;

	[Header("身体摇晃")]
	public List<Transform> bodyTransform = new List<Transform>();

	public float shakeSpeed;

	public float shakeTimer;

	public float shakeInterval;

	public float shakeRange;

	public float idleSpeed;

	private Vector3 underGroundLastPosition;

	[Header("飞行")]
	public List<Shadow> fakeBodyShadow = new List<Shadow>();

	public Transform fakeBodyRootTransform;

	public List<Transform> fakeBodyTransform = new List<Transform>();

	public List<Transform> fakeBodyMaskTransform = new List<Transform>();

	public List<Vector3> fakeBodyRealPosition = new List<Vector3>();

	public float bodyDistance;

	public float jumpingGravity;

	public float jumpTime;

	private Vector3 jumpStartPoint;

	private Vector3 jumpEndPoint;

	public float jumpMinDistance;

	public float jumpMaxDistance;

	public SpriteMask mask;

	public float headToGround;

	public int caculateTime;

	public float jumpAttackableHeight;

	public GameObject realBody;

	public GameObject fakeBody;

	public float maskOffset;

	private int fakeHeadIndex;

	public float hideDepth;

	private float singleJumpTimer;

	private float totalJumpTimer;

	public float undergroundChance;

	private int dragBody;

	public float holyJumpTime;

	private float originJumpTime;

	public float holyJumpChance;

	public GameObject Motion;

	public float dropSpikes;

	public List<ParticleSystem> dropParticles = new List<ParticleSystem>();

	[Header("身体表现")]
	public SpriteRenderer sr_DirtFore;

	public SpriteRenderer sr_DirtBack;

	[Header("音效")]
	public AudioSource audioSource;

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	public StateVariableMgr varMgr = new StateVariableMgr();

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

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundChange));
		SoundChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundChange));
	}

	private void SoundChange()
	{
		audioSource.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		originJumpTime = jumpTime;
		myPpt.RemoveSRFromArray(sr_DirtFore);
		myPpt.RemoveSRFromArray(sr_DirtBack);
	}

	public override void EveryInitialCallback()
	{
		for (int i = 0; i < dropParticles.Count; i++)
		{
			dropParticles[i].Clear();
		}
		dirtParticle.Clear();
		spikeWavePool.Clear();
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
		realBody.SetActive(value: true);
		fakeBody.SetActive(value: false);
		state = MonsterState.BornIdle;
		base.CC_Self.enabled = true;
		SetDotsCCEnable(isOpen: true);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = true;
		SetComponentData(componentData);
	}

	public override void Frame1InitialCallback()
	{
		for (int i = 0; i < fakeBodyShadow.Count; i++)
		{
			fakeBodyShadow[i].CreateShadow();
			fakeBodyShadow[i].Hide();
		}
	}

	private void JumpPrepare()
	{
		singleJumpTimer = 0f;
		if (fakeBodyRealPosition.Count == 0)
		{
			for (int i = 0; i < fakeBodyTransform.Count; i++)
			{
				fakeBodyRealPosition.Add(base.transform.position - new Vector3(0f, 0f, hideDepth));
			}
		}
		fakeBodyRootTransform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(fakeBodyRealPosition[0]));
		for (int j = 0; j < fakeBodyTransform.Count; j++)
		{
			fakeBodyRealPosition[j] = base.transform.position - new Vector3(0f, 0f, hideDepth);
			fakeBodyTransform[j].transform.position = Tool2D.GetLayerPoint(fakeBodyRealPosition[j]);
		}
		fakeHeadIndex = 0;
		realBody.SetActive(value: false);
		fakeBody.SetActive(value: true);
		dragBody = 0;
	}

	private bool JumpFixing()
	{
		if (singleJumpTimer > jumpTime)
		{
			fakeBodyShadow[fakeHeadIndex].Hide();
			if (fakeHeadIndex == fakeBodyTransform.Count - 1)
			{
				realBody.SetActive(value: true);
				fakeBody.SetActive(value: false);
				return true;
			}
			Vector3 vector = fakeBodyRealPosition[fakeHeadIndex] - fakeBodyRealPosition[fakeHeadIndex + 1];
			if (vector.x > vector.y)
			{
				singleJumpTimer -= vector.x / (jumpEndPoint - jumpStartPoint).x * jumpTime;
			}
			else
			{
				singleJumpTimer -= vector.y / (jumpEndPoint - jumpStartPoint).y * jumpTime;
			}
			fakeHeadIndex++;
		}
		Vector3 vector2 = Tool2D.IgnoreZPoint((jumpEndPoint - jumpStartPoint).normalized);
		float num = 3f * bodyDistance * bodyDistance;
		singleJumpTimer += Time.deltaTime;
		fakeBodyRealPosition[fakeHeadIndex] = jumpStartPoint + (jumpEndPoint - jumpStartPoint) * singleJumpTimer / jumpTime + new Vector3(0f, 0f, (jumpingGravity + 4f) / 2f * singleJumpTimer * (singleJumpTimer - jumpTime)) - new Vector3(0f, 0f, hideDepth);
		fakeBodyRootTransform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(fakeBodyRealPosition[0]));
		fakeBodyMaskTransform[fakeHeadIndex].transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(fakeBodyRealPosition[fakeHeadIndex])) - new Vector3(0f, maskOffset, 0f);
		fakeBodyTransform[fakeHeadIndex].position = Tool2D.GetLayerPoint(fakeBodyRealPosition[fakeHeadIndex]);
		fakeBodyTransform[fakeHeadIndex].localScale = Vector3.one / (2f * maskOffset);
		if (fakeHeadIndex == 0)
		{
			fakeBodyTransform[fakeHeadIndex].up = Tool2D.IgnoreZPoint(fakeBodyTransform[fakeHeadIndex].position - fakeBodyTransform[fakeHeadIndex + 1].position);
		}
		else
		{
			fakeBodyTransform[fakeHeadIndex].up = Tool2D.IgnoreZPoint(fakeBodyTransform[fakeHeadIndex - 1].position - fakeBodyTransform[fakeHeadIndex].position);
		}
		if (fakeBodyRealPosition[fakeHeadIndex].z < 0f && !fakeBodyShadow[fakeHeadIndex].IsShow)
		{
			fakeBodyShadow[fakeHeadIndex].Show();
		}
		else if (fakeBodyRealPosition[fakeHeadIndex].z > 0f && fakeBodyShadow[fakeHeadIndex].IsShow)
		{
			fakeBodyShadow[fakeHeadIndex].Hide();
		}
		for (int i = fakeHeadIndex + 1; i < fakeBodyTransform.Count; i++)
		{
			if ((fakeBodyRealPosition[i] - fakeBodyRealPosition[i - 1]).sqrMagnitude > num && dragBody < i)
			{
				dragBody = i;
			}
			fakeBodyMaskTransform[i].position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(fakeBodyRealPosition[i])) - new Vector3(0f, maskOffset, 0f);
			fakeBodyTransform[i].position = Tool2D.GetLayerPoint(fakeBodyRealPosition[i]);
			if (fakeBodyRealPosition[i].z < 0f && !fakeBodyShadow[i].IsShow)
			{
				fakeBodyShadow[i].Show();
			}
			else if (fakeBodyRealPosition[i].z > 0f && fakeBodyShadow[i].IsShow)
			{
				fakeBodyShadow[i].Hide();
			}
			if (i <= dragBody)
			{
				Vector3 vector3 = Tool2D.IgnoreZPoint(fakeBodyRealPosition[i - 1] - vector2 * bodyDistance);
				Vector3 vector4 = Tool2D.IgnoreZPoint(fakeBodyRealPosition[i - 1]);
				Vector3 heighted = GetHeighted((vector3 + vector4) / 2f);
				for (int j = 0; j < caculateTime; j++)
				{
					if ((fakeBodyRealPosition[i - 1] - heighted).sqrMagnitude > num)
					{
						vector3 = Tool2D.IgnoreZPoint(heighted);
						heighted = GetHeighted((vector3 + vector4) / 2f);
					}
					else
					{
						vector4 = Tool2D.IgnoreZPoint(heighted);
						heighted = GetHeighted((vector3 + vector4) / 2f);
					}
				}
				fakeBodyRealPosition[i] = GetHeighted(heighted);
			}
			if (Tool2D.IgnoreZPoint(fakeBodyTransform[i - 1].position - fakeBodyTransform[i].position).sqrMagnitude > num)
			{
				fakeBodyTransform[i].localScale = new Vector3(1f, Tool2D.IgnoreZPoint(fakeBodyTransform[i - 1].position - fakeBodyTransform[i].position).magnitude / bodyDistance * 0.7f, 1f) / (2f * maskOffset);
			}
			else
			{
				fakeBodyTransform[i].localScale = new Vector3(1f, 1f, 1f) / (2f * maskOffset);
			}
			fakeBodyTransform[i].up = Tool2D.IgnoreZPoint(fakeBodyTransform[i - 1].position - fakeBodyTransform[i].position);
		}
		return false;
	}

	private Vector3 GetHeighted(Vector3 origin)
	{
		float num = 0f;
		num = ((!(Mathf.Abs(origin.x - jumpStartPoint.x) > Mathf.Abs(origin.y - jumpStartPoint.y))) ? ((origin.y - jumpStartPoint.y) / (jumpEndPoint.y - jumpStartPoint.y) * jumpTime) : ((origin.x - jumpStartPoint.x) / (jumpEndPoint.x - jumpStartPoint.x) * jumpTime));
		return Tool2D.IgnoreZPoint(origin) - new Vector3(0f, 0f, (0f - (jumpingGravity + 4f)) / 2f * num * (num - jumpTime)) - new Vector3(0f, 0f, hideDepth);
	}

	private void BodyClear()
	{
		shakeTimer = 0f;
		for (int i = 0; i < bodyTransform.Count; i++)
		{
			bodyTransform[i].localEulerAngles = new Vector3(0f, 0f, 0f);
		}
	}

	private void BodyShake()
	{
		shakeTimer += Time.deltaTime;
		for (int i = 0; i < bodyTransform.Count; i++)
		{
			if (shakeInterval > shakeTimer)
			{
				bodyTransform[i].localEulerAngles = new Vector3(0f, 0f, Mathf.Sin((shakeTimer - (float)i * shakeInterval) * shakeSpeed)) * shakeRange * shakeTimer / shakeInterval;
			}
			else
			{
				bodyTransform[i].localEulerAngles = new Vector3(0f, 0f, Mathf.Sin((shakeTimer - (float)i * shakeInterval) * shakeSpeed)) * shakeRange;
			}
			if (i == bodyTransform.Count - 1)
			{
				if (shakeInterval > shakeTimer)
				{
					bodyTransform[i].localEulerAngles = new Vector3(0f, 0f, Mathf.Sin((shakeTimer - (float)i * shakeInterval) * shakeSpeed)) * shakeRange * shakeTimer / shakeInterval / 2f;
				}
				else
				{
					bodyTransform[i].localEulerAngles = new Vector3(0f, 0f, Mathf.Sin((shakeTimer - (float)i * shakeInterval) * shakeSpeed)) * shakeRange / 2f;
				}
			}
		}
	}

	private void BodyIdle()
	{
		shakeTimer = 0f;
		for (int i = 0; i < bodyTransform.Count; i++)
		{
			float num = bodyTransform[i].localEulerAngles.z;
			if (bodyTransform[i].localEulerAngles.z > 180f)
			{
				num -= 360f;
			}
			bodyTransform[i].localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(num, 0f, idleSpeed * Time.deltaTime));
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
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Monster35_idle");
				BodyClear();
			}
			BodyShake();
			SetMove(Vector3.zero, isFlip: false);
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= bornIdleTime)
			{
				checkTargetIntervalTimer = 0f;
				state = MonsterState.Hide;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Monster35_idle");
				BodyClear();
			}
			BodyShake();
			SetMove(Vector3.zero, isFlip: false);
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= actionInterval)
			{
				checkTargetIntervalTimer = 0f;
				state = MonsterState.Hide;
			}
			break;
		case MonsterState.Hide:
			if (changedState)
			{
				base.Anima.Play("Monster35_hide");
			}
			BodyIdle();
			SetMove(Vector3.zero);
			break;
		case MonsterState.Show:
			if (changedState)
			{
				BodyClear();
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				UnitProperty_Dots componentData5 = GetComponentData<UnitProperty_Dots>();
				componentData5.CanTouch = true;
				SetComponentData(componentData5);
				base.Anima.Play("Monster35_show");
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.UnderGround:
			if (changedState)
			{
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanTouch = false;
				SetComponentData(componentData);
				underGroundCount = 0;
				audioSource.Play();
				moveParticle.Play();
				fakeTarget = new Vector3(UnityEngine.Random.Range((0f - roomWidth) / 4f, roomWidth / 4f), UnityEngine.Random.Range((0f - roomHeight) / 4f, roomHeight / 4f), 0f) + roomCenterPoint;
				if (!base.HaveTarget)
				{
					underGroundDiration = fakeTarget - base.transform.position;
					underGroundDiration = Tool2D.IgnoreZPoint(underGroundDiration);
				}
				else
				{
					underGroundDiration = base.TargetPoint - base.transform.position;
					underGroundDiration = Tool2D.IgnoreZPoint(underGroundDiration);
				}
				underGroundLastPosition = base.transform.position;
				lastDirtPosition = base.transform.position;
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				fakeTarget = new Vector3(UnityEngine.Random.Range((0f - roomWidth) / 4f, roomWidth / 4f), UnityEngine.Random.Range((0f - roomHeight) / 4f, roomHeight / 4f), 0f) + roomCenterPoint;
				underGroundDiration = Vector3.RotateTowards(underGroundDiration, Tool2D.IgnoreZPoint(fakeTarget - base.transform.position), MathF.PI / 180f * underGroundRotateSpeed * myPpt.MoveSpeed * Time.deltaTime, 0f).normalized;
				underGroundDiration = Tool2D.IgnoreZPoint(underGroundDiration);
				SetMove(underGroundDiration * base.MoveSpeed);
			}
			else
			{
				underGroundDiration = Vector3.RotateTowards(underGroundDiration, Tool2D.IgnoreZPoint(base.TargetPoint - base.transform.position), MathF.PI / 180f * underGroundRotateSpeed * myPpt.MoveSpeed * Time.deltaTime, 0f).normalized;
				underGroundDiration = Tool2D.IgnoreZPoint(underGroundDiration);
				SetMove(underGroundDiration * base.MoveSpeed);
			}
			if ((base.transform.position - lastDirtPosition).sqrMagnitude > dirtDistance * dirtDistance)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster35_Dirt", base.transform.position);
				lastDirtPosition = base.transform.position;
			}
			if ((base.transform.position - underGroundLastPosition).sqrMagnitude > underGroundDistance * underGroundDistance)
			{
				underGroundLastPosition = base.transform.position;
				if (underGroundCount < MaxUnderGroundCount)
				{
					SEMgr.Inst.monster35Spike.PlaySE();
					ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + barrierId, base.transform.position);
					underGroundCount++;
				}
			}
			if (underGroundCount >= MaxUnderGroundCount)
			{
				audioSource.Stop();
				underGroundCount = 0;
				state = MonsterState.Jump;
				moveParticle.Stop();
			}
			break;
		case MonsterState.Jump:
		{
			ref bool reference = ref varMgr.RegBool(0);
			ref bool reference2 = ref varMgr.RegBool(1);
			ref bool reference3 = ref varMgr.RegBool(2);
			if (changedState)
			{
				if (UnityEngine.Random.Range(0f, 1f) < holyJumpChance)
				{
					reference3 = true;
					jumpTime = holyJumpTime;
				}
				else
				{
					reference3 = false;
					jumpTime = originJumpTime;
				}
				reference = false;
				reference2 = false;
				totalJumpTimer = 0f;
				jumpStartPoint = Tool2D.IgnoreZPoint(base.transform.position);
				if (base.HaveTarget)
				{
					jumpEndPoint = base.TargetPoint;
				}
				else
				{
					fakeTarget = new Vector3(UnityEngine.Random.Range((0f - roomWidth) / 4f, roomWidth / 4f), UnityEngine.Random.Range((0f - roomHeight) / 4f, roomHeight / 4f), 0f) + roomCenterPoint;
					jumpEndPoint = fakeTarget;
				}
				jumpEndPoint = Tool2D.IgnoreZPoint((jumpEndPoint - jumpStartPoint).normalized * UnityEngine.Random.Range(jumpMinDistance, jumpMaxDistance) + jumpStartPoint);
				if (Mathf.Abs(jumpEndPoint.y - roomCenterPoint.y) > roomHeight / 2f || Mathf.Abs(jumpEndPoint.x - roomCenterPoint.x) > roomWidth / 2f)
				{
					jumpEndPoint = Tool2D.IgnoreZPoint((jumpEndPoint - jumpStartPoint).normalized * jumpMinDistance + jumpStartPoint);
				}
				JumpPrepare();
			}
			Vector3 zero = Vector3.zero;
			zero = ((!(totalJumpTimer < jumpTime)) ? Tool2D.IgnoreZPoint(jumpEndPoint) : Tool2D.IgnoreZPoint(jumpStartPoint + (jumpEndPoint - jumpStartPoint) * totalJumpTimer / jumpTime));
			totalJumpTimer += Time.deltaTime;
			if (!reference && fakeBodyRealPosition[0].z < 0f - headToGround)
			{
				reference = true;
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				base.transform.position = zero;
				SEMgr.Inst.monster26BigJump.PlaySE();
				for (int i = 0; i < 5; i++)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster35_DirtLong", Tool2D.IgnoreZPoint(zero + Tool2D.GetDir() * 0.5f), 2f);
				}
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster35_DirtLong", Tool2D.IgnoreZPoint(zero + new Vector3(0f, -0.4f, 0f)), 2f);
				dirtParticle.Play();
				Tool2D.GetDir();
			}
			bool flag = true;
			for (int j = 0; j < fakeBodyRealPosition.Count; j++)
			{
				if (fakeBodyRealPosition[j].z > 0f - (headToGround + jumpAttackableHeight))
				{
					flag = false;
				}
			}
			if (totalJumpTimer > jumpTime / 2f && !reference2 && !flag)
			{
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				base.transform.position = zero;
				LocalTransform componentData2 = GetComponentData<LocalTransform>();
				componentData2.Position = zero;
				SetComponentData(componentData2);
			}
			if (flag && base.CC_Self.enabled)
			{
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
			}
			if ((fakeBodyRealPosition[fakeBodyRealPosition.Count - 1].z > 0f - headToGround) & reference2)
			{
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
			}
			if (totalJumpTimer > jumpTime / 2f && !reference2 && fakeBodyRealPosition[0].z > 0f - headToGround)
			{
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				base.transform.position = zero;
				LocalTransform componentData3 = GetComponentData<LocalTransform>();
				componentData3.Position = zero;
				SetComponentData(componentData3);
				if (reference3)
				{
					CamController.Inst.SetShock(0.5f, 15f, 0.5f);
					Vector3 dir = Tool2D.GetDir();
					if (!GameMgr.IsMobile_Static)
					{
						for (int k = 0; (float)k < dropSpikes; k++)
						{
							SummonSpike(Tool2D.GetDir(dir, (float)(k * 360) / dropSpikes));
						}
					}
				}
				else
				{
					CamController.Inst.SetShock(0.2f, 10f, 0.2f);
				}
				SEMgr.Inst.monster26BigLand.PlaySE();
				for (int l = 0; l < 5; l++)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster35_DirtLong", Tool2D.IgnoreZPoint(zero + Tool2D.GetDir() * 0.5f), 2f);
				}
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster35_DirtLong", Tool2D.IgnoreZPoint(zero + new Vector3(0f, -0.4f, 0f)), 2f);
				reference2 = true;
				dirtParticle.Play();
				for (int m = 0; m < dropParticles.Count; m++)
				{
					dropParticles[m].Play();
				}
			}
			if (JumpFixing())
			{
				base.transform.position = zero;
				LocalTransform componentData4 = GetComponentData<LocalTransform>();
				componentData4.Position = zero;
				SetComponentData(componentData4);
				if (UnityEngine.Random.Range(0f, 1f) < undergroundChance && !reference3)
				{
					state = MonsterState.UnderGround;
				}
				else
				{
					state = MonsterState.Show;
				}
			}
			SetMove(Vector3.zero);
			break;
		}
		case MonsterState.Shoot:
			if (changedState)
			{
				base.Anima.Play("Monster35_shoot");
			}
			BodyIdle();
			SetMove(Vector3.zero, isFlip: false);
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	private void SummonSpike(Vector3 diration)
	{
		bool flag = false;
		for (int i = 0; i < spikeWavePool.Count; i++)
		{
			if (spikeWavePool[i].useable)
			{
				spikeWavePool[i].targetEntity = targetEntity;
				spikeWavePool[i].useable = false;
				spikeWavePool[i].spiking = true;
				spikeWavePool[i].spikeDiration = diration;
				spikeWavePool[i].tracking = true;
				flag = true;
				return;
			}
		}
		if (!flag)
		{
			Monster35_SpikeWave component = UnityEngine.Object.Instantiate(wavePrefab, LevelMgr.Inst.CurrentRoomT).GetComponent<Monster35_SpikeWave>();
			spikeWavePool.Add(component);
			component.useable = false;
			component.master = this;
			component.spiking = true;
			component.tracking = true;
			component.targetEntity = targetEntity;
			component.spikeDiration = diration;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Attack":
			if (base.HaveTarget)
			{
				CamController.Inst.SetShock(0.3f, 15f, 0.1f);
				dirtParticle.Play();
				for (int i = 0; (float)i < spikeLines; i++)
				{
					spikeRange.RandomResult();
					SummonSpike(Tool2D.GetDir());
				}
			}
			break;
		case "HideImmume":
		{
			base.CC_Self.enabled = false;
			SetDotsCCEnable(isOpen: false);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = false;
			SetComponentData(componentData);
			break;
		}
		case "HideDone":
			state = MonsterState.UnderGround;
			break;
		case "Gulu":
			SEMgr.Inst.elite2Gulu.PlaySE();
			break;
		case "ShootBullet":
			GetNearestTargetPlayerFirst();
			if (base.HaveTarget)
			{
				SEMgr.Inst.monster35Shoot.PlaySE();
				UnityEngine.Object.Instantiate(boneBallPrefab, base.transform.position - new Vector3(0f, 0f, ballHeight), Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Monster35_BoneBall>().Iniaitlize(base.TargetPoint, this);
			}
			else
			{
				fakeTarget = new Vector3(UnityEngine.Random.Range((0f - roomWidth) / 4f, roomWidth / 4f), UnityEngine.Random.Range((0f - roomHeight) / 4f, roomHeight / 4f), 0f) + roomCenterPoint;
				SEMgr.Inst.monster35Shoot.PlaySE();
				UnityEngine.Object.Instantiate(boneBallPrefab, base.transform.position - new Vector3(0f, 0f, ballHeight), Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Monster35_BoneBall>().Iniaitlize(fakeTarget, this);
			}
			break;
		case "ShootDone":
			state = MonsterState.Idle;
			break;
		case "ShowDone":
			state = MonsterState.Idle;
			break;
		case "DirtPlay":
			SEMgr.Inst.monster35BreakEarth.PlaySE();
			dirtParticle.Play();
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}
}
