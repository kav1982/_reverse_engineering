using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoRelic : MonoBehaviour
{
	public RectTransform rtsf_Self;

	public Image image_Icon;

	public Text text_Name;

	public Text text_Type;

	public Text text_Rarity;

	public Text text_Info;

	public Text text_GreyText;

	public float greyTextInterval;

	public float paddingDown;

	public float typeAndRarityInterval = 10f;

	public Image imageFrame;

	public Sprite spriteCommonFrame;

	public Sprite spriteRareFrame;

	public Sprite spriteEpicFrame;

	public Sprite spriteSpecialFrame;

	public Image imageIconBg;

	public Sprite spriteCommonIconBG;

	public Sprite spriteRareIconBG;

	public Sprite spriteEPicIconBG;

	public Sprite spriteSpecialIconBG;

	public float textBGWidthExtra = 8f;

	public RectTransform textTypeBG;

	public RectTransform textRariatyBG;

	public RectTransform rtsfTextInfo;

	public RectTransform rtsfGeryText;

	public Button hideRelicButton;

	public Text buttonText;

	public RectTransform LostCastleRuneInfoBG;

	public GameObject RedRune;

	public GameObject GreenRune;

	public GameObject BlueRune;

	public Text RedRuneText;

	public Text GreenRuneText;

	public Text BlueRuneText;

	public void UpdateInfo(RelicConfig relicCfg, RectTransform rtsf_Curse = null, bool upgrade = false, bool showHideSkinTip = false, bool showRelicGroupInfo = false)
	{
		if (hideRelicButton != null)
		{
			hideRelicButton.gameObject.SetActive(value: false);
		}
		StartCoroutine(UpdateInfoIE(relicCfg, rtsf_Curse, upgrade, showHideSkinTip, showRelicGroupInfo));
	}

	private IEnumerator UpdateInfoIE(RelicConfig relicCfg, RectTransform rtsf_Curse = null, bool upgrade = false, bool showHideSkinTip = false, bool showRelicGroupInfo = false)
	{
		UpdateFrame(relicCfg);
		if ((bool)GetComponent<CanvasGroup>())
		{
			GetComponent<CanvasGroup>().alpha = 0f;
		}
		image_Icon.sprite = ABResources.LoadAsset<Sprite>(relicCfg.GetIconPath());
		text_Name.text = relicCfg.GetName();
		text_Type.text = 1002202.GetText();
		text_Rarity.text = relicCfg.GetStrRarity();
		text_Rarity.color = GeneralTool.GetRarityColor(relicCfg.dropType);
		text_Info.text = GeneralTool.FormatTextIfPublishTest(text_Info, relicCfg.GetInfo(includeExtraInfo: false, upgrade));
		text_GreyText.text = relicCfg.GetExtraInfo();
		if (showRelicGroupInfo && PlayerMgr.Inst.ItemCtrller.relicGroupConfigs.TryGetValue(RelicGroupConfig.GetRelicGroupIdByRelicId(relicCfg.id) ?? (-1), out var value))
		{
			string text = "\n<b>" + GameConst.colorRelicGroupDesc;
			if (GameMgr.IsMobile_Static)
			{
				text = "\n" + GameConst.colorRelicGroupDesc;
			}
			text = text + "\n◆\u00a0\u200a" + value.GetName() + "：";
			int[] items = value.items;
			foreach (int key in items)
			{
				text = text + "\n    ▸\u00a0\u200a" + RelicConfig.dic[key].GetName();
			}
			text = text + "\n◆\u00a0\u200a" + value.GetDesc();
			text = ((!GameMgr.IsMobile_Static) ? (text + "</color></b>") : (text + "</color>"));
			text_Info.text += text;
		}
		if (showHideSkinTip)
		{
			string text2 = "";
			if (DataMgr.settingData.DisableRelicSkins.Contains(relicCfg.id))
			{
				text2 = 1001511.GetText();
			}
			else
			{
				int i = relicCfg.id;
				if (i == 50 || i == 61 || i == 80)
				{
					text2 = 1001510.GetText();
				}
			}
			if (!string.IsNullOrEmpty(text2))
			{
				string text3 = GameConst.htmlColor_InfoGrey + "◆\u00a0\u200a" + text2 + "</color>";
				Text text4 = text_GreyText;
				text4.text = text4.text + "\n" + text3;
				text_GreyText.text = text_GreyText.text.Trim();
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.uiRelic_RuneWizard != null)
		{
			RedRune.SetActive(relicCfg.RedRunePoint > 0);
			GreenRune.SetActive(relicCfg.GreenRunePoint > 0);
			BlueRune.SetActive(relicCfg.BlueRunePoint > 0);
			RedRuneText.text = "<b>" + 7040251.GetText() + "+" + relicCfg.level * relicCfg.RedRunePoint + "</b>";
			GreenRuneText.text = "<b>" + 7040261.GetText() + "+" + relicCfg.level * relicCfg.GreenRunePoint + "</b>";
			BlueRuneText.text = "<b>" + 7040271.GetText() + "+" + relicCfg.level * relicCfg.BlueRunePoint + "</b>";
		}
		else
		{
			RedRune.SetActive(value: false);
			GreenRune.SetActive(value: false);
			BlueRune.SetActive(value: false);
		}
		yield return null;
		textTypeBG.sizeDelta = new Vector2(text_Type.preferredWidth + textBGWidthExtra, textTypeBG.rect.height);
		textRariatyBG.sizeDelta = new Vector2(text_Rarity.preferredWidth + textBGWidthExtra, textRariatyBG.rect.height);
		text_Rarity.GetComponent<RectTransform>().anchoredPosition = new Vector2(textTypeBG.anchoredPosition.x + textTypeBG.sizeDelta.x + typeAndRarityInterval, text_Rarity.GetComponent<RectTransform>().anchoredPosition.y);
		textRariatyBG.anchoredPosition = new Vector2(textTypeBG.anchoredPosition.x + textTypeBG.sizeDelta.x + typeAndRarityInterval - textBGWidthExtra / 2f, textRariatyBG.anchoredPosition.y);
		int num = 0;
		if (LostCastleRuneInfoBG.rect.height > 0f)
		{
			num = 15;
		}
		LostCastleRuneInfoBG.anchoredPosition = new Vector2(rtsfTextInfo.anchoredPosition.x, rtsfTextInfo.anchoredPosition.y - text_Info.rectTransform.sizeDelta.y - (float)num);
		rtsfGeryText.anchoredPosition = new Vector2(rtsfTextInfo.anchoredPosition.x, LostCastleRuneInfoBG.anchoredPosition.y - LostCastleRuneInfoBG.sizeDelta.y - greyTextInterval);
		if (string.IsNullOrEmpty(text_GreyText.text))
		{
			rtsf_Self.sizeDelta = new Vector2(rtsf_Self.sizeDelta.x, 0f - text_Info.rectTransform.anchoredPosition.y + text_Info.rectTransform.sizeDelta.y + LostCastleRuneInfoBG.sizeDelta.y + (float)num + paddingDown);
		}
		else
		{
			rtsf_Self.sizeDelta = new Vector2(rtsf_Self.sizeDelta.x, 0f - rtsfGeryText.anchoredPosition.y + rtsfGeryText.sizeDelta.y + paddingDown);
		}
		if (rtsf_Curse != null)
		{
			rtsf_Curse.anchoredPosition = rtsf_Self.anchoredPosition + new Vector2(0f, rtsf_Self.sizeDelta.y * rtsf_Self.transform.localScale.y);
		}
		if ((bool)GetComponent<CanvasGroup>())
		{
			GetComponent<CanvasGroup>().alpha = 1f;
		}
	}

	private void UpdateFrame(RelicConfig relicCfg)
	{
		if (RelicConfig.dic.TryGetValue(relicCfg.id, out var value))
		{
			switch (value.dropType)
			{
			case ItemDropType.None:
				imageFrame.sprite = spriteCommonFrame;
				imageIconBg.sprite = spriteCommonIconBG;
				break;
			case ItemDropType.Common:
				imageFrame.sprite = spriteCommonFrame;
				imageIconBg.sprite = spriteCommonIconBG;
				break;
			case ItemDropType.Rare:
				imageFrame.sprite = spriteRareFrame;
				imageIconBg.sprite = spriteRareIconBG;
				break;
			case ItemDropType.Epic:
				imageFrame.sprite = spriteEpicFrame;
				imageIconBg.sprite = spriteEPicIconBG;
				break;
			case ItemDropType.Special:
				imageFrame.sprite = spriteSpecialFrame;
				imageIconBg.sprite = spriteSpecialIconBG;
				break;
			default:
				Debug.LogError("错误的稀有度");
				imageFrame.sprite = spriteCommonFrame;
				break;
			}
		}
	}

	public void MobileShowRelicHideButton(Action action)
	{
		if (hideRelicButton != null)
		{
			hideRelicButton.onClick.RemoveAllListeners();
			hideRelicButton.onClick.AddListener(delegate
			{
				action();
			});
			hideRelicButton.gameObject.SetActive(value: true);
		}
	}
}
