using Unity.Entities;

public struct LegsAttackData : IBufferElementData
{
	public int LegIndex;

	public LegsAttackType AttackType;
}
