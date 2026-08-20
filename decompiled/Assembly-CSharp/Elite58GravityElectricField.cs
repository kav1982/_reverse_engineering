using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Elite58GravityElectricField : MonoBehaviour
{
	private EntityManager ettMgr;

	private CollisionFilter collisionFilter;

	public Transform AuraTransform;

	public float AuraEndDuration;

	private float moveSpeedDownRatio;

	private float bulletSpeedDownLerpRatio;

	private float remainDuration;

	private float range;

	private bool isStart;

	private bool isEnd;

	private void OnEnable()
	{
		isStart = false;
		isEnd = false;
	}

	private void Start()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		collisionFilter = new CollisionFilter
		{
			BelongsTo = uint.MaxValue,
			CollidesWith = 16777216u,
			GroupIndex = 0
		};
	}

	private void Update()
	{
		if (isStart)
		{
			remainDuration -= Time.deltaTime;
			ApplyGravityDebuff();
			if (remainDuration <= 0f && !isEnd)
			{
				StartCoroutine(AuraEnd(AuraEndDuration));
			}
		}
	}

	public void ForceEnd()
	{
		StartCoroutine(AuraEnd(0f));
	}

	private IEnumerator AuraEnd(float duration)
	{
		isEnd = true;
		AuraTransform.DOScale(0f, duration);
		yield return new WaitForSeconds(duration);
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}

	public void InitData(float range, float duration, float moveSpeedRatio, float bulletSpeedLerpRatio)
	{
		this.range = range;
		moveSpeedDownRatio = moveSpeedRatio;
		bulletSpeedDownLerpRatio = bulletSpeedLerpRatio;
		remainDuration = duration;
		isStart = true;
		AuraTransform.localScale = Vector3.one * range;
	}

	private void ApplyGravityDebuff()
	{
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(PhysicsWorldSingleton));
		entityQuery.GetSingleton<PhysicsWorldSingleton>().OverlapSphere(base.transform.position, range, ref outHits, collisionFilter);
		foreach (DistanceHit item in outHits)
		{
			if (ettMgr.HasComponent<SpellConfigComponentData>(item.Entity) && DTool.IsSameCamp(ettMgr.GetComponentData<SpellConfigComponentData>(item.Entity).ShooterType, UnitType.Player))
			{
				SpellMovementComponentData componentData = ettMgr.GetComponentData<SpellMovementComponentData>(item.Entity);
				componentData.Speed = Mathf.Lerp(componentData.Speed, 0f, bulletSpeedDownLerpRatio * Time.deltaTime);
				componentData.CurrentFallSpeed = Mathf.Lerp(componentData.CurrentFallSpeed, 0f, bulletSpeedDownLerpRatio * Time.deltaTime);
				componentData.Gravity = Mathf.Lerp(componentData.Gravity, 0f, bulletSpeedDownLerpRatio * Time.deltaTime);
				ettMgr.SetComponentData(item.Entity, componentData);
			}
		}
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, range, GameConst.Filter_MonsterAoe, list);
		for (int i = 0; i < list.Count; i++)
		{
			Entity entity = list[i].entity;
			if (UnitDotsSyncSystem.entityMgr.HasComponent<UnitProperty_Dots>(entity))
			{
				UnitProperty_Dots componentData2 = UnitDotsSyncSystem.entityMgr.GetComponentData<UnitProperty_Dots>(entity);
				if (componentData2.unitCfg.IsSameCamp(UnitType.Player))
				{
					componentData2.SetMucus(0.1f, moveSpeedDownRatio, 1f, changeColor: false);
					UnitDotsSyncSystem.entityMgr.SetComponentData(entity, componentData2);
				}
			}
		}
	}
}
