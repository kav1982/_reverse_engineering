using Unity.Entities;

public struct LegsTarget : IBufferElementData
{
	public int AttackedFuseHeadLegIndex;

	public Entity Target;

	public LegsTargetStatus Status;
}
