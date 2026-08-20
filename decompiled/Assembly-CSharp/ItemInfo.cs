public struct ItemInfo
{
	public ItemType type;

	public int id;

	public int specialInt;

	public ItemInfo(ItemType type, int id)
	{
		this.type = type;
		this.id = id;
		specialInt = 0;
	}

	public ItemInfo(SlotData slotData)
	{
		type = ItemType.Spell;
		id = slotData.id;
		specialInt = slotData.specialInt;
	}
}
