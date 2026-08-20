using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIHandbook : GameUI
{
	private new bool init;

	public UIHandbookSlot pfb_SlotPC;

	public RectTransform rtsf_Content;

	public Text text_Title;

	public Text text_Desc;

	public Color colorHandbookSlotWhenFold;

	public Color colorHandbookSlotWhenExpand;

	public Scrollbar scrollbar;

	[Header("Demo")]
	public GameObject go_Demo;

	public GameObject go_Texture;

	public GameObject go_VideoText;

	public GameObject go_Video;

	private UIHandbookVideoTextCtrl videoTextCtrl;

	private UIImageVideoPlayer imageVideoPlayer;

	public List<UIHandbookSlot> Slots { get; } = new List<UIHandbookSlot>();


	public int Selectindex { get; set; }

	private UIHandbookSlot pfb_Slot => pfb_SlotPC;

	private void OnEnable()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(OnInputChange));
		OnInputChange();
	}

	private void OnDisable()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(OnInputChange));
		ClearMp4GameObject();
	}

	private void LanguageChange()
	{
		for (int i = 0; i < Slots.Count; i++)
		{
			Slots[i].UpdateInfo();
		}
	}

	private void Start()
	{
		if ((bool)EasyFinishBackHomeMgr.Inst)
		{
			return;
		}
		init = true;
		scrollbar.value = 1f;
		List<HandbookBelongCategory> list = new List<HandbookBelongCategory>();
		int num = 0;
		foreach (HandbookConfig item in HandbookConfig.list)
		{
			if (!list.Contains(item.belongCategory))
			{
				list.Add(item.belongCategory);
			}
			UIHandbookSlot component = UnityEngine.Object.Instantiate(pfb_Slot, rtsf_Content).GetComponent<UIHandbookSlot>();
			component.InitializeSlot(this, num, item);
			Slots.Add(component);
			num++;
		}
		OnInputChange();
	}

	public void SlotEnter(UIHandbookSlot slot)
	{
		if (slot.HandbookCfg == null)
		{
			return;
		}
		text_Title.text = "?? " + slot.HandbookCfg.GetTitle() + " ??";
		if (ScriptableObjMgr.Inst.testCtrller.ShowItemID)
		{
			text_Title.text += slot.HandbookCfg.id;
		}
		text_Desc.text = slot.HandbookCfg.GetDesc();
		text_Desc.text = GeneralTool.FormatTextIfPublishTest(text_Desc, text_Desc.text);
		go_Demo.SetActive(slot.HandbookCfg.demoType == HandbookDemoType.Mp4);
		go_Texture.SetActive(slot.HandbookCfg.demoType == HandbookDemoType.Texture);
		switch (slot.HandbookCfg.demoType)
		{
		case HandbookDemoType.Mp4:
		{
			ClearMp4GameObject();
			GameObject gameObject = ABResources.LoadAsset<GameObject>("Handbook/" + slot.HandbookCfg.id + "Player");
			if (!(gameObject == null))
			{
				imageVideoPlayer = UnityEngine.Object.Instantiate(gameObject, go_Video.transform).GetComponent<UIImageVideoPlayer>();
				GameObject gameObject2 = ABResources.LoadAsset<GameObject>("Handbook/" + slot.HandbookCfg.id);
				if (!(gameObject2 == null))
				{
					videoTextCtrl = UnityEngine.Object.Instantiate(gameObject2, go_VideoText.transform).GetComponent<UIHandbookVideoTextCtrl>();
					videoTextCtrl.BindVideoPlayer(imageVideoPlayer);
				}
			}
			break;
		}
		case HandbookDemoType.Texture:
		{
			for (int i = 0; i < go_Texture.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(go_Texture.transform.GetChild(i).gameObject);
			}
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Handbook/" + slot.HandbookCfg.id), go_Texture.transform);
			break;
		}
		default:
			Debug.LogError(slot.HandbookCfg.demoType);
			break;
		case HandbookDemoType.None:
			break;
		}
	}

	public void SlotExit()
	{
		text_Title.text = "";
		text_Desc.text = "";
		go_Demo.SetActive(value: false);
		go_Texture.SetActive(value: false);
	}

	private void OnInputChange()
	{
		if (!init)
		{
			return;
		}
		if (GameMgr.IsMobile_Static)
		{
			HideKeyShortCut();
			return;
		}
		switch (ControlMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			ShowKeyShortCut();
			break;
		case PlayerInputType.Gamepad:
			HideKeyShortCut();
			break;
		}
		UIMgr.Inst.UIMenu.recthandbook.ScrollCountActive();
		void HideKeyShortCut()
		{
			for (int j = 0; j < Slots.Count; j++)
			{
				if (Slots[j].HandbookCfg.belongCategory == HandbookBelongCategory.ConvenientOpearation)
				{
					Slots[j].gameObject.SetActive(value: false);
				}
				Slots[j].go_HoverImage.SetActive(value: false);
			}
			UIMgr.Inst.UIMenu.recthandbook.ScrollUpdate(Selectindex, slideDirection: false);
		}
		void ShowKeyShortCut()
		{
			for (int i = 0; i < Slots.Count; i++)
			{
				if (Slots[i].HandbookCfg.belongCategory == HandbookBelongCategory.ConvenientOpearation)
				{
					Slots[i].gameObject.SetActive(value: true);
				}
				Slots[i].go_HoverImage.SetActive(value: false);
			}
			Selectindex = 0;
			UIMgr.Inst.UIMenu.recthandbook.ScrollUpdate(Selectindex, slideDirection: false);
		}
	}

	public void ShowAndSlideToCenter(int id)
	{
		UIHandbookSlot uIHandbookSlot = Slots.FirstOrDefault((UIHandbookSlot x) => x.HandbookCfg != null && x.HandbookCfg.id == id);
		if (uIHandbookSlot != null)
		{
			GeneralTool.ScrollToPadSelected(rtsf_Content.GetComponentInParent<ScrollRect>(), rtsf_Content.GetComponent<RectTransform>(), uIHandbookSlot.GetComponent<RectTransform>());
			uIHandbookSlot.OnPointerClick(null);
		}
	}

	protected override void OnHide()
	{
	}

	protected override void RegistarWhenInit()
	{
	}

	protected override void RegistarOnlyWhenOpen()
	{
	}

	protected override void UnRegistarOnlyWhenHide()
	{
	}

	protected override void UnRegistarWhenDestroy()
	{
	}

	public void ClearMp4GameObject()
	{
		if (videoTextCtrl != null)
		{
			UnityEngine.Object.Destroy(videoTextCtrl.gameObject);
		}
		if (imageVideoPlayer != null)
		{
			UnityEngine.Object.Destroy(imageVideoPlayer.gameObject);
		}
	}
}
