using Unity.Entities;

public struct BattleFinishDrop : IComponentData, IQueryTypeParameter
{
	public bool isInitailized;

	public UnityObjectRef<BattleFinishDropMono> dropMono;

	public DifficultyType type;
}
