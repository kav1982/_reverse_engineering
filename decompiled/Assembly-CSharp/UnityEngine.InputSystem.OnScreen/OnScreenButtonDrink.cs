using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem.OnScreen;

public class OnScreenButtonDrink : OnScreenControl, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	public UnityEvent dragleft;

	public float dragThreashold = 0.5f;

	private float _x;

	private float _y;

	private Vector2 screenPosition;

	public UIPotionSelectPopOut uiPotionSelectPopOut;

	public UIInfoPotion UIInfoPotion;

	public float rangeStart;

	public float rangeEnd;

	private int currentShowID;

	private Tween _tween;

	[SerializeField]
	[InputControl(layout = "Button")]
	private string m_ControlPath;

	protected override string controlPathInternal
	{
		get
		{
			return m_ControlPath;
		}
		set
		{
			m_ControlPath = value;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		PlayerMgr.Inst.PlayerCtrller.DrinkCanceld();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		PlayerMgr.Inst.PlayerCtrller.DrinkPerformed();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		_x = eventData.position.x;
		_y = eventData.position.y;
		screenPosition = RectTransformUtility.WorldToScreenPoint(CamController.Inst.cam_UI, TopUI.inst.rectPotionCenter.transform.position);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		uiPotionSelectPopOut.Hide();
		_x = eventData.position.x;
		_y = eventData.position.y;
		currentShowID = 0;
		UIInfoPotion.gameObject.SetActive(value: false);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (uiPotionSelectPopOut.IsOpen)
		{
			Vector2 vector = eventData.position - screenPosition;
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			if (num < 0f)
			{
				num += 360f;
			}
			int potionMaxCount = PlayerMgr.Inst.BaData.potionMaxCount;
			float num2 = (rangeEnd - rangeStart) / (float)potionMaxCount;
			if (!(num < rangeEnd) || !(num > rangeStart))
			{
				return;
			}
			int num3 = Mathf.CeilToInt((num - rangeStart) / num2);
			int num4 = PlayerMgr.Inst.BaData.potionIDs[num3 - 1];
			if (num4 == 0)
			{
				return;
			}
			UIInfoPotion.gameObject.SetActive(value: true);
			if (currentShowID != num3)
			{
				currentShowID = num3;
				UIInfoPotion.UpdateInfo(num4);
				PlayerMgr.Inst.ItemCtrller.PotionSelect(num3 - 1);
				_tween.Kill();
				for (int i = 0; i < uiPotionSelectPopOut.layout.transform.childCount; i++)
				{
					uiPotionSelectPopOut.layout.transform.GetChild(i).transform.localScale = Vector3.one;
					uiPotionSelectPopOut.layout.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(value: false);
				}
				_tween = uiPotionSelectPopOut.layout.transform.GetChild(num3 - 1).DOScale(1.4f, 0.3f).SetUpdate(isIndependentUpdate: true);
				uiPotionSelectPopOut.layout.transform.GetChild(num3 - 1).transform.GetChild(0).gameObject.SetActive(value: true);
			}
		}
		else if (Vector2.Distance(eventData.position, new Vector2(_x, _y)) > dragThreashold / 100f * (float)MobileMgr.inst.scalerwidth && !uiPotionSelectPopOut.IsOpen && DataMgr.selectedWorldData.battleData9.potionIDs.Count >= 2)
		{
			PlayerMgr.Inst.PlayerCtrller.DrinkCanceld();
			_x = eventData.position.x;
			_y = eventData.position.y;
			dragleft?.Invoke();
			uiPotionSelectPopOut.Show();
			if (TopUI.inst.mobilePotionDragTutorial.activeInHierarchy)
			{
				TopUI.inst.mobilePotionDragTutorial.SetActive(value: false);
				DataMgr.selectedWorldData.mobilePotionDragTutorialShown = true;
			}
		}
	}

	public void DropSelectPotion()
	{
		if (PlayerMgr.Inst.BaData.potionIDs[currentShowID - 1] != 0)
		{
			UIPlayerDataMgr.Inst.DropPotion(currentShowID - 1, PlayerMgr.Inst.BaData.potionIDs[currentShowID - 1]);
			currentShowID = PlayerMgr.Inst.ItemCtrller.SelectedPotionIndex + 1;
			if (PlayerMgr.Inst.BaData.potionIDs[currentShowID - 1] != 0)
			{
				UIInfoPotion.UpdateInfo(PlayerMgr.Inst.BaData.potionIDs[currentShowID - 1]);
			}
		}
	}
}
