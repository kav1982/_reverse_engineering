using Unity.Entities;

public struct UnitDead : IComponentData, IQueryTypeParameter
{
	public int deadlyInfoIndex;

	public TakeDamageInfo_Dots deadlyInfo;

	public bool ignoreBeforeAnnouncedDeath;

	public bool playerFinallyDead;
}
