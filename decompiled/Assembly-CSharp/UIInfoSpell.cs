using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoSpell : MonoBehaviour
{
	public CanvasGroup canvasGroup;

	public RectTransform rtsf_Self;

	public RectTransform rtsf_MP;

	public Image image_Icon;

	public Image SpellIconBackground;

	public Text text_Name;

	public Text text_Type;

	public Text text_Rarity;

	public Text text_MP;

	public Text text_Info;

	public Text text_Des;

	public Text text_greyText;

	public Sprite spriteCommonImageFrame;

	public Sprite spriteRareBGImageFrame;

	public Sprite spriteEpicBGImageFrame;

	public Sprite spriteSpecialBGImageFrame;

	private float textBGWidthExtra = 8f;

	public float typeAndRarityInterval = 10f;

	public RectTransform rtsfInfoAndDes;

	public VerticalLayoutGroup verticalLayoutGroup;

	public GameObject greySpace;

	public GameObject line;

	public RectTransform textTypeBG;

	public RectTransform textRarityBG;

	public float paddingDown;

	public float textInfoDefaultYPos;

	public GameObject gameobject_ControlKeyShow;

	public UpdatButtonShow[] UpdatButtonShows;

	public Image imageFrame;

	public Sprite spriteCommonFrame;

	public Sprite spriteRareFrame;

	public Sprite spriteEpicFrame;

	public Sprite spriteSpecialFrame;

	public Sprite spritePostSlotFrame;

	public GameObject panel_GamepadTip_SpellInfo;

	public Text textGamepadDrag;

	public Text textGamepadThrow;

	public UISlotWandTips uiSlotWandTips;

	public RectTransform rtsfSpellCost;

	public float hightSubIfPostSlot = 100f;

	public float spellCostPositionOffset;

	private float _hightSub;

	public void OnEnable()
	{
		ControlChange();
		EventMgr.ControlChange = (Action)Delegate.Combine(EventMgr.ControlChange, new Action(ControlChange));
	}

	public void OnDisable()
	{
		EventMgr.ControlChange = (Action)Delegate.Remove(EventMgr.ControlChange, new Action(ControlChange));
	}

	public void ControlChange()
	{
		UpdatButtonShow[] updatButtonShows = UpdatButtonShows;
		foreach (UpdatButtonShow updatButtonShow in updatButtonShows)
		{
			if (updatButtonShow != null)
			{
				updatButtonShow.UpdateButton();
			}
		}
	}

	public void UpdateInfo(UISlotWand slot, SlotData spellData, RectTransform rtsf_Curse = null, bool changeAlpha = true)
	{
		StartCoroutine(UpdateInfoIE(slot, spellData, rtsf_Curse, changeAlpha));
	}

	public void UpdateInfoExternal(UISlotWandExternal slot, SlotData spellData, RectTransform rtsf_Curse = null, bool changeAlpha = true)
	{
		StartCoroutine(UpdateInfoIE(null, spellData, rtsf_Curse, changeAlpha, isFromExternal: true, slot));
	}

	public void UpdateInfo(int SpellID, RectTransform rtsf_Curse = null, bool changeAlpha = true)
	{
		try
		{
			StartCoroutine(UpdateInfoIE(null, new SlotData(SpellID), rtsf_Curse, changeAlpha));
		}
		catch
		{
			Debug.LogError("报错了");
		}
	}

	public void UpdateInfo(SlotData spellData, RectTransform rtsf_Curse = null, bool changeAlpha = true)
	{
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(UpdateInfoIE(null, spellData, rtsf_Curse, changeAlpha));
		}
	}

	private IEnumerator UpdateInfoIE(UISlotWand uiWand, SlotData slotData, RectTransform rtsf_Curse = null, bool changeAlpha = true, bool isFromExternal = false, UISlotWandExternal slotWandExt = null)
	{
		textTypeBG.gameObject.SetActive(value: false);
		textRarityBG.gameObject.SetActive(value: false);
		UpdateFrame(slotData);
		ClearAll();
		if ((bool)canvasGroup && changeAlpha)
		{
			canvasGroup.alpha = 0f;
		}
		if (gameobject_ControlKeyShow != null)
		{
			if ((bool)UIBattleMgr.Inst && GameUISingletonMono<UILevelReward>.StaticIsOpen)
			{
				gameobject_ControlKeyShow.SetActive(value: false);
			}
			gameobject_ControlKeyShow.SetActive(UIMgr.Inst.InputType != 0 && slotData != null && !UIMgr.Inst.IsAnyBuildPanelOpen());
		}
		if ((object)uiWand == null && slotData == null)
		{
			Debug.LogError("同时丢失法杖和法术信息不能更新法术 UI");
			yield break;
		}
		SpellConfig spellConfig = null;
		if (slotWandExt != null)
		{
			if (slotData != null)
			{
				if (slotData.id == 0 || !SpellConfig.dic.ContainsKey(slotData.id))
				{
					Debug.LogWarning($"无效的法术id:{slotData.id}");
					yield break;
				}
				textTypeBG.gameObject.SetActive(value: true);
				textRarityBG.gameObject.SetActive(value: true);
				SpellIconBackground.enabled = true;
				image_Icon.enabled = true;
				spellConfig = SpellConfig.dic[slotData.id];
				switch (spellConfig.useType)
				{
				case SpellType.Missile:
				case SpellType.Summon:
					AppendTextToInfo(slotData.GetConfigIgnoreMimic().GetInfo(1f, "◆\u00a0\u200a"));
					AppendTextToDescription(slotData.GetConfigIgnoreMimic().GetDes(1f, "◆\u00a0\u200a", GameConst.colorSpellDes, "◆\u00a0\u200a"));
					AppendTextToDescription(slotData.GetAdditionalInfo(null), 2);
					break;
				case SpellType.Enhance:
				case SpellType.Passive:
					AppendTextToInfo(slotData.GetConfigIgnoreMimic().GetInfo(1f, "◆\u00a0\u200a"));
					AppendTextToInfo(slotData.GetConfigIgnoreMimic().GetDes(1f, "◆\u00a0\u200a", "", "◆\u00a0\u200a"));
					AppendTextToInfo(slotData.GetAdditionalInfo(null), 2);
					break;
				}
				rtsf_MP.gameObject.SetActive(spellConfig.mpCost > 0);
				if (spellConfig != null)
				{
					SetBase(spellConfig);
					if (spellConfig.mpCost > 0)
					{
						rtsf_MP.gameObject.SetActive(value: true);
						text_MP.text = spellConfig.mpCost.ToString();
						if (spellConfig.abilityType == SpellAbilityType.DragonBreath)
						{
							text_MP.text = (spellConfig.duration * spellConfig.float2).ToString();
						}
					}
				}
			}
			else
			{
				SpellIconBackground.enabled = false;
				image_Icon.enabled = false;
				image_Icon.sprite = null;
				text_Name.text = "";
				text_Type.text = "";
				text_Rarity.text = "";
			}
		}
		else if (slotData == null)
		{
			AppendTextToGreyText(GetSlotTypeInfo(uiWand));
			rtsf_MP.gameObject.SetActive(value: false);
		}
		else
		{
			textTypeBG.gameObject.SetActive(value: true);
			textRarityBG.gameObject.SetActive(value: true);
			spellConfig = slotData.GetConfigIgnoreMimic();
			SetBase(spellConfig);
			rtsf_MP.gameObject.SetActive(spellConfig.mpCost > 0);
			if ((object)uiWand != null && uiWand.build == null)
			{
				Wand wand = PlayerMgr.Inst.Wands[uiWand.WandIndex];
				switch (spellConfig.useType)
				{
				case SpellType.Missile:
				case SpellType.Summon:
					AppendTextToInfo(slotData.GetInfoInPlayerWand(wand));
					AppendTextToDescription(slotData.GetDesInPlayerWand(wand, "◆\u00a0\u200a", GameConst.colorSpellDes, "◆\u00a0\u200a"));
					break;
				case SpellType.Enhance:
				case SpellType.Passive:
					AppendTextToInfo(slotData.GetInfoInPlayerWand(wand));
					AppendTextToInfo(slotData.GetDesInPlayerWand(wand, "◆\u00a0\u200a", "", "◆\u00a0\u200a"));
					break;
				}
				string text = string.Join("\n", GetSlotTypeInfo(uiWand).Trim(), slotData.GetAdditionalInfo(wand).Trim());
				if (text.Length > 0)
				{
					AppendTextToGreyText(text, 2);
				}
				if (spellConfig.mpCost > 0)
				{
					SpellShootData shootDataByShootableSpell = wand.GetShootDataByShootableSpell(slotData);
					RatioValue spellManaCost_FinalPlayerValue = shootDataByShootableSpell.GetSpellManaCost_FinalPlayerValue(wand);
					text_MP.text = TextProcesser.GetColorText(GeneralTool.FloatToRetainDecimals(spellManaCost_FinalPlayerValue.Result, 1), SpellInfoExtend.ColorByValue_LowGood(spellConfig.mpCost, spellManaCost_FinalPlayerValue.Result));
					if (spellConfig.abilityType == SpellAbilityType.DragonBreath)
					{
						RatioValue spellDuration_FinalPlayerValue = shootDataByShootableSpell.GetSpellDuration_FinalPlayerValue(wand);
						text_MP.text = TextProcesser.GetColorText(GeneralTool.FloatToRetainDecimals(spellManaCost_FinalPlayerValue.CurrentFinalRatio * spellConfig.float2 * spellDuration_FinalPlayerValue.Result, 1), SpellInfoExtend.ColorByValue_LowGood(1f, spellManaCost_FinalPlayerValue.CurrentFinalRatio));
					}
				}
			}
			else
			{
				text_MP.text = GeneralTool.FloatToRetainDecimals(spellConfig.mpCost, 1);
				if (spellConfig.abilityType == SpellAbilityType.DragonBreath)
				{
					text_MP.text = GeneralTool.FloatToRetainDecimals(spellConfig.duration * spellConfig.float2, 1);
				}
				switch (spellConfig.useType)
				{
				case SpellType.Missile:
				case SpellType.Summon:
					AppendTextToInfo(slotData.GetConfigIgnoreMimic().GetInfo(1f, "◆\u00a0\u200a"));
					AppendTextToDescription(slotData.GetConfigIgnoreMimic().GetDes(1f, "◆\u00a0\u200a", GameConst.colorSpellDes, "◆\u00a0\u200a"));
					AppendTextToGreyText(slotData.GetAdditionalInfo(null), 2);
					break;
				case SpellType.Enhance:
				case SpellType.Passive:
					AppendTextToInfo(slotData.GetConfigIgnoreMimic().GetInfo(1f, "◆\u00a0\u200a"));
					AppendTextToInfo(slotData.GetConfigIgnoreMimic().GetDes(1f, "◆\u00a0\u200a", "", "◆\u00a0\u200a"));
					AppendTextToGreyText(slotData.GetAdditionalInfo(null), 2);
					break;
				}
			}
		}
		text_Info.text = GeneralTool.FormatTextIfPublishTest(text_Info, text_Info.text);
		text_Des.text = GeneralTool.FormatTextIfPublishTest(text_Des, text_Des.text);
		if (spellConfig != null)
		{
			switch (spellConfig.useType)
			{
			case SpellType.Missile:
			case SpellType.Summon:
				if (text_Info.text != "" && (!(text_Des.text == "") || !(text_greyText.text == "")))
				{
					line.SetActive(value: true);
				}
				break;
			case SpellType.Enhance:
			case SpellType.Passive:
				line.SetActive(value: false);
				break;
			}
		}
		if ((bool)canvasGroup && changeAlpha)
		{
			canvasGroup.alpha = 1f;
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)verticalLayoutGroup.transform);
		yield return StartCoroutine(UpdateSize(rtsf_Curse));
	}

	private void UpdateFrame(SlotData slotData)
	{
		rtsfInfoAndDes.anchoredPosition = new Vector2(rtsfInfoAndDes.anchoredPosition.x, rtsfInfoAndDes.anchoredPosition.y - _hightSub);
		_hightSub = 0f;
		if (slotData != null && slotData.id != 0)
		{
			switch (SpellConfig.dic[slotData.id].dropType)
			{
			case ItemDropType.None:
				Debug.LogError("错误的稀有度");
				imageFrame.sprite = spriteCommonFrame;
				SpellIconBackground.sprite = spriteCommonImageFrame;
				break;
			case ItemDropType.Common:
				imageFrame.sprite = spriteCommonFrame;
				SpellIconBackground.sprite = spriteCommonImageFrame;
				break;
			case ItemDropType.Rare:
				imageFrame.sprite = spriteRareFrame;
				SpellIconBackground.sprite = spriteRareBGImageFrame;
				break;
			case ItemDropType.Epic:
				imageFrame.sprite = spriteEpicFrame;
				SpellIconBackground.sprite = spriteEpicBGImageFrame;
				break;
			case ItemDropType.Special:
				imageFrame.sprite = spriteSpecialFrame;
				SpellIconBackground.sprite = spriteSpecialBGImageFrame;
				break;
			default:
				Debug.LogError("错误的稀有度");
				imageFrame.sprite = spriteCommonFrame;
				break;
			}
		}
		else
		{
			imageFrame.sprite = spritePostSlotFrame;
			_hightSub = hightSubIfPostSlot;
		}
		rtsfInfoAndDes.anchoredPosition = new Vector2(rtsfInfoAndDes.anchoredPosition.x, rtsfInfoAndDes.anchoredPosition.y + _hightSub);
	}

	public string GetSlotTypeInfo(UISlotWand targetSlot)
	{
		if (targetSlot.build == null)
		{
			return GetSlotTypeInfo(targetSlot.SlotType, PlayerMgr.Inst.Wands[targetSlot.WandIndex].WandCfg);
		}
		return GetSlotTypeInfo(targetSlot.SlotType, targetSlot.build.wandCfgs[targetSlot.WandIndex]);
	}

	private string GetSlotTypeInfo(WandSlotType wandSlotType, WandConfig wandConfig)
	{
		if (wandSlotType != WandSlotType.Post)
		{
			return "";
		}
		return new StringBuilder().Append(TextProcesser.GetColorText(wandConfig.GetPostSlotDesc(), DataTextColorType.Grey)).ToString();
	}

	public void SetManaLackAlertInfo(bool isAllSpellGroupsInvalid = true)
	{
		rtsf_MP.gameObject.SetActive(value: false);
		SpellIconBackground.enabled = false;
		image_Icon.enabled = false;
		image_Icon.sprite = null;
		text_Name.text = "";
		text_Type.text = "";
		text_Rarity.text = "";
		ClearTextInfo();
		AppendTextToInfo(isAllSpellGroupsInvalid ? 1000703.GetText() : 1000705.GetText());
		float num = 0f;
		num += text_Info.rectTransform.sizeDelta.y + paddingDown * 2f;
		text_Info.rectTransform.anchoredPosition = new Vector3(text_Info.rectTransform.anchoredPosition.x, 0f - paddingDown, 0f);
		rtsf_Self.sizeDelta = new Vector2(rtsf_Self.sizeDelta.x, num);
	}

	private void ClearAll()
	{
		SpellIconBackground.enabled = false;
		image_Icon.enabled = false;
		image_Icon.sprite = null;
		text_Name.text = "";
		text_Type.text = "";
		text_Rarity.text = "";
		text_Des.text = "";
		text_greyText.text = "";
		text_Info.gameObject.SetActive(value: true);
		text_Des.gameObject.SetActive(value: true);
		greySpace.gameObject.SetActive(value: true);
		text_greyText.gameObject.SetActive(value: true);
		line.gameObject.SetActive(value: false);
		ClearTextInfo();
	}

	private void SetBase(SpellConfig cfg)
	{
		SpellIconBackground.enabled = true;
		image_Icon.enabled = true;
		image_Icon.sprite = ABResources.LoadAsset<Sprite>(cfg.GetIconPath());
		text_Name.text = cfg.GetName();
		text_Type.text = cfg.GetStrType();
		text_Rarity.text = cfg.GetStrRarity();
		text_Rarity.color = GeneralTool.GetRarityColor(cfg.dropType);
		text_Type.color = GetColorBySpellType(cfg.useType);
	}

	private IEnumerator UpdateSize(RectTransform rtsf_Curse)
	{
		if (text_Info.text == "")
		{
			text_Info.gameObject.SetActive(value: false);
		}
		if (text_Des.text == "")
		{
			text_Des.gameObject.SetActive(value: false);
		}
		if (text_greyText.text == "")
		{
			text_greyText.gameObject.SetActive(value: false);
			greySpace.gameObject.SetActive(value: false);
		}
		yield return null;
		textTypeBG.sizeDelta = new Vector2(text_Type.preferredWidth + textBGWidthExtra, textTypeBG.rect.height);
		textRarityBG.sizeDelta = new Vector2(text_Rarity.preferredWidth + textBGWidthExtra, textRarityBG.rect.height);
		text_Rarity.GetComponent<RectTransform>().anchoredPosition = new Vector2(textTypeBG.anchoredPosition.x + textTypeBG.sizeDelta.x + typeAndRarityInterval, text_Rarity.GetComponent<RectTransform>().anchoredPosition.y);
		textRarityBG.anchoredPosition = new Vector2(textTypeBG.anchoredPosition.x + textTypeBG.sizeDelta.x + typeAndRarityInterval - textBGWidthExtra / 2f, textRarityBG.anchoredPosition.y);
		text_MP.GetComponent<RectTransform>().sizeDelta = new Vector2(text_MP.GetComponent<RectTransform>().sizeDelta.x, text_MP.GetComponent<RectTransform>().sizeDelta.y);
		rtsfSpellCost.anchoredPosition = new Vector2(spellCostPositionOffset - text_MP.preferredWidth, rtsfSpellCost.anchoredPosition.y);
		float num = 0f;
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)verticalLayoutGroup.transform);
		num -= ((RectTransform)verticalLayoutGroup.transform).anchoredPosition.y;
		num += ((RectTransform)verticalLayoutGroup.transform).sizeDelta.y;
		num += paddingDown;
		rtsf_Self.sizeDelta = new Vector2(rtsf_Self.sizeDelta.x, num);
		if (rtsf_Curse != null)
		{
			rtsf_Curse.anchoredPosition = rtsf_Self.anchoredPosition + new Vector2(0f, rtsf_Self.sizeDelta.y);
		}
	}

	private void ClearTextInfo()
	{
		text_Info.text = "";
	}

	private void AppendTextToInfo(string text, int newLineCount = 1)
	{
		text = text.Trim();
		if (text.Length == 0)
		{
			return;
		}
		if (text_Info.text.Length > 0)
		{
			for (int i = 0; i < newLineCount; i++)
			{
				text_Info.text += "\n";
			}
		}
		text_Info.text += text;
	}

	private void AppendTextToDescription(string text, int newLineCount = 1)
	{
		text = text.Trim();
		if (text.Length == 0)
		{
			return;
		}
		if (text_Des.text.Length > 0)
		{
			for (int i = 0; i < newLineCount; i++)
			{
				text_Des.text += "\n";
			}
		}
		text_Des.text += text;
	}

	private void AppendTextToGreyText(string text, int newLineCount = 1)
	{
		text = text.Trim();
		if (text.Length == 0)
		{
			return;
		}
		if (text_greyText.text.Length > 0)
		{
			for (int i = 0; i < newLineCount; i++)
			{
				text_greyText.text += "\n";
			}
		}
		text_greyText.text += text;
	}

	private Color GetColorBySpellType(SpellType type)
	{
		switch (type)
		{
		case SpellType.Missile:
			return GameConst.color_SpellUseTypeMissle;
		case SpellType.Summon:
			return GameConst.color_SpellUseTypeMissle;
		case SpellType.Enhance:
			return GameConst.color_SpellUseTypeEnhance;
		case SpellType.Passive:
			return GameConst.color_SpellUseTypePassive;
		default:
			Debug.LogError("未知的法术类型：" + type);
			return GameConst.color_SpellUseTypeMissle;
		}
	}
}
