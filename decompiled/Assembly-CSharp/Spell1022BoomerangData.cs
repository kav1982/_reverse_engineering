using Unity.Entities;

public struct Spell1022BoomerangData : IComponentData, IQueryTypeParameter
{
	public bool IsInitialize;

	public float IgnoreRecycleDurationTimer;

	public float extraLerpSpeed;

	public bool IsRecycleByPlayer;
}
