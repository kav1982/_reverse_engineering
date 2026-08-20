using Unity.Entities;

public struct Destructible4_T3_Dots : IComponentData, IQueryTypeParameter
{
	public Entity ett_Fruit;

	public bool isInitialized;

	public ItemInfo rewardItemInfo;

	public bool alreadyDropReward;
}
