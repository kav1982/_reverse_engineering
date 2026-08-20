using Unity.Entities;

public struct BladeShootListenerData : IBufferElementData
{
	public int ShootingWandId;

	public int ShootCount;

	public int EventType;
}
