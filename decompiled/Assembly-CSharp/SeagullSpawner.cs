using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SeagullSpawner : MonoBehaviour
{
	private float SeagullSpawnTimer;

	private float _prePlayerPosX;

	private EntityId roomEttId;

	private Vector3 CurrentRoomCenterPos;

	private Vector2 roomSizeToSpawn;

	private Vector2 roomXEdge;

	private bool roomIsInCampOrStoreOrProcess;

	private List<Monster994> _spawnedSegulls = new List<Monster994>();

	private void Awake()
	{
		SeagullSpawnTimer = UnityEngine.Random.Range(4f, 7f);
	}

	private void Update()
	{
		if (!PlayerMgr.Inst.PlayerGO)
		{
			return;
		}
		if (LevelMgr.Inst.CurrentRoomCtrller.GetEntityId() != roomEttId)
		{
			roomEttId = LevelMgr.Inst.CurrentRoomCtrller.GetEntityId();
			CurrentRoomCenterPos = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomSizeToSpawn = LevelMgr.Inst.CurrentRoomCtrller.RoomScale / 2.2f;
			roomXEdge = new Vector2(CurrentRoomCenterPos.x - LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x / 1.8f, CurrentRoomCenterPos.x + LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x / 1.8f);
			roomIsInCampOrStoreOrProcess = LevelMgr.Inst.CurrentRoomCtrller.MapPos.y == 0;
			foreach (Monster994 spawnedSegull in _spawnedSegulls)
			{
				if ((bool)spawnedSegull)
				{
					spawnedSegull.DotsAnnouncedDeath();
				}
			}
			_spawnedSegulls.Clear();
			SeagullSpawnTimer = UnityEngine.Random.Range(30f, 60f);
			if (roomIsInCampOrStoreOrProcess)
			{
				for (int i = 0; i < UnityEngine.Random.Range(0, 3); i++)
				{
					SpawnSegull(isInLand: true);
				}
			}
		}
		if (!roomIsInCampOrStoreOrProcess)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			SpawnSegull();
		}
		if (_spawnedSegulls.Count < 3)
		{
			SeagullSpawnTimer -= Time.deltaTime;
			if (SeagullSpawnTimer <= 0f)
			{
				SeagullSpawnTimer = UnityEngine.Random.Range(30f, 60f);
				SpawnSegull();
			}
		}
	}

	private void SpawnSegull(bool isInLand = false)
	{
		Vector3 startPoint = CurrentRoomCenterPos + new Vector3(UnityEngine.Random.Range(0f - roomSizeToSpawn.x, roomSizeToSpawn.x), UnityEngine.Random.Range(0f - roomSizeToSpawn.y, roomSizeToSpawn.y));
		Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(startPoint);
		float x = ((navMeshPointIngoreZ.x < startPoint.x) ? roomXEdge.x : roomXEdge.y);
		float z = 0f - UnityEngine.Random.Range(4f, 7f);
		Monster994 component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/199401", new Vector3(0f, 0f, 0f)).GetComponent<Monster994>();
		component.Init(new Vector3(x, navMeshPointIngoreZ.y, z), navMeshPointIngoreZ, this, roomXEdge, isInLand);
		_spawnedSegulls.Add(component);
	}

	public void ScareNearlySeagull(Monster994 monster)
	{
		foreach (Monster994 spawnedSegull in _spawnedSegulls)
		{
			if ((bool)monster && (bool)spawnedSegull && !(spawnedSegull == monster) && spawnedSegull.CanInteract)
			{
				float3 point = spawnedSegull.transform.position;
				float3 @float = monster.transform.position;
				if (!(DTool.IgnoreZDistance(in point, in @float) >= 4f))
				{
					spawnedSegull.ScaredSelf();
				}
			}
		}
	}

	public void DestroySegull(Monster994 monster)
	{
		_spawnedSegulls.Remove(monster);
		monster.DotsAnnouncedDeath();
	}
}
