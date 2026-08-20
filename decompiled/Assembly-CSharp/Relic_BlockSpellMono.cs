using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Relic_BlockSpellMono : MonoBehaviour
{
	public float rotateSpeed;

	public float extraDistanceWithCenter;

	private EntityManager ettMgr;

	public RelicConfig RelicCfg { get; private set; }

	public float DistanceWithPlayer { get; private set; }

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	public void Initialize(RelicConfig relicCfg)
	{
		RelicCfg = relicCfg;
		CorrectDistance();
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(Relic_BlockSpell));
		int num = entityQuery.CalculateEntityCount();
		if (num < PlayerMgr.Inst.BaData.relicBlockSpellCount)
		{
			int num2 = PlayerMgr.Inst.BaData.relicBlockSpellCount - num;
			for (int i = 0; i < num2; i++)
			{
				AddBlock(PlayerMgr.Inst.PlayerPoint);
			}
		}
	}

	public void CorrectDistance()
	{
		DistanceWithPlayer = PlayerMgr.Inst.PlayerPpt.CC_Self.radius * PlayerMgr.Inst.PlayerPpt.transform.localScale.x + extraDistanceWithCenter;
	}

	public void AddBlock(Vector3 bornPoint)
	{
		QuickCreateSystem.Inst.CreateMixedEtt("Relic_BlockSpell", Tool2D.IgnoreZPoint(bornPoint));
	}

	public void PointerToPlayer()
	{
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(Relic_BlockSpell));
		NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.TempJob);
		foreach (Entity item in nativeArray)
		{
			ettMgr.SetComponentData(item, LocalTransform.FromPosition(Tool2D.IgnoreZPoint(PlayerMgr.Inst.PlayerPoint)));
		}
		nativeArray.Dispose();
	}

	public void DestroySelf()
	{
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(Relic_BlockSpell));
		NativeArray<Entity> entities = entityQuery.ToEntityArray(Allocator.TempJob);
		ettMgr.DestroyEntity(entities);
		entities.Dispose();
		Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		if (!World.DefaultGameObjectInjectionWorld.IsCreated)
		{
			return;
		}
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(Relic_BlockSpell));
		NativeArray<Entity> entities = entityQuery.ToEntityArray(Allocator.TempJob);
		ettMgr.DestroyEntity(entities);
		entities.Dispose();
	}
}
