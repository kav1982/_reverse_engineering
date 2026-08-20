using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class OnScreenChangeWand : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	public UnityEvent dragleft;

	public float dragThreashold = 0.5f;

	private float _x;

	private float _y;

	private Vector2 screenPosition;

	public UIWandSelectPopOut uiWandSelectPopOut;

	public UIInfoWand UIInfoWand;

	public float rangeStart;

	public float rangeEnd;

	private int currentShowID = -1;

	private Tween _tween;

	public void OnBeginDrag(PointerEventData eventData)
	{
		_x = eventData.position.x;
		_y = eventData.position.y;
		screenPosition = RectTransformUtility.WorldToScreenPoint(CamController.Inst.cam_UI, TopUI.inst.rectWandCenter.transform.position);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		uiWandSelectPopOut.Hide();
		_x = eventData.position.x;
		_y = eventData.position.y;
		currentShowID = -1;
		UIInfoWand.gameObject.SetActive(value: false);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (uiWandSelectPopOut.IsOpen)
		{
			Vector2 vector = eventData.position - screenPosition;
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			if (num < 0f)
			{
				num += 360f;
			}
			int wandMaxCount = PlayerMgr.Inst.BaData.wandMaxCount;
			float num2 = (rangeEnd - rangeStart) / (float)wandMaxCount;
			if (!(num < ((rangeEnd >= rangeStart) ? rangeEnd : rangeStart)) || !(num > ((rangeEnd >= rangeStart) ? rangeStart : rangeEnd)))
			{
				return;
			}
			int num3 = Mathf.CeilToInt((num - rangeStart) / num2) - 1;
			if (PlayerMgr.Inst.BaData.wandCfgs[num3] != null)
			{
				UIInfoWand.gameObject.SetActive(value: true);
				if (currentShowID != num3)
				{
					Debug.Log(num3);
					currentShowID = num3;
					UIInfoWand.UpdateInfo(PlayerMgr.Inst.BaData.wandCfgs[num3]);
					PlayerMgr.Inst.WandSelect(num3);
					DOVirtual.DelayedCall(0.2f, UIPlayerDataMgr.Inst.MobileUpdateWandFold);
					_tween.Kill();
					for (int i = 0; i < uiWandSelectPopOut.layout.transform.childCount; i++)
					{
						uiWandSelectPopOut.layout.transform.GetChild(i).transform.localScale = Vector3.one;
						uiWandSelectPopOut.layout.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(value: false);
					}
					_tween = uiWandSelectPopOut.layout.transform.GetChild(num3).DOScale(1.4f, 0.3f).SetUpdate(isIndependentUpdate: true);
					uiWandSelectPopOut.layout.transform.GetChild(num3).transform.GetChild(0).gameObject.SetActive(value: true);
				}
				return;
			}
			currentShowID = -1;
			UIInfoWand.gameObject.SetActive(value: false);
		}
		if (Vector2.Distance(eventData.position, new Vector2(_x, _y)) > dragThreashold / 100f * (float)MobileMgr.inst.scalerwidth && !uiWandSelectPopOut.IsOpen)
		{
			_x = eventData.position.x;
			_y = eventData.position.y;
			dragleft?.Invoke();
			uiWandSelectPopOut.Show();
			if (TopUI.inst.mobileWandDragTutorial.activeInHierarchy)
			{
				TopUI.inst.mobileWandDragTutorial.SetActive(value: false);
				DataMgr.selectedWorldData.mobileWandDragTutorialShown = true;
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		PlayerMgr.Inst.WandSelectOffset(1);
		DOVirtual.DelayedCall(0.2f, UIPlayerDataMgr.Inst.MobileUpdateWandFold);
	}
}
