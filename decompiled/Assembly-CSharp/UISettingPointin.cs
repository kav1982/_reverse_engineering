using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISettingPointin : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public enum generalFunctions
	{
		ChangeLanguage,
		ChangeRes,
		FrameRateLimit,
		FullScreen,
		ASync,
		MainVolume,
		Music,
		Sound
	}

	public generalFunctions function;

	public Text Highlight_Text1;

	public Text Highlight_Text2;

	public Image Background;

	public GameObject Leftarrow;

	public GameObject Rightarrow;

	public Slider slider;

	public Image SliderPointin;

	public UnityEvent ToggleUnityEvent;

	public void Start()
	{
		if (GameMgr.IsMobile_Static)
		{
			if ((bool)Highlight_Text2)
			{
				Highlight_Text2.color = UIMgr.Inst.uiSetting.Text_pointin;
			}
			return;
		}
		Highlight_Text1.color = UIMgr.Inst.uiSetting.Text_pointout;
		if ((bool)Highlight_Text2)
		{
			Highlight_Text2.color = UIMgr.Inst.uiSetting.Text_pointout;
		}
		Background.color = UIMgr.Inst.uiSetting.pointoutcolor_background;
		if ((bool)Leftarrow)
		{
			Leftarrow.SetActive(value: false);
		}
		if ((bool)Rightarrow)
		{
			Rightarrow.SetActive(value: false);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static && base.enabled)
		{
			Highlight_Text1.color = UIMgr.Inst.uiSetting.Text_pointin;
			if ((bool)Highlight_Text2)
			{
				Highlight_Text2.color = UIMgr.Inst.uiSetting.Text_pointin;
			}
			Background.color = UIMgr.Inst.uiSetting.pointincolor_background;
			if ((bool)Leftarrow)
			{
				Leftarrow.SetActive(value: true);
			}
			if ((bool)Rightarrow)
			{
				Rightarrow.SetActive(value: true);
			}
			if ((bool)SliderPointin)
			{
				SliderPointin.color = UIMgr.Inst.uiSetting.Text_pointin;
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static && base.enabled)
		{
			Highlight_Text1.color = UIMgr.Inst.uiSetting.Text_pointout;
			if ((bool)Highlight_Text2)
			{
				Highlight_Text2.color = UIMgr.Inst.uiSetting.Text_pointout;
			}
			Background.color = UIMgr.Inst.uiSetting.pointoutcolor_background;
			if ((bool)Leftarrow)
			{
				Leftarrow.SetActive(value: false);
			}
			if ((bool)Rightarrow)
			{
				Rightarrow.SetActive(value: false);
			}
			if ((bool)SliderPointin)
			{
				SliderPointin.color = UIMgr.Inst.uiSetting.Text_pointout;
			}
		}
	}

	public void SetDisable()
	{
		base.enabled = false;
		Highlight_Text1.color = UIMgr.Inst.uiSetting.disabledcolor;
		if ((bool)Highlight_Text2)
		{
			Highlight_Text2.color = UIMgr.Inst.uiSetting.disabledcolor;
		}
		if ((bool)Leftarrow)
		{
			Leftarrow.SetActive(value: false);
			Leftarrow.GetComponent<Button>().interactable = false;
			Leftarrow.GetComponent<Image>().color = UIMgr.Inst.uiSetting.disabledcolor;
		}
		if ((bool)Rightarrow)
		{
			Rightarrow.SetActive(value: false);
			Rightarrow.GetComponent<Button>().interactable = false;
			Rightarrow.GetComponent<Image>().color = UIMgr.Inst.uiSetting.disabledcolor;
		}
		Background.color = UIMgr.Inst.uiSetting.disabledcolor;
		Background.sprite = UIMgr.Inst.uiSetting.pointinBackgroundDisabled;
	}

	public void SetEnable()
	{
		base.enabled = true;
		Highlight_Text1.color = UIMgr.Inst.uiSetting.colorUnselected_text;
		if ((bool)Highlight_Text2)
		{
			Highlight_Text2.color = UIMgr.Inst.uiSetting.enableColor;
		}
		if ((bool)Leftarrow)
		{
			if (GameMgr.IsMobile_Static)
			{
				Leftarrow.SetActive(value: true);
			}
			Leftarrow.GetComponent<Button>().interactable = true;
			Leftarrow.GetComponent<Image>().color = UIMgr.Inst.uiSetting.buttonEnable;
		}
		if ((bool)Rightarrow)
		{
			if (GameMgr.IsMobile_Static)
			{
				Rightarrow.SetActive(value: true);
			}
			Rightarrow.GetComponent<Button>().interactable = true;
			Rightarrow.GetComponent<Image>().color = UIMgr.Inst.uiSetting.buttonEnable;
		}
		Background.sprite = UIMgr.Inst.uiSetting.pointinBackgroundHighlight;
		if (GameMgr.IsMobile_Static)
		{
			Background.color = UIMgr.Inst.uiSetting.enableColor;
		}
		else
		{
			Background.color = Color.clear;
		}
	}
}
