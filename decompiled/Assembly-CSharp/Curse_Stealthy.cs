using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

public class Curse_Stealthy : MonoBehaviour
{
	public int maxSneaky;

	public float checkItemInterval;

	public float checkItemIntervalWhenGetOne;

	public float walkableThreshold;

	public float t6EntadDistance;

	private List<Monster996> monsters = new List<Monster996>();

	private List<Monster996Born> borns = new List<Monster996Born>();

	private float checkItemIntervalTimer;

	private EntityQuery itemQuery;

	private EntityManager ettMgr;

	public MiniObjPool MiniPool { get; set; }

	private void Start()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		itemQuery = ettMgr.CreateEntityQuery(typeof(Item));
		MiniPool = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool")).GetComponent<MiniObjPool>();
	}

	private void Update()
	{
		if (!PlayerMgr.Inst.PlayerCtrller.CanMotion)
		{
			return;
		}
		checkItemIntervalTimer += Time.deltaTime;
		if (!(checkItemIntervalTimer >= checkItemInterval))
		{
			return;
		}
		checkItemIntervalTimer = 0f;
		if (monsters.Count + borns.Count >= maxSneaky)
		{
			return;
		}
		Entity canStealItem = GetCanStealItem();
		if (!UnitDotsSyncSystem.EntityIsValid(canStealItem))
		{
			return;
		}
		Item componentData = ettMgr.GetComponentData<Item>(canStealItem);
		Vector3 vector = ettMgr.GetComponentData<LocalTransform>(canStealItem).Position;
		checkItemIntervalTimer = checkItemInterval - checkItemIntervalWhenGetOne;
		Vector3 zero = Vector3.zero;
		if (LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			float num = (float)(LevelMgr.Inst.CurrentRoomCfg.theme6Width / 2) - t6EntadDistance;
			float num2 = (float)(LevelMgr.Inst.CurrentRoomCfg.theme6Height / 2) - t6EntadDistance;
			if (Random.Range(0, 2) == 0)
			{
				num = 0f - num;
			}
			if (Random.Range(0, 2) == 0)
			{
				num2 = 0f - num2;
			}
			zero = Tool2D.GetNavMeshPointIngoreZ(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(num, num2, 0f));
		}
		else
		{
			NavMeshPath navMeshPath = Tool2D.GetNavMeshPath(vector, vector + Tool2D.GetDir() * 100f);
			zero = navMeshPath.corners[navMeshPath.corners.Length - 1];
		}
		Monster996Born component = MiniPool.GetGO("Prefabs/EF/EF_Monster996Born", zero).GetComponent<Monster996Born>();
		component.Initialize(this, LevelMgr.Inst.CurrentRoomCtrller, canStealItem);
		componentData.isCurseStealthyTarget = true;
		ettMgr.SetComponentData(canStealItem, componentData);
		BornRegister(component);
	}

	public Entity GetCanStealItem()
	{
		List<Entity> list = new List<Entity>();
		NativeArray<Entity> nativeArray = itemQuery.ToEntityArray(Allocator.Temp);
		NativeArray<Item> nativeArray2 = itemQuery.ToComponentDataArray<Item>(Allocator.Temp);
		for (int i = 0; i < nativeArray2.Length; i++)
		{
			if (!nativeArray2[i].isStore && !nativeArray2[i].isCurseStealthyTarget)
			{
				Vector3 vector = ettMgr.GetComponentData<LocalTransform>(nativeArray[i]).Position;
				if ((LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(vector) - vector).sqrMagnitude < walkableThreshold * walkableThreshold)
				{
					list.Add(nativeArray[i]);
				}
			}
		}
		nativeArray2.Dispose();
		if (list.Count > 0)
		{
			return list.GetRandom();
		}
		return Entity.Null;
	}

	public void BornRegister(Monster996Born born)
	{
		borns.Add(born);
	}

	public void BornUnregister(Monster996Born born)
	{
		borns.Remove(born);
	}

	public void MonsterRegister(Monster996 monster996)
	{
		monsters.Add(monster996);
	}

	public void MonsterUnregister(Monster996 monster996)
	{
		monsters.Remove(monster996);
	}

	public void EnterSideRoom(RoomController beforeRoomCtrller)
	{
		NativeArray<Entity> nativeArray = itemQuery.ToEntityArray(Allocator.Temp);
		for (int i = 0; i < nativeArray.Length; i++)
		{
			Entity entity = nativeArray[i];
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(entity);
			componentData.Position = Tool2D.IgnoreZPoint(componentData.Position);
			ettMgr.SetComponentData(entity, componentData);
			Item componentData2 = ettMgr.GetComponentData<Item>(entity);
			if (!componentData2.isStore)
			{
				PhysicsCollider pc = ettMgr.GetComponentData<PhysicsCollider>(entity);
				DTool.SetCollider(in pc, 262144u);
				ettMgr.SetComponentData(entity, pc);
			}
			componentData2.isCurseStealthyTarget = false;
			ettMgr.SetComponentData(entity, componentData2);
		}
		nativeArray.Dispose();
		for (int j = 0; j < monsters.Count; j++)
		{
			beforeRoomCtrller.UnitUnregister(monsters[j].myPpt.myEntity);
			ettMgr.DestroyEntity(monsters[j].myPpt.myEntity);
			MiniPool.RecycleGO(monsters[j].gameObject);
		}
		for (int num = borns.Count - 1; num >= 0; num--)
		{
			MiniPool.RecycleGO(borns[num].gameObject);
		}
		borns.Clear();
		monsters.Clear();
	}

	public void EnterDoor()
	{
		for (int num = borns.Count - 1; num >= 0; num--)
		{
			MiniPool.RecycleGO(borns[num].gameObject);
		}
		for (int i = 0; i < monsters.Count; i++)
		{
			LevelMgr.Inst.CurrentRoomCtrller.UnitUnregister(monsters[i].myPpt.myEntity);
			ettMgr.DestroyEntity(monsters[i].myPpt.myEntity);
			MiniPool.RecycleGO(monsters[i].gameObject);
		}
		borns.Clear();
		monsters.Clear();
	}

	public void DestroySelf()
	{
		for (int i = 0; i < monsters.Count; i++)
		{
			LevelMgr.Inst.CurrentRoomCtrller.UnitUnregister(monsters[i].myPpt.myEntity);
			ettMgr.DestroyEntity(monsters[i].myPpt.myEntity);
		}
		NativeArray<Entity> nativeArray = itemQuery.ToEntityArray(Allocator.Temp);
		for (int j = 0; j < nativeArray.Length; j++)
		{
			Entity entity = nativeArray[j];
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(entity);
			componentData.Position = Tool2D.IgnoreZPoint(componentData.Position);
			ettMgr.SetComponentData(entity, componentData);
			Item componentData2 = ettMgr.GetComponentData<Item>(entity);
			componentData.Position = Tool2D.IgnoreZPoint(componentData.Position);
			if (!componentData2.isStore)
			{
				PhysicsCollider pc = ettMgr.GetComponentData<PhysicsCollider>(entity);
				DTool.SetCollider(in pc, 262144u);
				ettMgr.SetComponentData(entity, pc);
			}
			componentData2.isCurseStealthyTarget = false;
			ettMgr.SetComponentData(entity, componentData2);
			ettMgr.SetComponentData(entity, componentData);
		}
		nativeArray.Dispose();
		Object.Destroy(MiniPool.gameObject);
		Object.Destroy(base.gameObject);
	}

	public void OnDestroy()
	{
		itemQuery.Dispose();
	}
}
