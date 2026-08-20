using DG.Tweening;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRewardRelic : UIRewardBase
{
	public Animator animator;

	public Image image_Icon;

	public Text text_Name;

	public Text text_Rarity;

	public Text text_Type;

	public Text text_Info;

	public RectTransform backgroundType;

	public RectTransform backgroundRarity;

	public float backgroundWidthExtra;

	public AnimationCurve RelicFadeInCoverCurve;

	public AnimationCurve SpellJumpLightCurve;

	public float RelicFadeInTime;

	public float JumpLightDuration;

	public Image RelicJumpLightImage;

	public Image RelicCardBaseImage;

	public Image RelicHoverFrameImage;

	public Sprite[] RelicBaseSprites;

	public Material[] RelicJumpLightEffect;

	public Sprite[] HoverFrames;

	public GameObject NormalRelicSelectParticle;

	public GameObject RareRelicSelectParticle;

	public GameObject EpicRelicSelectParticle;

	public GameObject AppearParticl;

	private static readonly int Progress = Shader.PropertyToID("_Progress");

	public bool Picked;

	private EntityManager ettMgr;

	public RectTransform LostCastleRuneInfoBG;

	public GameObject RedRune;

	public GameObject GreenRune;

	public GameObject BlueRune;

	public Text RedRuneText;

	public Text GreenRuneText;

	public Text BlueRuneText;

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	public void UpdateInfo()
	{
		NormalRelicSelectParticle.SetActive(value: false);
		RareRelicSelectParticle.SetActive(value: false);
		EpicRelicSelectParticle.SetActive(value: false);
		DynamicBuffer<LevelRewardInfoBED> buffer = ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt);
		RelicConfig config = RelicConfig.GetConfig(buffer[index].info.id);
		if (PlayerMgr.Inst.ItemCtrller.GetRelicConfig(buffer[index].info.id) != null)
		{
			config.level = PlayerMgr.Inst.ItemCtrller.GetRelicConfig(buffer[index].info.id).level + 1;
		}
		image_Icon.sprite = ABResources.LoadAsset<Sprite>(config.GetIconPath());
		text_Name.text = config.GetName();
		text_Rarity.text = config.GetStrRarity();
		text_Rarity.color = GeneralTool.GetRarityColor(config.dropType);
		text_Type.text = 1002202.GetText();
		text_Info.text = config.GetInfo(includeExtraInfo: false, upgrade: true);
		text_Info.text = GeneralTool.FormatTextIfPublishTest(text_Info, text_Info.text);
		if (PlayerMgr.Inst.ItemCtrller.uiRelic_RuneWizard != null)
		{
			RedRune.SetActive(config.RedRunePoint > 0);
			GreenRune.SetActive(config.GreenRunePoint > 0);
			BlueRune.SetActive(config.BlueRunePoint > 0);
			RedRuneText.text = "<b>" + 7040251.GetText() + "+" + config.level * config.RedRunePoint + "</b>";
			GreenRuneText.text = "<b>" + 7040261.GetText() + "+" + config.level * config.GreenRunePoint + "</b>";
			BlueRuneText.text = "<b>" + 7040271.GetText() + "+" + config.level * config.BlueRunePoint + "</b>";
		}
		else
		{
			RedRune.SetActive(value: false);
			GreenRune.SetActive(value: false);
			BlueRune.SetActive(value: false);
		}
		int num = 0;
		if (LostCastleRuneInfoBG.rect.height > 0f)
		{
			num = 15;
		}
		LostCastleRuneInfoBG.anchoredPosition = new Vector2(-80f, backgroundType.anchoredPosition.y - text_Info.rectTransform.sizeDelta.y - (float)num);
		backgroundType.sizeDelta = new Vector2(text_Type.preferredWidth + backgroundWidthExtra, backgroundType.sizeDelta.y + LostCastleRuneInfoBG.sizeDelta.y + (float)num);
		backgroundRarity.sizeDelta = new Vector2(text_Rarity.preferredWidth + backgroundWidthExtra, backgroundRarity.sizeDelta.y + LostCastleRuneInfoBG.sizeDelta.y + (float)num);
		switch (config.dropType)
		{
		case ItemDropType.Common:
			RelicJumpLightImage.material = RelicJumpLightEffect[0];
			RelicCardBaseImage.sprite = RelicBaseSprites[0];
			NormalRelicSelectParticle.SetActive(value: true);
			RelicHoverFrameImage.sprite = HoverFrames[0];
			break;
		case ItemDropType.Rare:
			RelicJumpLightImage.material = RelicJumpLightEffect[1];
			RelicCardBaseImage.sprite = RelicBaseSprites[1];
			RareRelicSelectParticle.SetActive(value: true);
			RelicHoverFrameImage.sprite = HoverFrames[1];
			break;
		case ItemDropType.Epic:
			RelicJumpLightImage.material = RelicJumpLightEffect[2];
			RelicCardBaseImage.sprite = RelicBaseSprites[2];
			EpicRelicSelectParticle.SetActive(value: true);
			RelicHoverFrameImage.sprite = HoverFrames[2];
			break;
		}
	}

	public override void Initialize(Entity levelRewardEtt, int index)
	{
		base.levelRewardEtt = levelRewardEtt;
		base.index = index;
		UpdateInfo();
		SetShow();
	}

	private void Update()
	{
	}

	public override void SetShow()
	{
		if (!Picked)
		{
			interactable = true;
			animator.SetTrigger("Appear");
			SpellFadeIn();
			DynamicBuffer<LevelRewardInfoBED> buffer = ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt);
			DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Relic, buffer[index].info.id);
		}
	}

	public override void SetHide()
	{
		interactable = false;
		if (!Picked)
		{
			animator.SetTrigger("Disappear");
			animator.SetTrigger("Unhover");
		}
	}

	private void SpellFadeIn()
	{
		AppearParticl.SetActive(value: false);
		AppearParticl.SetActive(value: true);
		GeneralTool.InitialImageMaterial(RelicJumpLightImage);
		RelicJumpLightImage.material.SetFloat(Progress, 0f);
		RelicJumpLightImage.material.SetFloat("_RandomSeed", Random.Range(-10f, 10f));
		RelicJumpLightImage.material.DOFloat(1f, Progress, JumpLightDuration).SetEase(SpellJumpLightCurve).SetUpdate(isIndependentUpdate: true);
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
		if (!Picked && interactable && GameUISingletonMono<UILevelReward>.Inst.canvasGroup.interactable)
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
		base.Select();
		interactable = false;
		Picked = true;
		animator.SetTrigger("Select");
		GameUISingletonMono<UILevelReward>.Inst.RewardSelect(index);
	}

	public override void Hover()
	{
		base.Hover();
		animator.SetTrigger("Hover");
		SEMgr.Inst.uiRewardRelicHover.PlaySE();
	}

	public override void UnHover()
	{
		base.UnHover();
		animator.SetTrigger("Unhover");
	}
}
