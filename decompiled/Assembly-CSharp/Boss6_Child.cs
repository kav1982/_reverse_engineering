using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class Boss6_Child : UnitBase, IComparable
{
	public enum MonsterState
	{
		Born,
		Idle,
		WaitTarget,
		AttackBefore,
		Attack,
		AttackAfter,
		NoTargetAttack,
		Show,
		Hide,
		UnderGround,
		KnockGround
	}

	[Header("状态")]
	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	public StateVariableMgr varMgr = new StateVariableMgr();

	public Boss6 master;

	private float stateExistTime;

	[Header("贝塞尔曲线身体表现")]
	public Transform tsf_Model;

	public Transform tsf_TargetRoot;

	public Sprite sprite_Body;

	public Sprite sprite_Head;

	public Sprite sprite_Head_Attacking;

	public Sprite sprite_BodyBack;

	public Sprite sprite_HeadBack;

	public SpriteRenderer SR_Head;

	public List<SpriteRenderer> SRs_Body = new List<SpriteRenderer>();

	public bool useBezierBody;

	public List<Transform> bezierTargetPoints = new List<Transform>();

	public int bezierRecordPointsCount;

	public List<Vector3> recordBezierPoints = new List<Vector3>();

	public List<float> recordBeizerPointsDistance = new List<float>();

	public float bodyInterval;

	public float nowFaceAngle;

	public float headRotateSpeed;

	private bool headChasePlayer;

	private Vector3 headExpectedDir;

	private Vector3 nowFaceDir;

	private int noMaskBodyIndex;

	public float bodyMaskOffset;

	[Header("活动")]
	public VariableFloat bornDelay;

	public VariableFloat idleTime;

	public VariableFloat waitTargetTime;

	[Header("吐子弹")]
	public float attackDistance;

	public VariableFloat bulletLandRadius;

	public float bulletShootInterval;

	public float bulletDistanceInterval;

	public int bulletShootCount;

	public float bulletDuration;

	public int bulletDamage;

	public float bulletSpeed;

	private SpellInitialParameter sip_bullet = new SpellInitialParameter();

	private float roomWidth;

	private float roomHeight;

	private Vector3 roomCenter;

	[Header("拍地板")]
	public VariableFloat asPitch;

	public float attackCloseRange;

	[Header("和谐")]
	public SpriteRenderer sr_Dirt;

	public SpriteRenderer sr_DirtBack;

	public Sprite sprite_dirt_H;

	public Sprite sprite_Head_H;

	public Sprite sprite_Head_Attacking_H;

	public Sprite sprite_HeadBack_H;

	[Header("光照材质")]
	public Material mat_DR;

	public Material mat_NODR;

	private SpellSpawnParams ssp;

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

	public int CompareTo(object obj)
	{
		Vector3 vector = roomCenter;
		float num = Tool2D.IgnoreZAngleWithSign(Vector3.up, base.transform.position - vector);
		if (num < 0f)
		{
			num += 360f;
		}
		float num2 = Tool2D.IgnoreZAngleWithSign(Vector3.up, (obj as Boss6_Child).transform.position - vector);
		if (num2 < 0f)
		{
			num2 += 360f;
		}
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	public override void EveryInitialCallback()
	{
	}

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.Duration = bulletDuration;
		sSPModifier.Damage = bulletDamage;
		sSPModifier.Gravity = 0f;
		sSPModifier.ApplyToSSP(ref ssp);
		if (GameMgr.IsHarmony_Static)
		{
			sr_Dirt.sprite = sprite_dirt_H;
			sr_DirtBack.sprite = sprite_dirt_H;
			sprite_Head = sprite_Head_H;
			sprite_HeadBack = sprite_HeadBack_H;
			sprite_Head_Attacking = sprite_Head_Attacking_H;
			SR_Head.sprite = sprite_Head;
		}
		SR_Head.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		UnityEngine.Object.Destroy(SR_Head.material);
		SR_Head.material = mat_NODR;
		for (int i = 0; i < bodyCount; i++)
		{
			UnityEngine.Object.Destroy(SRs_Body[i].material);
			SRs_Body[i].material = mat_NODR;
		}
	}

	public void Initialize(Boss6 master)
	{
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y;
		roomCenter = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		this.master = master;
		state = MonsterState.Born;
		base.Anima.Play("UnderGround");
		base.Anima.Update(1f);
		SetBody();
		tsf_Model.gameObject.SetActive(value: true);
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanBeTarget = false;
		componentData.CanTouch = false;
		SetComponentData(componentData);
		state = MonsterState.Born;
		tsf_TargetRoot.gameObject.SetActive(value: false);
		SetBodyFadeHeight();
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
		float num = (float)(bodyIndex + 1) * bodyInterval;
		int num2 = 0;
		for (int i = 0; i < recordBeizerPointsDistance.Count && !(num < recordBeizerPointsDistance[i]); i++)
		{
			num -= recordBeizerPointsDistance[i];
			num2++;
		}
		if (bodyIndex == -1)
		{
			return recordBezierPoints[0];
		}
		if (num2 < bezierRecordPointsCount - 1)
		{
			return recordBezierPoints[num2] + (recordBezierPoints[num2 + 1] - recordBezierPoints[num2]).normalized * num;
		}
		return recordBezierPoints[bezierRecordPointsCount - 1];
	}

	private void SetBodyFadeHeight()
	{
		SR_Head.material.SetFloat(GameConstManaged.shaderGroundHeightIndex, base.transform.position.y);
		for (int i = 0; i < SRs_Body.Count; i++)
		{
			SRs_Body[i].material.SetFloat(GameConstManaged.shaderGroundHeightIndex, base.transform.position.y);
		}
	}

	public override void Update()
	{
		if (base.CC_Self.enabled && ToPointDistanceSqr(master.transform.position) < master.damageZone.CC.radius * master.damageZone.CC.radius * 0.7f)
		{
			DotsAnnouncedDeath();
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
		SetBody();
		SetBodyFadeHeight();
		switch (state)
		{
		case MonsterState.Born:
		{
			ref bool reference4 = ref varMgr.RegBool(0);
			if (changedState)
			{
				tsf_Model.gameObject.SetActive(value: true);
				headChasePlayer = false;
				headExpectedDir = Tool2D.GetDir(GeneralTool.HalfChanceNPOne() * 135f);
				nowFaceDir = headExpectedDir;
				base.Anima.Play("UnderGround");
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanBeTarget = false;
				componentData.CanTouch = false;
				SetComponentData(componentData);
				bornDelay.RandomResult();
			}
			if (stateExistTime > bornDelay.result && !reference4)
			{
				reference4 = true;
				base.Anima.Play("Show");
			}
			break;
		}
		case MonsterState.Idle:
			_ = ref varMgr.RegFloat(0);
			if (changedState)
			{
				headChasePlayer = true;
				base.Anima.Play("Idle");
				idleTime.RandomResult();
			}
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.WaitTarget;
			}
			break;
		case MonsterState.WaitTarget:
			if (changedState)
			{
				base.Anima.Play("Idle");
				waitTargetTime.RandomResult();
			}
			if (base.HaveTarget && ToTargetDistanceSqr() < attackDistance * attackDistance)
			{
				state = MonsterState.AttackBefore;
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				GetNearestTarget();
			}
			if (stateExistTime > waitTargetTime.result)
			{
				if (!base.HaveTarget)
				{
					state = MonsterState.AttackBefore;
					break;
				}
				headExpectedDir = Tool2D.GetDir();
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.AttackBefore:
			if (changedState)
			{
				base.Anima.Play("AttackBefore");
			}
			break;
		case MonsterState.Attack:
		{
			ref float reference = ref varMgr.RegFloat(0);
			ref float reference2 = ref varMgr.RegFloat(1);
			ref int reference3 = ref varMgr.RegInt(0);
			if (changedState)
			{
				headChasePlayer = false;
				base.Anima.Play("Attack");
				reference2 = 0f;
				SEMgr.Inst.boss6_ChildAttack.PlaySE().pitch = asPitch.RandomResult();
			}
			reference += Time.deltaTime;
			if (reference > bulletShootInterval)
			{
				reference -= bulletShootInterval;
				reference2 += bulletDistanceInterval;
				reference3++;
				ShootBullet(reference2);
			}
			if (reference3 > bulletShootCount)
			{
				state = MonsterState.AttackAfter;
			}
			break;
		}
		case MonsterState.AttackAfter:
			if (changedState)
			{
				headChasePlayer = true;
				base.Anima.Play("AttackAfter");
			}
			break;
		}
	}

	private void SetBody()
	{
		if (!useBezierBody)
		{
			return;
		}
		if (!base.HaveTarget)
		{
			GetNearestTarget();
		}
		if (headChasePlayer && base.HaveTarget && ToTargetDistanceSqr() < attackDistance * attackDistance)
		{
			headExpectedDir = ToTargetDir();
		}
		nowFaceDir = Tool2D.RotateTowardsAroundZAxis(nowFaceDir, headExpectedDir, headRotateSpeed * Time.deltaTime);
		nowFaceAngle = Tool2D.IgnoreZAngleWithSign(Vector3.up, nowFaceDir);
		SetBezierInfo(nowFaceAngle);
		Vector3 bodyPoint = GetBodyPoint(-1);
		Vector3 position = Tool2D.GetLayerPoint(bodyPoint) + new Vector3(0f, 0f, bodyMaskOffset) * 0.01f;
		SR_Head.transform.position = position;
		SR_Head.flipX = nowFaceAngle >= 0f;
		if (0f - bodyPoint.z > bodyInterval * 0.8f)
		{
			if (SR_Head.maskInteraction != 0)
			{
				SR_Head.maskInteraction = SpriteMaskInteraction.None;
				UnityEngine.Object.Destroy(SR_Head.material);
				SR_Head.material = mat_DR;
				SR_Head.material.color = myPpt.BaseColor;
			}
		}
		else if (SR_Head.maskInteraction != SpriteMaskInteraction.VisibleOutsideMask)
		{
			SR_Head.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
			UnityEngine.Object.Destroy(SR_Head.material);
			SR_Head.material = mat_NODR;
			SR_Head.material.color = myPpt.BaseColor;
		}
		Sprite sprite = ((!faceDown) ? sprite_HeadBack : ((state == MonsterState.Attack) ? sprite_Head_Attacking : sprite_Head));
		SR_Head.sprite = sprite;
		for (int i = 0; i < bodyCount; i++)
		{
			Vector3 bodyPoint2 = GetBodyPoint(i);
			Vector3 position2 = Tool2D.GetLayerPoint(bodyPoint2) + new Vector3(0f, 0f, bodyMaskOffset) * 0.01f;
			if (0f - bodyPoint2.z > bodyInterval * 0.8f)
			{
				if (SRs_Body[i].maskInteraction != 0)
				{
					SRs_Body[i].maskInteraction = SpriteMaskInteraction.None;
					UnityEngine.Object.Destroy(SRs_Body[i].material);
					SRs_Body[i].material = mat_DR;
					SRs_Body[i].material.color = myPpt.BaseColor;
				}
			}
			else if (SRs_Body[i].maskInteraction != SpriteMaskInteraction.VisibleOutsideMask)
			{
				SRs_Body[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
				UnityEngine.Object.Destroy(SRs_Body[i].material);
				SRs_Body[i].material = mat_NODR;
				SRs_Body[i].material.color = myPpt.BaseColor;
			}
			if (position2.y < base.transform.position.y + bodyMaskOffset && SRs_Body[i].maskInteraction != 0)
			{
				position2.z = Tool2D.GetLayerPoint(base.transform.position).z;
			}
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
			Vector3 to = position3 - position4;
			SRs_Body[i].transform.eulerAngles = Vector3.forward * Tool2D.IgnoreZAngleWithSign(Vector3.up, to);
			SRs_Body[i].flipX = nowFaceAngle >= 0f;
			SRs_Body[i].sprite = (faceDown ? sprite_Body : sprite_BodyBack);
		}
	}

	private void ShootBullet(float distance)
	{
		Vector3 vector = Tool2D.IgnoreZPoint(recordBezierPoints[0] + nowFaceDir * distance + Tool2D.GetDir() * bulletLandRadius.RandomResult());
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = bulletSpeed;
		sSPModifier.Direction = ToPointDir(vector);
		sSPModifier.CurrentFallSpeed = (0f - (recordBezierPoints[0].z - vector.z)) / (Tool2D.IgnoreZDistance(base.transform.position, vector) / bulletSpeed);
		sSPModifier.SpawnPosition = recordBezierPoints[0] + Vector3.forward * 0.3f + nowFaceDir * 0.6f;
		sSPModifier.ApplyToSSP(ref ssp);
		ShootSpell(ssp);
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		SEMgr.Inst.boss6_ChildDead.PlaySE().pitch = asPitch.RandomResult();
		List<Vector3> list = new List<Vector3>();
		list.Add(GetBodyPoint(-1));
		for (int i = 0; i < bodyCount; i++)
		{
			Vector3 bodyPoint = GetBodyPoint(i);
			if (bodyPoint.z < 0f)
			{
				list.Add(bodyPoint);
			}
		}
		FixedString32Bytes fs = default(FixedString32Bytes);
		FixedStringMethods.CopyFrom(ref fs, in myPpt.unitCfg.deadEF);
		FixedStringMethods.Append(ref fs, "G");
		using EntityQuery entityQuery = UnitDotsSyncSystem.entityMgr.CreateEntityQuery(typeof(GlobalParticleEmitParams));
		DynamicBuffer<GlobalParticleEmitParams> singletonBuffer = entityQuery.GetSingletonBuffer<GlobalParticleEmitParams>();
		for (int j = 0; j < list.Count; j++)
		{
			singletonBuffer.Add(new GlobalParticleEmitParams
			{
				Position = Tool2D.GetLayerPoint(list[j]),
				Size = 1f,
				Name = fs,
				Type = GlobalParticleType.EF
			});
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "BornShow":
		{
			SEMgr.Inst.elite11ChildBorn.PlaySE();
			base.CC_Self.enabled = true;
			SetDotsCCEnable(isOpen: true);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanBeTarget = true;
			SetComponentData(componentData);
			break;
		}
		case "BornCanTouch":
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = true;
			SetComponentData(componentData);
			break;
		}
		case "Show":
		{
			base.CC_Self.enabled = true;
			SetDotsCCEnable(isOpen: true);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanBeTarget = true;
			componentData.CanTouch = true;
			SetComponentData(componentData);
			break;
		}
		case "Hide":
		{
			base.CC_Self.enabled = false;
			SetDotsCCEnable(isOpen: false);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanBeTarget = false;
			componentData.CanTouch = false;
			SetComponentData(componentData);
			break;
		}
		case "AttackStart":
			state = MonsterState.Attack;
			break;
		case "AttackFinish":
			state = MonsterState.Idle;
			break;
		case "BornShowFinish":
		case "ShowFinish":
			state = MonsterState.Idle;
			break;
		}
	}
}
