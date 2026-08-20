using Unity.Entities;
using UnityEngine.AI;

public struct CreateNavMeshObstacleCleanUp : ICleanupComponentData, IComponentData, IQueryTypeParameter
{
	public bool isObjPool;

	public UnityObjectRef<NavMeshObstacle> obstacle;
}
