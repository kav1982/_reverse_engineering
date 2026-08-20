using Unity.Burst;

[BurstCompile]
public struct PenetrateValue
{
	public int Base;

	public int Extra;

	public PenetrateValue(int ExtraValue = 0)
	{
		this = default(PenetrateValue);
		Base = 0;
		Extra = ExtraValue;
	}

	public void CostPenetrateValue()
	{
		if (Extra > 0)
		{
			Extra--;
		}
		else
		{
			Base--;
		}
	}

	public readonly int Calculate()
	{
		return Extra + Base;
	}
}
