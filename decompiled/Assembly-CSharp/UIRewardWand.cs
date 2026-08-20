using System.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRewardWand : UIRewardBase
{
	public UISlotWandExternal pfb_UISlotWandExternal;

	public GameObject pfb_UISpecialSlotBlocker;

	public RectTransform rtsf_Self;

	public RectTransform rtsf_Separator;

	public RectTransform rtsf_Spells;

	public RectTransform textTypeBG;

	public Image image_Icon;

	public Text text_Name;

	public Text text_Wand;

	public Text text_Info;

	public Text text_MP;

	public Text text_MPRecovery;

	public Text text_ShootInterval;

	public Text text_CoolDown;

	public Animator animator;

	public UILayout uiLayout;

	public Vector2 padding;

	public float space;

	public float infoBlockMinWidth;

	public float dataBlockerInvisibleWidth;

	public float borderWidth;

	public float originalSelfWidth = 400f;

	public float textBGWidthExtra = 8f;

	private float wandInfoWidthMaxTextCoolDown = 550f;

	private float spellMaxWidthOnMobile = 700f;

	private float initialTextWidth;

	private RectTransform rtsf_TextCoolDown;

	private RectTransform rtsf_text_ShootInterval;

	private EntityManager ettMgr;

	public void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		rtsf_TextCoolDown = text_CoolDown.GetComponent<RectTransform>();
		rtsf_text_ShootInterval = text_ShootInterval.GetComponent<RectTransform>();
		initialTextWidth = rtsf_TextCoolDown.sizeDelta.x;
	}

	public void UpdateInfo()
	{
		StartCoroutine(UpdateInfoIE());
	}

	private IEnumerator UpdateInfoIE()
	{
		DynamicBuffer<LevelRewardInfoBED> buffer = ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt);
		WandConfig wandConfig = WandConfig.dic[buffer[index].info.id];
		text_Name.text = wandConfig.GetName();
		text_Wand.text = 1002204.GetText(forceApplyAlogia: true);
		image_Icon.sprite = ABResources.LoadAsset<Sprite>(wandConfig.GetIconPath());
		float current = (float)wandConfig.maxMP + wandConfig.GetExtraMaxMP();
		string source = current.ToString("F0");
		text_MP.text = 14000202.GetText(forceApplyAlogia: true) + ": ";
		text_MP.text += TextProcesser.GetColorText(source, TextProcesser.GetColor_BigIsGood(wandConfig.maxMP, current));
		float num = (float)wandConfig.mpRecovery + wandConfig.GetExtraMPRecovery();
		string source2 = GeneralTool.FloatToRetainDecimals(num, 1);
		text_MPRecovery.text = 14000203.GetText(forceApplyAlogia: true) + ": ";
		text_MPRecovery.text += TextProcesser.GetColorText(source2, TextProcesser.GetColor_BigIsGood(wandConfig.mpRecovery, num));
		float num2 = wandConfig.shootInterval + wandConfig.GetExtraShootInterval();
		string source3 = GeneralTool.FloatToRetainDecimals(num2, 2);
		text_ShootInterval.text = 14000204.GetText(forceApplyAlogia: true) + ": ";
		text_ShootInterval.text += TextProcesser.GetColorText(source3, TextProcesser.GetColor_SmallIsGood(wandConfig.shootInterval, num2));
		float num3 = wandConfig.coolDown + wandConfig.GetExtraCoolDown();
		string source4 = GeneralTool.FloatToRetainDecimals(num3, 2);
		text_CoolDown.text = 14000205.GetText(forceApplyAlogia: true) + ": ";
		text_CoolDown.text += TextProcesser.GetColorText(source4, TextProcesser.GetColor_SmallIsGood(wandConfig.coolDown, num3));
		text_Info.text = GeneralTool.FormatTextIfPublishTest(text_Info, wandConfig.GetInfo());
		rtsf_Separator.gameObject.SetActive(text_Info.text.Length > 0);
		rtsf_Spells.DestroyAllChild();
		for (int i = 0; i < wandConfig.normalSlots.Length; i++)
		{
			Object.Instantiate(pfb_UISlotWandExternal, rtsf_Spells).Initialize(wandConfig, i, WandSlotType.Normal);
		}
		if (wandConfig.postSlots.Length != 0)
		{
			Object.Instantiate(pfb_UISpecialSlotBlocker, rtsf_Spells);
		}
		for (int j = 0; j < wandConfig.postSlots.Length; j++)
		{
			Object.Instantiate(pfb_UISlotWandExternal, rtsf_Spells).Initialize(wandConfig, j, WandSlotType.Post);
		}
		yield return null;
		if (!GameMgr.IsMobile_Static)
		{
			uiLayout.Layout();
		}
		textTypeBG.sizeDelta = new Vector2(text_Wand.preferredWidth + textBGWidthExtra, textTypeBG.rect.height);
		rtsf_Spells.anchoredPosition = new Vector2(padding.x, text_Info.rectTransform.anchoredPosition.y - text_Info.rectTransform.sizeDelta.y - space - padding.y / 4f);
		float num4 = 0f - text_Info.rectTransform.anchoredPosition.y + rtsf_Spells.sizeDelta.y + padding.y + space + padding.y / 4f;
		if (text_Info.text.Length != 0)
		{
			num4 += text_Info.rectTransform.sizeDelta.y;
		}
		else
		{
			Vector2 anchoredPosition = rtsf_Spells.anchoredPosition;
			anchoredPosition.y = text_Info.rectTransform.anchoredPosition.y;
			rtsf_Spells.anchoredPosition = anchoredPosition;
			num4 -= text_Info.rectTransform.sizeDelta.y;
		}
		float num5 = originalSelfWidth;
		if (text_Name.rectTransform.sizeDelta.x + borderWidth > num5)
		{
			num5 = text_Name.rectTransform.sizeDelta.x + borderWidth;
		}
		if (text_Info.rectTransform.sizeDelta.x + borderWidth > num5)
		{
			num5 = text_Info.rectTransform.sizeDelta.x + borderWidth;
		}
		if (GameMgr.IsMobile_Static && rtsf_Spells.sizeDelta.x > num5 && rtsf_Spells.sizeDelta.x + padding.x * 2f > spellMaxWidthOnMobile)
		{
			float num6 = spellMaxWidthOnMobile / (rtsf_Spells.sizeDelta.x + padding.x * 2f);
			rtsf_Spells.transform.localScale = new Vector3(num6, num6, 1f);
			num5 = spellMaxWidthOnMobile;
		}
		else if (rtsf_Spells.sizeDelta.x > num5)
		{
			num5 = rtsf_Spells.sizeDelta.x;
		}
		if (infoBlockMinWidth > num5)
		{
			num5 = infoBlockMinWidth;
		}
		float num7 = num5 + padding.x * 2f;
		float num8 = text_ShootInterval.preferredWidth + rtsf_text_ShootInterval.anchoredPosition.x;
		float num9 = text_ShootInterval.preferredWidth + rtsf_text_ShootInterval.anchoredPosition.x;
		float num10 = ((num8 > num9) ? num8 : num9);
		if (num7 < num9)
		{
			num7 = ((!(wandInfoWidthMaxTextCoolDown < num10) || !(wandInfoWidthMaxTextCoolDown > num7)) ? ((wandInfoWidthMaxTextCoolDown > num10) ? num10 : wandInfoWidthMaxTextCoolDown) : wandInfoWidthMaxTextCoolDown);
		}
		rtsf_TextCoolDown.sizeDelta = new Vector2(initialTextWidth + num7 - originalSelfWidth, rtsf_TextCoolDown.sizeDelta.y);
		rtsf_text_ShootInterval.sizeDelta = rtsf_TextCoolDown.sizeDelta;
		rtsf_Separator.sizeDelta = new Vector2(num5 - dataBlockerInvisibleWidth + 12f, rtsf_Separator.sizeDelta.y);
		rtsf_Self.sizeDelta = new Vector2(num7, num4);
		rtsf_BG.sizeDelta = new Vector2(num7, num4);
	}

	public override void Initialize(Entity levelRewardEtt, int index)
	{
		base.levelRewardEtt = levelRewardEtt;
		base.index = index;
		UpdateInfo();
		SetShow();
		DynamicBuffer<LevelRewardInfoBED> buffer = ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt);
		WandConfig wandConfig = WandConfig.dic[buffer[index].info.id];
		for (int i = 0; i < wandConfig.normalSlots.Length; i++)
		{
			if (wandConfig.normalSlots[i] != null)
			{
				DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, wandConfig.normalSlots[i].id);
			}
		}
		for (int j = 0; j < wandConfig.postSlots.Length; j++)
		{
			if (wandConfig.postSlots[j] != null)
			{
				DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, wandConfig.postSlots[j].id);
			}
		}
		DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Wand, buffer[index].info.id);
	}

	public override void SetShow()
	{
		interactable = true;
		animator.SetTrigger("Appear");
	}

	public override void SetHide()
	{
		interactable = false;
		animator.SetTrigger("Disappear");
		animator.SetTrigger("Unhover");
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (interactable && GameUISingletonMono<UILevelReward>.Inst.canvasGroup.interactable && (!GameMgr.IsMobile_Static || ControlMgr.Inst.usingpad))
		{
			Hover();
		}
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (interactable && GameUISingletonMono<UILevelReward>.Inst.canvasGroup.interactable && (!GameMgr.IsMobile_Static || ControlMgr.Inst.usingpad))
		{
			UnHover();
		}
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (interactable && GameUISingletonMono<UILevelReward>.Inst.canvasGroup.interactable)
		{
			if (GameMgr.IsMobile_Static && !ControlMgr.Inst.usingpad)
			{
				GameUISingletonMono<UILevelReward>.Inst.RewardMobileSelect(index, this);
			}
			else
			{
				Select();
			}
		}
	}

	public override void Select()
	{
		interactable = false;
		animator.SetTrigger("Select");
		GameUISingletonMono<UILevelReward>.Inst.RewardSelect(index);
	}

	public override void Hover()
	{
		animator.SetTrigger("Hover");
		SEMgr.Inst.wandChange.PlaySE();
	}

	public override void UnHover()
	{
		animator.SetTrigger("Unhover");
	}
}
