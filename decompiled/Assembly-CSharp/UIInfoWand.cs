using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoWand : MonoBehaviour
{
	public CanvasGroup canvasGroup;

	public UISlotWandExternal pfb_UISlotWandExternal;

	public GameObject pfb_UISpecialSlotBlocker;

	public RectTransform rtsf_Self;

	public RectTransform rtsf_Spells;

	public Image wandIcon;

	public Text text_Type;

	public Text text_Name;

	public Text text_MP;

	public Text text_MPRecover;

	public Text text_ShootInterval;

	public Text text_CoolDown;

	public RectTransform rtsf_Separator;

	public Text text_Info;

	public UILayout uiLayout;

	public Vector2 padding;

	public float space;

	private float wandInfoWidthMaxTextCoolDown = 550f;

	private float initialTextWidth;

	private RectTransform rtsf_TextCoolDown;

	private RectTransform rtsf_text_ShootInterval;

	public float subWidthMaxPC;

	public float subWidthMaxMobile;

	public float spellsAnchoredHightSub;

	public float infoBlockMinWidth;

	public float dataBlockerInvisibleWidth;

	public float borderWidth;

	public UpdatButtonShow[] UpdatButtonShows;

	private float originalSelfWidth;

	private string color_Higher = "<color=#05FF00>";

	private string color_Lower = "<color=#CB000F>";

	public float textBGWidthExtra = 8f;

	public RectTransform textTypeBG;

	public GameObject panel_GamepadTip_WandInfo;

	public Text textGamepadDrag;

	public Text textGamepadThrow;

	[Header("\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd")]
	public Canvas canvasRaise;

	public GraphicRaycaster raycaster;

	public float subWidthMax
	{
		get
		{
			if (GameMgr.IsMobile_Static)
			{
				return subWidthMaxPC;
			}
			return subWidthMaxMobile;
		}
	}

	public void Awake()
	{
		rtsf_TextCoolDown = text_CoolDown.GetComponent<RectTransform>();
		rtsf_text_ShootInterval = text_ShootInterval.GetComponent<RectTransform>();
		initialTextWidth = rtsf_TextCoolDown.sizeDelta.x;
	}

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

	private void Start()
	{
		originalSelfWidth = rtsf_Self.sizeDelta.x;
	}

	public void UpdateInfo(Wand wand, RectTransform rtsf_Curse = null, bool ChangeAlpha = true)
	{
		StartCoroutine(UpdateInfoIE(wand, wand.WandCfg, rtsf_Curse, raiseSpellLayer: false, ChangeAlpha));
	}

	public void UpdateInfo(WandConfig wandCfg, RectTransform rtsf_Curse = null, bool ItemIsStore = false, bool ChangeAlpha = true)
	{
		if (base.gameObject.activeSelf && base.enabled)
		{
			StartCoroutine(UpdateInfoIE(null, wandCfg, rtsf_Curse, ItemIsStore, ChangeAlpha));
		}
		else
		{
			Debug.LogWarning("\ufffd\ufffd\ufffd\ufffd\udabe\udeb5\ufffdû\ufffd\ufffd");
		}
	}

	private IEnumerator UpdateInfoIE(Wand wand, WandConfig wandCfg, RectTransform rtsf_Curse = null, bool raiseSpellLayer = false, bool ChangeAlpha = true)
	{
		if (canvasGroup != null && ChangeAlpha)
		{
			canvasGroup.alpha = 0f;
		}
		text_Type.text = 1002204.GetText(forceApplyAlogia: true);
		text_Name.text = wandCfg.GetName();
		wandIcon.sprite = ABResources.LoadAsset<Sprite>(wandCfg.GetIconPath());
		float num = ((!(wand == null)) ? wand.MaxMP : ((float)wandCfg.maxMP + wandCfg.GetExtraMaxMP()));
		if (num < 0f)
		{
			num = 0f;
		}
		string text = num.ToString("F0");
		text_MP.text = 14000202.GetText(forceApplyAlogia: true) + ": ";
		if (num > (float)wandCfg.maxMP)
		{
			Text text2 = text_MP;
			text2.text = text2.text + color_Higher + text + "</Color>";
		}
		else if (num < (float)wandCfg.maxMP)
		{
			Text text3 = text_MP;
			text3.text = text3.text + color_Lower + text + "</Color>";
		}
		else
		{
			text_MP.text += text;
		}
		float num2 = ((!(wand == null)) ? wand.GetWandMpRecoverSpeed() : ((float)wandCfg.mpRecovery + wandCfg.GetExtraMPRecovery()));
		if (num2 < 0f)
		{
			num2 = 0f;
		}
		string text4 = GeneralTool.FloatToRetainDecimals(num2, 1);
		text_MPRecover.text = 14000203.GetText(forceApplyAlogia: true) + ": ";
		if (num2 > (float)wandCfg.mpRecovery)
		{
			Text text5 = text_MPRecover;
			text5.text = text5.text + color_Higher + text4 + "</Color>";
		}
		else if (num2 < (float)wandCfg.mpRecovery)
		{
			Text text6 = text_MPRecover;
			text6.text = text6.text + color_Lower + text4 + "</Color>";
		}
		else
		{
			text_MPRecover.text += text4;
		}
		Text text7 = text_MPRecover;
		text7.text = text7.text + "/" + 1001101.GetText();
		float num3 = ((!(wand == null)) ? wand.WandShootInterval : (wandCfg.shootInterval + wandCfg.GetExtraShootInterval()));
		if (num3 < 0f)
		{
			num3 = 0f;
		}
		string text8 = GeneralTool.FloatToRetainDecimals(num3, 2);
		text_ShootInterval.text = 14000204.GetText(forceApplyAlogia: true) + ": ";
		if (num3 > wandCfg.shootInterval)
		{
			Text text9 = text_ShootInterval;
			text9.text = text9.text + color_Lower + text8 + "</Color>";
		}
		else if (num3 < wandCfg.shootInterval)
		{
			Text text10 = text_ShootInterval;
			text10.text = text10.text + color_Higher + text8 + "</Color>";
		}
		else
		{
			text_ShootInterval.text += text8;
		}
		text_ShootInterval.text += 1001102.GetText();
		float num4 = ((!(wand == null)) ? wand.WandCoolDown : (wandCfg.coolDown + wandCfg.GetExtraCoolDown()));
		if (num4 < 0f)
		{
			num4 = 0f;
		}
		string text11 = GeneralTool.FloatToRetainDecimals(num4, 2);
		text_CoolDown.text = 14000205.GetText() + ": ";
		if (num4 > wandCfg.coolDown)
		{
			Text text12 = text_CoolDown;
			text12.text = text12.text + color_Lower + text11 + "</Color>";
		}
		else if (num4 < wandCfg.coolDown)
		{
			Text text13 = text_CoolDown;
			text13.text = text13.text + color_Higher + text11 + "</Color>";
		}
		else
		{
			text_CoolDown.text += text11;
		}
		text_CoolDown.text += 1001103.GetText();
		text_Info.text = wandCfg.GetInfo();
		rtsf_Separator.gameObject.SetActive(text_Info.text.Length != 0);
		rtsf_Spells.DestroyAllChild();
		for (int i = 0; i < wandCfg.normalSlots.Length; i++)
		{
			UnityEngine.Object.Instantiate(pfb_UISlotWandExternal, rtsf_Spells).Initialize(wandCfg, i, WandSlotType.Normal);
		}
		if (wandCfg.postSlots.Length != 0)
		{
			UnityEngine.Object.Instantiate(pfb_UISpecialSlotBlocker, rtsf_Spells);
		}
		for (int j = 0; j < wandCfg.postSlots.Length; j++)
		{
			UnityEngine.Object.Instantiate(pfb_UISlotWandExternal, rtsf_Spells).Initialize(wandCfg, j, WandSlotType.Post);
		}
		text_Info.gameObject.SetActive(value: false);
		yield return null;
		text_Info.gameObject.SetActive(value: true);
		yield return null;
		uiLayout.Layout();
		textTypeBG.sizeDelta = new Vector2(text_Type.preferredWidth + textBGWidthExtra, textTypeBG.rect.height);
		rtsf_Spells.anchoredPosition = new Vector2(padding.x, text_Info.rectTransform.anchoredPosition.y - text_Info.rectTransform.sizeDelta.y - space - spellsAnchoredHightSub - padding.y / 4f);
		float num5 = originalSelfWidth;
		if (text_Name.rectTransform.sizeDelta.x + borderWidth > num5)
		{
			num5 = text_Name.rectTransform.sizeDelta.x + borderWidth;
		}
		if (text_Info.rectTransform.sizeDelta.x + borderWidth > num5)
		{
			num5 = text_Info.rectTransform.sizeDelta.x + borderWidth;
		}
		rtsf_Spells.localScale = new Vector3(1f, 1f, 1f);
		if ((rtsf_Spells.sizeDelta.x + subWidthMax + padding.x * 2f) * rtsf_Self.localScale.x > UIMgr.Inst.rtsf_Canvas1.rect.width)
		{
			float num6 = (UIMgr.Inst.rtsf_Canvas1.rect.width - padding.x * 2f - subWidthMax) / (rtsf_Spells.sizeDelta.x * rtsf_Self.localScale.x);
			rtsf_Spells.localScale = new Vector3(num6, num6, 1f);
			num5 = (UIMgr.Inst.rtsf_Canvas1.rect.width - padding.x * 2f - subWidthMax) / rtsf_Self.localScale.x;
		}
		else if (rtsf_Spells.sizeDelta.x > num5)
		{
			num5 = rtsf_Spells.sizeDelta.x;
		}
		float num7 = 0f - text_Info.rectTransform.anchoredPosition.y + rtsf_Spells.sizeDelta.y + padding.y + space + padding.y / 4f;
		if (text_Info.text.Length != 0)
		{
			num7 += text_Info.rectTransform.sizeDelta.y;
		}
		else
		{
			Vector2 anchoredPosition = rtsf_Spells.anchoredPosition;
			anchoredPosition.y = text_Info.rectTransform.anchoredPosition.y - spellsAnchoredHightSub;
			rtsf_Spells.anchoredPosition = anchoredPosition;
			num7 -= text_Info.rectTransform.sizeDelta.y;
		}
		if (infoBlockMinWidth > num5)
		{
			num5 = infoBlockMinWidth;
		}
		float num8 = num5 + padding.x * 2f;
		float num9 = text_ShootInterval.preferredWidth + rtsf_text_ShootInterval.anchoredPosition.x;
		float num10 = text_ShootInterval.preferredWidth + rtsf_text_ShootInterval.anchoredPosition.x;
		float num11 = ((num9 > num10) ? num9 : num10);
		if (num8 < num10)
		{
			num8 = ((!(wandInfoWidthMaxTextCoolDown < num11) || !(wandInfoWidthMaxTextCoolDown > num8)) ? ((wandInfoWidthMaxTextCoolDown > num11) ? num11 : wandInfoWidthMaxTextCoolDown) : wandInfoWidthMaxTextCoolDown);
		}
		rtsf_TextCoolDown.sizeDelta = new Vector2(initialTextWidth + num8 - originalSelfWidth, rtsf_TextCoolDown.sizeDelta.y);
		rtsf_text_ShootInterval.sizeDelta = rtsf_TextCoolDown.sizeDelta;
		rtsf_Separator.sizeDelta = new Vector2(num5 - dataBlockerInvisibleWidth + 12f, rtsf_Separator.sizeDelta.y);
		rtsf_Self.sizeDelta = new Vector2(num8, num7);
		if (rtsf_Curse != null)
		{
			rtsf_Curse.anchoredPosition = rtsf_Self.anchoredPosition + new Vector2(0f, rtsf_Self.sizeDelta.y);
		}
		if (raycaster != null)
		{
			UnityEngine.Object.DestroyImmediate(raycaster);
		}
		if (canvasRaise != null)
		{
			UnityEngine.Object.DestroyImmediate(canvasRaise);
		}
		if (raiseSpellLayer && GameMgr.IsMobile_Static)
		{
			canvasRaise = rtsf_Spells.gameObject.AddComponent<Canvas>();
			canvasRaise.overrideSorting = true;
			canvasRaise.sortingOrder = 999;
			raycaster = rtsf_Spells.gameObject.AddComponent<GraphicRaycaster>();
		}
		if (canvasGroup != null && ChangeAlpha)
		{
			canvasGroup.alpha = 1f;
		}
	}
}
