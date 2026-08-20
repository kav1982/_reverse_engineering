using System.Collections;
using Unity.Entities;
using UnityEngine;

public class Potion_Fortune : MonoBehaviour
{
	public float dropRadius;

	public float dropInterval;

	public void Initialize(int coinCount)
	{
		StartCoroutine(InitializeIE(coinCount));
	}

	private IEnumerator InitializeIE(int coinCount)
	{
		int _emeralldCount = 0;
		int _diamondCount = 0;
		int num = 0;
		if (coinCount >= 5000)
		{
			num = coinCount - 5000;
			coinCount = 5000;
		}
		if (coinCount >= 500)
		{
			_emeralldCount = coinCount / 50;
			_diamondCount = coinCount % 50 / 5;
			coinCount %= 5;
		}
		else if (coinCount >= 100)
		{
			_diamondCount = coinCount / 5;
			coinCount %= 5;
		}
		if (num > 0)
		{
			Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(base.transform.position, Random.Range(0f, dropRadius));
			Entity entity = QuickCreateSystem.Inst.CreateMixedEtt("Spell10201Coin", navMeshPointIngoreZ);
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			Spell10201Coin componentData = entityManager.GetComponentData<Spell10201Coin>(entity);
			componentData.belongRoomMapPos = LevelMgr.Inst.CurrentRoomMapPos;
			componentData.coinCount = num;
			entityManager.SetComponentData(entity, componentData);
		}
		for (int k = 0; k < _emeralldCount; k++)
		{
			Vector3 navMeshPointIngoreZ2 = Tool2D.GetNavMeshPointIngoreZ(base.transform.position, Random.Range(0f, dropRadius));
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomCtrller.MapPos, new ItemInfo(ItemType.Resource, 13), navMeshPointIngoreZ2);
			yield return new WaitForSeconds(dropInterval);
		}
		for (int k = 0; k < _diamondCount; k++)
		{
			Vector3 navMeshPointIngoreZ3 = Tool2D.GetNavMeshPointIngoreZ(base.transform.position, Random.Range(0f, dropRadius));
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomCtrller.MapPos, new ItemInfo(ItemType.Resource, 12), navMeshPointIngoreZ3);
			yield return new WaitForSeconds(dropInterval);
		}
		for (int k = 0; k < coinCount; k++)
		{
			Vector3 navMeshPointIngoreZ4 = Tool2D.GetNavMeshPointIngoreZ(base.transform.position, Random.Range(0f, dropRadius));
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomCtrller.MapPos, new ItemInfo(ItemType.Resource, 11), navMeshPointIngoreZ4);
			yield return new WaitForSeconds(dropInterval);
		}
		ObjPoolMgr.Inst.RecycleGO(base.gameObject, 3f);
	}
}
