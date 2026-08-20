using System.Collections.Generic;

public class FusionProperty
{
	public int CurrentFusionLevel { get; set; }

	public int MaxFusionLevel { get; set; }

	public int SourceTeammateID { get; set; }

	public bool IsFusing { get; set; }

	public bool IsFusedUnit { get; set; }

	public float FuseBuffRatio { get; set; }

	public void InitFuseData(int maxLevel, float ratio)
	{
		CurrentFusionLevel = 0;
		IsFusing = false;
		IsFusedUnit = false;
		FuseBuffRatio = 0f;
		MaxFusionLevel = maxLevel;
		FuseBuffRatio = ratio;
	}

	public void TryFuseTargetUnit(UnitProperty targetPpt, UnitProperty ownerPpt)
	{
	}

	private List<UnitProperty> GetFuseSearchList(UnitProperty targetPpt, UnitProperty ownerPpt)
	{
		if (ownerPpt != null && ownerPpt.unitCfg.unitType == UnitType.Player)
		{
			if (PlayerMgr.Inst.summonsPpts.Contains(targetPpt))
			{
				return PlayerMgr.Inst.summonsPpts;
			}
			if (PlayerMgr.Inst.summonsNotAttackPpts.Contains(targetPpt))
			{
				return PlayerMgr.Inst.summonsNotAttackPpts;
			}
		}
		return null;
	}
}
