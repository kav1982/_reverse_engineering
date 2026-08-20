using Unity.Entities;

public struct LevelRewardInfoBED : IBufferElementData
{
	public ItemInfo info;

	public bool isPicked;

	public bool isLock;

	public LevelRewardInfoBED(ItemType itemType, int itemID)
	{
		info = new ItemInfo(itemType, itemID);
		isPicked = false;
		isLock = false;
	}
}
