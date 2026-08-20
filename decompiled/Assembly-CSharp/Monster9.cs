using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Monster9 : UnitBase
{
	private enum UnitState
	{
		BornIdle,
		RandomFly,
		Avoid,
		AttackBefore,
		Attack
	}

	public VariableFloat randomFlyRadius;

	[Header("Avoid")]
	public float avoidCheckInterval;

	public float avoidCheckRadius;

	public float avoidDistance;

	public float avoidMiddlePointDistance;

	public float avoidTime;

	[Header("Attack")]
	public LayerMask laserCheckLayer;

	public LayerMask laserAttackLayer;

	public Monster9Laser laser;

	[Range(0f, 1f)]
	public float attackChance;

	public float laserRotateSpeed;

	public float laserHeight;

	public float laserDamageInterval;

	public int laserDamage;

	public float laserBeforeTime;

	public float laserDuration;

	[Header("Force Attack")]
	public float forceAttackCheckInterval;

	public float forceAttackDistance;

	[Range(0f, 1f)]
	public float beHitAttackChance;

	public AIPattern pattern;

	[Header("Pattern2")]
	public float attackBeforeTime;

	public float shockRadius;

	public float shockSpeed;

	[Header("Audio")]
	public AudioSource as_LaserLoop;

	public AudioSource as_Aim;

	private UnitState state;

	private Vector3 avoidBeforePoint;

	private Vector3 avoidToPoint;

	private Vector3 avoidMiddlePoint;

	private float avoidCheckIntervalTimer;

	private float avoidTimer;

	private Vector3 laserCurrentDir;

	private float laserBeforeTimer;

	private float laserDurationTimer;

	private float laserDamageIntervalTimer;

	private CollisionFilter avoidFilter = new CollisionFilter
	{
		BelongsTo = 2048u,
		CollidesWith = 16777216u,
		GroupIndex = 0
	};

	private List<UnitDotsSyncSystem.DistanceHitResult> resultContainer = new List<UnitDotsSyncSystem.DistanceHitResult>();

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
		if ((bool)as_LaserLoop)
		{
			as_LaserLoop.volume = DataMgr.settingData.GetFinalSound();
		}
		if ((bool)as_Aim)
		{
			as_Aim.volume = DataMgr.settingData.GetFinalSound();
		}
	}

	public override void EveryInitialCallback()
	{
		state = UnitState.BornIdle;
		avoidCheckIntervalTimer = 0f;
		avoidTimer = 0f;
		laserBeforeTimer = 0f;
		laserDurationTimer = 0f;
		laserDamageIntervalTimer = 0f;
		laser.StopImmediately();
		SetNavMeshArea(8);
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		UnitDotsSyncSystem.RayCastHitResult result2;
		switch (state)
		{
		case UnitState.BornIdle:
			SetMove(Vector3.zero);
			if (pattern == AIPattern.Pattern2)
			{
				CheckSpell();
			}
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = UnitState.RandomFly;
				GetNavInfo(LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(base.transform.position + Tool2D.GetDir() * randomFlyRadius.RandomResult()));
			}
			break;
		case UnitState.RandomFly:
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(base.transform.position + Tool2D.GetDir() * randomFlyRadius.RandomResult()));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			if (pattern == AIPattern.Pattern2)
			{
				CheckSpell();
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (!(checkTargetIntervalTimer >= forceAttackCheckInterval))
			{
				break;
			}
			checkTargetIntervalTimer = 0f;
			GetNearestTarget(checkWall: true);
			if (!base.HaveTarget || !(ToTargetDistanceSqr() < forceAttackDistance * forceAttackDistance))
			{
				break;
			}
			state = UnitState.AttackBefore;
			base.Anima.SetTrigger("Attack");
			as_Aim.Play();
			laserCurrentDir = ToTargetDir();
			if (pattern == AIPattern.Pattern2)
			{
				Vector3 vector3 = GetComponentData<LocalTransform>(targetEntity).Position;
				if (targetEntity == PlayerMgr.Inst.PlayerEtt)
				{
					laserCurrentDir = ToPointDir(vector3 + PlayerMgr.Inst.PlayerCtrller.CurrentMotion * attackBeforeTime * 2f / 3f);
				}
				else
				{
					laserCurrentDir = ToPointDir(vector3 + (Vector3)GetComponentData<UnitBase_Dots>(targetEntity).currentMotion * attackBeforeTime * 2f / 3f);
				}
			}
			else
			{
				laserCurrentDir = ToTargetDir();
			}
			break;
		case UnitState.Avoid:
		{
			avoidTimer += Time.deltaTime;
			base.transform.position = GeneralTool.QuadraticBezierCurve(avoidBeforePoint, avoidMiddlePoint, avoidToPoint, avoidTimer / avoidTime);
			LocalTransform componentData = GetComponentData<LocalTransform>();
			componentData.Position = base.transform.position;
			SetComponentData(componentData);
			if (!(avoidTimer >= avoidTime))
			{
				break;
			}
			avoidTimer = 0f;
			if (UnityEngine.Random.value <= attackChance)
			{
				GetNearestTarget(checkWall: true);
				if (base.HaveTarget)
				{
					state = UnitState.AttackBefore;
					base.Anima.SetTrigger("Attack");
					if (pattern == AIPattern.Pattern2)
					{
						LocalTransform componentData2 = GetComponentData<LocalTransform>(targetEntity);
						if (targetEntity == PlayerMgr.Inst.PlayerEtt)
						{
							PhysicsVelocity componentData3 = GetComponentData<PhysicsVelocity>(targetEntity);
							laserCurrentDir = ToPointDir(componentData2.Position + componentData3.Linear * attackBeforeTime * 2f / 3f);
						}
						else
						{
							UnitBase_Dots componentData4 = GetComponentData<UnitBase_Dots>(targetEntity);
							laserCurrentDir = ToPointDir(componentData2.Position + componentData4.currentMotion * attackBeforeTime * 2f / 3f);
						}
					}
					else
					{
						laserCurrentDir = ToTargetDir();
					}
					break;
				}
			}
			state = UnitState.RandomFly;
			GetNavInfo(LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(base.transform.position + Tool2D.GetDir() * randomFlyRadius.RandomResult()));
			break;
		}
		case UnitState.AttackBefore:
		{
			SetMove(Vector3.zero);
			if (!base.HaveTarget)
			{
				GetNearestTarget(checkWall: true);
			}
			if (base.HaveTarget)
			{
				laserCurrentDir = Tool2D.DirMoveTowards(laserCurrentDir, ToTargetDir(), laserRotateSpeed * Time.deltaTime);
			}
			Vector3 vector2 = base.transform.position + new Vector3(0f, 0f, 0f - laserHeight);
			Vector3 point2 = (UnitDotsSyncSystem.Raycast(vector2, laserCurrentDir, 100f, GameConst.Filter_Laser, out result2) ? result2.point : ((!UnitDotsSyncSystem.Raycast(vector2, laserCurrentDir, 100f, GameConst.Filter_Wall, out result2)) ? (vector2 + laserCurrentDir * 100f) : result2.point));
			laser.SetWarning(vector2, point2);
			laserBeforeTimer += Time.deltaTime;
			if (laserBeforeTimer >= laserBeforeTime)
			{
				laserBeforeTimer = 0f;
				state = UnitState.Attack;
				laserDamageIntervalTimer = 999999f;
				if (pattern == AIPattern.Pattern2)
				{
					CamController.Inst.SetShock(shockRadius, shockSpeed, laserDamageInterval);
				}
				as_Aim.Stop();
				as_LaserLoop.Play();
			}
			break;
		}
		case UnitState.Attack:
		{
			SetMove(Vector3.zero);
			if (!base.HaveTarget)
			{
				GetNearestTarget(checkWall: true);
			}
			if (base.HaveTarget)
			{
				laserCurrentDir = Tool2D.DirMoveTowards(laserCurrentDir, ToTargetDir(), laserRotateSpeed * Time.deltaTime);
			}
			Vector3 vector = base.transform.position + new Vector3(0f, 0f, 0f - laserHeight);
			laserDamageIntervalTimer += Time.deltaTime;
			Vector3 point;
			if (!UnitDotsSyncSystem.Raycast(vector, laserCurrentDir, 100f, GameConst.Filter_Laser, out var result))
			{
				point = ((!UnitDotsSyncSystem.Raycast(vector, laserCurrentDir, 100f, GameConst.Filter_Wall, out result2)) ? (vector + laserCurrentDir * 100f) : result2.point);
			}
			else
			{
				point = result.point;
				if (laserDamageIntervalTimer >= laserDamageInterval)
				{
					laserDamageIntervalTimer = 0f;
					if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(result.entity))
					{
						TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
						info.damage = laserDamage;
						UnitDotsSyncSystem.AddTakeDamageRequest(result.entity, info);
					}
					if (pattern == AIPattern.Pattern2)
					{
						CamController.Inst.SetShock(shockRadius, shockSpeed, laserDamageInterval);
					}
				}
			}
			laser.SetLaser(vector, point);
			laserDurationTimer += Time.deltaTime;
			if (laserDurationTimer >= laserDuration)
			{
				laserDurationTimer = 0f;
				state = UnitState.RandomFly;
				base.Anima.SetTrigger("AttackFinish");
				GetNavInfo(LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(base.transform.position + Tool2D.GetDir() * randomFlyRadius.RandomResult()));
				laser.Stop();
				as_LaserLoop.Stop();
			}
			break;
		}
		default:
			Debug.LogError(state);
			break;
		}
	}

	private void CheckSpell()
	{
		avoidCheckIntervalTimer += Time.deltaTime;
		if (!(avoidCheckIntervalTimer >= avoidCheckInterval))
		{
			return;
		}
		avoidCheckIntervalTimer = 0f;
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, avoidCheckRadius, avoidFilter, resultContainer);
		if (resultContainer.Count <= 0)
		{
			return;
		}
		Entity entity = default(Entity);
		float num = 9999999f;
		Vector3 vector = Vector3.zero;
		for (int i = 0; i < resultContainer.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = resultContainer[i];
			if (!(distanceHitResult.distance > num) && UnitDotsSyncSystem.TryGetComponent<SpellConfigComponentData>(distanceHitResult.entity, out var result) && !myPpt.unitCfg.IsSameCamp(result.ShooterType))
			{
				num = distanceHitResult.distance;
				entity = distanceHitResult.entity;
				vector = distanceHitResult.point;
			}
		}
		if (entity != Entity.Null)
		{
			state = UnitState.Avoid;
			avoidBeforePoint = base.transform.position;
			Vector3 vector2 = UnitDotsSyncSystem.GetComponentData<SpellMovementComponentData>(entity).Direction;
			Vector3 vector3 = base.transform.position + Tool2D.GetDir(vector2, 90f) * avoidDistance;
			Vector3 vector4 = base.transform.position + Tool2D.GetDir(vector2, -90f) * avoidDistance;
			if ((vector - vector3).sqrMagnitude < (vector - vector4).sqrMagnitude)
			{
				avoidToPoint = Tool2D.GetNavMeshPointIngoreZ(vector4);
			}
			else
			{
				avoidToPoint = Tool2D.GetNavMeshPointIngoreZ(vector3);
			}
			avoidMiddlePoint = (base.transform.position + avoidToPoint) / 2f + vector2 * avoidMiddlePointDistance;
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (state != UnitState.RandomFly)
		{
			return;
		}
		GetNearestTarget(checkWall: true);
		if (!base.HaveTarget || !(UnityEngine.Random.value <= beHitAttackChance))
		{
			return;
		}
		state = UnitState.AttackBefore;
		base.Anima.SetTrigger("Attack");
		if (pattern == AIPattern.Pattern2)
		{
			Vector3 vector = GetComponentData<LocalTransform>(targetEntity).Position;
			if (targetEntity == PlayerMgr.Inst.PlayerEtt)
			{
				laserCurrentDir = ToPointDir(vector + PlayerMgr.Inst.PlayerCtrller.CurrentMotion * attackBeforeTime * 2f / 3f);
			}
			else
			{
				laserCurrentDir = ToPointDir(vector + (Vector3)GetComponentData<UnitBase_Dots>(targetEntity).currentMotion * attackBeforeTime * 2f / 3f);
			}
		}
		else
		{
			laserCurrentDir = ToTargetDir();
		}
	}
}
