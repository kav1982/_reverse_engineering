using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

public class UIPotionSelectDrop : MonoBehaviour
{
	public OnScreenButtonDrink onScreenButtonDrink;

	public void ButtonClick()
	{
		if (!UIMgr.Inst.uiSetting.customMobileControl.activeInHierarchy)
		{
			onScreenButtonDrink.DropSelectPotion();
			TopUI.inst.uiPotionSelectPopOut.RefreshPotionPopOut();
		}
	}
}
