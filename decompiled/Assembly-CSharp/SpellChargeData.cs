using Unity.Entities;

public struct SpellChargeData : IComponentData, IQueryTypeParameter
{
	public float ChargeTimer;
}
