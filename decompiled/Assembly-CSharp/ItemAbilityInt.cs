public struct ItemAbilityInt
{
	public int value;

	public int valueUpgrade;

	public int result;

	public ItemAbilityInt(int value, int valueUpgrade)
	{
		this.value = value;
		this.valueUpgrade = valueUpgrade;
		result = 0;
	}

	public int RandomResult(int itemLevel)
	{
		result = value + (itemLevel - 1) * valueUpgrade;
		return result;
	}
}
