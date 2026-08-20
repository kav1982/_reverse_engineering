using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayerSlider : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public UIPlayerSliderType sliderType;

	public void OnPointerEnter(PointerEventData eventData)
	{
		switch (sliderType)
		{
		case UIPlayerSliderType.HP:
			UIPlayerDataMgr.Inst.MouseHoverHPMP(sliderType, isHover: true);
			break;
		case UIPlayerSliderType.MP:
			UIPlayerDataMgr.Inst.MouseHoverHPMP(sliderType, isHover: true);
			break;
		default:
			Debug.LogError(sliderType);
			break;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		switch (sliderType)
		{
		case UIPlayerSliderType.HP:
			UIPlayerDataMgr.Inst.MouseHoverHPMP(sliderType, isHover: false);
			break;
		case UIPlayerSliderType.MP:
			UIPlayerDataMgr.Inst.MouseHoverHPMP(sliderType, isHover: false);
			break;
		default:
			Debug.LogError(sliderType);
			break;
		}
	}
}
