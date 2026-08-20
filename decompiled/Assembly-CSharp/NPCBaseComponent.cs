using Unity.Entities;

public struct NPCBaseComponent : IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<NPCBase> npcBase;
}
