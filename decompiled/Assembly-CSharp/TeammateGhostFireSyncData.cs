using Unity.Entities;

public struct TeammateGhostFireSyncData : IComponentData, IQueryTypeParameter
{
	public Entity Teammate;
}
