using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class Boss56Elite58ThunderMach : MonoBehaviour
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

	public Transform RCannonRotateTransform;

	private Elite58State eliteState;

	private Elite58Skills currentSkill;

	private float eliteTimer;

	private bool isFaceRight = true;

	public float FaceDirectionChangeDuration;

	public float SkillInterval;

	public float MissileHiveRotateBackAngleSpeed;

	public Transform ModelTransform;

	public List<SpriteRenderer> SignalList;

	public Transform ShootPositionTransform;

	private EntityManager ettMgr;

	private CollisionFilter collisionFilter;

	public Animator Anima;

	[Header("电量相关")]
	public Transform BodyAuraTransform;

	public float AuraDebuffMoveSpeedRatio;

	public float AuraDebuffBulletSPeedDownLerp;

	public float DebuffApplyInterval;

	private float currentAuraRange;

	[Header("电磁力场炮")]
	public float EBCannonArmRotateSpeed;

	public float EBMaxFlyDuration;

	public float EBFullRangeDuration;

	public float EBExplosionTriggerRadius;

	public float EBFlySpeed;

	public float EBGroundAuraExistTime;

	public float EBExplosionDamage;

	public float EBExplosionDelayTime;

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

	public float FirstRingRadius;

	public float MineFinishShootStartDisappearTime;

	public float MineExplosionRange;

	public float MineExplosionDamage;

	public float MineTriggerDelayExplosionTime;

	public float MineManaDrainPercent;

	public float MineExistTime;

	public float SignalPopInterval;

	private float siganalPopTimer;

	public float MineAfterSkillBonusWaitTime;

	private List<(Elite58Marker, Vector3)> markers = new List<(Elite58Marker, Vector3)>();

	private float pillarRadius;

	private float pillarMarkTimer;

	private bool isEvenCountShoot;

	private float rotateSpeedUp;

	private float maxRotateAngle;

	private float stopRotateDuration;

	private bool isClockWiseRotate;

	private void OnEnable()
	{
		BodyAuraTransform.localScale = Vector3.zero;
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		collisionFilter = new CollisionFilter
		{
			BelongsTo = uint.MaxValue,
			CollidesWith = 16777216u,
			GroupIndex = 0
		};
		eliteState = Elite58State.Idle;
		currentSkill = Elite58Skills.ManaDrainMine;
		eliteTimer = 0f;
		markers.Clear();
		pillarMarkTimer = 0f;
		DOTween.To(() => currentAuraRange, delegate(float x)
		{
			currentAuraRange = x;
		}, 0f, 0f);
	}

	public void ReadyToEnd(float progressTime)
	{
		DOTween.To(() => currentAuraRange, delegate(float x)
		{
			currentAuraRange = x;
		}, 0f, progressTime);
	}

	public void Update()
	{
		UpdateState();
		UpdateMinePillarsMarkState();
	}

	public void InitializeData(float pillarEffectRadius, float rotateSpeedUp, float MaxAngleSpeed, float stopRotateDuration, float auraMaxRange, float auraExpandDuration)
	{
		pillarRadius = pillarEffectRadius;
		this.rotateSpeedUp = rotateSpeedUp;
		maxRotateAngle = MaxAngleSpeed;
		this.stopRotateDuration = stopRotateDuration;
		currentAuraRange = 0f;
		DOTween.To(() => currentAuraRange, delegate(float x)
		{
			currentAuraRange = x;
		}, auraMaxRange, auraExpandDuration);
	}

	public void StartSpawnPillar(List<Vector3> targetPoints, bool isEvenCountShoot)
	{
		if (markers.Count <= 0)
		{
			for (int i = 0; i < targetPoints.Count; i++)
			{
				Elite58Marker component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite58_LandPositionMark", base.transform.position.IgnoreZ()).GetComponent<Elite58Marker>();
				component.StartMarker();
				markers.Add((component, targetPoints[i]));
			}
			this.isEvenCountShoot = isEvenCountShoot;
			siganalPopTimer = SignalPopInterval;
			SignalList[0].enabled = true;
			pillarMarkTimer = 0f;
		}
	}

	public void ShootThunderBall(int shootCount, float StartAngle, bool isClockWise)
	{
		isClockWiseRotate = isClockWise;
		float num = 360f / (float)shootCount;
		for (int i = 0; i < shootCount; i++)
		{
			Boss56Elite58SpecialBall component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite58_Boss56SpecialElectricBall", ShootPositionTransform.position.IgnoreZ(), quaternion.identity).GetComponent<Boss56Elite58SpecialBall>();
			Vector3 oldDir = (isFaceRight ? RCannonRotateTransform.right : (-RCannonRotateTransform.right));
			oldDir = Tool2D.GetDir(oldDir, StartAngle + num * (float)i);
			int num2 = ((!isClockWiseRotate) ? 1 : (-1));
			component.InitialData(oldDir, EBMaxFlyDuration, EBFlySpeed, EBExplosionDamage, EBExplosionTriggerRadius, EBGroundAuraExistTime, EBExplosionDelayTime, AuraDebuffMoveSpeedRatio, AuraDebuffBulletSPeedDownLerp, rotateSpeedUp * (float)num2, maxRotateAngle * (float)num2, stopRotateDuration);
		}
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite58_Shoot", ShootPositionTransform.position.IgnoreZ() + new Vector3(0f, 0f, -0.01f), quaternion.identity).transform.right = ShootPositionTransform.right;
	}

	public void ShootThunderBall(int shootCount, float sectorAngle)
	{
		float num = ((shootCount == 1) ? 0f : (sectorAngle / (float)(shootCount - 1)));
		Vector3 normalized = (PlayerMgr.Inst.PlayerPoint - ShootPositionTransform.position).IgnoreZ().normalized;
		for (int i = 0; i < shootCount; i++)
		{
			Boss56Elite58SpecialBall component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite58_Boss56SpecialElectricBall", ShootPositionTransform.position.IgnoreZ(), quaternion.identity).GetComponent<Boss56Elite58SpecialBall>();
			Vector3 dir = Tool2D.GetDir(normalized, (0f - sectorAngle) / 2f + num * (float)i);
			int num2 = ((!isClockWiseRotate) ? 1 : (-1));
			component.InitialData(dir, EBMaxFlyDuration, EBFlySpeed, EBExplosionDamage, EBExplosionTriggerRadius, EBGroundAuraExistTime, EBExplosionDelayTime, AuraDebuffMoveSpeedRatio, AuraDebuffBulletSPeedDownLerp, rotateSpeedUp * (float)num2, maxRotateAngle * (float)num2, stopRotateDuration);
		}
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite58_Shoot", ShootPositionTransform.position.IgnoreZ() + new Vector3(0f, 0f, -0.01f), quaternion.identity).transform.right = ShootPositionTransform.right;
		SEMgr.Inst.elite53Shoot.PlaySE();
	}

	private void UpdateMinePillarsMarkState()
	{
		if (markers.Count <= 0)
		{
			return;
		}
		pillarMarkTimer += Time.deltaTime;
		bool flag = false;
		foreach (var (elite58Marker, vector) in markers)
		{
			if (pillarMarkTimer <= MinePillarLandPosLockTime)
			{
				float b = Vector3.Distance(vector, elite58Marker.transform.position);
				Vector3 centerPos = elite58Marker.transform.position + Tool2D.IgnoreZV2ToV1Normal(vector, elite58Marker.transform.position) * Mathf.Min(MineLockMoveSpeed * Time.deltaTime, b);
				elite58Marker.UpdateTransform(centerPos);
				siganalPopTimer += Time.deltaTime;
				if (siganalPopTimer >= SignalPopInterval)
				{
					siganalPopTimer -= SignalPopInterval;
					Anima.SetTrigger(Pop);
				}
			}
			else
			{
				flag = true;
				float num = MinePillarLandSpeed * MinePillarLandDelay;
				Elite58MinePillar component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite58_MinePillar", elite58Marker.transform.position.IgnoreZ() + new Vector3(0f, 0f, 0f - num), quaternion.identity).GetComponent<Elite58MinePillar>();
				component.transform.right = Vector3.zero;
				component.InitialPillarData(MinePillarLandSpeed, PillarLandDelayOpenTime, SingleRingMineCount, MineShootRingCount, BonusMineCountPerRing, MinePillarShootInterval, RingBonusRadiusPerShoot, MineFinishShootStartDisappearTime, MineExplosionDamage, MineExplosionRange, MineExistTime, MineTriggerDelayExplosionTime, MineManaDrainPercent, isEvenCountShoot ? 60 : 0, 0f, FirstRingRadius);
				elite58Marker.EndMarker();
			}
		}
		if (flag)
		{
			SignalList[0].enabled = false;
			EnterState(Elite58State.Move);
			markers.Clear();
		}
	}

	private void UpdateState()
	{
		BodyAuraTransform.localScale = Vector3.one * currentAuraRange;
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
			FaceToPlayer();
			UpdateFaceDirection();
			if (eliteTimer >= SkillInterval)
			{
				EnterState(Elite58State.CastSpell);
			}
			break;
		case Elite58State.CastSpell:
		{
			FaceToPlayer();
			UpdateFaceDirection();
			eliteTimer += Time.deltaTime;
			Elite58Skills elite58Skills = currentSkill;
			if (elite58Skills != 0 && elite58Skills != Elite58Skills.ManaDrainMine)
			{
				throw new ArgumentOutOfRangeException();
			}
			break;
		}
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
				UnitProperty_Dots componentData2 = UnitDotsSyncSystem.entityMgr.GetComponentData<UnitProperty_Dots>(entity);
				if (componentData2.unitCfg.IsSameCamp(UnitType.Player))
				{
					componentData2.SetMucus(DebuffApplyInterval + 0.1f, AuraDebuffMoveSpeedRatio, 1f, changeColor: false);
					UnitDotsSyncSystem.entityMgr.SetComponentData(entity, componentData2);
				}
			}
		}
	}

	private void EnterState(Elite58State state)
	{
		eliteState = state;
		eliteTimer = 0f;
		switch (state)
		{
		case Elite58State.CastSpell:
			switch (currentSkill)
			{
			case Elite58Skills.GravityShockBomb:
				EBArmStopMotionTimer = 0f;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case Elite58Skills.ManaDrainMine:
				break;
			}
			break;
		default:
			throw new ArgumentOutOfRangeException("state", state, null);
		case Elite58State.Idle:
		case Elite58State.Move:
			break;
		}
	}

	private void FaceToPlayer()
	{
		isFaceRight = PlayerMgr.Inst.PlayerPoint.x >= base.transform.position.x;
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
		if (markers.Count <= 0)
		{
			return;
		}
		foreach (var marker in markers)
		{
			marker.Item1.EndMarker();
		}
	}
}
