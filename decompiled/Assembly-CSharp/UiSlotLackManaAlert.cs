public class UiSlotLackManaAlert : UISlotWandTipBase
{
	public override void Show()
	{
		UIPlayerDataMgr.Inst.uiSlotWandTips.text_UnableToCastSlotSpellAlert.text = 1000705.GetText();
		base.Show();
	}
}
