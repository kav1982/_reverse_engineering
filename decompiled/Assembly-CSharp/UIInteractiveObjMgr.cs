using System;
using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInteractiveObjMgr : MonoBehaviour
{
	public GameObject panel_Interact;

	[HideInInspector]
	public UIPanelInteract uiPanelInteract;

	[HideInInspector]
	public UIInfoWand uiInfoWand;

	[HideInInspector]
	public UIInfoSpell uiInfoSpell;

	[HideInInspector]
	public UIInfoRelic uiInfoRelic;

	[HideInInspector]
	public UIResourceInfo uiInfoResource;

	[HideInInspector]
	public UIInfoPotion uiInfoPotion;

	[HideInInspector]
	public UIInfoCurse uiInfoCurse;

	[HideInInspector]
	public UIInfoGeneral uiInfoGeneral;

	[HideInInspector]
	public UIInfoGeneralPure uiInfoGeneralPure;

	private EntityManager ettMgr;

	private Entity currentInteractiveObjEtt;

	private Coroutine rectTsfUpdate;

	private InputActions inputActions;

	public static UIInteractiveObjMgr Inst { get; private set; }

	public int InteractableCount { get; set; }

	public void Initialize()
	{
		Inst = this;
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		uiPanelInteract = panel_Interact.GetComponentInChildren<UIPanelInteract>();
		uiPanelInteract.Init();
		uiInfoWand = panel_Interact.GetComponentInChildren<UIInfoWand>();
		uiInfoSpell = panel_Interact.GetComponentInChildren<UIInfoSpell>();
		uiInfoRelic = panel_Interact.GetComponentInChildren<UIInfoRelic>();
		uiInfoResource = panel_Interact.GetComponentInChildren<UIResourceInfo>();
		uiInfoPotion = panel_Interact.GetComponentInChildren<UIInfoPotion>();
		uiInfoCurse = panel_Interact.GetComponentInChildren<UIInfoCurse>();
		uiInfoGeneral = panel_Interact.GetComponentInChildren<UIInfoGeneral>();
		uiInfoGeneralPure = panel_Interact.GetComponentInChildren<UIInfoGeneralPure>();
		uiInfoWand.gameObject.SetActive(value: false);
		uiInfoSpell.gameObject.SetActive(value: false);
		uiInfoRelic.gameObject.SetActive(value: false);
		uiInfoResource.gameObject.SetActive(value: false);
		uiInfoPotion.gameObject.SetActive(value: false);
		uiInfoCurse.gameObject.SetActive(value: false);
		uiInfoGeneral.gameObject.SetActive(value: false);
		uiInfoGeneralPure.gameObject.SetActive(value: false);
	}

	private void OnEnable()
	{
		inputActions = ControlMgr.Inst.inputActions;
		inputActions.Player.Interact.performed += InteractPerformed;
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Combine(EventMgr.ControlChange, new Action(ControlChange));
	}

	private void OnDisable()
	{
		inputActions.Player.Interact.performed -= InteractPerformed;
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Remove(EventMgr.ControlChange, new Action(ControlChange));
	}

	private void InteractPerformed(InputAction.CallbackContext context)
	{
		Interact();
	}

	private void InputChange()
	{
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			if (!GameMgr.IsMobile_Static)
			{
				uiPanelInteract.panel_InteractBtnShow.UpdateButton();
			}
			break;
		case PlayerInputType.Gamepad:
			if (!GameMgr.IsMobile_Static)
			{
				uiPanelInteract.panel_InteractBtnShow.UpdateButton();
			}
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
	}

	private void ControlChange()
	{
		if (!GameMgr.IsMobile_Static)
		{
			uiPanelInteract.panel_InteractBtnShow.UpdateButton();
		}
	}

	private void Update()
	{
		UIFollow();
	}

	private void UIFollow()
	{
		if (!panel_Interact.activeSelf)
		{
			return;
		}
		if (currentInteractiveObjEtt != Entity.Null && ettMgr.Exists(currentInteractiveObjEtt) && ettMgr.HasComponent<InteractiveObj_Dots>(currentInteractiveObjEtt))
		{
			InteractiveObj_Dots componentData = ettMgr.GetComponentData<InteractiveObj_Dots>(currentInteractiveObjEtt);
			LocalTransform componentData2 = ettMgr.GetComponentData<LocalTransform>(currentInteractiveObjEtt);
			if (ettMgr.HasComponent<Item>(currentInteractiveObjEtt) && ettMgr.GetComponentData<Item>(currentInteractiveObjEtt).info.type == ItemType.Wand && ettMgr.GetComponentData<Item>(currentInteractiveObjEtt).isStore)
			{
				panel_Interact.transform.localPosition = GeneralTool.WorldToCanvasLocalPoint_FitDisplay(componentData2.Position + componentData.uiOffset + new float3(2f, 0f, 0f));
			}
			else
			{
				panel_Interact.transform.localPosition = GeneralTool.WorldToCanvasLocalPoint_FitDisplay(componentData2.Position + componentData.uiOffset);
			}
		}
		else
		{
			panel_Interact.SetActive(value: false);
		}
		UIMgr.InteractiveFollowFitChild(panel_Interact);
	}

	public void InteractiveObjCheck(Entity ett)
	{
		if (ett == Entity.Null || !ettMgr.Exists(ett) || !ettMgr.HasComponent<InteractiveObj_Dots>(ett))
		{
			if (GameMgr.IsMobile_Static && !DataMgr.settingData.Mobiledata.indieInteractButton)
			{
				MobileMgr.inst.UpdateActiveButton(MobileMgr.InteractState.Shoot);
			}
			if (panel_Interact.activeSelf)
			{
				panel_Interact.SetActive(value: false);
			}
			if (currentInteractiveObjEtt != Entity.Null && ettMgr.Exists(currentInteractiveObjEtt) && ettMgr.HasComponent<InteractiveObj_Dots>(currentInteractiveObjEtt))
			{
				InteractiveObj_Dots componentData = ettMgr.GetComponentData<InteractiveObj_Dots>(currentInteractiveObjEtt);
				componentData.onDeselect = true;
				ettMgr.SetComponentData(currentInteractiveObjEtt, componentData);
				currentInteractiveObjEtt = Entity.Null;
			}
		}
		else
		{
			if (currentInteractiveObjEtt == ett)
			{
				return;
			}
			if (currentInteractiveObjEtt != Entity.Null && ettMgr.Exists(currentInteractiveObjEtt) && ettMgr.HasComponent<InteractiveObj_Dots>(currentInteractiveObjEtt))
			{
				InteractiveObj_Dots componentData2 = ettMgr.GetComponentData<InteractiveObj_Dots>(currentInteractiveObjEtt);
				componentData2.onDeselect = true;
				ettMgr.SetComponentData(currentInteractiveObjEtt, componentData2);
			}
			currentInteractiveObjEtt = ett;
			InteractiveObj_Dots componentData3 = ettMgr.GetComponentData<InteractiveObj_Dots>(currentInteractiveObjEtt);
			componentData3.onSelect = true;
			ettMgr.SetComponentData(currentInteractiveObjEtt, componentData3);
			InteractiveObj_Dots componentData4 = ettMgr.GetComponentData<InteractiveObj_Dots>(currentInteractiveObjEtt);
			uiPanelInteract.panel_InteractBtn.SetActive(value: true);
			panel_Interact.SetActive(value: true);
			panel_Interact.GetComponent<CanvasGroup>().alpha = 0f;
			if (GameMgr.IsMobile_Static)
			{
				if (componentData4.type == InteractiveObjType.NPC1Vivian || componentData4.type == InteractiveObjType.NPC2Nimue || componentData4.type == InteractiveObjType.NPC3 || componentData4.type == InteractiveObjType.NPC4 || componentData4.type == InteractiveObjType.NPC4_Trapped || componentData4.type == InteractiveObjType.NPC5 || componentData4.type == InteractiveObjType.NPC5_Trapped || componentData4.type == InteractiveObjType.NPC6 || componentData4.type == InteractiveObjType.NPC7)
				{
					MobileMgr.inst.ActiveButtonInteract(MobileMgr.InteractState.Talk);
				}
				else
				{
					MobileMgr.inst.ActiveButtonInteract(MobileMgr.InteractState.Other);
				}
			}
			uiInfoWand.gameObject.SetActive(value: false);
			uiInfoSpell.gameObject.SetActive(value: false);
			uiInfoRelic.gameObject.SetActive(value: false);
			uiInfoResource.gameObject.SetActive(value: false);
			uiInfoPotion.gameObject.SetActive(value: false);
			uiInfoCurse.gameObject.SetActive(value: false);
			uiInfoGeneral.gameObject.SetActive(value: false);
			uiInfoGeneralPure.gameObject.SetActive(value: false);
			uiPanelInteract.panel_InteractBtnMask.gameObject.SetActive(value: false);
			switch (componentData4.type)
			{
			case InteractiveObjType.Item:
			{
				Item componentData9 = ettMgr.GetComponentData<Item>(currentInteractiveObjEtt);
				if (componentData9.isStore)
				{
					uiPanelInteract.text_InteractButton.text = 1001501.GetText();
					if (componentData9.itemMono.Value.go_Canvas.activeSelf)
					{
						if (componentData9.info.type == ItemType.Relic)
						{
							if (RelicConfig.dic[componentData9.info.id].dropType == ItemDropType.Epic)
							{
								PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt);
								if (playerPpt.unitCfg.maxHP <= (float)componentData9.GetFinalPrice())
								{
									uiPanelInteract.panel_InteractBtnMask.SetActive(value: true);
									uiPanelInteract.SetText(1001309.GetText());
								}
							}
						}
						else if (PlayerMgr.Inst.BaData.coinCount < componentData9.GetFinalPrice())
						{
							uiPanelInteract.panel_InteractBtnMask.SetActive(value: true);
							uiPanelInteract.SetText(1001310.GetText());
						}
					}
					else if (componentData9.curseID != 0)
					{
						uiInfoCurse.gameObject.SetActive(value: true);
						uiInfoCurse.UpdateInfo(componentData9.curseID);
					}
					else
					{
						Debug.LogError("不应该会出现，既有价格，又不是稀有诅咒的情况");
					}
				}
				else
				{
					uiPanelInteract.text_InteractButton.text = 1001502.GetText();
				}
				switch (componentData9.info.type)
				{
				case ItemType.Wand:
				{
					uiInfoWand.gameObject.SetActive(value: true);
					WandConfig cfg = ettMgr.GetComponentObject<WandConfigComponent>(ett).cfg;
					if (componentData9.isStore)
					{
						uiInfoWand.UpdateInfo(cfg, uiInfoCurse.gameObject.activeSelf ? uiInfoCurse.rtsf_Self : null, ItemIsStore: true);
					}
					else
					{
						uiInfoWand.UpdateInfo(cfg, uiInfoCurse.gameObject.activeSelf ? uiInfoCurse.rtsf_Self : null);
					}
					break;
				}
				case ItemType.Spell:
					uiInfoSpell.gameObject.SetActive(value: true);
					uiInfoSpell.UpdateInfo(new SlotData(componentData9.info.id, componentData9.info.specialInt), uiInfoCurse.gameObject.activeSelf ? uiInfoCurse.rtsf_Self : null);
					break;
				case ItemType.Relic:
				{
					uiInfoRelic.gameObject.SetActive(value: true);
					RelicConfig config = RelicConfig.GetConfig(componentData9.info.id);
					RelicConfig relicConfig = PlayerMgr.Inst.ItemCtrller.GetRelicConfig(componentData9.info.id);
					if (relicConfig != null)
					{
						config.level = relicConfig.level + 1;
					}
					uiInfoRelic.UpdateInfo(config, uiInfoCurse.gameObject.activeSelf ? uiInfoCurse.rtsf_Self : null, upgrade: true);
					break;
				}
				case ItemType.Potion:
					uiInfoPotion.gameObject.SetActive(value: true);
					uiInfoPotion.UpdateInfo(componentData9.info.id, uiInfoCurse.gameObject.activeSelf ? uiInfoCurse.rtsf_Self : null);
					break;
				case ItemType.Resource:
				case ItemType.RuneWizardRune:
				case ItemType.MaxHp:
					uiInfoResource.gameObject.SetActive(value: true);
					uiInfoResource.UpdateInfo(componentData9.info.id, uiInfoCurse.gameObject.activeSelf ? uiInfoCurse.rtsf_Self : null);
					break;
				case ItemType.Curse:
					Debug.LogError("理论上不应该有可以交互诅咒");
					break;
				default:
					Debug.LogError(componentData9.info.type);
					break;
				}
				break;
			}
			case InteractiveObjType.NPC1Vivian:
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				uiInfoGeneralPure.UpdateInfo(1005011.GetText());
				uiPanelInteract.text_InteractButton.text = 1001507.GetText();
				break;
			case InteractiveObjType.NPC2Nimue:
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				uiInfoGeneralPure.UpdateInfo(1005021.GetText());
				uiPanelInteract.text_InteractButton.text = 1001507.GetText();
				break;
			case InteractiveObjType.NPC3:
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				uiInfoGeneralPure.UpdateInfo(1005031.GetText());
				uiPanelInteract.text_InteractButton.text = 1001507.GetText();
				break;
			case InteractiveObjType.NPC4_Trapped:
			case InteractiveObjType.NPC4:
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				uiInfoGeneralPure.UpdateInfo(1005041.GetText());
				uiPanelInteract.text_InteractButton.text = 1001507.GetText();
				break;
			case InteractiveObjType.NPC5_Trapped:
			case InteractiveObjType.NPC5:
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				uiInfoGeneralPure.UpdateInfo(1005051.GetText());
				uiPanelInteract.text_InteractButton.text = 1001507.GetText();
				break;
			case InteractiveObjType.NPC6:
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				uiInfoGeneralPure.UpdateInfo(1005061.GetText());
				uiPanelInteract.text_InteractButton.text = 1001507.GetText();
				break;
			case InteractiveObjType.NPC7:
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				uiInfoGeneralPure.UpdateInfo(1005071.GetText());
				uiPanelInteract.text_InteractButton.text = 1001507.GetText();
				break;
			case InteractiveObjType.NPC9_EndlessGuide:
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				uiInfoGeneralPure.UpdateInfo(1005091.GetText());
				uiPanelInteract.text_InteractButton.text = 1001507.GetText();
				break;
			case InteractiveObjType.Boss99Interaction:
			case InteractiveObjType.Boss99InteractionBuyGame:
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				uiInfoGeneralPure.UpdateInfo(1005003.GetText());
				uiPanelInteract.text_InteractButton.text = 1001507.GetText();
				break;
			case InteractiveObjType.AccessBase:
				if (ettMgr.GetComponentData<AccessBase_Dots>(currentInteractiveObjEtt).needKey)
				{
					uiPanelInteract.text_InteractButton.text = 1001506.GetText();
					if (!PlayerMgr.Inst.IsKeyEnough())
					{
						uiPanelInteract.panel_InteractBtnMask.SetActive(value: true);
						uiPanelInteract.SetText(1001311.GetText());
					}
				}
				else
				{
					Debug.LogError("为什么不需要钥匙的门，而又能交互");
				}
				break;
			case InteractiveObjType.DoorBase:
			{
				uiPanelInteract.text_InteractButton.text = 1001503.GetText();
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				if (PlayerMgr.Inst.ItemCtrller.curse_IsInvisibleDoor)
				{
					uiInfoGeneralPure.UpdateInfo("?");
					break;
				}
				DoorBase_Dots componentData11 = ettMgr.GetComponentData<DoorBase_Dots>(currentInteractiveObjEtt);
				switch (componentData11.rewardType)
				{
				case LevelRewardType.Spell:
					uiInfoGeneralPure.UpdateInfo(1001402.GetText());
					break;
				case LevelRewardType.Relic:
					uiInfoGeneralPure.UpdateInfo(1001403.GetText());
					break;
				case LevelRewardType.MaxHP:
					uiInfoGeneralPure.UpdateInfo(1001404.GetText());
					break;
				case LevelRewardType.Coin:
					uiInfoGeneralPure.UpdateInfo(1001405.GetText());
					break;
				case LevelRewardType.Store:
					uiInfoGeneralPure.UpdateInfo(1001406.GetText());
					break;
				case LevelRewardType.Process:
					uiInfoGeneralPure.UpdateInfo(1001407.GetText());
					break;
				case LevelRewardType.Spring:
					uiInfoGeneralPure.UpdateInfo(1001411.GetText());
					break;
				case LevelRewardType.Elite:
					uiInfoGeneralPure.UpdateInfo(1001408.GetText());
					break;
				case LevelRewardType.Boss:
					uiInfoGeneralPure.UpdateInfo(1001409.GetText());
					break;
				case LevelRewardType.Chapter:
					uiInfoGeneralPure.UpdateInfo(1001410.GetText());
					break;
				case LevelRewardType.Shortcut:
					uiInfoGeneralPure.UpdateInfo(1001412.GetText());
					break;
				case LevelRewardType.None:
					uiInfoGeneralPure.gameObject.SetActive(value: false);
					break;
				default:
					Debug.LogError(componentData11.rewardType);
					uiInfoGeneralPure.UpdateInfo("");
					break;
				}
				break;
			}
			case InteractiveObjType.Door_Camp:
			case InteractiveObjType.Door_Camp_Guide:
				uiPanelInteract.text_InteractButton.text = 1001503.GetText();
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				uiInfoGeneralPure.UpdateInfo(1001401.GetText());
				break;
			case InteractiveObjType.SpecialObj4:
			{
				SpecialObj4_Dots componentData10 = ettMgr.GetComponentData<SpecialObj4_Dots>(currentInteractiveObjEtt);
				switch (componentData10.chestType)
				{
				case ChestType.Lock:
					if (!PlayerMgr.Inst.IsKeyEnough())
					{
						uiPanelInteract.panel_InteractBtnMask.SetActive(value: true);
						uiPanelInteract.SetText(1001311.GetText());
					}
					break;
				case ChestType.Curse:
					uiInfoCurse.gameObject.SetActive(value: true);
					uiInfoCurse.UpdateInfo(componentData10.curseID);
					uiInfoCurse.rtsf_Self.anchoredPosition = uiInfoSpell.rtsf_Self.anchoredPosition;
					break;
				default:
					Debug.LogError(componentData10.chestType);
					break;
				case ChestType.NoLock:
				case ChestType.Spike:
					break;
				}
				uiPanelInteract.text_InteractButton.text = 1001506.GetText();
				break;
			}
			case InteractiveObjType.Toilet:
				uiPanelInteract.text_InteractButton.text = 1001504.GetText();
				break;
			case InteractiveObjType.RankingList:
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				uiInfoGeneralPure.UpdateInfo(1003101.GetText());
				uiPanelInteract.text_InteractButton.text = 1001505.GetText();
				break;
			case InteractiveObjType.LevelReward:
			case InteractiveObjType.BedroomDoor:
			case InteractiveObjType.WandInStone:
			case InteractiveObjType.Gallery:
			case InteractiveObjType.ResourceChanger:
			case InteractiveObjType.BattleFinishDrop:
			case InteractiveObjType.CampMirror:
			case InteractiveObjType.SpecialObj40:
			case InteractiveObjType.SpecialObj212Book:
			case InteractiveObjType.ToEndlessCampTeleporter:
			case InteractiveObjType.ToNormalCampTeleporter:
			case InteractiveObjType.CampSkinChanger:
			case InteractiveObjType.EndlessRankingList:
			case InteractiveObjType.EndlessGallery:
				uiPanelInteract.text_InteractButton.text = 1001505.GetText();
				break;
			case InteractiveObjType.SO101Reroll:
			{
				uiPanelInteract.text_InteractButton.text = 1001504.GetText();
				uiInfoGeneral.gameObject.SetActive(value: true);
				string text2 = 1001305.GetText();
				for (int i = 0; i < DataMgr.selectedWorldData.researchedIDs.Count; i++)
				{
					if (ResearchConfig.dic[DataMgr.selectedWorldData.researchedIDs[i]].abilityType == ResearchAbilityType.ProcessReroll)
					{
						text2 += "+";
					}
				}
				uiInfoGeneral.UpdateInfo(text2, 1001306.GetText());
				break;
			}
			case InteractiveObjType.SO101Compound:
				uiPanelInteract.text_InteractButton.text = 1001504.GetText();
				uiInfoGeneral.gameObject.SetActive(value: true);
				uiInfoGeneral.UpdateInfo(1001303.GetText(), 1001304.GetText());
				break;
			case InteractiveObjType.SO101MoreInOne:
				uiPanelInteract.text_InteractButton.text = 1001504.GetText();
				uiInfoGeneral.gameObject.SetActive(value: true);
				uiInfoGeneral.UpdateInfo(1001307.GetText(), 1001308.GetText());
				break;
			case InteractiveObjType.SpecialObj10:
			{
				uiPanelInteract.text_InteractButton.text = 1001504.GetText();
				uiInfoGeneral.gameObject.SetActive(value: true);
				if (!GameMgr.IsMobile_Static)
				{
					uiInfoGeneral.UpdateInfo(1001301.GetText(), 1001302.GetText());
				}
				else
				{
					uiInfoGeneral.UpdateInfo(1001319.GetText(), 1001302.GetText());
				}
				SpecialObj10_Dots componentData8 = ettMgr.GetComponentData<SpecialObj10_Dots>(currentInteractiveObjEtt);
				if (!componentData8.IsPlayerHaveCurse())
				{
					uiPanelInteract.panel_InteractBtnMask.SetActive(value: true);
					uiPanelInteract.SetText(1001312.GetText());
				}
				else if (!componentData8.IsHpAndShieldEnoughToBuy())
				{
					uiPanelInteract.panel_InteractBtnMask.SetActive(value: true);
					uiPanelInteract.SetText(1002503.GetText());
				}
				break;
			}
			case InteractiveObjType.SpecialObj17:
			{
				uiPanelInteract.text_InteractButton.text = 1001504.GetText();
				uiInfoGeneral.gameObject.SetActive(value: true);
				SpecialObj17_Dots componentData7 = ettMgr.GetComponentData<SpecialObj17_Dots>(currentInteractiveObjEtt);
				uiInfoGeneral.UpdateInfo(componentData7.GetName(), componentData7.GetDesc());
				break;
			}
			case InteractiveObjType.SpecialObj46:
			{
				uiPanelInteract.text_InteractButton.text = 1001504.GetText();
				uiInfoGeneral.gameObject.SetActive(value: true);
				SpecialObj46 componentData6 = ettMgr.GetComponentData<SpecialObj46>(currentInteractiveObjEtt);
				uiInfoGeneral.UpdateInfo(componentData6.GetName(), componentData6.GetDesc());
				break;
			}
			case InteractiveObjType.SpecialObj21:
			{
				uiPanelInteract.text_InteractButton.text = 1001504.GetText();
				uiInfoGeneral.gameObject.SetActive(value: true);
				SpecialObj21_Dots componentData5 = ettMgr.GetComponentData<SpecialObj21_Dots>(currentInteractiveObjEtt);
				uiInfoGeneral.UpdateInfo(componentData5.GetName(), componentData5.GetDesc());
				break;
			}
			case InteractiveObjType.SpecialObj18:
				uiPanelInteract.text_InteractButton.text = 1001504.GetText();
				uiInfoGeneral.gameObject.SetActive(value: true);
				uiInfoGeneral.UpdateInfo(1001315.GetText(), 1001316.GetText());
				break;
			case InteractiveObjType.GiftSet:
				uiPanelInteract.text_InteractButton.text = 1006305.GetText();
				break;
			case InteractiveObjType.BackCampPortal:
				uiPanelInteract.text_InteractButton.text = 1000204.GetText();
				break;
			case InteractiveObjType.SpecialObj217_Handle:
				uiPanelInteract.text_InteractButton.text = 1001504.GetText();
				if (!SpecialObj217.Inst.HPAndShiledEnough)
				{
					uiPanelInteract.SetText(1002503.GetText());
				}
				break;
			case InteractiveObjType.SpecialObj222_PayInteract:
				uiPanelInteract.text_InteractButton.text = 1001504.GetText();
				if (!SpecialObj222_PayInteract.Inst.HPAndShiledEnough)
				{
					uiPanelInteract.SetText(1002503.GetText());
				}
				break;
			case InteractiveObjType.DoorEndlessCamp:
				uiPanelInteract.text_InteractButton.text = 1001503.GetText();
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				uiInfoGeneralPure.UpdateInfo(1001414.GetText());
				break;
			case InteractiveObjType.SpecialObj301EndlessMonsterSpawner:
				uiPanelInteract.text_InteractButton.text = 1001503.GetText();
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				uiInfoGeneralPure.UpdateInfo(1001323.GetText());
				break;
			case InteractiveObjType.SpecialObj301EndlessMonsterSpawnerLevel0:
				uiPanelInteract.text_InteractButton.text = 1001503.GetText();
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				uiInfoGeneralPure.UpdateInfo(1001326.GetText());
				break;
			case InteractiveObjType.SpecialObj309EndlessSideTeleporter:
				uiPanelInteract.text_InteractButton.text = 1001503.GetText();
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				uiInfoGeneralPure.UpdateInfo(1001406.GetText());
				break;
			case InteractiveObjType.SpecialObj309EndlessSideTeleporterToBattle:
				uiPanelInteract.text_InteractButton.text = 1001503.GetText();
				uiInfoGeneralPure.gameObject.SetActive(value: true);
				uiInfoGeneralPure.UpdateInfo(1001413.GetText());
				break;
			case InteractiveObjType.SpecialObj313EndlessStoreLocker:
				uiPanelInteract.text_InteractButton.text = 1001508.GetText();
				uiInfoGeneral.gameObject.SetActive(value: true);
				uiInfoGeneral.UpdateInfo(1001324.GetText(), 1001325.GetText());
				break;
			case InteractiveObjType.SpecialObj313EndlessStoreLockerUnlock:
				uiPanelInteract.text_InteractButton.text = 1001509.GetText();
				uiInfoGeneral.gameObject.SetActive(value: true);
				uiInfoGeneral.UpdateInfo(1001324.GetText(), 1001325.GetText());
				break;
			case InteractiveObjType.SpecialObj304EndlessSpellSeller:
				uiPanelInteract.text_InteractButton.text = 1001509.GetText();
				uiInfoGeneral.gameObject.SetActive(value: true);
				uiInfoGeneral.UpdateInfo(1001328.GetText(), 1001329.GetText());
				break;
			case InteractiveObjType.SO306EndlessReroll:
			{
				uiPanelInteract.text_InteractButton.text = 1001504.GetText();
				uiInfoGeneral.gameObject.SetActive(value: true);
				string text = 1001331.GetText();
				uiInfoGeneral.UpdateInfo(text, 1001306.GetText());
				break;
			}
			case InteractiveObjType.SO305EndlessCompound:
				uiPanelInteract.text_InteractButton.text = 1001504.GetText();
				uiInfoGeneral.gameObject.SetActive(value: true);
				uiInfoGeneral.UpdateInfo(1001330.GetText(), 1001304.GetText());
				break;
			default:
				Debug.LogError("交互了什么？" + componentData4.type);
				break;
			}
			uiPanelInteract.UpdateRect();
			if (rectTsfUpdate == null)
			{
				rectTsfUpdate = StartCoroutine(WaitShowUIInteract());
				return;
			}
			StopAllCoroutines();
			rectTsfUpdate = StartCoroutine(WaitShowUIInteract());
		}
	}

	private IEnumerator WaitShowUIInteract()
	{
		yield return new WaitForEndOfFrame();
		panel_Interact.GetComponent<CanvasGroup>().alpha = 1f;
		rectTsfUpdate = null;
	}

	public void Interact()
	{
		if (currentInteractiveObjEtt != Entity.Null && ettMgr.Exists(currentInteractiveObjEtt) && ettMgr.HasComponent<InteractiveObj_Dots>(currentInteractiveObjEtt))
		{
			InteractiveObj_Dots componentData = ettMgr.GetComponentData<InteractiveObj_Dots>(currentInteractiveObjEtt);
			componentData.onDeselect = true;
			componentData.onInteract = true;
			ettMgr.SetComponentData(currentInteractiveObjEtt, componentData);
			uiPanelInteract.panel_InteractBtn.SetActive(value: false);
			currentInteractiveObjEtt = Entity.Null;
		}
	}

	public void Reset()
	{
		currentInteractiveObjEtt = Entity.Null;
		panel_Interact.SetActive(value: false);
	}
}
