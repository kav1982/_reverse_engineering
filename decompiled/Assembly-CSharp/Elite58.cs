using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using UnityEngine.Serialization;

public class Elite58 : UnitBase
{
	public enum Elite58State
	{
		Idle,
		Move,
		CastSpell
	}

	public enum Elite58Skills
	{
		GravityShockBomb,
		ManaDrainMine
	}

	private static readonly int Progress = Shader.PropertyToID("_Progress");

	private static readonly int Pop = Animator.StringToHash("Pop");

	private UIEndlessEliteHpBar hpBar;

	public Transform RCannonRotateTransform;

	private Elite58State eliteState;

	private Elite58Skills currentSkill;

	private float eliteTimer;

	public float CloseToTargetStopMotionDistance;

	private bool isFaceRight = true;

	public float FaceDirectionChangeDuration;

	public float SkillInterval;

	public float MissileHiveRotateBackAngleSpeed;

	public Transform ModelTransform;

	public List<SpriteRenderer> SignalList;

	public Transform ShootPositionTransform;

	private EntityManager ettMgr;

	private CollisionFilter collisionFilter;

	[Header("电量相关")]
	public Transform BodyAuraTransform;

	public List<SpriteRenderer> BatteryPercentSpriteList;

	public float BatteryChargeSpeed;

	private float currentBattery;

	public float BaseGraviryAuraRange;

	public float MaxBatteryBonusAuraRange;

	public float AuraDebuffMoveSpeedRatio;

	public float AuraDebuffBulletSPeedDownLerp;

	public float DebuffApplyInterval;

	public float MaxBatterySelfSpeedRatio;

	private float currentAuraRange;

	private float currentSpeedRatio;

	public int ShootPillarRequireShootCounter;

	private int CurrentShootCount;

	public int EBShootWave;

	public int MPShootWave;

	[Header("电磁力场炮")]
	public float EBStartShootDistance;

	public float EBCannonArmRotateSpeed;

	public float EBBatteryCost;

	public float EBChargeEnergyCostPerSecond;

	private float EBCurrentCharge;

	public float EBMaxFlyDuration;

	public float EBFullRangeDuration;

	public float EBExplosionTriggerRadius;

	public float EBFlySpeed;

	public float EBGroundAuraExistTime;

	public float EBExplosionDamage;

	public float EBExplosionDelayTime;

	public float EBAfterSkillBonusWaitTime;

	public float EBAfterSkillStopMotionTime;

	private float EBArmStopMotionTimer;

	[Header("吸魔地雷")]
	public float MinePillarSkillMinCastInterval;

	public float MinePillarLandPosLockTime;

	public float MinePillarLandDelay;

	public float MinePillarShootInterval;

	public float MineLockMoveSpeed;

	private Vector3 MineLockPosition;

	public float MinePillarLandSpeed;

	public float PillarLandDelayOpenTime;

	public int MineShootRingCount;

	public int SingleRingMineCount;

	public int BonusMineCountPerRing;

	public float RingBonusRadiusPerShoot;

	public float MineFinishShootStartDisappearTime;

	public float MineExplosionRange;

	public float MineExplosionDamage;

	public float MineTriggerDelayExplosionTime;

	public float MineManaDrainPercent;

	public float MineExistTime;

	private Elite58Marker marker;

	public float SignalPopInterval;

	private float siganalPopTimer;

	[FormerlySerializedAs("AfterSkillBonusWaitTime")]
	public float MineAfterSkillBonusWaitTime;

	private void OnEnable()
	{
		BodyAuraTransform.localScale = Vector3.zero;
	}

	public override void SingleInitialCallback()
	{
		hpBar = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIEndlessEliteHpBar"), myPpt.tsf_Layer.position + new Vector3(0f, myPpt.unitCfg.relicShowHPUIHight - 0.2f, 0f) * myPpt.tsf_Layer.lossyScale.y, Quaternion.identity, myPpt.tsf_Layer).GetComponent<UIEndlessEliteHpBar>();
		hpBar.Initialize(this);
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		collisionFilter = new CollisionFilter
		{
			BelongsTo = uint.MaxValue,
			CollidesWith = 16777216u,
			GroupIndex = 0
		};
	}

	public override void EveryInitialCallback()
	{
		base.EveryInitialCallback();
		eliteState = Elite58State.Idle;
		currentSkill = Elite58Skills.ManaDrainMine;
		eliteTimer = 0f;
		base.Rigid.isKinematic = true;
		currentBattery = 0f;
		SyncDotsRigidKindmatic();
	}

	public override void Update()
	{
		base.Update();
		UpdateState();
	}

	private void UpdateState()
	{
		if (currentBattery < 100f)
		{
			currentBattery += BatteryChargeSpeed * Time.deltaTime;
			currentBattery = Mathf.Clamp(currentBattery, 0f, 100f);
			float num = currentBattery / 100f;
			int num2 = Mathf.FloorToInt(num / (1f / (float)BatteryPercentSpriteList.Count));
			for (int i = 0; i < BatteryPercentSpriteList.Count; i++)
			{
				BatteryPercentSpriteList[i].enabled = i <= num2;
			}
			currentAuraRange = BaseGraviryAuraRange + num * MaxBatteryBonusAuraRange;
			currentSpeedRatio = 1f - num * MaxBatterySelfSpeedRatio;
			BodyAuraTransform.localScale = Vector3.one * currentAuraRange;
		}
		ApplyGravityDebuff();
		switch (eliteState)
		{
		case Elite58State.Idle:
			EnterState(Elite58State.Move);
			break;
		case Elite58State.Move:
			if (EBArmStopMotionTimer >= 0f)
			{
				EBArmStopMotionTimer -= Time.deltaTime;
				break;
			}
			RCannonRotateTransform.right = Tool2D.RotateTowardsAroundZAxis(RCannonRotateTransform.right, Tool2D.GetDir(-90f), MissileHiveRotateBackAngleSpeed * Time.deltaTime);
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			FaceToPlayer();
			UpdateFaceDirection();
			if (base.HaveTarget)
			{
				MoveToTarget();
			}
			else
			{
				SetMove(Vector3.zero, isFlip: false);
			}
			if (eliteTimer >= SkillInterval)
			{
				EnterState(Elite58State.CastSpell);
			}
			break;
		case Elite58State.CastSpell:
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			SetMove(Vector3.zero, isFlip: false);
			FaceToPlayer();
			UpdateFaceDirection();
			eliteTimer += Time.deltaTime;
			switch (currentSkill)
			{
			case Elite58Skills.GravityShockBomb:
				if (EBCurrentCharge < EBBatteryCost)
				{
					float num4 = Mathf.Min(EBChargeEnergyCostPerSecond * Time.deltaTime, EBBatteryCost - EBCurrentCharge);
					EBCurrentCharge += num4;
					currentBattery -= num4;
					Vector3 to = (isFaceRight ? (base.TargetPoint - RCannonRotateTransform.position).IgnoreZ().normalized : (RCannonRotateTransform.position - base.TargetPoint).IgnoreZ().normalized);
					RCannonRotateTransform.right = Tool2D.RotateTowardsAroundZAxis(RCannonRotateTransform.right, to, MissileHiveRotateBackAngleSpeed * Time.deltaTime);
				}
				else
				{
					Elite58ElectricBall component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite58_ElectricBall", ShootPositionTransform.position.IgnoreZ(), quaternion.identity).GetComponent<Elite58ElectricBall>();
					Vector3 vector = (isFaceRight ? RCannonRotateTransform.right : (-RCannonRotateTransform.right));
					component2.InitialData(vector, EBMaxFlyDuration, EBFlySpeed, EBExplosionDamage, EBExplosionTriggerRadius, EBGroundAuraExistTime, EBExplosionDelayTime, AuraDebuffMoveSpeedRatio, AuraDebuffBulletSPeedDownLerp);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite58_Shoot", ShootPositionTransform.position.IgnoreZ() + new Vector3(0f, 0f, -0.01f), quaternion.identity).transform.right = vector;
					SEMgr.Inst.elite58Shoot.PlaySE();
					EnterState(Elite58State.Move);
					eliteTimer -= EBAfterSkillBonusWaitTime;
					EBArmStopMotionTimer = EBAfterSkillStopMotionTime;
				}
				break;
			case Elite58Skills.ManaDrainMine:
				if (!(marker != null))
				{
					break;
				}
				if (eliteTimer <= MinePillarLandPosLockTime)
				{
					float b = Vector3.Distance(PlayerMgr.Inst.PlayerPoint, marker.transform.position);
					Vector3 centerPos = marker.transform.position + Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, marker.transform.position) * Mathf.Min(MineLockMoveSpeed * Time.deltaTime, b);
					marker.UpdateTransform(centerPos);
					siganalPopTimer += Time.deltaTime;
					if (siganalPopTimer >= SignalPopInterval)
					{
						siganalPopTimer -= SignalPopInterval;
						base.Anima.SetTrigger(Pop);
					}
				}
				else
				{
					float num3 = MinePillarLandSpeed * MinePillarLandDelay;
					Elite58MinePillar component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite58_MinePillar", marker.transform.position.IgnoreZ() + new Vector3(0f, 0f, 0f - num3), quaternion.identity).GetComponent<Elite58MinePillar>();
					component.transform.right = Vector3.zero;
					component.InitialPillarData(MinePillarLandSpeed, PillarLandDelayOpenTime, SingleRingMineCount, MineShootRingCount, BonusMineCountPerRing, MinePillarShootInterval, RingBonusRadiusPerShoot, MineFinishShootStartDisappearTime, MineExplosionDamage, MineExplosionRange, MineExistTime, MineTriggerDelayExplosionTime, MineManaDrainPercent);
					marker.EndMarker();
					marker = null;
					SignalList[0].enabled = false;
					EnterState(Elite58State.Move);
					eliteTimer -= MineAfterSkillBonusWaitTime;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			break;
		}
	}

	public void ApplyGravityDebuff()
	{
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(PhysicsWorldSingleton));
		entityQuery.GetSingleton<PhysicsWorldSingleton>().OverlapSphere(base.transform.position, currentAuraRange, ref outHits, collisionFilter);
		foreach (DistanceHit item in outHits)
		{
			if (ettMgr.HasComponent<SpellConfigComponentData>(item.Entity) && DTool.IsSameCamp(ettMgr.GetComponentData<SpellConfigComponentData>(item.Entity).ShooterType, UnitType.Player))
			{
				SpellMovementComponentData componentData = ettMgr.GetComponentData<SpellMovementComponentData>(item.Entity);
				componentData.Speed = Mathf.Lerp(componentData.Speed, 0f, AuraDebuffBulletSPeedDownLerp * Time.deltaTime);
				componentData.CurrentFallSpeed = Mathf.Lerp(componentData.CurrentFallSpeed, 0f, AuraDebuffBulletSPeedDownLerp * Time.deltaTime);
				componentData.Gravity = Mathf.Lerp(componentData.Gravity, 0f, AuraDebuffBulletSPeedDownLerp * Time.deltaTime);
				ettMgr.SetComponentData(item.Entity, componentData);
			}
		}
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, currentAuraRange, GameConst.Filter_MonsterAoe, list);
		for (int i = 0; i < list.Count; i++)
		{
			Entity entity = list[i].entity;
			if (UnitDotsSyncSystem.entityMgr.HasComponent<UnitProperty_Dots>(entity))
			{
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>(entity);
				if (componentData2.unitCfg.IsSameCamp(UnitType.Player))
				{
					componentData2.SetMucus(DebuffApplyInterval + 0.1f, AuraDebuffMoveSpeedRatio, 1f, changeColor: false);
					SetComponentData(componentData2, entity);
				}
			}
		}
	}

	private void MoveToTarget()
	{
		if (!base.HaveTarget)
		{
			GetNearestTargetPlayerFirst();
		}
		if (base.HaveTarget)
		{
			if (Tool2D.IgnoreZDistance(base.transform.position, base.TargetPoint) <= CloseToTargetStopMotionDistance)
			{
				SetMove(Vector3.zero);
				eliteTimer += Time.deltaTime;
				return;
			}
			GetNavInfo(base.TargetPoint);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * currentSpeedRatio);
			CheckNavInfo();
			eliteTimer += Time.deltaTime;
		}
	}

	private void EnterState(Elite58State state)
	{
		eliteState = state;
		eliteTimer = 0f;
		switch (state)
		{
		case Elite58State.CastSpell:
			CastSpell();
			switch (currentSkill)
			{
			case Elite58Skills.GravityShockBomb:
				EBCurrentCharge = 0f;
				EBArmStopMotionTimer = 0f;
				break;
			case Elite58Skills.ManaDrainMine:
				marker = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite58_LandPositionMark", base.transform.position.IgnoreZ()).GetComponent<Elite58Marker>();
				marker.StartMarker();
				siganalPopTimer = SignalPopInterval;
				SignalList[0].enabled = true;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			break;
		default:
			throw new ArgumentOutOfRangeException("state", state, null);
		case Elite58State.Idle:
		case Elite58State.Move:
			break;
		}
	}

	private void CastSpell()
	{
		int num = ((currentSkill == Elite58Skills.ManaDrainMine) ? MPShootWave : EBShootWave);
		if (CurrentShootCount >= num)
		{
			CurrentShootCount = 0;
			currentSkill = ((currentSkill != Elite58Skills.ManaDrainMine) ? Elite58Skills.ManaDrainMine : Elite58Skills.GravityShockBomb);
		}
		else
		{
			CurrentShootCount++;
		}
	}

	private void FaceToPlayer()
	{
		if (base.HaveTarget)
		{
			isFaceRight = base.TargetPoint.x >= base.transform.position.x;
		}
	}

	private void UpdateFaceDirection(bool instantLerp = false)
	{
		float num = (isFaceRight ? 1f : (-1f));
		if (instantLerp)
		{
			num = Mathf.Lerp(base.transform.localScale.x, num, 10f * Time.deltaTime);
			ModelTransform.localScale = new Vector3(num, ModelTransform.localScale.y, ModelTransform.localScale.z);
		}
		else
		{
			ModelTransform.DOScaleX(num, FaceDirectionChangeDuration);
		}
	}

	private void OnDisable()
	{
		if (marker != null)
		{
			marker.EndMarker();
		}
	}
}
