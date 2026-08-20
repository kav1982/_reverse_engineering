using Unity.Entities;

public struct SpellradiuDcreaseTransIntoDamageData : IComponentData, IQueryTypeParameter
{
	public float radiuDecreaseRatio;

	public float radiuDcreaseTransIntoDamageRatio;
}
