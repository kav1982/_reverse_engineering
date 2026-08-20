using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIPotionsController : MonoBehaviour
{
	public static UIPotionsController Inst;

	public GameObject pfb_UISlotPotion;

	public RectTransform rtsf_Self;

	public UIInfoPotion InfoUI;

	public UpdatButtonShow potionMoveLeftBtnShow;

	public UpdatButtonShow potionMoveRightBtnShow;

	public Vector2 originalLocalPoint;

	public float space;

	private readonly List<UISlotPotion> uiSlotPotions = new List<UISlotPotion>();

	private int _lastUpdateSelectIndex = -1;

	private int _lastUpdateSelectId = -1;

	private float rightBtnSrcPosX;

	private Sequence _infoPanelSequence;

	private float _hideInfoUITimer;

	private bool _needToShow;

	private void Awake()
	{
		Inst = this;
		InfoUI.gameObject.SetActive(value: false);
		InfoUI.GetComponent<CanvasGroup>().alpha = 0f;
		if (potionMoveRightBtnShow != null)
		{
			rightBtnSrcPosX = potionMoveRightBtnShow.GetComponent<RectTransform>().anchoredPosition.x;
		}
		EventMgr.ControlChange = (Action)Delegate.Combine(EventMgr.ControlChange, new Action(ControlChange));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(ControlChange));
	}

	private void OnDestroy()
	{
		EventMgr.ControlChange = (Action)Delegate.Remove(EventMgr.ControlChange, new Action(ControlChange));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(ControlChange));
	}

	private void Update()
	{
		if (_hideInfoUITimer > 0f)
		{
			_hideInfoUITimer -= Time.deltaTime;
			if (_hideInfoUITimer <= 0f)
			{
				CloseInfoPanel();
			}
		}
	}

	public void ControlChange()
	{
		if (potionMoveLeftBtnShow != null)
		{
			potionMoveLeftBtnShow.gameObject.SetActive(uiSlotPotions.Count > 1 && !GameMgr.IsMobile_Static);
			potionMoveLeftBtnShow.UpdateButton();
		}
		if (potionMoveRightBtnShow != null)
		{
			potionMoveRightBtnShow.gameObject.SetActive(uiSlotPotions.Count > 1 && !GameMgr.IsMobile_Static);
			potionMoveRightBtnShow.UpdateButton();
		}
	}

	public void CheckCountAndUpdateAllUI()
	{
		if (uiSlotPotions.Count != PlayerMgr.Inst.BaData.potionMaxCount)
		{
			base.transform.DestroyChildByName(pfb_UISlotPotion.name + "(Clone)");
			uiSlotPotions.Clear();
			for (int i = 0; i < PlayerMgr.Inst.BaData.potionMaxCount; i++)
			{
				UISlotPotion component = UnityEngine.Object.Instantiate(pfb_UISlotPotion, base.transform).GetComponent<UISlotPotion>();
				component.Initialize(i);
				uiSlotPotions.Add(component);
				component.rtsf_Self.anchoredPosition = new Vector2(space * (float)i, 0f);
			}
			if (potionMoveRightBtnShow != null)
			{
				RectTransform component2 = potionMoveRightBtnShow.GetComponent<RectTransform>();
				component2.anchoredPosition = new Vector3(rightBtnSrcPosX + space * (float)(PlayerMgr.Inst.BaData.potionMaxCount - 1), component2.anchoredPosition.y);
			}
			if (PlayerMgr.Inst.BaData.potionMaxCount > 0)
			{
				rtsf_Self.anchoredPosition = originalLocalPoint - new Vector2((float)(PlayerMgr.Inst.BaData.potionMaxCount - 1) * space, 0f);
			}
		}
		PlayerMgr.Inst.ItemCtrller.PotionSelectFirst();
		UpdateAllUI();
	}

	public void UpdateAllUI(bool showInfoPanel = true)
	{
		if (GameMgr.IsMobile_Static)
		{
			TopUI.inst.UpdateMobilePositionUI();
			TopUI.inst.UpdateMobileWandUI();
		}
		potionMoveLeftBtnShow?.gameObject.SetActive(uiSlotPotions.Count > 1 && !GameMgr.IsMobile_Static);
		potionMoveRightBtnShow?.gameObject.SetActive(uiSlotPotions.Count > 1 && !GameMgr.IsMobile_Static);
		bool flag = false;
		for (int i = 0; i < uiSlotPotions.Count; i++)
		{
			UISlotPotion uISlotPotion = uiSlotPotions[i];
			uISlotPotion.UpdateInfo();
			if (PlayerMgr.Inst.ItemCtrller.SelectedPotionIndex == i && uISlotPotion.ID != 0)
			{
				flag = true;
				uISlotPotion.BigDirect();
				uISlotPotion.transform.SetSiblingIndex(base.transform.childCount - 1);
				if (_lastUpdateSelectIndex != i || _lastUpdateSelectId != uISlotPotion.ID)
				{
					if (showInfoPanel)
					{
						ShowInfoPanel(uISlotPotion.ID);
					}
					_lastUpdateSelectId = uISlotPotion.ID;
					_lastUpdateSelectIndex = i;
				}
			}
			else
			{
				uiSlotPotions[i].SmallDirect();
			}
		}
		if (!flag)
		{
			_lastUpdateSelectId = -1;
			_lastUpdateSelectIndex = -1;
		}
	}

	public void ShowInfoPanel(int potionId, float showTime = 1.5f)
	{
		_hideInfoUITimer = showTime;
		if (_needToShow)
		{
			InfoUI.UpdateInfo(potionId);
			return;
		}
		InfoUI.gameObject.SetActive(value: true);
		_needToShow = true;
		InfoUI.UpdateInfo(potionId);
		_infoPanelSequence?.Complete();
		CanvasGroup component = InfoUI.GetComponent<CanvasGroup>();
		_infoPanelSequence = DOTween.Sequence(component).Append(component.DOFade(1f, 0.2f));
	}

	public void CloseInfoPanel()
	{
		if (_needToShow)
		{
			_needToShow = false;
			_infoPanelSequence?.Complete();
			CanvasGroup component = InfoUI.GetComponent<CanvasGroup>();
			_infoPanelSequence = DOTween.Sequence(component).Append(component.DOFade(0f, 0.2f)).AppendCallback(delegate
			{
				InfoUI.gameObject.SetActive(value: false);
			});
		}
	}
}
