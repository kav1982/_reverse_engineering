using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlotPotion : MonoBehaviour, IPointerExitHandler, IEventSystemHandler, IPointerEnterHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	public RectTransform rtsf_Self;

	public Image image_BG;

	public Image image_Icon;

	public Sprite sprite_Hover;

	public Sprite sprite_Unhover;

	public Animator anima;

	[Header("Shortcut")]
	public Image image_Shortcut;

	public UpdatButtonShow updatebuttonshow;

	public bool isFromBuild;

	public Vector3 offsetFromBuild;

	private bool hover;

	public int Index { get; private set; }

	public int ID
	{
		get
		{
			if (isFromBuild)
			{
				return Index;
			}
			return PlayerMgr.Inst.BaData.potionIDs[Index];
		}
	}

	private void ControlChange()
	{
		updatebuttonshow.UpdateButton();
	}

	private void OnEnable()
	{
		updatebuttonshow.UpdateButton();
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Combine(EventMgr.ControlChange, new Action(ControlChange));
	}

	private void OnDisable()
	{
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
		EventMgr.ControlChange = (Action)Delegate.Remove(EventMgr.ControlChange, new Action(ControlChange));
	}

	private void InputChange()
	{
		updatebuttonshow.UpdateButton();
	}

	private void Start()
	{
		InputChange();
	}

	private void Update()
	{
		if (!isFromBuild && hover && ID != 0)
		{
			UIPotionsController.Inst.ShowInfoPanel(ID, 0.1f);
		}
	}

	public void Initialize(int index, bool isFromBuild = false)
	{
		Index = index;
		this.isFromBuild = isFromBuild;
	}

	public void UpdateInfo()
	{
		if (ID == 0)
		{
			image_Icon.gameObject.SetActive(value: false);
		}
		else if (PotionConfig.dic.ContainsKey(ID))
		{
			image_Icon.gameObject.SetActive(value: true);
			image_Icon.sprite = ABResources.LoadAsset<Sprite>(PotionConfig.dic[ID].GetIconPath());
		}
		else
		{
			Debug.LogError("PotionConfig.dic.ContainsKey(ID) == false" + ID);
		}
		if (isFromBuild)
		{
			image_Shortcut.gameObject.SetActive(value: false);
		}
		else if (PlayerMgr.Inst.ItemCtrller.SelectedPotionIndex == Index)
		{
			image_Shortcut.gameObject.SetActive(value: true);
		}
		else
		{
			image_Shortcut.gameObject.SetActive(value: false);
		}
	}

	public void Hover()
	{
		hover = true;
		image_BG.sprite = sprite_Hover;
		image_Icon.transform.localScale = Vector3.one * 1.2f;
	}

	public void Unhover()
	{
		hover = false;
		image_BG.sprite = sprite_Unhover;
		image_Icon.transform.localScale = Vector3.one;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (GameMgr.IsMobile_Static)
		{
			return;
		}
		if (isFromBuild)
		{
			if (ID != 0)
			{
				UIPlayerDataMgr.Inst.uiInfoPotionHover.gameObject.SetActive(value: true);
				UIPlayerDataMgr.Inst.uiInfoPotionHover.transform.position = base.transform.position + offsetFromBuild;
				UIPlayerDataMgr.Inst.uiInfoPotionHover.UpdateInfo(ID, null, isFromBuild);
			}
		}
		else
		{
			Hover();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static)
		{
			if (isFromBuild)
			{
				UIPlayerDataMgr.Inst.uiInfoPotionHover.gameObject.SetActive(value: false);
			}
			else
			{
				Unhover();
			}
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!isFromBuild && !GameMgr.IsMobile_Static)
		{
			UIPlayerDataMgr.Inst.UISlotPotionDragBegin(this);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (!isFromBuild)
		{
			UIPlayerDataMgr.Inst.UISlotPotionDragEnd();
		}
	}

	public void OnCancelDrag()
	{
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!isFromBuild && !GameMgr.IsMobile_Static && ID != 0)
		{
			if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
			{
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002502.GetText(), UITextFloatType.Normal, PlayerMgr.Inst.PlayerPoint);
			}
			PlayerMgr.Inst.ItemCtrller.PotionSelect(Index, showInfoPanel: false);
		}
	}

	public void ToBig()
	{
		anima.SetTrigger("ToBig");
	}

	public void ToSmall()
	{
		anima.SetTrigger("ToSmall");
	}

	public void BigDirect()
	{
		anima.SetTrigger("BigDirect");
	}

	public void SmallDirect()
	{
		anima.SetTrigger("SmallDirect");
	}
}
