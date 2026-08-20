using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EndlessChallengePatternData : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public Image Icon;

	public Image Frame;

	public Image BgOn;

	public Button Button;

	public void SetBgState(bool BgOn)
	{
		this.BgOn.gameObject.SetActive(BgOn);
	}

	public void SetIconSprite(Sprite sprite)
	{
		Icon.sprite = sprite;
	}

	public void SetFrameState(bool isOn)
	{
		Frame.gameObject.SetActive(isOn);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SetFrameState(isOn: true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SetFrameState(isOn: false);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
	}
}
