using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIActivateGirlShowItem : MonoBehaviour, IPointerExitHandler, IEventSystemHandler, IPointerEnterHandler
{
	public Image image_Outline;

	public Image image_Icon;

	private UIActivateGirl uiActivateGirl;

	public int ID { get; private set; }

	public bool IsSpell { get; private set; }

	public void Initialize(UIActivateGirl uiActivateGirl, int id, bool isSpell = true)
	{
		this.uiActivateGirl = uiActivateGirl;
		ID = id;
		IsSpell = isSpell;
		if (isSpell)
		{
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[id].GetIconPath());
		}
		else
		{
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(RelicConfig.dic[id].GetIconPath());
		}
		image_Outline.sprite = image_Icon.sprite;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		uiActivateGirl.ShowItemEnter(this);
		image_Outline.gameObject.SetActive(value: true);
		SEMgr.Inst.uiButtonSwitch.PlaySE();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		uiActivateGirl.ShowItemExit();
		image_Outline.gameObject.SetActive(value: false);
	}
}
