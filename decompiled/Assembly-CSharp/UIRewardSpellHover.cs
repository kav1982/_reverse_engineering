using UnityEngine;
using UnityEngine.EventSystems;

public class UIRewardSpellHover : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public UIRewardSpell uiRewardSpell;

	public UIRewardSpellHoverType type;

	public void OnPointerEnter(PointerEventData eventData)
	{
		switch (type)
		{
		case UIRewardSpellHoverType.BG:
			uiRewardSpell.PointerEnter();
			break;
		case UIRewardSpellHoverType.LockButton:
			uiRewardSpell.PointerEnterLockButton();
			break;
		default:
			Debug.LogError(type);
			break;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		switch (type)
		{
		case UIRewardSpellHoverType.BG:
			uiRewardSpell.PointerExit();
			break;
		case UIRewardSpellHoverType.LockButton:
			uiRewardSpell.PointerExitLockButton();
			break;
		default:
			Debug.LogError(type);
			break;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		switch (type)
		{
		case UIRewardSpellHoverType.BG:
			uiRewardSpell.PointerClick();
			break;
		case UIRewardSpellHoverType.LockButton:
			uiRewardSpell.PointerClickLockButton();
			break;
		default:
			Debug.LogError(type);
			break;
		}
	}
}
