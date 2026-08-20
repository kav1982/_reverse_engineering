using Unity.Entities;

public struct SpellHalfLifeTeleportData : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public float TeleportRadius;

	public int TeleportCount;
}
