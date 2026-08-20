using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Newtonsoft.Json;
using PlayerLogger.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UIResearch")]
public class UIResearch : GameUISingletonMono<UIResearch>
{
	public bool showFinish;

	public GameObject pfb_UIResearchSlot;

	public GameObject pfb_UIResearchSlotMobile;

	public Animator anima;

	public GridLayoutGroup uiLayout;

	public Text text_BloodCount;

	[Header("Gamepad")]
	public Custom_ScrollRect scrollRect;

	[Header("LanguageChange")]
	public Text text_Title;

	private List<UIResearchSlot> uiSlots = new List<UIResearchSlot>();

	private UIResearchSlot gamepadSelectedSlot;

	private ResearchChangeLogger researchChangeLogger;

	public GameObject UnlockResearchConfirmObj;

	public Image UnlockResearchIcon;

	public Text UnlockResearchName;

	public Text UnlockResearchDescription;

	public Text UnlockResearchText;

	public Text UnlockResearchRequireBloodText;

	public CanvasGroup UnlockResearchUIGroup;

	private UIResearchSlot currentSlot;

	[Header("pc demo")]
	public GameObject demoBlockPlayer;

	public void OpenResearchConfirmPanel(UIResearchSlot slot)
	{
		currentSlot = slot;
		UnlockResearchUIGroup.DOFade(1f, 0.3f);
		UnlockResearchConfirmObj.gameObject.SetActive(value: true);
		UnlockResearchIcon.sprite = slot.image_Icon.sprite;
		UnlockResearchName.text = slot.text_Name.text;
		UnlockResearchDescription.text = slot.text_Desc.text;
		UnlockResearchRequireBloodText.text = slot.text_Cost.text;
	}

	public void CloseResearchConfirmPanel()
	{
		UnlockResearchUIGroup.alpha = 0f;
		currentSlot = null;
		UnlockResearchConfirmObj.gameObject.SetActive(value: false);
	}

	public void ConfirmUnlockResearch()
	{
		StartCoroutine("UnlockCurrentResearch");
	}

	private void UnlockStage1(UIResearchSlot slot)
	{
		PlayerMgr.Inst.ChangeAncientBlood(-slot.Cfg.cost);
		DataMgr.selectedWorldData.researchedIDs.Add(slot.ID);
		if (slot.Cfg.abilityType != ResearchAbilityType.ResourceChanger && slot.Cfg.abilityType != ResearchAbilityType.Gallery)
		{
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIUnlockNewResearch"), UICampMgr.Inst.canvas.transform).GetComponent<UIUnlockNewResearch>().Initialize(slot.Cfg);
		}
		for (int i = 0; i < uiSlots.Count; i++)
		{
			uiSlots[i].UpdateState();
		}
		OrderSlots();
	}

	private void UnlockStage2(UIResearchSlot slot)
	{
		DataMgr.selectedWorldData.CalculateAddingPoints();
		SEMgr.Inst.uiClick.PlaySE();
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			gamepadSelectedSlot.OnPointerExit(null);
			gamepadSelectedSlot = uiLayout.transform.GetChild(0).GetComponent<UIResearchSlot>();
			gamepadSelectedSlot.SkipOnceSE();
			gamepadSelectedSlot.OnPointerEnter(null);
			UpdateGamepadScrollRectPoint(slideDirectionDown: false);
		}
		if (slot.Cfg.abilityType == ResearchAbilityType.PotionLimit)
		{
			PlayerMgr.Inst.BaData.potionMaxCount = 1 + DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.PotionLimit);
			PlayerMgr.Inst.ItemCtrller.PotionChangeSlot(0);
		}
		else if (slot.Cfg.abilityType == ResearchAbilityType.KeyChain)
		{
			DataMgr.selectedWorldData.battleData9.keyCount = DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.KeyChain);
			UIPlayerDataMgr.Inst.UpdateKey();
		}
		else if (slot.Cfg.abilityType == ResearchAbilityType.AdvancedScarecrow)
		{
			Destructible5[] componentsInChildren = CampMgr.Inst.tsf_ScarecrowParent.GetComponentsInChildren<Destructible5>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SingleInitialCallback();
			}
			UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/SpecialObjs/" + 3601), GameMgr.IsMobile_Static ? CampMgr.Inst.so36CreatePointMobile : CampMgr.Inst.so36CreatePoint, Quaternion.identity);
		}
		if (slot.Cfg.hdID != 0)
		{
			if (slot.Cfg.abilityType == ResearchAbilityType.Gallery)
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/Story_UnlockGallery")).GetComponent<StoryUnlock_Gallary>().Initialize(CampMgr.Inst.CurrentCampSkin.ett_Gallery, slot.Cfg.hdID);
			}
			else if (slot.Cfg.abilityType == ResearchAbilityType.ResourceChanger)
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/Story_UnlockResourceChanger")).GetComponent<StoryUnlock_Gallary>().Initialize(CampMgr.Inst.CurrentCampSkin.ett_ResourceChanger, slot.Cfg.hdID);
			}
			else
			{
				UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/Story_Unlock")).GetComponent<StoryUnlock>().Initialize(slot.Cfg.hdID, StoryUnlockUIType.Research);
			}
		}
	}

	private IEnumerator UnlockCurrentResearch()
	{
		UIResearchSlot slot = currentSlot;
		UnlockStage1(slot);
		yield return null;
		UnlockStage2(slot);
		CloseResearchConfirmPanel();
	}

	public override void Hide()
	{
		if (GameMgr.IsMobile_Static && UnlockResearchConfirmObj.activeSelf)
		{
			CloseResearchConfirmPanel();
		}
		else
		{
			base.Hide();
		}
	}

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
		EventMgr.AncienBloodChange = (Action)Delegate.Combine(EventMgr.AncienBloodChange, new Action(AncienBloodChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		base.inputActions.Player.Interact.performed += InteractPerformed;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		base.inputActions.Player.Interact.performed -= InteractPerformed;
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.AncienBloodChange = (Action)Delegate.Remove(EventMgr.AncienBloodChange, new Action(AncienBloodChange));
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if ((GameMgr.IsMobile_Static || ICJNOGPFMAM.MIFJADDOODN) && UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			MoveDirect(direct);
		}
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if ((GameMgr.IsMobile_Static || ICJNOGPFMAM.MIFJADDOODN) && UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector, ControlMgr.rampType.UpDown);
			MoveDirect(vector);
		}
	}

	private void MoveDirect(Vector2 _direct)
	{
		if ((bool)UnlockResearchConfirmObj && UnlockResearchConfirmObj.activeInHierarchy)
		{
			return;
		}
		if (!gamepadSelectedSlot || !gamepadSelectedSlot.gameObject.activeInHierarchy)
		{
			InitGamepadSelectedSlot(forceReset: false);
			if (gamepadSelectedSlot == null)
			{
				return;
			}
		}
		Debug.Log(gamepadSelectedSlot.transform.GetSiblingIndex());
		if (_direct == Vector2.up)
		{
			UIResearchSlot uIResearchSlot = null;
			for (int num = gamepadSelectedSlot.transform.GetSiblingIndex() - 1; num >= 0; num--)
			{
				Transform child = uiLayout.transform.GetChild(num);
				if (child.gameObject.activeSelf)
				{
					uIResearchSlot = child.GetComponent<UIResearchSlot>();
					break;
				}
			}
			if (uIResearchSlot != null && gamepadSelectedSlot != uIResearchSlot)
			{
				gamepadSelectedSlot.OnPointerExitPad();
				gamepadSelectedSlot = uIResearchSlot;
				gamepadSelectedSlot.OnPointerEnterPad();
				UpdateGamepadScrollRectPoint(slideDirectionDown: false);
			}
		}
		else
		{
			if (!(_direct == Vector2.down))
			{
				return;
			}
			UIResearchSlot uIResearchSlot2 = null;
			for (int i = gamepadSelectedSlot.transform.GetSiblingIndex() + 1; i < uiSlots.Count; i++)
			{
				Transform child2 = uiLayout.transform.GetChild(i);
				if (child2.gameObject.activeSelf)
				{
					uIResearchSlot2 = child2.GetComponent<UIResearchSlot>();
					break;
				}
			}
			if (uIResearchSlot2 != null && gamepadSelectedSlot != uIResearchSlot2)
			{
				gamepadSelectedSlot.OnPointerExitPad();
				gamepadSelectedSlot = uIResearchSlot2;
				gamepadSelectedSlot.OnPointerEnterPad();
				UpdateGamepadScrollRectPoint(slideDirectionDown: true);
			}
		}
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		if ((bool)UnlockResearchConfirmObj && UnlockResearchConfirmObj.activeInHierarchy)
		{
			ConfirmUnlockResearch();
		}
		else if ((GameMgr.IsMobile_Static || ICJNOGPFMAM.MIFJADDOODN) && UIMgr.Inst.InputType == PlayerInputType.Gamepad && base.IsOpen && (!gamepadSelectedSlot.IsResearched || gamepadSelectedSlot.Cfg.canDisactive))
		{
			SlotClick(gamepadSelectedSlot);
		}
	}

	private void LanguageChange()
	{
		text_Title.text = 1002101.GetText();
		for (int i = 0; i < uiSlots.Count; i++)
		{
			uiSlots[i].UpdateLanguage();
		}
	}

	private void InputChange()
	{
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			if (gamepadSelectedSlot != null)
			{
				gamepadSelectedSlot.OnPointerExit(null);
				gamepadSelectedSlot = null;
			}
			break;
		case PlayerInputType.Gamepad:
			gamepadSelectedSlot = uiLayout.transform.GetChild(0).GetComponent<UIResearchSlot>();
			gamepadSelectedSlot.SkipOnceSE();
			gamepadSelectedSlot.OnPointerEnter(null);
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
	}

	private void AncienBloodChange()
	{
		text_BloodCount.text = DataMgr.selectedWorldData.ancientBloodCount.ToString();
		for (int i = 0; i < uiSlots.Count; i++)
		{
			uiSlots[i].CheckCost();
		}
	}

	protected override IEnumerator OnInit()
	{
		yield return StartCoroutine(StartIE());
	}

	private IEnumerator StartIE()
	{
		uiLayout.transform.DestroyAllChild();
		foreach (ResearchConfig item in ResearchConfig.list)
		{
			bool flag = false;
			switch (item.openType)
			{
			case ResearchOpenType.Always:
				flag = true;
				break;
			case ResearchOpenType.ReachChapter2:
				if (DataMgr.selectedWorldData.isReachChatper2)
				{
					flag = true;
				}
				break;
			case ResearchOpenType.ReachChapter3:
				if (DataMgr.selectedWorldData.isReachChatper3)
				{
					flag = true;
				}
				break;
			case ResearchOpenType.ReachChapter4:
				if (DataMgr.selectedWorldData.isReachChatper4)
				{
					flag = true;
				}
				break;
			case ResearchOpenType.ReachChapter5:
				if (DataMgr.selectedWorldData.isReachChatper5)
				{
					flag = true;
				}
				break;
			case ResearchOpenType.FinishEasy:
				if (DataMgr.selectedWorldData.finishedDifficulty.Contains(DifficultyType.Easy))
				{
					flag = true;
				}
				break;
			case ResearchOpenType.FinishNormal:
				if (DataMgr.selectedWorldData.finishedDifficulty.Contains(DifficultyType.Normal))
				{
					flag = true;
				}
				break;
			case ResearchOpenType.FinishHard:
				if (DataMgr.selectedWorldData.finishedDifficulty.Contains(DifficultyType.Hard))
				{
					flag = true;
				}
				break;
			default:
				Debug.LogError(item.openType);
				break;
			}
			if (flag)
			{
				int num = ResearchConfig.HavePostResearch(item.id);
				bool flag2 = false;
				if (num != -1)
				{
					flag2 = DataMgr.selectedWorldData.researchedIDs.Contains(num);
				}
				if (!DataMgr.selectedWorldData.researchedIDs.Contains(item.id) || !(num != -1 && flag2))
				{
					UIResearchSlot uIResearchSlot = (GameMgr.IsMobile_Static ? UnityEngine.Object.Instantiate(pfb_UIResearchSlotMobile, uiLayout.transform).GetComponent<UIResearchSlot>() : UnityEngine.Object.Instantiate(pfb_UIResearchSlot, uiLayout.transform).GetComponent<UIResearchSlot>());
					uIResearchSlot.Initialize(this, item.id);
					uiSlots.Add(uIResearchSlot);
				}
			}
		}
		yield return null;
		OrderSlots();
		LanguageChange();
		InputChange();
		AncienBloodChange();
	}

	private void OrderSlots()
	{
		for (int i = 0; i < uiSlots.Count; i++)
		{
			if (uiSlots[i].IsResearched && uiSlots[i].Cfg.canDisactive)
			{
				uiSlots[i].transform.SetSiblingIndex(uiSlots.Count - 1);
			}
		}
		for (int j = 0; j < uiSlots.Count; j++)
		{
			if (uiSlots[j].IsResearched && !uiSlots[j].Cfg.canDisactive)
			{
				uiSlots[j].transform.SetSiblingIndex(uiSlots.Count - 1);
			}
		}
	}

	private void UpdateGamepadScrollRectPoint(bool slideDirectionDown)
	{
		int num = 0;
		int num2 = 0;
		bool flag = false;
		for (int i = 0; i < uiLayout.transform.childCount; i++)
		{
			Transform child = uiLayout.transform.GetChild(i);
			num++;
			if (!flag)
			{
				num2++;
				if (child == gamepadSelectedSlot.transform)
				{
					flag = true;
				}
			}
		}
		int currentRow = Mathf.CeilToInt(num2 / scrollRect.int_widthnum);
		if (scrollRect.gameObject.activeInHierarchy)
		{
			scrollRect.ScrollUpdate(currentRow, slideDirectionDown);
		}
	}

	protected override void OnShow(object obj = null)
	{
		demoBlockPlayer.gameObject.SetActive(!ICJNOGPFMAM.MIFJADDOODN && !GameMgr.IsMobile_Static);
		showFinish = false;
		researchChangeLogger = new ResearchChangeLogger
		{
			before_unlocked = DataMgr.selectedWorldData.researchedIDs.ToList()
		};
		researchChangeLogger.AutoRecordBeforeResources();
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		anima.SetTrigger("Appear");
		UIMgr.TryAdditionalMobileShow(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		SEMgr.Inst.uiOpen.PlaySE();
		UIPlayerDataMgr.Inst.ResourceUIPopUp(UIPlayerDataMgr.ResourceUIPop.Blood);
		if (UIMgr.Inst.InputType == PlayerInputType.Gamepad)
		{
			InitGamepadSelectedSlot(forceReset: true);
		}
	}

	private void InitGamepadSelectedSlot(bool forceReset)
	{
		if (uiLayout == null)
		{
			Debug.LogError("[UIResearch] uiLayout is null");
		}
		else
		{
			if (!forceReset)
			{
				return;
			}
			if (gamepadSelectedSlot != null)
			{
				if (!gamepadSelectedSlot.gameObject.activeInHierarchy)
				{
					gamepadSelectedSlot.gameObject.SetActive(value: true);
				}
				return;
			}
			Debug.Log("[UIResearch] Init gamepadSelectedSlot");
			Canvas.ForceUpdateCanvases();
			gamepadSelectedSlot = null;
			for (int i = 0; i < uiLayout.transform.childCount; i++)
			{
				Transform child = uiLayout.transform.GetChild(i);
				if (child.gameObject.activeInHierarchy)
				{
					UIResearchSlot component = child.GetComponent<UIResearchSlot>();
					if (!(component == null))
					{
						gamepadSelectedSlot = component;
						break;
					}
				}
			}
			if (uiLayout.transform.childCount > 0 && gamepadSelectedSlot == null)
			{
				gamepadSelectedSlot = uiLayout.transform.GetChild(0).GetComponent<UIResearchSlot>();
				gamepadSelectedSlot.gameObject.SetActive(value: true);
			}
			if (gamepadSelectedSlot == null)
			{
				Debug.LogError("[UIResearch] no active slot found");
				return;
			}
			gamepadSelectedSlot.SkipOnceSE();
			gamepadSelectedSlot.OnPointerEnterPad();
			UpdateGamepadScrollRectPoint(slideDirectionDown: false);
			Debug.Log("[UIResearch] Init gamepadSelectedSlot = " + gamepadSelectedSlot.name);
		}
	}

	protected override void OnHide()
	{
		showFinish = false;
		StopAllCoroutines();
		anima.SetTrigger("Disappear");
		UIMgr.TryAdditionalMobileHide(base.transform);
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		DataMgr.SaveSelectedWorldData();
		SEMgr.Inst.uiClose.PlaySE();
		researchChangeLogger.after_unlocked = DataMgr.selectedWorldData.researchedIDs.ToList();
		researchChangeLogger.AutoRecordAfterResourcesAndFlow();
		researchChangeLogger.Report();
		if (GameMgr.IsMobile_Static && !GeneralTool.ListContentEquals(researchChangeLogger.before_unlocked, researchChangeLogger.after_unlocked))
		{
			string properties = JsonConvert.SerializeObject(researchChangeLogger);
			PluginActivity.Inst.UploadEvent("research_change", properties);
		}
		UIPlayerDataMgr.Inst.ResourceUISetToDefault(UIPlayerDataMgr.ResourceUIPop.Blood);
	}

	public void SlotClick(UIResearchSlot slot)
	{
		StartCoroutine(SlotClickIE(slot));
	}

	private IEnumerator SlotClickIE(UIResearchSlot slot)
	{
		if (slot.IsResearched)
		{
			if (slot.Cfg.canDisactive)
			{
				if (slot.IsActive)
				{
					if (!DataMgr.selectedWorldData.researchDisactive.Contains(slot.Cfg.id))
					{
						DataMgr.selectedWorldData.researchDisactive.Add(slot.Cfg.id);
					}
				}
				else if (DataMgr.selectedWorldData.researchDisactive.Contains(slot.Cfg.id))
				{
					DataMgr.selectedWorldData.researchDisactive.Remove(slot.Cfg.id);
				}
				slot.UpdateState(anime: true);
				slot.UpdateLanguage();
			}
		}
		else if (slot.Cfg.cost <= DataMgr.selectedWorldData.ancientBloodCount)
		{
			if (GameMgr.IsMobile_Static)
			{
				OpenResearchConfirmPanel(slot);
			}
			else
			{
				UnlockStage1(slot);
				yield return null;
				UnlockStage2(slot);
			}
		}
		else
		{
			slot.anima.SetTrigger("NotEnough");
			SEMgr.Inst.uiResearchWrong.PlaySE();
		}
		yield return new WaitForEndOfFrame();
	}

	public void ShowFinish()
	{
		showFinish = true;
	}

	public override void _Close()
	{
		base._Close();
		SEMgr.Inst.uiClick.PlaySE();
	}
}
