using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualStickSizeAdjust : MonoBehaviour
{
	public enum AdjustType
	{
		Menu,
		LeftStick,
		RightStick,
		Skill,
		Postion,
		KillSummon,
		DamageRecordj,
		SwitchWand,
		IndieAttack,
		SkillCancle,
		DropPotion
	}

	public CanvasGroup canvasGroup;

	private Vector2 defaultLocalPosition;

	public Image selectColorChange;

	public Color colorSelected;

	private RectTransform rect;

	private Vector2 lastPos;

	public AdjustType adjustType;

	public Transform changeTransformTarget;

	public Transform changeSizeTarget;

	private Canvas canvas => TopUI.inst.canvas;

	public void SetDefaultLocalPosition()
	{
		if (canvasGroup == null)
		{
			canvasGroup = changeTransformTarget.GetComponent<CanvasGroup>();
		}
		rect = changeTransformTarget as RectTransform;
		defaultLocalPosition = rect.anchoredPosition;
		if (changeSizeTarget == null)
		{
			changeSizeTarget = changeTransformTarget;
		}
	}

	public void InitSizeAndPositiion()
	{
		MobileVirtualButtonData mobileVirtualButtonData = DataMgr.settingData.Mobiledata.virtualStickData2[(int)adjustType];
		changeSizeTarget.localScale = mobileVirtualButtonData.size * Vector3.one;
		canvasGroup.alpha = mobileVirtualButtonData.transparency;
		if (mobileVirtualButtonData.globalPositionx != 999f && mobileVirtualButtonData.globalPositiony != 999f)
		{
			rect.anchoredPosition = new Vector2(mobileVirtualButtonData.globalPositionx, mobileVirtualButtonData.globalPositiony);
		}
	}

	public void SaveToSetting()
	{
		if (base.gameObject.activeInHierarchy)
		{
			MobileVirtualButtonData mobileVirtualButtonData = DataMgr.settingData.Mobiledata.virtualStickData2[(int)adjustType];
			mobileVirtualButtonData.globalPositionx = rect.anchoredPosition.x;
			mobileVirtualButtonData.globalPositiony = rect.anchoredPosition.y;
			mobileVirtualButtonData.transparency = canvasGroup.alpha;
			mobileVirtualButtonData.size = changeSizeTarget.localScale.x;
		}
	}

	public void SetToDefault()
	{
		rect.anchoredPosition = defaultLocalPosition;
		canvasGroup.alpha = 1f;
		changeSizeTarget.localScale = Vector3.one;
	}

	public void OnDragPosition(BaseEventData eventData)
	{
		if (eventData is PointerEventData pointerEventData)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.parent as RectTransform, pointerEventData.position, (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : canvas.worldCamera, out var localPoint);
			rect.anchoredPosition += localPoint - lastPos;
			lastPos = localPoint;
		}
	}

	public void OnBeginDragPosition(BaseEventData eventData)
	{
		StartAdjusting();
		if (eventData is PointerEventData pointerEventData)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.parent as RectTransform, pointerEventData.position, (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : canvas.worldCamera, out lastPos);
		}
	}

	public void StartAdjusting()
	{
		TopUI.inst.currentVirtualStickSizeAdjust?.EndAdjusting();
		selectColorChange.color = colorSelected;
		TopUI.inst.currentVirtualStickSizeAdjust = this;
		UIMgr.Inst.uiSetting.slider_changeVirtualSize.value = changeSizeTarget.localScale.x;
		UIMgr.Inst.uiSetting.slider_changeVirtualTransparency.value = canvasGroup.alpha;
	}

	public void EndAdjusting()
	{
		selectColorChange.color = Color.clear;
	}

	public void SetSize(Vector3 value)
	{
		changeSizeTarget.transform.localScale = value;
	}
}
