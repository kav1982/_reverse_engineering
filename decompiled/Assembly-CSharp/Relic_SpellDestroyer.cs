using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Relic_SpellDestroyer : MonoBehaviour
{
	public float checkSpellInterval;

	public float checkRadius;

	public float rotateSpeed;

	public float distance;

	public float zMinHeight;

	public float zMaxHeight;

	public float recoveryTime;

	private List<GameObject> destroyers = new List<GameObject>();

	private List<float> zOffset = new List<float>();

	private float currentAngle;

	private float recoveryTimer;

	private float checkSpellIntervalTimer;

	private EntityManager ettMgr;

	private CollisionFilter collisionFilter;

	private void Start()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		collisionFilter = new CollisionFilter
		{
			BelongsTo = uint.MaxValue,
			CollidesWith = 8388608u,
			GroupIndex = 0
		};
	}

	private void LateUpdate()
	{
		base.transform.position = PlayerMgr.Inst.PlayerPoint;
		if (!destroyers[0].activeSelf)
		{
			recoveryTimer += Time.deltaTime;
			if (recoveryTimer >= recoveryTime)
			{
				recoveryTimer = 0f;
				for (int num = destroyers.Count - 1; num >= 0; num--)
				{
					if (!destroyers[num].activeSelf)
					{
						destroyers[num].SetActive(value: true);
						zOffset[num] = 0f - UnityEngine.Random.Range(zMinHeight, zMaxHeight);
						break;
					}
				}
			}
		}
		else
		{
			recoveryTimer = 0f;
		}
		currentAngle += rotateSpeed * Time.deltaTime;
		if (currentAngle > 360f)
		{
			currentAngle -= 360f;
		}
		for (int i = 0; i < destroyers.Count; i++)
		{
			destroyers[i].transform.position = base.transform.position + new Vector3(0f, 0f, zOffset[i]) + Tool2D.GetDir(currentAngle + (float)(360 / destroyers.Count * i)) * distance;
		}
		if (!destroyers[destroyers.Count - 1].activeSelf)
		{
			return;
		}
		checkSpellIntervalTimer += Time.deltaTime;
		if (checkSpellIntervalTimer < checkSpellInterval)
		{
			return;
		}
		checkSpellIntervalTimer = 0f;
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(PhysicsWorldSingleton));
		entityQuery.GetSingleton<PhysicsWorldSingleton>().OverlapSphere(base.transform.position, checkRadius, ref outHits, collisionFilter);
		foreach (DistanceHit item in outHits)
		{
			GameObject gameObject = null;
			float3 @float = float3.zero;
			if (ettMgr.HasComponent<SpellConfigComponentData>(item.Entity) && DTool.IsSameCamp(ettMgr.GetComponentData<SpellConfigComponentData>(item.Entity).ShooterType, UnitType.Monster))
			{
				for (int j = 0; j < destroyers.Count; j++)
				{
					if (destroyers[j].activeSelf)
					{
						gameObject = destroyers[j];
						break;
					}
				}
				@float = ettMgr.GetComponentData<LocalTransform>(item.Entity).Position;
				using EntityQuery entityQuery2 = ettMgr.CreateEntityQuery(typeof(SpellSingleton));
				SpellSingleton singleton = entityQuery2.GetSingleton<SpellSingleton>();
				int prefabId = ettMgr.GetComponentData<SpellComponentData>(item.Entity).PrefabId;
				FixedString32Bytes effectName = "Hit";
				if (singleton.TryGetSpellEffectEntity(prefabId, in effectName, SpellColorType.Monster, out var entity))
				{
					Entity entity2 = ettMgr.Instantiate(entity);
					ettMgr.SetComponentData(entity2, LocalTransform.FromPosition(@float));
				}
				ettMgr.SetComponentEnabled<SpellDestroyTag>(item.Entity, value: true);
			}
			if (gameObject != null)
			{
				gameObject.SetActive(value: false);
				ObjPoolMgr.Inst.GetGO("Prefabs/Item/Relic_SpellDestroyer_Hit", Vector3.zero, 2f).GetComponent<Relic_SpellDestroyer_Hit>().Initialize(gameObject.transform.position, @float);
				break;
			}
		}
	}

	public void Initialize(RelicConfig relicCfg)
	{
		while (destroyers.Count < relicCfg.int1.result)
		{
			GameObject item = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Relic_SpellDestroyer_Missile"), base.transform);
			destroyers.Add(item);
			zOffset.Add(0f - UnityEngine.Random.Range(zMinHeight, zMaxHeight));
		}
	}

	public void DestroySelf()
	{
		for (int num = destroyers.Count - 1; num >= 0; num--)
		{
			Object.Destroy(destroyers[num]);
		}
		Object.Destroy(base.gameObject);
	}
}
