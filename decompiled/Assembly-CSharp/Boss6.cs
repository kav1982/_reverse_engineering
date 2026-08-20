using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;

public class Boss6 : UnitBase
{
	public enum MonsterState
	{
		BornIdleHide,
		BornIdleShow,
		BornIdleShout,
		Idle,
		ContinueAttackBefore,
		ContinueAttack,
		ContinueAttackAfter,
		Hide,
		UnderGround,
		Show,
		KnockGround,
		Summon,
		ExplodeCannon,
		BulletRain,
		SwitchStageBefore,
		SwitchStageFly
	}

	[Header("状态")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("登场")]
	public Boss6_DirtGenerator bornDirtParticle;

	public float checkPlayerDistance;

	public float shockTime;

	public ShockParam shockParam;

	public ShockParam shockFinishParam;

	[Header("贝塞尔曲线身体表现")]
	public Transform tsf_TargetRoot;

	public Sprite sprite_Body;

	public Sprite sprite_Head;

	public Sprite sprite_BodyBack;

	public Sprite sprite_BodyBack1;

	public Sprite sprite_BodyBack2;

	public Sprite sprite_HeadBack;

	public Sprite sprite_Tail;

	public SpriteRenderer SR_Head;

	public Transform headEffectRoot;

	public SpriteMask SR_HeadMask;

	public List<SpriteRenderer> SRs_Body = new List<SpriteRenderer>();

	public List<SpriteMask> SRs_Mask = new List<SpriteMask>();

	public bool useBezierBody;

	public List<Transform> bezierTargetPoints = new List<Transform>();

	public int bezierRecordPointsCount;

	public List<Vector3> recordBezierPoints = new List<Vector3>();

	public List<float> recordBeizerPointsDistance = new List<float>();

	public float bodyInterval;

	public float nowFaceAngle;

	private Vector3 nowFaceDir;

	private int noMaskBodyIndex;

	public float bodyMaskOffset;

	[Header("手部表现")]
	public List<Boss6_Hand> hands = new List<Boss6_Hand>();

	public List<float> handsNowPhase = new List<float>();

	public float handsWaveSpeed;

	public float handsPhaseInterval;

	[Header("玩家隐身处理和瞄准")]
	public bool headChasePlayer;

	public VariableFloat noTargetAimDistance;

	public float headRotateSpeed;

	public Vector3 headExpectedDir;

	public float headStartLerpAngle;

	[Header("影子")]
	public List<SpriteRenderer> SRs_Shadow = new List<SpriteRenderer>();

	public float shadowfadeMinDistance;

	public float shadowfadeMaxDistance;

	public float shadowOriginAlpha;

	[Header("地面")]
	public Boss6_Dirt dirt;

	[Header("额外视角")]
	public float extraViewScale;

	[Header("头部翻转控制")]
	private Vector3 attackPointOffset;

	[Header("连续扫射攻击")]
	public SortingGroup particleGroup;

	public Transform tsf_AttackPoint;

	public ParticleSystem continueAttackParticle;

	public ParticleSystem continueAttackParticle_H;

	public float continueShootInterval;

	public float continueShootDistance;

	public VariableFloat continueShootOffsetRange;

	public float continueBulletGravity;

	public float continueBulletBounceRatio;

	public VariableFloat continueBulletUpSpeed;

	public VariableFloat continueBulletDropSpeed;

	public VariableFloat continueBulletDropAngle;

	public float continueAttackBounceSpeed;

	public float continueAngleRotateSpeed;

	public float continueRotateAngle;

	public float continueStartOffsetAngle;

	private float continueRotateRight;

	private Vector3 continueAttackDir;

	[Header("爆炸抛物线子弹")]
	public VariableFloat cannonShootOffsetRange;

	public VariableInt cannonShootCount;

	public float cannonGravity;

	public float cannonUpSpeed;

	public int cannonShootRounds;

	private int cannonRoundsCounter;

	public ParticleSystem cannonShootParticle;

	public ParticleSystem cannonShootParticle_H;

	[Header("行动")]
	public VariableFloat actCD;

	private float actCDTimer;

	public VariableInt skillCountBeforeReposition;

	private int skillCounter;

	[Header("快乐钻地")]
	public int underGroundCorpseCount;

	public VariableFloat underGroundCorpseSpeed;

	public VariableFloat underGroundCorpseUpSpeed;

	public ShockParam hideShowShock;

	public float chanceToChase;

	public VariableFloat underGroundRepositionDistance;

	public float keepFromBorderDistance;

	public ShockParam underGroundShock;

	public ParticleSystem showParticle;

	public ParticleSystem showParticle_H;

	public Boss6_DamageZone damageZone;

	[Header("裂地头槌")]
	public float earthQuakeAngle;

	public float earthQuakeMinAngle;

	public float earthQuakeStartConstraintDistance;

	public float earthQuakeMaxConstraintDistance;

	public int earthQuakeCount;

	public int knockGroundDamage;

	public int knockGroundKnockback;

	public float knockGroundDistance;

	public float knockGroundRadius;

	public float knockGroundTime;

	public ParticleSystem knockGroundParticle;

	public ParticleSystem knockGroundParticle_H;

	public VariableFloat knockDirtCount;

	public ShockParam knockGroundShock;

	[Header("召唤")]
	public int childCount;

	public int reSummonChildCount;

	public float reSummonOnceCount;

	public VariableFloat childKeepDistance;

	public float SummonChance;

	public ParticleSystem summonParticle;

	public List<Boss6_Child> children = new List<Boss6_Child>();

	private List<float> childrenAngle = new List<float>();

	private List<float> childrenAngleDelta = new List<float>();

	public float mobileMaxSummonCount;

	private float mobileMaxSummonCounter;

	[Header("转阶段")]
	public float switchStageRotateRange;

	public float switchStageRotateFrequency;

	public float switchStageFlyHeight;

	public int flyPointsCount;

	public List<Vector3> flyPoints = new List<Vector3>();

	public List<float> flyPointsInterval = new List<float>();

	public float flyRecordDistance;

	public float flySpeed;

	public float flyCorpseBurstInterval;

	private bool switchStageCorpseOut;

	private bool bossDeadStay;

	private bool switchStageShout;

	public ShockParam switchStageShoutShock;

	public ShockParam switchStageShock;

	[Header("音效")]
	public AudioSource as_UnderGround;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	[Header("和谐")]
	public Sprite sprite_Head_H;

	[Header("光照材质")]
	public Material mat_DR;

	public Material mat_NODR;

	public static Boss6 Inst;

	private List<MonsterState> skills = new List<MonsterState>
	{
		MonsterState.KnockGround,
		MonsterState.ContinueAttackBefore,
		MonsterState.ExplodeCannon
	};

	private MonsterState lastSkill;

	private MonsterState foreLastSkill;

	private List<UnitDotsSyncSystem.DistanceHitResult> targetsInRange = new List<UnitDotsSyncSystem.DistanceHitResult>();

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

	public int bodyCount => SRs_Body.Count;

	public bool faceDown => Mathf.Abs(nowFaceAngle) >= 90f;

	public override void SingleInitialCallback()
	{
		for (int i = 0; i < SRs_Shadow.Count; i++)
		{
			myPpt.RemoveSRFromArray(SRs_Shadow[i]);
		}
		for (int j = 0; j < dirt.SR_dirts.Count; j++)
		{
			myPpt.RemoveSRFromArray(dirt.SR_dirts[j]);
		}
		attackPointOffset = tsf_AttackPoint.position - SR_Head.transform.position;
		for (int k = 0; k < hands.Count; k++)
		{
			handsNowPhase.Add(0f);
		}
		if (GameMgr.IsChAge14_Static)
		{
			cannonShootParticle = cannonShootParticle_H;
			continueAttackParticle = continueAttackParticle_H;
			knockGroundParticle = knockGroundParticle_H;
		}
		if (GameMgr.IsHarmony_Static)
		{
			showParticle = showParticle_H;
			sprite_Head = sprite_Head_H;
			SR_Head.sprite = sprite_Head_H;
		}
		if (GameMgr.IsMobile_Static)
		{
			earthQuakeCount--;
			continueBulletDropSpeed.value1 *= 0.8f;
			continueBulletDropSpeed.value2 *= 0.8f;
			childCount -= 2;
			reSummonOnceCount -= 1f;
		}
	}

	public override void EveryInitialCallback()
	{
		Inst = this;
		ObjPoolMgr.Inst.PreloadGO("Prefabs/Units/500621", 1f, ObjPoolMgr.PreloadType.Unit);
		ObjPoolMgr.Inst.PreloadGO("Prefabs/Units/500622", 16f, ObjPoolMgr.PreloadType.Unit);
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y;
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		state = MonsterState.BornIdleHide;
		base.Anima.Play("UnderGround");
		skillCountBeforeReposition.RandomResult();
		tsf_TargetRoot.gameObject.SetActive(value: false);
		headChasePlayer = false;
		headExpectedDir = Tool2D.GetDir(135f);
		nowFaceDir = Tool2D.GetDir(135f);
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanBeTarget = false;
		componentData.CanTouch = false;
		SetComponentData(componentData);
		SR_Head.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		UnityEngine.Object.Destroy(SR_Head.material);
		SR_Head.material = mat_NODR;
		for (int i = 0; i < bodyCount; i++)
		{
			SetBodyMaterial(i, mat_NODR);
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
		as_UnderGround.volume = DataMgr.settingData.GetFinalSound();
	}

	private void SetBodyMaterial(int i, Material material)
	{
		material.color = myPpt.BaseColor;
		UnityEngine.Object.Destroy(SRs_Body[i].material);
		UnityEngine.Object.Destroy(hands[i].rightHand.material);
		UnityEngine.Object.Destroy(hands[i].leftHand.material);
		UnityEngine.Object.Destroy(hands[i].rightHandRoot.material);
		UnityEngine.Object.Destroy(hands[i].leftHandRoot.material);
		SRs_Body[i].material = material;
		hands[i].rightHand.material = material;
		hands[i].leftHand.material = material;
		hands[i].rightHandRoot.material = material;
		hands[i].leftHandRoot.material = material;
	}

	private void SetBodyFadeHeight()
	{
		SR_Head.material.SetFloat(GameConstManaged.shaderGroundHeightIndex, base.transform.position.y);
		for (int i = 0; i < SRs_Body.Count; i++)
		{
			SRs_Body[i].material.SetFloat(GameConstManaged.shaderGroundHeightIndex, base.transform.position.y);
			hands[i].rightHand.material.SetFloat(GameConstManaged.shaderGroundHeightIndex, base.transform.position.y);
			hands[i].leftHand.material.SetFloat(GameConstManaged.shaderGroundHeightIndex, base.transform.position.y);
			hands[i].rightHandRoot.material.SetFloat(GameConstManaged.shaderGroundHeightIndex, base.transform.position.y);
			hands[i].leftHandRoot.material.SetFloat(GameConstManaged.shaderGroundHeightIndex, base.transform.position.y);
		}
	}

	private void SetBezierInfo(float faceAngle)
	{
		recordBezierPoints.Clear();
		recordBeizerPointsDistance.Clear();
		Vector3[] array = new Vector3[bezierTargetPoints.Count];
		for (int i = 0; i < bezierTargetPoints.Count; i++)
		{
			Vector3 vector = (array[i] = base.transform.position + -Tool2D.GetDir(faceAngle) * (bezierTargetPoints[i].position - base.transform.position).x - Vector3.forward * (bezierTargetPoints[i].position - base.transform.position).y);
		}
		for (int j = 0; j < bezierRecordPointsCount; j++)
		{
			recordBezierPoints.Add(GeneralTool.FreeBezierCurve((float)j / (float)bezierRecordPointsCount, array));
			if (j >= 1)
			{
				recordBeizerPointsDistance.Add((recordBezierPoints[j] - recordBezierPoints[j - 1]).magnitude);
			}
		}
	}

	private Vector3 GetBodyPoint(int bodyIndex)
	{
		if (bodyIndex == -1)
		{
			return recordBezierPoints[0];
		}
		float num = ((float)bodyIndex + 0.5f) * bodyInterval;
		int num2 = 0;
		for (int i = 0; i < recordBeizerPointsDistance.Count && !(num < recordBeizerPointsDistance[i]); i++)
		{
			num -= recordBeizerPointsDistance[i];
			num2++;
		}
		if (num2 < bezierRecordPointsCount - 1)
		{
			return recordBezierPoints[num2] + (recordBezierPoints[num2 + 1] - recordBezierPoints[num2]).normalized * num;
		}
		return recordBezierPoints[bezierRecordPointsCount - 1];
	}

	private Vector3 GetFlyBodyPoint(int bodyIndex)
	{
		if (bodyIndex == -1)
		{
			return flyPoints[0];
		}
		float num = ((float)bodyIndex + 0.8f) * bodyInterval;
		int num2 = 0;
		for (int i = 0; i < flyPointsInterval.Count && !(num < flyPointsInterval[i]); i++)
		{
			num -= flyPointsInterval[i];
			num2++;
		}
		if (num2 < flyPointsCount - 1)
		{
			return flyPoints[num2] + (flyPoints[num2 + 1] - flyPoints[num2]).normalized * num;
		}
		return flyPoints[flyPointsCount - 1];
	}

	private int GetNoMaskPoint()
	{
		int num = -1;
		for (int i = 0; i < recordBezierPoints.Count; i++)
		{
			if (recordBezierPoints[i].z < (0f - bodyInterval) * 0.5f)
			{
				num = i;
			}
		}
		if (num == -1)
		{
			return -2;
		}
		float num2 = 0f;
		for (int j = 0; j < num; j++)
		{
			num2 += recordBeizerPointsDistance[j];
		}
		num2 += bodyInterval * 0.2f;
		return Mathf.FloorToInt(num2 / bodyInterval) - 1;
	}

	private void TryAct()
	{
		if (bossDeadStay)
		{
			state = MonsterState.SwitchStageBefore;
			return;
		}
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
		if (skillCounter >= skillCountBeforeReposition.result)
		{
			skillCountBeforeReposition.RandomResult();
			skillCounter = 0;
			state = MonsterState.Hide;
			return;
		}
		skillCounter++;
		for (state = skills[GeneralTool.GetWeightRandom(1f, 1f, 1f)]; state == lastSkill; state = skills[GeneralTool.GetWeightRandom(1f, 1f, 1f)])
		{
		}
		foreLastSkill = lastSkill;
		lastSkill = state;
	}

	public override void Update()
	{
		if (bossDeadStay)
		{
			myPpt.ClearBurnState();
			if (myPpt.affect_FrozenTime > 0f)
			{
				myPpt.ClearFrozenState();
				base.Rigid.linearVelocity = Vector3.zero;
				base.CurrentMotion = Vector3.zero;
			}
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
		SetBodyFadeHeight();
		if (useBezierBody)
		{
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (headChasePlayer && base.HaveTarget)
			{
				headExpectedDir = ToTargetDir();
			}
			float num2 = Tool2D.IgnoreZAngle(nowFaceDir, headExpectedDir);
			nowFaceDir = Tool2D.RotateTowardsAroundZAxis(nowFaceDir, headExpectedDir, Mathf.Min(1f, num2 / headStartLerpAngle) * headRotateSpeed * Time.deltaTime);
			nowFaceAngle = Tool2D.IgnoreZAngleWithSign(Vector3.up, nowFaceDir);
			SetBezierInfo(nowFaceAngle);
			Vector3 bodyPoint = GetBodyPoint(-1);
			Vector3 position = Tool2D.GetLayerPoint(bodyPoint) + new Vector3(0f, 0f, bodyMaskOffset * 0.01f);
			if (position.y < base.transform.position.y + bodyMaskOffset && SR_Head.maskInteraction != 0)
			{
				position.z = Tool2D.GetLayerPoint(base.transform.position).z;
			}
			SR_HeadMask.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f) + new Vector3(0f, 0f, position.z);
			SR_Head.transform.position = position;
			SR_Head.flipX = nowFaceAngle > 0f;
			headEffectRoot.localScale = new Vector3((!(nowFaceAngle > 0f)) ? 1 : (-1), 1f, faceDown ? 1 : (-1));
			if (faceDown)
			{
				particleGroup.sortingOrder = 0;
			}
			else
			{
				particleGroup.sortingOrder = -1;
			}
			SR_Head.sprite = (faceDown ? sprite_Head : sprite_HeadBack);
			SRs_Shadow[bodyCount].transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(bodyPoint), LayerCorrectType.Shadow);
			float num3 = Tool2D.IgnoreZDistance(base.transform.position, bodyPoint);
			SRs_Shadow[bodyCount].color = new Color(0f, 0f, 0f, shadowOriginAlpha * Mathf.Lerp(0f, 1f, (num3 - shadowfadeMinDistance) / (shadowfadeMaxDistance - shadowfadeMinDistance)));
			if (bodyPoint.z < bodyInterval)
			{
				SRs_Shadow[bodyCount].enabled = true;
			}
			else
			{
				SRs_Shadow[bodyCount].enabled = false;
			}
			int noMaskPoint = GetNoMaskPoint();
			if (noMaskPoint >= -1)
			{
				if (SR_Head.maskInteraction != 0)
				{
					SR_Head.maskInteraction = SpriteMaskInteraction.None;
					UnityEngine.Object.Destroy(SR_Head.material);
					SR_Head.material = mat_DR;
				}
			}
			else if (SR_Head.maskInteraction != SpriteMaskInteraction.VisibleOutsideMask)
			{
				SR_Head.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
				UnityEngine.Object.Destroy(SR_Head.material);
				SR_Head.material = mat_NODR;
			}
			for (int i = 0; i < bodyCount; i++)
			{
				Vector3 bodyPoint2 = GetBodyPoint(i);
				Vector3 position2 = Tool2D.GetLayerPoint(bodyPoint2) + new Vector3(0f, 0f, bodyMaskOffset * 0.01f);
				if (noMaskPoint >= i)
				{
					if (SRs_Body[i].maskInteraction != 0)
					{
						SRs_Body[i].maskInteraction = SpriteMaskInteraction.None;
						SetBodyMaterial(i, mat_DR);
					}
				}
				else if (SRs_Body[i].maskInteraction != SpriteMaskInteraction.VisibleOutsideMask)
				{
					SRs_Body[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
					SetBodyMaterial(i, mat_NODR);
				}
				if (position2.y < base.transform.position.y + bodyMaskOffset && SRs_Body[i].maskInteraction != 0)
				{
					position2.z = Tool2D.GetLayerPoint(base.transform.position).z;
				}
				if (faceDown && position2.z > Tool2D.GetLayerPoint(base.transform.position).z)
				{
					position2.z = Tool2D.GetLayerPoint(base.transform.position).z;
				}
				SRs_Mask[i].transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f) + new Vector3(0f, 0f, position2.z);
				SRs_Body[i].transform.position = position2;
				Vector3 position3;
				Vector3 position4;
				if (i == 0)
				{
					position3 = SR_Head.transform.position;
					position4 = SRs_Body[i + 1].transform.position;
				}
				else if (i < bodyCount - 1)
				{
					position3 = SRs_Body[i - 1].transform.position;
					position4 = SRs_Body[i + 1].transform.position;
				}
				else
				{
					position3 = SRs_Body[i - 1].transform.position;
					position4 = SRs_Body[i].transform.position;
				}
				if (faceDown && position3.z > SRs_Body[i].transform.position.z)
				{
					Vector3 position5 = SRs_Body[i].transform.position;
					position5.z = position3.z + 0.001f;
					SRs_Mask[i].transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f) + new Vector3(0f, 0f, position5.z);
					SRs_Body[i].transform.position = position5;
				}
				Vector3 to = position3 - position4;
				SRs_Body[i].transform.eulerAngles = Vector3.forward * Tool2D.IgnoreZAngleWithSign(Vector3.up, to);
				SRs_Body[i].flipX = nowFaceAngle >= 0f;
				SRs_Body[i].sprite = (faceDown ? sprite_Body : sprite_BodyBack);
				if (!faceDown && position3.z > SRs_Body[i].transform.position.z)
				{
					SRs_Body[i].sprite = sprite_BodyBack1;
					if (position4.z > SRs_Body[i].transform.position.z)
					{
						SRs_Body[i].sprite = sprite_BodyBack2;
					}
				}
				if (i == SRs_Body.Count - 1)
				{
					SRs_Body[i].sprite = sprite_Tail;
				}
				SRs_Shadow[i].transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(bodyPoint2), LayerCorrectType.Shadow);
				float num4 = Tool2D.IgnoreZDistance(base.transform.position, bodyPoint2);
				SRs_Shadow[i].color = new Color(0f, 0f, 0f, shadowOriginAlpha * Mathf.Lerp(0f, 1f, (num4 - shadowfadeMinDistance) / (shadowfadeMaxDistance - shadowfadeMinDistance)));
				if (bodyPoint2.z < bodyInterval)
				{
					SRs_Shadow[i].enabled = true;
				}
				else
				{
					SRs_Shadow[i].enabled = false;
				}
			}
		}
		else
		{
			Vector3 position6 = Tool2D.GetLayerPoint(GetFlyBodyPoint(-1)) + new Vector3(0f, 0f, bodyMaskOffset * 0.01f);
			if (position6.y < base.transform.position.y + bodyMaskOffset && SR_Head.maskInteraction != 0)
			{
				position6.z = Tool2D.GetLayerPoint(base.transform.position).z;
			}
			SR_HeadMask.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f) + new Vector3(0f, 0f, position6.z);
			SR_Head.transform.position = position6;
			SR_Head.transform.localScale = Vector3.one;
			for (int j = 0; j < bodyCount; j++)
			{
				Vector3 flyBodyPoint = GetFlyBodyPoint(j);
				if (0f - flyBodyPoint.z > bodyInterval * 0.8f)
				{
					if (SRs_Body[j].maskInteraction != 0)
					{
						SRs_Body[j].maskInteraction = SpriteMaskInteraction.None;
						SetBodyMaterial(j, mat_DR);
					}
				}
				else if (SRs_Body[j].maskInteraction != SpriteMaskInteraction.VisibleOutsideMask)
				{
					SRs_Body[j].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
					SetBodyMaterial(j, mat_NODR);
				}
				Vector3 position7 = Tool2D.GetLayerPoint(flyBodyPoint) + new Vector3(0f, 0f, bodyMaskOffset * 0.01f);
				if (position7.y < base.transform.position.y + bodyMaskOffset && SRs_Body[j].maskInteraction != 0)
				{
					position7.z = Tool2D.GetLayerPoint(base.transform.position).z;
				}
				SRs_Mask[j].transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f) + new Vector3(0f, 0f, position7.z);
				SRs_Body[j].transform.position = position7;
				Vector3 position8;
				Vector3 position9;
				if (j == 0)
				{
					position8 = SR_Head.transform.position;
					position9 = SRs_Body[j + 1].transform.position;
				}
				else if (j < bodyCount - 1)
				{
					position8 = SRs_Body[j - 1].transform.position;
					position9 = SRs_Body[j + 1].transform.position;
				}
				else
				{
					position8 = SRs_Body[j - 1].transform.position;
					position9 = SRs_Body[j].transform.position;
				}
				if (faceDown && position8.z > SRs_Body[j].transform.position.z)
				{
					Vector3 position10 = SRs_Body[j].transform.position;
					position10.z = position8.z + 0.0001f;
					SRs_Mask[j].transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f) + new Vector3(0f, 0f, position10.z);
					SRs_Body[j].transform.position = position10;
				}
				Vector3 to2 = position8 - position9;
				SRs_Body[j].transform.eulerAngles = Vector3.forward * Tool2D.IgnoreZAngleWithSign(Vector3.up, to2);
				SRs_Body[j].sprite = sprite_Body;
				if (j == SRs_Body.Count - 1)
				{
					SRs_Body[j].sprite = sprite_Tail;
				}
			}
		}
		for (int k = 0; k < hands.Count; k++)
		{
			if (k == 0)
			{
				handsNowPhase[k] += handsWaveSpeed * (MathF.PI / 180f) * Time.deltaTime;
			}
			else
			{
				handsNowPhase[k] = handsNowPhase[k - 1] + MathF.PI / 180f * handsPhaseInterval;
			}
			hands[k].SetSort(nowFaceAngle < 0f, faceDown);
			hands[k].SetAngle(handsNowPhase[k]);
			if (k == hands.Count - 1)
			{
				hands[k].SetInvisible();
			}
		}
		switch (state)
		{
		case MonsterState.BornIdleHide:
		{
			ref bool reference6 = ref varMgr.RegBool(0);
			if (changedState)
			{
				base.Anima.Play("UnderGround");
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanBeTarget = false;
				componentData.CanTouch = false;
				SetComponentData(componentData);
			}
			if (ToPointDistanceSqr(PlayerMgr.Inst.PlayerCtrller.transform.position) < checkPlayerDistance * checkPlayerDistance && !reference6)
			{
				as_UnderGround.Play();
				reference6 = true;
				bornDirtParticle.enabled = true;
			}
			if (!reference6)
			{
				stateExistTime = 0f;
				break;
			}
			CamController.Inst.SetShock(stateExistTime / shockTime * shockParam.radius, stateExistTime / shockTime * shockParam.speed, shockParam.time);
			if (stateExistTime > shockTime)
			{
				state = MonsterState.BornIdleShow;
			}
			break;
		}
		case MonsterState.BornIdleShow:
			if (changedState)
			{
				CamController.Inst.SetShock(shockFinishParam);
				CameraFocusSizeData data = new CameraFocusSizeData(extraViewScale, 1, 1000000f);
				CamController.Inst.AddNewCameraFocusRequirement(data);
				base.Anima.Play("BornShow");
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				headChasePlayer = true;
				base.Anima.Play("Idle");
			}
			TryAct();
			break;
		case MonsterState.ContinueAttackBefore:
			if (changedState)
			{
				SEMgr.Inst.boss6_Roar1.PlaySE();
				continueRotateRight = ((!GeneralTool.ChanceResult(0.5f)) ? 1 : (-1));
				headChasePlayer = false;
				base.Anima.Play("AttackBefore");
				GetNearestTargetPlayerFirst();
				continueAttackDir = ToPointDir(roomCenterPoint);
				if (base.HaveTarget)
				{
					continueAttackDir = ToTargetDir();
				}
				headExpectedDir = Tool2D.GetDir(continueAttackDir, continueRotateRight * continueStartOffsetAngle);
			}
			break;
		case MonsterState.ContinueAttack:
		{
			ref float reference7 = ref varMgr.RegFloat(0);
			ref float reference8 = ref varMgr.RegFloat(1);
			ref Vector3 reference9 = ref varMgr.RegV3(0);
			if (changedState)
			{
				base.Anima.Play("Attack");
				reference8 = 0f;
				reference9 = headExpectedDir;
			}
			reference8 += Time.deltaTime * (0f - continueRotateRight) * continueAngleRotateSpeed;
			headExpectedDir = Tool2D.GetDir(reference9, reference8);
			reference7 += Time.deltaTime;
			if (reference7 > continueShootInterval)
			{
				ShootBullet();
				reference7 -= continueShootInterval;
			}
			if (Mathf.Abs(reference8) > continueRotateAngle)
			{
				state = MonsterState.ContinueAttackAfter;
			}
			break;
		}
		case MonsterState.ContinueAttackAfter:
			if (changedState)
			{
				headChasePlayer = true;
				base.Anima.Play("AttackAfter");
			}
			break;
		case MonsterState.Hide:
			if (changedState)
			{
				base.Anima.Play("Hide");
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.UnderGround:
		{
			ref Vector3 reference4 = ref varMgr.RegV3(0);
			if (changedState)
			{
				damageZone.Open();
				as_UnderGround.Play();
				base.Anima.Play("UnderGround");
				if (GeneralTool.ChanceResult(chanceToChase))
				{
					reference4 = Tool2D.GetNavMeshPointIngoreZ(base.transform.position, underGroundRepositionDistance, nowFaceDir, 0f);
				}
				else
				{
					reference4 = Tool2D.GetNavMeshPointIngoreZ(base.transform.position, underGroundRepositionDistance);
				}
				if (Mathf.Abs(reference4.x - roomCenterPoint.x) > roomWidth / 2f - keepFromBorderDistance)
				{
					reference4.x = Mathf.Sign(reference4.x - roomCenterPoint.x) * (roomWidth / 2f - keepFromBorderDistance) + roomCenterPoint.x;
				}
				if (Mathf.Abs(reference4.y - roomCenterPoint.y) > roomHeight / 2f - keepFromBorderDistance)
				{
					reference4.y = Mathf.Sign(reference4.y - roomCenterPoint.y) * (roomHeight / 2f - keepFromBorderDistance) + roomCenterPoint.y;
				}
			}
			base.transform.position += ToPointDir(reference4) * base.MoveSpeed * Time.deltaTime;
			SyncDotsPosition();
			if (ToPointDistanceSqr(reference4) < 0.04f)
			{
				as_UnderGround.Stop();
				state = MonsterState.Show;
			}
			CamController.Inst.SetShock(underGroundShock);
			break;
		}
		case MonsterState.Show:
			if (changedState)
			{
				base.Anima.Play("Show");
				damageZone.Close();
				base.Rigid.isKinematic = true;
				SyncDotsRigidKindmatic();
				base.CurrentMotion = Vector3.zero;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.KnockGround:
			if (changedState)
			{
				SEMgr.Inst.boss6_Roar2.PlaySE();
				base.Anima.Play("KnockGround");
				knockGroundParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + nowFaceDir * knockGroundDistance);
			}
			if (stateExistTime < knockGroundTime)
			{
				knockGroundParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + nowFaceDir * knockGroundDistance);
			}
			break;
		case MonsterState.Summon:
			if (changedState)
			{
				base.Anima.Play("Summon");
			}
			break;
		case MonsterState.ExplodeCannon:
			if (changedState)
			{
				SEMgr.Inst.boss6_Roar3.PlaySE();
				base.Anima.Play("Cannon", 0, 0f);
				cannonRoundsCounter = 0;
			}
			break;
		case MonsterState.SwitchStageBefore:
		{
			ref float reference5 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				base.Rigid.isKinematic = true;
				SyncDotsRigidKindmatic();
				base.Anima.Play("SwitchStageBefore", 0, 0f);
				headChasePlayer = false;
				headExpectedDir = Tool2D.GetDir(Vector3.right, -1f);
			}
			if (switchStageShout)
			{
				CamController.Inst.SetShock(switchStageShoutShock);
			}
			if (switchStageCorpseOut)
			{
				reference5 += Time.deltaTime;
				if (reference5 > flyCorpseBurstInterval)
				{
					reference5 -= flyCorpseBurstInterval;
					CurpseBurstSingle();
				}
			}
			SetMove(Vector3.zero);
			break;
		}
		case MonsterState.SwitchStageFly:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			ref bool reference2 = ref varMgr.RegBool(0);
			ref float reference3 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				as_UnderGround.Play();
				for (int l = 0; l < SRs_Shadow.Count; l++)
				{
					SRs_Shadow[l].enabled = false;
				}
				useBezierBody = false;
				for (int m = 0; m < flyPointsCount; m++)
				{
					if (m < recordBezierPoints.Count)
					{
						flyPoints.Add(recordBezierPoints[m]);
					}
					else
					{
						flyPoints.Add(recordBezierPoints[recordBezierPoints.Count - 1]);
					}
					if (m > 0)
					{
						flyPointsInterval.Add((flyPoints[m - 1] - flyPoints[m]).magnitude);
					}
				}
				reference = flyPoints[0];
			}
			flyPoints[0] += Time.deltaTime * flySpeed * (Quaternion.Euler(0f, switchStageRotateRange * Mathf.Sin(stateExistTime * switchStageRotateFrequency * MathF.PI * 2f), 0f) * Vector3.back);
			while ((flyPoints[0] - flyPoints[1]).sqrMagnitude > flyRecordDistance * flyRecordDistance)
			{
				for (int num5 = flyPointsCount - 1; num5 > 0; num5--)
				{
					if (num5 > 1)
					{
						flyPoints[num5] = flyPoints[num5 - 1];
					}
					else
					{
						flyPoints[num5] += flyRecordDistance * (flyPoints[0] - flyPoints[1]).normalized;
					}
				}
				for (int num6 = flyPointsCount - 2; num6 > 0; num6--)
				{
					if (num6 > 1)
					{
						flyPointsInterval[num6] = flyPointsInterval[num6 - 1];
					}
					else
					{
						flyPointsInterval[num6] = flyRecordDistance;
					}
				}
			}
			flyPointsInterval[0] = (flyPoints[0] - flyPoints[1]).magnitude;
			if (switchStageCorpseOut && !reference2)
			{
				reference3 += Time.deltaTime;
				if (reference3 > flyCorpseBurstInterval)
				{
					reference3 -= flyCorpseBurstInterval;
					CurpseBurstSingle();
				}
			}
			if (!reference2)
			{
				CamController.Inst.SetShock(switchStageShock);
			}
			if (GetFlyBodyPoint(bodyCount - 1).z < 0.5f && !reference2)
			{
				base.Anima.Play("DirtHide");
				reference2 = true;
				as_UnderGround.Stop();
				showParticle.Play();
				SEMgr.Inst.boss6_OutOfDirt.PlaySE();
				SEMgr.Inst.boss6_Show.PlaySE();
				CurpseBurst();
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_Hole" + (GameMgr.IsChAge14_Static ? "_H" : ""), base.transform.position);
			}
			if (flyPoints[0].z < 0f - switchStageFlyHeight)
			{
				DotsAnnouncedDeath();
			}
			SetMove(Vector3.zero);
			break;
		}
		case MonsterState.BornIdleShout:
		case MonsterState.BulletRain:
			break;
		}
	}

	private void KnockGround()
	{
		SEMgr.Inst.monster34Explosion.PlaySE();
		SEMgr.Inst.boss6_KnockGround.PlaySE();
		knockGroundParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + nowFaceDir * knockGroundDistance);
		knockGroundParticle.Play();
		CamController.Inst.SetShock(knockGroundShock);
		for (int i = 0; (float)i < knockDirtCount.RandomResult(); i++)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_SingleDirt" + (GameMgr.IsChAge14_Static ? " H" : ""), Tool2D.IgnoreZPoint(recordBezierPoints[0]) + UnityEngine.Random.value * Tool2D.GetDir() * knockGroundRadius * 3f, 2f);
		}
		float num = earthQuakeAngle;
		GetNearestTargetPlayerFirst();
		if (base.HaveTarget)
		{
			float num2 = ToTargetDistance();
			num = Mathf.Lerp(earthQuakeAngle, earthQuakeMinAngle, (num2 - earthQuakeStartConstraintDistance) / (earthQuakeMaxConstraintDistance - earthQuakeStartConstraintDistance));
		}
		for (int j = 0; j < earthQuakeCount; j++)
		{
			Vector3 dir = Tool2D.GetDir(Vector3.up, nowFaceAngle + num / (float)(earthQuakeCount - 1) * (float)j - num / 2f);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_EarthQuake", Tool2D.IgnoreZPoint(recordBezierPoints[0]) + dir * knockGroundRadius * 0.7f).GetComponent<Boss6_EarthQuake>().Initialize(isOriginal: true, dir, dir, 0);
		}
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position + nowFaceDir * knockGroundDistance, knockGroundRadius, GameConst.Filter_MonsterAoe, targetsInRange);
		for (int k = 0; k < targetsInRange.Count; k++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = targetsInRange[k];
			Entity entity = distanceHitResult.entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, knockGroundDamage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
					info.damage = knockGroundDamage;
					info.knockbackForce = Tool2D.IgnoreZPoint(distanceHitResult.point - base.transform.position).normalized * knockGroundKnockback;
					info.teammateTakeDamageRatio = 4f;
					UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				}
				break;
			}
		}
	}

	private void ShootBullet()
	{
		SEMgr.Inst.boss6_BulletShoot.PlaySE(SEPlayMode.Replay, 3, 0.2f);
		continueAttackParticle.Play();
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_BounceBullet" + (GameMgr.IsChAge14_Static ? " H" : ""), GetBodyPoint(-1) + tsf_AttackPoint.position - SR_Head.transform.position + Vector3.up * (faceDown ? 0f : 0.5f)).GetComponent<Boss6_Cannon>().Initialize(base.transform.position + nowFaceDir * continueShootDistance + continueShootOffsetRange.RandomResult() * Tool2D.GetDir(), continueBulletUpSpeed.RandomResult(), continueBulletGravity, continueBulletDropSpeed, continueBulletDropAngle, continueAttackBounceSpeed);
	}

	private void ShootCannon()
	{
		SEMgr.Inst.boss6_Cannon.PlaySE();
		SEMgr.Inst.boss6_Cannon1.PlaySE();
		cannonShootParticle.Play();
		cannonShootCount.RandomResult();
		for (int i = 0; i < cannonShootCount.result; i++)
		{
			Boss6_Cannon component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_BounceBulletBig" + (GameMgr.IsChAge14_Static ? " H" : ""), GetBodyPoint(-1) + tsf_AttackPoint.position - SR_Head.transform.position + Vector3.up * (faceDown ? 0f : 0.5f)).GetComponent<Boss6_Cannon>();
			Vector3 targetPoint = roomCenterPoint;
			GetNearestTarget();
			if (base.HaveTarget)
			{
				targetPoint = base.TargetPoint;
			}
			component.Initialize(Tool2D.GetNavMeshPoint(targetPoint + cannonShootOffsetRange.RandomResult() * Tool2D.GetDir()), cannonUpSpeed, cannonGravity, continueBulletDropSpeed, continueBulletDropAngle, continueAttackBounceSpeed, isExplode: true);
		}
	}

	private void CurpseBurst()
	{
		for (int i = 0; i < underGroundCorpseCount; i++)
		{
			Vector3 dir = Tool2D.GetDir();
			CorpseSystem.Inst.CreateCorpse(CorpseType.Boss6, base.transform.position + base.CC_Self.radius * dir * 0.5f, dir * underGroundCorpseSpeed.RandomResult(), underGroundCorpseUpSpeed.RandomResult());
		}
	}

	private void CurpseBurstSingle()
	{
		Vector3 dir = Tool2D.GetDir();
		CorpseSystem.Inst.CreateCorpse(CorpseType.Boss6, base.transform.position + base.CC_Self.radius * dir * 0.5f, dir * underGroundCorpseSpeed.RandomResult(), underGroundCorpseUpSpeed.RandomResult());
	}

	private void Summon()
	{
		if (bossDeadStay)
		{
			return;
		}
		int num = (int)Mathf.Min(childCount - children.Count, reSummonOnceCount);
		for (int i = 0; i < num; i++)
		{
			if (GameMgr.IsMobile_Static)
			{
				if (mobileMaxSummonCounter > mobileMaxSummonCount)
				{
					break;
				}
				mobileMaxSummonCounter += 1f;
			}
			Vector3 navMeshPoint = Tool2D.GetNavMeshPoint(roomCenterPoint, childKeepDistance, GetSortDir(), 5f);
			Boss6_Child component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/500653", navMeshPoint).GetComponent<Boss6_Child>();
			component.Initialize(this);
			children.Add(component);
		}
	}

	private Vector3 GetSortDir()
	{
		if (children.Count < 3)
		{
			return Tool2D.GetDir();
		}
		children.Sort();
		childrenAngle.Clear();
		childrenAngleDelta.Clear();
		for (int i = 0; i < children.Count; i++)
		{
			float num = Tool2D.IgnoreZAngleWithSign(Vector3.up, children[i].transform.position - roomCenterPoint);
			if (num < 0f)
			{
				num += 360f;
			}
			childrenAngle.Add(num);
		}
		for (int j = 0; j < childrenAngle.Count; j++)
		{
			int num2 = j + 1;
			if (num2 >= childrenAngle.Count)
			{
				num2 = 0;
			}
			float num3 = childrenAngle[j] - childrenAngle[num2];
			if (num3 < 0f)
			{
				num3 += 360f;
			}
			childrenAngleDelta.Add(num3);
		}
		int index = 0;
		float num4 = 0f;
		for (int k = 0; k < childrenAngleDelta.Count; k++)
		{
			if (childrenAngleDelta[k] > num4)
			{
				index = k;
				num4 = childrenAngleDelta[k];
			}
		}
		return Tool2D.GetDir(Vector3.up, childrenAngle[index] - childrenAngleDelta[index] / 2f);
	}

	public override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		GameUISingletonMono<UIBossHP>.HideIfInited();
		info.stopAnnouncedDeath = true;
		if (!bossDeadStay)
		{
			bossDeadStay = true;
			if (state == MonsterState.UnderGround)
			{
				state = MonsterState.Show;
			}
			base.CC_Self.enabled = false;
			SetDotsCCEnable(isOpen: false);
			myPpt.enabled = false;
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.BossDeadStay();
			SetComponentData(componentData);
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + 500621, roomCenterPoint + Vector3.up * roomHeight);
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Cannon":
			ShootCannon();
			break;
		case "CannonFinish":
			cannonRoundsCounter++;
			if (cannonShootRounds <= cannonRoundsCounter)
			{
				state = MonsterState.Idle;
			}
			else
			{
				base.Anima.Play("Cannon", 0, 0f);
			}
			break;
		case "Summon":
			Summon();
			summonParticle.Play();
			break;
		case "SummonStop":
			summonParticle.Stop();
			break;
		case "SummonFinish":
			state = MonsterState.Idle;
			break;
		case "Shout":
		{
			SEMgr.Inst.boss6_SwitchStage.PlaySE();
			summonParticle.Play();
			for (int i = 0; i < children.Count; i++)
			{
				children[i].DotsAnnouncedDeath();
			}
			switchStageShout = true;
			break;
		}
		case "ShoutFinish":
			summonParticle.Stop();
			switchStageShout = false;
			break;
		case "SwitchStageCorpseOut":
			switchStageCorpseOut = true;
			break;
		case "SwitchStageBeforeFinish":
			state = MonsterState.SwitchStageFly;
			break;
		case "StopChase":
			headChasePlayer = false;
			break;
		case "StartChase":
			headChasePlayer = true;
			break;
		case "Hide":
		{
			base.CC_Self.enabled = false;
			SetDotsCCEnable(isOpen: false);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanBeTarget = false;
			componentData.CanTouch = false;
			SetComponentData(componentData);
			CamController.Inst.SetShock(hideShowShock);
			SEMgr.Inst.boss6_Hide.PlaySE();
			Summon();
			break;
		}
		case "Show":
			showParticle.Play();
			CurpseBurst();
			CamController.Inst.SetShock(hideShowShock);
			SEMgr.Inst.boss6_OutOfDirt.PlaySE();
			SEMgr.Inst.boss6_Show.PlaySE();
			as_UnderGround.Stop();
			if (!bossDeadStay)
			{
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanBeTarget = true;
				componentData.CanTouch = true;
				SetComponentData(componentData);
			}
			break;
		case "HideDone":
			state = MonsterState.UnderGround;
			break;
		case "ShowDone":
			state = MonsterState.Idle;
			break;
		case "KnockGround":
			KnockGround();
			break;
		case "KnockGroundFinish":
			state = MonsterState.Idle;
			break;
		case "BornShowFinish":
			state = MonsterState.Idle;
			break;
		case "AttackBeforeFinish":
			state = MonsterState.ContinueAttack;
			break;
		case "AttackAfterFinish":
			state = MonsterState.Idle;
			break;
		case "DirtShow":
			dirt.Show();
			break;
		case "DirtHide":
			dirt.Hide();
			break;
		case "FinalDirtHide":
			dirt.FinalHide();
			break;
		}
	}
}
