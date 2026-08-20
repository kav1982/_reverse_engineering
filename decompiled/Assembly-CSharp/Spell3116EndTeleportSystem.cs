using UnityEngine;

public static class Spell3116EndTeleportSystem
{
	public static void Teleport(UnitProperty ownerPpt, Vector3 pos)
	{
		if (ownerPpt != null && ownerPpt.gameObject.activeInHierarchy)
		{
			Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(pos);
			CreateAndInsertNewTrail(navMeshPointIngoreZ);
			ownerPpt.transform.position = navMeshPointIngoreZ;
			SEMgr.Inst.spell31161Teleport.PlaySE();
		}
	}

	private static void CreateAndInsertNewTrail(Vector3 spawnPosition)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/Spell/31161", spawnPosition, 0.5f);
	}
}
