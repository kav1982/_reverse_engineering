public class UiSlotMimicError : UISlotWandTipBase
{
	public override void Show()
	{
		UIPlayerDataMgr.Inst.uiSlotWandTips.text_MimicError.text = 1000705.GetText();
		base.Show();
	}
}
