using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIAchievement : GameUI
{
	public Text achieveNum;

	public Text percent;

	public RectTransform progress;

	public CanvasGroup canvasGroup;

	public List<UIAchievementListItem> uIAchievementListItems = new List<UIAchievementListItem>();

	public GameObject mobileControllerFrame;

	public ScrollRect scrollRect;

	public RectTransform content;

	public new bool IsOpen => canvasGroup.alpha == 1f;

	public int mobileControllerIndex { get; set; }

	protected override void OnShow(object obj = null)
	{
		SEMgr.Inst.uiChangeLabel.PlaySE();
		canvasGroup.alpha = 1f;
		canvasGroup.blocksRaycasts = true;
		canvasGroup.interactable = true;
		UpdateInfo();
		UIMgr.TryAdditionalMobileShow(base.transform);
		if (MobileMgr.inst.gamepadPlugged)
		{
			mobileControllerIndex = 0;
			mobileControllerFrame.gameObject.SetActive(value: true);
			mobileControllerFrame.transform.position = uIAchievementListItems[mobileControllerIndex].transform.position;
		}
	}

	protected override void OnHide()
	{
		canvasGroup.alpha = 0f;
		canvasGroup.blocksRaycasts = false;
		canvasGroup.interactable = false;
		SEMgr.Inst.uiChangeLabelClose.PlaySE();
		UIMgr.TryAdditionalMobileHide(base.transform);
		mobileControllerFrame.gameObject.SetActive(value: false);
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += DirectPerformed;
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(UpdateInfo));
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(InputChange));
	}

	protected override void RegistarWhenInit()
	{
	}

	protected override void UnRegistarWhenDestroy()
	{
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= DirectPerformed;
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(UpdateInfo));
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(InputChange));
	}

	private void UpdateInfo()
	{
		achieveNum.text = $"您已解锁成就 {DataMgr.settingData.mobileAchievement.Count}/{uIAchievementListItems.Count}";
		float num = (float)DataMgr.settingData.mobileAchievement.Count / (float)uIAchievementListItems.Count * 100f;
		percent.text = $"({num:F1}%)";
		progress.sizeDelta = new Vector2((float)DataMgr.settingData.mobileAchievement.Count / (float)uIAchievementListItems.Count * progress.sizeDelta.x, progress.sizeDelta.y);
		foreach (UIAchievementListItem uIAchievementListItem in uIAchievementListItems)
		{
			uIAchievementListItem.UpdateInfo();
		}
	}

	private void InputChange()
	{
		switch (UIMgr.Inst.InputType)
		{
		case PlayerInputType.Keyboard:
			mobileControllerFrame.gameObject.SetActive(value: false);
			break;
		case PlayerInputType.Gamepad:
			mobileControllerFrame.gameObject.SetActive(value: true);
			mobileControllerFrame.transform.position = uIAchievementListItems[mobileControllerIndex].transform.position;
			break;
		default:
			Debug.LogError(UIMgr.Inst.InputType);
			break;
		}
	}

	private void DirectPerformed(InputAction.CallbackContext context)
	{
		if (IsOpen)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			MoveDirect(direct);
		}
	}

	private void MoveDirect(Vector2 direct)
	{
		if (direct == new Vector2(0f, 1f))
		{
			mobileControllerIndex--;
		}
		else if (direct == new Vector2(0f, -1f))
		{
			mobileControllerIndex++;
		}
		mobileControllerIndex = Mathf.Clamp(mobileControllerIndex, 0, uIAchievementListItems.Count - 1);
		mobileControllerFrame.transform.position = uIAchievementListItems[mobileControllerIndex].transform.position;
		GeneralTool.ScrollToPadSelected(scrollRect, content, uIAchievementListItems[mobileControllerIndex].GetComponent<RectTransform>());
	}
}
