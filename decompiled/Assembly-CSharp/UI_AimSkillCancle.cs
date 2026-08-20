using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AimSkillCancle : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public UI_AimSkill uI_AimSkill;

	public Image image;

	public Color cancleColor;

	public Color normalColor;

	private void OnEnable()
	{
		image.color = normalColor;
		uI_AimSkill.skillCancle = false;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		uI_AimSkill.skillCancle = false;
		image.color = normalColor;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!UIMgr.Inst.uiSetting.customMobileControl.activeInHierarchy)
		{
			uI_AimSkill.skillCancle = true;
			image.color = cancleColor;
		}
	}
}
