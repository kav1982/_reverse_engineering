public class SummonUnitPropertyValueFix
{
	public float damageRatio = 1f;

	public float moveSpeedRatio;

	public float attackSpeedRatio;

	public SummonUnitPropertyValueFix(SpellBase targetbase)
	{
		moveSpeedRatio = targetbase.SpellSummonMoveRatio;
		attackSpeedRatio = targetbase.SpellSummonAttackSpeedRatio;
	}
}
