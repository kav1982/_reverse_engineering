using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICampSkinSlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public GameObject go_CurrentSymbol;

	public GameObject go_Outline;

	public UICampSkinChanger uiCampSkinChanger;

	public CampSkinType skinType;

	public Text text_Name;

	public void LanguageChange()
	{
		switch (skinType)
		{
		case CampSkinType.Default:
			text_Name.text = 1004152.GetText();
			break;
		case CampSkinType.Halloween:
			text_Name.text = 1004153.GetText();
			break;
		case CampSkinType.Spring:
			text_Name.text = 1004154.GetText();
			break;
		case CampSkinType.Summer:
			text_Name.text = 1004155.GetText();
			break;
		case CampSkinType.Christmas:
			text_Name.text = 1004156.GetText();
			break;
		default:
			Debug.LogError(skinType);
			break;
		}
		go_CurrentSymbol.SetActive(skinType == DataMgr.selectedWorldData.campSkinType);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SEMgr.Inst.uiButtonSwitch.PlaySE();
		go_Outline.SetActive(value: true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		go_Outline.SetActive(value: false);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		uiCampSkinChanger.UICampSkinClick(this);
	}
}
