using UnityEngine;

internal static class EffectTransparencyExtend
{
	public static float GetTransparency(this EffectTransparencyController.ControlMode Mode)
	{
		return Mode switch
		{
			EffectTransparencyController.ControlMode.Spell => DataMgr.settingData.FinalSpellTransparent, 
			EffectTransparencyController.ControlMode.Summon => DataMgr.settingData.FinalSummonTransparent, 
			EffectTransparencyController.ControlMode.Min => Mathf.Min(DataMgr.settingData.FinalSpellTransparent, DataMgr.settingData.FinalSummonTransparent), 
			EffectTransparencyController.ControlMode.Max => Mathf.Max(DataMgr.settingData.FinalSpellTransparent, DataMgr.settingData.FinalSummonTransparent), 
			_ => 1f, 
		};
	}
}
