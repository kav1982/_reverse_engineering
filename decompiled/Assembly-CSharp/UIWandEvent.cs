using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIWandEvent : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	public UIWand uiWand;

	private bool canDrag = true;

	public bool isFromBuild;

	[Header("手游")]
	private Vector3 pointedInPosition;

	public Canvas Canvas;

	public GraphicRaycaster graphicRaycaster;

	private bool isDragging => UIPlayerDataMgr.Inst.uiWand_Drag == uiWand;

	private void Awake()
	{
		if (GameMgr.IsMobile_Static)
		{
			UIGamePadNav uIGamePadNav = base.transform.AddComponent<UIGamePadNav>();
			uIGamePadNav.OnDeselectAction = (Action<PointerEventData>)Delegate.Combine(uIGamePadNav.OnDeselectAction, new Action<PointerEventData>(OnPointerExit));
			UIGamePadNav uIGamePadNav2 = base.transform.AddComponent<UIGamePadNav>();
			uIGamePadNav2.OnSelectAction = (Action<PointerEventData>)Delegate.Combine(uIGamePadNav2.OnSelectAction, new Action<PointerEventData>(OnPointerEnter));
		}
		else
		{
			UIGamePadNav component = base.transform.GetComponent<UIGamePadNav>();
			component.OnDeselectAction = (Action<PointerEventData>)Delegate.Combine(component.OnDeselectAction, new Action<PointerEventData>(OnPointerExit));
			UIGamePadNav component2 = base.transform.GetComponent<UIGamePadNav>();
			component2.OnSelectAction = (Action<PointerEventData>)Delegate.Combine(component2.OnSelectAction, new Action<PointerEventData>(OnPointerEnter));
		}
	}

	private void Update()
	{
		if (GameMgr.IsMobile_Static && (bool)Canvas)
		{
			if (((bool)PlayerMgr.Inst.PlayerCtrller && PlayerMgr.Inst.PlayerCtrller.CanMotion) || UIPlayerDataMgr.Inst.IsBagOpen)
			{
				Canvas.sortingOrder = 20;
			}
			else if (GameUISingletonMono<UILevelReward>.StaticIsOpen && GameUISingletonMono<UILevelReward>.Inst.type == LevelRewardType.Wand && GameUISingletonMono<UILevelReward>.Inst.isShowingWand)
			{
				Canvas.sortingOrder = 20;
			}
			else
			{
				Canvas.sortingOrder = 1;
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData != null)
		{
			pointedInPosition = eventData.position;
		}
		if (isFromBuild)
		{
			UIPlayerDataMgr.Inst.UIWandEventEnterBuild(uiWand, uiWand.WandCfgFromBuild);
		}
		else
		{
			UIPlayerDataMgr.Inst.UIWandEventEnter(uiWand);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		UIPlayerDataMgr.Inst.UIWandEventExit(uiWand);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static && canDrag)
		{
			UIPlayerDataMgr.Inst.UIWandEventDragBegin(uiWand);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (GameMgr.IsMobile_Static)
		{
			if (Vector3.Distance(pointedInPosition, (Vector3)eventData.position) > base.transform.parent.transform.localScale.x * 50f / 3f && !isDragging && canDrag)
			{
				UIPlayerDataMgr.Inst.UIWandEventDragBegin(uiWand);
			}
			UIPlayerDataMgr.Inst.UpdateDropAreaHighLight(eventData);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (canDrag)
		{
			UIPlayerDataMgr.Inst.UIWandEventDragEnd();
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (UIPlayerDataMgr.Inst.IsDraging || ((bool)UIBattleMgr.Inst && UIBattleMgr.Inst.uiFinishBuildShow.IsOpen) || ((bool)UICampMgr.Inst && GameUISingletonMono<UI_RankingList>.StaticIsOpen) || uiWand.WandCfg == null)
		{
			return;
		}
		if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl))
		{
			WandConfig wandCfg = uiWand.WandCfg;
			int index = 0;
			for (int i = 0; i < wandCfg.normalSlots.Length; i++)
			{
				if (wandCfg.normalSlots[i] == null || wandCfg.normalSlotIsLock[i])
				{
					continue;
				}
				if (index < PlayerMgr.Inst.BaData.bagSpellDatas.Count)
				{
					FindNextEmptyBagSlot(ref index);
				}
				if (index < PlayerMgr.Inst.BaData.bagSpellDatas.Count)
				{
					int id = wandCfg.normalSlots[i].id;
					PlayerMgr.Inst.BagSpellChange(index, wandCfg.normalSlots[i]);
					PlayerMgr.Inst.ChangeWandSpell(uiWand.WandIndex, WandSlotType.Normal, i, null);
					UISlotWand uISlot = uiWand.GetUISlot(WandSlotType.Normal, i);
					if ((bool)uISlot)
					{
						ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UISpellFly", uISlot.transform.position).GetComponent<UISpellFly>().Initialize(id, UIPlayerDataMgr.Inst.GetUISlotBag(index));
					}
				}
			}
			for (int j = 0; j < wandCfg.postSlots.Length; j++)
			{
				if (wandCfg.postSlots[j] == null || wandCfg.postSlotIsLock[j])
				{
					continue;
				}
				if (index < PlayerMgr.Inst.BaData.bagSpellDatas.Count)
				{
					FindNextEmptyBagSlot(ref index);
				}
				if (index < PlayerMgr.Inst.BaData.bagSpellDatas.Count)
				{
					int id2 = wandCfg.postSlots[j].id;
					PlayerMgr.Inst.BagSpellChange(index, wandCfg.postSlots[j]);
					PlayerMgr.Inst.ChangeWandSpell(uiWand.WandIndex, WandSlotType.Post, j, null);
					UISlotWand uISlot2 = uiWand.GetUISlot(WandSlotType.Post, j);
					if ((bool)uISlot2)
					{
						ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UISpellFly", uISlot2.transform.position).GetComponent<UISpellFly>().Initialize(id2, UIPlayerDataMgr.Inst.GetUISlotBag(index));
					}
				}
			}
			PlayerMgr.Inst.SelectedWand.ResetAndRecheck();
		}
		else
		{
			PlayerMgr.Inst.WandSelect(uiWand.WandIndex);
		}
	}

	public void FindNextEmptyBagSlot(ref int index)
	{
		for (int i = index; i <= PlayerMgr.Inst.BaData.bagSpellDatas.Count; i++)
		{
			index = i;
			if (i < PlayerMgr.Inst.BaData.bagSpellDatas.Count && PlayerMgr.Inst.BaData.bagSpellDatas[i] == null)
			{
				break;
			}
		}
	}

	public void SetDrag(bool drag)
	{
		canDrag = drag;
	}
}
