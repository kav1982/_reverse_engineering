using Unity.Entities;
using Unity.Mathematics;

public struct Monster322Data : IComponentData, IQueryTypeParameter
{
	public float HealPercent;

	public float HealInterval;

	public float HealTimer;

	public float HealRange;

	public float CloseToTargetRange;

	public Entity TargetEntity;

	public float3 TargetLastFramePosition;

	public bool TargetMoveToRight;

	public float RecheckTimer;

	public float RecheckInterval;

	public bool IsInitialized;

	public Entity ChainEntity;
}
