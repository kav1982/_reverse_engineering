using UnityEngine.UI;

public class UiSlotUnusedAlert : UISlotWandTipBase
{
	public Wand.UnusedEnhanceType Type;

	public override void Show()
	{
		Text text_Unused = UIPlayerDataMgr.Inst.uiSlotWandTips.text_Unused;
		text_Unused.text = Type switch
		{
			Wand.UnusedEnhanceType.LeftNoSpell => 1000707.GetText(), 
			Wand.UnusedEnhanceType.RightNoSpell => 1000706.GetText(), 
			_ => UIPlayerDataMgr.Inst.uiSlotWandTips.text_Unused.text, 
		};
		base.Show();
	}
}
