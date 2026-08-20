public struct ItemAbilityFloat
{
	public float value;

	public float valueUpgrade;

	public float result;

	public ItemAbilityFloat(float value, float valueUpgrade)
	{
		this.value = value;
		this.valueUpgrade = valueUpgrade;
		result = 0f;
	}

	public float RandomResult(int itemLevel)
	{
		result = value + (float)(itemLevel - 1) * valueUpgrade;
		return result;
	}
}
