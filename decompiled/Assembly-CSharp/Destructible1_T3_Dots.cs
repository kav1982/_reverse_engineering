using Unity.Entities;

public struct Destructible1_T3_Dots : IComponentData, IQueryTypeParameter
{
	public Entity ett_Anima;

	public bool isInitialized;

	public ItemInfo rewardItemInfo;

	public bool alreadyDropReward;
}
