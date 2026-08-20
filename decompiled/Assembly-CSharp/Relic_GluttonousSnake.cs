using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Relic_GluttonousSnake : MonoBehaviour
{
	public float bodyDistance;

	public int perBodyFrame;

	private RelicConfig relicCfg;

	private List<Vector3> allPosList = new List<Vector3>();

	private List<Entity> bodyEttList = new List<Entity>();

	private EntityManager ettMgr;

	private float distanceWithPlayer;

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	private void LateUpdate()
	{
		if (bodyEttList.Count == 0)
		{
			if (allPosList.Count != 0)
			{
				allPosList.Clear();
			}
		}
		else if ((PlayerMgr.Inst.PlayerPoint - allPosList[0]).sqrMagnitude > bodyDistance * bodyDistance)
		{
			allPosList.Insert(0, PlayerMgr.Inst.PlayerPoint);
			allPosList.RemoveAt(allPosList.Count - 1);
			for (int i = 0; i < bodyEttList.Count; i++)
			{
				LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(bodyEttList[i]);
				Vector3 v = Vector3.Lerp((Vector3)componentData.Position, allPosList[(i + 1) * perBodyFrame - 1], 10f * Time.deltaTime);
				componentData.Position = Tool2D.IgnoreZPoint(v);
				ettMgr.SetComponentData(bodyEttList[i], componentData);
			}
		}
	}

	public void Initialize(RelicConfig relicCfg)
	{
		this.relicCfg = relicCfg;
	}

	public void CreateBody(float maxHP)
	{
		Entity entity = QuickCreateSystem.Inst.CreateMixedEtt("Relic_GluttonousSnakeBody", float3.zero);
		int num = Mathf.CeilToInt(maxHP * (float)relicCfg.int1.result / 100f);
		if (maxHP > 50000000f)
		{
			num = 10;
		}
		Relic_GluttonousSnakeBody componentData = ettMgr.GetComponentData<Relic_GluttonousSnakeBody>(entity);
		componentData.damage = num;
		componentData.RelicID = relicCfg.id;
		ettMgr.SetComponentData(entity, componentData);
		for (int i = 0; i < perBodyFrame; i++)
		{
			if (allPosList.Count == 0)
			{
				allPosList.Add(PlayerMgr.Inst.PlayerPoint);
			}
			else
			{
				allPosList.Add(allPosList[allPosList.Count - 1]);
			}
		}
		bodyEttList.Add(entity);
		LocalTransform componentData2 = ettMgr.GetComponentData<LocalTransform>(entity);
		componentData2.Position = allPosList[allPosList.Count - 1];
		ettMgr.SetComponentData(entity, componentData2);
	}

	public void RemoveBody(Entity bodyEtt)
	{
		int index = bodyEttList.IndexOf(bodyEtt);
		bodyEttList.RemoveAt(index);
		for (int i = 0; i < perBodyFrame; i++)
		{
			allPosList.RemoveAt(allPosList.Count - 1);
		}
	}

	public void PointerToPlayer()
	{
		allPosList.Clear();
		for (int i = 0; i < bodyEttList.Count * perBodyFrame; i++)
		{
			allPosList.Add(PlayerMgr.Inst.PlayerPoint);
		}
		for (int j = 0; j < bodyEttList.Count; j++)
		{
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(bodyEttList[j]);
			componentData.Position = PlayerMgr.Inst.PlayerPoint;
			ettMgr.SetComponentData(bodyEttList[j], componentData);
		}
	}

	public void DestroySelf()
	{
		for (int i = 0; i < bodyEttList.Count; i++)
		{
			ettMgr.DestroyEntity(bodyEttList[i]);
		}
		Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		if (World.DefaultGameObjectInjectionWorld.IsCreated)
		{
			for (int i = 0; i < bodyEttList.Count; i++)
			{
				ettMgr.DestroyEntity(bodyEttList[i]);
			}
		}
	}
}
