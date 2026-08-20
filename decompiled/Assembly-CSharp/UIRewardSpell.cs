using DG.Tweening;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class UIRewardSpell : UIRewardBase
{
	public Animator animator;

	public Image image_Icon;

	public Image image_Background;

	public Image image_BackgroundFrame;

	public Image image_RareFrame;

	public GameObject go_Star1;

	public GameObject go_Star2;

	public Text text_Name;

	public Text text_Rarity;

	public Text text_Type;

	public Text text_Cost;

	public Text text_Info;

	public Text text_Des;

	public RectTransform textTypeBG;

	public float textBGWidthExtra = 8f;

	[Header("Lock")]
	public GameObject panel_LockBtn;

	public Image image_LockBtn;

	public Image image_LockBtnOutline;

	public Sprite sprite_LockOff;

	public Sprite sprite_LockOn;

	public Image image_Chain1;

	public Image image_Chain2;

	public float chainOffsetMoveSpeed;

	public UIRewardSpellHover bgHover;

	public UIRewardSpellHover lockHover;

	public Sprite spriteCommonBG;

	public Sprite spriteRareBG;

	public Sprite spriteEpicBG;

	public Sprite spriteCommonFrame;

	public Sprite spriteRareBGFrame;

	public Sprite spriteEpicBGFrame;

	public Sprite spriteCommonSpellBg;

	public Sprite spriteRareSpellBg;

	public Sprite spriteEpicSpellBg;

	public Sprite spriteSpecialSpellBg;

	public float intervalCost;

	public float intervalInfoDes;

	public GameObject line;

	public RectTransform rtsf_InfoAndDes;

	public float infoAndDesYShowCost = 80f;

	public float infoAndDesYHideCost = 58f;

	public RectTransform rtsf_Type;

	public RectTransform rtsf_Des;

	public RectTransform rtsf_line;

	public RectTransform rtsf_Info;

	public RectTransform rtsf_BackgroundType;

	public RectTransform rtsf_BackgroundRarity;

	public Image SpellCoverFadeImage;

	public AnimationCurve SpellFadeInCoverCurve;

	public AnimationCurve SpellJumpLightCurve;

	public float SpellFadeInTime;

	public float JumpLightDuration;

	public Image SpellJumpLightImage;

	public Sprite[] SpellSoftBorderSprites;

	public Material[] SpellHardBorder;

	public Material[] SpellFadeInEffect;

	public Material[] SpellJumpLightEffect;

	public GameObject NormalSpellSelectParticle;

	public GameObject RareSpellSelectParticle;

	public GameObject EpicSpellSelectParticle;

	private static readonly int Progress = Shader.PropertyToID("_Progress");

	private float currentOffset;

	private EntityManager ettMgr;

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	private void Update()
	{
		if (!GameUISingletonMono<UILevelReward>.Inst.IsOpen)
		{
			return;
		}
		if (ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt)[index].isLock)
		{
			if (currentOffset != 0f)
			{
				currentOffset = Mathf.MoveTowards(currentOffset, 0f, chainOffsetMoveSpeed * Time.unscaledDeltaTime);
				image_Chain1.material.SetFloat("_OffsetY", currentOffset);
			}
		}
		else if (currentOffset != 1f)
		{
			currentOffset = Mathf.MoveTowards(currentOffset, 1f, chainOffsetMoveSpeed * Time.unscaledDeltaTime);
			image_Chain1.material.SetFloat("_OffsetY", currentOffset);
		}
	}

	public void SpellFadeIn()
	{
		GeneralTool.InitialImageMaterial(SpellCoverFadeImage);
		SpellCoverFadeImage.material.SetFloat(Progress, 0f);
		SpellCoverFadeImage.material.DOFloat(1f, Progress, SpellFadeInTime).SetEase(SpellFadeInCoverCurve).SetUpdate(isIndependentUpdate: true);
		GeneralTool.InitialImageMaterial(SpellJumpLightImage);
		SpellJumpLightImage.material.SetFloat(Progress, 0f);
		SpellJumpLightImage.material.SetFloat("_RandomSeed", Random.Range(-10f, 10f));
		SpellJumpLightImage.material.DOFloat(1f, Progress, JumpLightDuration).SetEase(SpellJumpLightCurve).SetUpdate(isIndependentUpdate: true);
	}

	public void UpdateInfo()
	{
		text_Info.text = "";
		text_Des.text = "";
		SpellConfig configCopy = SpellConfig.GetConfigCopy(ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt)[index].info.id);
		if (configCopy.level >= 2)
		{
			go_Star1.SetActive(value: true);
		}
		else
		{
			go_Star1.SetActive(value: false);
		}
		if (configCopy.level >= 3)
		{
			go_Star2.SetActive(value: true);
		}
		else
		{
			go_Star2.SetActive(value: false);
		}
		NormalSpellSelectParticle.SetActive(value: false);
		RareSpellSelectParticle.SetActive(value: false);
		EpicSpellSelectParticle.SetActive(value: false);
		switch (configCopy.dropType)
		{
		case ItemDropType.None:
			image_Background.sprite = spriteCommonBG;
			image_BackgroundFrame.sprite = spriteCommonFrame;
			image_RareFrame.sprite = spriteCommonSpellBg;
			break;
		case ItemDropType.Common:
			image_Background.sprite = spriteCommonBG;
			image_BackgroundFrame.sprite = spriteCommonFrame;
			image_RareFrame.sprite = spriteCommonSpellBg;
			SpellCoverFadeImage.sprite = spriteCommonBG;
			NormalSpellSelectParticle.SetActive(value: true);
			SpellCoverFadeImage.material = SpellFadeInEffect[0];
			SpellJumpLightImage.material = SpellJumpLightEffect[0];
			break;
		case ItemDropType.Rare:
			image_Background.sprite = spriteRareBG;
			image_BackgroundFrame.sprite = spriteRareBGFrame;
			image_RareFrame.sprite = spriteRareSpellBg;
			SpellCoverFadeImage.sprite = spriteRareBG;
			RareSpellSelectParticle.SetActive(value: true);
			SpellCoverFadeImage.material = SpellFadeInEffect[1];
			SpellJumpLightImage.material = SpellJumpLightEffect[1];
			break;
		case ItemDropType.Epic:
			image_Background.sprite = spriteEpicBG;
			image_BackgroundFrame.sprite = spriteEpicBGFrame;
			image_RareFrame.sprite = spriteEpicSpellBg;
			EpicSpellSelectParticle.SetActive(value: true);
			SpellCoverFadeImage.sprite = spriteEpicBG;
			SpellCoverFadeImage.material = SpellFadeInEffect[2];
			SpellJumpLightImage.material = SpellJumpLightEffect[2];
			break;
		case ItemDropType.Special:
			image_Background.sprite = spriteCommonBG;
			image_BackgroundFrame.sprite = spriteCommonFrame;
			image_RareFrame.sprite = spriteSpecialSpellBg;
			break;
		}
		text_Cost.gameObject.SetActive(value: false);
		switch (configCopy.useType)
		{
		case SpellType.Missile:
			text_Type.text = 1002205.GetText();
			text_Cost.gameObject.SetActive(value: true);
			text_Cost.text = configCopy.mpCost.ToString();
			text_Type.color = GameConst.color_SpellUseTypeMissle;
			break;
		case SpellType.Summon:
			text_Type.text = 1002206.GetText();
			text_Cost.gameObject.SetActive(value: true);
			text_Cost.text = configCopy.mpCost.ToString();
			text_Type.color = GameConst.color_SpellUseTypeMissle;
			break;
		case SpellType.Enhance:
			text_Type.text = 1002207.GetText();
			text_Type.color = GameConst.color_SpellUseTypeEnhance;
			break;
		case SpellType.Passive:
			text_Type.text = 1002208.GetText();
			text_Type.color = GameConst.color_SpellUseTypePassive;
			break;
		default:
			text_Type.text = 1002203.GetText();
			break;
		}
		rtsf_InfoAndDes.anchoredPosition = new Vector2(rtsf_InfoAndDes.anchoredPosition.x, infoAndDesYHideCost);
		image_Icon.sprite = ABResources.LoadAsset<Sprite>(configCopy.GetIconPath());
		text_Name.text = configCopy.GetName();
		text_Rarity.text = configCopy.GetStrRarity();
		text_Rarity.color = GeneralTool.GetRarityColor(configCopy.dropType);
		rtsf_BackgroundType.sizeDelta = new Vector2(text_Type.preferredWidth + 4f, rtsf_BackgroundType.sizeDelta.y);
		rtsf_BackgroundRarity.sizeDelta = new Vector2(text_Rarity.preferredWidth + 4f, rtsf_BackgroundRarity.sizeDelta.y);
		switch (configCopy.useType)
		{
		case SpellType.Missile:
		case SpellType.Summon:
			text_Info.text = configCopy.GetInfo(1f, "◆\u00a0\u200a");
			text_Des.text = configCopy.GetDes(1f, "◆\u00a0\u200a", GameConst.colorSpellDes, "◆\u00a0\u200a");
			break;
		case SpellType.Enhance:
		case SpellType.Passive:
			if (GameConstManaged.LostCastleRuneID.Contains(configCopy.id))
			{
				text_Info.text = configCopy.GetInfo(1f, "◆\u00a0\u200a", withDetailInfo: false);
			}
			else
			{
				text_Info.text = configCopy.GetInfo(1f, "◆\u00a0\u200a");
			}
			if (text_Info.text != null && text_Info.text != "")
			{
				text_Info.text += "\n";
			}
			text_Info.text += configCopy.GetDes(1f, "◆\u00a0\u200a", "", "◆\u00a0\u200a");
			break;
		default:
			text_Type.text = 1002203.GetText();
			break;
		}
		if (text_Des.text == "")
		{
			text_Des.gameObject.SetActive(value: false);
			line.SetActive(value: false);
		}
		else
		{
			text_Des.gameObject.SetActive(value: true);
			line.SetActive(value: true);
		}
		text_Info.text = GeneralTool.FormatTextIfPublishTest(text_Info, text_Info.text);
		text_Des.text = GeneralTool.FormatTextIfPublishTest(text_Des, text_Des.text);
		textTypeBG.sizeDelta = new Vector2(text_Type.preferredWidth + textBGWidthExtra, textTypeBG.rect.height);
		rtsf_line.anchoredPosition = new Vector2(rtsf_line.anchoredPosition.x, rtsf_Info.anchoredPosition.y - text_Info.preferredHeight - rtsf_line.sizeDelta.y / 2f - intervalInfoDes - 2f);
		rtsf_Des.anchoredPosition = new Vector2(rtsf_Des.anchoredPosition.x, rtsf_line.anchoredPosition.y - rtsf_line.sizeDelta.y / 2f - intervalInfoDes);
	}

	public void PointerEnter()
	{
		if (interactable && GameUISingletonMono<UILevelReward>.Inst.canvasGroup.interactable && (!GameMgr.IsMobile_Static || ControlMgr.Inst.usingpad))
		{
			Hover();
		}
	}

	public void PointerExit()
	{
		if (interactable && GameUISingletonMono<UILevelReward>.Inst.canvasGroup.interactable && (!GameMgr.IsMobile_Static || ControlMgr.Inst.usingpad))
		{
			UnHover();
		}
	}

	public void PointerClick()
	{
		if (interactable && !ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt)[index].isPicked && GameUISingletonMono<UILevelReward>.Inst.canvasGroup.interactable)
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
		DynamicBuffer<LevelRewardInfoBED> buffer = ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt);
		base.Select();
		LevelRewardInfoBED value = buffer[index];
		value.isPicked = true;
		buffer[index] = value;
		interactable = false;
		animator.Play("Select");
		if (DataMgr.selectedWorldData.ActivateGirlHaveSpellLock())
		{
			panel_LockBtn.SetActive(value: false);
			value.isLock = false;
			buffer[index] = value;
		}
		GameUISingletonMono<UILevelReward>.Inst.RewardSelect(index);
	}

	public override void Hover()
	{
		base.Hover();
		animator.Play("Hover");
		SEMgr.Inst.uiRewardRelicHover.PlaySE();
	}

	public override void UnHover()
	{
		base.UnHover();
		animator.Play("Unhover", 1, 0f);
	}

	public void PointerEnterLockButton()
	{
		animator.Play("Layer2_Hover");
		SEMgr.Inst.uiRewardSpellLockBtnHover.PlaySE();
	}

	public void PointerExitLockButton()
	{
		animator.Play("Layer2_Unhover");
	}

	public void PointerClickLockButton()
	{
		if (DataMgr.selectedWorldData.ActivateGirlHaveSpellLock())
		{
			animator.Play("Layer2_Click", 2, 0f);
			SEMgr.Inst.uiRewardSpellChain.PlaySE();
			DynamicBuffer<LevelRewardInfoBED> buffer = ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt);
			LevelRewardInfoBED value = buffer[index];
			value.isLock = !value.isLock;
			buffer[index] = value;
			image_LockBtn.sprite = (buffer[index].isLock ? sprite_LockOn : sprite_LockOff);
			image_LockBtnOutline.sprite = image_LockBtn.sprite;
		}
	}

	public void Reroll(int id)
	{
		DynamicBuffer<LevelRewardInfoBED> buffer = ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt);
		if (!buffer[index].isLock)
		{
			LevelRewardInfoBED value = buffer[index];
			value.info.id = id;
			buffer[index] = value;
			UpdateInfo();
			SetShow();
			DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, id);
		}
	}

	public override void Initialize(Entity levelRewardEtt, int index)
	{
		base.levelRewardEtt = levelRewardEtt;
		base.index = index;
		UpdateInfo();
		SetShow();
		DynamicBuffer<LevelRewardInfoBED> buffer = ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt);
		DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, buffer[index].info.id);
		if (DataMgr.selectedWorldData.ActivateGirlHaveSpellLock() && SpellConfig.GetConfigCopy(buffer[index].info.id).dropType != ItemDropType.Special)
		{
			panel_LockBtn.SetActive(value: true);
			image_Chain1.material = Object.Instantiate(image_Chain1.material);
			image_Chain2.material = image_Chain1.material;
			if (buffer[index].isLock)
			{
				image_Chain1.material.SetFloat("_OffsetY", 0f);
				image_LockBtn.sprite = sprite_LockOn;
				image_LockBtnOutline.sprite = image_LockBtn.sprite;
			}
			else
			{
				image_Chain1.material.SetFloat("_OffsetY", 1f);
				image_LockBtn.sprite = sprite_LockOff;
				image_LockBtnOutline.sprite = image_LockBtn.sprite;
			}
		}
		else
		{
			panel_LockBtn.SetActive(value: false);
		}
	}

	public override void SetShow()
	{
		if (!ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt)[index].isPicked)
		{
			interactable = true;
			animator.Play("Show", 0, 0f);
			SpellFadeIn();
		}
	}

	public override void SetHide()
	{
		base.SetHide();
		if (!ettMgr.GetBuffer<LevelRewardInfoBED>(levelRewardEtt)[index].isPicked)
		{
			interactable = false;
			animator.Play("Hide");
			animator.Play("Unhover");
		}
	}
}
