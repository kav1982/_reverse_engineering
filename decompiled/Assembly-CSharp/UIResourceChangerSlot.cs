using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIResourceChangerSlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
	public UIResourceChangerSlotType slotType;

	public GameObject go_Outline;

	public Animator anima;

	public Image image_Goods;

	public Image image_Cost;

	public Sprite sprite_Core;

	public Sprite sprite_Blood;

	public Sprite sprite_Crystal;

	public Text text_GoodsCount;

	public Text text_CostCount;

	public float longPressTimeInterval;

	public float frequency;

	public float frequencySubPerTrigger;

	public float frequencymin;

	private float _longPressTime;

	private float _longPressTimeAfterLastTrigger;

	private bool _pointerDown;

	public int goodsCount => UIResourceChanger.ChangerDic[slotType].get;

	public int costCount => UIResourceChanger.ChangerDic[slotType].cost;

	private void OnEnable()
	{
		EventMgr.MagicCrystalChange = (Action)Delegate.Combine(EventMgr.MagicCrystalChange, new Action(UpdateMagicCrystal));
		EventMgr.AncienBloodChange = (Action)Delegate.Combine(EventMgr.AncienBloodChange, new Action(UpdateAncienBlood));
		EventMgr.ChaosCoreChange = (Action)Delegate.Combine(EventMgr.ChaosCoreChange, new Action(UpdateChaosCore));
	}

	private void OnDisable()
	{
		EventMgr.MagicCrystalChange = (Action)Delegate.Remove(EventMgr.MagicCrystalChange, new Action(UpdateMagicCrystal));
		EventMgr.AncienBloodChange = (Action)Delegate.Remove(EventMgr.AncienBloodChange, new Action(UpdateAncienBlood));
		EventMgr.ChaosCoreChange = (Action)Delegate.Remove(EventMgr.ChaosCoreChange, new Action(UpdateChaosCore));
	}

	private void UpdateMagicCrystal()
	{
		UIResourceChangerSlotType uIResourceChangerSlotType = slotType;
		if ((uint)uIResourceChangerSlotType <= 1u)
		{
			text_CostCount.color = ((costCount <= DataMgr.selectedWorldData.magicCrystalCount) ? Color.green : Color.red);
		}
	}

	private void UpdateAncienBlood()
	{
		UIResourceChangerSlotType uIResourceChangerSlotType = slotType;
		if ((uint)(uIResourceChangerSlotType - 2) <= 1u)
		{
			if (costCount <= DataMgr.selectedWorldData.ancientBloodCount)
			{
				text_CostCount.color = Color.green;
			}
			else
			{
				text_CostCount.color = Color.red;
			}
		}
	}

	private void UpdateChaosCore()
	{
		UIResourceChangerSlotType uIResourceChangerSlotType = slotType;
		if ((uint)(uIResourceChangerSlotType - 4) <= 1u)
		{
			if (costCount <= DataMgr.selectedWorldData.chaosCoreCount)
			{
				text_CostCount.color = Color.green;
			}
			else
			{
				text_CostCount.color = Color.red;
			}
		}
	}

	private void Start()
	{
		switch (slotType)
		{
		case UIResourceChangerSlotType.CrystalBuyCore:
			image_Goods.sprite = sprite_Core;
			image_Cost.sprite = sprite_Crystal;
			break;
		case UIResourceChangerSlotType.CrystalBuyBlood:
			image_Goods.sprite = sprite_Blood;
			image_Cost.sprite = sprite_Crystal;
			break;
		case UIResourceChangerSlotType.BloodBuyCore:
			image_Goods.sprite = sprite_Core;
			image_Cost.sprite = sprite_Blood;
			break;
		case UIResourceChangerSlotType.BloodBuyCrystal:
			image_Goods.sprite = sprite_Crystal;
			image_Cost.sprite = sprite_Blood;
			break;
		case UIResourceChangerSlotType.CoreBuyBlood:
			image_Goods.sprite = sprite_Blood;
			image_Cost.sprite = sprite_Core;
			break;
		case UIResourceChangerSlotType.CoreBuyCrystal:
			image_Goods.sprite = sprite_Crystal;
			image_Cost.sprite = sprite_Core;
			break;
		default:
			Debug.LogError(slotType);
			break;
		}
		text_GoodsCount.text = "×" + goodsCount;
		text_CostCount.text = costCount.ToString();
		UpdateResourses();
	}

	public void UpdateResourses()
	{
		UpdateMagicCrystal();
		UpdateAncienBlood();
		UpdateChaosCore();
	}

	private void Update()
	{
		if (!_pointerDown)
		{
			return;
		}
		_longPressTime += Time.deltaTime;
		_longPressTimeAfterLastTrigger += Time.deltaTime;
		if (_longPressTime > longPressTimeInterval && _longPressTimeAfterLastTrigger > frequency)
		{
			int num = 0;
			switch (slotType)
			{
			case UIResourceChangerSlotType.CrystalBuyCore:
			case UIResourceChangerSlotType.CrystalBuyBlood:
				num = DataMgr.selectedWorldData.magicCrystalCount;
				break;
			case UIResourceChangerSlotType.BloodBuyCore:
			case UIResourceChangerSlotType.BloodBuyCrystal:
				num = DataMgr.selectedWorldData.ancientBloodCount;
				break;
			case UIResourceChangerSlotType.CoreBuyBlood:
			case UIResourceChangerSlotType.CoreBuyCrystal:
				num = DataMgr.selectedWorldData.chaosCoreCount;
				break;
			}
			if (num >= costCount)
			{
				_longPressTimeAfterLastTrigger = 0f;
				ApplyChange(slotType);
				frequency -= frequencySubPerTrigger;
				frequency = Mathf.Clamp(frequency, frequencymin, frequency);
				SEMgr.Inst.uiSlotPut.PlaySE();
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (GameUISingletonMono<UIResourceChanger>.StaticIsOpen)
		{
			SEMgr.Inst.uiResearchHover.PlaySE();
			go_Outline.SetActive(value: true);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (GameUISingletonMono<UIResourceChanger>.StaticIsOpen)
		{
			go_Outline.SetActive(value: false);
		}
	}

	public void ReSetLongPress()
	{
		_pointerDown = false;
		_longPressTime = 0f;
	}

	public void ApplyChange(UIResourceChangerSlotType slotType, int num = 1)
	{
		int num2 = costCount * num;
		int value = goodsCount * num;
		switch (slotType)
		{
		case UIResourceChangerSlotType.CrystalBuyCore:
			if (DataMgr.selectedWorldData.magicCrystalCount >= num2)
			{
				base.transform.SetSiblingIndex(base.transform.parent.childCount - 1);
				anima.Play("Click", 0, 0f);
				PlayerMgr.Inst.ChangeMagicCrystal(-num2);
				PlayerMgr.Inst.ChangeChaosCore(value);
				SEMgr.Inst.itemPick_ChaosCore.PlaySE();
			}
			else
			{
				anima.Play("Shake", 0, 0f);
				SEMgr.Inst.uiResearchWrong.PlaySE();
			}
			break;
		case UIResourceChangerSlotType.CrystalBuyBlood:
			if (DataMgr.selectedWorldData.magicCrystalCount >= num2)
			{
				base.transform.SetSiblingIndex(base.transform.parent.childCount - 1);
				anima.Play("Click", 0, 0f);
				PlayerMgr.Inst.ChangeMagicCrystal(-num2);
				PlayerMgr.Inst.ChangeAncientBlood(value);
				SEMgr.Inst.itemPick_AncientBlood.PlaySE();
			}
			else
			{
				anima.Play("Shake", 0, 0f);
				SEMgr.Inst.uiResearchWrong.PlaySE();
			}
			break;
		case UIResourceChangerSlotType.BloodBuyCore:
			if (DataMgr.selectedWorldData.ancientBloodCount >= num2)
			{
				base.transform.SetSiblingIndex(base.transform.parent.childCount - 1);
				anima.Play("Click", 0, 0f);
				PlayerMgr.Inst.ChangeAncientBlood(-num2);
				PlayerMgr.Inst.ChangeChaosCore(value);
				SEMgr.Inst.itemPick_ChaosCore.PlaySE();
			}
			else
			{
				anima.Play("Shake", 0, 0f);
				SEMgr.Inst.uiResearchWrong.PlaySE();
			}
			break;
		case UIResourceChangerSlotType.BloodBuyCrystal:
			if (DataMgr.selectedWorldData.ancientBloodCount >= num2)
			{
				base.transform.SetSiblingIndex(base.transform.parent.childCount - 1);
				anima.Play("Click", 0, 0f);
				PlayerMgr.Inst.ChangeAncientBlood(-num2);
				PlayerMgr.Inst.ChangeMagicCrystal(value);
				SEMgr.Inst.itemPick_MagicCrystal.PlaySE();
			}
			else
			{
				anima.Play("Shake", 0, 0f);
				SEMgr.Inst.uiResearchWrong.PlaySE();
			}
			break;
		case UIResourceChangerSlotType.CoreBuyBlood:
			if (DataMgr.selectedWorldData.chaosCoreCount >= num2)
			{
				base.transform.SetSiblingIndex(base.transform.parent.childCount - 1);
				anima.Play("Click", 0, 0f);
				PlayerMgr.Inst.ChangeChaosCore(-num2);
				PlayerMgr.Inst.ChangeAncientBlood(value);
				SEMgr.Inst.itemPick_AncientBlood.PlaySE();
			}
			else
			{
				anima.Play("Shake", 0, 0f);
				SEMgr.Inst.uiResearchWrong.PlaySE();
			}
			break;
		case UIResourceChangerSlotType.CoreBuyCrystal:
			if (DataMgr.selectedWorldData.chaosCoreCount >= num2)
			{
				base.transform.SetSiblingIndex(base.transform.parent.childCount - 1);
				anima.Play("Click", 0, 0f);
				PlayerMgr.Inst.ChangeChaosCore(-num2);
				PlayerMgr.Inst.ChangeMagicCrystal(value);
				SEMgr.Inst.itemPick_MagicCrystal.PlaySE();
			}
			else
			{
				anima.Play("Shake", 0, 0f);
				SEMgr.Inst.uiResearchWrong.PlaySE();
			}
			break;
		default:
			Debug.LogError(slotType);
			break;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_pointerDown = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_pointerDown = false;
		ReSetLongPress();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		int num = 0;
		switch (slotType)
		{
		case UIResourceChangerSlotType.CrystalBuyCore:
		case UIResourceChangerSlotType.CrystalBuyBlood:
			num = DataMgr.selectedWorldData.magicCrystalCount;
			break;
		case UIResourceChangerSlotType.BloodBuyCore:
		case UIResourceChangerSlotType.BloodBuyCrystal:
			num = DataMgr.selectedWorldData.ancientBloodCount;
			break;
		case UIResourceChangerSlotType.CoreBuyBlood:
		case UIResourceChangerSlotType.CoreBuyCrystal:
			num = DataMgr.selectedWorldData.chaosCoreCount;
			break;
		}
		if (num >= costCount)
		{
			ApplyChange(slotType);
			SEMgr.Inst.uiSlotPut.PlaySE();
		}
	}
}
