using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoCurse : MonoBehaviour
{
	public CanvasGroup canvasGroup;

	public RectTransform rtsf_Self;

	public Image image_Icon;

	public Text text_Name;

	public Text text_Type;

	public Text text_Rarity;

	public Text text_Info;

	public float paddingDown;

	public float textBGWidthExtra = 8f;

	public float typeAndRarityInterval = 10f;

	public RectTransform textTypeBG;

	public RectTransform textRariatyBG;

	public void UpdateInfo(int id, bool isPlayerHad = false, int overrideLevel = 0)
	{
		StartCoroutine(UpdateInfoIE(id, isPlayerHad, overrideLevel));
	}

	private IEnumerator UpdateInfoIE(int id, bool isPlayerHad = false, int overrideLevel = 0)
	{
		CurseConfig config = CurseConfig.GetConfig(id);
		if (overrideLevel != 0)
		{
			config.level = overrideLevel;
		}
		else if (isPlayerHad)
		{
			config.level = PlayerMgr.Inst.BaData.curseLevels[PlayerMgr.Inst.BaData.curseIDs.IndexOf(id)];
		}
		else if (PlayerMgr.Inst.BaData.curseIDs.Contains(id))
		{
			config.level = PlayerMgr.Inst.BaData.curseLevels[PlayerMgr.Inst.BaData.curseIDs.IndexOf(id)] + 1;
		}
		image_Icon.sprite = ABResources.LoadAsset<Sprite>(config.GetIconPath());
		text_Name.text = config.GetName();
		text_Type.text = 1002210.GetText();
		text_Rarity.text = config.GetStrRarity();
		text_Rarity.color = GeneralTool.GetRarityColor(config.dropType);
		text_Info.text = config.GetInfo(includeExtraInfo: true);
		yield return null;
		textTypeBG.sizeDelta = new Vector2(text_Type.preferredWidth + textBGWidthExtra, textTypeBG.rect.height);
		textRariatyBG.sizeDelta = new Vector2(text_Rarity.preferredWidth + textBGWidthExtra, textRariatyBG.rect.height);
		text_Rarity.GetComponent<RectTransform>().anchoredPosition = new Vector2(textTypeBG.anchoredPosition.x + textTypeBG.sizeDelta.x + typeAndRarityInterval, text_Rarity.GetComponent<RectTransform>().anchoredPosition.y);
		textRariatyBG.anchoredPosition = new Vector2(textTypeBG.anchoredPosition.x + textTypeBG.sizeDelta.x + typeAndRarityInterval - textBGWidthExtra / 2f, textRariatyBG.anchoredPosition.y);
		rtsf_Self.sizeDelta = new Vector2(rtsf_Self.sizeDelta.x, 0f - text_Info.rectTransform.anchoredPosition.y + text_Info.rectTransform.sizeDelta.y + paddingDown);
	}
}
