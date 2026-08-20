using Unity.Entities;

public struct Spell1020ManaCoinData : IComponentData, IQueryTypeParameter
{
	public bool IsInitialized;

	public int CoinUseCount;

	public float BuffRatio;
}
