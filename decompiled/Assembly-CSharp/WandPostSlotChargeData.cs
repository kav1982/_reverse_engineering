public class WandPostSlotChargeData
{
	public WandPostSlotTriggerType chargeType;

	public Wand chargeTargetWand;

	public WandConfig chargeWandCfg;

	public float chargeRatioAmount;

	public WandPostSlotChargeData(WandPostSlotTriggerType type, Wand wand, WandConfig wandCfg, float amount)
	{
		chargeType = type;
		chargeTargetWand = wand;
		chargeWandCfg = wandCfg;
		chargeRatioAmount = amount;
	}
}
